using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MultipleDBSource.Extensions;
using MultipleDBSource.Models;
using System.Text.Json;

namespace MultipleDBSource.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {

    }

    public DbSet<Person> Persons { get; set; }

    // Define options once to be efficient
    private static readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNameCaseInsensitive = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase, // Or null if you want PascalCase
        WriteIndented = false
    };

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Person>()
            .Property(e => e.CreatedTimestamp)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Person>()
            .Property(e => e.UpdatedTimestamp)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .ValueGeneratedOnAddOrUpdate();

        modelBuilder.Entity<Person>(entity =>
        {
            entity.HasKey(e => e.Id);

            #region First way using JsonSerializer but SQL still considers it as NVARCHAR so SQL will cast explicitly

            // Configure the Details property with a Value Converter
            //entity.Property(p => p.Details)
            //    .HasConversion(
            //        // 1. How to convert MoreInfo -> JSON string (To Database)
            //        v => JsonSerializer.Serialize(v, _jsonOptions),

            //        // 2. How to convert JSON string -> MoreInfo (From Database)
            //        v => JsonSerializer.Deserialize<MoreInfo>(v, _jsonOptions)
            //    )
            //    // 3. Force the SQL Server 2025 native 'json' type
            //    .HasColumnType("json");

            //entity.Property(p => p.History)
            //    .HasConversion(
            //        // 1. How to convert AuditTrail -> JSON string (To Database)
            //        v => JsonSerializer.Serialize(v, _jsonOptions),

            //        // 2. How to convert JSON string -> AuditTrail (From Database)
            //        v => JsonSerializer.Deserialize<AuditTrail>(v, _jsonOptions)
            //    )
            //    // 3. Force the SQL Server 2025 native 'json' type
            //    .HasColumnType("json");

            #endregion

            #region Another alternative way

            // map Config Json, also map inner properties
            //entity.OwnsOne(c => c.Details, navBuilder =>
            //{
            //    navBuilder.ToJson();

            //    navBuilder.Property(d => d.FirstName).HasJsonPropertyName("firstName");
            //    navBuilder.Property(d => d.LastName).HasJsonPropertyName("lastName");
            //    navBuilder.Property(d => d.FullName).HasJsonPropertyName("fullName");
            //    navBuilder.Property(d => d.Email).HasJsonPropertyName("email");
            //    navBuilder.Property(d => d.Address).HasJsonPropertyName("address");

            //    // NESTED OBJECT: preferences inside the Details JSON
            //    navBuilder.OwnsOne(d => d.UserPrefs, prefBuilder =>
            //    {
            //        prefBuilder.Property(p => p.Theme).HasJsonPropertyName("theme");
            //        prefBuilder.Property(p => p.Notifications).HasJsonPropertyName("notifications");
            //    });

            //    // OwnsMany for Lists/Arrays
            //    navBuilder.OwnsMany(d => d.Contacts, contactBuilder =>
            //    {
            //        contactBuilder.ToJson();

            //        contactBuilder.Property(c => c.Name).HasJsonPropertyName("name");
            //        contactBuilder.Property(c => c.Phone).HasJsonPropertyName("phone");
            //        contactBuilder.Property(c => c.Relationship).HasJsonPropertyName("relationship");
            //    });
            //});

            //entity.OwnsOne(p => p.History, histBuilder =>
            //{
            //    histBuilder.ToJson(); // This creates a SEPARATE JSON column named 'History'

            //    histBuilder.Property(p => p.CreatedBy).HasJsonPropertyName("createdBy");
            //    histBuilder.Property(p => p.ModifiedAt).HasJsonPropertyName("modifiedAt");
            //});

            //// Giving error : (This avoids the "Property already exists" or "Cannot map MoreInfo" error)
            //// Use the shadow property to set the SQL Server 2025 native type
            //// This targets the actual column in the DB named 'Details'
            ////entity.Property(p => p.Details)
            ////      .HasColumnType("json");

            //// Force SQL Server 2025 Native JSON types
            //entity.Metadata.FindNavigation(nameof(Person.Details))?.TargetEntityType.SetContainerColumnType("json");
            //entity.Metadata.FindNavigation(nameof(Person.History))?.TargetEntityType.SetContainerColumnType("json");

            #endregion Another alternative way

            #region Another way using extension

            // 1. Configure the 'Details' Column
            entity.OwnsOne(p => p.Details, nav =>
            {
                // Call ROOT here because 'Details' is the column
                nav.ToNativeJsonRoot();

                // Call NESTED here because these are INSIDE the 'Details' column
                nav.OwnsOne(d => d.UserPrefs, pref => pref.ToNativeJsonNested());
                nav.OwnsMany(d => d.Contacts, cont => cont.ToNativeJsonNested());
            });

            // 2. Configure the 'History' Column
            entity.OwnsOne(p => p.History, nav => nav.ToNativeJsonRoot());

            // 3. THE FIX: Apply the native type ONLY to the top-level navigations
            // These are the only "Containers"
            ConfigureNativeJson(entity, nameof(Person.Details));
            ConfigureNativeJson(entity, nameof(Person.History));

            #endregion Another way using extension
        });
    }

    // Helper method to apply the Native SQL 2025 type safely
    private static void ConfigureNativeJson<T>(EntityTypeBuilder<T> builder, string propertyName) where T : class
    {
        IMutableNavigation? navigation = builder.Metadata.FindNavigation(propertyName);

        if (navigation is not null)
        {
            // This tells SQL Server: "The column holding this object is binary JSON"
            navigation.TargetEntityType.SetContainerColumnType("json");
        }
    }
}
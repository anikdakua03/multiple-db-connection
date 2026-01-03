using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System.Reflection;
using System.Text.Json;

namespace MultipleDBSource.Extensions;

/// <summary>
/// Provides helper extension methods to configure owned navigations to be
/// stored in native JSON columns when using EF Core. Methods map simple
/// CLR properties to camel-cased JSON property names and preserve existing
/// usage patterns for root and nested JSON structures.
/// </summary>
public static class EFCoreJsonExtensions
{
    extension<THeader, TData>(OwnedNavigationBuilder<THeader, TData> builder)
        where THeader : class where TData : class
    {
        /// <summary>
        /// Configure an owned navigation as the root JSON column on the parent entity.
        /// Use this only for the property directly on the root entity which will be
        /// stored as a JSON column. This method defines the column as JSON and then
        /// maps the owned type's simple properties to JSON property names.
        /// </summary>
        public OwnedNavigationBuilder<THeader, TData> ToNativeJsonRoot()
        {
            builder.ToJson(); // This defines the column

            return builder.MapJsonProperties();
        }

        /// <summary>
        /// Configure an owned navigation that represents a nested object or collection
        /// inside an existing JSON column. This maps simple properties but does not
        /// define a new JSON column on the parent (it assumes a containing JSON
        /// structure already exists).
        /// </summary>
        public OwnedNavigationBuilder<THeader, TData> ToNativeJsonNested()
            => builder.MapJsonProperties();

        /// <summary>
        /// Shared implementation that maps simple, writable public properties on the
        /// owned type to JSON property names using camelCase. Properties without a
        /// setter are ignored. Complex or nested types are left to be handled by
        /// additional mapping calls (e.g. nested owned navigation).
        /// </summary>
        private OwnedNavigationBuilder<THeader, TData> MapJsonProperties()
        {
            PropertyInfo[] properties = typeof(TData).GetProperties(BindingFlags.Public | BindingFlags.Instance);

            foreach (PropertyInfo property in properties)
            {
                if (property.GetSetMethod() is null)
                {
                    builder.Ignore(property.Name);
                    continue;
                }

                if (IsSimpleType(property.PropertyType))
                {
                    string? camelCaseName = JsonNamingPolicy.CamelCase.ConvertName(property.Name);

                    builder.Property(property.Name).HasJsonPropertyName(camelCaseName);
                }
            }

            return builder;
        }
    }

    /// <summary>
    /// Determines whether the provided <see cref="Type"/> is a "simple" type
    /// that can be represented directly as a JSON primitive value. Nullable
    /// wrappers are unwrapped before the check. Includes primitives, enums,
    /// string, decimal, Guid, DateTime and DateTimeOffset.
    /// </summary>
    private static bool IsSimpleType(Type type)
    {
        Type? actualType = Nullable.GetUnderlyingType(type) ?? type;

        return actualType.IsPrimitive || actualType.IsEnum || actualType == typeof(string)
               || actualType == typeof(decimal) || actualType == typeof(Guid)
               || actualType == typeof(DateTime) || actualType == typeof(DateTimeOffset);
    }
}
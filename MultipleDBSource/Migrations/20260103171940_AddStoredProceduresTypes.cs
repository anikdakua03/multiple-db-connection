using Microsoft.EntityFrameworkCore.Migrations;
using MultipleDBSource.Extensions;

#nullable disable

namespace MultipleDBSource.Migrations
{
    /// <inheritdoc />
    public partial class AddStoredProceduresTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This automatically picks up EVERY file in these folders
            // and applies them to the DB when you run 'migrate'
            migrationBuilder.ApplySqlScripts("Types");
            migrationBuilder.ApplySqlScripts("StoredProcedures");
            //migrationBuilder.ApplySqlScripts("Indexes"); // NOTE : Creating index on exisitng db may be costly since have data already 
            migrationBuilder.ApplySqlScripts("Functions");

            migrationBuilder.Sql("PRINT 'Running Functions scripts';");

        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

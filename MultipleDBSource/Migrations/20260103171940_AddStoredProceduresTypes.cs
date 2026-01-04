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
            // NOTE : Creating index on exisitng db may be costly since have data already 
            // Also can be added in entity configuration
            //migrationBuilder.ApplySqlScripts("Indexes"); 
            migrationBuilder.ApplySqlScripts("Functions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

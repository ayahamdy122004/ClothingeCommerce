using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace E_Commerce.Migrations
{
    /// <inheritdoc />
    public partial class seedroles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
           table: "AspNetRoles",
             columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
             values: new object[,]
             {
                    {Guid.NewGuid().ToString(),Guid.NewGuid().ToString(), "Customer", "Customer".ToUpper() },
                    {Guid.NewGuid().ToString(), Guid.NewGuid().ToString(), "Administrator", "Administrator".ToUpper() }
             });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
          migrationBuilder.DeleteData(
            table: "AspNetRoles",
            keyColumn: "Name",
            keyValues: new object[] { "Customer", "Administrator" });
        }
    }
}

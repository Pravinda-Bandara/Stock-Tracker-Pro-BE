using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class CommentOneToOne : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "4e0cd34a-599a-4ce7-a74d-d435c07f34d5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e21435aa-8220-4d07-a4ab-fd718c3783f4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "15392987-efa3-4474-837f-ada89b77c929", null, "Admin", "ADMIN" },
                    { "95d70dc5-6559-4767-88ca-f9ee0157a2e4", null, "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "15392987-efa3-4474-837f-ada89b77c929");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "95d70dc5-6559-4767-88ca-f9ee0157a2e4");

            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "4e0cd34a-599a-4ce7-a74d-d435c07f34d5", null, "User", "USER" },
                    { "e21435aa-8220-4d07-a4ab-fd718c3783f4", null, "Admin", "ADMIN" }
                });
        }
    }
}

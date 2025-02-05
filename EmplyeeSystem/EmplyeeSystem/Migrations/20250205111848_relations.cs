using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmplyeeSystem.Migrations
{
    /// <inheritdoc />
    public partial class relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDepartments",
                table: "UserDepartments");

            migrationBuilder.AddColumn<int>(
                name: "id",
                table: "UserDepartments",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDepartments",
                table: "UserDepartments",
                column: "id");

            migrationBuilder.CreateIndex(
                name: "IX_UserDepartments_UserId",
                table: "UserDepartments",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_UserDepartments",
                table: "UserDepartments");

            migrationBuilder.DropIndex(
                name: "IX_UserDepartments_UserId",
                table: "UserDepartments");

            migrationBuilder.DropColumn(
                name: "id",
                table: "UserDepartments");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserDepartments",
                table: "UserDepartments",
                columns: new[] { "UserId", "DepartmentId" });
        }
    }
}

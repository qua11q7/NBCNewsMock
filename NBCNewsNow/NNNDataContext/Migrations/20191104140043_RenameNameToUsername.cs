using Microsoft.EntityFrameworkCore.Migrations;

namespace NNNDataContext.Migrations
{
    public partial class RenameNameToUsername : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "User",
                table: "Traces",
                newName: "Username");

            migrationBuilder.RenameColumn(
                name: "User",
                table: "Logs",
                newName: "Username");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Traces",
                newName: "User");

            migrationBuilder.RenameColumn(
                name: "Username",
                table: "Logs",
                newName: "User");
        }
    }
}

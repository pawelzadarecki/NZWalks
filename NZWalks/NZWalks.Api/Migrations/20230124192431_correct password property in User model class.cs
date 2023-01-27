using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NZWalks.Api.Migrations
{
    /// <inheritdoc />
    public partial class correctpasswordpropertyinUsermodelclass : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Passward",
                table: "Users",
                newName: "Password");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Password",
                table: "Users",
                newName: "Passward");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AllupMVCProject.Migrations
{
    /// <inheritdoc />
    public partial class addedProductImageIdsAndRedirectUrlColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RedirectUrl",
                table: "Sliders",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ProductImageIds",
                table: "Products",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectUrl",
                table: "Sliders");

            migrationBuilder.DropColumn(
                name: "ProductImageIds",
                table: "Products");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Edit_Hiring_Entity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "JobApplicationId",
                table: "HiringHistories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "IX_HiringHistories_JobApplicationId",
                table: "HiringHistories",
                column: "JobApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_HiringHistories_JobApplications_JobApplicationId",
                table: "HiringHistories",
                column: "JobApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HiringHistories_JobApplications_JobApplicationId",
                table: "HiringHistories");

            migrationBuilder.DropIndex(
                name: "IX_HiringHistories_JobApplicationId",
                table: "HiringHistories");

            migrationBuilder.DropColumn(
                name: "JobApplicationId",
                table: "HiringHistories");
        }
    }
}

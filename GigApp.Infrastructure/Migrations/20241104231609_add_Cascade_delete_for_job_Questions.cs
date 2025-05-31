using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Cascade_delete_for_job_Questions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobQuestionnaires_Job",
                table: "JobQuestionnaires");

            migrationBuilder.AddForeignKey(
                name: "FK_JobQuestionnaires_Job",
                table: "JobQuestionnaires",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobQuestionnaires_Job",
                table: "JobQuestionnaires");

            migrationBuilder.AddForeignKey(
                name: "FK_JobQuestionnaires_Job",
                table: "JobQuestionnaires",
                column: "JobId",
                principalTable: "Jobs",
                principalColumn: "Id");
        }
    }
}

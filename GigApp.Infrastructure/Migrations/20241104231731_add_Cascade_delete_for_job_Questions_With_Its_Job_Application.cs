using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class add_Cascade_delete_for_job_Questions_With_Its_Job_Application : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__JobQuesti__JobAp__793DFFAF",
                table: "JobQuestionnaireAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK__JobQuesti__JobAp__793DFFAF",
                table: "JobQuestionnaireAnswers",
                column: "JobApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__JobQuesti__JobAp__793DFFAF",
                table: "JobQuestionnaireAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK__JobQuesti__JobAp__793DFFAF",
                table: "JobQuestionnaireAnswers",
                column: "JobApplicationId",
                principalTable: "JobApplications",
                principalColumn: "Id");
        }
    }
}

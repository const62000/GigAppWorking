using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Resolve_JobQuestion_Answer_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__JobQuesti__Quest__7A3223E8",
                table: "JobQuestionnaireAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK__JobQuesti__Quest__7A3223E8",
                table: "JobQuestionnaireAnswers",
                column: "QuestionId",
                principalTable: "JobQuestionnaires",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK__JobQuesti__Quest__7A3223E8",
                table: "JobQuestionnaireAnswers");

            migrationBuilder.AddForeignKey(
                name: "FK__JobQuesti__Quest__7A3223E8",
                table: "JobQuestionnaireAnswers",
                column: "QuestionId",
                principalTable: "JobQuestionnaires",
                principalColumn: "Id");
        }
    }
}

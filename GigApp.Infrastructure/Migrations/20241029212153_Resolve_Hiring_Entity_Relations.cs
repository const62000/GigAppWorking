using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GigApp.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Resolve_Hiring_Entity_Relations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HiringHistories_Companies_CompanyId",
                table: "HiringHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_HiringHistories_Contracts_ContractId",
                table: "HiringHistories");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "HiringHistories",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                table: "HiringHistories",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_HiringHistories_Companies_CompanyId",
                table: "HiringHistories",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_HiringHistories_Contracts_ContractId",
                table: "HiringHistories",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_HiringHistories_Companies_CompanyId",
                table: "HiringHistories");

            migrationBuilder.DropForeignKey(
                name: "FK_HiringHistories_Contracts_ContractId",
                table: "HiringHistories");

            migrationBuilder.AlterColumn<int>(
                name: "ContractId",
                table: "HiringHistories",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CompanyId",
                table: "HiringHistories",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_HiringHistories_Companies_CompanyId",
                table: "HiringHistories",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_HiringHistories_Contracts_ContractId",
                table: "HiringHistories",
                column: "ContractId",
                principalTable: "Contracts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

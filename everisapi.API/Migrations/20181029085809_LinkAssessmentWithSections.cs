using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class LinkAssessmentWithSections : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssessmentId",
                table: "Sections",
                nullable: false,
                defaultValue: 1);

            // migrationBuilder.DropTable(
            //     name: "Assessment");

            migrationBuilder.CreateTable(
                name: "Assessment",
                columns: table => new
                {
                    AssessmentId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AssessmentName = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Assessment", x => x.AssessmentId);
                });

            string[] columnas = new string[] { "AssessmentId", "AssessmentName" };
            string[] valores = new string[] { "1", "SCRUM" };
            migrationBuilder.InsertData(
                table: "assessment",
                columns: columnas,
                values: valores);

            string[] columnas2 = new string[] { "AssessmentId", "AssessmentName" };
            string[] valores2 = new string[] { "2", "KANBAN" };
            migrationBuilder.InsertData(
                table: "assessment",
                columns: columnas2,
                values: valores2);

            migrationBuilder.CreateIndex(
                name: "IX_Sections_AssessmentId",
                table: "Sections",
                column: "AssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Sections_Assessment_AssessmentId",
                table: "Sections",
                column: "AssessmentId",
                principalTable: "Assessment",
                principalColumn: "AssessmentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sections_Assessment_AssessmentId",
                table: "Sections");

            migrationBuilder.DropTable(
                name: "Assessment");

            migrationBuilder.DropIndex(
                name: "IX_Sections_AssessmentId",
                table: "Sections");

            migrationBuilder.DropColumn(
                name: "AssessmentId",
                table: "Sections");
        }
    }
}

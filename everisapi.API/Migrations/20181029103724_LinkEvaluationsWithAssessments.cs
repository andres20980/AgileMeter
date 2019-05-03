using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class LinkEvaluationsWithAssessments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AssessmentId",
                table: "Evaluaciones",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluaciones_AssessmentId",
                table: "Evaluaciones",
                column: "AssessmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluaciones_Assessment_AssessmentId",
                table: "Evaluaciones",
                column: "AssessmentId",
                principalTable: "Assessment",
                principalColumn: "AssessmentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluaciones_Assessment_AssessmentId",
                table: "Evaluaciones");

            migrationBuilder.DropIndex(
                name: "IX_Evaluaciones_AssessmentId",
                table: "Evaluaciones");

            migrationBuilder.DropColumn(
                name: "AssessmentId",
                table: "Evaluaciones");
        }
    }
}

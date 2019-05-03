using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class AddAssessments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // migrationBuilder.CreateTable(
            // name: "everisapi.API.Entities.AssessmentEntity",
            // columns: table => new
            // {
            //     AssessmentId = table.Column<int>(nullable: false),
            //     AssessmentName = table.Column<string>(nullable: false)
            // }
            // );

            // migrationBuilder.RenameTable(
            //     name: "everisapi.API.Entities.AssessmentEntity",
            //     newName: "assessment");

            // string[] columnas = new string[] { "AssessmentId", "AssessmentName" };
            // string[] valores = new string[] { "1", "SCRUM" };
            // migrationBuilder.InsertData(
            //     table: "assessment",
            //     columns: columnas,
            //     values: valores);
        }



        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}

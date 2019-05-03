using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class AddUserNameToEvaluation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserNombre",
                table: "Evaluaciones",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Evaluaciones_UserNombre",
                table: "Evaluaciones",
                column: "UserNombre");

            migrationBuilder.AddForeignKey(
                name: "FK_Evaluaciones_Users_UserNombre",
                table: "Evaluaciones",
                column: "UserNombre",
                principalTable: "Users",
                principalColumn: "Nombre",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Evaluaciones_Users_UserNombre",
                table: "Evaluaciones");

            migrationBuilder.DropIndex(
                name: "IX_Evaluaciones_UserNombre",
                table: "Evaluaciones");

            migrationBuilder.DropColumn(
                name: "UserNombre",
                table: "Evaluaciones");
        }
    }
}

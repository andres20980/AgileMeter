using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class AddUserProyecto : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropForeignKey(
                name: "FK_User_Proyecto_Proyectos_ProyectoId",
                table: "userproyectos");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Proyecto_Users_UserNombre",
                table: "userproyectos");

            migrationBuilder.DropTable(
                name : "userproyectos"
            );

            migrationBuilder.CreateTable(
                name: "everisapi.API.Entities.UserProyectoEntity",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false), 
                    UserNombre = table.Column<string>(nullable: false),
                    ProyectoId = table.Column<int>(nullable: false)
                }
            );

            migrationBuilder.RenameTable(
                name: "everisapi.API.Entities.UserProyectoEntity",
                newName: "userproyectos");

            migrationBuilder.AlterColumn<string>(
                name: "UserNombre",
                table: "userproyectos",
                type: "varchar(127)",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<Int32>(
                name: "ProyectoId",
                table: "userproyectos",
                nullable: false,
                oldClrType: typeof(Int32),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "userproyectos",
                nullable: false,
                defaultValue: 0)
                .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserProyecto",
                table: "userproyectos",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserProyecto_ProyectoId",
                table: "userproyectos",
                column: "ProyectoId");

            migrationBuilder.CreateIndex(
                name: "IX_UserProyecto_UserNombre",
                table: "userproyectos",
                column: "UserNombre");

            migrationBuilder.AddForeignKey(
                name: "FK_UserProyecto_Proyectos_ProyectoId",
                table: "userproyectos",
                column: "ProyectoId",
                principalTable: "Proyectos",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_UserProyecto_Users_UserNombre",
                table: "userproyectos",
                column: "UserNombre",
                principalTable: "Users",
                principalColumn: "Nombre",
                onDelete: ReferentialAction.Cascade);

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            
            migrationBuilder.DropForeignKey(
                name: "FK_User_Proyecto_Proyectos_ProyectoId",
                table: "userproyectos");

            migrationBuilder.DropForeignKey(
                name: "FK_User_Proyecto_Users_UserNombre",
                table: "userproyectos");

            migrationBuilder.DropTable(
                name : "userproyectos"
            );
        }
    }
}

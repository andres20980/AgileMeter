using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class AddTableOficinaUnidad_Linea : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LineaId",
                table: "Proyectos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "OficinaId",
                table: "Proyectos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ProjectSize",
                table: "Proyectos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UnidadId",
                table: "Proyectos",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Oficina",
                columns: table => new
                {
                    OficinaId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OficinaNombre = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Oficina", x => x.OficinaId);
                });

            migrationBuilder.CreateTable(
                name: "Unidad",
                columns: table => new
                {
                    UnidadId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    OficinaId = table.Column<int>(nullable: false),
                    UnidadNombre = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Unidad", x => x.UnidadId);
                    table.ForeignKey(
                        name: "FK_Unidad_Oficina_OficinaId",
                        column: x => x.OficinaId,
                        principalTable: "Oficina",
                        principalColumn: "OficinaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Linea",
                columns: table => new
                {
                    LineaId = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    LineaNombre = table.Column<string>(maxLength: 50, nullable: false),
                    UnidadId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Linea", x => x.LineaId);
                    table.ForeignKey(
                        name: "FK_Linea_Unidad_UnidadId",
                        column: x => x.UnidadId,
                        principalTable: "Unidad",
                        principalColumn: "UnidadId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_LineaId",
                table: "Proyectos",
                column: "LineaId");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_OficinaId",
                table: "Proyectos",
                column: "OficinaId");

            migrationBuilder.CreateIndex(
                name: "IX_Proyectos_UnidadId",
                table: "Proyectos",
                column: "UnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Linea_UnidadId",
                table: "Linea",
                column: "UnidadId");

            migrationBuilder.CreateIndex(
                name: "IX_Unidad_OficinaId",
                table: "Unidad",
                column: "OficinaId");

            migrationBuilder.AddForeignKey(
                name: "FK_Proyectos_Linea_LineaId",
                table: "Proyectos",
                column: "LineaId",
                principalTable: "Linea",
                principalColumn: "LineaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proyectos_Oficina_OficinaId",
                table: "Proyectos",
                column: "OficinaId",
                principalTable: "Oficina",
                principalColumn: "OficinaId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Proyectos_Unidad_UnidadId",
                table: "Proyectos",
                column: "UnidadId",
                principalTable: "Unidad",
                principalColumn: "UnidadId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Proyectos_Linea_LineaId",
                table: "Proyectos");

            migrationBuilder.DropForeignKey(
                name: "FK_Proyectos_Oficina_OficinaId",
                table: "Proyectos");

            migrationBuilder.DropForeignKey(
                name: "FK_Proyectos_Unidad_UnidadId",
                table: "Proyectos");

            migrationBuilder.DropTable(
                name: "Linea");

            migrationBuilder.DropTable(
                name: "Unidad");

            migrationBuilder.DropTable(
                name: "Oficina");

            migrationBuilder.DropIndex(
                name: "IX_Proyectos_LineaId",
                table: "Proyectos");

            migrationBuilder.DropIndex(
                name: "IX_Proyectos_OficinaId",
                table: "Proyectos");

            migrationBuilder.DropIndex(
                name: "IX_Proyectos_UnidadId",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "LineaId",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "OficinaId",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "ProjectSize",
                table: "Proyectos");

            migrationBuilder.DropColumn(
                name: "UnidadId",
                table: "Proyectos");
        }
    }
}

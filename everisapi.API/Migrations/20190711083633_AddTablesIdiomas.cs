using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace everisapi.API.Migrations
{
    public partial class AddTablesIdiomas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Idiomas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Idioma = table.Column<string>(maxLength: 50, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Idiomas", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "TraduccionesAsignaciones",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    AsignacionesId = table.Column<int>(nullable: false),
                    IdiomaId = table.Column<int>(nullable: false),
                    Traduccion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraduccionesAsignaciones", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TraduccionesAsignaciones_Asignaciones_AsignacionesId",
                        column: x => x.AsignacionesId,
                        principalTable: "Asignaciones",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TraduccionesAsignaciones_Idiomas_IdiomaId",
                        column: x => x.IdiomaId,
                        principalTable: "Idiomas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TraduccionesIdiomas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdiomaId = table.Column<int>(nullable: false),
                    Traduccion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraduccionesIdiomas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TraduccionesIdiomas_Idiomas_IdiomaId",
                        column: x => x.IdiomaId,
                        principalTable: "Idiomas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TraduccionesOficinas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdiomaId = table.Column<int>(nullable: false),
                    OficinaId = table.Column<int>(nullable: false),
                    Traduccion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraduccionesOficinas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TraduccionesOficinas_Idiomas_IdiomaId",
                        column: x => x.IdiomaId,
                        principalTable: "Idiomas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TraduccionesOficinas_Oficina_OficinaId",
                        column: x => x.OficinaId,
                        principalTable: "Oficina",
                        principalColumn: "OficinaId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TraduccionesPreguntas",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdiomaId = table.Column<int>(nullable: false),
                    PreguntaId = table.Column<int>(nullable: false),
                    Traduccion = table.Column<string>(maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraduccionesPreguntas", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TraduccionesPreguntas_Idiomas_IdiomaId",
                        column: x => x.IdiomaId,
                        principalTable: "Idiomas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TraduccionesPreguntas_Preguntas_PreguntaId",
                        column: x => x.PreguntaId,
                        principalTable: "Preguntas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TraduccionesRoles",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdiomaId = table.Column<int>(nullable: false),
                    RoleId = table.Column<int>(nullable: false),
                    Traduccion = table.Column<string>(maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraduccionesRoles", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TraduccionesRoles_Idiomas_IdiomaId",
                        column: x => x.IdiomaId,
                        principalTable: "Idiomas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TraduccionesRoles_Roles_RoleId",
                        column: x => x.RoleId,
                        principalTable: "Roles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TraduccionesSections",
                columns: table => new
                {
                    ID = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    IdiomaId = table.Column<int>(nullable: false),
                    SectionsId = table.Column<int>(nullable: false),
                    Traduccion = table.Column<string>(maxLength: 120, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TraduccionesSections", x => x.ID);
                    table.ForeignKey(
                        name: "FK_TraduccionesSections_Idiomas_IdiomaId",
                        column: x => x.IdiomaId,
                        principalTable: "Idiomas",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TraduccionesSections_Sections_SectionsId",
                        column: x => x.SectionsId,
                        principalTable: "Sections",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TraduccionesAsignaciones_AsignacionesId",
                table: "TraduccionesAsignaciones",
                column: "AsignacionesId");

            migrationBuilder.CreateIndex(
                name: "IX_TraduccionesAsignaciones_IdiomaId",
                table: "TraduccionesAsignaciones",
                column: "IdiomaId");

            migrationBuilder.CreateIndex(
                name: "IX_TraduccionesIdiomas_IdiomaId",
                table: "TraduccionesIdiomas",
                column: "IdiomaId");

            migrationBuilder.CreateIndex(
                name: "IX_TraduccionesOficinas_IdiomaId",
                table: "TraduccionesOficinas",
                column: "IdiomaId");

            migrationBuilder.CreateIndex(
                name: "IX_TraduccionesOficinas_OficinaId",
                table: "TraduccionesOficinas",
                column: "OficinaId");

            migrationBuilder.CreateIndex(
                name: "IX_TraduccionesPreguntas_IdiomaId",
                table: "TraduccionesPreguntas",
                column: "IdiomaId");

            migrationBuilder.CreateIndex(
                name: "IX_TraduccionesPreguntas_PreguntaId",
                table: "TraduccionesPreguntas",
                column: "PreguntaId");

            migrationBuilder.CreateIndex(
                name: "IX_TraduccionesRoles_IdiomaId",
                table: "TraduccionesRoles",
                column: "IdiomaId");

            migrationBuilder.CreateIndex(
                name: "IX_TraduccionesRoles_RoleId",
                table: "TraduccionesRoles",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_TraduccionesSections_IdiomaId",
                table: "TraduccionesSections",
                column: "IdiomaId");

            migrationBuilder.CreateIndex(
                name: "IX_TraduccionesSections_SectionsId",
                table: "TraduccionesSections",
                column: "SectionsId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TraduccionesAsignaciones");

            migrationBuilder.DropTable(
                name: "TraduccionesIdiomas");

            migrationBuilder.DropTable(
                name: "TraduccionesOficinas");

            migrationBuilder.DropTable(
                name: "TraduccionesPreguntas");

            migrationBuilder.DropTable(
                name: "TraduccionesRoles");

            migrationBuilder.DropTable(
                name: "TraduccionesSections");

            migrationBuilder.DropTable(
                name: "Idiomas");
        }
    }
}

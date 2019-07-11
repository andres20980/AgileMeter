﻿// <auto-generated />
using everisapi.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using System;

namespace everisapi.API.Migrations
{
    [DbContext(typeof(AsignacionInfoContext))]
    partial class AsignacionInfoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn)
                .HasAnnotation("ProductVersion", "2.0.2-rtm-10011");

            modelBuilder.Entity("everisapi.API.Entities.AsignacionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("Peso")
                        .HasMaxLength(50);

                    b.Property<int>("SectionId");

                    b.HasKey("Id");

                    b.HasIndex("SectionId");

                    b.ToTable("Asignaciones");
                });

            modelBuilder.Entity("everisapi.API.Entities.AssessmentEntity", b =>
                {
                    b.Property<int>("AssessmentId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("AssessmentName")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("AssessmentId");

                    b.ToTable("Assessment");
                });

            modelBuilder.Entity("everisapi.API.Entities.EvaluacionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssessmentId");

                    b.Property<bool>("Estado");

                    b.Property<DateTime>("Fecha");

                    b.Property<int?>("LastQuestionUpdated");

                    b.Property<string>("NotasEvaluacion")
                        .HasMaxLength(4000);

                    b.Property<string>("NotasObjetivos")
                        .HasMaxLength(4000);

                    b.Property<int>("ProyectoId");

                    b.Property<double>("Puntuacion");

                    b.Property<string>("UserNombre");

                    b.HasKey("Id");

                    b.HasIndex("AssessmentId");

                    b.HasIndex("ProyectoId");

                    b.HasIndex("UserNombre");

                    b.ToTable("Evaluaciones");
                });

            modelBuilder.Entity("everisapi.API.Entities.IdiomasEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Idioma")
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.ToTable("Idiomas");
                });

            modelBuilder.Entity("everisapi.API.Entities.LineaEntity", b =>
                {
                    b.Property<int>("LineaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("LineaNombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int>("UnidadId");

                    b.HasKey("LineaId");

                    b.HasIndex("UnidadId");

                    b.ToTable("Linea");
                });

            modelBuilder.Entity("everisapi.API.Entities.NotasAsignacionesEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AsignacionId");

                    b.Property<int>("EvaluacionId");

                    b.Property<string>("Notas")
                        .HasMaxLength(4000);

                    b.HasKey("Id");

                    b.HasIndex("AsignacionId");

                    b.HasIndex("EvaluacionId");

                    b.ToTable("NotasAsignaciones");
                });

            modelBuilder.Entity("everisapi.API.Entities.NotasSectionsEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("EvaluacionId");

                    b.Property<string>("Notas")
                        .HasMaxLength(4000);

                    b.Property<int>("SectionId");

                    b.HasKey("Id");

                    b.HasIndex("EvaluacionId");

                    b.HasIndex("SectionId");

                    b.ToTable("NotasSections");
                });

            modelBuilder.Entity("everisapi.API.Entities.OficinaEntity", b =>
                {
                    b.Property<int>("OficinaId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("OficinaNombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("OficinaId");

                    b.ToTable("Oficina");
                });

            modelBuilder.Entity("everisapi.API.Entities.PreguntaEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AsignacionId");

                    b.Property<string>("Correcta");

                    b.Property<bool>("EsHabilitante");

                    b.Property<int>("Nivel");

                    b.Property<float>("Peso");

                    b.Property<string>("Pregunta")
                        .IsRequired()
                        .HasMaxLength(120);

                    b.Property<int?>("PreguntaHabilitanteId");

                    b.HasKey("Id");

                    b.HasIndex("AsignacionId");

                    b.HasIndex("PreguntaHabilitanteId");

                    b.ToTable("Preguntas");
                });

            modelBuilder.Entity("everisapi.API.Entities.ProyectoEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Codigo")
                        .HasMaxLength(100);

                    b.Property<DateTime>("Fecha");

                    b.Property<int?>("LineaId")
                        .IsRequired();

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<string>("Oficina")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("OficinaId")
                        .IsRequired();

                    b.Property<int>("ProjectSize");

                    b.Property<string>("Proyecto")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("TestProject");

                    b.Property<string>("Unidad")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<int?>("UnidadId")
                        .IsRequired();

                    b.Property<string>("UserNombre");

                    b.HasKey("Id");

                    b.HasIndex("LineaId");

                    b.HasIndex("OficinaId");

                    b.HasIndex("UnidadId");

                    b.HasIndex("UserNombre");

                    b.ToTable("Proyectos");
                });

            modelBuilder.Entity("everisapi.API.Entities.RespuestaEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Estado")
                        .HasMaxLength(5);

                    b.Property<int>("EvaluacionId");

                    b.Property<string>("Notas")
                        .HasMaxLength(4000);

                    b.Property<string>("NotasAdmin")
                        .HasMaxLength(4000);

                    b.Property<int>("PreguntaId");

                    b.HasKey("Id");

                    b.HasIndex("EvaluacionId");

                    b.HasIndex("PreguntaId");

                    b.ToTable("Respuestas");
                });

            modelBuilder.Entity("everisapi.API.Entities.RoleEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Role")
                        .IsRequired();

                    b.HasKey("Id");

                    b.ToTable("Roles");
                });

            modelBuilder.Entity("everisapi.API.Entities.SectionEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AssessmentId");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(120);

                    b.Property<int>("Peso")
                        .HasMaxLength(50);

                    b.Property<int>("PesoNivel1");

                    b.Property<int>("PesoNivel2");

                    b.Property<int>("PesoNivel3");

                    b.HasKey("Id");

                    b.HasIndex("AssessmentId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesAsignacionesEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AsignacionesId");

                    b.Property<int>("IdiomaId");

                    b.Property<string>("Traduccion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.HasIndex("AsignacionesId");

                    b.HasIndex("IdiomaId");

                    b.ToTable("TraduccionesAsignaciones");
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesIdiomasEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdiomaId");

                    b.Property<string>("Traduccion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.HasIndex("IdiomaId");

                    b.ToTable("TraduccionesIdiomas");
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesOficinasEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdiomaId");

                    b.Property<int>("OficinaId");

                    b.Property<string>("Traduccion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.HasIndex("IdiomaId");

                    b.HasIndex("OficinaId");

                    b.ToTable("TraduccionesOficinas");
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesPreguntasEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdiomaId");

                    b.Property<int>("PreguntaId");

                    b.Property<string>("Traduccion")
                        .IsRequired()
                        .HasMaxLength(500);

                    b.HasKey("ID");

                    b.HasIndex("IdiomaId");

                    b.HasIndex("PreguntaId");

                    b.ToTable("TraduccionesPreguntas");
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesRolesEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdiomaId");

                    b.Property<int>("RoleId");

                    b.Property<string>("Traduccion")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("ID");

                    b.HasIndex("IdiomaId");

                    b.HasIndex("RoleId");

                    b.ToTable("TraduccionesRoles");
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesSectionsEntity", b =>
                {
                    b.Property<int>("ID")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("IdiomaId");

                    b.Property<int>("SectionsId");

                    b.Property<string>("Traduccion")
                        .IsRequired()
                        .HasMaxLength(120);

                    b.HasKey("ID");

                    b.HasIndex("IdiomaId");

                    b.HasIndex("SectionsId");

                    b.ToTable("TraduccionesSections");
                });

            modelBuilder.Entity("everisapi.API.Entities.UnidadEntity", b =>
                {
                    b.Property<int>("UnidadId")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("OficinaId");

                    b.Property<string>("UnidadNombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.HasKey("UnidadId");

                    b.HasIndex("OficinaId");

                    b.ToTable("Unidad");
                });

            modelBuilder.Entity("everisapi.API.Entities.UserEntity", b =>
                {
                    b.Property<string>("Nombre");

                    b.Property<bool>("Activo");

                    b.Property<string>("NombreCompleto");

                    b.Property<string>("Password")
                        .IsRequired();

                    b.Property<int>("RoleId");

                    b.HasKey("Nombre");

                    b.HasIndex("RoleId");

                    b.ToTable("Users");
                });

            modelBuilder.Entity("everisapi.API.Entities.UserProyectoEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ProyectoId");

                    b.Property<string>("UserNombre")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("ProyectoId");

                    b.HasIndex("UserNombre");

                    b.ToTable("UserProyectos");
                });

            modelBuilder.Entity("everisapi.API.Entities.AsignacionEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.SectionEntity", "SectionEntity")
                        .WithMany("Asignaciones")
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.EvaluacionEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.AssessmentEntity", "Assessment")
                        .WithMany()
                        .HasForeignKey("AssessmentId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.PreguntaEntity", "PreguntaEntity")
                        .WithMany()
                        .HasForeignKey("Id")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.ProyectoEntity", "ProyectoEntity")
                        .WithMany("Evaluaciones")
                        .HasForeignKey("ProyectoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.UserEntity", "UserEntity")
                        .WithMany()
                        .HasForeignKey("UserNombre");
                });

            modelBuilder.Entity("everisapi.API.Entities.LineaEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.UnidadEntity", "UnidadEntity")
                        .WithMany()
                        .HasForeignKey("UnidadId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.NotasAsignacionesEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.AsignacionEntity", "AsignacionEntity")
                        .WithMany()
                        .HasForeignKey("AsignacionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.EvaluacionEntity", "EvaluacionEntity")
                        .WithMany()
                        .HasForeignKey("EvaluacionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.NotasSectionsEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.EvaluacionEntity", "EvaluacionEntity")
                        .WithMany()
                        .HasForeignKey("EvaluacionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.SectionEntity", "SectionEntity")
                        .WithMany()
                        .HasForeignKey("SectionId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.PreguntaEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.AsignacionEntity", "AsignacionEntity")
                        .WithMany("PreguntasDeAsignacion")
                        .HasForeignKey("AsignacionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.PreguntaEntity", "PreguntaHabilitante")
                        .WithMany()
                        .HasForeignKey("PreguntaHabilitanteId");
                });

            modelBuilder.Entity("everisapi.API.Entities.ProyectoEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.LineaEntity", "LineaEntity")
                        .WithMany()
                        .HasForeignKey("LineaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.OficinaEntity", "OficinaEntity")
                        .WithMany()
                        .HasForeignKey("OficinaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.UnidadEntity", "UnidadEntity")
                        .WithMany()
                        .HasForeignKey("UnidadId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.UserEntity", "UserEntity")
                        .WithMany("ProyectosDeUsuario")
                        .HasForeignKey("UserNombre");
                });

            modelBuilder.Entity("everisapi.API.Entities.RespuestaEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.EvaluacionEntity", "EvaluacionEntity")
                        .WithMany("Respuestas")
                        .HasForeignKey("EvaluacionId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.PreguntaEntity", "PreguntaEntity")
                        .WithMany()
                        .HasForeignKey("PreguntaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.SectionEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.AssessmentEntity", "Assessment")
                        .WithMany()
                        .HasForeignKey("AssessmentId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesAsignacionesEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.AsignacionEntity", "AsignacionEntity")
                        .WithMany()
                        .HasForeignKey("AsignacionesId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.IdiomasEntity", "IdiomasEntity")
                        .WithMany()
                        .HasForeignKey("IdiomaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesIdiomasEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.IdiomasEntity", "IdiomasEntity")
                        .WithMany()
                        .HasForeignKey("IdiomaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesOficinasEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.IdiomasEntity", "IdiomasEntity")
                        .WithMany()
                        .HasForeignKey("IdiomaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.OficinaEntity", "OficinaEntity")
                        .WithMany()
                        .HasForeignKey("OficinaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesPreguntasEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.IdiomasEntity", "IdiomasEntity")
                        .WithMany()
                        .HasForeignKey("IdiomaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.PreguntaEntity", "PreguntaEntity")
                        .WithMany()
                        .HasForeignKey("PreguntaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesRolesEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.IdiomasEntity", "IdiomasEntity")
                        .WithMany()
                        .HasForeignKey("IdiomaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.RoleEntity", "RoleEntity")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.TraduccionesSectionsEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.IdiomasEntity", "IdiomasEntity")
                        .WithMany()
                        .HasForeignKey("IdiomaId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.SectionEntity", "SectionEntity")
                        .WithMany()
                        .HasForeignKey("SectionsId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.UnidadEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.OficinaEntity", "OficinaEntity")
                        .WithMany()
                        .HasForeignKey("OficinaId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.UserEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.RoleEntity", "Role")
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("everisapi.API.Entities.UserProyectoEntity", b =>
                {
                    b.HasOne("everisapi.API.Entities.ProyectoEntity", "Proyecto")
                        .WithMany()
                        .HasForeignKey("ProyectoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.UserEntity", "User")
                        .WithMany()
                        .HasForeignKey("UserNombre")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

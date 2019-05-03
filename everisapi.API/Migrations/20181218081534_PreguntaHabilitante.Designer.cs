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
    [Migration("20181218081534_PreguntaHabilitante")]
    partial class PreguntaHabilitante
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
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

            modelBuilder.Entity("everisapi.API.Entities.PreguntaEntity", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("AsignacionId");

                    b.Property<string>("Correcta");

                    b.Property<bool>("EsHabilitante");

                    b.Property<int>("Peso")
                        .HasMaxLength(50);

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

                    b.Property<DateTime>("Fecha");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasMaxLength(50);

                    b.Property<bool>("TestProject");

                    b.Property<string>("UserNombre");

                    b.HasKey("Id");

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

                    b.HasKey("Id");

                    b.HasIndex("AssessmentId");

                    b.ToTable("Sections");
                });

            modelBuilder.Entity("everisapi.API.Entities.UserEntity", b =>
                {
                    b.Property<string>("Nombre");

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

                    b.HasOne("everisapi.API.Entities.ProyectoEntity", "ProyectoEntity")
                        .WithMany("Evaluaciones")
                        .HasForeignKey("ProyectoId")
                        .OnDelete(DeleteBehavior.Cascade);

                    b.HasOne("everisapi.API.Entities.UserEntity", "UserEntity")
                        .WithMany()
                        .HasForeignKey("UserNombre");
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

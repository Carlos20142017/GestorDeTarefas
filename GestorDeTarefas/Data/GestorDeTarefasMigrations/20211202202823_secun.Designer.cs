﻿// <auto-generated />
using System;
using GestorDeTarefas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GestorDeTarefas.Data.GestorDeTarefasMigrations
{
    [DbContext(typeof(GestorDeTarefasContext))]
    [Migration("20211202202823_secun")]
    partial class secun
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("GestorDeTarefas.Models.Colaborador", b =>
                {
                    b.Property<int>("ColaboradorId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("Cargo")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Contacto")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<int?>("SistemaProdutividadeId")
                        .HasColumnType("int");

                    b.HasKey("ColaboradorId");

                    b.HasIndex("SistemaProdutividadeId");

                    b.ToTable("Colaborador");
                });

            modelBuilder.Entity("GestorDeTarefas.Models.SistemaProdutividade", b =>
                {
                    b.Property<int>("SistemaProdutividadeId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("ColaboradorId")
                        .HasColumnType("int");

                    b.Property<string>("Concluido")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Estamos_a_fazer")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.Property<string>("O_que_fazer")
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("SistemaProdutividadeId");

                    b.HasIndex("ColaboradorId");

                    b.ToTable("SistemaProdutividade");
                });

            modelBuilder.Entity("GestorDeTarefas.Models.Tarefas", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ColaboradorId")
                        .HasColumnType("int");

                    b.Property<DateTime>("DataFim")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DataInicio")
                        .HasColumnType("datetime2");

                    b.Property<string>("Nome")
                        .IsRequired()
                        .HasMaxLength(80)
                        .HasColumnType("nvarchar(80)");

                    b.HasKey("Id");

                    b.HasIndex("ColaboradorId");

                    b.ToTable("Tarefas");
                });

            modelBuilder.Entity("GestorDeTarefas.Models.Colaborador", b =>
                {
                    b.HasOne("GestorDeTarefas.Models.SistemaProdutividade", null)
                        .WithMany("Colaboradors")
                        .HasForeignKey("SistemaProdutividadeId");
                });

            modelBuilder.Entity("GestorDeTarefas.Models.SistemaProdutividade", b =>
                {
                    b.HasOne("GestorDeTarefas.Models.Colaborador", "Colaborador")
                        .WithMany()
                        .HasForeignKey("ColaboradorId");

                    b.Navigation("Colaborador");
                });

            modelBuilder.Entity("GestorDeTarefas.Models.Tarefas", b =>
                {
                    b.HasOne("GestorDeTarefas.Models.Colaborador", "Colaborador")
                        .WithMany("Tarefas")
                        .HasForeignKey("ColaboradorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colaborador");
                });

            modelBuilder.Entity("GestorDeTarefas.Models.Colaborador", b =>
                {
                    b.Navigation("Tarefas");
                });

            modelBuilder.Entity("GestorDeTarefas.Models.SistemaProdutividade", b =>
                {
                    b.Navigation("Colaboradors");
                });
#pragma warning restore 612, 618
        }
    }
}
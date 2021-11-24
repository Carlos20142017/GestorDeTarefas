﻿// <auto-generated />
using GestorDeTarefas.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace GestorDeTarefas.Data.QuadrosMigration
{
    [DbContext(typeof(GestorDeTarefasContext))]
    [Migration("20211121221439_Tarefa")]
    partial class Tarefa
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

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("ColaboradorId");

                    b.ToTable("Colaborador");
                });

            modelBuilder.Entity("GestorDeTarefas.Models.Quadros", b =>
                {
                    b.Property<int>("QuadrosId")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("ColaboradorId")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(256)
                        .HasColumnType("nvarchar(256)");

                    b.HasKey("QuadrosId");

                    b.HasIndex("ColaboradorId");

                    b.ToTable("Quadros");
                });

            modelBuilder.Entity("GestorDeTarefas.Models.Quadros", b =>
                {
                    b.HasOne("GestorDeTarefas.Models.Colaborador", "Colaborador")
                        .WithMany("Quadros")
                        .HasForeignKey("ColaboradorId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Colaborador");
                });

            modelBuilder.Entity("GestorDeTarefas.Models.Colaborador", b =>
                {
                    b.Navigation("Quadros");
                });
#pragma warning restore 612, 618
        }
    }
}

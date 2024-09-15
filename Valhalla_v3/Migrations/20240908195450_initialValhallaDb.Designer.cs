﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Valhalla_v3.Database;

#nullable disable

namespace Valhalla_v3.Migrations
{
    [DbContext(typeof(ValhallaComtext))]
    [Migration("20240908195450_initialValhallaDb")]
    partial class initialValhallaDb
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Valhalla_v3.Shared.Operator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Password")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Operators", (string)null);
                });

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Comment", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Content")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DateTimeAdd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTimeModify")
                        .HasColumnType("datetime2");

                    b.Property<int>("OperatorCreateId")
                        .HasColumnType("int");

                    b.Property<int>("OperatorModifyId")
                        .HasColumnType("int");

                    b.Property<int?>("TaskId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OperatorCreateId");

                    b.HasIndex("OperatorModifyId");

                    b.HasIndex("TaskId");

                    b.ToTable("TaskComments", (string)null);
                });

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Project", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTimeAdd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTimeModify")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("OperatorCreateId")
                        .HasColumnType("int");

                    b.Property<int>("OperatorModifyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OperatorCreateId");

                    b.HasIndex("OperatorModifyId");

                    b.ToTable("Projects", (string)null);
                });

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Task", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTimeAdd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTimeModify")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<bool>("IsCompleted")
                        .HasColumnType("bit");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("OperatorCreateId")
                        .HasColumnType("int");

                    b.Property<int>("OperatorModifyId")
                        .HasColumnType("int");

                    b.Property<int>("ProjectId")
                        .HasColumnType("int");

                    b.Property<DateTime?>("Term")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.HasIndex("OperatorCreateId");

                    b.HasIndex("OperatorModifyId");

                    b.HasIndex("ProjectId");

                    b.ToTable("Tasks", (string)null);
                });

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Comment", b =>
                {
                    b.HasOne("Valhalla_v3.Shared.Operator", "OperatorCreate")
                        .WithMany()
                        .HasForeignKey("OperatorCreateId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Valhalla_v3.Shared.Operator", "OperatorModify")
                        .WithMany()
                        .HasForeignKey("OperatorModifyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Valhalla_v3.Shared.ToDo.Task", null)
                        .WithMany("Comments")
                        .HasForeignKey("TaskId");

                    b.Navigation("OperatorCreate");

                    b.Navigation("OperatorModify");
                });

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Project", b =>
                {
                    b.HasOne("Valhalla_v3.Shared.Operator", "OperatorCreate")
                        .WithMany()
                        .HasForeignKey("OperatorCreateId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Valhalla_v3.Shared.Operator", "OperatorModify")
                        .WithMany()
                        .HasForeignKey("OperatorModifyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("OperatorCreate");

                    b.Navigation("OperatorModify");
                });

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Task", b =>
                {
                    b.HasOne("Valhalla_v3.Shared.Operator", "OperatorCreate")
                        .WithMany()
                        .HasForeignKey("OperatorCreateId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Valhalla_v3.Shared.Operator", "OperatorModify")
                        .WithMany()
                        .HasForeignKey("OperatorModifyId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Valhalla_v3.Shared.ToDo.Project", "Project")
                        .WithMany("Tasks")
                        .HasForeignKey("ProjectId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.Navigation("OperatorCreate");

                    b.Navigation("OperatorModify");

                    b.Navigation("Project");
                });

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Project", b =>
                {
                    b.Navigation("Tasks");
                });

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Task", b =>
                {
                    b.Navigation("Comments");
                });
#pragma warning restore 612, 618
        }
    }
}

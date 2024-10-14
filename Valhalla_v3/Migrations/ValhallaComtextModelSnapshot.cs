﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Valhalla_v3.Database;

#nullable disable

namespace Valhalla_v3.Migrations
{
    [DbContext(typeof(ValhallaComtext))]
    partial class ValhallaComtextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Brand")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<DateTime>("DateTimeAdd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTimeModify")
                        .HasColumnType("datetime2");

                    b.Property<int>("EngineCC")
                        .HasColumnType("int");

                    b.Property<string>("Model")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("OperatorCreateId")
                        .HasColumnType("int");

                    b.Property<int>("OperatorModifyId")
                        .HasColumnType("int");

                    b.Property<string>("VIN")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("nvarchar(100)");

                    b.Property<int>("Year")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("OperatorCreateId");

                    b.HasIndex("OperatorModifyId");

                    b.ToTable("Car", (string)null);
                });

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.CarHistoryFuel", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("CostPerLitr")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTimeAdd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTimeModify")
                        .HasColumnType("datetime2");

                    b.Property<int>("GasStationId")
                        .HasColumnType("int");

                    b.Property<int?>("GasStationId1")
                        .HasColumnType("int");

                    b.Property<int>("Mileage")
                        .HasColumnType("int");

                    b.Property<int>("OperatorCreateId")
                        .HasColumnType("int");

                    b.Property<int>("OperatorModifyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("GasStationId");

                    b.HasIndex("GasStationId1");

                    b.HasIndex("OperatorCreateId");

                    b.HasIndex("OperatorModifyId");

                    b.ToTable("CarHistoryFuel", (string)null);
                });

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.CarHistoryRepair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("CarId")
                        .HasColumnType("int");

                    b.Property<decimal>("Cost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("Date")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTimeAdd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTimeModify")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<int>("MechanicId")
                        .HasColumnType("int");

                    b.Property<int?>("MechanicId1")
                        .HasColumnType("int");

                    b.Property<int>("Mileage")
                        .HasColumnType("int");

                    b.Property<int>("OperatorCreateId")
                        .HasColumnType("int");

                    b.Property<int>("OperatorModifyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("CarId");

                    b.HasIndex("MechanicId");

                    b.HasIndex("MechanicId1");

                    b.HasIndex("OperatorCreateId");

                    b.HasIndex("OperatorModifyId");

                    b.ToTable("CarHistoryRepair", (string)null);
                });

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.GasStation", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

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

                    b.Property<string>("Phone1")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone2")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("OperatorCreateId");

                    b.HasIndex("OperatorModifyId");

                    b.ToTable("GasStation", (string)null);
                });

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.Mechanic", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("City")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Country")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

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

                    b.Property<string>("Phone1")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Phone2")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("PostalCode")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Street")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("StreetNumber")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.HasKey("Id");

                    b.HasIndex("OperatorCreateId");

                    b.HasIndex("OperatorModifyId");

                    b.ToTable("Mechanic", (string)null);
                });

            modelBuilder.Entity("Valhalla_v3.Shared.Operator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<DateTime>("DateTimeAdd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTimeModify")
                        .HasColumnType("datetime2");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

                    b.Property<string>("Password")
                        .HasMaxLength(50)
                        .HasColumnType("nvarchar(50)");

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
                        .HasMaxLength(500)
                        .HasColumnType("nvarchar(500)");

                    b.Property<DateTime>("DateTimeAdd")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("DateTimeModify")
                        .HasColumnType("datetime2");

                    b.Property<int?>("JobId")
                        .HasColumnType("int");

                    b.Property<int>("OperatorCreateId")
                        .HasColumnType("int");

                    b.Property<int>("OperatorModifyId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("JobId");

                    b.HasIndex("OperatorCreateId");

                    b.HasIndex("OperatorModifyId");

                    b.ToTable("TaskComments", (string)null);
                });

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Job", b =>
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

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.Car", b =>
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

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.CarHistoryFuel", b =>
                {
                    b.HasOne("Valhalla_v3.Shared.CarHistory.Car", null)
                        .WithMany("Fuels")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Valhalla_v3.Shared.CarHistory.GasStation", "GasStation")
                        .WithMany()
                        .HasForeignKey("GasStationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Valhalla_v3.Shared.CarHistory.GasStation", null)
                        .WithMany("Fuels")
                        .HasForeignKey("GasStationId1");

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

                    b.Navigation("GasStation");

                    b.Navigation("OperatorCreate");

                    b.Navigation("OperatorModify");
                });

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.CarHistoryRepair", b =>
                {
                    b.HasOne("Valhalla_v3.Shared.CarHistory.Car", null)
                        .WithMany("CarHistoryRepair")
                        .HasForeignKey("CarId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Valhalla_v3.Shared.CarHistory.Mechanic", "Mechanic")
                        .WithMany()
                        .HasForeignKey("MechanicId")
                        .OnDelete(DeleteBehavior.NoAction)
                        .IsRequired();

                    b.HasOne("Valhalla_v3.Shared.CarHistory.Mechanic", null)
                        .WithMany("Repair")
                        .HasForeignKey("MechanicId1");

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

                    b.Navigation("Mechanic");

                    b.Navigation("OperatorCreate");

                    b.Navigation("OperatorModify");
                });

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.GasStation", b =>
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

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.Mechanic", b =>
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

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Comment", b =>
                {
                    b.HasOne("Valhalla_v3.Shared.ToDo.Job", null)
                        .WithMany("Comments")
                        .HasForeignKey("JobId");

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

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Job", b =>
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

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.Car", b =>
                {
                    b.Navigation("CarHistoryRepair");

                    b.Navigation("Fuels");
                });

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.GasStation", b =>
                {
                    b.Navigation("Fuels");
                });

            modelBuilder.Entity("Valhalla_v3.Shared.CarHistory.Mechanic", b =>
                {
                    b.Navigation("Repair");
                });

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Job", b =>
                {
                    b.Navigation("Comments");
                });

            modelBuilder.Entity("Valhalla_v3.Shared.ToDo.Project", b =>
                {
                    b.Navigation("Tasks");
                });
#pragma warning restore 612, 618
        }
    }
}

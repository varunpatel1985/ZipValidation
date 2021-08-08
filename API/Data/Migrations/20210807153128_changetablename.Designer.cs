﻿// <auto-generated />
using System;
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210807153128_changetablename")]
    partial class changetablename
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("API.Entities.AllowedFileTypes", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int?>("FileSetupId")
                        .HasColumnType("INTEGER");

                    b.Property<string>("FileTypeExtension")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.HasIndex("FileSetupId");

                    b.ToTable("AllowedFileTypes");
                });

            modelBuilder.Entity("API.Entities.FileSetup", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<string>("adminEmail")
                        .HasColumnType("TEXT");

                    b.Property<string>("source")
                        .HasColumnType("TEXT");

                    b.Property<string>("target")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("FileSetups");
                });

            modelBuilder.Entity("API.Entities.AllowedFileTypes", b =>
                {
                    b.HasOne("API.Entities.FileSetup", null)
                        .WithMany("allowedFileTypes")
                        .HasForeignKey("FileSetupId");
                });

            modelBuilder.Entity("API.Entities.FileSetup", b =>
                {
                    b.Navigation("allowedFileTypes");
                });
#pragma warning restore 612, 618
        }
    }
}
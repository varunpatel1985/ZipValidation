﻿// <auto-generated />
using API.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Data.Migrations
{
    [DbContext(typeof(DataContext))]
    [Migration("20210807142226_InitialCreate")]
    partial class InitialCreate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "5.0.8");

            modelBuilder.Entity("API.Entities.ZipValidation", b =>
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

                    b.ToTable("ZipValidations");
                });
#pragma warning restore 612, 618
        }
    }
}

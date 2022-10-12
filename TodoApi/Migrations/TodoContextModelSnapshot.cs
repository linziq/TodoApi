﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoApi.Context;

#nullable disable

namespace TodoApi.Migrations
{
    [DbContext(typeof(TodoContext))]
    partial class TodoContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.9")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TodoApi.Models.TodoListItem", b =>
                {
                    b.Property<int>("PrimaryID")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("PrimaryID"), 1L, 1);

                    b.Property<DateTime>("AddDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("DetailedContent")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("IsDone")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int?>("UserID")
                        .HasColumnType("int");

                    b.HasKey("PrimaryID");

                    b.ToTable("TodoListItems");
                });
#pragma warning restore 612, 618
        }
    }
}

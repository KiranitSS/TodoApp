﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using TodoAppWeb.Models;

#nullable disable

namespace TodoAppWeb.Migrations
{
    [DbContext(typeof(TodosDbContext))]
    partial class TodosDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "6.0.14")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, 1L, 1);

            modelBuilder.Entity("TodoApp_DomainEntities.Reminder", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("RemindTime")
                        .HasColumnType("datetime2");

                    b.Property<long?>("ReminderListId")
                        .HasColumnType("bigint");

                    b.Property<long>("TodoId")
                        .HasColumnType("bigint");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("ReminderListId");

                    b.ToTable("Reminders");
                });

            modelBuilder.Entity("TodoApp_DomainEntities.ReminderList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.HasKey("Id");

                    b.ToTable("ReminderLists");
                });

            modelBuilder.Entity("TodoApp_DomainEntities.Todo", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<DateTime>("CreationDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Description")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("DueDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<long>("ListId")
                        .HasColumnType("bigint");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long?>("TodoListId")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("TodoListId");

                    b.ToTable("Todo");
                });

            modelBuilder.Entity("TodoApp_DomainEntities.TodoList", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<long>("Id"), 1L, 1);

                    b.Property<bool>("IsDeleted")
                        .HasColumnType("bit");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TodoLists");
                });

            modelBuilder.Entity("TodoApp_DomainEntities.Reminder", b =>
                {
                    b.HasOne("TodoApp_DomainEntities.ReminderList", null)
                        .WithMany("TodoIds")
                        .HasForeignKey("ReminderListId");
                });

            modelBuilder.Entity("TodoApp_DomainEntities.Todo", b =>
                {
                    b.HasOne("TodoApp_DomainEntities.TodoList", null)
                        .WithMany("Todos")
                        .HasForeignKey("TodoListId");
                });

            modelBuilder.Entity("TodoApp_DomainEntities.ReminderList", b =>
                {
                    b.Navigation("TodoIds");
                });

            modelBuilder.Entity("TodoApp_DomainEntities.TodoList", b =>
                {
                    b.Navigation("Todos");
                });
#pragma warning restore 612, 618
        }
    }
}

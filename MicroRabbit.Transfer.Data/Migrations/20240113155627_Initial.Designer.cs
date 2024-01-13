﻿// <auto-generated />
using MicroRabbit.Transfer.Data.Context;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace MicroRabbit.Transfer.Data.Migrations
{
    [DbContext(typeof(TransferDbContext))]
    [Migration("20240113155627_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.1");

            modelBuilder.Entity("MicroRabbit.Transfer.Domain.Models.TransferLog", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER");

                    b.Property<int>("FromAccount")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ToAccount")
                        .HasColumnType("INTEGER");

                    b.Property<decimal>("TransferAmount")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("TransferLogs");
                });
#pragma warning restore 612, 618
        }
    }
}

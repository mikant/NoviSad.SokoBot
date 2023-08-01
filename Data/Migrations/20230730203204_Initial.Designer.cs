﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using NoviSad.SokoBot.Data;

#nullable disable

namespace NoviSad.SokoBot.Data.Migrations
{
    [DbContext(typeof(BotDbContext))]
    [Migration("20230730203204_Initial")]
    partial class Initial
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "7.0.9");

            modelBuilder.Entity("NoviSad.SokoBot.Data.Dto.PassengerDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<string>("Nickname")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("nickname");

                    b.HasKey("Id");

                    b.HasIndex("Nickname")
                        .IsUnique();

                    b.ToTable("passenger", (string)null);
                });

            modelBuilder.Entity("NoviSad.SokoBot.Data.Dto.TrainDto", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("INTEGER")
                        .HasColumnName("id");

                    b.Property<long>("ArrivalTime")
                        .HasColumnType("INTEGER")
                        .HasColumnName("arrival_time");

                    b.Property<long>("DepartureTime")
                        .HasColumnType("INTEGER")
                        .HasColumnName("departure_time");

                    b.Property<int>("Direction")
                        .HasColumnType("INTEGER")
                        .HasColumnName("direction");

                    b.Property<string>("Tag")
                        .HasColumnType("TEXT")
                        .HasColumnName("tag");

                    b.Property<int>("TrainNumber")
                        .HasColumnType("INTEGER")
                        .HasColumnName("number");

                    b.HasKey("Id");

                    b.HasIndex("ArrivalTime");

                    b.HasIndex("DepartureTime");

                    b.HasIndex("Id", "DepartureTime")
                        .IsUnique();

                    b.ToTable("train", (string)null);
                });

            modelBuilder.Entity("PassengerDtoTrainDto", b =>
                {
                    b.Property<int>("PassengersId")
                        .HasColumnType("INTEGER");

                    b.Property<int>("TrainsId")
                        .HasColumnType("INTEGER");

                    b.HasKey("PassengersId", "TrainsId");

                    b.HasIndex("TrainsId");

                    b.ToTable("PassengerDtoTrainDto");
                });

            modelBuilder.Entity("PassengerDtoTrainDto", b =>
                {
                    b.HasOne("NoviSad.SokoBot.Data.Dto.PassengerDto", null)
                        .WithMany()
                        .HasForeignKey("PassengersId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("NoviSad.SokoBot.Data.Dto.TrainDto", null)
                        .WithMany()
                        .HasForeignKey("TrainsId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}
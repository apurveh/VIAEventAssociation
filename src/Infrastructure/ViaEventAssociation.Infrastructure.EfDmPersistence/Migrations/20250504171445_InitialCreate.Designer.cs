﻿// <auto-generated />
using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace ViaEventAssociation.Infrastructure.SqliteDmPersistence.Migrations
{
    [DbContext(typeof(DmContext))]
    [Migration("20250504171445_InitialCreate")]
    partial class InitialCreate
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.4");

            modelBuilder.Entity("Event", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Status")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Status");

                    b.Property<string>("Visibility")
                        .IsRequired()
                        .HasColumnType("TEXT")
                        .HasColumnName("Visibility");

                    b.ComplexProperty<Dictionary<string, object>>("Description", "Event.Description#EventDescription", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("Description");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("MaxNumberOfGuests", "Event.MaxNumberOfGuests#NumberOfGuests", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<int>("Value")
                                .HasColumnType("INTEGER")
                                .HasColumnName("NumberOfGuests");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("Title", "Event.Title#EventTitle", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("Title");
                        });

                    b.HasKey("Id");

                    b.ToTable("Events");
                });

            modelBuilder.Entity("Organizer", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.ComplexProperty<Dictionary<string, object>>("OrganizerEmail", "Organizer.OrganizerEmail#Email", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("TEXT")
                                .HasColumnName("Email");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("OrganizerName", "Organizer.OrganizerName#OrganizerName", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasMaxLength(100)
                                .HasColumnType("TEXT")
                                .HasColumnName("Name");
                        });

                    b.HasKey("Id");

                    b.ToTable("Organizers");
                });

            modelBuilder.Entity("ViaEventAssociation.Core.Domain.Agregates.Guests.Guest", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.ComplexProperty<Dictionary<string, object>>("Email", "ViaEventAssociation.Core.Domain.Agregates.Guests.Guest.Email#Email", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("Email");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("FirstName", "ViaEventAssociation.Core.Domain.Agregates.Guests.Guest.FirstName#NameType", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("FirstName");
                        });

                    b.ComplexProperty<Dictionary<string, object>>("LastName", "ViaEventAssociation.Core.Domain.Agregates.Guests.Guest.LastName#NameType", b1 =>
                        {
                            b1.IsRequired();

                            b1.Property<string>("Value")
                                .IsRequired()
                                .HasColumnType("TEXT")
                                .HasColumnName("LastName");
                        });

                    b.HasKey("Id");

                    b.ToTable("Guests");
                });

            modelBuilder.Entity("ViaEventAssociation.Core.Domain.Agregates.Locations.Location", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.HasKey("Id");

                    b.ToTable("Locations");
                });

            modelBuilder.Entity("ViaEventAssociation.Core.Domain.Entities.Participation", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("TEXT");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasMaxLength(13)
                        .HasColumnType("TEXT");

                    b.Property<string>("EventId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<string>("GuestId")
                        .IsRequired()
                        .HasColumnType("TEXT");

                    b.Property<int>("ParticipationStatus")
                        .HasColumnType("INTEGER");

                    b.Property<int>("ParticipationType")
                        .HasColumnType("INTEGER");

                    b.HasKey("Id");

                    b.HasIndex("EventId");

                    b.HasIndex("GuestId");

                    b.ToTable("Participation");

                    b.HasDiscriminator<string>("Discriminator").HasValue("Participation");

                    b.UseTphMappingStrategy();
                });

            modelBuilder.Entity("JoinRequest", b =>
                {
                    b.HasBaseType("ViaEventAssociation.Core.Domain.Entities.Participation");

                    b.HasDiscriminator().HasValue("JoinRequest");
                });

            modelBuilder.Entity("ViaEventAssociation.Core.Domain.Entities.Invitation.Invitation", b =>
                {
                    b.HasBaseType("ViaEventAssociation.Core.Domain.Entities.Participation");

                    b.HasDiscriminator().HasValue("Invitation");
                });

            modelBuilder.Entity("Event", b =>
                {
                    b.OwnsOne("ViaEventAssociation.Core.Domain.Agregates.Events.EventDateTime", "TimeSpan", b1 =>
                        {
                            b1.Property<string>("EventId")
                                .HasColumnType("TEXT");

                            b1.Property<DateTime>("End")
                                .HasColumnType("datetime")
                                .HasColumnName("EventEnd");

                            b1.Property<DateTime>("Start")
                                .HasColumnType("datetime")
                                .HasColumnName("EventStart");

                            b1.HasKey("EventId");

                            b1.ToTable("Events");

                            b1.WithOwner()
                                .HasForeignKey("EventId");
                        });

                    b.Navigation("TimeSpan");
                });

            modelBuilder.Entity("ViaEventAssociation.Core.Domain.Entities.Participation", b =>
                {
                    b.HasOne("Event", "Event")
                        .WithMany("Participations")
                        .HasForeignKey("EventId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("ViaEventAssociation.Core.Domain.Agregates.Guests.Guest", "Guest")
                        .WithMany("Participations")
                        .HasForeignKey("GuestId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Event");

                    b.Navigation("Guest");
                });

            modelBuilder.Entity("Event", b =>
                {
                    b.Navigation("Participations");
                });

            modelBuilder.Entity("ViaEventAssociation.Core.Domain.Agregates.Guests.Guest", b =>
                {
                    b.Navigation("Participations");
                });
#pragma warning restore 612, 618
        }
    }
}

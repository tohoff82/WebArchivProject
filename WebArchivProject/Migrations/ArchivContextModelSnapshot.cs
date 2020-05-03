﻿// <auto-generated />
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WebArchivProject.Persistance.Contexts;

namespace WebArchivProject.Migrations
{
    [DbContext(typeof(ArchivContext))]
    partial class ArchivContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.3")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("WebArchivProject.Models.ArchivDb.AppUser", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("AvatarUrl")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Mail")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Password")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Role")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("AppUsers");
                });

            modelBuilder.Entity("WebArchivProject.Models.ArchivDb.Author", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<int>("BookId")
                        .HasColumnType("int");

                    b.Property<bool>("IsFirst")
                        .HasColumnType("tinyint(1)");

                    b.Property<string>("NameEn")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NameRu")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("NameUa")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("PostId")
                        .HasColumnType("int");

                    b.Property<int>("ThesisId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("BookId");

                    b.HasIndex("PostId");

                    b.HasIndex("ThesisId");

                    b.ToTable("Authors");
                });

            modelBuilder.Entity("WebArchivProject.Models.ArchivDb.Book", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Issuer")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("MaxPageCount")
                        .HasColumnType("int");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("Type")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Year")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Books");
                });

            modelBuilder.Entity("WebArchivProject.Models.ArchivDb.Post", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("Magazine")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("MagazineNumber")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("PagesInterval")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("TomName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Year")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Posts");
                });

            modelBuilder.Entity("WebArchivProject.Models.ArchivDb.Thesis", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<string>("City")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("ConferenceName")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Country")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("DatesInterval")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Name")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<int>("OwnerId")
                        .HasColumnType("int");

                    b.Property<string>("PagesInterval")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("Year")
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("Theses");
                });

            modelBuilder.Entity("WebArchivProject.Models.ArchivDb.Author", b =>
                {
                    b.HasOne("WebArchivProject.Models.ArchivDb.Book", "Book")
                        .WithMany("Authors")
                        .HasForeignKey("BookId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebArchivProject.Models.ArchivDb.Post", "Post")
                        .WithMany("AuthorsList")
                        .HasForeignKey("PostId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("WebArchivProject.Models.ArchivDb.Thesis", "Thesis")
                        .WithMany("AuthorsList")
                        .HasForeignKey("ThesisId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();
                });
#pragma warning restore 612, 618
        }
    }
}

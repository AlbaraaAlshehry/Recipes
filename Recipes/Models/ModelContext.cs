using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace Recipes.Models;

public partial class ModelContext : DbContext
{
    public ModelContext()
    {
    }

    public ModelContext(DbContextOptions<ModelContext> options)
        : base(options)
    {
    }

    public virtual DbSet<About> Abouts { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Contact> Contacts { get; set; }

    public virtual DbSet<Home> Homes { get; set; }

    public virtual DbSet<Payment> Payments { get; set; }

    public virtual DbSet<Recipe> Recipes { get; set; }

    public virtual DbSet<Role> Roles { get; set; }

    public virtual DbSet<Tetstimonial> Tetstimonials { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<UserLogin> UserLogins { get; set; }

    public virtual DbSet<UserRecipe> UserRecipes { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseOracle("USER ID=C##WORLDRECIPES;PASSWORD=Test123;DATA SOURCE=localhost:1521/xe");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .HasDefaultSchema("C##WORLDRECIPES")
            .UseCollation("USING_NLS_COMP");

        modelBuilder.Entity<About>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008343");

            entity.ToTable("ABOUT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.AboutImage)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ABOUT_IMAGE");
            entity.Property(e => e.AboutParagraph)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ABOUT_PARAGRAPH");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008317");

            entity.ToTable("CATEGORY");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CategoryName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("CATEGORY_NAME");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
        });

        modelBuilder.Entity<Contact>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008341");

            entity.ToTable("CONTACT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Text)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("TEXT");
        });

        modelBuilder.Entity<Home>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008339");

            entity.ToTable("HOME");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.AboutImage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ABOUT_IMAGE");
            entity.Property(e => e.AboutParagraph)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("ABOUT_PARAGRAPH");
            entity.Property(e => e.FooterAddress)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("FOOTER_ADDRESS");
            entity.Property(e => e.HeroImage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("HERO_IMAGE");
            entity.Property(e => e.HomeParagraph)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("HOME_PARAGRAPH");
            entity.Property(e => e.RecipesImage)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RECIPES_IMAGE");
        });

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008337");

            entity.ToTable("PAYMENT");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CardNumber)
                .HasColumnType("NUMBER")
                .HasColumnName("CARD_NUMBER");
            entity.Property(e => e.Ccv)
                .HasColumnType("NUMBER")
                .HasColumnName("CCV");
            entity.Property(e => e.ExDate)
                .HasColumnType("DATE")
                .HasColumnName("EX_DATE");
        });

        modelBuilder.Entity<Recipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008321");

            entity.ToTable("RECIPE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CategoryId)
                .HasColumnType("NUMBER")
                .HasColumnName("CATEGORY_ID");
            entity.Property(e => e.CreatedDate)
                .HasPrecision(6)
                .HasDefaultValueSql("current_timestamp")
                .HasColumnName("CREATED_DATE");
            entity.Property(e => e.Descreption)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("DESCREPTION");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
            entity.Property(e => e.Ingrediients)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("INGREDIIENTS");
            entity.Property(e => e.Preparation)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PREPARATION");
            entity.Property(e => e.Price)
                .HasColumnType("NUMBER")
                .HasColumnName("PRICE");
            entity.Property(e => e.RecipeName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("RECIPE_NAME");
            entity.Property(e => e.Status)
                .HasMaxLength(10)
                .IsUnicode(false)
                .HasDefaultValueSql("'PENDING'")
                .HasColumnName("STATUS");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Category).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.CategoryId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("CATEGORY_ID_FK");

            entity.HasOne(d => d.User).WithMany(p => p.Recipes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("CHEF_ID_FK");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008319");

            entity.ToTable("ROLE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.RoleName)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("ROLE_NAME");
        });

        modelBuilder.Entity<Tetstimonial>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008329");

            entity.ToTable("TETSTIMONIAL");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.CreatedDate)
                .HasPrecision(6)
                .HasDefaultValueSql("current_timestamp")
                .HasColumnName("CREATED_DATE");
            entity.Property(e => e.Status)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasDefaultValueSql("'PENDING'")
                .HasColumnName("STATUS");
            entity.Property(e => e.TestimonialText)
                .HasMaxLength(250)
                .IsUnicode(false)
                .HasColumnName("TESTIMONIAL_TEXT");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.User).WithMany(p => p.Tetstimonials)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USERS_TESTI_ID_FK");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008315");

            entity.ToTable("USERS");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Email)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("EMAIL");
            entity.Property(e => e.Fname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("FNAME");
            entity.Property(e => e.ImagePath)
                .HasMaxLength(100)
                .IsUnicode(false)
                .HasColumnName("IMAGE_PATH");
            entity.Property(e => e.Lname)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("LNAME");
        });

        modelBuilder.Entity<UserLogin>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008332");

            entity.ToTable("USER_LOGIN");

            entity.HasIndex(e => e.UserName, "SYS_C008333").IsUnique();

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.Password)
                .HasMaxLength(50)
                .IsUnicode(false)
                .HasColumnName("PASSWORD");
            entity.Property(e => e.RoleId)
                .HasColumnType("NUMBER")
                .HasColumnName("ROLE_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");
            entity.Property(e => e.UserName)
                .HasMaxLength(200)
                .IsUnicode(false)
                .HasColumnName("USER_NAME");

            entity.HasOne(d => d.Role).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.RoleId)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("ROLE_ID_FK");

            entity.HasOne(d => d.User).WithMany(p => p.UserLogins)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USERS_LOGIN_ID_FK");
        });

        modelBuilder.Entity<UserRecipe>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("SYS_C008325");

            entity.ToTable("USER_RECIPE");

            entity.Property(e => e.Id)
                .ValueGeneratedOnAdd()
                .HasColumnType("NUMBER")
                .HasColumnName("ID");
            entity.Property(e => e.ParchaseDate)
                .HasPrecision(6)
                .HasDefaultValueSql("current_timestamp")
                .HasColumnName("PARCHASE_DATE");
            entity.Property(e => e.RecipeId)
                .HasColumnType("NUMBER")
                .HasColumnName("RECIPE_ID");
            entity.Property(e => e.UserId)
                .HasColumnType("NUMBER")
                .HasColumnName("USER_ID");

            entity.HasOne(d => d.Recipe).WithMany(p => p.UserRecipes)
                .HasForeignKey(d => d.RecipeId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("RECIPE_ID_FK");

            entity.HasOne(d => d.User).WithMany(p => p.UserRecipes)
                .HasForeignKey(d => d.UserId)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("USERS_ID_FK");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

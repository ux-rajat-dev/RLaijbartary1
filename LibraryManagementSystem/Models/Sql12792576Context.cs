using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using Pomelo.EntityFrameworkCore.MySql.Scaffolding.Internal;

namespace LibraryManagementSystem.Models;

public partial class Sql12792576Context : DbContext
{
    public Sql12792576Context()
    {
    }

    public Sql12792576Context(DbContextOptions<Sql12792576Context> options)
        : base(options)
    {
    }

    public virtual DbSet<Author> Authors { get; set; }

    public virtual DbSet<Book> Books { get; set; }

    public virtual DbSet<BorrowTransaction> BorrowTransactions { get; set; }

    public virtual DbSet<Genre> Genres { get; set; }

    public virtual DbSet<RefreshToken> RefreshTokens { get; set; }

    public virtual DbSet<Review> Reviews { get; set; }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseMySql("server=sql12.freesqldatabase.com;port=3306;database=sql12792576;user=sql12792576;password=sU8AEYyvz1", Microsoft.EntityFrameworkCore.ServerVersion.Parse("5.5.62-mysql"));

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder
            .UseCollation("latin1_swedish_ci")
            .HasCharSet("latin1");

        modelBuilder.Entity<Author>(entity =>
        {
            entity.HasKey(e => e.AuthorId).HasName("PRIMARY");

            entity.Property(e => e.AuthorId).HasColumnType("int(11)");
            entity.Property(e => e.Bio).HasColumnType("text");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<Book>(entity =>
        {
            entity.HasKey(e => e.BookId).HasName("PRIMARY");

            entity.HasIndex(e => e.AuthorId, "AuthorId");

            entity.HasIndex(e => e.GenreId, "GenreId");

            entity.HasIndex(e => e.Isbn, "ISBN").IsUnique();

            entity.Property(e => e.BookId).HasColumnType("int(11)");
            entity.Property(e => e.AuthorId).HasColumnType("int(11)");
            entity.Property(e => e.AvailableCopies)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)");
            entity.Property(e => e.CoverImageUrl).HasColumnType("text");
            entity.Property(e => e.Description).HasColumnType("text");
            entity.Property(e => e.GenreId).HasColumnType("int(11)");
            entity.Property(e => e.Isbn)
                .HasMaxLength(20)
                .HasColumnName("ISBN");
            entity.Property(e => e.PublishedYear).HasColumnType("int(11)");
            entity.Property(e => e.Title).HasMaxLength(200);
            entity.Property(e => e.TotalCopies)
                .HasDefaultValueSql("'1'")
                .HasColumnType("int(11)");

            entity.HasOne(d => d.Author).WithMany(p => p.Books)
                .HasForeignKey(d => d.AuthorId)
                .HasConstraintName("Books_ibfk_1");

            entity.HasOne(d => d.Genre).WithMany(p => p.Books)
                .HasForeignKey(d => d.GenreId)
                .HasConstraintName("Books_ibfk_2");
        });

        modelBuilder.Entity<BorrowTransaction>(entity =>
        {
            entity.HasKey(e => e.TransactionId).HasName("PRIMARY");

            entity.HasIndex(e => e.BookId, "BookId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.TransactionId).HasColumnType("int(11)");
            entity.Property(e => e.BookId).HasColumnType("int(11)");
            entity.Property(e => e.FineAmount)
                .HasPrecision(10, 2)
                .HasDefaultValueSql("'0.00'");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.Book).WithMany(p => p.BorrowTransactions)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("BorrowTransactions_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.BorrowTransactions)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("BorrowTransactions_ibfk_1");
        });

        modelBuilder.Entity<Genre>(entity =>
        {
            entity.HasKey(e => e.GenreId).HasName("PRIMARY");

            entity.HasIndex(e => e.Name, "Name").IsUnique();

            entity.Property(e => e.GenreId).HasColumnType("int(11)");
            entity.Property(e => e.Name).HasMaxLength(100);
        });

        modelBuilder.Entity<RefreshToken>(entity =>
        {
            entity.HasKey(e => e.TokenId).HasName("PRIMARY");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.TokenId).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.IsRevoked).HasDefaultValueSql("'0'");
            entity.Property(e => e.Token).HasColumnType("text");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.User).WithMany(p => p.RefreshTokens)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("RefreshTokens_ibfk_1");
        });

        modelBuilder.Entity<Review>(entity =>
        {
            entity.HasKey(e => e.ReviewId).HasName("PRIMARY");

            entity.HasIndex(e => e.BookId, "BookId");

            entity.HasIndex(e => e.UserId, "UserId");

            entity.Property(e => e.ReviewId).HasColumnType("int(11)");
            entity.Property(e => e.BookId).HasColumnType("int(11)");
            entity.Property(e => e.Comment).HasColumnType("text");
            entity.Property(e => e.Rating).HasColumnType("int(11)");
            entity.Property(e => e.ReviewDate)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.UserId).HasColumnType("int(11)");

            entity.HasOne(d => d.Book).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.BookId)
                .HasConstraintName("Reviews_ibfk_2");

            entity.HasOne(d => d.User).WithMany(p => p.Reviews)
                .HasForeignKey(d => d.UserId)
                .HasConstraintName("Reviews_ibfk_1");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.UserId).HasName("PRIMARY");

            entity.HasIndex(e => e.Email, "Email").IsUnique();

            entity.Property(e => e.UserId).HasColumnType("int(11)");
            entity.Property(e => e.CreatedAt)
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .HasColumnType("timestamp");
            entity.Property(e => e.Email).HasMaxLength(100);
            entity.Property(e => e.FullName).HasMaxLength(100);
            entity.Property(e => e.PasswordHash).HasColumnType("text");
            entity.Property(e => e.Role)
                .HasDefaultValueSql("'user'")
                .HasColumnType("enum('admin','user')");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}

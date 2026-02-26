using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Book> Books => Set<Book>();
    public DbSet<Member> Member => Set<Member>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<BookIssue> BookIssue => Set<BookIssue>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Book>()
            .ToTable("Book")
            .HasOne(b => b.Category)
            .WithMany()
            .HasForeignKey(b => b.CategoryId);

        builder.Entity<BookIssue>()
            .HasOne(b => b.Book)
            .WithMany()
            .HasForeignKey(b => b.BookId);

        builder.Entity<BookIssue>()
            .HasOne(b => b.Member)
            .WithMany()
            .HasForeignKey(b => b.MemberId);
    }
}
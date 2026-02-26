using Microsoft.EntityFrameworkCore;

namespace LibraryManagementSystem.Persistence;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options)
    : DbContext(options)
{
    public DbSet<Book> Book => Set<Book>();
    public DbSet<Member> Member => Set<Member>();
    public DbSet<Category> Category => Set<Category>();
    public DbSet<BookIssue> BookIssue => Set<BookIssue>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Book>()
            .HasOne(b => b.Category)
            .WithMany(c => c.Book)  
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
using NewNew.Models.Domain;
using Microsoft.EntityFrameworkCore;

namespace NewNew.Data;

public class MVCDemoContext : DbContext
{
    public MVCDemoContext()
    {
    }

    public MVCDemoContext(DbContextOptions<MVCDemoContext> options) : base(options)
    {
    }

    public DbSet<Book> Books { get; set; }
    public DbSet<Member> Members { get; set; }
    public DbSet<BookCopy> BCopy { get; set; }
    public DbSet<BookAuthor> Authors { get; set; }
    public DbSet<Fine> fines { get; set; }
    public DbSet<FinePayment> finePayments { get; set; }
    public DbSet<Reservation> reservation { get; set; }
    public DbSet<Publisher> publisher { get; set; }
    public DbSet<Author> author { get; set; }
    public DbSet<BookCategory> bookCategory { get; set; }
    public DbSet<BookBorrow> bookBorrow { get; set; }
    public DbSet<Admin> admin { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configure the Book entity
        modelBuilder.Entity<Book>()
    .Property(b => b.bookId)
    .HasColumnType("int")
    .ValueGeneratedOnAdd();


        modelBuilder.Entity<Book>()
            .HasMany(b => b.Reservations)
            .WithOne(r => r.Book)
            .HasForeignKey(r => r.bookId);

        modelBuilder.Entity<Book>()
            .HasMany(b => b.bookCopies)
            .WithOne(bc => bc.Book)
            .HasForeignKey(bc => bc.bookId);

        modelBuilder.Entity<Book>()
            .HasOne(b => b.BookCategory)
            .WithMany(c => c.Books)
            .HasForeignKey(b => b.bookCategroryId);

        modelBuilder.Entity<Book>()
            .Property(b => b.bookId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<BookCopy>()
            .HasOne(b => b.Publisher)
            .WithMany(p => p.copies)
            .HasForeignKey(b => b.PublisherId);

        modelBuilder.Entity<BookCopy>()
            .Property(bc => bc.bookCopyId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Member>()
            .Property(m => m.memberId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Member>()
            .HasMany(m => m.Borrows)
            .WithOne(b => b.Member)
            .HasForeignKey(m => m.memberId);

        modelBuilder.Entity<Member>()
            .HasMany(m => m.finePayments)
            .WithOne(f => f.Member)
            .HasForeignKey(m => m.memberId);

        modelBuilder.Entity<Author>()
            .Property(a => a.authorId)
            .ValueGeneratedOnAdd();

        modelBuilder.Entity<Author>()
            .HasMany(a => a.BookAuthors)
            .WithOne(ba => ba.Author)
            .HasForeignKey(ba => ba.authorId);

        modelBuilder.Entity<BookAuthor>()
            .HasKey(ba => new { ba.bookId, ba.authorId });

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Member)
            .WithMany(m => m.reservations)
            .HasForeignKey(r => r.memberId);

        modelBuilder.Entity<BookCopy>()
            .HasMany(b => b.bookBorrows)
            .WithOne(bw => bw.BookCopy)
            .HasForeignKey(b => b.bookCopyId);

        modelBuilder.Entity<BookBorrow>()
            .HasOne(bb => bb.Member)
            .WithMany(m => m.Borrows)
            .HasForeignKey(bb => bb.memberId);

        modelBuilder.Entity<BookBorrow>()
            .HasMany(br => br.fines)
            .WithOne(f => f.bookBorrow)
            .HasForeignKey(br => br.borrowBookId);

        modelBuilder.Entity<Fine>()
            .HasOne(f => f.FinePayment)
            .WithOne(fp => fp.Fine)
            .HasForeignKey<FinePayment>(fp => fp.FineId);
        modelBuilder.Entity<Fine>()
               .HasIndex(f => new { f.borrowBookId, f.FineDate })
               .IsUnique();


        modelBuilder.Entity<Member>(entity =>
        {
            entity.ToTable("Members"); // Set the table name
            entity.HasKey(e => e.memberId); // Set the primary key

            // Configure the properties
            entity.Property(e => e.memberFullName).IsRequired();
            entity.Property(e => e.gender).IsRequired();
            entity.Property(e => e.email).IsRequired();
            entity.Property(e => e.phoneNumber).IsRequired();
            entity.Property(e => e.memberPassword).IsRequired();
        });
    } 
}

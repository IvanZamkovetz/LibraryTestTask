namespace Library.Model
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class LibraryContext : DbContext
    {
        public LibraryContext()
            : base("name=LibraryContext")
        {
        }

        public virtual DbSet<Book> Books { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Book>()
                .Property(e => e.RowVersion)
                .IsFixedLength();
        }
    }
}

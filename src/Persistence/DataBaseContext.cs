using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using src.Models;

namespace src.Persistence;

public class DataBaseContext : DbContext
{
    public DbSet<Person> Persons { get; set; }
    public DbSet<Contract> Contracts {get; set;} 

    public DataBaseContext(DbContextOptions<DataBaseContext> options) : base(options)  
    {

    }  

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<Person>(p => {
                                        p.HasKey(e => e.Id);

                                        p.HasMany(e => e.Contracts)
                                          .WithOne()
                                          .HasForeignKey(c => c.PersonId);
                                    });

        builder.Entity<Contract>(c => {
                                            c.HasKey(e => e.Id);
                                       });
    }
}
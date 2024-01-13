using MicroRabbit.Banking.Domain.Models;
using Microsoft.EntityFrameworkCore;

namespace MicroRabbit.Banking.Data.Context;

public class BankingDbContext(DbContextOptions<BankingDbContext> options):DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().HasData(
            new Account { Id = 1, AccountType = "Savings", AccountBalance = 1000.00m },
            new Account { Id = 2, AccountType = "Checking", AccountBalance = 500.50m }
            // Add more seed data as needed
        );
    }
    
}
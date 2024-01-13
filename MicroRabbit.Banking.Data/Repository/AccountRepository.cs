using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Data.Repository;

public class AccountRepository(BankingDbContext dbContext):IAccountRepository
{
    public IEnumerable<Account> GetAccounts()
    {
        return dbContext.Accounts;
    }
}
using MicroRabbit.Banking.Data.Context;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;

namespace MicroRabbit.Banking.Data.Repository;

public class AccountRepository(BankingDbContext _dbContext):IAccountRepository
{
    public IEnumerable<Account> GetAccounts()
    {
        return _dbContext.Accounts;
    }
}
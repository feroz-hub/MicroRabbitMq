using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Models;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Banking.Domain.Models;
using MicroRabbit.Domain.Core.Bus;

namespace MicroRabbit.Banking.Application.Services;

public class AccountService(IAccountRepository accountRepository,IEventBus eventBus):IAccountService
{
    public IEnumerable<Account> GetAccounts()
    {
        return accountRepository.GetAccounts();
    }

    public void TransferFunds(AccountTransfer accountTransfer)
    {
        var createTransferCommand = new CreateTransferCommand(
            accountTransfer.FromAccount, accountTransfer.ToAccount, accountTransfer.Amount);
        
        eventBus.SendCommand(createTransferCommand);
        

    }
}
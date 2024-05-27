using MicroRabbit.Transfer.Data.Context;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Data.Repository;

public class TransferRepository(TransferDbContext transferDbContext):ITransferRepository
{
    public IEnumerable<TransferLog> GetTransferLogs()
    {
        return transferDbContext.TransferLogs;
    }

    public void Add(TransferLog transferLog)
    {
        transferDbContext.TransferLogs.Add(transferLog);
        transferDbContext.SaveChanges();
    }
}
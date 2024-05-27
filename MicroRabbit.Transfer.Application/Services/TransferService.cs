using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Transfer.Application.Interfaces;
using MicroRabbit.Transfer.Domain.Interfaces;
using MicroRabbit.Transfer.Domain.Models;

namespace MicroRabbit.Transfer.Application.Services;

public class TransferService(ITransferRepository transferRepository):ITransferService
{
    public IEnumerable<TransferLog> GetTransferLogs()
    {
        return transferRepository.GetTransferLogs();
    }
}
using MediatR;
using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Repository;
using MicroRabbit.Banking.Domain.CommandHandlers;
using MicroRabbit.Banking.Domain.Commands;
using MicroRabbit.Banking.Domain.Interfaces;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC;

public static class DependencyContainer
{
    public static void RegisterService(this IServiceCollection serviceCollection)
    {
       
        //Domain Bus
        serviceCollection.AddScoped<IEventBus, RabbitMQBus>();
        //Domain Banking Commands


        serviceCollection.AddTransient<IRequestHandler<CreateTransferCommand ,bool>, TransferCommandHandler>();
        //Application Service
        serviceCollection.AddTransient<IAccountService, AccountService>();
        
        //Repository Service
        serviceCollection.AddTransient<IAccountRepository,AccountRepository>();
        serviceCollection.AddSingleton(typeof(Dictionary<string, List<Type>>));
        serviceCollection.AddSingleton(typeof(List<Type>));

    }
}
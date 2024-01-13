using MicroRabbit.Banking.Application.Interfaces;
using MicroRabbit.Banking.Application.Services;
using MicroRabbit.Banking.Data.Repository;
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
        
        //Application Service
        serviceCollection.AddTransient<IAccountService, AccountService>();
        
        //Repository Service
        serviceCollection.AddTransient<IAccountRepository,AccountRepository>();
        serviceCollection.AddSingleton(typeof(Dictionary<string, List<Type>>));
        serviceCollection.AddSingleton(typeof(List<Type>));

    }
}
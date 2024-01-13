using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Infra.Bus;
using Microsoft.Extensions.DependencyInjection;

namespace MicroRabbit.Infra.IoC;

public class DependencyContainer
{
    public static void RegisterService(IServiceCollection serviceCollection)
    {
        serviceCollection.AddTransient<IEventBus, RabbitMQBus>();
    }
}
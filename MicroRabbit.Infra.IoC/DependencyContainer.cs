namespace MicroRabbit.Infra.IoC;

public static class DependencyContainer
{
    public static void RegisterService(this IServiceCollection serviceCollection)
    {
       
        //Domain Bus
        serviceCollection.AddScoped<IEventBus, RabbitMqBus>();
        //Domain Banking Commands


        serviceCollection.AddTransient<IRequestHandler<CreateTransferCommand ,bool>, TransferCommandHandler>();
        //Application Service
        serviceCollection.AddScoped<IAccountService, AccountService>();
        
        //Repository Service
        serviceCollection.AddScoped<IAccountRepository,AccountRepository>();
        serviceCollection.AddSingleton(typeof(Dictionary<string, List<Type>>));
        serviceCollection.AddSingleton(typeof(List<Type>));

    }
}
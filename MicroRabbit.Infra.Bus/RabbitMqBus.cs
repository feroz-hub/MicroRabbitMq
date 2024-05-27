namespace MicroRabbit.Infra.Bus;

public sealed class RabbitMqBus(ISender mediator,IDictionary<string, List<Type>> handlers,ICollection<Type> eventTypes):IEventBus
{
    public Task SendCommand<T>(T command) where T : Command
    {
        return mediator.Send(command);
    }

    public void Publish<T>(T @event) where T : Event
    {
        using var connection = new ConnectionFactory{ HostName = "localhost"}.CreateConnection();
        using var channel = connection.CreateChannel();
        var eventName=@event.GetType().Name;
        channel.QueueDeclare(eventName, false, false, false, null);

        var message = JsonConvert.SerializeObject(@event);
        var body = Encoding.UTF8.GetBytes(message);
            
        channel.BasicPublish("",eventName,body);
    }

    public void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);

        if (!eventTypes.Contains(typeof(T))) eventTypes.Add(typeof(T));

        if (!handlers.TryGetValue(eventName, out var eventHandlers)) handlers[eventName] = [];

        if (eventHandlers != null && eventHandlers.Any(h => h == handlerType))
            throw new ArgumentException($"Handler Type {handlerType.Name} already is registered for {eventName}", nameof(handlerType));
        eventHandlers?.Add(handlerType);
        StartBasicConsume<T>();
    }

    private void StartBasicConsume<T>() where T : Event
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            DispatchConsumersAsync = true
        };

        var connection = factory.CreateConnection();
        var channel = connection.CreateChannel();
        
        var eventName=typeof(T).Name;
        
        channel.QueueDeclare(eventName,false,false,false,null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        
        consumer.Received+=Consumer_Received;

        channel.BasicConsume(eventName, true, consumer);
    }

    private async Task Consumer_Received(object sender, BasicDeliverEventArgs e)
    {
        var eventName = e.RoutingKey;
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            // ignored
        }
    }

    private Task ProcessEvent(string eventName, string message)
    {
        if (!handlers.TryGetValue(eventName, out var subscriptions))
            return Task.CompletedTask;

        foreach (var concreteType in from eventType in subscriptions.Select(subscription => Activator.CreateInstance(subscription)).OfType<object>().Select(handler => eventTypes.SingleOrDefault(t => t.Name == eventName)).OfType<Type>() let @event = JsonConvert.DeserializeObject(message, eventType) select typeof(IEventHandler<>).MakeGenericType(eventType))
        {
        }

        return Task.CompletedTask;
    }

    // private async Task ProcessEvent(string eventName, string message)
    // {
    //     if (handlers.ContainsKey(eventName))
    //     {
    //         var subscriptions = handlers[eventName];
    //         foreach (var subscription in subscriptions)
    //         {
    //             var handler = Activator.CreateInstance(subscription);
    //             if(handler==null) continue;
    //             var eventType=eventTypes.SingleOrDefault(t=>t.Name==eventName);
    //             var @event = JsonConvert.DeserializeObject(message, eventType);
    //             var concreteType=typeof(IEventHandler<>).MakeGenericType(eventType);
    //             await (Task)concreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
    //         }
    //     }
    // }
}
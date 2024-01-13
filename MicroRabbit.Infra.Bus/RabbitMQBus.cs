using System.Text;
using MediatR;
using MicroRabbit.Domain.Core.Bus;
using MicroRabbit.Domain.Core.Commands;
using MicroRabbit.Domain.Core.Events;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;

namespace MicroRabbit.Infra.Bus;

public sealed class RabbitMQBus(IMediator _mediator,Dictionary<string,List<Type>> _handlers,List<Type> _eventTypes):IEventBus
{
    public Task SendCommand<T>(T command) where T : Command
    {
        return _mediator.Send(command);
    }

    public void Publish<T>(T @event) where T : Event
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
        };
        using var connection = factory.CreateConnection();
        using (var channel = connection.CreateChannel())
        {
            var eventName=@event.GetType().Name;
            channel.QueueDeclare(eventName, false, false, false, null);

            var message = JsonConvert.SerializeObject(@event);
            var body = Encoding.UTF8.GetBytes(message);
            
            channel.BasicPublish("",eventName,body);
        }


    }

    public void Subscribe<T, TH>() where T : Event where TH : IEventHandler<T>
    {
        var eventName = typeof(T).Name;
        var handlerType = typeof(TH);

        if (!_eventTypes.Contains(typeof(T)))
        {
            _eventTypes.Add(typeof(T));
        }

        if (!_handlers.ContainsKey(eventName))
        {
            _handlers.Add(eventName,new List<Type>());
        }

        if (_handlers[eventName].Any(s => s.GetType() == handlerType))
        {
            throw new ArgumentException($"Handler Type {handlerType.Name} already is registered for {eventName}", nameof(handlerType));
        }
        
        _handlers[eventName].Add(handlerType);

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
        
        var eventname=typeof(T).Name;
        
        channel.QueueDeclare(eventname,false,false,false,null);

        var consumer = new AsyncEventingBasicConsumer(channel);
        
        consumer.Received+=Consumer_Recieved;

        channel.BasicConsume(eventname, true, consumer);
    }

    private async Task Consumer_Recieved(object sender, BasicDeliverEventArgs e)
    {
        var eventName = e.RoutingKey;
        var message = Encoding.UTF8.GetString(e.Body.ToArray());

        try
        {
            await ProcessEvent(eventName, message).ConfigureAwait(false);
        }
        catch (Exception ex)
        {
            
            
        }
    }

    private async Task ProcessEvent(string eventName, string message)
    {
        if (_handlers.ContainsKey(eventName))
        {
            var subscriptions = _handlers[eventName];
            foreach (var subscription in subscriptions)
            {
                var handler = Activator.CreateInstance(subscription);
                if(handler==null) continue;
                var eventType=_eventTypes.SingleOrDefault(t=>t.Name==eventName);
                var @event = JsonConvert.DeserializeObject(message, eventType);
                var conreteType=typeof(IEventHandler<>).MakeGenericType(eventType);
                await (Task)conreteType.GetMethod("Handle").Invoke(handler, new object[] { @event });
            }
        }
    }
}
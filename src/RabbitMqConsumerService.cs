using System;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.Hosting;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using IModel = RabbitMQ.Client.IModel;


namespace Webapi;

public class RabbitMqConsumerService : IHostedService
{
    private readonly IConnection _connection;
    private readonly IModel _channel;

    public RabbitMqConsumerService()
    {
        var factory = new ConnectionFactory
        {
            HostName = "localhost", // Replace with your RabbitMQ server details
            UserName = "guest",
            Password = "guest"
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        var consumer = new EventingBasicConsumer(_channel);
        consumer.Received += (model, ea) =>
        {
            var body = ea.Body.ToArray();
            var message = Encoding.UTF8.GetString(body);

            Console.WriteLine($"Received message: {message}");
        };

        _channel.BasicConsume(queue: "your_queue_name", autoAck: true, consumer: consumer);

        return Task.CompletedTask;
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        _channel.Dispose();
        _connection.Dispose();

        return Task.CompletedTask;
    }
}

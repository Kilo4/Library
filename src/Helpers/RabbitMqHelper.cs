using RabbitMQ.Client;
using Constants = Webapi.Types.Constant.Constants;

namespace Webapi.Helpers;

public class RabbitMqHelper(ConnectionFactory connectionFactory): IRabbitMqHelper
{
    public IModel createConnection()
    {
        var connection = connectionFactory.CreateConnection();
        var channel = connection.CreateModel();
        channel.QueueDeclare(queue: Constants.RabbitMqQueueName, durable: false, exclusive: false, autoDelete: false,
            arguments: null);
        return channel;
    }
}
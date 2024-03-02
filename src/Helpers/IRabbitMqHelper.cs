using RabbitMQ.Client;

namespace Webapi.Helpers;

public interface IRabbitMqHelper
{
    public IModel createConnection();
}
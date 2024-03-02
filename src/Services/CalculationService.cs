using System.Text;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Webapi.Dto.Response;
using Webapi.Helpers;
using Constants = Webapi.Types.Constant.Constants;

namespace Webapi.Services;

public class CalculationService(ConnectionFactory connectionFactory): ICalculationService
{
    public decimal Calculate(decimal inputValue, decimal previousValue)
    {
        return (decimal)Math.Pow(Math.E, Math.Log((double)inputValue) / (double)previousValue);
    }

    public void SendMessage(CalculationResponse message)
    {
        var channel = new RabbitMqHelper(connectionFactory).createConnection();
        var messageJson = JsonConvert.SerializeObject(message);
        var body = Encoding.UTF8.GetBytes(messageJson);

        channel.BasicPublish(exchange: String.Empty,
            routingKey: Constants.RabbitMqQueueName,
            basicProperties: null,
            body: body);
    
    }
}
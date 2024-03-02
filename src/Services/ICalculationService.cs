using RabbitMQ.Client;
using Webapi.Dto.Response;

namespace Webapi.Services;

public interface ICalculationService
{
    decimal Calculate(decimal inputValue, decimal previousValue);

    public void SendMessage(CalculationResponse message);
}
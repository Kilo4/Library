namespace Webapi.Services;

public interface ICalculationService
{
    decimal Calculate(decimal inputValue, decimal previousValue);
}
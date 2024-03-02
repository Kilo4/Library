namespace Webapi.Services;

public class CalculationService: ICalculationService
{
    public decimal Calculate(decimal inputValue, decimal previousValue)
    {
        return (decimal)Math.Pow(Math.E, Math.Log((double)inputValue) / (double)previousValue);
    }
}
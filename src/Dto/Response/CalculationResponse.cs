namespace Webapi.Dto.Response;

public class CalculationResponse
{
    public decimal ComputedValue { get; set; }
    public decimal InputValue { get; set; }
    public decimal PreviousValue { get; set; }
}
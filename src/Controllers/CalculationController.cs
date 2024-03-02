using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Webapi.Dto.Request;
using Webapi.Dto.Response;
using Webapi.Models.DbContext;
using Webapi.Services;

namespace Webapi.Controllers;

[Route("api/calculation/")]
[ApiController]
public class CalculationController(AppDbContext context, IMapper mapper, ICalculationService calculationService): ControllerBase
{
    [HttpPost("{key:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult> Calculation(int key, [FromBody] CalculationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        decimal inputValue = request.Input;
        // decimal previousValue = _keyValueStorage.GetValue(key.ToString());
        var previousValue = (decimal)key;

        // Your calculation logic here
        var calculatedValue = calculationService.Calculate(inputValue, previousValue);
        var response = new CalculationResponse
        {
            ComputedValue = calculatedValue,
            InputValue = inputValue,
            PreviousValue = previousValue
        };

        return Ok(response);
    }
}
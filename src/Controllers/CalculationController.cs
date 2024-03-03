using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using RabbitMQ.Client;
using Webapi.Dto.Request;
using Webapi.Dto.Response;
using Webapi.Models.DbContext;
using Webapi.Services;
using Webapi.Storage;
using Constants = Webapi.Types.Constant.Constants;

namespace Webapi.Controllers;

[Route("api/calculation/")]
[ApiController]
public class CalculationController(AppDbContext context, ICalculationService calculationService, IKeyValueStorage keyValueStorage, ConnectionFactory connectionFactory): ControllerBase
{
    [HttpPost("{key:int}")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    public async Task<ActionResult<CalculationResponse>> Calculation(int key, [FromBody] CalculationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        decimal inputValue = request.input;
        decimal previousValue = Constants.DefaultValueForCalculation;
        var cacheValue = keyValueStorage.GetValue(key.ToString());
        if (cacheValue.HasValue)
            previousValue = (decimal)cacheValue;

        var calculatedValue = calculationService.Calculate(inputValue, previousValue);
        keyValueStorage.SetValue(key.ToString(), calculatedValue);
        var response = new CalculationResponse
        {
            ComputedValue = calculatedValue,
            InputValue = inputValue,
            PreviousValue = previousValue
        };
        calculationService.SendMessage(response);

        return Ok(response);
    }
}
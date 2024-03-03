using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Webapi.Dto.Request;

public class CalculationRequest
{
    [Range(0.01, 100.00, ErrorMessage = "Value must be in the range of 0.01 and 100.00.")]
    [DefaultValue(54.99)]
    [Required(ErrorMessage = "Input is required")]
    public decimal input { get; set; }

}
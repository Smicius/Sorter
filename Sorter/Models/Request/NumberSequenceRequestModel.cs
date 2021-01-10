using System.ComponentModel.DataAnnotations;

namespace Sorter.Models.Request
{
  public class NumberSequenceRequestModel
  {
    [Required(ErrorMessage = "Numbers must be specified.")]
    public int[] Numbers { get; set; }
  }
}

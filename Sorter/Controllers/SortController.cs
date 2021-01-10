using Microsoft.AspNetCore.Mvc;
using Sorter.Models.Request;
using Sorter.Models.Response;
using Sorter.Services;

namespace Sorter.Controllers
{
  public class SortController : Controller
  {
    private readonly ISortService SortService;

    public SortController(ISortService sortService)
    {
      SortService = sortService;
    }

    [HttpPost]
    public IActionResult Provide([FromBody] NumberSequenceRequestModel numberSequence)
    {
      SortService.SaveSorted(numberSequence.Numbers);
      return Ok();
    }

    [HttpGet]
    public IActionResult Latest()
    {
      var sortedNumberSequence = SortService.LoadLatest();
      return Ok(new NumberSequenceResponseModel { Numbers = sortedNumberSequence });
    }
  }
}

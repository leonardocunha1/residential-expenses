using Microsoft.AspNetCore.Mvc;
using ResidentialExpenses.Communication.Responses;

namespace ResidentialExpenses.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResidentialExpensesBaseController : ControllerBase
{
	protected IActionResult SuccessOk<T>(T data, ResponseMetadataJson? metadata = null)
	{
		return Ok(new ResponseApiJson<T>
		{
			Data = data,
			Metadata = metadata ?? new ResponseMetadataJson()
		});
	}

	protected IActionResult SuccessCreated<T>(T data, string? location = null, ResponseMetadataJson? metadata = null)
	{
		return Created(location ?? string.Empty, new ResponseApiJson<T>
		{
			Data = data,
			Metadata = metadata ?? new ResponseMetadataJson()
		});
	}
}

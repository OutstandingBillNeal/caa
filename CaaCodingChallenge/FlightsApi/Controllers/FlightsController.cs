using FlightsData.Models;
using FluentValidation;
using FluentValidation.Results;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using UnitsOfWork;
using static System.Runtime.InteropServices.JavaScript.JSType;

/* 
A note on validation
====================
Jeremy Skinner - the author of FluentValidation - writes
(https://github.com/FluentValidation/FluentValidation/issues/1960)
"We recommend using a manual validation approach with ASP.NET".

In my own experience, we used to be able to have validation run 
as part of the pipeline.  My attempts to make this work in .NET 
Core have been unsuccessful so far.  I'm now adopting Skinner's
reccommendation.
*/

namespace FlightsApi.Controllers;

[Authorize]
[Route("api/[controller]")]
[ApiController]
public class FlightsController(
        IMediator mediator
        , IValidator<CreateFlightRequest> createFlightValidator
        , IValidator<UpdateFlightRequest> updateFlightValidator
    ) : ControllerBase
{
    private readonly IMediator _mediator = mediator;
    private readonly IValidator<CreateFlightRequest> _createFlightValidator = createFlightValidator;
    private readonly IValidator<UpdateFlightRequest> _updateFlightValidator = updateFlightValidator;

    // GET: api/<FlightsController>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Flight>>> Get()
    {
        var request = new GetFlightsRequest();
        var result = await _mediator.Send(request, CancellationToken.None);
        return new JsonResult(result);
    }

    // GET api/<FlightsController>/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Flight>> Get(int id)
    {
        var request = new GetFlightByIdRequest { Id = id };
        var result = await _mediator.Send(request, CancellationToken.None);

        return result == null
            ? new NotFoundResult()
            : new JsonResult(result);
    }

    // POST api/<FlightsController>
    [HttpPost]
    public async Task<ActionResult<Flight>> Post([FromBody] Flight value)
    {
        var request = new CreateFlightRequest { Flight = value };
        var validationResult = await _createFlightValidator.ValidateAsync(request);

        if (validationResult != null && !validationResult.IsValid)
        {
            var message = GetValidationMessage(validationResult);
            return BadRequest(message);
        }
        
        var result = await _mediator.Send(request, CancellationToken.None);
        return new JsonResult(result);
    }

    // PUT api/<FlightsController>/5
    [HttpPut("{id}")]
    public async Task<ActionResult<Flight>> Put(int id, [FromBody] Flight value)
    {
        var request = new UpdateFlightRequest { Flight = value };
        request.Flight.Id = id; // Just to be sure
        var validationResult = await _updateFlightValidator.ValidateAsync(request);

        if (validationResult != null && !validationResult.IsValid)
        {
            var message = GetValidationMessage(validationResult);
            return BadRequest(message);
        }

        var result = await _mediator.Send(request, CancellationToken.None);
        return result == null
            ? NotFound()
            : new JsonResult(result);
    }

    // DELETE api/<FlightsController>/5
    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var request = new DeleteFlightRequest { Id = id };
        var result = await _mediator.Send(request);

        return result.Success
            ? Ok(result)
            : NotFound();
    }

    private static string GetValidationMessage(ValidationResult validationResult)
    {
        var errorMessages = validationResult
            .Errors
            .Select(err => $"Property: {err.PropertyName}, error: {err.ErrorMessage}.");
        return string.Join(' ', errorMessages);
    }
}

using Backend.Application.Dtos.Review;
using Backend.Application.UseCases.Reviews.CreateReview;
using Backend.Application.UseCases.Reviews.UpdateReview;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.Controllers;

[Route("api/[controller]")]
public class ReviewController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromForm] CreateReviewCommand command)
    {
        try
        {
            var result = await mediator.Send(command);
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid id, [FromForm] UpdateReviewDto dto)
    {
        try
        {
            var result = await mediator.Send(new UpdateReviewCommand() { Id = id, ReviewDto = dto });
            return Ok(result);
        }
        catch (Exception ex)
        {
            return BadRequest(ex.Message);
        }
    }
}
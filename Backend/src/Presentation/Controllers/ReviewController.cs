using Backend.Application.Dtos.Review;
using Backend.Application.UseCases.Reviews.CreateReview;
using Backend.Application.UseCases.Reviews.DeleteReview;
using Backend.Application.UseCases.Reviews.GetAllReviews;
using Backend.Application.UseCases.Reviews.GetReview;
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
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid id)
    {
        try
        {
            var result = await mediator.Send(new DeleteReviewCommand() { Id = id });
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid id)
    {
        try
        {
            var result = await mediator.Send(new GetReviewQuery(id));
            return Ok(result);
        }
        catch (Exception e)
        {
            return NotFound(e.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync()
    {
        try
        {
            var result = await mediator.Send(new GetAllReviewsQuery());
            return Ok(result);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}
using Backend.Application.Dtos.Review;
using Backend.Application.Exceptions;
using Backend.Application.UseCases.Reviews.CreateReview;
using Backend.Application.UseCases.Reviews.DeleteReview;
using Backend.Application.UseCases.Reviews.GetAllReviews;
using Backend.Application.UseCases.Reviews.GetReview;
using Backend.Application.UseCases.Reviews.UpdateReview;
using Backend.Presentation.Hypermedia;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Presentation.Controllers;

[ApiController]
[Route("api/shops/{shopId:guid}/products/{productId:guid}/reviews")]
public class ReviewController(IMediator mediator) : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromRoute] Guid shopId, [FromRoute] Guid productId,
        [FromForm] CreateReviewDto dto)
    {
        try
        {
            var result = await mediator.Send(new CreateReviewCommand
            {
                Title = dto.Title,
                Content = dto.Content,
                Rating = dto.Rating,
                Author = dto.Author,
                ShopId = shopId,
                ProductId = productId
            });
            result.Links = LinkFactory.ForReview(shopId, result.ProductId, result.Id);
            return Created($"/api/shops/{shopId}/products/{result.ProductId}/reviews/{result.Id}", result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
    }
    
    [HttpPut("{reviewId:guid}")]
    public async Task<IActionResult> UpdateAsync([FromRoute] Guid shopId, [FromRoute] Guid productId,
        [FromRoute] Guid reviewId, [FromForm] UpdateReviewDto dto)
    {
        try
        {
            var result = await mediator.Send(new UpdateReviewCommand()
            {
                ShopId = shopId,
                ProductId = productId,
                Id = reviewId,
                ReviewDto = dto
            });
            result.Links = LinkFactory.ForReview(shopId, result.ProductId, result.Id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (ValidationException ex)
        {
            return UnprocessableEntity(ex.Message);
        }
    }
    
    [HttpDelete("{reviewId:guid}")]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid shopId, [FromRoute] Guid productId,
        [FromRoute] Guid reviewId)
    {
        try
        {
            await mediator.Send(new DeleteReviewCommand()
            {
                ShopId = shopId,
                ProductId = productId,
                Id = reviewId
            });
            return NoContent();
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
    
    [HttpGet("{reviewId:guid}")]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid shopId, [FromRoute] Guid productId,
        [FromRoute] Guid reviewId)
    {
        try
        {
            var result = await mediator.Send(new GetReviewQuery(shopId, productId, reviewId));
            result.Links = LinkFactory.ForReview(shopId, result.ProductId, result.Id);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet]
    public async Task<IActionResult> GetAllAsync(
        [FromRoute] Guid shopId,
        [FromRoute] Guid productId,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10,
        [FromQuery] int? minRating = null)
    {
        if (page < 1 || pageSize < 1 || pageSize > 50)
        {
            return BadRequest("page must be >= 1 and pageSize must be between 1 and 50.");
        }

        if (minRating < 1 || minRating > 5)
        {
            return BadRequest("minRating must be between 1 and 5.");
        }

        try
        {
            var result = await mediator.Send(new GetAllReviewsQuery
            {
                ShopId = shopId,
                ProductId = productId,
                Page = page,
                PageSize = pageSize,
                MinRating = minRating
            });
            foreach (var item in result.Items)
            {
                item.Links = LinkFactory.ForReview(shopId, item.ProductId, item.Id);
            }

            result.Links = LinkFactory.ForPage(Request, result.Page, result.TotalPages);
            return Ok(result);
        }
        catch (NotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}
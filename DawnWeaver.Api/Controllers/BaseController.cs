using DawnWeaver.Domain.Common;
using DawnWeaver.Domain.Errors;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace DawnWeaver.Api.Controllers;

[ApiController]
public class BaseController : ControllerBase
{
    private IMediator _mediator;
    
    protected IMediator Mediator => _mediator ??= HttpContext.RequestServices.GetRequiredService<IMediator>();
    
    // Dodać żeby nie rzucało tylko BadRequest tylko różne kody błędów jak 404 itp.
    
    protected IActionResult HandleFailure(Result result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            _ =>
                BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Error))
        };
    
    protected ActionResult<T> HandleFailure<T>(ResultT<T> result) =>
        result switch
        {
            { IsSuccess: true } => throw new InvalidOperationException(),
            _ =>
                BadRequest(
                    CreateProblemDetails(
                        "Bad Request",
                        StatusCodes.Status400BadRequest,
                        result.Error))
        };
    
    private static ProblemDetails CreateProblemDetails(
        string title,
        int status,
        Error error,
        Error[]? errors = null) =>
        new()
        {
            Title = title,
            Type = error.Code,
            Detail = error.Description,
            Status = status,
            Extensions = { { nameof(errors), errors } }
        };
}
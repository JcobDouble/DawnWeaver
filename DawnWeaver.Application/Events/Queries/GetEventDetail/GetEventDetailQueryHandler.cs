using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Application.Common.Mapping;
using DawnWeaver.Domain.Common;
using DawnWeaver.Domain.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace DawnWeaver.Application.Events.Queries.GetEventDetail;

public class GetEventDetailQueryHandler(IAppDbContext context) : IRequestHandler<GetEventDetailQuery, ResultT<EventDetailViewModel>>
{
    public async Task<ResultT<EventDetailViewModel>> Handle(GetEventDetailQuery request, CancellationToken cancellationToken)
    {
        var eventInDb = await context.Events.FirstOrDefaultAsync(e => e.Id == request.EventId, cancellationToken);
        
        if (eventInDb == null)
        {
            return Result.Failure<EventDetailViewModel>(EventErrors.EventNotFound);
        }

        var eventDetail = eventInDb.MapToEventDetailViewModel();

        return Result.Success(eventDetail);
    }
}
using DawnWeaver.Application.Services.TimeManagement;
using DawnWeaver.Domain.Common;
using MediatR;

namespace DawnWeaver.Application.TimeManagement.Queries;

public class GetTimeManagementProposalQueryHandler(TimeProposalService timeProposalService)
    : IRequestHandler<GetTimeManagementProposalQuery, ResultT<List<DateTime>>>
{
    public async Task<ResultT<List<DateTime>>> Handle(GetTimeManagementProposalQuery request, CancellationToken cancellationToken)
    {
        var result = await timeProposalService.GetTimeProposals(request.DurationInMinutes, cancellationToken);
        
        return Result.Success(result);
    }
}
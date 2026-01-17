using DawnWeaver.Domain.Common;
using MediatR;

namespace DawnWeaver.Application.TimeManagement.Queries;

public class GetTimeManagementProposalQuery : IRequest<ResultT<List<DateTime>>>
{
    public int DurationInMinutes { get; set; }
}
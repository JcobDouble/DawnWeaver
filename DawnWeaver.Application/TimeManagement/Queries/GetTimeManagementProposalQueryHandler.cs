using DawnWeaver.Application.Common.Interfaces;
using DawnWeaver.Application.Services;
using DawnWeaver.Domain.Common;
using MediatR;

namespace DawnWeaver.Application.TimeManagement.Queries;

public class GetTimeManagementProposalQueryHandler(IDateTime dateTime, GetAllEventsService service)
    : IRequestHandler<GetTimeManagementProposalQuery, ResultT<List<DateTime>>>
{
    public async Task<ResultT<List<DateTime>>> Handle(GetTimeManagementProposalQuery request, CancellationToken cancellationToken)
    {
        List<DateTime> suitableFreeTimes = new List<DateTime>();
        
        var j = 0;
        
        do
        {
            var eventsDuringTheDay = await service.GetAllEvents(dateTime.Today.AddDays(j), dateTime.Today.AddDays(j).AddHours(23).AddMinutes(59).AddSeconds(59), cancellationToken);

            List<DateTime[]> eventsDates = new();

            foreach (var oneOfEvents in eventsDuringTheDay)
            {
                var startDate = oneOfEvents.StartDate;
                var endDate = oneOfEvents.EndDate;

                eventsDates.Add(new DateTime[] { startDate, endDate });
            }

            if (!eventsDates.Any())
            {
                suitableFreeTimes.Add(dateTime.Today.AddDays(j).AddHours(12));
                continue;
            }

// zmerguj nachodzące interwały. Zrobić z tego metodę

            eventsDates.Sort((a, b) => a[0].CompareTo(b[0]));

            List<DateTime[]> result = new List<DateTime[]>();

            result.Add(new DateTime[] { eventsDates[0][0], eventsDates[0][1] });

            for (int i = 1; i < eventsDates.Count; i++)
            {
                DateTime[] last = result[result.Count - 1];
                DateTime[] current = eventsDates[i];
                
                if (last[1] >= current[0])
                {
                    last[1] = last[1] > current[1] ? last[1] : current[1];
                }
                else
                {
                    result.Add(new DateTime[] { current[0], current[1] });
                }
            }
            
            List<DateTime[]> freeTimes = new List<DateTime[]>();
            
            var currentTime = dateTime.Today.AddDays(j);
            
            for (int i = 0; i < result.Count; i++)
            {
                freeTimes.Add(new DateTime[] { currentTime, result[i][0] });
                
                currentTime = result[i][1];
                
                if(currentTime == dateTime.Today.AddDays(j).AddHours(23).AddMinutes(59).AddSeconds(59))
                {
                    break;
                }
                
                if(i == result.Count - 1)
                {
                    freeTimes.Add(new DateTime[] { currentTime, dateTime.Today.AddDays(j).AddHours(23).AddMinutes(59).AddSeconds(59) });
                }
            }

            
            for (int i = 0; i < freeTimes.Count; i++)
            {
                var freeTimeRange = (freeTimes[i][1] - freeTimes[i][0]).TotalMinutes;
                if (freeTimeRange >= request.DurationInMinutes)
                {
                    suitableFreeTimes.Add(freeTimes[i][0]);
                }

                if (suitableFreeTimes.Count == 3)
                {
                    break;
                }
            }
            
            j++;
        } while (suitableFreeTimes.Count < 3 && j < 90);

        return suitableFreeTimes;
    }
}
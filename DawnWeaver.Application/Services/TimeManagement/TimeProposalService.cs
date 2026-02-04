using DawnWeaver.Application.Common.Interfaces;

namespace DawnWeaver.Application.Services.TimeManagement;

public class TimeProposalService(IDateTime dateTime, GetAllEventsService service)
{
    public async Task<List<DateTime>> GetTimeProposals(int eventDurationInMinutes, CancellationToken cancellationToken)
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
                j++;
                continue;
            }
        
            var result = Normalize(eventsDates);
            
            var freeTimes = Calculate(result, j);
            
            Find(freeTimes, suitableFreeTimes, eventDurationInMinutes);
        
            j++;
        } while (suitableFreeTimes.Count < 3 && j < 90);

        return suitableFreeTimes;
    }
    
    // Merguje nachodzące na siebie daty [startDate, endDate]
    private List<DateTime[]> Normalize(List<DateTime[]> eventsDates)
    {
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
        return result;
    }
    
    // Wylicza wolne sloty na podstawie zajętych przedziałów [startFreeDate, endFreeDate]
    private List<DateTime[]> Calculate(List<DateTime[]> result, int iterationAdder)
    {
        List<DateTime[]> freeTimes = new List<DateTime[]>();
            
        var currentTime = dateTime.Today.AddDays(iterationAdder);
            
        for (int i = 0; i < result.Count; i++)
        {
            freeTimes.Add(new DateTime[] { currentTime, result[i][0] });
                
            currentTime = result[i][1];
                
            if(currentTime == dateTime.Today.AddDays(iterationAdder).AddHours(23).AddMinutes(59).AddSeconds(59))
            {
                break;
            }
                
            if(i == result.Count - 1)
            {
                freeTimes.Add(new DateTime[] { currentTime, dateTime.Today.AddDays(iterationAdder).AddHours(23).AddMinutes(59).AddSeconds(59) });
            }
        }
        
        return freeTimes;
    }
    
    // Znajdź wolne sloty które pomieszczą wydarzenie
    private void Find(List<DateTime[]> freeTimes, List<DateTime> suitableFreeTimes, int eventDurationInMinutes)
    {
        for (int i = 0; i < freeTimes.Count; i++)
        {
            var freeTimeRange = (freeTimes[i][1] - freeTimes[i][0]).TotalMinutes;
            if (freeTimeRange >= eventDurationInMinutes)
            {
                suitableFreeTimes.Add(freeTimes[i][0]);
            }

            if (suitableFreeTimes.Count == 3)
            {
                break;
            }
        }
    }
}
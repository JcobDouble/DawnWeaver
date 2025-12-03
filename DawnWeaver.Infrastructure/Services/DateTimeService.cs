using DawnWeaver.Application.Common.Interfaces;

namespace DawnWeaver.Infrastructure.Services;

public class DateTimeService : IDateTime
{
    public DateTime Now => DateTime.Now;
}
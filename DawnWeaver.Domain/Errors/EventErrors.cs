namespace DawnWeaver.Domain.Errors;

public static class EventErrors
{
    public static readonly Error EventNotFound = new Error("Event.NotFound", "The specified event was not found.");
}
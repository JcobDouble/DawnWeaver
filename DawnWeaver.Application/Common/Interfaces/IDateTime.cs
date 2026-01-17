namespace DawnWeaver.Application.Common.Interfaces;

public interface IDateTime
{
    DateTime Now { get; }
    
    DateTime Today { get; }
}
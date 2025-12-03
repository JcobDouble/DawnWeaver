using DawnWeaver.Domain.Common;

namespace DawnWeaver.Domain.Entities;

public class User : AuditableEntity
{
    public required string Nickname { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public string CoverImageUrl { get; set; } = string.Empty;
}

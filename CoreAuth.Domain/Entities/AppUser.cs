using CoreAuth.Domain.Common.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace CoreAuth.Domain.Entities;

public class AppUser : IdentityUser, IAuditableEntity, ISoftDeleteEntity
{
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedDate { get; set; }
    public DateTime? UpdatedDate { get; set; }

    public bool IsDeleted { get; set; }
    public DateTime? DeletedDate { get; set; }

    public DateTime? LastLoginDate { get; set; }
}
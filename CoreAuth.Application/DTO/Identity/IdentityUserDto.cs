namespace CoreAuth.Application.DTO.Identity;

public record IdentityUserDto(string Id, string UserName, string Email, bool IsActive, bool IsDeleted, bool EmailConfirmed);
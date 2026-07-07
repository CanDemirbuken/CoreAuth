namespace CoreAuth.Domain.Common.Interfaces;

public interface ISoftDeleteEntity
{
    bool IsDeleted { get; set; }
    DateTime? DeletedDate { get; set; }
}
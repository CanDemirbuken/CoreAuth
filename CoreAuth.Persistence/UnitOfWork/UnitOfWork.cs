using CoreAuth.Application.Interfaces.Repositories;
using CoreAuth.Persistence.Context;

namespace CoreAuth.Persistence.Repositories;

public class UnitOfWork(AppDbContext context) : IUnitOfWork
{
    public async Task<int> SaveChangesAsync()
    {
        return await context.SaveChangesAsync();
    }
}
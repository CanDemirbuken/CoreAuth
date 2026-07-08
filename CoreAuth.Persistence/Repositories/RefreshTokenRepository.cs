using CoreAuth.Application.Interfaces.Repositories;
using CoreAuth.Domain.Entities;
using CoreAuth.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace CoreAuth.Persistence.Repositories;

public class RefreshTokenRepository(AppDbContext context) : IRefreshTokenRepository
{
    public async Task AddAsync(RefreshToken refreshToken)
    {
        await context.RefreshTokens.AddAsync(refreshToken);
    }

    public async Task<RefreshToken?> GetByTokenAsync(string token)
    {
        return await context.RefreshTokens.FirstOrDefaultAsync(t => t.Token == token);
    }
}
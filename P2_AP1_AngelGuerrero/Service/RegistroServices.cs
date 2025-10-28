using Microsoft.EntityFrameworkCore;
using P2_AP1_AngelGuerrero.DAL;
using P2_AP1_AngelGuerrero.Models;
using System.Linq.Expressions;

namespace P2_AP1_AngelGuerrero.Service;

public class RegistroServices(IDbContextFactory<Contexto> DbFactory) 
{
    public async Task<List<Generico>>Listar(Expression<Func<Generico, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Generico.Where(criterio).AsNoTracking().ToListAsync();
    }
}

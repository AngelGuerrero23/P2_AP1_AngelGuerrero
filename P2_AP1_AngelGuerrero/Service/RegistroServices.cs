using Microsoft.EntityFrameworkCore;
using P2_AP1_AngelGuerrero.DAL;
using P2_AP1_AngelGuerrero.Models;
using System.Linq.Expressions;

namespace P2_AP1_AngelGuerrero.Service;

public class RegistroServices(IDbContextFactory<Contexto> DbFactory) 
{
    public async Task<List<Pedidos>>Listar(Expression<Func<Pedidos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedidos.Where(criterio).AsNoTracking().ToListAsync();
    }
}

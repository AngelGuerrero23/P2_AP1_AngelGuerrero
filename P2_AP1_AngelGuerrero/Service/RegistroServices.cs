using Microsoft.EntityFrameworkCore;
using P2_AP1_AngelGuerrero.DAL;
using P2_AP1_AngelGuerrero.Models;
using System.Linq.Expressions;

namespace P2_AP1_AngelGuerrero.Service;

public class RegistroServices(IDbContextFactory<Contexto> DbFactory)
{

    public async Task<bool> Guardar(Pedidos pedidos)
    {
        if (!await Existe(pedidos.PedidoId))
        {
            return await Insertar(pedidos);
        }
        else
        {
            return await Modificar(pedidos);
        }
    }
    public async Task<bool> Insertar(Pedidos pedido)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Pedidos.Add(pedido);
        await AfectarExistencia(pedido.GetPedidosDetalles.ToArray(), TipoOperacion.Suma);
        return await contexto.SaveChangesAsync() > 0;

    }
    private async Task<bool> Modificar(Pedidos pedido)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        contexto.Pedidos.Update(pedido);
        var pedidoAnterior = await contexto.Pedidos
            .Include(p => p.GetPedidosDetalles)
            .AsNoTracking()
            .SingleOrDefaultAsync(c => c.PedidoId == pedido.PedidoId);

        if (pedidoAnterior == null) return false;

        await AfectarExistencia(pedidoAnterior.GetPedidosDetalles.ToArray(), TipoOperacion.Resta);

        contexto.PedidosDetalle.RemoveRange(pedidoAnterior.GetPedidosDetalles);
        contexto.Update(pedido);

        await AfectarExistencia(pedido.GetPedidosDetalles.ToArray(), TipoOperacion.Suma);

        return await contexto.SaveChangesAsync() > 0;
    }
    public async Task<bool> Eliminar(int Id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        var pedidoAnterior = await contexto.Pedidos
            .Include(p => p.GetPedidosDetalles)
            .FirstOrDefaultAsync(c => c.PedidoId == Id);

        if (pedidoAnterior == null) return false;

        await AfectarExistencia(pedidoAnterior.GetPedidosDetalles.ToArray(), TipoOperacion.Resta);

        contexto.PedidosDetalle.RemoveRange(pedidoAnterior.GetPedidosDetalles);
        contexto.Pedidos.Remove(pedidoAnterior);
        var cantidad = await contexto.SaveChangesAsync();
        return cantidad > 0;
    }

    public async Task<bool> Existe(int Id)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.PedidosDetalle
            .AnyAsync(c => c.PedidoId == Id);
    }
    public async Task<List<Pedidos>> Listar(Expression<Func<Pedidos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedidos.Where(criterio).AsNoTracking().ToListAsync();
    }

    private async Task AfectarExistencia(PedidosDetalle[] Detalles, TipoOperacion tipoOperacion)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        foreach (var detalle in Detalles)
        {
            var pedido = await contexto.Componentes.FindAsync(detalle.Cantidad);
            if (tipoOperacion == TipoOperacion.Resta)
                pedido.Existencia -= detalle.Cantidad;
            else
                pedido.Existencia += detalle.Cantidad;
        }
        await contexto.SaveChangesAsync();
    }

    public async Task<PedidosDetalle[]> ListarDetalles(Expression<Func<Pedidos, bool>> criterio)
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Pedidos
            .Where(criterio)
            .Select(h => new PedidosDetalle
            {
                PedidoId = h.PedidoId,
            }).ToArrayAsync();
    }

    public async Task<Componente[]> ListarTipo()
    {
        await using var contexto = await DbFactory.CreateDbContextAsync();
        return await contexto.Componentes
            .Where(h => h.ComponenteId > 0).Select(h => new Componente
            {
                Descripcion = h.Descripcion,
                Existencia = h.Existencia,
                ComponenteId = h.ComponenteId
            }).ToArrayAsync();
    }
    public enum TipoOperacion
    {
        Suma = 1,
        Resta = 2
    }
        
}

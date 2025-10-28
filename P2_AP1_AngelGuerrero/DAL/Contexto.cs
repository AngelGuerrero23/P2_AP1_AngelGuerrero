using Microsoft.EntityFrameworkCore;
using P2_AP1_AngelGuerrero.Models;
using P2_AP1_AngelGuerrero.Service;

namespace P2_AP1_AngelGuerrero.DAL;

public class Contexto: DbContext
{
    public Contexto (DbContextOptions<Contexto>options) : base(options)
    {

    }
    public DbSet<Generico> Generico { get; set; }
}

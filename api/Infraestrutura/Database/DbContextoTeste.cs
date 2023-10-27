using Microsoft.EntityFrameworkCore;
using MinimalApiDesafio;


namespace minimal_api_desafio.Infraestrutura.Database;

public class DbContextoTeste : DbContext
{        
    public DbContextoTeste(){}

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) =>
        optionsBuilder.UseSqlServer(Startup.Configuration?.GetConnectionString("ConexaoSqlServer"));
       
}
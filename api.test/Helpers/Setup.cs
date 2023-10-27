using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualBasic;
using minimal_api_desafio.Infraestrutura.Database;
using MinimalApiDesafio;

namespace Api.Test.Helpers;

public class Setup
{
    public const string PORT = "5001";
    public static TestContext testContext = default!;
    public static WebApplicationFactory<Startup> http = default!;
    public static HttpClient client = default!;

    public static void ExecutaComandoSql(string sql)
    {
        new DbContextoTeste().Database.ExecuteSqlRaw(sql);
    }

     public static void FakeCliente()
    {
        new DbContextoTeste().Database.ExecuteSqlRaw("""
        insert clientes(Nome, Telefone, Email, DataCriacao) values ('Victor','(11)1111-1111','teste@email.com','2022-12-15 06:09:00')
        """);
    }
    public static void ClassInit(TestContext testContext)
    {
        Setup.testContext = testContext;
        Setup.http = new WebApplicationFactory<Startup>();

        Setup.http = Setup.http.WithWebHostBuilder(builder =>
        {
            builder.UseSetting("https_port", Setup.PORT).UseEnvironment("Testing");
        });

        Setup.client = Setup.http.CreateClient();
    }

    public static void ClassCleanup()
    {
        Setup.http.Dispose();
    }
}


using Microsoft.AspNetCore.Mvc;
using MinimalApiDesafio.DTOs;
using MinimalApiDesafio.Models;
using MinimalApiDesafio.ModelViews;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

MapRoutes(app);
MapRoutesClientes(app);
app.Run();


#region Rotas utilizando Minimal API

void MapRoutes(WebApplication app)
{
    app.MapGet("/", () => new { Mensagem = "Bem vindo a API" })
     .Produces<dynamic>(StatusCodes.Status200OK)
     .WithName("Home")
     .WithTags("Testes");

    app.MapGet("/recebe-parametro", (HttpRequest request, HttpResponse response, string? nome) =>
     {

         if (string.IsNullOrEmpty(nome))
         {
             return Results.BadRequest(new
             {
                 Mensagem = "Olha você não mandou uma informação importante, o nome é obrigatorio"
             });
         }

         nome = $"""
        Alterando parametro recebido {nome}
        """;
         var objetoDeRetorno = new
         {
             ParametroPassado = nome,
             Mensagem = "Passando parametro por querystring"
         };

         return Results.Created($"/recebe-parametro/{objetoDeRetorno.ParametroPassado}", objetoDeRetorno);
     })
     .Produces<dynamic>(StatusCodes.Status201Created)
     .Produces(StatusCodes.Status400BadRequest)
     .WithName("TesteRecebeParametro")
     .WithTags("Testes");
}

void MapRoutesClientes(WebApplication app)
{
    app.MapGet("/clientes", () =>
     {
         var clientes = new List<Cliente>();
         return Results.Ok(clientes);
     })
     .Produces<dynamic>(StatusCodes.Status200OK)
     .WithName("GetCliente")
     .WithTags("Clientes");
     
    app.MapPost("/clientes", ([FromBody] ClienteDTO clienteDto) =>
    {
        Cliente cliente = new Cliente
        {
            Nome = clienteDto.Nome,
            Telefone = clienteDto.Telefone,
            Email = clienteDto.Email
        };


        return Results.Created($"/cliente/{cliente.Id}", cliente);
    })
    .Produces<Cliente>(StatusCodes.Status201Created)
    .Produces<Error>(StatusCodes.Status400BadRequest)
    .WithName("PostClientes")
    .WithTags("Clientes");

    app.MapPut("/clientes/{id}", ([FromRoute] int id, [FromBody] ClienteDTO clienteDto) =>
    {
        if (string.IsNullOrEmpty(clienteDto.Nome))
        {
            return Results.BadRequest(new Error
            {
                Codigo = 1213,
                Mensagem = "Você passou um cliente inexistente"
            });

        }
        /*
            var cliente = ClienteServico.BuscaPorId(id);
            
            if(cliente == null)
                return Results.NotFound(new Error{Codigo = 1213, Mensagem = "Você passou um cliente inexistente"});
            
            cliente.Nome = clienteDto.Nome.
            cliente.Telefone = clienteDto.Telefone,
            cliente.Email = clienteDto.Email

            ClienteServico.Update(cliente);
        */
        Cliente cliente = new Cliente
        {
            Nome = clienteDto.Nome,
            Telefone = clienteDto.Telefone,
            Email = clienteDto.Email
        };

        return Results.Ok(cliente);
    })
    .Produces<Cliente>(StatusCodes.Status200OK)
    .Produces<Error>(StatusCodes.Status404NotFound)
    .Produces<Error>(StatusCodes.Status400BadRequest)
    .WithName("PutClientes")
    .WithTags("Clientes");

    
}

#endregion


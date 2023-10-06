using Microsoft.EntityFrameworkCore;
using minimal_api_desafio.Infraestrutura.Database;
using MinimalApiDesafio.Infraestrutura.Interfaces;

namespace MinimalApiDesafio.Models;

public class ClientesServico : IBancoDeDadosServicos<Cliente>
{
    public ClientesServico(DbContexto dbContexto)
    {
        _dbContexto = dbContexto;
    }

    private DbContexto _dbContexto;

    public async Task<Cliente> BuscaPorId(int id) =>
     await _dbContexto.Clientes.Where(c => c.Id == id).FirstAsync();

    public async Task Salvar(Cliente cliente)
    {              
        if (cliente.Id == 0)
            await _dbContexto.AddAsync(cliente);
        else
           _dbContexto.Clientes.Update(cliente);

        await _dbContexto.SaveChangesAsync();        
    }

    public async Task ExcluirPorId(int id)
    {
        var cliente = await _dbContexto.Clientes.Where(c => c.Id == id).FirstAsync();
        if(cliente is not null)
        {
           await Excluir(cliente);
        }
    }

    public async Task<List<Cliente>> Todos() =>
        await _dbContexto.Clientes.AsNoTracking().ToListAsync();

    public async Task Excluir(Cliente cliente)
    {
       _dbContexto.Clientes.Remove(cliente);
       await _dbContexto.SaveChangesAsync();       
    }
}
namespace MinimalApiDesafio.Infraestrutura.Interfaces; 

public interface IBancoDeDadosServicos<T>
{
    Task Salvar(T objeto);
    Task Excluir(T objeto);
    Task ExcluirPorId(int id);
    Task<T> BuscaPorId(int id);
    Task<List<T>> Todos();
}
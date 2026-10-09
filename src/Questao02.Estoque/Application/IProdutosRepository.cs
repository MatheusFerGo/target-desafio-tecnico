using Questao02.Estoque.Domain;

namespace Questao02.Estoque.Application;

public interface IProdutosRepository
{
    IReadOnlyList<Produto> ObterTodos();
}

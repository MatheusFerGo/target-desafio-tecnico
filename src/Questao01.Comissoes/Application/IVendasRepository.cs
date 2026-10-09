using Questao01.Comissoes.Domain;

namespace Questao01.Comissoes.Application;

public interface IVendasRepository
{
    IReadOnlyList<Venda> ObterTodas();
}

using Questao01.Comissoes.Domain;

namespace Questao01.Comissoes.Application;

public class ApuradorDeComissoes
{
    private readonly CalculadoraDeComissao _calculadora;

    public ApuradorDeComissoes(CalculadoraDeComissao calculadora)
    {
        ArgumentNullException.ThrowIfNull(calculadora);

        _calculadora = calculadora;
    }

    public IReadOnlyList<ComissaoDoVendedor> Apurar(IEnumerable<Venda> vendas)
    {
        ArgumentNullException.ThrowIfNull(vendas);

        return vendas
            .GroupBy(venda => venda.Vendedor)
            .Select(grupo => new ComissaoDoVendedor(
                grupo.Key,
                grupo.Sum(venda => _calculadora.Calcular(venda.Valor))))
            .ToArray();
    }
}

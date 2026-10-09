using Questao01.Comissoes.Application;
using Questao01.Comissoes.Domain;
using Xunit;

namespace Questao01.Comissoes.Tests.Application;

public class ApuradorDeComissoesTests
{
    private readonly ApuradorDeComissoes _apurador = new(new CalculadoraDeComissao(FaixasDeComissao.Padrao));

    [Fact]
    public void Deve_SomarComissaoDeCadaVenda_PorVendedor()
    {
        var vendas = new[]
        {
            new Venda("João Silva", 1200.50m),
            new Venda("Maria Souza", 500.00m),
            new Venda("João Silva", 250.30m),
            new Venda("Maria Souza", 99.99m),
            new Venda("João Silva", 50.00m),
            new Venda("Maria Souza", 100.00m),
        };

        var resultado = _apurador.Apurar(vendas);

        var esperado = new[]
        {
            new ComissaoDoVendedor("João Silva", 62.53m),
            new ComissaoDoVendedor("Maria Souza", 26.00m),
        };
        Assert.Equal(esperado, resultado);
    }

    [Fact]
    public void Deve_RetornarListaVazia_QuandoNaoHouverVendas()
    {
        var resultado = _apurador.Apurar([]);

        Assert.Empty(resultado);
    }
}

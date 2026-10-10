using Questao01.Comissoes.Domain;
using Xunit;

namespace Questao01.Comissoes.Tests.Domain;

public class CalculadoraDeComissaoTests
{
    private readonly CalculadoraDeComissao _calculadora = new(FaixasDeComissao.Padrao);

    public static TheoryData<decimal, decimal> ValoresDeVendaEComissoesEsperadas => new()
    {
        { 0m, 0m },
        { 99.99m, 0m },        // abaixo de 100: sem comissão
        { 100.00m, 1.00m },    // limite inferior da faixa de 1%
        { 250.30m, 2.50m },    // 2,503 arredonda para 2,50
        { 499.99m, 5.00m },    // 4,9999 arredonda para 5,00
        { 500.00m, 25.00m },   // limite inferior da faixa de 5%
        { 1200.50m, 60.03m },  // 60,025 arredonda para cima (AwayFromZero)
    };

    [Theory]
    [MemberData(nameof(ValoresDeVendaEComissoesEsperadas))]
    public void Deve_CalcularComissaoCorreta_ParaCadaFaixaDeValor(decimal valorVenda, decimal comissaoEsperada)
    {
        var comissao = _calculadora.Calcular(valorVenda);

        Assert.Equal(comissaoEsperada, comissao);
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoValorDaVendaForNegativo()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _calculadora.Calcular(-0.01m));
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoAsFaixasNaoCobremValoresAPartirDeZero()
    {
        var faixasSemZero = new[] { new FaixaDeComissao(100m, 0.01m) };

        Assert.Throws<ArgumentException>(() => new CalculadoraDeComissao(faixasSemZero));
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoNaoHouverFaixas()
    {
        Assert.Throws<ArgumentException>(() => new CalculadoraDeComissao([]));
    }
}

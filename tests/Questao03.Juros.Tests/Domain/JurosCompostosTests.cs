using Questao03.Juros.Domain;
using Xunit;

namespace Questao03.Juros.Tests.Domain;

public class JurosCompostosTests
{
    private readonly JurosCompostos _regra = new(0.025m);

    public static TheoryData<decimal, int, decimal> ValoresDeJurosEsperados => new()
    {
        { 1000m, 0, 0m },
        { 1000m, 1, 25.00m },
        { 1000m, 2, 50.63m },    // 50,625 arredonda para cima
        { 1000m, 10, 280.08m },
        { 100m, 30, 109.76m },
        { 333.33m, 1, 8.33m },   // 8,33325 arredonda para baixo
    };

    [Theory]
    [MemberData(nameof(ValoresDeJurosEsperados))]
    public void Deve_CalcularJurosCompostos_ConformeOsDiasDeAtraso(decimal valor, int diasEmAtraso, decimal jurosEsperados)
    {
        var juros = _regra.Calcular(valor, diasEmAtraso);

        Assert.Equal(jurosEsperados, juros);
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoValorForNegativo()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _regra.Calcular(-0.01m, 1));
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoDiasEmAtrasoForemNegativos()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _regra.Calcular(100m, -1));
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoTaxaForNegativa()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => new JurosCompostos(-0.01m));
    }

    [Fact]
    public void Deve_LancarExcecaoDeIntervalo_QuandoOPeriodoForGrandeDemaisParaOCalculo()
    {
        Assert.Throws<ArgumentOutOfRangeException>(() => _regra.Calcular(1000m, 100_000));
    }
}

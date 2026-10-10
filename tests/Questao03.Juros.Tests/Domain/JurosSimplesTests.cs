using Questao03.Juros.Domain;
using Xunit;

namespace Questao03.Juros.Tests.Domain;

public class JurosSimplesTests
{
    private readonly JurosSimples _regra = new(0.025m);

    public static TheoryData<decimal, int, decimal> ValoresDeJurosEsperados => new()
    {
        { 1000m, 0, 0m },
        { 100m, 1, 2.50m },
        { 1000m, 10, 250.00m },
        { 333.33m, 1, 8.33m },   // 8,33325 arredonda para baixo
        { 200.10m, 3, 15.01m },  // 15,0075 arredonda para cima
    };

    [Theory]
    [MemberData(nameof(ValoresDeJurosEsperados))]
    public void Deve_CalcularJurosSimples_ConformeOsDiasDeAtraso(decimal valor, int diasEmAtraso, decimal jurosEsperados)
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
        Assert.Throws<ArgumentOutOfRangeException>(() => new JurosSimples(-0.01m));
    }
}

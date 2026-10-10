using Questao03.Juros.Application;
using Questao03.Juros.Domain;
using Xunit;

namespace Questao03.Juros.Tests.Application;

public class CalculadoraDeJurosTests
{
    private static readonly DateOnly Hoje = new(2026, 10, 10);

    private static CalculadoraDeJuros Criar(IRegraDeJuros regra) =>
        new(regra, new RelogioFixo(new DateTimeOffset(2026, 10, 10, 12, 0, 0, TimeSpan.Zero)));

    [Fact]
    public void Deve_CalcularJurosEValorAtualizado_QuandoVencimentoEstiverNoPassado()
    {
        var calculadora = Criar(new JurosSimples(0.025m));

        var resultado = calculadora.Calcular(1000m, new DateOnly(2026, 9, 30));

        Assert.Equal(1000m, resultado.ValorOriginal);
        Assert.Equal(10, resultado.DiasEmAtraso);
        Assert.Equal(250.00m, resultado.Juros);
        Assert.Equal(1250.00m, resultado.ValorAtualizado);
    }

    [Fact]
    public void Deve_ContarUmDiaDeAtraso_QuandoVencimentoForOntem()
    {
        var calculadora = Criar(new JurosSimples(0.025m));

        var resultado = calculadora.Calcular(100m, Hoje.AddDays(-1));

        Assert.Equal(1, resultado.DiasEmAtraso);
        Assert.Equal(2.50m, resultado.Juros);
        Assert.Equal(102.50m, resultado.ValorAtualizado);
    }

    [Fact]
    public void Deve_RetornarJurosZero_QuandoVencimentoForHoje()
    {
        var calculadora = Criar(new JurosSimples(0.025m));

        var resultado = calculadora.Calcular(1000m, Hoje);

        Assert.Equal(0, resultado.DiasEmAtraso);
        Assert.Equal(0m, resultado.Juros);
        Assert.Equal(1000m, resultado.ValorAtualizado);
    }

    [Fact]
    public void Deve_RetornarJurosZero_QuandoVencimentoEstiverNoFuturo()
    {
        var calculadora = Criar(new JurosSimples(0.025m));

        var resultado = calculadora.Calcular(1000m, Hoje.AddDays(5));

        Assert.Equal(0, resultado.DiasEmAtraso);
        Assert.Equal(0m, resultado.Juros);
        Assert.Equal(1000m, resultado.ValorAtualizado);
    }

    [Fact]
    public void Deve_UsarARegraDeJurosRecebida()
    {
        var calculadora = Criar(new JurosCompostos(0.025m));

        var resultado = calculadora.Calcular(1000m, Hoje.AddDays(-2));

        Assert.Equal(50.63m, resultado.Juros);
        Assert.Equal(1050.63m, resultado.ValorAtualizado);
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoValorForNegativo()
    {
        var calculadora = Criar(new JurosSimples(0.025m));

        Assert.Throws<ArgumentOutOfRangeException>(() => calculadora.Calcular(-0.01m, Hoje.AddDays(-1)));
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoARegraForNula()
    {
        Assert.Throws<ArgumentNullException>(() => Criar(null!));
    }

    private sealed class RelogioFixo(DateTimeOffset agora) : TimeProvider
    {
        public override DateTimeOffset GetUtcNow() => agora;

        public override TimeZoneInfo LocalTimeZone => TimeZoneInfo.Utc;
    }
}

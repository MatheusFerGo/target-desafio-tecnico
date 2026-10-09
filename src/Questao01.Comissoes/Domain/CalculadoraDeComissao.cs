namespace Questao01.Comissoes.Domain;

public class CalculadoraDeComissao
{
    private const int CasasDecimais = 2;

    private readonly IReadOnlyList<FaixaDeComissao> _faixasDaMaiorParaMenor;

    public CalculadoraDeComissao(IEnumerable<FaixaDeComissao> faixas)
    {
        ArgumentNullException.ThrowIfNull(faixas);

        _faixasDaMaiorParaMenor = faixas.OrderByDescending(faixa => faixa.ValorMinimo).ToArray();

        if (_faixasDaMaiorParaMenor.Count == 0 || _faixasDaMaiorParaMenor[^1].ValorMinimo > 0m)
        {
            throw new ArgumentException("As faixas devem cobrir valores a partir de zero.", nameof(faixas));
        }
    }

    public decimal Calcular(decimal valorVenda)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(valorVenda);

        var faixa = _faixasDaMaiorParaMenor.First(f => f.Contem(valorVenda));

        return Math.Round(valorVenda * faixa.Aliquota, CasasDecimais, MidpointRounding.AwayFromZero);
    }
}

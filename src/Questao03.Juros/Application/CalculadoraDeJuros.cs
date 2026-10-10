using Questao03.Juros.Domain;

namespace Questao03.Juros.Application;

public class CalculadoraDeJuros
{
    private readonly IRegraDeJuros _regra;
    private readonly TimeProvider _relogio;

    public CalculadoraDeJuros(IRegraDeJuros regra, TimeProvider relogio)
    {
        ArgumentNullException.ThrowIfNull(regra);
        ArgumentNullException.ThrowIfNull(relogio);

        _regra = regra;
        _relogio = relogio;
    }

    public ResultadoDoCalculo Calcular(decimal valor, DateOnly vencimento)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(valor);

        var hoje = DateOnly.FromDateTime(_relogio.GetLocalNow().DateTime);
        var diasEmAtraso = Math.Max(0, hoje.DayNumber - vencimento.DayNumber);

        var juros = _regra.Calcular(valor, diasEmAtraso);

        return new ResultadoDoCalculo(valor, diasEmAtraso, juros);
    }
}

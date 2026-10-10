namespace Questao03.Juros.Domain;

public sealed class JurosCompostos : IRegraDeJuros
{
    private const int CasasDecimais = 2;

    private readonly decimal _taxaDiaria;

    public JurosCompostos(decimal taxaDiaria)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(taxaDiaria);

        _taxaDiaria = taxaDiaria;
    }

    public decimal Calcular(decimal valor, int diasEmAtraso)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(valor);
        ArgumentOutOfRangeException.ThrowIfNegative(diasEmAtraso);

        try
        {
            var fatorAcumulado = Elevar(1m + _taxaDiaria, diasEmAtraso);
            var juros = valor * (fatorAcumulado - 1m);

            return Math.Round(juros, CasasDecimais, MidpointRounding.AwayFromZero);
        }
        catch (OverflowException)
        {
            throw new ArgumentOutOfRangeException(
                nameof(diasEmAtraso),
                diasEmAtraso,
                "O período é grande demais para calcular juros compostos.");
        }
    }

    private static decimal Elevar(decimal baseDaPotencia, int expoente)
    {
        var resultado = 1m;

        while (expoente > 0)
        {
            if ((expoente & 1) == 1)
            {
                resultado *= baseDaPotencia;
            }

            expoente >>= 1;

            if (expoente > 0)
            {
                baseDaPotencia *= baseDaPotencia;
            }
        }

        return resultado;
    }
}

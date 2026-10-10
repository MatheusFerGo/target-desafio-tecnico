namespace Questao03.Juros.Domain;

public sealed class JurosSimples : IRegraDeJuros
{
    private const int CasasDecimais = 2;

    private readonly decimal _taxaDiaria;

    public JurosSimples(decimal taxaDiaria)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(taxaDiaria);

        _taxaDiaria = taxaDiaria;
    }

    public decimal Calcular(decimal valor, int diasEmAtraso)
    {
        ArgumentOutOfRangeException.ThrowIfNegative(valor);
        ArgumentOutOfRangeException.ThrowIfNegative(diasEmAtraso);

        return Math.Round(valor * _taxaDiaria * diasEmAtraso, CasasDecimais, MidpointRounding.AwayFromZero);
    }
}

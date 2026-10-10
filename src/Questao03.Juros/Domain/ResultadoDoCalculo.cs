namespace Questao03.Juros.Domain;

public sealed record ResultadoDoCalculo(decimal ValorOriginal, int DiasEmAtraso, decimal Juros)
{
    public decimal ValorAtualizado => ValorOriginal + Juros;
}

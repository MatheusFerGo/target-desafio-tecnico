namespace Questao03.Juros.Domain;

public interface IRegraDeJuros
{
    decimal Calcular(decimal valor, int diasEmAtraso);
}

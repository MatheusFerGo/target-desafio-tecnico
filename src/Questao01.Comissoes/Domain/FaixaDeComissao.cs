namespace Questao01.Comissoes.Domain;

public sealed record FaixaDeComissao(decimal ValorMinimo, decimal Aliquota)
{
    public bool Contem(decimal valorVenda) => valorVenda >= ValorMinimo;
}

namespace Questao02.Estoque.Domain;

public sealed class EstoqueInsuficienteException(int codigoProduto, int quantidadeEmEstoque, int quantidadeSolicitada)
    : Exception($"Estoque insuficiente para o produto {codigoProduto}: saldo de {quantidadeEmEstoque}, saída solicitada de {quantidadeSolicitada}.")
{
    public int CodigoProduto { get; } = codigoProduto;

    public int QuantidadeEmEstoque { get; } = quantidadeEmEstoque;

    public int QuantidadeSolicitada { get; } = quantidadeSolicitada;
}

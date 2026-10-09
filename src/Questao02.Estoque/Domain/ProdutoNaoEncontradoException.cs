namespace Questao02.Estoque.Domain;

public sealed class ProdutoNaoEncontradoException(int codigoProduto)
    : Exception($"Produto {codigoProduto} não encontrado no estoque.")
{
    public int CodigoProduto { get; } = codigoProduto;
}

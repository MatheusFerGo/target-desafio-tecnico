using Questao02.Estoque.Domain;

namespace Questao02.Estoque.Application;

public class MovimentadorDeEstoque
{
    private readonly Dictionary<int, Produto> _produtosPorCodigo;

    public MovimentadorDeEstoque(IEnumerable<Produto> produtos)
    {
        ArgumentNullException.ThrowIfNull(produtos);

        _produtosPorCodigo = produtos.ToDictionary(produto => produto.Codigo);
    }

    public Movimentacao Movimentar(int codigoProduto, TipoMovimentacao tipo, int quantidade, string descricao)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(descricao);

        if (!_produtosPorCodigo.TryGetValue(codigoProduto, out var produto))
        {
            throw new ProdutoNaoEncontradoException(codigoProduto);
        }

        switch (tipo)
        {
            case TipoMovimentacao.Entrada:
                produto.DarEntrada(quantidade);
                break;
            case TipoMovimentacao.Saida:
                produto.DarSaida(quantidade);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(tipo), tipo, "Tipo de movimentação inválido.");
        }

        return new Movimentacao(Guid.NewGuid(), codigoProduto, tipo, descricao, quantidade, produto.QuantidadeEmEstoque);
    }
}

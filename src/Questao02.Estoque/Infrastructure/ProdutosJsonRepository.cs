using System.Text.Json;
using Questao02.Estoque.Application;
using Questao02.Estoque.Domain;

namespace Questao02.Estoque.Infrastructure;

public class ProdutosJsonRepository : IProdutosRepository
{
    private static readonly JsonSerializerOptions OpcoesDeLeitura = new() { PropertyNameCaseInsensitive = true };

    private readonly string _caminhoArquivo;

    public ProdutosJsonRepository(string caminhoArquivo)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(caminhoArquivo);

        _caminhoArquivo = caminhoArquivo;
    }

    public IReadOnlyList<Produto> ObterTodos()
    {
        var json = File.ReadAllText(_caminhoArquivo);

        var arquivo = JsonSerializer.Deserialize<ArquivoDeEstoque>(json, OpcoesDeLeitura);

        var produtos = arquivo?.Estoque
            ?? throw new InvalidDataException("O arquivo de estoque não contém a lista 'estoque'.");

        return produtos
            .Select(produto => new Produto(produto.CodigoProduto, produto.DescricaoProduto, produto.Estoque))
            .ToArray();
    }

    private sealed record ArquivoDeEstoque(List<ProdutoJson>? Estoque);

    private sealed record ProdutoJson(int CodigoProduto, string DescricaoProduto, int Estoque);
}

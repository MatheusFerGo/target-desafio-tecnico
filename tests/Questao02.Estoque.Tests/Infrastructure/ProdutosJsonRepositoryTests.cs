using Questao02.Estoque.Infrastructure;
using Xunit;

namespace Questao02.Estoque.Tests.Infrastructure;

public sealed class ProdutosJsonRepositoryTests : IDisposable
{
    private readonly string _caminhoArquivo = Path.Combine(Path.GetTempPath(), $"{Guid.NewGuid()}.json");

    public void Dispose()
    {
        if (File.Exists(_caminhoArquivo))
        {
            File.Delete(_caminhoArquivo);
        }
    }

    [Fact]
    public void Deve_LerProdutos_DoArquivoJson()
    {
        File.WriteAllText(_caminhoArquivo, """
            {
              "estoque": [
                { "codigoProduto": 101, "descricaoProduto": "Caneta Azul", "estoque": 150 },
                { "codigoProduto": 102, "descricaoProduto": "Caderno Universitário", "estoque": 75 }
              ]
            }
            """);
        var repository = new ProdutosJsonRepository(_caminhoArquivo);

        var produtos = repository.ObterTodos();

        Assert.Collection(
            produtos,
            primeiro =>
            {
                Assert.Equal(101, primeiro.Codigo);
                Assert.Equal("Caneta Azul", primeiro.Descricao);
                Assert.Equal(150, primeiro.QuantidadeEmEstoque);
            },
            segundo =>
            {
                Assert.Equal(102, segundo.Codigo);
                Assert.Equal("Caderno Universitário", segundo.Descricao);
                Assert.Equal(75, segundo.QuantidadeEmEstoque);
            });
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoArquivoNaoExistir()
    {
        var repository = new ProdutosJsonRepository(_caminhoArquivo);

        Assert.Throws<FileNotFoundException>(() => repository.ObterTodos());
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoArquivoNaoTiverAListaDeEstoque()
    {
        File.WriteAllText(_caminhoArquivo, "{}");
        var repository = new ProdutosJsonRepository(_caminhoArquivo);

        Assert.Throws<InvalidDataException>(() => repository.ObterTodos());
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoOConteudoDoArquivoForNulo()
    {
        File.WriteAllText(_caminhoArquivo, "null");
        var repository = new ProdutosJsonRepository(_caminhoArquivo);

        Assert.Throws<InvalidDataException>(() => repository.ObterTodos());
    }
}

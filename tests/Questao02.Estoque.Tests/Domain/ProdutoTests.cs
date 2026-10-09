using Questao02.Estoque.Domain;
using Xunit;

namespace Questao02.Estoque.Tests.Domain;

public class ProdutoTests
{
    private static Produto CriarProduto(int quantidadeEmEstoque = 150) =>
        new(101, "Caneta Azul", quantidadeEmEstoque);

    [Fact]
    public void Deve_AumentarEstoque_AoDarEntrada()
    {
        var produto = CriarProduto(150);

        produto.DarEntrada(50);

        Assert.Equal(200, produto.QuantidadeEmEstoque);
    }

    [Fact]
    public void Deve_DiminuirEstoque_AoDarSaida()
    {
        var produto = CriarProduto(150);

        produto.DarSaida(50);

        Assert.Equal(100, produto.QuantidadeEmEstoque);
    }

    [Fact]
    public void Deve_PermitirZerarEstoque_QuandoSaidaForIgualAoSaldo()
    {
        var produto = CriarProduto(150);

        produto.DarSaida(150);

        Assert.Equal(0, produto.QuantidadeEmEstoque);
    }

    [Fact]
    public void Deve_LancarExcecaoSemAlterarSaldo_QuandoSaidaForMaiorQueOSaldo()
    {
        var produto = CriarProduto(150);

        Assert.Throws<EstoqueInsuficienteException>(() => produto.DarSaida(151));

        Assert.Equal(150, produto.QuantidadeEmEstoque);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Deve_LancarExcecao_QuandoQuantidadeDaEntradaForInvalida(int quantidade)
    {
        var produto = CriarProduto();

        Assert.Throws<ArgumentOutOfRangeException>(() => produto.DarEntrada(quantidade));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void Deve_LancarExcecao_QuandoQuantidadeDaSaidaForInvalida(int quantidade)
    {
        var produto = CriarProduto();

        Assert.Throws<ArgumentOutOfRangeException>(() => produto.DarSaida(quantidade));
    }
}

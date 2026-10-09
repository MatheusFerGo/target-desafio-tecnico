using Questao02.Estoque.Application;
using Questao02.Estoque.Domain;
using Xunit;

namespace Questao02.Estoque.Tests.Application;

public class MovimentadorDeEstoqueTests
{
    private readonly MovimentadorDeEstoque _movimentador = new(
    [
        new Produto(101, "Caneta Azul", 150),
        new Produto(102, "Caderno Universitário", 75),
    ]);

    [Fact]
    public void Deve_RetornarSaldoFinal_AposEntrada()
    {
        var movimentacao = _movimentador.Movimentar(101, TipoMovimentacao.Entrada, 50, "Compra de fornecedor");

        Assert.Equal(200, movimentacao.SaldoFinal);
    }

    [Fact]
    public void Deve_RetornarSaldoFinal_AposSaida()
    {
        var movimentacao = _movimentador.Movimentar(101, TipoMovimentacao.Saida, 50, "Venda ao cliente");

        Assert.Equal(100, movimentacao.SaldoFinal);
    }

    [Fact]
    public void Deve_RegistrarOsDadosInformados_NaMovimentacao()
    {
        var movimentacao = _movimentador.Movimentar(102, TipoMovimentacao.Saida, 5, "Venda ao cliente");

        Assert.Equal(102, movimentacao.CodigoProduto);
        Assert.Equal(TipoMovimentacao.Saida, movimentacao.Tipo);
        Assert.Equal("Venda ao cliente", movimentacao.Descricao);
        Assert.Equal(5, movimentacao.Quantidade);
    }

    [Fact]
    public void Deve_AcumularSaldo_EmMovimentacoesSeguidasDoMesmoProduto()
    {
        _movimentador.Movimentar(101, TipoMovimentacao.Entrada, 50, "Compra de fornecedor");

        var movimentacao = _movimentador.Movimentar(101, TipoMovimentacao.Saida, 30, "Venda ao cliente");

        Assert.Equal(170, movimentacao.SaldoFinal);
    }

    [Fact]
    public void Deve_GerarIdentificadoresUnicos_ParaCadaMovimentacao()
    {
        var primeira = _movimentador.Movimentar(101, TipoMovimentacao.Entrada, 1, "Ajuste de inventário");
        var segunda = _movimentador.Movimentar(101, TipoMovimentacao.Entrada, 1, "Ajuste de inventário");

        Assert.NotEqual(Guid.Empty, primeira.Id);
        Assert.NotEqual(primeira.Id, segunda.Id);
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoProdutoNaoExistir()
    {
        Assert.Throws<ProdutoNaoEncontradoException>(
            () => _movimentador.Movimentar(999, TipoMovimentacao.Entrada, 1, "Compra de fornecedor"));
    }

    [Fact]
    public void Deve_LancarExcecaoSemAlterarSaldo_QuandoSaidaForMaiorQueOSaldo()
    {
        Assert.Throws<EstoqueInsuficienteException>(
            () => _movimentador.Movimentar(101, TipoMovimentacao.Saida, 151, "Venda ao cliente"));

        var movimentacao = _movimentador.Movimentar(101, TipoMovimentacao.Entrada, 1, "Ajuste de inventário");

        Assert.Equal(151, movimentacao.SaldoFinal);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Deve_LancarExcecao_QuandoDescricaoForInvalida(string descricao)
    {
        Assert.Throws<ArgumentException>(
            () => _movimentador.Movimentar(101, TipoMovimentacao.Entrada, 1, descricao));
    }
}

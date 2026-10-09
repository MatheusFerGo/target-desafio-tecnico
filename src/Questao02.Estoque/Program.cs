using System.Text;
using Questao02.Estoque.Application;
using Questao02.Estoque.Domain;
using Questao02.Estoque.Infrastructure;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var caminhoDoEstoque = Path.Combine(AppContext.BaseDirectory, "Data", "estoque.json");

IProdutosRepository produtosRepository = new ProdutosJsonRepository(caminhoDoEstoque);
var produtos = produtosRepository.ObterTodos();
var movimentador = new MovimentadorDeEstoque(produtos);

Console.WriteLine("Estoque atual:");
foreach (var produto in produtos)
{
    Console.WriteLine($"  {produto.Codigo} - {produto.Descricao}: {produto.QuantidadeEmEstoque}");
}

while (true)
{
    Console.WriteLine();

    var codigoDigitado = Ler("Código do produto (Enter para sair): ");
    if (string.IsNullOrWhiteSpace(codigoDigitado))
    {
        break;
    }

    if (!int.TryParse(codigoDigitado, out var codigo))
    {
        Console.WriteLine("Código inválido.");
        continue;
    }

    TipoMovimentacao tipo;
    switch (Ler("Tipo (E = entrada, S = saída): ").Trim().ToUpperInvariant())
    {
        case "E":
            tipo = TipoMovimentacao.Entrada;
            break;
        case "S":
            tipo = TipoMovimentacao.Saida;
            break;
        default:
            Console.WriteLine("Tipo inválido. Use E para entrada ou S para saída.");
            continue;
    }

    if (!int.TryParse(Ler("Quantidade: "), out var quantidade) || quantidade <= 0)
    {
        Console.WriteLine("Quantidade inválida. Informe um número inteiro maior que zero.");
        continue;
    }

    var descricao = Ler("Descrição da movimentação: ").Trim();
    if (descricao.Length == 0)
    {
        Console.WriteLine("A descrição é obrigatória.");
        continue;
    }

    try
    {
        var movimentacao = movimentador.Movimentar(codigo, tipo, quantidade, descricao);
        var produto = produtos.First(p => p.Codigo == movimentacao.CodigoProduto);

        Console.WriteLine($"Movimentação {movimentacao.Id} registrada ({movimentacao.Tipo}).");
        Console.WriteLine($"Saldo final de {produto.Descricao}: {movimentacao.SaldoFinal}");
    }
    catch (Exception ex) when (ex is ProdutoNaoEncontradoException or EstoqueInsuficienteException)
    {
        Console.WriteLine(ex.Message);
    }
}

static string Ler(string pergunta)
{
    Console.Write(pergunta);
    return Console.ReadLine() ?? string.Empty;
}

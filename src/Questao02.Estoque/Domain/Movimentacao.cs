namespace Questao02.Estoque.Domain;

public sealed record Movimentacao(
    Guid Id,
    int CodigoProduto,
    TipoMovimentacao Tipo,
    string Descricao,
    int Quantidade,
    int SaldoFinal);

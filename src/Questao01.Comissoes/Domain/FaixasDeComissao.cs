namespace Questao01.Comissoes.Domain;

public static class FaixasDeComissao
{
    public static IReadOnlyList<FaixaDeComissao> Padrao { get; } =
    [
        new FaixaDeComissao(ValorMinimo: 0m, Aliquota: 0m),
        new FaixaDeComissao(ValorMinimo: 100m, Aliquota: 0.01m),
        new FaixaDeComissao(ValorMinimo: 500m, Aliquota: 0.05m),
    ];
}

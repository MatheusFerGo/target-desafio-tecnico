namespace Questao02.Estoque.Domain;

public sealed class Produto
{
    public Produto(int codigo, string descricao, int quantidadeEmEstoque)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(descricao);
        ArgumentOutOfRangeException.ThrowIfNegative(quantidadeEmEstoque);

        Codigo = codigo;
        Descricao = descricao;
        QuantidadeEmEstoque = quantidadeEmEstoque;
    }

    public int Codigo { get; }

    public string Descricao { get; }

    public int QuantidadeEmEstoque { get; private set; }

    public void DarEntrada(int quantidade)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantidade);

        QuantidadeEmEstoque += quantidade;
    }

    public void DarSaida(int quantidade)
    {
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(quantidade);

        if (quantidade > QuantidadeEmEstoque)
        {
            throw new EstoqueInsuficienteException(Codigo, QuantidadeEmEstoque, quantidade);
        }

        QuantidadeEmEstoque -= quantidade;
    }
}

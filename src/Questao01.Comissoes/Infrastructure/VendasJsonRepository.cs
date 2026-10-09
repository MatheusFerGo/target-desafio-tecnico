using System.Text.Json;
using Questao01.Comissoes.Application;
using Questao01.Comissoes.Domain;

namespace Questao01.Comissoes.Infrastructure;

public class VendasJsonRepository : IVendasRepository
{
    private static readonly JsonSerializerOptions OpcoesDeLeitura = new() { PropertyNameCaseInsensitive = true };

    private readonly string _caminhoArquivo;

    public VendasJsonRepository(string caminhoArquivo)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(caminhoArquivo);

        _caminhoArquivo = caminhoArquivo;
    }

    public IReadOnlyList<Venda> ObterTodas()
    {
        var json = File.ReadAllText(_caminhoArquivo);

        var arquivo = JsonSerializer.Deserialize<ArquivoDeVendas>(json, OpcoesDeLeitura);

        var vendas = arquivo?.Vendas
            ?? throw new InvalidDataException("O arquivo de vendas não contém a lista 'vendas'.");

        return vendas.Select(venda => new Venda(venda.Vendedor, venda.Valor)).ToArray();
    }

    private sealed record ArquivoDeVendas(List<VendaJson>? Vendas);

    private sealed record VendaJson(string Vendedor, decimal Valor);
}

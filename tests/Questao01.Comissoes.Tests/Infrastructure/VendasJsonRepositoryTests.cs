using Questao01.Comissoes.Domain;
using Questao01.Comissoes.Infrastructure;
using Xunit;

namespace Questao01.Comissoes.Tests.Infrastructure;

public sealed class VendasJsonRepositoryTests : IDisposable
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
    public void Deve_LerVendas_DoArquivoJson()
    {
        File.WriteAllText(_caminhoArquivo, """
            {
              "vendas": [
                { "vendedor": "João Silva", "valor": 1200.50 },
                { "vendedor": "Maria Souza", "valor": 90.75 }
              ]
            }
            """);
        var repository = new VendasJsonRepository(_caminhoArquivo);

        var vendas = repository.ObterTodas();

        var esperado = new[]
        {
            new Venda("João Silva", 1200.50m),
            new Venda("Maria Souza", 90.75m),
        };
        Assert.Equal(esperado, vendas);
    }

    [Fact]
    public void Deve_LancarExcecao_QuandoArquivoNaoExistir()
    {
        var repository = new VendasJsonRepository(_caminhoArquivo);

        Assert.Throws<FileNotFoundException>(() => repository.ObterTodas());
    }
}

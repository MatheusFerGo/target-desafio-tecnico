using System.Globalization;
using System.Text;
using Questao01.Comissoes.Application;
using Questao01.Comissoes.Domain;
using Questao01.Comissoes.Infrastructure;

Console.OutputEncoding = Encoding.UTF8;

var caminhoDasVendas = Path.Combine(AppContext.BaseDirectory, "Data", "vendas.json");

IVendasRepository vendasRepository = new VendasJsonRepository(caminhoDasVendas);
var apurador = new ApuradorDeComissoes(new CalculadoraDeComissao(FaixasDeComissao.Padrao));

var comissoes = apurador.Apurar(vendasRepository.ObterTodas());

var cultura = new CultureInfo("pt-BR");

foreach (var comissao in comissoes)
{
    Console.WriteLine($"{comissao.Vendedor}: {comissao.TotalComissao.ToString("C", cultura)}");
}

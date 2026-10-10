using System.Globalization;
using System.Text;
using Questao03.Juros.Application;
using Questao03.Juros.Domain;

const decimal TaxaDiaria = 0.025m;

Console.OutputEncoding = Encoding.UTF8;
Console.InputEncoding = Encoding.UTF8;

var cultura = new CultureInfo("pt-BR");

while (true)
{
    Console.WriteLine();

    var valorDigitado = Ler("Valor (Enter para sair): ");
    if (string.IsNullOrWhiteSpace(valorDigitado))
    {
        break;
    }

    if (!decimal.TryParse(valorDigitado, NumberStyles.AllowDecimalPoint, cultura, out var valor) || valor < 0)
    {
        Console.WriteLine("Valor inválido. Use o formato 1000,50.");
        continue;
    }

    if (!DateOnly.TryParseExact(Ler("Data de vencimento (dd/MM/aaaa): "), "dd/MM/yyyy", cultura, DateTimeStyles.None, out var vencimento))
    {
        Console.WriteLine("Data inválida. Use o formato dd/MM/aaaa.");
        continue;
    }

    IRegraDeJuros regra;
    switch (Ler("Tipo de juros (S = simples, C = compostos): ").Trim().ToUpperInvariant())
    {
        case "S":
            regra = new JurosSimples(TaxaDiaria);
            break;
        case "C":
            regra = new JurosCompostos(TaxaDiaria);
            break;
        default:
            Console.WriteLine("Tipo inválido. Use S para simples ou C para compostos.");
            continue;
    }

    var calculadora = new CalculadoraDeJuros(regra, TimeProvider.System);

    try
    {
        var resultado = calculadora.Calcular(valor, vencimento);

        Console.WriteLine($"Dias em atraso: {resultado.DiasEmAtraso}");
        Console.WriteLine($"Juros: {resultado.Juros.ToString("C", cultura)}");
        Console.WriteLine($"Valor atualizado: {resultado.ValorAtualizado.ToString("C", cultura)}");
    }
    catch (ArgumentOutOfRangeException ex) when (ex.ParamName == "diasEmAtraso")
    {
        Console.WriteLine("O período de atraso é grande demais para calcular juros compostos.");
    }
}

static string Ler(string pergunta)
{
    Console.Write(pergunta);
    return Console.ReadLine() ?? string.Empty;
}
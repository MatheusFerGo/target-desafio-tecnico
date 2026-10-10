# Questão 3: Juros por atraso

## Enunciado

A partir de um valor e de uma data de vencimento, calcular o valor dos juros na data de hoje, considerando que a multa seja de 2,5% ao dia.

## Premissas

O enunciado mistura os termos "multa" e "juros", e não diz se o acúmulo é simples ou composto. A interpretação adotada foi:

- **A taxa de 2,5% ao dia é aplicada como juros por dia de atraso.** O valor devolvido pelo cálculo são os **juros**, e o programa mostra também o **valor atualizado** (valor original mais juros).
- **Juros simples e compostos são ambos suportados.** A escolha fica em aberto e é feita por quem usa o programa:
  - Simples: `valor × taxa × dias`.
  - Compostos: `valor × ((1 + taxa)^dias − 1)`.
- **Dias de atraso** são dias de calendário entre o vencimento e a data de hoje.
- **Vencimento hoje ou no futuro** não gera atraso: 0 dias e juros zero.
- **Arredondamento** em 2 casas decimais (`MidpointRounding.AwayFromZero`), aplicado ao valor dos juros no final do cálculo.
- **Valor, dias e taxa negativos** são inválidos e lançam `ArgumentOutOfRangeException`.
- **Período grande demais para os juros compostos** (o `decimal` estoura) lança `ArgumentOutOfRangeException` com mensagem clara, em vez de um `OverflowException`.
- **A data de "hoje"** vem do relógio do sistema em execução normal.

## Decisões de design

- **`decimal` para dinheiro**, nunca `double`.
- **Strategy para o tipo de juros**: `IRegraDeJuros` define o contrato, e `JurosSimples` e `JurosCompostos` o implementam. Uma nova regra entra sem alterar a calculadora (Open/Closed).
- **Taxa configurável**: cada regra recebe a taxa diária pelo construtor. Os 2,5% ao dia do enunciado são uma constante nomeada no `Program.cs`.
- **`TimeProvider` injetado** na `CalculadoraDeJuros`, em vez de `DateTime.Today` direto. Os testes usam um relógio fixo, então o resultado não depende do dia em que rodam.
- **Exponenciação por quadrados** nos juros compostos: poucas multiplicações mesmo para muitos dias, em vez de um laço com um passo por dia.
- **`ResultadoDoCalculo`** é um `record` imutável. O `ValorAtualizado` é uma propriedade calculada (original mais juros), então nunca fica inconsistente com os outros campos.
- **`Program.cs`** é a raiz de composição e a interface com o usuário. Valida a digitação com mensagens em português.

## Estrutura

```
src/Questao03.Juros/
├── Domain/        IRegraDeJuros, JurosSimples, JurosCompostos,
│                  ResultadoDoCalculo
├── Application/   CalculadoraDeJuros
└── Program.cs
tests/Questao03.Juros.Tests/
```

## Como executar

```powershell
dotnet run --project src/Questao03.Juros
dotnet test tests/Questao03.Juros.Tests
```

O programa pede, um valor por vez:

- Valor, com vírgula decimal (por exemplo, `1000,50`). Enter vazio encerra.
- Data de vencimento, no formato `dd/MM/aaaa`.
- Tipo de juros: `S` para simples ou `C` para compostos.

Valor com ponto decimal (como `1000.50`) é rejeitado de propósito: em `pt-BR` o ponto é separador de milhar, e aceitá-lo leria o valor errado em silêncio.

## Exemplo de uso

Execução em 09/10/2026:

```
Valor (Enter para sair): 1000
Data de vencimento (dd/MM/aaaa): 30/09/2026
Tipo de juros (S = simples, C = compostos): S
Dias em atraso: 9
Juros: R$ 225,00
Valor atualizado: R$ 1.225,00

Valor (Enter para sair): 1000,50
Data de vencimento (dd/MM/aaaa): 30/09/2026
Tipo de juros (S = simples, C = compostos): c
Dias em atraso: 9
Juros: R$ 248,99
Valor atualizado: R$ 1.249,49
```

O resultado depende do dia em que o programa é executado.

## Testes

25 testes automatizados (xUnit) para esta questão, cobrindo:

- `JurosSimples` e `JurosCompostos`: valores esperados, arredondamento, zero dias, valor, dias e taxa negativos e, nos compostos, o período grande demais.
- `CalculadoraDeJuros`: vencimento no passado, ontem, hoje e no futuro, uso da regra recebida, valor negativo e regra nula.
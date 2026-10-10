# Questão 1: Comissão de vendedores

## Enunciado

A partir de um JSON com vendas de um time comercial, calcular a comissão de cada vendedor. A regra vale **para cada venda**:

- Venda abaixo de R$ 100,00: sem comissão.
- Venda abaixo de R$ 500,00: comissão de 1%.
- Venda a partir de R$ 500,00: comissão de 5%.

## Premissas

O enunciado deixa os limites em aberto, então a interpretação adotada foi:

| Valor da venda | Comissão |
|---|---|
| menor que R$ 100,00 | 0% |
| de R$ 100,00 até R$ 499,99 | 1% |
| a partir de R$ 500,00 | 5% |

- Os limites são **inclusivos** na faixa de cima: uma venda de exatamente R$ 100,00 gera 1% e de exatamente R$ 500,00 gera 5%. Há testes para os dois casos, e o vendedor Carlos Oliveira tem uma venda de R$ 500,00 exatos.
- A comissão é calculada **venda a venda**, e não sobre o total do vendedor.
- Cada comissão é arredondada para 2 casas decimais (`MidpointRounding.AwayFromZero`) **antes** de somar. Exemplo: 5% de R$ 1.200,50 é R$ 60,025, que vira R$ 60,03.
- Valor de venda negativo é inválido e lança `ArgumentOutOfRangeException`.
- Os vendedores são listados na ordem em que aparecem pela primeira vez no JSON.

## Decisões de design

- **`decimal` para dinheiro**, nunca `double`, para evitar erros de ponto flutuante.
- **Regra como dado**: as faixas são uma lista de `FaixaDeComissao` (valor mínimo e alíquota). Uma nova faixa entra na tabela sem alterar a calculadora (Open/Closed).
- **`CalculadoraDeComissao`** recebe as faixas pelo construtor e valida que existe uma faixa começando em zero.
- **`ApuradorDeComissoes`** (camada `Application`) agrupa as vendas por vendedor e delega o cálculo à calculadora.
- **`IVendasRepository`** isola a origem dos dados. Só `VendasJsonRepository` (camada `Infrastructure`) conhece JSON e arquivo, então trocar por banco ou API não afeta o restante (Dependency Inversion).
- **`Program.cs`** é a raiz de composição: o único lugar que conhece todas as peças.

## Estrutura

```
src/Questao01.Comissoes/
├── Domain/           Venda, FaixaDeComissao, FaixasDeComissao,
│                     CalculadoraDeComissao, ComissaoDoVendedor
├── Application/      IVendasRepository, ApuradorDeComissoes
├── Infrastructure/   VendasJsonRepository
├── Data/vendas.json
└── Program.cs
tests/Questao01.Comissoes.Tests/
```

## Como executar

```powershell
dotnet run --project src/Questao01.Comissoes
dotnet test tests/Questao01.Comissoes.Tests
```

## Resultado esperado

```
João Silva: R$ 495,69
Maria Souza: R$ 465,96
Carlos Oliveira: R$ 379,38
Ana Lima: R$ 404,99
```

## Testes

16 testes automatizados (xUnit) cobrindo as faixas e seus limites, o arredondamento, o valor negativo, a apuração por vendedor e a leitura do JSON.
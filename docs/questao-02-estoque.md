# Questão 2: Movimentação de estoque

## Enunciado

Um programa para lançar movimentações de estoque dos produtos de um JSON, dando entrada ou saída de mercadoria no depósito. Cada movimentação deve ter:

- Um número identificador único.
- Uma descrição para identificar o tipo da movimentação realizada.

Ao final de cada movimentação, o programa retorna a quantidade final em estoque do produto movimentado.

## Premissas

O enunciado deixa alguns pontos em aberto, então a interpretação adotada foi:

- **Tipo e descrição são coisas diferentes.** O tipo da movimentação é um enum (`Entrada` ou `Saida`), e a descrição é um texto livre informado por quem lança (por exemplo, "Compra de fornecedor"). Os dois ficam registrados na movimentação.
- **A descrição é obrigatória.** Texto vazio ou só com espaços é inválido e não altera o estoque.
- **Identificador único** é um `Guid`, gerado no momento da movimentação, sem contador nem estado compartilhado.
- **Quantidade** é um número inteiro maior que zero. Zero ou negativo é inválido.
- **Saída maior que o saldo é rejeitada** com `EstoqueInsuficienteException`, e o saldo permanece o mesmo. Saída igual ao saldo é permitida e zera o estoque.
- **Produto inexistente** lança `ProdutoNaoEncontradoException`.
- **O estoque vive em memória.** A cada execução o programa parte do JSON original, e as movimentações **não são gravadas de volta** no arquivo.
- **O retorno de cada movimentação** é a própria `Movimentacao`, que já traz o `SaldoFinal` do produto.

## Decisões de design

- **`Produto` é dono do próprio saldo.** `QuantidadeEmEstoque` tem `private set`, e a única forma de alterá-lo é `DarEntrada` ou `DarSaida`. A regra "não existe estoque negativo" não pode ser burlada por quem chama, e a validação acontece antes de subtrair.
- **`Movimentacao` é um `record` imutável**: uma movimentação feita não muda.
- **`MovimentadorDeEstoque`** (camada `Application`) localiza o produto pelo código, aplica a entrada ou a saída e devolve a movimentação. Valida a descrição antes de mexer no produto.
- **Exceções de domínio próprias**, para quem chama poder tratar cada caso de forma específica.
- **`IProdutosRepository`** isola a origem dos dados. Só `ProdutosJsonRepository` (camada `Infrastructure`) conhece JSON e arquivo, então trocar por banco ou API não afeta o restante (Dependency Inversion).
- **`Program.cs`** é a raiz de composição e a interface com o usuário. Valida a digitação (código, tipo, quantidade e descrição) com mensagens em português e só captura as exceções de domínio, para não esconder erros inesperados.

## Estrutura

```
src/Questao02.Estoque/
├── Domain/           Produto, Movimentacao, TipoMovimentacao,
│                     EstoqueInsuficienteException,
│                     ProdutoNaoEncontradoException
├── Application/      IProdutosRepository, MovimentadorDeEstoque
├── Infrastructure/   ProdutosJsonRepository
├── Data/estoque.json
└── Program.cs
tests/Questao02.Estoque.Tests/
```

## Como executar

```powershell
dotnet run --project src/Questao02.Estoque
dotnet test tests/Questao02.Estoque.Tests
```

O programa mostra o estoque atual e pede, um valor por vez: código do produto, tipo (`E` para entrada, `S` para saída), quantidade e descrição. Um Enter vazio no código encerra.

## Exemplo de uso

```
Estoque atual:
  101 - Caneta Azul: 150
  102 - Caderno Universitário: 75
  103 - Borracha Branca: 200
  104 - Lápis Preto HB: 320
  105 - Marcador de Texto Amarelo: 90

Código do produto (Enter para sair): 101
Tipo (E = entrada, S = saída): e
Quantidade: 50
Descrição da movimentação: Compra de fornecedor
Movimentação 702910ba-67f2-49cc-9228-546e191375e8 registrada (Entrada).
Saldo final de Caneta Azul: 200
```

O identificador muda a cada movimentação.

## Testes

20 testes automatizados (xUnit) para esta questão, cobrindo as regras do `Produto` (entrada, saída, saldo zerado, saída maior que o saldo e quantidades inválidas), o `MovimentadorDeEstoque` (saldo final, dados registrados, acúmulo entre movimentações, identificadores únicos, produto inexistente e descrição inválida) e a leitura do JSON.
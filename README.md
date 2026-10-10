# Desafio técnico C#

Soluções de um desafio técnico para a vaga de desenvolvedor C#, em três questões independentes. O foco está em **princípios SOLID**, **nomenclatura clara** e **testes automatizados** (xUnit, escritos primeiro, em TDD), com cada questão desenvolvida em uma branch própria e integrada à `main` por um merge explícito.

## Questões

| # | Tema | Documentação | Código |
|---|---|---|---|
| 1 | Comissão de vendedores por faixa de valor | [questao-01-comissoes.md](docs/questao-01-comissoes.md) | [src/Questao01.Comissoes](src/Questao01.Comissoes) |
| 2 | Movimentação de estoque (entrada e saída) | [questao-02-estoque.md](docs/questao-02-estoque.md) | [src/Questao02.Estoque](src/Questao02.Estoque) |
| 3 | Juros por atraso (simples ou compostos) | [questao-03-juros.md](docs/questao-03-juros.md) | [src/Questao03.Juros](src/Questao03.Juros) |

Cada documento traz o enunciado, as **premissas** adotadas onde o texto deixava pontos em aberto, as decisões de design e um exemplo de uso.

## Requisitos

- [.NET SDK 9](https://dotnet.microsoft.com/download)

## Como executar

```powershell
git clone https://github.com/MatheusFerGo/target-desafio-tecnico.git
cd target-desafio-tecnico
dotnet build
```

Cada questão é um programa de console:

```powershell
dotnet run --project src/Questao01.Comissoes
dotnet run --project src/Questao02.Estoque
dotnet run --project src/Questao03.Juros
```

As questões 2 e 3 são interativas: o programa pede os dados um por vez e um Enter vazio no primeiro campo encerra.

## Testes

```powershell
dotnet test
```

São 63 testes automatizados: 16 na questão 1, 22 na questão 2 e 25 na questão 3.

### Cobertura

```powershell
dotnet test --settings coverlet.runsettings --collect:"XPlat Code Coverage"
```

| Projeto | Linhas | Ramos |
|---|---|---|
| Questao01.Comissoes | 100% | 100% |
| Questao02.Estoque | 100% | 100% |
| Questao03.Juros | 100% | 100% |

Os três arquivos `Program.cs` ficam **fora da métrica** (configurado em [`coverlet.runsettings`](coverlet.runsettings)). Eles são a interface de console e a raiz de composição: só montam as dependências e leem a digitação, e foram validados com um roteiro manual. Toda a regra de negócio, a leitura de arquivos e os casos de erro estão dentro da métrica.

## Estrutura

```
.
├── docs/                      Documentação de cada questão
├── src/
│   ├── Questao01.Comissoes/   Domain, Application, Infrastructure, Data
│   ├── Questao02.Estoque/     Domain, Application, Infrastructure, Data
│   └── Questao03.Juros/       Domain, Application
├── tests/                     Um projeto xUnit por questão
├── coverlet.runsettings       Configuração da cobertura
└── DesafioTecnico.sln
```

## Histórico do Git

Uma branch por questão (`feat/questao-01-comissoes`, `feat/questao-02-estoque`, `feat/questao-03-juros`) e uma para a cobertura (`test/cobertura-de-regras`), cada uma integrada à `main` com `merge --no-ff`. Os commits seguem o padrão Conventional Commits, com a descrição em português. Para ver a divisão:

```powershell
git log --oneline --graph
```
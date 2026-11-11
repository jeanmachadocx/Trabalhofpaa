# Trabalho Prático de FPAA - Caminho Mínimo com Dijkstra

Implementação do algoritmo de Dijkstra (técnica gulosa) aplicada a
roteamento de menor custo em uma rede logística. Projeto em C# / .NET 8,
sem dependências externas.

## Estrutura

```
DijkstraApp.csproj    Projeto console (.NET 8)
Aresta.cs             Aresta do grafo (origem, destino, peso, capacidade)
Grafo.cs              Grafo dirigido com lista de adjacência
LeitorDIMACS.cs       Leitura de arquivos de entrada
Dijkstra.cs           Algoritmo de Dijkstra (caminho mínimo)
Program.cs            Menu interativo
grafos_dimacs/        Instâncias de teste
```

## Como executar

Requisito: .NET SDK 8.0 ou superior (`dotnet --version`).

```bash
dotnet run
```

O programa inicia com um grafo de exemplo de 6 vértices e 11 arestas. Para
carregar uma instância DIMACS direto na inicialização:

```bash
dotnet run -- grafos_dimacs/grafo01.dimacs
```

Também é possível compilar e executar o binário:

```bash
dotnet build -c Release
dotnet bin/Release/net8.0/DijkstraApp.dll grafos_dimacs/grafo04.dimacs
```

## Menu

```
1 - Rota de menor custo entre dois hubs
2 - Menores custos a partir de um hub
3 - Carregar arquivo DIMACS
0 - Sair
```

A opção 1 exibe custo total, sequência de vértices, quantidade de arestas no
caminho, relaxamentos bem-sucedidos e tempo de execução.

## Formato de entrada

Primeira linha com a quantidade de vértices e de arestas. Cada linha seguinte
representa uma aresta no formato `origem destino peso [capacidade]`, com
vértices numerados de 1 a N. Linhas iniciadas por `#` ou `c` são ignoradas.

```
6 12
1 2 2 10
1 3 3 7
1 4 1 8
```

As instâncias vieram de um projeto maior e trazem uma quarta coluna
(capacidade). Ela é ignorada: aqui o que importa é o peso da aresta.

## Complexidade

Com fila de prioridade binária (`PriorityQueue`), o algoritmo executa em
O((V + E) log V), com O(V) de memória adicional para os vetores de distância e
predecessor. Pesos negativos não são suportados e são rejeitados na inserção
da aresta.

## Verificação

As distâncias produzidas foram comparadas com uma implementação de referência
independente em 189 casos (todas as origens dos sete arquivos em
`grafos_dimacs`), sem divergências. O modo de conferência imprime as distâncias
a partir de um vértice:

```bash
dotnet bin/Release/net8.0/DijkstraApp.dll --dist grafos_dimacs/grafo07.dimacs 1
```

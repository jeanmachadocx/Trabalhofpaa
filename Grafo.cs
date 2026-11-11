public class Grafo
{
    public int QuantidadeVertices { get; private set; }
    public int QuantidadeArestas { get; private set; }
    public List<Aresta> Arestas { get; private set; }

    private readonly List<(int destino, int peso)>[] _listaAdj;

    public Grafo(int vertices)
    {
        QuantidadeVertices = vertices;
        QuantidadeArestas = 0;
        Arestas = new List<Aresta>();

        _listaAdj = new List<(int, int)>[vertices + 1];
        for (int i = 0; i <= vertices; i++)
            _listaAdj[i] = new List<(int, int)>();
    }

    public void AdicionarAresta(int origem, int destino, int peso)
    {
        if (origem < 1 || origem > QuantidadeVertices)
            throw new ArgumentOutOfRangeException(nameof(origem), $"Vértice de origem inválido. Informe um valor entre 1 e {QuantidadeVertices}.");

        if (destino < 1 || destino > QuantidadeVertices)
            throw new ArgumentOutOfRangeException(nameof(destino), $"Vértice de destino inválido. Informe um valor entre 1 e {QuantidadeVertices}.");

        if (peso < 0)
            throw new ArgumentException("Dijkstra exige pesos não negativos.", nameof(peso));

        _listaAdj[origem].Add((destino, peso));
        Arestas.Add(new Aresta(origem, destino, peso));
        QuantidadeArestas++;
    }

    public List<(int destino, int peso)> GetVizinhos(int vertice)
    {
        return _listaAdj[vertice];
    }

    public int GetGrauSaida(int vertice)
    {
        return _listaAdj[vertice].Count;
    }

    public void ExibirEstrutura()
    {
        Console.WriteLine($"Vértices: {QuantidadeVertices} | Arestas: {QuantidadeArestas}");
        Console.WriteLine("Representação: lista de adjacência (grafo dirigido e ponderado)");
    }
}

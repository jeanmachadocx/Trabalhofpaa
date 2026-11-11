// Dijkstra: técnica gulosa para caminho mínimo de fonte única em grafo
// dirigido com pesos não negativos. A cada passo extrai da fila de
// prioridade o vértice de menor custo acumulado ainda não fechado e
// relaxa suas arestas. Com fila de prioridade binária: O((V + E) log V).
public static class Dijkstra
{
    public static (int custo, List<int> caminho) Executar(Grafo grafo, int origem, int destino)
    {
        var r = ExecutarComMetricas(grafo, origem, destino);
        return (r.custo, r.caminho);
    }

    public static (int custo, List<int> caminho, int extracoes, int relaxamentos) ExecutarComMetricas(
        Grafo grafo, int origem, int destino)
    {
        int n = grafo.QuantidadeVertices;

        if (origem < 1 || origem > n)
            throw new ArgumentOutOfRangeException(nameof(origem), $"Hub de origem inválido. Informe um valor entre 1 e {n}.");

        if (destino < 1 || destino > n)
            throw new ArgumentOutOfRangeException(nameof(destino), $"Hub de destino inválido. Informe um valor entre 1 e {n}.");

        int[] dist = new int[n + 1];
        int[] anterior = new int[n + 1];
        Array.Fill(dist, int.MaxValue);
        Array.Fill(anterior, -1);

        dist[origem] = 0;

        int extracoes = 0;
        int relaxamentos = 0;

        // Fila de prioridade mínima: (custo acumulado, vértice)
        PriorityQueue<int, int> fila = new PriorityQueue<int, int>();
        fila.Enqueue(origem, 0);

        while (fila.Count > 0)
        {
            fila.TryDequeue(out int atual, out int prioridade);
            extracoes++;

            // Entrada obsoleta: existe na fila com custo maior que o já registrado
            if (prioridade > dist[atual])
                continue;

            // Se já chegamos ao destino, podemos parar
            if (atual == destino)
                break;

            foreach (var (vizinho, peso) in grafo.GetVizinhos(atual))
            {
                int novaDist = dist[atual] + peso;

                if (novaDist < dist[vizinho])
                {
                    relaxamentos++;
                    dist[vizinho] = novaDist;
                    anterior[vizinho] = atual;
                    fila.Enqueue(vizinho, novaDist);
                }
            }
        }

        List<int> caminho = new List<int>();

        if (dist[destino] == int.MaxValue)
            return (-1, caminho, extracoes, relaxamentos);

        // Reconstrói o caminho de trás para frente
        int v = destino;
        while (v != -1)
        {
            caminho.Add(v);
            v = anterior[v];
        }

        caminho.Reverse();

        return (dist[destino], caminho, extracoes, relaxamentos);
    }

    public static int[] ExecutarDeFonteUnica(Grafo grafo, int origem)
    {
        int n = grafo.QuantidadeVertices;

        if (origem < 1 || origem > n)
            throw new ArgumentOutOfRangeException(nameof(origem), $"Hub de origem inválido. Informe um valor entre 1 e {n}.");

        int[] dist = new int[n + 1];
        Array.Fill(dist, int.MaxValue);
        dist[origem] = 0;

        PriorityQueue<int, int> fila = new PriorityQueue<int, int>();
        fila.Enqueue(origem, 0);

        while (fila.Count > 0)
        {
            fila.TryDequeue(out int atual, out int prioridade);

            if (prioridade > dist[atual])
                continue;

            foreach (var (vizinho, peso) in grafo.GetVizinhos(atual))
            {
                int novaDist = dist[atual] + peso;

                if (novaDist < dist[vizinho])
                {
                    dist[vizinho] = novaDist;
                    fila.Enqueue(vizinho, novaDist);
                }
            }
        }

        return dist;
    }
}

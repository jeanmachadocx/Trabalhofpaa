public static class LeitorDIMACS
{
    public static Grafo Ler(string caminho)
    {
        string[] linhas = File.ReadAllLines(caminho)
            .Where(l => !string.IsNullOrWhiteSpace(l) && !l.TrimStart().StartsWith("#") && !l.TrimStart().StartsWith("c"))
            .ToArray();

        if (linhas.Length == 0)
            throw new InvalidDataException("Arquivo vazio ou apenas com comentários.");

        string[] primeira = linhas[0].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);
        int vertices = int.Parse(primeira[0]);

        Grafo grafo = new Grafo(vertices);

        for (int i = 1; i < linhas.Length; i++)
        {
            string[] dados = linhas[i].Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            int origem = int.Parse(dados[0]);
            int destino = int.Parse(dados[1]);
            int peso = int.Parse(dados[2]);

            grafo.AdicionarAresta(origem, destino, peso);
        }

        return grafo;
    }
}

class Program
{
    static Grafo grafo;

    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        if (args.Length == 3 && args[0] == "--dist")
        {
            ModoDistancias(args[1], int.Parse(args[2]));
            return;
        }

        Console.WriteLine("=== Roteamento de Menor Custo (Dijkstra) ===\n");

        if (args.Length > 0 && File.Exists(args[0]))
            Carregar(args[0]);
        else
            CarregarPadrao();

        bool rodando = true;
        while (rodando)
        {
            Console.WriteLine("\n--- Menu ---");
            Console.WriteLine("1 - Rota de menor custo entre dois hubs");
            Console.WriteLine("2 - Menores custos a partir de um hub");
            Console.WriteLine("3 - Carregar arquivo DIMACS");
            Console.WriteLine("0 - Sair");
            Console.Write("\nEscolha: ");

            string entrada = Console.ReadLine()?.Trim();

            switch (entrada)
            {
                case "1":
                    MenuRota();
                    break;
                case "2":
                    MenuFonteUnica();
                    break;
                case "3":
                    MenuCarregar();
                    break;
                case null:
                case "0":
                    rodando = false;
                    break;
                default:
                    Console.WriteLine("Opção inválida.");
                    break;
            }
        }
    }

    static void CarregarPadrao()
    {
        grafo = new Grafo(6);
        grafo.AdicionarAresta(1, 2, 2);
        grafo.AdicionarAresta(1, 3, 3);
        grafo.AdicionarAresta(1, 4, 1);
        grafo.AdicionarAresta(2, 3, 4);
        grafo.AdicionarAresta(2, 5, 5);
        grafo.AdicionarAresta(2, 6, 6);
        grafo.AdicionarAresta(3, 4, 3);
        grafo.AdicionarAresta(3, 6, 6);
        grafo.AdicionarAresta(4, 5, 2);
        grafo.AdicionarAresta(4, 6, 5);
        grafo.AdicionarAresta(5, 6, 4);

        Console.WriteLine("Grafo de exemplo carregado (6 hubs, 11 rotas).");
        grafo.ExibirEstrutura();
    }

    static void MenuCarregar()
    {
        Console.Write("Caminho do arquivo DIMACS: ");
        Carregar(Console.ReadLine()?.Trim());
    }

    static void Carregar(string caminho)
    {
        try
        {
            grafo = LeitorDIMACS.Ler(caminho);
            Console.WriteLine($"Grafo carregado: {Path.GetFileName(caminho)}");
            grafo.ExibirEstrutura();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Falha ao carregar: {ex.Message}");
        }
    }

    static void MenuRota()
    {
        int origem = LerVertice("Hub de origem: ");
        int destino = LerVertice("Hub de destino: ");

        try
        {
            var inicio = System.Diagnostics.Stopwatch.StartNew();
            var (custo, caminho, extracoes, relaxamentos) = Dijkstra.ExecutarComMetricas(grafo, origem, destino);
            inicio.Stop();

            Console.WriteLine();
            if (custo == -1)
            {
                Console.WriteLine("Não existe caminho entre os hubs informados.");
                return;
            }

            Console.WriteLine($"Custo total: {custo}");
            Console.WriteLine($"Caminho: {string.Join(" -> ", caminho)}");
            Console.WriteLine($"Arestas no caminho: {caminho.Count - 1}");
            Console.WriteLine($"Relaxamentos bem-sucedidos: {relaxamentos}");
            Console.WriteLine($"Tempo: {inicio.Elapsed.TotalMilliseconds:F3} ms");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void MenuFonteUnica()
    {
        int origem = LerVertice("Hub de origem: ");

        try
        {
            var inicio = System.Diagnostics.Stopwatch.StartNew();
            int[] dist = Dijkstra.ExecutarDeFonteUnica(grafo, origem);
            inicio.Stop();

            Console.WriteLine();
            Console.WriteLine($"Menores custos a partir do hub {origem}:");
            for (int v = 1; v <= grafo.QuantidadeVertices; v++)
            {
                string valor = dist[v] == int.MaxValue ? "inalcançável" : dist[v].ToString();
                Console.WriteLine($"  {origem} -> {v}: {valor}");
            }
            Console.WriteLine($"Tempo: {inicio.Elapsed.TotalMilliseconds:F3} ms");
        }
        catch (ArgumentOutOfRangeException ex)
        {
            Console.WriteLine(ex.Message);
        }
    }

    static void ModoDistancias(string caminho, int origem)
    {
        Grafo g = LeitorDIMACS.Ler(caminho);
        int[] dist = Dijkstra.ExecutarDeFonteUnica(g, origem);
        Console.WriteLine(string.Join(" ", dist.Skip(1).Select(d => d == int.MaxValue ? "-1" : d.ToString())));
    }

    static int LerVertice(string mensagem)
    {
        while (true)
        {
            Console.Write(mensagem);
            string entrada = Console.ReadLine();

            if (entrada == null)
            {
                Console.WriteLine();
                Environment.Exit(0);
            }

            if (!int.TryParse(entrada, out int valor))
            {
                Console.WriteLine("Digite um número inteiro válido.");
                continue;
            }

            if (valor < 1 || valor > grafo.QuantidadeVertices)
            {
                Console.WriteLine($"Digite um valor entre 1 e {grafo.QuantidadeVertices}.");
                continue;
            }

            return valor;
        }
    }
}

public class Aresta
{
    public int Origem { get; set; }
    public int Destino { get; set; }
    public int Peso { get; set; }

    public Aresta(int origem, int destino, int peso)
    {
        Origem = origem;
        Destino = destino;
        Peso = peso;
    }
}

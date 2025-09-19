namespace SistemaLogica
{
    public class GerenciadorMemoria
    {
        // Guarda o tamanho total da memória
        public int TamanhoTotal { get; private set; }
        // Armazena todas as alocações de memória feitas
        public List<Memoria> Alocacoes { get; private set; } = new List<Memoria>();

        // Construtor da classe
        public GerenciadorMemoria(int tamanho)
        {
            TamanhoTotal = tamanho;
        }

        // Método que aloca todas as threads de um processo na memória
        public void Alocar(Processo processo)
        {
            foreach (var t in processo.Threads)
            {
                // Verifica se já existe uma alocação para esta thread específica
                bool jaAlocada = Alocacoes.Any(m => m.ProcessoId == processo.ProcessoId && m.ThreadId == t.ThreadId);

                if (!jaAlocada)
                {
                    // Cria um objeto Memoria e adiciona na lista de alocações
                    Alocacoes.Add(new Memoria
                    {
                        // Endereço da thread
                        Endereco = t.EnderecoMemoria,
                        ProcessoId = processo.ProcessoId,
                        ThreadId = t.ThreadId
                    });
                }
            }
        }

        // Método que remove todas as alocações de memória de um processo
        public void Desalocar(string processoId)
        {
            Alocacoes.RemoveAll(m => m.ProcessoId == processoId);
        }

        // Método que mostra a alocação de memória no WPF
        // Adicionei uma verificação para o caso de não haver alocações
        public void Mostrar()
        {
            if (Alocacoes.Count == 0)
            {
                Console.WriteLine("Nenhuma memória alocada.");
                return;
            }

            // Cabeçalho da tabela
            Console.WriteLine($"{"Endereço",-10} | {"Processo",-10} | {"Thread",-10}");
            Console.WriteLine(new string('-', 36));

            // Ordenado por endereço para uma visualização mais limpa
            foreach (var m in Alocacoes.OrderBy(a => a.Endereco))
            {
                Console.WriteLine($"{m.Endereco,-10} | {m.ProcessoId,-10} | {m.ThreadId,-10}");
            }
        }
    }
}

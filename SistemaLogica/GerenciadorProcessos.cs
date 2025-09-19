namespace SistemaLogica
{
    public class GerenciadorProcessos
    {
        public List<Processo> Processos { get; private set; } = new List<Processo>();

        // Adiciona um processo somente se ele ainda não existe
        public void Adicionar(Processo processo)
        {
            if (!Existe(processo.ProcessoId))
            {
                Processos.Add(processo);
            }
            else
            {
                // Se o processo já existe, apenas adiciona as threads novas
                var existente = Buscar(processo.ProcessoId);
                foreach (var t in processo.Threads)
                {
                    if (!existente.Threads.Any(th => th.ThreadId == t.ThreadId))
                        existente.Threads.Add(t);
                }
            }
        }

        // Remove processo pelo ID
        public void Remover(string processoId)
        {
            Processos.RemoveAll(p => p.ProcessoId == processoId);
        }

        // Busca um processo pelo ID
        public Processo? Buscar(string processoId)
        {
            return Processos.FirstOrDefault(p => p.ProcessoId == processoId);
        }

        // Verifica se o processo existe
        public bool Existe(string processoId)
        {
            return Processos.Any(p => p.ProcessoId == processoId);
        }

        // Lista todos os processos e suas threads
        public void Listar()
        {
            if (Processos.Count == 0)
            {
                Console.WriteLine("Nenhum processo cadastrado.");
                return;
            }

            // Cabeçalho da tabela
            Console.WriteLine($"{"Processo",-10} | {"Thread",-10} | {"Tempo",-7} | {"Memória",-10} | {"Prioridade",-10}");
            Console.WriteLine(new string('-', 60)); // Linha separadora

            foreach (var p in Processos)
            {
                if (p.Threads.Count == 0)
                {
                    Console.WriteLine($"{p.ProcessoId,-10} | Nenhuma thread.");
                    continue;
                }
                foreach (var t in p.Threads)
                {
                    // Corpo da tabela com colunas alinhadas
                    Console.WriteLine($"{p.ProcessoId,-10} | {t.ThreadId,-10} | {t.TempoExecucao,-7} | {t.EnderecoMemoria,-10} | {t.Prioridade,-10}");
                }
            }
        }

        // Calcula o tempo total de execução de todas as threads de todos os processos
        public int TempoExecucaoTotal()
        {
            return Processos.Sum(p => p.Threads.Sum(t => t.TempoExecucao));
        }

        public List<Processo> ObterTodos()
        {
            return Processos; // supondo que você guarda numa List<Processo> chamada processos
        }
    }
}

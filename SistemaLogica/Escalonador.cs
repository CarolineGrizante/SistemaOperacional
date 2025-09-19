namespace SistemaLogica
{
    public class Escalonador
    {
        // Guarda a referência para o GerenciadorProcessos
        private readonly GerenciadorProcessos _gerenciador;

        // Construtor Escalonador
        public Escalonador(GerenciadorProcessos gerenciador)
        {
            _gerenciador = gerenciador;
        }

        // A thread que chega primeiro é escalonada primeiro
        public void FCFS()
        {
            var todas = _gerenciador.Processos
                .SelectMany(p => p.Threads.Select(t => new { Processo = p.ProcessoId, Thread = t }))
                .ToList();

            // Cabeçalho da tabela
            Console.WriteLine($"{"Ordem",-7} | {"Processo-Thread",-20} | {"Tempo de Execução",-20}");
            Console.WriteLine(new string('-', 55));

            int ordem = 1;
            // Executa cada thread exatamente na ordem da lista
            foreach (var item in todas)
            {
                Console.WriteLine($"{ordem,-7} | {$"{item.Processo}-{item.Thread.ThreadId}",-20} | {item.Thread.TempoExecucao,-20}");
                ordem++;
            }
        }

        // A thread com menor tempo de execução é escalonada primeiro
        public void SJF()
        {
            // Monta uma lista com todas as threads e ordena pelo tempo de execução
            var todas = _gerenciador.Processos
                .SelectMany(p => p.Threads.Select(t => new { Processo = p.ProcessoId, Thread = t }))
                .OrderBy(x => x.Thread.TempoExecucao)
                .ToList();

            // Cabeçalho da tabela
            Console.WriteLine($"{"Ordem",-7} | {"Processo-Thread",-20} | {"Tempo de Execução",-20}");
            Console.WriteLine(new string('-', 55));

            int ordem = 1;
            // Percorre a lista já ordenada e imprime na ordem
            foreach (var item in todas)
            {
                Console.WriteLine($"{ordem,-7} | {$"{item.Processo}-{item.Thread.ThreadId}",-20} | {item.Thread.TempoExecucao,-20}");
                ordem++;
            }
        }

        // A ideia é dar um "pedaço de tempo" igual para cada processo/thread
        public void RR(int quantum = 3) // Foi definido um quantum padrão de 3 mas pode ser alterado ao chamar o método

        {
            // Monta uma lista temporária com todas as threads de todos os processos
            // Cada item da lista tem o ID do processo e a referência da thread
            // Isso permite que o texto de execução total seja preservado
            var fila = _gerenciador.Processos
                .SelectMany(p => p.Threads.Select(t => new ThreadExecucaoRR
                {
                    ProcessoId = p.ProcessoId,
                    ThreadId = t.ThreadId,
                    TempoRestante = t.TempoExecucao // Copia o tempo de execução original
                }))
                .ToList();

            if (!fila.Any())
            {
                Console.WriteLine("Nenhuma thread para executar.");
                return;
            }

            // Cabeçalho da tabela
            Console.WriteLine($"Execução RR (Quantum = {quantum}):");
            Console.WriteLine($"{"Processo-Thread",-20} | {"Executou",-10} | {"Restante",-10}");
            Console.WriteLine(new string('-', 45));

            // Continua enquanto houver alguma thread com tempo restante
            while (fila.Any(t => t.TempoRestante > 0))
            {
                // Percorre a fila de threads
                foreach (var item in fila)
                {
                    // Se a thread ainda precisa executar
                    if (item.TempoRestante > 0)
                    {
                        // Calcula o tempo que será executado nesta rodada
                        int tempoExecutar = Math.Min(quantum, item.TempoRestante);

                        // Subtrai o tempo executado
                        item.TempoRestante -= tempoExecutar;

                        // Exibe a execução em formato de tabela
                        Console.WriteLine($"{$"{item.ProcessoId}-{item.ThreadId}",-20} | {tempoExecutar,-10} | {item.TempoRestante,-10}");
                    }
                }
            }
        }
    }
}

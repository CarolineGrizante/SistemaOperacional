namespace SistemaLogica
{
    public class SistemaOperacional
    {
        // Gerenciador de processos (adicionar, remover, listar)
        public readonly GerenciadorProcessos GerenciadorProcessos = new GerenciadorProcessos();

        // Gerenciador de memória, aqui inicializado com 5000 blocos/endereço
        public readonly GerenciadorMemoria GerenciadorMemoria = new GerenciadorMemoria(5000);

        // Escalonador de processos
        public readonly Escalonador Escalonador;

        public SistemaOperacional()
        {
            Escalonador = new Escalonador(GerenciadorProcessos);
        }

        public void CarregarArquivo(string caminho)
        {
            // Lê todas as linhas do arquivo
            var linhas = File.ReadAllLines(caminho);
            foreach (var linha in linhas)
            {
                // Ignora comentários (#) e linhas vazias
                if (linha.StartsWith("#") || string.IsNullOrWhiteSpace(linha)) continue;

                // Divide a linha pelos ; para extrair os dados
                var partes = linha.Split(';');
                string pId = partes[0];
                string tId = partes[1];
                int tempo = int.Parse(partes[2]);
                int endereco = int.Parse(partes[3]);
                int prioridade = int.Parse(partes[4]);

                // Procura se o processo já existe na lista
                var processo = GerenciadorProcessos.Processos.Find(p => p.ProcessoId == pId);
                if (processo == null)
                {
                    // Se não existe, cria um novo processo e adiciona ao gerenciador
                    processo = new Processo { ProcessoId = pId };
                    GerenciadorProcessos.Adicionar(processo);
                }

                // Adiciona a thread ao processo
                processo.Threads.Add(new MyThread
                {
                    ThreadId = tId,
                    TempoExecucao = tempo,
                    EnderecoMemoria = endereco,
                    Prioridade = prioridade
                });
            }

            // Após carregar todos os processos, aloca memória para cada um
            foreach (var p in GerenciadorProcessos.Processos)
                GerenciadorMemoria.Alocar(p);
        }
    }
}

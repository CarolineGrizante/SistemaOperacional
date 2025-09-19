namespace SistemaLogica
{
    public class MyThread
    {
        // ID da thread
        public string ThreadId { get; set; } = string.Empty;

        // Tempo de execução da thread
        public int TempoExecucao { get; set; }

        // Endereço de memória onde a thread está alocada
        public int EnderecoMemoria { get; set; }

        // Prioridade da thread (quanto menor o número, maior a prioridade)
        public int Prioridade { get; set; }

        public override string ToString()
        {
            return $"Thread {ThreadId} | Tempo: {TempoExecucao} | Endereço: {EnderecoMemoria} | Prioridade: {Prioridade}";
        }
    }
}




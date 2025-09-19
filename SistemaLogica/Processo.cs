namespace SistemaLogica
{
    public class Processo
    {
        // ID do processo (ex: P1, P2, etc.)
        public string ProcessoId { get; set; } = string.Empty;

        // Lista de threads associadas a este processo
        public List<MyThread> Threads { get; set; } = new List<MyThread>();

        public override string ToString()
        {
            // Retorna uma string representando o processo e a quantidade de threads que ele possui
            return $"Processo {ProcessoId} - {Threads.Count} threads";
        }
    }
}

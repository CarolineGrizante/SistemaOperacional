namespace SistemaLogica
{
    // Classe auxiliar para Escalonador.cs
    public class ThreadExecucaoRR
    {
        // Ela guardará o estado de cada thread durante a execução do RR
        // Permtindo que o Escalonador saiba quanto tempo resta para cada thread ser executada 
        public string ProcessoId { get; set; }
        public string ThreadId { get; set; }
        public int TempoRestante { get; set; }
    }
}

namespace SistemaLogica
{
    public class Memoria
    {
        // Endereço da memória (ex: posição 1000)
        public int Endereco { get; set; }

        // ID do processo que está ocupando o endereço de memória
        public string ProcessoId { get; set; }

        // Identificador da thread que está usando o endereço
        public string ThreadId { get; set; }
    }
}

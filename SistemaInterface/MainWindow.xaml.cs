using SistemaLogica;
using System;
using System.IO;
using System.Windows;

namespace SimuladorSO
{
    public partial class MainWindow : Window
    {
        private readonly SistemaOperacional so;

        public MainWindow()
        {
            InitializeComponent();
            so = new SistemaOperacional();

            // Carrega processos do arquivo na inicialização
            CarregarProcessosDoArquivo();

            txtSaida.Text += "Simulador de Sistema Operacional iniciado!\n";
        }

        private void CarregarProcessosDoArquivo()
        {
            string caminho = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "processos.txt");

            if (!File.Exists(caminho))
            {
                txtSaida.Text += $"Arquivo de processos '{caminho}' não encontrado.\n";
                return;
            }

            try
            {
                // Usa o método centralizado em SistemaOperacional para carregar e alocar
                so.CarregarArquivo(caminho);
                txtSaida.Text += "Processos carregados com sucesso do arquivo.\n";
            }
            catch (Exception ex)
            {
                txtSaida.Text += $"Erro ao carregar o arquivo de processos: {ex.Message}\n";
            }
        }

        private void btnVerProcessos_Click(object sender, RoutedEventArgs e)
        {
            txtSaida.Text += "\n--- Processos e Threads ---\n";
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            so.GerenciadorProcessos.Listar();
            txtSaida.Text += sw.ToString();
            ResetConsoleOutput();
        }

        private void btnAdicionarProcesso_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string processoId = txtProcessoIdAdd.Text;
                if (string.IsNullOrWhiteSpace(processoId))
                {
                    throw new Exception("O ID do Processo é obrigatório.");
                }

                // Busca o processo, se não existir, cria um novo
                var processo = so.GerenciadorProcessos.Buscar(processoId);
                if (processo == null)
                {
                    processo = new Processo { ProcessoId = processoId };
                    so.GerenciadorProcessos.Adicionar(processo);
                }

                // Cria a nova thread com os dados da UI
                var novaThread = new MyThread
                {
                    ThreadId = txtThreadIdAdd.Text,
                    TempoExecucao = int.Parse(txtTempoExecucaoAdd.Text),
                    EnderecoMemoria = int.Parse(txtEnderecoMemoriaAdd.Text),
                    Prioridade = int.Parse(txtPrioridadeAdd.Text)
                };

                // Adiciona a thread ao processo e aloca sua memória
                processo.Threads.Add(novaThread);
                so.GerenciadorMemoria.Alocar(processo); // O método Alocar já previne duplicatas

                txtSaida.Text += $"\nThread {novaThread.ThreadId} adicionada ao Processo {processo.ProcessoId} com sucesso.\n";

                // Limpa os campos de texto
                txtProcessoIdAdd.Clear();
                txtThreadIdAdd.Clear();
                txtTempoExecucaoAdd.Clear();
                txtEnderecoMemoriaAdd.Clear();
                txtPrioridadeAdd.Clear();
            }
            catch (FormatException)
            {
                txtSaida.Text += "\nErro ao adicionar: verifique se os campos numéricos (Tempo, Endereço, Prioridade) contêm apenas números.\n";
            }
            catch (Exception ex)
            {
                txtSaida.Text += $"\nErro ao adicionar processo: {ex.Message}\n";
            }
        }

        private void btnRemoverProcesso_Click(object sender, RoutedEventArgs e)
        {
            string id = txtProcessoIdRemover.Text;
            if (string.IsNullOrWhiteSpace(id))
            {
                txtSaida.Text += "\nPor favor, insira um ID de processo para remover.\n";
                return;
            }

            // Desaloca a memória associada ao processo
            so.GerenciadorMemoria.Desalocar(id);

            // Remover o processo da lista de processos
            so.GerenciadorProcessos.Remover(id);

            txtSaida.Text += $"\nProcesso {id} e suas alocações de memória foram removidos (se existiam).\n";
            txtProcessoIdRemover.Clear();
        }

        private void btnFCFS_Click(object sender, RoutedEventArgs e)
        {
            txtSaida.Text += "\n--- Escalonamento FCFS ---\n";
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            so.Escalonador.FCFS();
            txtSaida.Text += sw.ToString();
            ResetConsoleOutput();
        }

        private void btnSJF_Click(object sender, RoutedEventArgs e)
        {
            txtSaida.Text += "\n--- Escalonamento SJF ---\n";
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            so.Escalonador.SJF();
            txtSaida.Text += sw.ToString();
            ResetConsoleOutput();
        }

        private void btnRR_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                int quantum = int.Parse(txtQuantumRR.Text);
                txtSaida.Text += $"\n--- Escalonamento RR (Quantum = {quantum}) ---\n";
                StringWriter sw = new StringWriter();
                Console.SetOut(sw);
                so.Escalonador.RR(quantum);
                txtSaida.Text += sw.ToString();
                ResetConsoleOutput();
            }
            catch (FormatException)
            {
                txtSaida.Text += "\nErro: O valor do Quantum deve ser um número inteiro.\n";
            }
            catch (Exception ex)
            {
                txtSaida.Text += $"\nErro no Escalonamento RR: {ex.Message}\n";
            }
        }

        private void btnAlocacaoMemoria_Click(object sender, RoutedEventArgs e)
        {
            txtSaida.Text += "\n--- Alocação de Memória ---\n";
            StringWriter sw = new StringWriter();
            Console.SetOut(sw);
            so.GerenciadorMemoria.Mostrar();
            txtSaida.Text += sw.ToString();
            ResetConsoleOutput();
        }

        private void btnTempoExecucao_Click(object sender, RoutedEventArgs e)
        {
            txtSaida.Text += "\n--- Tempo de Execução Total ---\n";
            int tempoTotal = so.GerenciadorProcessos.TempoExecucaoTotal();
            txtSaida.Text += $"Tempo total de todas as threads: {tempoTotal}\n";
        }

        private void ResetConsoleOutput()
        {
            var standardOutput = new StreamWriter(Console.OpenStandardOutput())
            {
                AutoFlush = true
            };
            Console.SetOut(standardOutput);
        }

    }
}

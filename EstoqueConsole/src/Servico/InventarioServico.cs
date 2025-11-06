using System.Text;
using EstoqueConsole.src.Modelo;

namespace EstoqueConsole.src.Servico
{
    public class Uteis
    {
        public static string _Path() // Pega o caminho do arquivo.csv independente do computador
        {
            // Tratamento do Path
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;
            string path = Path.Combine(projectDir, "data\\produtos.csv");

            return path;
        }

        // Lista os produtos dentro do .csv
        public static List<Produto> ListarProduto()
        {
            var produtos = new List<Produto>();
            foreach (var line in File.ReadAllLines(_Path(), Encoding.UTF8).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue;
                var p = line.Split(';');
                produtos.Add(new Produto(
                    Id: int.Parse(p[0]),
                    Nome: p[1],
                    Categoria: p[2],
                    EstoqueMinimo: int.Parse(p[3]),
                    Saldo: int.Parse(p[4])
                ));
            }
            return produtos;
        }

        // Cria produto 
        public static Produto CriarProduto()
        {
            Console.Write("Nome do produto: ");
            var nome = Console.ReadLine();

            Console.Write("Categoria do produto: ");
            var categoria = Console.ReadLine();

            int estoqueMinimo;
            while (true)
            {
                Console.Write("Estoque Mínimo do produto: ");
                estoqueMinimo = int.Parse(Console.ReadLine());
                if (estoqueMinimo < 0)
                {
                    Console.WriteLine("Estoque Mínimo não pode ser menor que zero!");
                    continue;
                }
                break;
            }

            Console.Write("Saldo do produto: ");
            var saldo = int.Parse(Console.ReadLine());

            // Criando obj Produto para retornar
            var produto = new Produto(
                Id: 0,
                Nome: nome,
                Categoria: categoria,
                EstoqueMinimo: estoqueMinimo,
                Saldo: saldo
            );
            return produto;
        }
    }
}

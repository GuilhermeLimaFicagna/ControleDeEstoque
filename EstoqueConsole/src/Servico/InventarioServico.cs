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
        public static void RemoverProdutoRAM(List<Produto> produtos)
        {
            if (!produtos.Any())
            {
                Console.WriteLine("Nenhum produto cadastrado para excluir.");
                return;
            }

            Console.WriteLine("\n ** Produtos disponíveis para exclusão **");
            ProdutosFormatados(produtos); 

            Console.Write("\nDigite o ID do produto que deseja excluir: ");
            if (!int.TryParse(Console.ReadLine(), out int idExclusao))
            {
                Console.WriteLine("ID inválido!");
                return;
            }
            var produtoRemocao = produtos.FirstOrDefault(p => p.Id == idExclusao);
            if (produtoRemocao == null)
            {
                Console.WriteLine("Produto não encontrado!");
                return;
            }

            Console.Write($"Tem certeza que deseja excluir o produto \"{produtoRemocao.Nome}\"? (s/n): ");
            var confirmacaoUsuario = Console.ReadLine();

            if (confirmacaoUsuario?.ToLower() == "s")
            {
                produtos.Remove(produtoRemocao);
                Console.WriteLine($"Produto \"{produtoRemocao.Nome}\" removido.");
            }
            else
            {
                Console.WriteLine("Exclusão cancelada.");
            }
        }
        public static void ProdutosFormatados(List<Produto> produtos)
        {
            if (!produtos.Any())
            {
                Console.WriteLine("Nenhum produto cadastrado.");
                return;
            }

            int idWidth = Math.Max(2, produtos.Max(p => p.Id.ToString().Length));
            int nomeWidth = Math.Max(4, produtos.Max(p => p.Nome.Length));
            int categoriaWidth = Math.Max(9, produtos.Max(p => p.Categoria.Length));
            int estoqueMinWidth = Math.Max(15, produtos.Max(p => p.EstoqueMinimo.ToString().Length + 2));
            int saldoWidth = Math.Max(5, produtos.Max(p => p.Saldo.ToString().Length));

            string cabecalho =
                "ID".PadRight(idWidth + 2) +
                "NOME".PadRight(nomeWidth + 2) +
                "CATEGORIA".PadRight(categoriaWidth + 2) +
                "ESTOQUE MÍNIMO".PadRight(estoqueMinWidth + 2) +
                "SALDO".PadRight(saldoWidth + 2);

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(cabecalho);
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('─', cabecalho.Length));
            Console.ResetColor();

            foreach (var produto in produtos)
            {
                if (produto.Id % 2 == 0)
                    Console.ForegroundColor = ConsoleColor.Gray;
                else
                    Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine(
                    produto.Id.ToString().PadRight(idWidth + 2) +
                    produto.Nome.PadRight(nomeWidth + 2) +
                    produto.Categoria.PadRight(categoriaWidth + 2) +
                    produto.EstoqueMinimo.ToString().PadRight(estoqueMinWidth + 2) +
                    produto.Saldo.ToString().PadRight(saldoWidth + 2));
                Console.WriteLine();
            }
            Console.ResetColor();
        }
    }
}

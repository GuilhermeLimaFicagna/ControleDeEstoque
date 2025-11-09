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

        public static void EditarProduto()
        {
            var produtos = ListarProduto();
            if (produtos.Count == 0)
            {
                Console.WriteLine("Nenhum produto cadastrado!");
                return;
            }

            Console.WriteLine("\n--- Lista de Produtos ---");
            foreach (var p in produtos)
                Console.WriteLine($"ID: {p.Id} | Nome: {p.Nome} | Categoria: {p.Categoria} | Estoque Mínimo: {p.EstoqueMinimo} | Saldo: {p.Saldo}");

            Console.Write("\nDigite o ID do produto que deseja editar: ");
            if (!int.TryParse(Console.ReadLine(), out int idEdicao))
            {
                Console.WriteLine("ID inválido!");
                return;
            }

            var produtoEdicao = produtos.FirstOrDefault(p => p.Id == idEdicao);
            if (produtoEdicao == null)
            {
                Console.WriteLine("Produto não encontrado!");
                return;
            }

            Console.WriteLine($"\nEditando Produto: {produtoEdicao.Nome}");

            Console.Write("Novo Nome (vazio mantém): ");
            string novoNome = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoNome))
                produtoEdicao.Nome = novoNome;

            Console.Write("Nova Categoria (vazio mantém): ");
            string novaCategoria = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novaCategoria))
                produtoEdicao.Categoria = novaCategoria;

            Console.Write("Novo Estoque Mínimo (vazio mantém): ");
            string novoEstoque = Console.ReadLine();
            if (int.TryParse(novoEstoque, out int estoqueNovo))
                produtoEdicao.EstoqueMinimo = estoqueNovo;

            Console.Write("Novo Saldo (vazio mantém): ");
            string novoSaldo = Console.ReadLine();
            if (int.TryParse(novoSaldo, out int saldoNovo))
                produtoEdicao.Saldo = saldoNovo;

            var linhas = new List<string> { "Id;Nome;Categoria;EstoqueMinimo;Saldo" };
            foreach (var p in produtos)
                linhas.Add($"{p.Id};{p.Nome};{p.Categoria};{p.EstoqueMinimo};{p.Saldo}");

            File.WriteAllLines(_Path(), linhas, Encoding.UTF8);

            Console.WriteLine("\nProduto atualizado com sucesso!");
        }

        public static void ExcluirProduto()
        {
            var produtos = ListarProduto();
            if (produtos.Count == 0)
            {
                Console.WriteLine("Nenhum produto cadastrado!");
                return;
            }

            Console.WriteLine("\n--- Lista de Produtos ---");
            foreach (var p in produtos)
            {
                Console.WriteLine($"ID: {p.Id} | Nome: {p.Nome} | Categoria: {p.Categoria} | Estoque Mínimo: {p.EstoqueMinimo} | Saldo: {p.Saldo}");
            }

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
            var confirmacao = Console.ReadLine();
            if (confirmacao?.ToLower() != "s")
            {
                Console.WriteLine("Exclusão cancelada.");
                return;
            }

            produtos.Remove(produtoRemocao);

            var linhas = new List<string> { "Id;Nome;Categoria;EstoqueMinimo;Saldo" };
            foreach (var p in produtos)
                linhas.Add($"{p.Id};{p.Nome};{p.Categoria};{p.EstoqueMinimo};{p.Saldo}");

            Console.WriteLine("Produto excluído com sucesso!");
            File.WriteAllLines(_Path(), linhas, Encoding.UTF8);
        }

    }
}

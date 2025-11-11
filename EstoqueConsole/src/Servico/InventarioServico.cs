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

        // Lista os Produtos dentro do .csv
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

        // Cria Produto 
        public static Produto CriarProduto()
        {
            // Nome
            string nome = "";
            while (string.IsNullOrWhiteSpace(nome))
            {
                Console.Write("Nome do produto: ");
                nome = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(nome))
                    Console.WriteLine("O nome não pode ser vazio!\n");
            }

            // Categoria
            string categoria = "";
            while (string.IsNullOrWhiteSpace(categoria))
            {
                Console.Write("Nome da Categoria: ");
                categoria = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(categoria))
                    Console.WriteLine("A categoria não pode ser vazia!\n");
            }

            // Estoque minimo
            int estoqueMinimo;
            while (true)
            {
                Console.Write("Estoque Mínimo do produto: ");
                if (!int.TryParse(Console.ReadLine(), out estoqueMinimo))
                {
                    Console.WriteLine("Digite um número válido!\n");
                    continue;
                }
                if (estoqueMinimo < 0)
                {
                    Console.WriteLine("Estoque Mínimo não pode ser menor que zero!");
                    continue;
                }
                break;
            }

            // Saldo
            int saldo;
            while (true)
            {
                Console.Write("Novo Saldo: ");
                if (int.TryParse(Console.ReadLine(), out saldo))
                    break;

                Console.WriteLine("Digite um número válido!\n");
            }

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
        // Edita Produto
        public static void EditarProduto(List<Produto> produtos)
        {
            Console.WriteLine("\n ** Produtos disponíveis para editar **");
            ProdutosFormatados(produtos);

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

            //Salva o Id do Produto para não substituí-lo
            int salvarId = produtoEdicao.Id;

            //Remove o antifgo produto para substituição
            produtos.Remove(produtoEdicao);

            Console.WriteLine($"\nEditando Produto: {produtoEdicao.Nome}");

            //Novo Nome
            string novoNome = "";
            while (string.IsNullOrWhiteSpace(novoNome))
            {
                Console.Write("Novo Nome: ");
                novoNome = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(novoNome))
                    Console.WriteLine("O nome não pode ser vazio!\n");
            }

            //Nova Categoria
            string novaCategoria = "";
            while (string.IsNullOrWhiteSpace(novaCategoria))
            {
                Console.Write("Nova Categoria: ");
                novaCategoria = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(novaCategoria))
                    Console.WriteLine("A categoria não pode ser vazia!\n");
            }

            //Novo Estoque
            int novoEstoque;
            while (true)
            {
                Console.Write("Novo Estoque Mínimo: ");
                if (!int.TryParse(Console.ReadLine(), out novoEstoque))
                {
                    Console.WriteLine("Digite um número válido!\n");
                    continue;
                }
                if (novoEstoque < 0)
                {
                    Console.WriteLine("Estoque Mínimo não pode ser menor que zero!");
                    continue;
                }
                break;
            }

            //Novo Saldo
            int novoSaldo;
            while (true)
            {
                Console.Write("Novo Saldo: ");
                if (int.TryParse(Console.ReadLine(), out novoSaldo))
                    break;

                Console.WriteLine("Digite um número válido!\n");
            }

            // Criando o novo produto
            var produto = new Produto(
                Id: salvarId,
                Nome: novoNome,
                Categoria: novaCategoria,
                EstoqueMinimo: novoEstoque,
                Saldo: novoSaldo
            );

            produtos.Add(produto);

            Console.WriteLine("\nProduto atualizado com sucesso!");
        }
        // Remove Produto
        public static void RemoverProduto(List<Produto> produtos)
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
        // Formatação para exibir Produtos na tela 
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
        // Salvar Produtos em Ram no produtos.csv
        public static void SalvarProdutos(List<Produto> produtos)
        {
            try
            {
                var stringb = new StringBuilder();

                stringb.AppendLine("ID;Nome;Categoria;EstoqueMinimo;Saldo");

                foreach (var p in produtos)
                {
                    stringb.AppendLine($"{p.Id};{p.Nome};{p.Categoria};{p.EstoqueMinimo};{p.Saldo}");
                }
                File.WriteAllText(_Path(), stringb.ToString(), Encoding.UTF8);

                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"Os dados foram salvos com sucesso em: {_Path()}");
                Console.ResetColor();
            }
            catch (Exception ex)
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine($"Ocorreu um erro ao salvar o arquivo: {ex.Message}");
                Console.ResetColor();
            }
        }
    }
}

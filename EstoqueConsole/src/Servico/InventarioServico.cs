using System.Text;
using EstoqueConsole.src.Modelo;

namespace EstoqueConsole.src.Servico
{
    public class Uteis
    {
        // =========== MÉTODOS PRODUTO ============  //
        public static string _Path() // Pega o caminho do produtos.csv independente do computador
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
                if (string.IsNullOrWhiteSpace(line)) continue; // Verificação
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
                if (estoqueMinimo < 0) // Regra de Negócio
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

            if (!produtos.Any())
            {
                Console.WriteLine("Nenhum produto cadastrado.");
                return;
            }

            ProdutosFormatados(produtos);

            Console.Write("\nDigite o ID do produto que deseja editar: ");
            if (!int.TryParse(Console.ReadLine(), out int idEdicao))
            {
                Console.WriteLine("ID inválido!");
                return;
            }

            var produtoEdicao = produtos.FirstOrDefault(p => p.Id == idEdicao);
            if (produtoEdicao == null || !produtos.Any(p => p.Id == idEdicao))
            {
                Console.WriteLine("Produto não encontrado!");
                return;
            }

            //Salva o Id do Produto para não substituí-lo
            int salvarId = produtoEdicao.Id;

            //Remove o antigo produto para substituição
            produtos.Remove(produtoEdicao);

            Console.WriteLine($"\nEditando Produto: {produtoEdicao.Nome}");

            //Novo Nome
            string novoNome = "";
            while (string.IsNullOrWhiteSpace(novoNome))
            {
                Console.Write("Novo Nome: ");
                novoNome = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(novoNome))
                {
                    Console.WriteLine("O nome não pode ser vazio!\n");
                }
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
                if (novoEstoque < 0) // Regra de Negócio
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
                {
                    break;
                }

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
            if (produtoRemocao == null || !produtos.Any(p => p.Id == idExclusao))
            {
                Console.WriteLine("Produto não encontrado!");
                return;
            }

            // Regra de Negócio
            int verificarSaldo = produtoRemocao.Saldo;

            if (verificarSaldo < 0)
            {
                Console.WriteLine("\nNão é possível remover um produto com saldo negativo!");
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

        // =========== MÉTODOS MOVIMENTO ============  //
        public static string _PathMovimento() // Pega o caminho do movimentos.csv independente do computador
        {
            // Tratamento do Path
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;
            string path = Path.Combine(projectDir, "data\\movimentos.csv");

            return path;
        }

        // Lista os Movimentos dentro do .csv
        public static List<Movimento> ListarMovimentos()
        {
            var movimentos = new List<Movimento>();
            foreach (var line in File.ReadAllLines(_PathMovimento(), Encoding.UTF8).Skip(1))
            {
                if (string.IsNullOrWhiteSpace(line)) continue; // verificação
                var m = line.Split(';');
                movimentos.Add(new Movimento(
                    Id: int.Parse(m[0]),
                    ProdutoId: int.Parse(m[1]),
                    Tipo: m[2],
                    Quantidade: int.Parse(m[3]),
                    Data: DateTime.Parse(m[4]),
                    Observacao: m[5]
                ));
            }
            return movimentos;
        }
        // Dando entrada em produto e adicionando em movimento
        public static void EntradaProduto(List<Produto> produtos, List<Movimento> movimentos)
        {
            Console.WriteLine("\n ** Produtos disponíveis para dar entrada **");

            if (!produtos.Any())
            {
                Console.WriteLine("Nenhum produto cadastrado.");
                return;
            }

            ProdutosFormatados(produtos);

            Console.Write("\nDigite o ID do produto que deseja dar entrada: ");
            if (!int.TryParse(Console.ReadLine(), out int idEntrada))
            {
                Console.WriteLine("ID inválido!");
                return;
            }

            var produtoEntrada = produtos.FirstOrDefault(p => p.Id == idEntrada);
            if (produtoEntrada == null || !produtos.Any(p => p.Id == idEntrada))
            {
                Console.WriteLine("Produto não encontrado!");
                return;
            }

            // Dando entrada no saldo
            Console.WriteLine($"Saldo atual do produto {produtoEntrada.Nome}: {produtoEntrada.Saldo}");
            int entradaSaldo;
            Produto produtoAtualizado; // Armazenando item atualizado para pegar id depois.
            while (true)
            {
                Console.Write($"Digite o valor de entrada de {produtoEntrada.Nome}: ");
                if (int.TryParse(Console.ReadLine(), out entradaSaldo))
                {
                    int indiceProduto = produtos.FindIndex(p => p.Id == idEntrada); // Encontrando índice na lista de Produtos

                    // Criando nova instância para atualizar
                    produtoAtualizado = produtoEntrada with { Saldo = produtoEntrada.Saldo + entradaSaldo };
                    produtos[indiceProduto] = produtoAtualizado;

                    Console.WriteLine($"Saldo de {produtoEntrada.Nome} atualizado para: {produtoAtualizado.Saldo}");
                    break;
                }

                Console.WriteLine("Digite um número válido!\n");
            }

            // Adicionando movimento a variável movimentos

            int NextIdM() => movimentos.Any() ? movimentos.Max(c => c.Id) + 1 : 1; // adicionando id do movimento
            movimentos.Add(new Movimento(
                Id: NextIdM(),
                ProdutoId: produtoAtualizado.Id,
                Tipo: "ENTRADA",
                Quantidade: entradaSaldo,
                Data: DateTime.Now,
                Observacao: "Entrada em estoque"
            ));
        }
        // Dando saída em produto e adicionando em movimento
        public static void SaidaProduto(List<Produto> produtos, List<Movimento> movimentos)
        {
            Console.WriteLine("\n ** Produtos disponíveis para dar Saída **");

            if (!produtos.Any())
            {
                Console.WriteLine("Nenhum produto cadastrado.");
                return;
            }

            ProdutosFormatados(produtos);

            Console.Write("\nDigite o ID do produto que deseja dar Saída: ");
            if (!int.TryParse(Console.ReadLine(), out int idEntrada))
            {
                Console.WriteLine("ID inválido!");
                return;
            }

            var produtoEntrada = produtos.FirstOrDefault(p => p.Id == idEntrada);
            if (produtoEntrada == null || !produtos.Any(p => p.Id == idEntrada))
            {
                Console.WriteLine("Produto não encontrado!");
                return;
            }

            // Dando saída no saldo
            Console.WriteLine($"Saldo atual do produto {produtoEntrada.Nome}: {produtoEntrada.Saldo}");
            int entradaSaldo;
            Produto produtoAtualizado; // Armazenando item atualizado para pegar id depois.
            while (true)
            {
                Console.Write($"Digite o valor de saída de {produtoEntrada.Nome}: ");
                if (int.TryParse(Console.ReadLine(), out entradaSaldo))
                {
                    // Regra de negócio
                    if (produtoEntrada.Saldo < entradaSaldo)
                    {
                        Console.WriteLine("Quantidade maior que o saldo !!!");
                        continue;
                    }
                    int indiceProduto = produtos.FindIndex(p => p.Id == idEntrada); // Encontrando índice na lista de Produtos

                    // Criando nova instância para atualizar
                    produtoAtualizado = produtoEntrada with { Saldo = produtoEntrada.Saldo - entradaSaldo };
                    produtos[indiceProduto] = produtoAtualizado;

                    Console.WriteLine($"Saldo de {produtoEntrada.Nome} atualizado para: {produtoAtualizado.Saldo}");
                    break;
                }

                Console.WriteLine("Digite um número válido!\n");
            }

            // Adicionando movimento a variável movimentos
            int NextIdM() => movimentos.Any() ? movimentos.Max(c => c.Id) + 1 : 1; // adicionando id do movimento
            movimentos.Add(new Movimento(
                Id: NextIdM(),
                ProdutoId: produtoAtualizado.Id,
                Tipo: "SAIDA",
                Quantidade: entradaSaldo,
                Data: DateTime.Now,
                Observacao: "Saída do estoque"
            ));
        }

        // =========== MÉTODOS EXTRATO ============  //

        //Mostrando Extrato (Produto Específico ou Geral)
        public static void ExtratoProduto(int produtoId, List<Produto> produtos, List<Movimento> movimentos)
        {
            Console.WriteLine("\n===== EXTRATO =====\n");

            if (!produtos.Any())
            {
                Console.WriteLine("Nenhum produto cadastrado!\n");
                return;
            }

            // Se produtoId == 0, mostrar submenu
            if (produtoId == 0)
            {
                Console.WriteLine("1. Extrato de um produto específico");
                Console.WriteLine("2. Extrato geral (todos os produtos)");
                Console.WriteLine("0. Voltar");
                Console.Write("Opção: ");
                string opc = Console.ReadLine();

                if (opc == "0")
                    return;

                if (opc == "1")
                {
                    Console.WriteLine();
                    ProdutosFormatados(produtos);
                    Console.Write("\nDigite o ID do produto: ");
                    if (!int.TryParse(Console.ReadLine(), out produtoId))
                    {
                        Console.WriteLine("ID inválido!");
                        return;
                    }
                }
                else if (opc == "2")
                {
                    Console.WriteLine();
                    MostrarExtratoGeral(produtos, movimentos);
                    return;
                }
                else
                {
                    Console.WriteLine("Opção inválida!");
                    return;
                }
            }

            // Id como parâmetro do método
            var produto = produtos.FirstOrDefault(p => p.Id == produtoId);

            // Verifica se existe
            if (produto.Id == 0 || string.IsNullOrWhiteSpace(produto.Nome))
            {
                Console.WriteLine("Produto não encontrado!\n");
                return;
            }

            // Filtra movimentos do produto pela data
            var ordemMovimento = movimentos
                .Where(m => m.ProdutoId == produtoId)
                .OrderBy(m => m.Data)
                .ToList();

            if (!ordemMovimento.Any())
            {
                Console.WriteLine("Nenhum movimento encontrado para este produto.\n");
                return;
            }

            //Printa os dados do produto
            Console.WriteLine($"\nProduto: {produto.Nome}");
            Console.WriteLine($"Categoria: {produto.Categoria}");
            Console.WriteLine($"Saldo atual: {produto.Saldo}\n");

            // Ajuste das colunas e cores igual ao método da formatação
            int dataWidth = 20;
            int idWidth = Math.Max(2, ordemMovimento.Max(m => m.Tipo.Length));
            int prodWidth = Math.Max(5, ordemMovimento.Max(m => m.Tipo.Length));
            int tipoWidth = Math.Max(5, ordemMovimento.Max(m => m.Tipo.Length));
            int qtdWidth = Math.Max(3, ordemMovimento.Max(m => m.Quantidade.ToString().Length));
            int obsWidth = Math.Max(10, ordemMovimento.Max(m => m.Observacao.Length));

            string cabecalho =
                "DATA".PadRight(dataWidth + 2) +
                "ID".PadRight(prodWidth + 2) +
                "PRODUTO".PadRight(prodWidth + 5) +
                "TIPO".PadRight(tipoWidth + 2) +
                "QTD".PadRight(qtdWidth + 2) +
                "OBSERVAÇÃO".PadRight(obsWidth + 2);

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(cabecalho);
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('─', cabecalho.Length));
            Console.ResetColor();

            string nomeProduto = produto.Nome ?? "Desconhecido";

            foreach (var mov in ordemMovimento)
            {
                string linhaTexto =
                    mov.Data.ToString("dd/MM/yyyy HH:mm").PadRight(dataWidth + 2) +
                    mov.ProdutoId.ToString().PadRight(idWidth + 2) +
                    nomeProduto.PadRight(tipoWidth + 5) +
                    mov.Tipo.PadRight(tipoWidth + 2) +
                    mov.Quantidade.ToString().PadRight(qtdWidth + 2) +
                    mov.Observacao.PadRight(obsWidth + 2);

                // Cor para Entrada
                if (mov.Tipo == "ENTRADA")
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                // Cor para Saída
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                // Escreve texto colorido
                Console.Write(linhaTexto);

                // Resetar cor
                Console.ResetColor();

                // Completa o resto da linha com espaços
                int restante = Console.WindowWidth - linhaTexto.Length;
                if (restante > 0)
                {
                    Console.Write(new string(' ', restante));
                }

                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine("\n===== FIM DO EXTRATO =====\n");
        }

        //Método de suporte para mostrar todo extrato (extrato geral)
        private static void MostrarExtratoGeral(List<Produto> produtos, List<Movimento> movimentos)
        {
            Console.WriteLine("\n===== EXTRATO GERAL =====\n");

            // Ordena todos por data
            var ordem = movimentos.OrderBy(m => m.Data).ToList();

            if (!ordem.Any())
            {
                Console.WriteLine("Nenhum movimento encontrado!\n");
                return;
            }

            // Ajuste das colunas com base em TODOS os movimentos
            int dataWidth = 20;
            int tipoWidth = Math.Max(5, ordem.Max(m => m.Tipo.Length));
            int qtdWidth = Math.Max(3, ordem.Max(m => m.Quantidade.ToString().Length));
            int obsWidth = Math.Max(10, ordem.Max(m => m.Observacao.Length));
            int prodWidth = Math.Max(10, produtos.Max(p => p.Nome.Length));

            string cabecalho =
                "DATA".PadRight(dataWidth + 1) +
                "ID".PadRight(prodWidth + 2) +
                "PRODUTO".PadRight(prodWidth + 5) +
                "TIPO".PadRight(tipoWidth + 3) +
                "QTD".PadRight(qtdWidth + 3) +
                "OBSERVAÇÃO".PadRight(obsWidth + 3);

            Console.BackgroundColor = ConsoleColor.Yellow;
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(cabecalho);
            Console.ResetColor();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine(new string('─', cabecalho.Length));
            Console.ResetColor();

            // Percorre todos os movimentos
            foreach (var mov in ordem)
            {
                var produto = produtos.FirstOrDefault(p => p.Id == mov.ProdutoId);
                // Caso haja um produto que ele não reconheça (foi excluído) aparece "Desconhecido"
                string nomeProduto = produto.Nome ?? "Desconhecido";

                string linhaTexto =
                    mov.Data.ToString("dd/MM/yyyy HH:mm").PadRight(dataWidth + 1) +
                    mov.ProdutoId.ToString().PadRight(prodWidth + 2) +
                    nomeProduto.PadRight(prodWidth + 5) +
                    mov.Tipo.PadRight(tipoWidth + 3) +
                    mov.Quantidade.ToString().PadRight(qtdWidth + 3) +
                    mov.Observacao.PadRight(obsWidth + 3);

                // Cor para Entrada
                if (mov.Tipo == "ENTRADA")
                {
                    Console.BackgroundColor = ConsoleColor.Green;
                    Console.ForegroundColor = ConsoleColor.Black;
                }
                // Cor para Saída
                else
                {
                    Console.BackgroundColor = ConsoleColor.Red;
                    Console.ForegroundColor = ConsoleColor.White;
                }

                // Escreve texto colorido
                Console.Write(linhaTexto);

                Console.ResetColor();

                // Preenche o restante da linha
                int restante = Console.WindowWidth - linhaTexto.Length;
                if (restante > 0)
                {
                    Console.Write(new string(' ', restante));
                }

                Console.WriteLine();
                Console.WriteLine();
            }

            Console.ResetColor();
            Console.WriteLine("\n===== FIM DO EXTRATO GERAL =====\n");
        }

        // =========== MÉTODOS ABAIXO MÍNIMO ============  //
        public static void AbaixoMinimo(List<Produto> produtos)
        {
            Console.WriteLine("\n===== PRODUTOS ABAIXO DO MINIMO =====\n");
            var produtosAbaixoMinimo = produtos.Where(p => p.Saldo < p.EstoqueMinimo).ToList();
            if (!produtosAbaixoMinimo.Any())
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("Nenhum produto está com saldo inferior ao mínimo!");
                Console.ResetColor();
                return;
            }
            ProdutosFormatados(produtosAbaixoMinimo);
        }
    }
}

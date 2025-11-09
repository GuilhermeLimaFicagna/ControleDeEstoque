using EstoqueConsole.src.Modelo;
using EstoqueConsole.src.Servico;

Console.WriteLine("Integrantes: ");
Console.WriteLine("Cleberson Cesar Bueno dos Santos - 2025105040");
Console.WriteLine("Guilherme de lima Ficagna - 2025105145");
Console.WriteLine("Eduardo Lopes Barros dos Santos - 2025105015");

// Produtos em RAM
int NextId()
{
    var listaAtual = Uteis.ListarProduto();
    return listaAtual.Any() ? listaAtual.Max(c => c.Id) + 1 : 1;
}

while (true)
{
    var produtos = Uteis.ListarProduto();

    Console.WriteLine("1. Listar produtos");
    Console.WriteLine("2. Cadastrar produto");
    Console.WriteLine("3. Editar produto");
    Console.WriteLine("4. Excluir produto");
    Console.WriteLine("5. Dar ENTRADA em estoque");
    Console.WriteLine("6. Dar SAÍDA de estoque");
    Console.WriteLine("7. Relatório: Estoque abaixo do mínimo");
    Console.WriteLine("8. Relatório: Extrato de movimentos por produto");
    Console.WriteLine("9. Salvar");
    Console.WriteLine("0. Sair");
    Console.Write("Opção: ");
    var op = Console.ReadLine();
    Console.WriteLine(); // Estética

    switch (op)
    {
        case "1":
            produtos = Uteis.ListarProduto();
            Console.WriteLine();

            if (!produtos.Any())
            {
                Console.WriteLine("Nenhum produto cadastrado.");
                break;
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
            }

            Console.ResetColor();
            Console.WriteLine();
            break;

        case "2":
            produtos = Uteis.ListarProduto();
            var novoProduto = Uteis.CriarProduto();
            novoProduto.Id = NextId(); // Definindo id

            // Adicionando a lista em RAM
            produtos.Add(novoProduto);

            File.WriteAllLines(
                Uteis._Path(),
                new[] { "Id;Nome;Categoria;EstoqueMinimo;Saldo" }
                .Concat(produtos.Select(p =>
                    $"{p.Id};{p.Nome};{p.Categoria};{p.EstoqueMinimo};{p.Saldo}"
                )), System.Text.Encoding.UTF8);

            Console.WriteLine($"Produto {novoProduto.Id} adicionado com sucesso !");
            Console.WriteLine();
            break;

        case "3":
            Uteis.EditarProduto();
            produtos = Uteis.ListarProduto();
            Console.WriteLine();
            break;

        case "4":
            Uteis.ExcluirProduto();
            produtos = Uteis.ListarProduto(); // recarrega lista atualizada
            Console.WriteLine();
            break;

        case "0":
            Console.WriteLine("Volte sempre");
            return;
        default:
            Console.WriteLine("Opção invalida !!!");
            break;
    }
}

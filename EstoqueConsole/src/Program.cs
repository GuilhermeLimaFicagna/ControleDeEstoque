using System.Text;
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
var produtos = Uteis.ListarProduto();

while (true)
{
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
            Uteis.ProdutosFormatados(produtos);
            Console.WriteLine();
            break;

        case "2":
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
            Uteis.EditarProduto(produtos);
            Console.WriteLine();
            break;

        case "4":
            Uteis.RemoverProduto(produtos);
            Console.WriteLine();
            break;

        case "9":
            Uteis.SalvarProdutos(produtos);
            Console.WriteLine();
            Console.WriteLine("\nDados salvos com sucesso!");
            break;

        case "0":
            Console.WriteLine("Volte sempre!");
            return;
        default:
            Console.WriteLine("Opção invalida!");
            break;
    }
}

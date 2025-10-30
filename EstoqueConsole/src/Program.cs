using EstoqueConsole.src.Modelo;
using EstoqueConsole.src.Servico;

Console.WriteLine("Integrantes: ");
Console.WriteLine("Cleberson Cesar Bueno dos Santos - 2025105040");
Console.WriteLine("Guilherme de lima Ficagna - 2025105145");
Console.WriteLine("Eduardo Lopes Barros dos Santos - 2025105015");

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

    switch (op)
    {
        case "1":
            Console.WriteLine("ID | NOME | CATEGORIA | ESTOQUE MÍNIMO | SALDO");
            var produtos = Uteis.ListarProduto();
            foreach (var produto in produtos)
            {
                Console.WriteLine($"{produto.Id} | {produto.Nome} | {produto.Categoria} | {produto.EstoqueMinimo} | {produto.Saldo} ");
            }
            break;
        case "2":
            Console.WriteLine("Em construção");
            break;
        default:
            Console.WriteLine("Em construção");
            break;
    }
    break;
}

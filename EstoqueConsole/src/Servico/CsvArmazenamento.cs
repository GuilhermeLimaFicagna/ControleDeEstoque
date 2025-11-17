using System.Text;
using EstoqueConsole.src.Modelo;

namespace EstoqueConsole.src.Servico
{
    public class Armazenamento
    {
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
                File.WriteAllText(Uteis._Path(), stringb.ToString(), Encoding.UTF8);

                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"Os dados foram salvos com sucesso em: {Uteis._Path()}");
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

        // Salvar Movimentos em Ram no movimentos.csv
        public static void SalvarMovimentos()
        {
            // Em construção
        }
    }
}

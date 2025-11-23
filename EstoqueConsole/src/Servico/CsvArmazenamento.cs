using System.IO;
using System.Text;
using EstoqueConsole.src.Modelo;

namespace EstoqueConsole.src.Servico
{
    public class Armazenamento
    {
        // Salvar Produtos em Ram no produtos.csv
        public static void SalvarProdutos(List<Produto> produtos)
        {
            // Variável para escrita atômica
            var path = Uteis._Path();
            var tempPath = path + ".tmp";
            try
            {
                var stringb = new StringBuilder();

                stringb.AppendLine("ID;Nome;Categoria;EstoqueMinimo;Saldo");

                foreach (var p in produtos)
                {
                    stringb.AppendLine($"{p.Id};{p.Nome};{p.Categoria};{p.EstoqueMinimo};{p.Saldo}");
                }
                // Escreve em arquivo temporário e depois move para o .csv
                File.WriteAllText(tempPath, stringb.ToString(), Encoding.UTF8);
                File.Move(tempPath, path, true);

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
        public static void SalvarMovimentos(List<Movimento> movimentos)
        {
            // Variável para escrita atômica
            var path = Uteis._PathMovimento();
            var tempPath = path + ".tmp";
            try
            {
                var stringb = new StringBuilder();

                stringb.AppendLine("id;produtoId;tipo;quantidade;data;observacao");

                foreach (var m in movimentos)
                {
                    stringb.AppendLine($"{m.Id};{m.ProdutoId};{m.Tipo};{m.Quantidade};{m.Data};{m.Observacao}");
                }
                // Escreve em arquivo temporário e depois move para o .csv
                File.WriteAllText(tempPath, stringb.ToString(), Encoding.UTF8);
                File.Move(tempPath, path, true);

                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.Black;
                Console.WriteLine($"Os dados foram salvos com sucesso em: {Uteis._PathMovimento()}");
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

using System.Text;
using EstoqueConsole.src.Modelo;

namespace EstoqueConsole.src.Servico
{
    public class Uteis
    {
        public static string _Path () // Pega o caminho do arquivo
        {
            // Tratamento do Path
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string projectDir = Directory.GetParent(baseDir)!.Parent!.Parent!.Parent!.FullName;
            string path = Path.Combine(projectDir, "data\\produtos.csv");

            return path;
        }

        public static List<Produto> ListarProduto() // Lista os arquivos
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
    }
}

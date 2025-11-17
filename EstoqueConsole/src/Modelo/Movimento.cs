namespace EstoqueConsole.src.Modelo;

public record struct Movimento(
    int Id,
    int ProdutoId,
    string Tipo, // "ENTRADA" ou "SAIDA"
    int Quantidade,
    DateTime Data,
    string Observacao
);
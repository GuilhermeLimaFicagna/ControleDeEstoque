namespace EstoqueConsole.src.Modelo;

public record struct Produto(
    int Id,
    string Nome,
    string Categoria,
    int EstoqueMinimo,
    int Saldo
);
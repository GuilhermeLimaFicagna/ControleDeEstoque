namespace EstoqueConsole.src.Modelo;

public readonly record struct Produto(
    int id,
    string nome,
    string categoria,
    int estoqueMinimo,
    int saldo
);
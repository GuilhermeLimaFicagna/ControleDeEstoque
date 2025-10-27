namespace EstoqueConsole.src.Modelo;

public readonly record struct Movimento(
    int id,
    int produtoId,
    string tipo, // "ENTRADA" ou "SAIDA"
    int quantidade,
    DateTime data,
    string observacao
);
using System.Data;
using Dapper;
using Demo.Models;
using Microsoft.Data.SqlClient;

namespace Demo.Repositorios;

public class LibroRepositorio
{
    private readonly string _connectionString;
    public LibroRepositorio(IConfiguration configuration) =>
        _connectionString = configuration.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión BibliotecaDB.");

    private SqlConnection Connection() => new(_connectionString);

    public async Task<IEnumerable<Libro>> ListarAsync(string? titulo)
    {
        using var connection = Connection();
        if (string.IsNullOrWhiteSpace(titulo))
            return await connection.QueryAsync<Libro>("usp_Libros_Listar", commandType: CommandType.StoredProcedure);

        return await connection.QueryAsync<Libro>("usp_Libros_BuscarPorTitulo", new { Titulo = titulo }, commandType: CommandType.StoredProcedure);
    }

    public async Task<Libro?> ObtenerPorIdAsync(int id)
    {
        using var connection = Connection();
        return await connection.QueryFirstOrDefaultAsync<Libro>("usp_Libros_ObtenerPorId", new { LibroId = id }, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<Autor>> ListarAutoresAsync()
    {
        using var connection = Connection();
        return await connection.QueryAsync<Autor>("usp_Autores_ListarActivos", commandType: CommandType.StoredProcedure);
    }

    public async Task CrearAsync(Libro libro)
    {
        using var connection = Connection();
        await connection.ExecuteAsync("usp_Libros_Insertar", new { libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares }, commandType: CommandType.StoredProcedure);
    }

    public async Task ActualizarAsync(Libro libro)
    {
        using var connection = Connection();
        await connection.ExecuteAsync("usp_Libros_Actualizar", new { libro.LibroId, libro.Titulo, libro.ISBN, libro.AutorId, libro.Ejemplares }, commandType: CommandType.StoredProcedure);
    }

    public async Task EliminarAsync(int id)
    {
        using var connection = Connection();
        await connection.ExecuteAsync("usp_Libros_Eliminar", new { LibroId = id }, commandType: CommandType.StoredProcedure);
    }
}

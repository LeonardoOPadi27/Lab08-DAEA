using System.Data;
using Dapper;
using Demo.Models;
using Microsoft.Data.SqlClient;

namespace Demo.Repositorios;

public class SocioRepositorio
{
    private readonly string _connectionString;
    public SocioRepositorio(IConfiguration configuration) =>
        _connectionString = configuration.GetConnectionString("BibliotecaDB")
            ?? throw new InvalidOperationException("No se encontró la cadena de conexión BibliotecaDB.");
    private SqlConnection Connection() => new(_connectionString);

    public async Task<IEnumerable<Socio>> ListarAsync()
    {
        using var connection = Connection();
        return await connection.QueryAsync<Socio>("usp_Socios_ListarActivos", commandType: CommandType.StoredProcedure);
    }

    public async Task CrearAsync(Socio socio)
    {
        using var connection = Connection();
        await connection.ExecuteAsync("usp_Socios_Insertar", new { socio.DNI, socio.Nombre, socio.Email }, commandType: CommandType.StoredProcedure);
    }

    public async Task<IEnumerable<PrestamoReporte>> ReporteAsync(DateTime? desde, DateTime? hasta)
    {
        using var connection = Connection();
        return await connection.QueryAsync<PrestamoReporte>("usp_Prestamos_Reporte", new { Desde = desde, Hasta = hasta }, commandType: CommandType.StoredProcedure);
    }
}

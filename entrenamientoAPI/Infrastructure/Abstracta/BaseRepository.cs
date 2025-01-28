using Microsoft.Extensions.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;

namespace entrenamientoAPI.Infrastructure.Abstracta
{
    public abstract class BaseRepository <Parametre, Resul>
    {
        public Parametre Parameters { get; set; }
        public Resul Result { get; set; }

        protected readonly string _connectionString;

        protected BaseRepository()
        {
            //conseguimos la cadena de conexion 
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();

            _connectionString = configuration.GetConnectionString("PesajeInteligenteConnection") ??
                                throw new InvalidOperationException("Connection string not found.");           
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }

        protected async Task<Resul> ExecuteStoredProcedureAsync(string NameProcedure)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();

                    Result = await connection.QuerySingleOrDefaultAsync<Resul>(
                        NameProcedure,
                        this.Parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return Result;
                }
            }
            catch (SqlException sqlEx)
            {
                ////TODO: si se va guardar las excepciones de sql server
                return Result;
            }
        }

        protected async Task<List<Resul>> ExecuteStoredProcedureListAsync(string NameProcedure)
        {
            try
            {
                using (var connection = CreateConnection())
                {
                    await connection.OpenAsync();

                    var result = await connection.QueryAsync<Resul>(
                        NameProcedure,
                        this.Parameters,
                        commandType: CommandType.StoredProcedure
                    );

                    return result.ToList();
                }
            }
            catch (SqlException sqlEx)
            {
                ////TODO: si se va guardar las excepciones de sql server
                return null;
            }
        }
    }
}

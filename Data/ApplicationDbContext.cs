using Microsoft.EntityFrameworkCore;
using Warehouseweb2.Models;
using Npgsql; 
using System.Collections.Generic;
using System.Data;
using Microsoft.Extensions.Configuration;
using System;
using System.Threading.Tasks;

namespace WarehouseWeb2.Data
{
    public class ApplicationDbContext : DbContext
    {
        private readonly IConfiguration _configuration;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, IConfiguration configuration)
            : base(options)
        {
            _configuration = configuration;
        }

        public async Task<List<T>> RawSqlQuery<T>(string query, Func<NpgsqlDataReader, T> map, params NpgsqlParameter[] parameters)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            var entities = new List<T>();

            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        while (await reader.ReadAsync())
                        {
                            entities.Add(map(reader));
                        }
                    }
                }
            }
            return entities;
        }

        public async Task<T> RawScalarQuery<T>(string query, params NpgsqlParameter[] parameters)
        {
            var connectionString = _configuration.GetConnectionString("DefaultConnection");
            using (var connection = new NpgsqlConnection(connectionString))
            {
                await connection.OpenAsync();
                using (var command = new NpgsqlCommand(query, connection))
                {
                    if (parameters != null && parameters.Length > 0)
                    {
                        command.Parameters.AddRange(parameters);
                    }

                    var result = await command.ExecuteScalarAsync();
                    if (result == DBNull.Value || result == null) return default(T);
                    return (T)Convert.ChangeType(result, typeof(T));
                }
            }
        }
    }
}
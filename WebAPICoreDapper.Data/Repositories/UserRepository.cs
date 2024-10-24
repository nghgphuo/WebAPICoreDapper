using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using WebAPICoreDapper.Data.Models;
using WebAPICoreDapper.Data.Repositories.Interfaces;
using WebAPICoreDapper.Utilities.Dtos;

namespace WebAPICoreDapper.Data.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly string _connectionString;

        public UserRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        public async Task<IEnumerable<AppUser>> Get()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();

                var result = await conn.QueryAsync<AppUser>("Get_User_All", null, null, null, System.Data.CommandType.StoredProcedure);
                return result;
            }
        }

        public async Task<PagedResult<AppUser>> GetPaging(string keyword, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();

                var parameters = new DynamicParameters();
                parameters.Add("@keyword", keyword);
                parameters.Add("@pageIndex", pageIndex);
                parameters.Add("@pageSize", pageSize);
                parameters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var result = await conn.QueryAsync<AppUser>("Get_User_AllPaging", parameters, null, null, System.Data.CommandType.StoredProcedure);
                int totalRow = parameters.Get<int>("@totalRow");

                return new PagedResult<AppUser>
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
            }
        }
    }
}

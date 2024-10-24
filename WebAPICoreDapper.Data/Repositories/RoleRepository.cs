using Dapper;
using Microsoft.Extensions.Configuration;
using System.Data.SqlClient;
using WebAPICoreDapper.Data.Models;
using WebAPICoreDapper.Data.Repositories.Interfaces;
using WebAPICoreDapper.Utilities.Dtos;

namespace WebAPICoreDapper.Data.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        private readonly string _connectionString;

        public RoleRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        // Lấy tất cả vai trò từ database
        public async Task<IEnumerable<AppRole>> Get()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();

                var result = await conn.QueryAsync<AppRole>("Get_Role_All", commandType: System.Data.CommandType.StoredProcedure);
                return result;
            }
        }

        // Lấy danh sách vai trò có phân trang từ database
        public async Task<PagedResult<AppRole>> GetPaging(string keyword, int pageIndex, int pageSize)
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if (conn.State == System.Data.ConnectionState.Closed)
                    await conn.OpenAsync();

                var paramaters = new DynamicParameters();
                paramaters.Add("@keyword", keyword);
                paramaters.Add("@pageIndex", pageIndex);
                paramaters.Add("@pageSize", pageSize);
                paramaters.Add("@totalRow", dbType: System.Data.DbType.Int32, direction: System.Data.ParameterDirection.Output);

                var result = await conn.QueryAsync<AppRole>("Get_Role_AllPaging", paramaters, commandType: System.Data.CommandType.StoredProcedure);

                int totalRow = paramaters.Get<int>("@totalRow");

                var pagedResult = new PagedResult<AppRole>()
                {
                    Items = result.ToList(),
                    TotalRow = totalRow,
                    PageIndex = pageIndex,
                    PageSize = pageSize
                };
                return pagedResult;
            }
        }
    }
}

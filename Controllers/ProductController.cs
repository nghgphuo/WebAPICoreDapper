using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;
using WebAPICoreDapper.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace WebAPICoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly string _connectionString;
        public ProductController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DbConnectionString");
        }

        // GET: api/<ProductController>
        [HttpGet]
        public async Task<IEnumerable<Product>> Get()
        {
            using (var conn = new SqlConnection(_connectionString))
            {
                if(conn.State == System.Data.ConnectionState.Closed)
                    conn.Open();
                var result = await conn.QueryAsync<Product>("SELECT Id, Sku, Price, DiscountPrice, ImageUrl, CreateAt, IsActive, ViewCount FROM Products\r\n", null, null, null, System.Data.CommandType.Text);
                return result;
            }
        }

        // GET api/<ProductController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ProductController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ProductController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ProductController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}

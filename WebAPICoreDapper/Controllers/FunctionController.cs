using Dapper;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data.SqlClient;
using WebAPICoreDapper.Data.Models;
using WebAPICoreDapper.Data.Repositories;
using WebAPICoreDapper.Data.Repositories.Interfaces;
using WebAPICoreDapper.Filters;
using WebAPICoreDapper.Utilities.Dtos;
namespace WebAPICoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FunctionController : ControllerBase
    {
        private readonly IFunctionRepository _functionRepository;
        public FunctionController(IConfiguration configuration, IFunctionRepository functionRepository)
        {
            _functionRepository = functionRepository;
        }
        // GET: api/Role
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _functionRepository.GetAllAsync();  
            return Ok(result);
        }

        // GET: api/Role/5
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var result = await _functionRepository.GetAsync(id);
            return Ok(result);
        }

        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(string keyword, int pageIndex, int pageSize)
        {
            var pagedResult = await _functionRepository.GetPaging(keyword, pageIndex, pageSize);
            return Ok(pagedResult);
        }

        // POST: api/Role
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] Function function)
        {
            await _functionRepository.Post(function);
            return Ok();
        }

        // PUT: api/Role/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([Required] Guid id, [FromBody] Function function)
        {
            await _functionRepository.Put(id, function);
            return Ok();
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            await _functionRepository.Delete(id);
            return Ok();
        }
    }
}
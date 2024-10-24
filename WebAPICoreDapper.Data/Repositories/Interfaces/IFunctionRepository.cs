using System.ComponentModel.DataAnnotations;
using WebAPICoreDapper.Data.Models;
using WebAPICoreDapper.Utilities.Dtos;

namespace WebAPICoreDapper.Data.Repositories.Interfaces
{
    public interface IFunctionRepository
    {
        public Task<IEnumerable<Function>> GetAllAsync();
        public Task<Function> GetAsync(string id);
        public Task<PagedResult<Function>> GetPaging(string keyword, int pageIndex, int pageSize);
        public Task Post( Function function);
        public Task Put([Required] Guid id, Function function);
        public Task Delete(string id);

    }
}

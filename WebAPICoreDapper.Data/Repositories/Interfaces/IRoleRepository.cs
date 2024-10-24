using System.ComponentModel.DataAnnotations;
using WebAPICoreDapper.Data.Models;
using WebAPICoreDapper.Utilities.Dtos;

namespace WebAPICoreDapper.Data.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        public Task<IEnumerable<AppRole>> Get();
        public Task<PagedResult<AppRole>> GetPaging(string keyword, int pageIndex, int pageSize);
    }
}

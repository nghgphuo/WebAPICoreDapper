using System.ComponentModel.DataAnnotations;
using WebAPICoreDapper.Data.Models;
using WebAPICoreDapper.Utilities.Dtos;

namespace WebAPICoreDapper.Data.Repositories.Interfaces
{
    public interface IUserRepository
    {
        public Task<IEnumerable<AppUser>> Get();
        public Task<PagedResult<AppUser>> GetPaging(string keyword, int pageIndex, int pageSize);

    }
}

using WebAPICoreDapper.Models;

namespace WebAPICoreDapper.Dtos
{
    public class PagedResult<T>
    {
        public List<T> Items { get; set; }
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public int TotalRow { get; set; }
    }
}

using WebAPICoreDapper.Data.ViewModels;

namespace WebAPICoreDapper.Data.Repositories.Interfaces
{
    public interface IPermissionRepository
    {
        public Task<IEnumerable<FunctionActionViewModel>> GetAllWithPermission();
        public Task<IEnumerable<PermissionViewModel>> GetAllRolePermissions(Guid? role);
        public Task SavePermissions(Guid role, List<PermissionViewModel> permissions);
        public Task<IEnumerable<FunctionViewModel>> GetAllFunctionByPermission(Guid userId);
    }
}

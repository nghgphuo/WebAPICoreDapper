using Microsoft.AspNetCore.Mvc;
using WebAPICoreDapper.Data.Repositories.Interfaces;
using WebAPICoreDapper.Data.ViewModels;
using WebAPICoreDapper.Extensions;
namespace WebAPICoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PermissionController : ControllerBase
    {
        private readonly IPermissionRepository _permissionRepository;
        public PermissionController(IConfiguration configuration, IPermissionRepository permissionRepository)
        {
            _permissionRepository = permissionRepository;
        }
        [HttpGet("function-actions")]
        public async Task<IActionResult> GetAllWithPermission()
        {
            var result = await _permissionRepository.GetAllWithPermission();
            return Ok(result);
        }

        [HttpGet("{role}/role-permissions")]
        public async Task<IActionResult> GetAllRolePermissions(Guid? role)
        {
            var result = await _permissionRepository.GetAllRolePermissions(role);
            return Ok(result);           
        }

        [HttpPost("{role}/save-permissions")]
        public async Task<IActionResult> SavePermissions(Guid role, [FromBody] List<PermissionViewModel> permissions)
        {
            await _permissionRepository.SavePermissions(role, permissions);
            return Ok();
        }

        [HttpGet("functions-view")]
        public async Task<IActionResult> GetAllFunctionByPermission()
        {
            var result = _permissionRepository.GetAllFunctionByPermission(User.GetUserId());
            return Ok(result);
        }
    }
}
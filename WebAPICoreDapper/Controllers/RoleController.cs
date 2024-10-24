using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using WebAPICoreDapper.Data.Models;
using WebAPICoreDapper.Data.Repositories.Interfaces;
using WebAPICoreDapper.Filters;

namespace WebAPICoreDapper.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RoleController : ControllerBase
    {
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IRoleRepository _roleRepository;

        public RoleController(RoleManager<AppRole> roleManager, IRoleRepository roleRepository)
        {
            _roleManager = roleManager;
            _roleRepository = roleRepository;
        }

        // Lấy tất cả các vai trò từ cơ sở dữ liệu
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var result = await _roleRepository.Get(); // Lấy dữ liệu qua repository
            return Ok(result);
        }

        // Lấy thông tin một vai trò cụ thể bằng Id từ RoleManager
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();
            return Ok(role);
        }

        // Lấy danh sách vai trò có phân trang
        [HttpGet("paging")]
        public async Task<IActionResult> GetPaging(string keyword, int pageIndex, int pageSize)
        {
            var pagedResult = await _roleRepository.GetPaging(keyword, pageIndex, pageSize);
            return Ok(pagedResult);
        }

        // Tạo một vai trò mới
        [HttpPost]
        [ValidateModel]
        public async Task<IActionResult> Post([FromBody] AppRole role)
        {
            var result = await _roleManager.CreateAsync(role);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Errors);
        }

        // Cập nhật thông tin một vai trò
        [HttpPut("{id}")]
        public async Task<IActionResult> Put([Required] Guid id, [FromBody] AppRole role)
        {
            role.Id = id;
            var result = await _roleManager.UpdateAsync(role);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Errors);
        }

        // Xóa một vai trò
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null) return NotFound();

            var result = await _roleManager.DeleteAsync(role);
            if (result.Succeeded)
                return Ok();
            return BadRequest(result.Errors);
        }
    }
}

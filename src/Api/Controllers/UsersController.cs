using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using OTUserManagementSystem.src.Core.DTOs;
using OTUserManagementSystem.src.Core.Interfaces;
using OTUserManagementSystem.src.Infrastructure.Repositories;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace OTUserManagementSystem.src.Api.Controllers
{
    [ApiController]
    [Route("users")]
    [Authorize]

    public class UsersController : ControllerBase
    {
        private readonly IUserRepository _repo;

        public UsersController(IUserRepository repo) => _repo = repo;

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var users = await _repo.GetAllAsync();
            var dtos = users.Select(u => new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                CreatedAt = u.CreatedAt,
                LastLoginAt = u.LastLoginAt
            });
            return Ok(dtos);
        }

        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var u = await _repo.GetByIdAsync(id);
            if (u == null) return NotFound();
            return Ok(new UserDTO
            {
                Id = u.Id,
                Username = u.Username,
                Email = u.Email,
                FirstName = u.FirstName,
                LastName = u.LastName,
                CreatedAt = u.CreatedAt,
                LastLoginAt = u.LastLoginAt
            });
        }

        [HttpPut("{id:guid}")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UserDTO dto)
        {
            if (id != dto.Id) return BadRequest();

            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();

            user.Username = dto.Username ?? user.Username;
            user.Email = dto.Email ?? user.Email;
            user.FirstName = dto.FirstName ?? user.FirstName;
            user.LastName = dto.LastName ?? user.LastName;
            await _repo.UpdateAsync(user);
            return NoContent();
        }

        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var user = await _repo.GetByIdAsync(id);
            if (user == null) return NotFound();
            await _repo.DeleteAsync(user);
            return NoContent();
        }
    }
}

using Microsoft.EntityFrameworkCore;
using HabitosApp.Web.Data;
using HabitosApp.Web.Data.Entities;
using HabitosApp.Web.DTOs;

namespace HabitosApp.Web.Helpers
{
    public interface IConverterHelper
    {
        public AccountUserDTO ToAccountDTO(User user);
        public HabitosAppRole ToRole(HabitosAppRoleDTO dto);
        public Task<HabitosAppRoleDTO> ToRoleDTOAsync(HabitosAppRole role);
    }

    public class ConverterHelper : IConverterHelper
    {
        private readonly DataContext _context;

        public ConverterHelper(DataContext context)
        {
            _context = context;
        }

        public AccountUserDTO ToAccountDTO(User user)
        {
            return new AccountUserDTO
            {
                Id = Guid.Parse(user.Id),
                Document = user.Document,
                Email = user.Email,
                FirstName = user.FirstName,
                LastName = user.LastName,
                PhoneNumber = user.PhoneNumber,
            };
        }

        public HabitosAppRole ToRole(HabitosAppRoleDTO dto)
        {
            return new HabitosAppRole
            {
                Id = dto.Id,
                Name = dto.Name,
            };
        }

        public async Task<HabitosAppRoleDTO> ToRoleDTOAsync(HabitosAppRole role)
        {
            List<PermissionForDTO> permissions = await _context.Permissions.Select(p => new PermissionForDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
                Selected = _context.RolePermissions.Any(rp => rp.PermissionId == p.Id && rp.RoleId == role.Id)

            }).ToListAsync();

            return new HabitosAppRoleDTO
            {
                Id = role.Id,
                Name = role.Name,
                Permissions = permissions,
            };
        }
    }
}

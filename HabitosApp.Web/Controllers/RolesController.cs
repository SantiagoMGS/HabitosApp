using Microsoft.AspNetCore.Mvc;
using HabitosApp.Web.Core.Attributes;
using HabitosApp.Web.Core;
using HabitosApp.Web.Services;
using HabitosApp.Web.Data.Entities;
using HabitosApp.Web.DTOs;
using HabitosApp.Web.Data;
using Microsoft.EntityFrameworkCore;

namespace HabitosApp.Web.Controllers
{
    public class RolesController : Controller
    {
        private IRolesService _rolesService;
        private readonly DataContext _context;

        public RolesController(IRolesService rolesService, DataContext context)
        {
            _rolesService = rolesService;
            _context = context;
        }

        [HttpGet]
        [CustomAuthorize(permission: "showRoles", module: "Roles")]
        public async Task<IActionResult> Index()
        {
            IEnumerable<HabitosAppRole> roles = await _context.HabitosAppRoles.ToListAsync();

            return View(roles);
        }

        [HttpGet]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create()
        {
            Response<IEnumerable<Permission>> response = await _rolesService.GetPermissionsAsync();

            if (!response.IsSuccess)
            {
                
                return RedirectToAction(nameof(Index));
            }

            HabitosAppRoleDTO dto = new HabitosAppRoleDTO
            {
                Permissions = response.Result.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                }).ToList()
            };
            Console.WriteLine(dto);
            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize(permission: "createRoles", module: "Roles")]
        public async Task<IActionResult> Create(HabitosAppRoleDTO dto)
        {
            Response<IEnumerable<Permission>> permissionsResponse = await _rolesService.GetPermissionsAsync();

            if (!ModelState.IsValid) 
            {

                dto.Permissions = permissionsResponse.Result.Select(p => new PermissionForDTO
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description,
                    Module = p.Module,
                }).ToList();

                return View(dto);
            }

            Response<HabitosAppRole> createResponse = await _rolesService.CreateAsync(dto);

            if (createResponse.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }

            dto.Permissions = permissionsResponse.Result.Select(p => new PermissionForDTO
            {
                Id = p.Id,
                Name = p.Name,
                Description = p.Description,
                Module = p.Module,
            }).ToList();

            return View(dto);
        }

        [HttpGet]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(int id)
        {
            Response<HabitosAppRoleDTO> response = await _rolesService.GetOneAsync(id);

            if (!response.IsSuccess)
            {
                
                return RedirectToAction(nameof(Index));
            }

            return View(response.Result);
        }

        [HttpPost]
        [CustomAuthorize(permission: "updateRoles", module: "Roles")]
        public async Task<IActionResult> Edit(HabitosAppRoleDTO dto)
        {
            if (!ModelState.IsValid)
            {

                Response<IEnumerable<PermissionForDTO>> res = await _rolesService.GetPermissionsByRoleAsync(dto.Id);
                dto.Permissions = res.Result.ToList();

                return View(dto);
            }

            Response<HabitosAppRole> response = await _rolesService.EditAsync(dto);

            if (response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }
            Response<IEnumerable<PermissionForDTO>> res2 = await _rolesService.GetPermissionsByRoleAsync(dto.Id);
            dto.Permissions = res2.Result.ToList();
            return View(dto);
        }

        [HttpPost]
        [CustomAuthorize("deleteRoles", "Roles")]
        public async Task<IActionResult> Delete(int id)
        {
            Response<object> response = await _rolesService.DeleteAsync(id);

            if (!response.IsSuccess)
            {
                
                return RedirectToAction(nameof(Index));
            }

            return RedirectToAction(nameof(Index));
        }
    }
}

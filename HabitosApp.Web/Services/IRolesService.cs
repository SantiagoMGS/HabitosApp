using HabitosApp.Web.Core;
using HabitosApp.Web.Data.Entities;
using HabitosApp.Web.Data;
using HabitosApp.Web.Helpers;
using HabitosApp.Web.DTOs;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Newtonsoft.Json;
using Microsoft.EntityFrameworkCore;

namespace HabitosApp.Web.Services
{
    public interface IRolesService
    {
        public Task<Response<HabitosAppRole>> CreateAsync(HabitosAppRoleDTO dto);

        public Task<Response<object>> DeleteAsync(int id);

        public Task<Response<HabitosAppRole>> EditAsync(HabitosAppRoleDTO dto);

        public Task<Response<HabitosAppRoleDTO>> GetOneAsync(int id);

        public Task<Response<IEnumerable<Permission>>> GetPermissionsAsync();

        public Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id);
    }

    public class RolesService : IRolesService
    {
        private readonly DataContext _context;
        private readonly IConverterHelper _converterHelper;

        public RolesService(DataContext context, IConverterHelper converterHelper)
        {
            _context = context;
            _converterHelper = converterHelper;
        }

        public async Task<Response<HabitosAppRole>> CreateAsync(HabitosAppRoleDTO dto)
        {
            using IDbContextTransaction transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                // Creación de Rol
                HabitosAppRole model = _converterHelper.ToRole(dto);
                EntityEntry<HabitosAppRole> modelStored = await _context.HabitosAppRoles.AddAsync(model);

                await _context.SaveChangesAsync();

                // Inserción de permisos
                int roleId = modelStored.Entity.Id;

                List<int> permissionIds = new List<int>();

                if (!string.IsNullOrWhiteSpace(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                }

                foreach (int permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleId = roleId,
                        PermissionId = permissionId
                    };

                    _context.RolePermissions.Add(rolePermission);
                }

                await _context.SaveChangesAsync();
                transaction.Commit();

                return ResponseHelper<HabitosAppRole>.MakeResponseSuccess("Rol creado con éxito");
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                return ResponseHelper<HabitosAppRole>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<object>> DeleteAsync(int id)
        {
            try
            {
                Response<HabitosAppRole> roleResponse = await GetOneModelAsync(id);

                if (!roleResponse.IsSuccess) 
                {
                    return ResponseHelper<object>.MakeResponseFail(roleResponse.Message);
                }

                if (roleResponse.Result.Name == "Administrador")
                {
                    return ResponseHelper<object>.MakeResponseFail($"El rol Administrador no puede ser eliminado");
                }

                if (roleResponse.Result.Users.Count() > 0)
                {
                    return ResponseHelper<object>.MakeResponseFail($"El rol no puede ser eliminado debido a que tiene usuarios relacionados");
                }

                _context.HabitosAppRoles.Remove(roleResponse.Result);
                await _context.SaveChangesAsync();

                return ResponseHelper<object>.MakeResponseSuccess("Rol eliminado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<object>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<HabitosAppRole>> EditAsync(HabitosAppRoleDTO dto)
        {
            try
            {
                if (dto.Name == "Administrador")
                {
                    return ResponseHelper<HabitosAppRole>.MakeResponseFail($"El Rol Administrador no puede ser editado");
                }

                List<int> permissionIds = new List<int>();

                if (!string.IsNullOrEmpty(dto.PermissionIds))
                {
                    permissionIds = JsonConvert.DeserializeObject<List<int>>(dto.PermissionIds);
                }

                // Eliminación de antiguos permisos
                List<RolePermission> oldRolePermissions = await _context.RolePermissions.Where(rs => rs.RoleId == dto.Id).ToListAsync();
                _context.RolePermissions.RemoveRange(oldRolePermissions);

                // Inserción de nuevos permisos
                foreach (int permissionId in permissionIds)
                {
                    RolePermission rolePermission = new RolePermission
                    {
                        RoleId = dto.Id,
                        PermissionId = permissionId
                    };

                    _context.RolePermissions.Add(rolePermission);
                }

                // Actualización de rol
                HabitosAppRole model = _converterHelper.ToRole(dto);
                _context.HabitosAppRoles.Update(model);

                await _context.SaveChangesAsync();

                return ResponseHelper<HabitosAppRole>.MakeResponseSuccess("Rol editado con éxito");
            }
            catch (Exception ex)
            {
                return ResponseHelper<HabitosAppRole>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<HabitosAppRoleDTO>> GetOneAsync(int id)
        {
            try
            {
                HabitosAppRole? HabitosAppRole = await _context.HabitosAppRoles.FirstOrDefaultAsync(r => r.Id == id);

                if (HabitosAppRole is null)
                {
                    return ResponseHelper<HabitosAppRoleDTO>.MakeResponseFail($"El Rol con id '{id}' no existe.");
                }

                return ResponseHelper<HabitosAppRoleDTO>.MakeResponseSuccess(await _converterHelper.ToRoleDTOAsync(HabitosAppRole));
            }
            catch (Exception ex)
            {
                return ResponseHelper<HabitosAppRoleDTO>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<Permission>>> GetPermissionsAsync()
        {
            try
            {
                IEnumerable<Permission> permissions = await _context.Permissions.ToListAsync();

                return ResponseHelper<IEnumerable<Permission>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<Permission>>.MakeResponseFail(ex);
            }
        }

        public async Task<Response<IEnumerable<PermissionForDTO>>> GetPermissionsByRoleAsync(int id)
        {
            try
            {
                Response<HabitosAppRoleDTO> response = await GetOneAsync(id);

                if (!response.IsSuccess)
                {
                    return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseSuccess(response.Message);
                }

                List<PermissionForDTO> permissions = response.Result.Permissions;

                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseSuccess(permissions);
            }
            catch (Exception ex)
            {
                return ResponseHelper<IEnumerable<PermissionForDTO>>.MakeResponseFail(ex);
            }
        }

        private async Task<Response<HabitosAppRole>> GetOneModelAsync(int id)
        {
            try
            {
                HabitosAppRole? role = await _context.HabitosAppRoles.Include(r => r.Users)
                                                                       .FirstOrDefaultAsync(r => r.Id == id);

                if (role is null)
                {
                    return ResponseHelper<HabitosAppRole>.MakeResponseFail($"El Rol con id '{id}' no existe");
                }

                return ResponseHelper<HabitosAppRole>.MakeResponseSuccess(role);

            }
            catch (Exception ex)
            {
                return ResponseHelper<HabitosAppRole>.MakeResponseFail(ex);
            }
        }
    }
}
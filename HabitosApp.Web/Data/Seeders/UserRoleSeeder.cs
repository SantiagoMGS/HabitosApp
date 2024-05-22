using Microsoft.EntityFrameworkCore;
using HabitosApp.Web.Core;
using HabitosApp.Web.Data.Entities;
using HabitosApp.Web.Services;

namespace HabitosApp.Web.Data.Seeders
{
    public class UserRoleSeeder
    {
        private readonly IUsersService _usersService;
        private readonly DataContext _context;

        public UserRoleSeeder(IUsersService usersService, DataContext context)
        {
            _usersService = usersService;
            _context = context;
        }

        public async Task SeedAsync()
        {
            await CheckRolesAsync();
            await CheckUsers();
        }

        private async Task AdministradorRoleAsync()
        {
            HabitosAppRole? tmp = await _context.HabitosAppRoles.Where(ir => ir.Name == "Administrador").FirstOrDefaultAsync();

            if (tmp == null)
            {
                HabitosAppRole role = new HabitosAppRole { Name = "Administrador" };
                _context.HabitosAppRoles.Add(role);
                await _context.SaveChangesAsync();
            }
        }

        private async Task CheckUsers()
        {
            // Administrador
            User? user = await _usersService.GetUserAsync("smartinez@gmail.com");

            HabitosAppRole adminRole = _context.HabitosAppRoles.Where(r => r.Name == "Administrador").First();

            if (user is null)
            {
                user = new User
                {
                    Email = "smartinez@gmail.com",
                    FirstName = "Santiago",
                    LastName = "Martinez Gutierrez",
                    PhoneNumber = "0000000000",
                    UserName = "smartinez@gmail.com",
                    Document = "111111111",
                    HabitosAppRole = adminRole,
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }

            // Profesional de Salud
            user = await _usersService.GetUserAsync("karencita@gmail.com");


            HabitosAppRole profesionalSaludRole = await _context.HabitosAppRoles.Where(pbr => pbr.Name == "Profesional de Salud").FirstAsync();

            if (user == null)
            {
                user = new User
                {
                    Email = "karencita@gmail.com",
                    FirstName = "Kakaren",
                    LastName = "Morales",
                    PhoneNumber = "1111111111",
                    UserName = "karencita@gmail.com",
                    Document = "222222222",
                    HabitosAppRole = profesionalSaludRole
                };

                await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }

            // Usuario Regular
            user = await _usersService.GetUserAsync("sebas@cebollas.com");

            HabitosAppRole usuarioRegular = await _context.HabitosAppRoles
            .Where(pbr => pbr.Name == "Usuario Regular")
            .FirstAsync();

            if (user == null)
            {
                user = new User
                {
                    Email = "sebas@cebollas.com",
                    FirstName = "Sebollas",
                    LastName = "Baratas",
                    PhoneNumber = "3333333",
                    UserName = "sebas@cebollas.com",
                    Document = "33333333",
                    HabitosAppRole = usuarioRegular
                };

                var result = await _usersService.AddUserAsync(user, "1234");

                string token = await _usersService.GenerateEmailConfirmationTokenAsync(user);
                await _usersService.ConfirmEmailAsync(user, token);
            }
        }

        private async Task ProfesionalSaludRoleAsync()
        {

            HabitosAppRole? tmp = await _context.HabitosAppRoles
            .Where(pbr => pbr.Name == "Profesional de Salud")
            .FirstOrDefaultAsync();

            if (tmp == null)
            {
                HabitosAppRole role = new HabitosAppRole { Name = "Profesional de Salud" };

                _context.HabitosAppRoles.Add(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Secciones").ToListAsync();

                foreach (Permission permission in permissions)
                {
                    _context.RolePermissions.Add(new RolePermission { Role = role, Permission = permission });
                }
            }

            await _context.SaveChangesAsync();
        }

        private async Task UsuarioRegularRoleAsync()
        {

            HabitosAppRole? tmp = await _context.HabitosAppRoles
            .Where(pbr => pbr.Name == "Usuario Regular")
            .FirstOrDefaultAsync();

            if (tmp == null)
            {
                HabitosAppRole role = new HabitosAppRole { Name = "Usuario Regular" };

                _context.HabitosAppRoles.Add(role);

                List<Permission> permissions = await _context.Permissions.Where(p => p.Module == "Usuarios").ToListAsync();

                foreach (Permission permission in permissions)
                {
                    _context.RolePermissions.Add(new RolePermission { Role = role, Permission = permission });
                }
            }

            await _context.SaveChangesAsync();
        }


        private async Task CheckRolesAsync()
        {
            await AdministradorRoleAsync();
            await ProfesionalSaludRoleAsync();
            await UsuarioRegularRoleAsync();
        }
    }
}

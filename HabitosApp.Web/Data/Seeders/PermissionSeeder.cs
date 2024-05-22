using HabitosApp.Web.Data.Entities;

namespace HabitosApp.Web.Data.Seeders
{
    public class PermissionSeeder
    {
        private readonly DataContext _context;

        public PermissionSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Permission> permissions = new List<Permission>();
            permissions.AddRange(Habits());
            permissions.AddRange(Roles());
            permissions.AddRange(Users());
            permissions.AddRange(Medications());

            foreach (Permission permission in permissions)
            {
                Permission? tmpPermission = _context.Permissions.Where(p => p.Name == permission.Name && p.Module == permission.Module)
                                                                .FirstOrDefault();
                if (tmpPermission is null)
                {
                    _context.Permissions.Add(permission);
                }
            }

            await _context.SaveChangesAsync();
        }

        private List<Permission> Habits()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showHabits", Description = "Ver Habitos", Module = "Habits" },
                new Permission { Name = "createHabits", Description = "Crear Habitos", Module = "Habits" },
                new Permission { Name = "updateHabits", Description = "Editar Habitos", Module = "Habits" },
                new Permission { Name = "deleteHabits", Description = "Eliminar Habitos", Module = "Habits" },
            };

            return list;
        }
        private List<Permission> Roles()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showRoles", Description = "Ver Roles", Module = "Roles" },
                new Permission { Name = "createRoles", Description = "Crear Roles", Module = "Roles" },
                new Permission { Name = "updateRoles", Description = "Editar Roles", Module = "Roles" },
                new Permission { Name = "deleteRoles", Description = "Eliminar Roles", Module = "Roles" },
            };

            return list;
        }

        private List<Permission> Medications()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showMedications", Description = "Ver Medicamentos", Module = "Medications" },
                new Permission { Name = "createMedications", Description = "Crear Medicamentos", Module = "Medications" },
                new Permission { Name = "updateMedications", Description = "Editar Medicamentos", Module = "Medications" },
                new Permission { Name = "deleteMedications", Description = "Eliminar Medicamentos", Module = "Medications" },
            };

            return list;
        }

        private List<Permission> Users()
        {
            List<Permission> list = new List<Permission>
            {
                new Permission { Name = "showUsers", Description = "Ver Usuarios", Module = "Users" },
                new Permission { Name = "createUsers", Description = "Crear Usuarios", Module = "Users" },
                new Permission { Name = "updateUsers", Description = "Editar Usuarios", Module = "Users" },
                new Permission { Name = "deleteUsers", Description = "Eliminar Usuarios", Module = "Users" },
            };

            return list;
        }

    }
}

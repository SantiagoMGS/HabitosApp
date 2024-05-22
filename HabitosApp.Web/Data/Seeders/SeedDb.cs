using HabitosApp.Web.Services;

namespace HabitosApp.Web.Data.Seeders
{
    public class SeedDb
    {
        private readonly DataContext _context;
        private readonly IUsersService _usersService;

        public SeedDb(DataContext context, IUsersService usersService)
        {
            _context = context;
            _usersService = usersService;
        }

        public async Task SeedAsync()
        {
            await new PermissionSeeder(_context).SeedAsync();
            await new UserRoleSeeder(_usersService, _context).SeedAsync();
            await new HabitTypeSeeder(_context).SeedAsync();
            await new UnitSeeder(_context).SeedAsync();
            await new ViaAdminSeeder(_context).SeedAsync();
        }
    }
}
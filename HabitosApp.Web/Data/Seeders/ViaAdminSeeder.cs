using Microsoft.EntityFrameworkCore;
using HabitosApp.Web.Data.Entities;

namespace HabitosApp.Web.Data.Seeders
{
    public class ViaAdminSeeder
    {
        private readonly DataContext _context;

        public ViaAdminSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<ViaAdmin> ViaAdmins = new List<ViaAdmin>
            {
                new ViaAdmin { Description = "Oral" },
                new ViaAdmin { Description = "Ocular" },
                new ViaAdmin { Description = "Anal" },
                new ViaAdmin { Description = "Inyeccion" },
            };

            foreach (ViaAdmin viaAdmin in ViaAdmins)
            {
                bool exists = await _context.ViaAdmin.AnyAsync(ht => ht.Description == viaAdmin.Description);

                if (!exists)
                {
                    await _context.ViaAdmin.AddAsync(viaAdmin);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
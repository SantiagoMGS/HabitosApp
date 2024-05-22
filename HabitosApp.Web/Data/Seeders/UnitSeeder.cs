using Microsoft.EntityFrameworkCore;
using HabitosApp.Web.Data.Entities;

namespace HabitosApp.Web.Data.Seeders
{
    public class UnitSeeder
    {
        private readonly DataContext _context;

        public UnitSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<Unit> Units = new List<Unit>
            {
                new Unit { Description = "GR" },
                new Unit { Description = "ML" },
                new Unit { Description = "Otros" },
            };

            foreach (Unit unit in Units)
            {
                bool exists = await _context.Unit.AnyAsync(ht => ht.Description == unit.Description);

                if (!exists)
                {
                    await _context.Unit.AddAsync(unit);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
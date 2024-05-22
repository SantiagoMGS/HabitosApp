using Microsoft.EntityFrameworkCore;
using HabitosApp.Web.Data.Entities;

namespace HabitosApp.Web.Data.Seeders
{
    public class HabitTypeSeeder
    {
        private readonly DataContext _context;

        public HabitTypeSeeder(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            List<HabitType> habitTypes = new List<HabitType>
            {
                new HabitType { Description = "Fisico" },
                new HabitType { Description = "Mental" },
                new HabitType { Description = "Otros" },
            };

            foreach (HabitType habitType in habitTypes)
            {
                bool exists = await _context.HabitType.AnyAsync(ht => ht.Description == habitType.Description);

                if (!exists)
                {
                    await _context.HabitType.AddAsync(habitType);
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
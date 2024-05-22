using HabitosApp.Web.Data;
using HabitosApp.Web.Data.Entities;
using HabitosApp.Web.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace habitos_web_app.Controllers
{
    public class HabitTypesController : Controller
    {
        private readonly DataContext _context;

        public HabitTypesController(DataContext context)
        {
            _context = context;
        }


        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.HabitType.ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(HabitTypeDto dto)
        {

            if (ModelState.IsValid)
            {
                HabitType habitType = new HabitType
                {
                    Description = dto.Description,
                };

                await _context.HabitType.AddAsync(habitType);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(dto);
        }



        [HttpGet]
        public async Task<IActionResult> Edit([FromRoute] int id)
        {
      
            HabitType habitType = await _context.HabitType.FindAsync(id);
            // Si el usuario no existe, retorna un error 404 y no revienta la aplicación
            if (habitType == null)
            {
                return NotFound();
            }

            HabitTypeDto habitTypeEditDto = new HabitTypeDto
            {
                Description = habitType.Description,
            };

            return View(habitTypeEditDto);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(HabitTypeDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            HabitType habitType = await _context.HabitType.FirstOrDefaultAsync(ht => ht.Id == dto.Id);
            if (habitType == null)
            {
                return NotFound();
            }

            habitType.Description = dto.Description;

            _context.Update(habitType);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        public async Task<IActionResult> Delete(HabitTypeDto dto)
        {
            HabitType habitType = await _context.HabitType.FirstOrDefaultAsync(ht => ht.Id == dto.Id);
            if (habitType == null)
            {
                return NotFound();
            }

            _context.HabitType.Remove(habitType);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }
    }
}

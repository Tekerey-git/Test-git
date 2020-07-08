using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HistorySiteIdentity.Models;
using Microsoft.AspNetCore.Authorization;
using HistorySiteIdentity.ViewModels;
using System.IO;

namespace HistorySiteIdentity.Controllers
{
    public class RegimentsController : Controller
    {
        private readonly ForceDBContext _context;

        public RegimentsController(ForceDBContext context)
        {
            _context = context;
        }

        // GET: Regiments
        public async Task<IActionResult> Index(string searchString)
        {
            var Regiments = from m in _context.Regiments
                           select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                Regiments = Regiments.Where(s => s.Name.Contains(searchString));
            }

            return View(await Regiments.ToListAsync());
        }

        // GET: Regiments/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regiment = await _context.Regiments.Include(r=>r.Battalions).Include(r=>r.Commanders)
                .FirstOrDefaultAsync(m => m.RegimentId == id);
            if (regiment == null)
            {
                return NotFound();
            }

            return View(regiment);
        }

        // GET: Regiments/Create
        [Authorize]

        public IActionResult Create()
        {
            
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            SelectList brigList = new SelectList(_context.Brigades, "BrigadeId", "Name");
            ViewBag.brigList = brigList;

            return View();
        }

        // POST: Regiments/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Create(CombatFormationViewModel combatForm)
        {
            Regiment regiment = new Regiment()
            {
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                WeekId = combatForm.WeekId,
                BrigadeId = combatForm.BrigadeId,
                DivisionId=combatForm.DivisionId,
                Image = combatForm.Image,
                Week = combatForm.Week,
                CoordinatesXY = combatForm.Coordinates,
                CoordX = combatForm.CoordX,
                CoordY = combatForm.CoordY,
                Adress = combatForm.Adress
            };
            if (ModelState.IsValid)
            {
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    regiment.CoordX = str[0];
                    regiment.CoordY = str[1];
                }
                if (combatForm.File != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(combatForm.File.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)combatForm.File.Length);
                    }
                    // установка массива байтов
                    regiment.Image = imageData;
                }
                _context.Add(regiment);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id =regiment.RegimentId });
            }
            return View(regiment);
        }

        // GET: Regiments/Edit/5
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regiment = await _context.Regiments.FindAsync(id);
            
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            SelectList brigList = new SelectList(_context.Brigades.Where(f=>f.WeekId==regiment.WeekId), "BrigadeId", "Name");
            ViewBag.brigList = brigList;

            if (regiment == null)
            {
                return NotFound();
            }
            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                Id = regiment.RegimentId,
                Name = regiment.Name,
                Number = regiment.Number,
                TotalStrenght = regiment.TotalStrenght,
                AdditionalInformation = regiment.AdditionalInformation,
                Commanders = regiment.Commanders,
                BrigadeId = regiment.BrigadeId,
                WeekId = regiment.WeekId,
                Image = regiment.Image,
                Week = regiment.Week,
                OldWeekId=regiment.WeekId,
                Coordinates= regiment.CoordinatesXY,
                CoordX = regiment.CoordX,
                CoordY = regiment.CoordY,
                Adress = regiment.Adress
            };
            return View(combatForm);
        }

        // POST: Regiments/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Edit(int id, CombatFormationViewModel combatForm)
        {
            Regiment regiment = new Regiment()
            {
                RegimentId=combatForm.Id,
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                WeekId = combatForm.WeekId,
                BrigadeId = combatForm.BrigadeId,
                DivisionId=combatForm.DivisionId,
                Image = combatForm.Image,
                Week = combatForm.Week,
                CoordinatesXY = combatForm.Coordinates,
                CoordX = combatForm.CoordX,
                CoordY = combatForm.CoordY,
                Adress = combatForm.Adress
            };
            if (id != regiment.RegimentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if(combatForm.OldWeekId!=regiment.WeekId)
                {
                    regiment.Commanders = null;
                    regiment.BrigadeId = null;
                    regiment.Battalions = null;
                    regiment.DivisionId = null;
                }
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    regiment.CoordX = str[0];
                    regiment.CoordY = str[1];
                }
                if (combatForm.File != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(combatForm.File.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)combatForm.File.Length);
                    }
                    // установка массива байтов
                    regiment.Image = imageData;
                    try
                    {
                        _context.Update(regiment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RegimentExists(regiment.RegimentId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else if (combatForm.File == null)
                {
                    regiment.Image = combatForm.Image;
                    try
                    {
                        _context.Update(regiment);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RegimentExists(regiment.RegimentId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(regiment);
        }

        // GET: Regiments/Delete/5
        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var regiment = await _context.Regiments
                .FirstOrDefaultAsync(m => m.RegimentId == id);
            if (regiment == null)
            {
                return NotFound();
            }

            return View(regiment);
        }

        // POST: Regiments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var regiment = await _context.Regiments.FindAsync(id);
            _context.Regiments.Remove(regiment);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        [Authorize]

        public async Task<IActionResult> Copy(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            var regiment = await _context.Regiments
                .FirstOrDefaultAsync(m => m.RegimentId == id);

            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                Name = regiment.Name,
                Number = regiment.Number,
                TotalStrenght = regiment.TotalStrenght,
                AdditionalInformation = regiment.AdditionalInformation,
                Commanders = regiment.Commanders,
                BrigadeId = regiment.BrigadeId,
                DivisionId=regiment.DivisionId,
                WeekId = regiment.WeekId,
                Image = regiment.Image,
                Week = regiment.Week,
                OldWeekId=regiment.WeekId,
                Coordinates = regiment.CoordinatesXY,
                CoordX = regiment.CoordX,
                CoordY = regiment.CoordY,
                Adress = regiment.Adress
            };
            return View(combatForm);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Copy(int id, CombatFormationViewModel combatForm)
        {
            Regiment regiment = new Regiment()
            {
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                WeekId = combatForm.WeekId,
                BrigadeId = combatForm.BrigadeId,
                Image = combatForm.Image,
                Week = combatForm.Week,
                CoordinatesXY = combatForm.Coordinates,
                CoordX = combatForm.CoordX,
                CoordY = combatForm.CoordY,
                Adress = combatForm.Adress
            };
            

            if (ModelState.IsValid)
            {
                if (combatForm.OldWeekId != regiment.WeekId)
                {
                    regiment.Commanders = null;
                    regiment.BrigadeId = null;
                    regiment.Battalions = null;
                    regiment.DivisionId = null;
                }
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    regiment.CoordX = str[0];
                    regiment.CoordY = str[1];
                }
                if (combatForm.File != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(combatForm.File.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)combatForm.File.Length);
                    }
                    // установка массива байтов
                    regiment.Image = imageData;
                    try
                    {
                        _context.Add(regiment);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RegimentExists(regiment.RegimentId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else if (combatForm.File == null)
                {
                    regiment.Image = combatForm.Image;
                    try
                    {
                        _context.Add(regiment);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!RegimentExists(regiment.RegimentId))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(regiment);
        }
        private bool RegimentExists(int id)
        {
            return _context.Regiments.Any(e => e.RegimentId == id);
        }
    }
}

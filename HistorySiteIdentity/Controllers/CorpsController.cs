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

namespace HistorySiteIdentity.Views
{
    public class CorpsController : Controller
    {
        private readonly ForceDBContext _context;

        public CorpsController(ForceDBContext context)
        {
            _context = context;
        }

        // GET: Corps
        public async Task<IActionResult> Index(string searchString)
        {
            var corps = from m in _context.Corpss
                         select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                corps = corps.Where(s => s.Name.Contains(searchString));
            }

            return View(await corps.ToListAsync());
        }

        // GET: Corps/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corps = await _context.Corpss.Include(f=>f.Divisions).Include(f=>f.Commanders)
                .FirstOrDefaultAsync(m => m.CorpsId == id);
            if (corps == null)
            {
                return NotFound();
            }

            return View(corps);
        }

        // GET: Corps/Create
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
            var corps = await _context.Corpss
                .FirstOrDefaultAsync(m => m.CorpsId == id);
            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {

                Name = corps.Name,
                Number = corps.Number,
                TotalStrenght = corps.TotalStrenght,
                AdditionalInformation = corps.AdditionalInformation,
                Commanders = corps.Commanders,
                Divisions = corps.Divisions,
                Brigades = corps.Brigades,
                Regiments = corps.Regiments,
                Battalions = corps.Battalions,
                BattleFrontId=corps.BattlefrontId,
                OldWeekId = corps.WeekId,
                WeekId = corps.WeekId,
                Image = corps.Image,
                Week = corps.Week,
                Coordinates = corps.CoordinatesXY,
                CoordX = corps.CoordX,
                CoordY = corps.CoordY,
                Adress = corps.Adress
            };
            return View(combatForm);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Copy(int id, CombatFormationViewModel combatForm)
        {
            if (id != combatForm.Id)
            {
                return NotFound();
            }
            Corps corps = new Corps()
            {
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                Divisions = combatForm.Divisions,
                Brigades = combatForm.Brigades,
                Regiments = combatForm.Regiments,
                Battalions = combatForm.Battalions,
                BattlefrontId = combatForm.BattleFrontId,
                WeekId = combatForm.WeekId,
                Image = combatForm.Image,
                Week = combatForm.Week,
                CoordinatesXY = combatForm.Coordinates,
                CoordX = combatForm.CoordX,
                CoordY = combatForm.CoordY,
                Adress = combatForm.Adress
            };
            if (ModelState.IsValid)
            {
                if (combatForm.OldWeekId != corps.WeekId)
                {
                    corps.Commanders = null;
                    corps.Divisions = null;
                    corps.Brigades = null;
                    corps.Regiments = null;
                    corps.Battalions = null;
                    corps.BattlefrontId = null;
                }
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    corps.CoordX = str[0];
                    corps.CoordY = str[1];
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
                    corps.Image = imageData;
                    try
                    {
                        _context.Add(corps);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CorpsExists(corps.CorpsId))
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
                    corps.Image = combatForm.Image;
                    try
                    {
                        _context.Add(corps);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CorpsExists(corps.CorpsId))
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
            return View(corps);
        }



        [Authorize]

        public IActionResult Create()
        {
            SelectList armies = new SelectList(_context.Armies, "ArmyId", "Name");
            ViewBag.Armies = armies;
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            return View();
        }

        // POST: Corps/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Create(CombatFormationViewModel combatForm)
        {
            Corps corps = new Corps()
            {
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                Divisions = combatForm.Divisions,
                Brigades = combatForm.Brigades,
                WeekId = combatForm.WeekId,
                Image = combatForm.Image,
                Week= combatForm.Week,
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
                    corps.CoordX = str[0];
                    corps.CoordY = str[1];
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
                    corps.Image = imageData;
                }
                _context.Add(corps);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                return RedirectToAction("Edit", new { id=corps.CorpsId});
            }
            return View(corps);
        }

        // GET: Corps/Edit/5
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corps = await _context.Corpss.FindAsync(id);
            
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            SelectList armies = new SelectList(_context.Armies.Where(f=>f.WeekId==corps.WeekId), "ArmyId", "Name");
            ViewBag.Armies = armies;
            if (corps == null)
            {
                return NotFound();
            }
            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                Id=corps.CorpsId,
                Name = corps.Name,
                Number = corps.Number,
                TotalStrenght = corps.TotalStrenght,
                AdditionalInformation = corps.AdditionalInformation,
                Commanders = corps.Commanders,
                Divisions = corps.Divisions,
                Brigades = corps.Brigades,
                Regiments=corps.Regiments,
                Battalions=corps.Battalions,
                BattleFrontId=corps.BattlefrontId,
                OldWeekId=corps.WeekId,
                WeekId = corps.WeekId,
                Image = corps.Image,
                Week = corps.Week,
                Coordinates = corps.CoordinatesXY,
                CoordX = corps.CoordX,
                CoordY = corps.CoordY,
                Adress = corps.Adress
            };
            return View(combatForm);
        }

        // POST: Corps/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Edit(int id, CombatFormationViewModel combatForm)
        {
            if (id != combatForm.Id)
            {
                return NotFound();
            }
            Corps corps = new Corps()
            {
                CorpsId=combatForm.Id,
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                Divisions = combatForm.Divisions,
                Brigades = combatForm.Brigades,
                Regiments = combatForm.Regiments,
                Battalions = combatForm.Battalions,
                BattlefrontId=combatForm.BattleFrontId,
                WeekId = combatForm.WeekId,
                Image = combatForm.Image,
                Week = combatForm.Week,
                CoordinatesXY = combatForm.Coordinates,
                CoordX = combatForm.CoordX,
                CoordY = combatForm.CoordY,
                Adress = combatForm.Adress
            };
            if (ModelState.IsValid)
            {
                if(combatForm.OldWeekId!=corps.WeekId)
                {
                    corps.Commanders = null;
                    corps.Divisions = null;
                    corps.Brigades = null;
                    corps.Regiments = null;
                    corps.Battalions = null;
                    corps.BattlefrontId = null;
                }
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    corps.CoordX = str[0];
                    corps.CoordY = str[1];
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
                    corps.Image = imageData;
                    try
                    {
                        _context.Update(corps);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CorpsExists(corps.CorpsId))
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
                    corps.Image = combatForm.Image;
                    try
                    {
                        _context.Update(corps);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CorpsExists(corps.CorpsId))
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
            return View(corps);
        }

        // GET: Corps/Delete/5
        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var corps = await _context.Corpss
                .FirstOrDefaultAsync(m => m.CorpsId == id);
            if (corps == null)
            {
                return NotFound();
            }

            return View(corps);
        }

        // POST: Corps/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var corps = await _context.Corpss.FindAsync(id);
            _context.Corpss.Remove(corps);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CorpsExists(int id)
        {
            return _context.Corpss.Any(e => e.CorpsId == id);
        }
    }
}

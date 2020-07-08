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
    public class DivisionsController : Controller
    {
        private readonly ForceDBContext _context;

        public DivisionsController(ForceDBContext context)
        {
            _context = context;
        }

        // GET: Divisions
        public async Task<IActionResult> Index(string searchString)
        {
            var Divisions = from m in _context.Divisions
                        select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                Divisions = Divisions.Where(s => s.Name.Contains(searchString));
            }

            return View(await Divisions.ToListAsync());
        }
        // GET: Divisions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.Divisions.Include(d=>d.Brigades).Include(d=>d.Commanders)
                .FirstOrDefaultAsync(m => m.DivisionId == id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        // GET: Divisions/Create
        [Authorize]

        public IActionResult Create()
        {
            SelectList corpuslist = new SelectList(_context.Corpss, "CorpsId", "Name");
            ViewBag.corpusList = corpuslist;
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            //SelectList fronts = new SelectList(_context.BattleFronts, "BattleFrontId", "Name");
            //ViewBag.Fronts = fronts;
            return View();
        }

        // POST: Divisions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Create(CombatFormationViewModel combatForm)
        {
            Division division = new Division()
            {
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                Brigades = combatForm.Brigades,
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
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    division.CoordX = str[0];
                    division.CoordY = str[1];
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
                    division.Image = imageData;
                }
                _context.Add(division);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = division.DivisionId });
            }
            return View(division);
        }

        // GET: Divisions/Edit/5
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.Divisions.FindAsync(id);

            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            SelectList corpuslist = new SelectList(_context.Corpss.Where(f => f.WeekId == division.WeekId), "CorpsId", "Name");
            ViewBag.corpusList = corpuslist;
            if (division == null)
            {
                return NotFound();
            }
            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                Id = division.DivisionId,
                Name = division.Name,
                Number = division.Number,
                TotalStrenght = division.TotalStrenght,
                AdditionalInformation = division.AdditionalInformation,
                Commanders = division.Commanders,
                Brigades = division.Brigades,
                Regiments=division.Regiments,
                ArmyId=division.ArmyId,
                CorpsId = division.CorpsId,
                WeekId = division.WeekId,
                OldWeekId=division.WeekId,
                Image = division.Image,
                Week = division.Week,
                Coordinates = division.CoordinatesXY,
                CoordX = division.CoordX,
                CoordY = division.CoordY,
                Adress = division.Adress
            };
            return View(combatForm);
        }


        // POST: Divisions/Edit/5
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
            Division division = new Division()
            {
                DivisionId = combatForm.Id,
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                CorpsId=combatForm.CorpsId,
                ArmyId=combatForm.ArmyId,
                Brigades = combatForm.Brigades,
                Regiments=combatForm.Regiments,
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
                if (combatForm.OldWeekId != division.WeekId)
                {
                    division.Commanders = null;
                    division.ArmyId = null;
                    division.Brigades = null;
                    division.CorpsId = null;
                    division.Regiments = null;
                }
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    division.CoordX = str[0];
                    division.CoordY = str[1];
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
                    division.Image = imageData;
                    try
                    {
                        _context.Update(division);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DivisionExists(division.DivisionId))
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
                    division.Image = combatForm.Image;
                    try
                    {
                        _context.Update(division);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DivisionExists(division.DivisionId))
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
            return View(division);
        }

        // GET: Divisions/Delete/5
        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var division = await _context.Divisions
                .FirstOrDefaultAsync(m => m.DivisionId == id);
            if (division == null)
            {
                return NotFound();
            }

            return View(division);
        }

        // POST: Divisions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var division = await _context.Divisions.FindAsync(id);
            _context.Divisions.Remove(division);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DivisionExists(int id)
        {
            return _context.Divisions.Any(e => e.DivisionId == id);
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

            var division = await _context.Divisions
                .FirstOrDefaultAsync(m => m.DivisionId == id);

            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                Id = division.DivisionId,
                Name = division.Name,
                Number = division.Number,
                TotalStrenght = division.TotalStrenght,
                AdditionalInformation = division.AdditionalInformation,
                Commanders = division.Commanders,
                Brigades = division.Brigades,
                Regiments = division.Regiments,
                ArmyId = division.ArmyId,
                CorpsId = division.CorpsId,
                WeekId = division.WeekId,
                OldWeekId = division.WeekId,
                Image = division.Image,
                Week = division.Week,
                Coordinates = division.CoordinatesXY,
                CoordX = division.CoordX,
                CoordY = division.CoordY,
                Adress = division.Adress
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
            Division division = new Division()
            {
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                CorpsId = combatForm.CorpsId,
                ArmyId = combatForm.ArmyId,
                Brigades = combatForm.Brigades,
                Regiments = combatForm.Regiments,
                Image = combatForm.Image,
                Week = combatForm.Week,
                CoordinatesXY = combatForm.Coordinates,
                CoordX = combatForm.CoordX,
                CoordY = combatForm.CoordY,
                Adress = combatForm.Adress
            };
            if (ModelState.IsValid)
            {
                if (combatForm.OldWeekId != division.WeekId)
                {
                    division.Commanders = null;
                    division.ArmyId = null;
                    division.Brigades = null;
                    division.CorpsId = null;
                    division.Regiments = null;
                }
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    division.CoordX = str[0];
                    division.CoordY = str[1];
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
                    division.Image = imageData;
                    try
                    {
                        _context.Add(division);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DivisionExists(division.DivisionId))
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
                    division.Image = combatForm.Image;
                    try
                    {
                        _context.Add(division);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!DivisionExists(division.DivisionId))
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
            return View(division);
        }
    }
}

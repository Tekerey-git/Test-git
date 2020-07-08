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
    public class BrigadesController : Controller
    {
        private readonly ForceDBContext _context;

        public BrigadesController(ForceDBContext context)
        {
            _context = context;
        }

        // GET: Brigades
        public async Task<IActionResult> Index(string searchString)
        {
            var Brigades = from m in _context.Brigades
                            select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                Brigades = Brigades.Where(s => s.Name.Contains(searchString));
            }

            return View(await Brigades.ToListAsync());
        }

        // GET: Brigades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brigade = await _context.Brigades.Include(b=>b.Regiments).Include(b=>b.Commanders)
                .FirstOrDefaultAsync(m => m.BrigadeId == id);
            if (brigade == null)
            {
                return NotFound();
            }

            return View(brigade);
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
            var brigade = await _context.Brigades
                .FirstOrDefaultAsync(m => m.BrigadeId == id);

            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                Name = brigade.Name,
                Number = brigade.Number,
                TotalStrenght = brigade.TotalStrenght,
                AdditionalInformation = brigade.AdditionalInformation,
                Commanders = brigade.Commanders,
                Battalions = brigade.Battalions,
                Regiments = brigade.Regiments,
                DivisionId = brigade.DivisionId,
                ArmyId = brigade.ArmyId,
                CorpsId = brigade.CorpsId,
                WeekId = brigade.WeekId,
                OldWeekId = brigade.WeekId,
                Image = brigade.Image,
                Week = brigade.Week,
                Coordinates = brigade.CoordinatesXY,
                CoordX = brigade.CoordX,
                CoordY = brigade.CoordY,
                Adress = brigade.Adress
            };
            return View(combatForm);

        }

        

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Copy(int id, CombatFormationViewModel combatForm)
        {
            Brigade brigade = new Brigade()
            {
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                Regiments = combatForm.Regiments,
                DivisionId = combatForm.DivisionId,
                CorpsId = combatForm.CorpsId,
                Battalions = combatForm.Battalions,
                ArmyId=combatForm.ArmyId,
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
                if (combatForm.OldWeekId != brigade.WeekId)
                {
                    brigade.Commanders = null;
                    brigade.ArmyId = null;
                    brigade.Battalions = null;
                    brigade.CorpsId = null;
                    brigade.Regiments = null;
                    brigade.DivisionId = null;
                }
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    brigade.CoordX = str[0];
                    brigade.CoordY = str[1];
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
                    brigade.Image = imageData;
                    try
                    {
                        _context.Add(brigade);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BrigadeExists(brigade.BrigadeId))
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
                    brigade.Image = combatForm.Image;
                    try
                    {
                        _context.Add(brigade);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BrigadeExists(brigade.BrigadeId))
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
            return View(brigade);
        }


        // GET: Brigades/Create
        [Authorize]
        [HttpGet]
        public IActionResult Create()
        {
            SelectList divList = new SelectList(_context.Divisions, "DivisionId", "Name");
            ViewBag.divList = divList;
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            return View();
        }
        // POST: Brigades/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Create(CombatFormationViewModel combatForm)
        {
            Brigade brigade = new Brigade()
            {
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                Regiments = combatForm.Regiments,
                WeekId = combatForm.WeekId,
                DivisionId=combatForm.DivisionId,
                ArmyId=combatForm.ArmyId,
                CorpsId=combatForm.CorpsId,
                Image = combatForm.Image,
                Week = combatForm.Week,
                CoordinatesXY= combatForm.Coordinates,
                CoordX = combatForm.CoordX,
                CoordY = combatForm.CoordY,
                Adress = combatForm.Adress
            };
            if (ModelState.IsValid)
            {
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    brigade.CoordX = str[0];
                    brigade.CoordY = str[1];
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
                    brigade.Image = imageData;
                }
                _context.Add(brigade);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = brigade.BrigadeId});
            }
            return View(brigade);
        }

        // GET: Brigades/Edit/5
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brigade = await _context.Brigades.FindAsync(id);
            
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            SelectList divList = new SelectList(_context.Divisions.Where(f=>f.WeekId==brigade.WeekId), "DivisionId", "Name");
            ViewBag.divList = divList;

            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                Id=brigade.BrigadeId,
                Name = brigade.Name,
                Number = brigade.Number,
                TotalStrenght = brigade.TotalStrenght,
                AdditionalInformation = brigade.AdditionalInformation,
                Commanders = brigade.Commanders,
                Battalions = brigade.Battalions,
                Regiments=brigade.Regiments,
                DivisionId = brigade.DivisionId,
                ArmyId=brigade.ArmyId,
                CorpsId=brigade.CorpsId,
                WeekId = brigade.WeekId,
                OldWeekId=brigade.WeekId,
                Image = brigade.Image,
                Week = brigade.Week,
                Coordinates = brigade.CoordinatesXY,
                CoordX = brigade.CoordX,
                CoordY = brigade.CoordY,
                Adress = brigade.Adress
            };
            
            return View(combatForm);
        }

        // POST: Brigades/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Edit(int id, CombatFormationViewModel combatForm)
        {
            Brigade brigade = new Brigade()
            {
                BrigadeId=combatForm.Id,
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                Regiments = combatForm.Regiments,
                DivisionId=combatForm.DivisionId,
                CorpsId=combatForm.CorpsId,
                WeekId = combatForm.WeekId,
                Battalions = combatForm.Battalions,
                Image = combatForm.Image,
                Week = combatForm.Week,
                CoordinatesXY= combatForm.Coordinates,
                CoordX = combatForm.CoordX,
                CoordY = combatForm.CoordY,
                Adress = combatForm.Adress
            };
            if (id != brigade.BrigadeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (combatForm.OldWeekId != brigade.WeekId)
                {
                    brigade.Commanders = null;
                    brigade.ArmyId = null;
                    brigade.Battalions = null;
                    brigade.CorpsId = null;
                    brigade.Regiments = null;
                    brigade.DivisionId = null;
                }
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    brigade.CoordX = str[0];
                    brigade.CoordY = str[1];
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
                    brigade.Image = imageData;
                    try
                    {
                        _context.Update(brigade);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BrigadeExists(brigade.BrigadeId))
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
                    brigade.Image = combatForm.Image;
                    try
                    {
                        _context.Update(brigade);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BrigadeExists(brigade.BrigadeId))
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
            return View(brigade);
        }

        // GET: Brigades/Delete/5
        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var brigade = await _context.Brigades
                .FirstOrDefaultAsync(m => m.BrigadeId == id);
            if (brigade == null)
            {
                return NotFound();
            }

            return View(brigade);
        }

        // POST: Brigades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var brigade = await _context.Brigades.FindAsync(id);
            _context.Brigades.Remove(brigade);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BrigadeExists(int id)
        {
            return _context.Brigades.Any(e => e.BrigadeId == id);
        }
    }
}

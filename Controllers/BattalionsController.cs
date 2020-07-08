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
    public class BattalionsController : Controller
    {
        private readonly ForceDBContext _context;
        public int? temp;
        public BattalionsController(ForceDBContext context)
        {
            _context = context;
        }

        // GET: Battalions
        public async Task<IActionResult> Index(string searchString)
        {
            var Battalions = from m in _context.Battalions
                            select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                Battalions = Battalions.Where(s => s.Name.Contains(searchString));
            }

            return View(await Battalions.ToListAsync());
        }

        // GET: Battalions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var battalion = await _context.Battalions
                .FirstOrDefaultAsync(m => m.BattalionId == id);
            if (battalion == null)
            {
                return NotFound();
            }

            return View(battalion);
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
            var battalion = await _context.Battalions
                .FirstOrDefaultAsync(m => m.BattalionId == id);

            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                Name = battalion.Name,
                Number = battalion.Number,
                TotalStrenght = battalion.TotalStrenght,
                AdditionalInformation = battalion.AdditionalInformation,
                Commanders = battalion.Commanders,
                RegimentId = battalion.RegimentId,
                WeekId = battalion.WeekId,
                Image = battalion.Image,
                Week = battalion.Week,
                OldWeekId=battalion.WeekId,
                Coordinates=battalion.CoordinatesXY,
                CoordX=battalion.CoordX,
                CoordY=battalion.CoordY,
                Adress=battalion.Adress
            };
            return View(combatForm);

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Copy(int id, CombatFormationViewModel combatForm)
        {
            Battalion battalion = new Battalion()
            {
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                WeekId = combatForm.WeekId,
                RegimentId = combatForm.RegimentId,
                Image = combatForm.Image,
                Week = combatForm.Week,
                CoordinatesXY  = combatForm.Coordinates,
                CoordX = combatForm.CoordX,
                CoordY = combatForm.CoordY,
                Adress = combatForm.Adress
            };

            if (ModelState.IsValid)
            {
                if(combatForm.OldWeekId!=battalion.WeekId)
                {
                    battalion.RegimentId = null;
                    battalion.Commanders = null;
                }
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    battalion.CoordX = str[0];
                    battalion.CoordY = str[1];
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
                    battalion.Image = imageData;
                    try
                    {
                        _context.Add(battalion);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BattalionExists(battalion.BattalionId))
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
                    battalion.Image = combatForm.Image;
                    try
                    {
                        _context.Add(battalion);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BattalionExists(battalion.BattalionId))
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
            return View(battalion);
        }

        // GET: Battalions/Create
        [Authorize]
        public IActionResult Create()
        {
            SelectList regList = new SelectList(_context.Regiments, "RegimentId", "Name");
            ViewBag.regList = regList;
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            return View();
        }

        // POST: Battalions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Create(CombatFormationViewModel combatForm)
        {
            Battalion battalion = new Battalion()
            {
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                WeekId = combatForm.WeekId,
                RegimentId = combatForm.RegimentId,
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
                    battalion.CoordX = str[0];
                    battalion.CoordY = str[1];
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
                    battalion.Image = imageData;
                }
                _context.Add(battalion);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id =battalion.BattalionId });
            }
            return View(battalion);
        }

        // GET: Battalions/Edit/5
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var battalion = await _context.Battalions.FindAsync(id);
            SelectList regList = new SelectList(_context.Regiments.Where(f=>f.WeekId==battalion.WeekId), "RegimentId", "Name");
            ViewBag.regList = regList;
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            if (battalion == null)
            {
                return NotFound();
            }
            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                Id = battalion.BattalionId,
                Name = battalion.Name,
                Number = battalion.Number,
                TotalStrenght = battalion.TotalStrenght,
                AdditionalInformation = battalion.AdditionalInformation,
                Commanders = battalion.Commanders,
                RegimentId = battalion.RegimentId,
                WeekId = battalion.WeekId,
                OldWeekId=battalion.WeekId,
                Image = battalion.Image,
                Week = battalion.Week,
                Coordinates = battalion.CoordinatesXY,
                CoordX = battalion.CoordX,
                CoordY = battalion.CoordY,
                Adress = battalion.Adress
            };
            return View(combatForm);
        }

        // POST: Battalions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Edit(int id, CombatFormationViewModel combatForm)
        {
            Battalion battalion = new Battalion()
            {
                BattalionId=combatForm.Id,
                Name = combatForm.Name,
                Number = combatForm.Number,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation = combatForm.AdditionalInformation,
                Commanders = combatForm.Commanders,
                WeekId = combatForm.WeekId,
                RegimentId = combatForm.RegimentId,
                Image = combatForm.Image,
                //Week = combatForm.Week,
                CoordinatesXY = combatForm.Coordinates,
                CoordX = combatForm.CoordX,
                CoordY = combatForm.CoordY,
                Adress = combatForm.Adress
            };
            if(combatForm.OldWeekId != battalion.WeekId)
            {
                battalion.RegimentId = null;
                battalion.Commanders = null;
            }
            if (id != battalion.BattalionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    battalion.CoordX = str[0];
                    battalion.CoordY = str[1];
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
                    battalion.Image = imageData;
                    try
                    {
                        _context.Update(battalion);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BattalionExists(battalion.BattalionId))
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
                    battalion.Image = combatForm.Image;
                    try
                    {
                        _context.Update(battalion);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BattalionExists(battalion.BattalionId))
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
            return View(battalion);
        }

        // GET: Battalions/Delete/5
        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var battalion = await _context.Battalions
                .FirstOrDefaultAsync(m => m.BattalionId == id);
            if (battalion == null)
            {
                return NotFound();
            }

            return View(battalion);
        }

        // POST: Battalions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var battalion = await _context.Battalions.FindAsync(id);
            _context.Battalions.Remove(battalion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BattalionExists(int id)
        {
            return _context.Battalions.Any(e => e.BattalionId == id);
        }
    }
}

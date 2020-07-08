using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HistorySiteIdentity.ViewModels;
using System.IO;

namespace HistorySiteIdentity.Models
{
    public class ArmiesController : Controller
    {
        private readonly ForceDBContext _context;
        
        
        
        public ArmiesController(ForceDBContext context)
        {
            _context = context;
            
        }


        // GET: Armies
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.Armies.ToListAsync());
        //}
        public async Task<IActionResult> Index(string searchString)
        {
            var armies = from m in _context.Armies
                               select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                armies = armies.Where(s => s.Name.Contains(searchString));
            }

            return View(await armies.ToListAsync());
        }
        // GET: Armies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var army = await _context.Armies.Include(f => f.Commanders).Include(f => f.Corps).Include(f=>f.Week).Include(f=>f.BattleFront)
                .FirstOrDefaultAsync(m => m.ArmyId == id);
            if (army == null)
            {
                return NotFound();
            }

            return View(army);
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
            var army = await _context.Armies
                .FirstOrDefaultAsync(m => m.ArmyId == id);
            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {

                Name = army.Name,
                TotalStrenght = army.TotalStrenght,
                AdditionalInformation=army.AdditionalInformation,
                Number = army.Number,
                Commanders = army.Commanders,
                Corps = army.Corps,
                Divisions = army.Divisions,
                Brigades = army.Brigades,
                BattleFrontId =army.BattleFrontId,
                OldWeekId=army.WeekId,
                WeekId = army.WeekId,
                Image = army.Image,
                Coordinates=army.CoordinatesXY,
                CoordX=army.CoordX,
                CoordY=army.CoordY,
                Adress = army.Adress
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
            Army army = new Army()
            {

                Name = combatForm.Name,
                TotalStrenght = combatForm.TotalStrenght,
                AdditionalInformation=combatForm.AdditionalInformation,
                Number = combatForm.Number,
                Commanders = combatForm.Commanders,
                Corps = combatForm.Corps,
                Divisions = combatForm.Divisions,
                Brigades = combatForm.Brigades,
                BattleFrontId = combatForm.BattleFrontId,
                WeekId = combatForm.WeekId,
                Image = combatForm.Image,
                CoordinatesXY=combatForm.Coordinates,
                CoordX=combatForm.CoordX,
                CoordY=combatForm.CoordY,
                Adress = combatForm.Adress
            };
            if (ModelState.IsValid)
            {
                if(combatForm.OldWeekId!=army.WeekId)
                {
                    army.BattleFrontId = null;
                    army.Corps = null;
                    army.Divisions = null;
                    army.Brigades = null;
                    army.Commanders = null;
                }
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    army.CoordX = str[0];
                    army.CoordY = str[1];
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
                    army.Image = imageData;
                    try
                    {
                        _context.Add(army);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ArmyExists(army.ArmyId))
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
                    army.Image = combatForm.Image;
                    try
                    {
                        _context.Add(army);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ArmyExists(army.ArmyId))
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
            return View(army);
        }
        // GET: Armies/Create

        [Authorize]

        public IActionResult Create()
        {
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            //SelectList fronts = new SelectList(_context.BattleFronts, "BattleFrontId", "Name");
            //ViewBag.Fronts = fronts;
            return View();
        }

        // POST: Armies/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Create(CombatFormationViewModel combatForm)
        {

            Army army = new Army()
            {
                Name = combatForm.Name,
                TotalStrenght = combatForm.TotalStrenght,
                Number = combatForm.Number,
                Commanders = combatForm.Commanders,
                Corps = combatForm.Corps,
                Divisions = combatForm.Divisions,
                Brigades = combatForm.Brigades,
                BattleFrontId = combatForm.BattleFrontId,
                WeekId = combatForm.WeekId,
                Image = combatForm.Image,
                AdditionalInformation = combatForm.AdditionalInformation,
                CoordinatesXY = combatForm.Coordinates,
                Adress=combatForm.Adress
                
                
            };
            if (ModelState.IsValid)
            {
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    army.CoordX = str[0];
                    army.CoordY = str[1];
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
                    army.Image = imageData;
                }
                _context.Add(army);
                await _context.SaveChangesAsync();
                return RedirectToAction("Edit", new { id = army.ArmyId });
                //return RedirectToAction(nameof(Index));
            }

            return View(army);
        }

        // GET: Armies/Edit/5
        [HttpGet]
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var army = await _context.Armies.FindAsync(id);
            if (army == null)
            {
                return NotFound();
            }
            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                Name = army.Name,
                TotalStrenght = army.TotalStrenght,
                Number = army.Number,
                Commanders = army.Commanders,
                Corps = army.Corps,
                Divisions = army.Divisions,
                Brigades = army.Brigades,
                BattleFrontId = army.BattleFrontId,
                OldWeekId=army.WeekId,
                WeekId = army.WeekId,
                Image = army.Image,
                AdditionalInformation=army.AdditionalInformation,
                Coordinates=army.CoordinatesXY,
                Adress=army.Adress,
                CoordX=army.CoordX,
                CoordY=army.CoordY
            };
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            ViewBag.OldWeek = army.WeekId;
            SelectList fronts = new SelectList(_context.BattleFronts.Where(f=>f.WeekId==army.WeekId), "BattleFrontId", "Name");
            ViewBag.Fronts = fronts;
            return View(combatForm);
        }

        // POST: Armies/Edit/5
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
            Army army = new Army()
            {
                ArmyId=combatForm.Id,
                Name = combatForm.Name,
                TotalStrenght = combatForm.TotalStrenght,
                Number = combatForm.Number,
                Commanders = combatForm.Commanders,
                Corps = combatForm.Corps,
                Divisions = combatForm.Divisions,
                Brigades = combatForm.Brigades,
                BattleFrontId = combatForm.BattleFrontId,
                AdditionalInformation=combatForm.AdditionalInformation,
                WeekId = combatForm.WeekId,
                Image = combatForm.Image,
                CoordinatesXY=combatForm.Coordinates,
                Adress=combatForm.Adress,
                CoordX=combatForm.CoordX,
                CoordY=combatForm.CoordY
            };
            if (ModelState.IsValid)
            {
                if(combatForm.OldWeekId!=army.WeekId)
                {
                    army.Commanders = null;
                    army.Corps = null;
                    army.Divisions = null;
                    army.Brigades = null;
                    army.BattleFrontId = null;
                }
                if (combatForm.Coordinates != null)
                {
                    string[] str = combatForm.Coordinates.Split(',', 2, StringSplitOptions.None);
                    army.CoordX = str[0];
                    army.CoordY = str[1];
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
                    army.Image = imageData;
                    try
                    {
                        _context.Update(army);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ArmyExists(army.ArmyId))
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
                    army.Image = combatForm.Image;
                    try
                    {
                        _context.Update(army);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!ArmyExists(army.ArmyId))
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
            return View(army);
        }

        // GET: Armies/Delete/5
        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var army = await _context.Armies
                .FirstOrDefaultAsync(m => m.ArmyId == id);
            if (army == null)
            {
                return NotFound();
            }

            return View(army);
        }

        // POST: Armies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var army = await _context.Armies.FindAsync(id);
            _context.Armies.Remove(army);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArmyExists(int id)
        {
            return _context.Armies.Any(e => e.ArmyId == id);
        }
    }
}

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

namespace HistorySiteIdentity
{
    public class BattleFrontsController : Controller
    {
        private readonly ForceDBContext _context;
        
        public BattleFrontsController(ForceDBContext context)
        {
            _context = context;
        }

        // GET: BattleFronts
        //public async Task<IActionResult> Index()
        //{
        //    return View(await _context.BattleFronts.ToListAsync());
        //}

        public async Task<IActionResult> Index(string searchString)
        {
            var battleFronts = from m in _context.BattleFronts
                                select m;

            if (!String.IsNullOrEmpty(searchString))
            {
                battleFronts = battleFronts.Where(s => s.Name.Contains(searchString));
            }

            return View(await battleFronts.ToListAsync());
        }
        [HttpPost]
        public string Index(string searchString, bool notUsed)
        {
            return "From [HttpPost]Index: filter on " + searchString;
        }

        // GET: BattleFronts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var battleFront = await _context.BattleFronts.Include(f=>f.Armies).Include(f=>f.Commanders)
                .FirstOrDefaultAsync(m => m.BattleFrontId == id);
           
            if (battleFront == null)
            {
                return NotFound();
            }
            //SelectList ArmList = new SelectList(_context.Armies, "BattleFrontId", "Name");
            //ViewBag.AL = ArmList;

            //var BFwithArmies = _context.BattleFronts.Include(u => u.Armies).ToList();
            //ViewBag.BFL = BFwithArmies;

            //var armies = _context.BattleFronts.Include(u => u.Armies).ToList();
            //ViewBag.BFL = armies;
            //SelectList fronts = new SelectList(_context.BattleFrontContext, "BattleFrontId", "Name");
            //ViewBag.Fronts = fronts;
            return View(battleFront);
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
            var battleFront = await _context.BattleFronts
                .FirstOrDefaultAsync(m => m.BattleFrontId == id);
            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                
                Name = battleFront.Name,
                TotalStrenght = battleFront.TotalStrenght,
                Commanders = battleFront.Commanders,
                Armies = battleFront.Armies,
                WeekId = battleFront.WeekId,
                Image = battleFront.Image,
                Coordinates = battleFront.CoordinatesXY,
                CoordX = battleFront.CoordX,
                CoordY = battleFront.CoordY,
                Adress = battleFront.Adress
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
            BattleFront battleFront = new BattleFront()
            {
                
                Name = combatForm.Name,
                TotalStrenght = combatForm.TotalStrenght,
                Commanders = combatForm.Commanders,
                Armies = combatForm.Armies,
                WeekId = combatForm.WeekId,
                Image = combatForm.Image,
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
                    battleFront.CoordX = str[0];
                    battleFront.CoordY = str[1];
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
                    battleFront.Image = imageData;
                    try
                    {
                        _context.Add(battleFront);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BattleFrontExists(battleFront.BattleFrontId))
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
                    battleFront.Image = combatForm.Image;
                    try
                    {
                        _context.Add(battleFront);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BattleFrontExists(battleFront.BattleFrontId))
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
            return View(battleFront);
        }


        // GET: BattleFronts/Create
        [Authorize]

        public IActionResult Create()
        {
            SelectList weeks = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.weeks = weeks;
            
            return View();
        }

        // POST: BattleFronts/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Create(CombatFormationViewModel combatForm)
        {
            BattleFront battleFront = new BattleFront()
            {
                //BattleFrontId = combatForm.Id,
                Name = combatForm.Name,
                TotalStrenght = combatForm.TotalStrenght,
                Commanders = combatForm.Commanders,
                Armies = combatForm.Armies,
                WeekId = combatForm.WeekId,
                Image = combatForm.Image,
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
                    battleFront.CoordX = str[0];
                    battleFront.CoordY = str[1];
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
                    battleFront.Image = imageData;
                }
                _context.Add(battleFront);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            
            return View(battleFront);
        }

        // GET: BattleFronts/Edit/5
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var battleFront = await _context.BattleFronts.FindAsync(id);
            SelectList week = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.week = week;
            if (battleFront == null)
            {
                return NotFound();
            }
            CombatFormationViewModel combatForm = new CombatFormationViewModel()
            {
                Id = battleFront.BattleFrontId,
                Name = battleFront.Name,
                TotalStrenght = battleFront.TotalStrenght,
                Commanders = battleFront.Commanders,
                Armies = battleFront.Armies,
                WeekId = battleFront.WeekId,
                Image = battleFront.Image,
                Coordinates = battleFront.CoordinatesXY,
                CoordX = battleFront.CoordX,
                CoordY = battleFront.CoordY,
                Adress = battleFront.Adress
            };
            return View(combatForm);
        }

        // POST: BattleFronts/Edit/5
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
            BattleFront battleFront = new BattleFront()
            {
                BattleFrontId = combatForm.Id,
                Name = combatForm.Name,
                TotalStrenght = combatForm.TotalStrenght,
                Commanders = combatForm.Commanders,
                Armies = combatForm.Armies,
                WeekId = combatForm.WeekId,
                Image = combatForm.Image,
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
                    battleFront.CoordX = str[0];
                    battleFront.CoordY = str[1];
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
                    battleFront.Image = imageData;
                    try
                    {
                        _context.Update(battleFront);
                        await _context.SaveChangesAsync();
                        return RedirectToAction(nameof(Index));
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BattleFrontExists(battleFront.BattleFrontId))
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
                    battleFront.Image = combatForm.Image;
                    try
                    {
                        _context.Update(battleFront);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!BattleFrontExists(battleFront.BattleFrontId))
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
            return View(battleFront);
        }

        // GET: BattleFronts/Delete/5
        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var battleFront = await _context.BattleFronts
                .FirstOrDefaultAsync(m => m.BattleFrontId == id);
            if (battleFront == null)
            {
                return NotFound();
            }

            return View(battleFront);
        }

        // POST: BattleFronts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var battleFront = await _context.BattleFronts.FindAsync(id);
            _context.BattleFronts.Remove(battleFront);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index)); //nameof
        }

        private bool BattleFrontExists(int id)
        {
            return _context.BattleFronts.Any(e => e.BattleFrontId == id);
        }
    }
}

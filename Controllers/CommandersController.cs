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
    public class CommandersController : Controller
    {
        private readonly ForceDBContext _context;

        public CommandersController(ForceDBContext context)
        {
            _context = context;
        }
        
        // GET: Commanders
        public async Task<IActionResult> Index()
        {
            var forceDBContext = _context.Commander.Include(c => c.Week);
            return View(await forceDBContext.ToListAsync());
        }

        // GET: Commanders/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commander = await _context.Commander
                .Include(c => c.Week)
                .Include(c=>c.BattleFront)
                .Include(c => c.Army)
                .Include(c => c.Corps)
                .Include(c => c.Division)
                .Include(c => c.Brigade)
                .Include(c => c.Regiment)
                .Include(c => c.Battalion)
                .FirstOrDefaultAsync(m => m.id == id);
            if (commander == null)
            {
                return NotFound();
            }

            return View(commander);
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Copy(int? id)
        {
            SelectList weeks = new SelectList(_context.Week, "WeekId", "WeekNumber");
            ViewBag.weeks = weeks;
            if (id == null)
            {
                return NotFound();
            }
            var commander = await _context.Commander
                .FirstOrDefaultAsync(m => m.id == id);

            CommanderViewModel commanderVM = new CommanderViewModel()
            {
                Name = commander.Name,
                Surname = commander.Surname,
                Paronymic = commander.Paronymic,
                DateOfBirth = commander.DateOfBirth,
                Position = commander.Position,
                AdditionalInformation = commander.AdditionalInformation,
                //WeekId = commander.WeekId,
                Image = commander.Image
            };
            //Array.Copy(commander.Image, commanderVM.Image, commander.Image.Length);
            return View(commanderVM);

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Copy([Bind("Name,Surname,Paronymic,DateOfBirth,Position,AdditionalInformation,WeekId,Image")] CommanderViewModel commanderVM)
        {


            Commander commander = new Commander()
            {
                Name = commanderVM.Name,
                Surname = commanderVM.Surname,
                Paronymic = commanderVM.Paronymic,
                DateOfBirth = commanderVM.DateOfBirth,
                Position = commanderVM.Position,
                AdditionalInformation = commanderVM.AdditionalInformation,
                WeekId = commanderVM.WeekId,
                Image = commanderVM.Image
            };
            /**/
            if (ModelState.IsValid)
            {
                if (commanderVM.File != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(commanderVM.File.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)commanderVM.File.Length);
                    }
                    // установка массива байтов
                    commander.Image = imageData;
                    _context.Add(commander);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                else if(commanderVM.File==null)
                {
                    commander.Image = commanderVM.Image;
                    _context.Add(commander);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
           
            ViewData["WeekId"] = new SelectList(_context.Week, "WeekId", "WeekId", commander.WeekId);
            return View(commander);
        }
        // GET: Commanders/Create
        [Authorize]

        public IActionResult Create()
        {
            ViewData["WeekId"] = new SelectList(_context.Week, "WeekId", "WeekId");
            //SelectList fronts = new SelectList(_context.BattleFronts, "BattleFrontId", "Name");
            //ViewBag.Fronts = fronts;
            //SelectList armies = new SelectList(_context.Armies, "ArmyId", "Name");
            //ViewBag.Armies = armies;
            //SelectList corpuslist = new SelectList(_context.Corpss, "CorpsId", "Name");
            //ViewBag.corpusList = corpuslist;
            //SelectList divList = new SelectList(_context.Divisions, "DivisionId", "Name");
            //ViewBag.divList = divList;
            //SelectList brigList = new SelectList(_context.Brigades, "BrigadeId", "Name");
            //ViewBag.brigList = brigList;
            //SelectList regList = new SelectList(_context.Regiments, "RegimentId", "Name");
            //ViewBag.regList = regList;
            //SelectList battalionList = new SelectList(_context.Battalions, "BattalionId", "Name");
            //ViewBag.battalionList = battalionList;
            return View();
        }

        // POST: Commanders/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Create(CommanderViewModel commanderVM)
        {
            Commander commander = new Commander()
            {
                Name = commanderVM.Name, Surname = commanderVM.Surname, Paronymic = commanderVM.Paronymic,
                DateOfBirth = commanderVM.DateOfBirth, Position = commanderVM.Position, 
                AdditionalInformation = commanderVM.AdditionalInformation, WeekId = commanderVM.WeekId
                //Week = commanderVM.Week
            };
            if (ModelState.IsValid)
            {
                if (commanderVM.File != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(commanderVM.File.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)commanderVM.File.Length);
                    }
                    // установка массива байтов
                    commander.Image = imageData;
                }
                _context.Add(commander);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WeekId"] = new SelectList(_context.Week, "WeekId", "WeekId", commander.WeekId);
            return View(commander);
        }

        // GET: Commanders/Edit/5
        [Authorize]

        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commander = await _context.Commander.FindAsync(id);
            if (commander == null)
            {
                return NotFound();
            }
            CommanderViewModel commanderVM = new CommanderViewModel()
            {
                id=commander.id,
                Name = commander.Name,
                Surname = commander.Surname,
                Paronymic = commander.Paronymic,
                DateOfBirth = commander.DateOfBirth,
                Position = commander.Position,
                AdditionalInformation = commander.AdditionalInformation,
                WeekId = commander.WeekId,
                Image = commander.Image,
                BattleFrontId = commander.BattleFrontId,
                ArmyId=commander.ArmyId,
                CorpsId=commander.CorpsId,
                DivisionId=commander.DivisionId,
                BrigadeId=commander.BrigadeId,
                RegimentId=commander.RegimentId,
                BattalionId=commander.BattalionId
            };

            ViewData["WeekId"] = new SelectList(_context.Week, "WeekId", "WeekId", commander.WeekId);
            SelectList fronts = new SelectList(_context.BattleFronts.Where(f=>f.WeekId==commander.WeekId), "BattleFrontId", "Name");
            ViewBag.Fronts = fronts;
            SelectList armies = new SelectList(_context.Armies.Where(f=>f.WeekId==commander.WeekId), "ArmyId", "Name");
            ViewBag.Armies = armies;
            SelectList corpuslist = new SelectList(_context.Corpss.Where(f=>f.WeekId==commander.WeekId), "CorpsId", "Name");
            ViewBag.corpusList = corpuslist;
            SelectList divList = new SelectList(_context.Divisions.Where(f=>f.WeekId==commander.WeekId), "DivisionId", "Name");
            ViewBag.divList = divList;
            SelectList brigList = new SelectList(_context.Brigades.Where(f=>f.WeekId==commander.WeekId), "BrigadeId", "Name");
            ViewBag.brigList = brigList;
            SelectList regList = new SelectList(_context.Regiments.Where(f => f.WeekId == commander.WeekId), "RegimentId", "Name");
            ViewBag.regList = regList;
            SelectList battalionList = new SelectList(_context.Battalions.Where(f => f.WeekId == commander.WeekId), "BattalionId", "Name");
            ViewBag.battalionList = battalionList;
            return View(commanderVM);
        }

        // POST: Commanders/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> Edit(int id, CommanderViewModel commanderVM)
        {
            if (id != commanderVM.id)
            {
                return NotFound();
            }
            Commander commander = new Commander()
            {
                id=commanderVM.id,
                Name = commanderVM.Name,
                Surname = commanderVM.Surname,
                Paronymic = commanderVM.Paronymic,
                DateOfBirth = commanderVM.DateOfBirth,
                Position = commanderVM.Position,
                AdditionalInformation = commanderVM.AdditionalInformation,
                WeekId = commanderVM.WeekId,
                Image = commanderVM.Image,
                BattleFrontId = commanderVM.BattleFrontId,
                ArmyId = commanderVM.ArmyId,
                CorpsId = commanderVM.CorpsId,
                DivisionId = commanderVM.DivisionId,
                BrigadeId = commanderVM.BrigadeId,
                RegimentId = commanderVM.RegimentId,
                BattalionId = commanderVM.BattalionId
            };

            if (ModelState.IsValid)
            {
                if (commanderVM.File != null)
                {
                    byte[] imageData = null;
                    // считываем переданный файл в массив байтов
                    using (var binaryReader = new BinaryReader(commanderVM.File.OpenReadStream()))
                    {
                        imageData = binaryReader.ReadBytes((int)commanderVM.File.Length);
                    }
                    // установка массива байтов
                    commander.Image = imageData;
                    try
                    {
                        _context.Update(commander);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CommanderExists(commander.id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                else if (commanderVM.File == null)
                {
                    commander.Image = commanderVM.Image;
                    try
                    {
                        _context.Update(commander);
                        await _context.SaveChangesAsync();
                    }
                    catch (DbUpdateConcurrencyException)
                    {
                        if (!CommanderExists(commander.id))
                        {
                            return NotFound();
                        }
                        else
                        {
                            throw;
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["WeekId"] = new SelectList(_context.Week, "WeekId", "WeekId", commander.WeekId);
            return View(commander);
        }

        // GET: Commanders/Delete/5
        [Authorize]

        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var commander = await _context.Commander
                .Include(c => c.Week)
                .FirstOrDefaultAsync(m => m.id == id);
            if (commander == null)
            {
                return NotFound();
            }

            return View(commander);
        }

        // POST: Commanders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize]

        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var commander = await _context.Commander.FindAsync(id);
            _context.Commander.Remove(commander);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool CommanderExists(int id)
        {
            return _context.Commander.Any(e => e.id == id);
        }
        [HttpGet]
        public void TestCopy()
        {
            View();
        }
        [HttpPost]
        public void TestCopy([FromBody]CommanderViewModel commanderVM)
        {
           
        }
    }
}

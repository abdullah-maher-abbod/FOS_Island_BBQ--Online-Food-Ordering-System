using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FOS_ISLAND_BBQ.Data;
using FOS_ISLAND_BBQ.Models;
using Microsoft.AspNetCore.Authorization;

namespace FOS_ISLAND_BBQ
{
    [Authorize]
    public class MenuModelsController : Controller
    {
        private readonly FOS_ISLAND_BBQMenuContext _context;

        public MenuModelsController(FOS_ISLAND_BBQMenuContext context)
        {
            _context = context;
        }

        // GET: MenuModels
        public async Task<IActionResult> Index()
        {
            return View(await _context.MenuModel.ToListAsync());
        }

        // GET: MenuModels/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuModel = await _context.MenuModel
                .FirstOrDefaultAsync(m => m.id == id);
            if (menuModel == null)
            {
                return NotFound();
            }

            return View(menuModel);
        }

        // GET: MenuModels/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: MenuModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([Bind("id,imageURL,name,price")] MenuModel menuModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(menuModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(menuModel);
        }

        // GET: MenuModels/Edit/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id)
        {
            
            if (id == null)
            {
                return NotFound();
            }

            var menuModel = await _context.MenuModel.FindAsync(id);
            if (menuModel == null)
            {
                return NotFound();
            }
            return View(menuModel);
        }

        // POST: MenuModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(string id, [Bind("id,imageURL,name,price")] MenuModel menuModel)
        {
            if (id != menuModel.id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(menuModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MenuModelExists(menuModel.id))
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
            return View(menuModel);
        }

        // GET: MenuModels/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var menuModel = await _context.MenuModel
                .FirstOrDefaultAsync(m => m.id == id);
            if (menuModel == null)
            {
                return NotFound();
            }

            return View(menuModel);
        }

        // POST: MenuModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]

        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var menuModel = await _context.MenuModel.FindAsync(id);
            _context.MenuModel.Remove(menuModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MenuModelExists(string id)
        {
            return _context.MenuModel.Any(e => e.id == id);
        }
    }
}

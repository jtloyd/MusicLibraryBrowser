using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicLibraryBrowser;

namespace MusicLibraryBrowser.Controllers
{
    public class WorkVersionsController : Controller
    {
        private readonly musiclibraryContext _context;

        public WorkVersionsController(musiclibraryContext context)
        {
            _context = context;
        }

        // GET: WorkVersions
        public async Task<IActionResult> Index(int? id)
        {
            var musiclibraryContext = _context.WorkVersion.Include(w => w.Work).ThenInclude(a => a.Artist).ThenInclude(g => g.Genre);
            var workversions = from v in musiclibraryContext select v;
            if (id != null)
            {
                workversions = workversions.Where(w => w.WorkId == id);
            }
            return View(await workversions.ToListAsync());
        }

        // GET: WorkVersions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workVersion = await _context.WorkVersion
                .Include(w => w.Work)
                .FirstOrDefaultAsync(m => m.WorkVersionId == id);
            if (workVersion == null)
            {
                return NotFound();
            }

            return View(workVersion);
        }

        // GET: WorkVersions/Create
        public IActionResult Create()
        {
            ViewData["WorkId"] = new SelectList(_context.Work, "WorkId", "WorkName");
            return View();
        }

        // POST: WorkVersions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkVersionId,WorkId,WorkVersionName,Lossless")] WorkVersion workVersion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workVersion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["WorkId"] = new SelectList(_context.Work, "WorkId", "WorkName", workVersion.WorkId);
            return View(workVersion);
        }

        // GET: WorkVersions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workVersion = await _context.WorkVersion.FindAsync(id);
            if (workVersion == null)
            {
                return NotFound();
            }
            ViewData["WorkId"] = new SelectList(_context.Work, "WorkId", "WorkName", workVersion.WorkId);
            return View(workVersion);
        }

        // POST: WorkVersions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkVersionId,WorkId,WorkVersionName,Lossless")] WorkVersion workVersion)
        {
            if (id != workVersion.WorkVersionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workVersion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkVersionExists(workVersion.WorkVersionId))
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
            ViewData["WorkId"] = new SelectList(_context.Work, "WorkId", "WorkName", workVersion.WorkId);
            return View(workVersion);
        }

        // GET: WorkVersions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workVersion = await _context.WorkVersion
                .Include(w => w.Work)
                .FirstOrDefaultAsync(m => m.WorkVersionId == id);
            if (workVersion == null)
            {
                return NotFound();
            }

            return View(workVersion);
        }

        // POST: WorkVersions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workVersion = await _context.WorkVersion.FindAsync(id);
            _context.WorkVersion.Remove(workVersion);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkVersionExists(int id)
        {
            return _context.WorkVersion.Any(e => e.WorkVersionId == id);
        }
    }
}

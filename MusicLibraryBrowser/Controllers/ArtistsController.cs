using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MusicLibraryBrowser;
using MusicLibraryBrowser.ViewModels;

namespace MusicLibraryBrowser.Controllers
{
    public class ArtistsController : Controller
    {
        private readonly musiclibraryContext _context;

        public ArtistsController(musiclibraryContext context)
        {
            _context = context;
        }

        // GET: Artists
        public async Task<IActionResult> Index(int? genreid)
        {
            var musiclibraryContext = _context.Artist.Include(a => a.Genre);

 //         var artists = from a in musiclibraryContext select a;
            var artists = (from a in _context.Artist
                          join i in _context.Image
                          on a.ImageId equals i.ImageId
                          into a1
                          from b1 in a1.DefaultIfEmpty(new Image())
                          select new ArtistViewModel
                          {
                              ArtistId = a.ArtistId,
                              GenreId = a.GenreId,
                              GenreName = a.Genre.GenreName,
                              ArtistName = a.ArtistName,
                              ImageId = a.ImageId,
                              ImageData = b1.Data
                          });

            if (genreid != null)
            {
                artists = artists.Where(a => a.GenreId == genreid);
                var genrename = from g in _context.Genre where g.GenreId == genreid select g.GenreName;
                ViewData["GenreName"] = genrename.FirstOrDefault().ToString();
            }
            return View(await artists.OrderBy(a => a.ArtistName).ToListAsync());
        }

        public async Task<ActionResult> RenderImage(int id)
        {
            var image = await _context.Image.FindAsync(id);

            byte[] photoBack = image.Data;

            return File(photoBack, "image/png");
        }

        // GET: Artists/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // GET: Artists/Create
        public IActionResult Create()
        {
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreName");
            return View();
        }

        // POST: Artists/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArtistId,GenreId,ArtistName")] Artist artist)
        {
            if (ModelState.IsValid)
            {
                _context.Add(artist);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreName", artist.GenreId);
            return View(artist);
        }

        // GET: Artists/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist.FindAsync(id);
            if (artist == null)
            {
                return NotFound();
            }
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreName", artist.GenreId);
            return View(artist);
        }

        // POST: Artists/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArtistId,GenreId,ArtistName")] Artist artist)
        {
            if (id != artist.ArtistId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(artist);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArtistExists(artist.ArtistId))
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
            ViewData["GenreId"] = new SelectList(_context.Genre, "GenreId", "GenreName", artist.GenreId);
            return View(artist);
        }

        // GET: Artists/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var artist = await _context.Artist
                .Include(a => a.Genre)
                .FirstOrDefaultAsync(m => m.ArtistId == id);
            if (artist == null)
            {
                return NotFound();
            }

            return View(artist);
        }

        // POST: Artists/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var artist = await _context.Artist.FindAsync(id);
            _context.Artist.Remove(artist);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArtistExists(int id)
        {
            return _context.Artist.Any(e => e.ArtistId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using employeeManagement.Config;
using employeeManagement.Models;

namespace employeeManagement.Controllers
{
    public class UserProjectsController : Controller
    {
        private readonly AppDbContext _context;

        public UserProjectsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: UserProjects
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.UserProjects.Include(u => u.Project).Include(u => u.User);
            return View(await appDbContext.ToListAsync());
        }

        // GET: UserProjects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProject = await _context.UserProjects
                .Include(u => u.Project)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProject == null)
            {
                return NotFound();
            }

            return View(userProject);
        }

        // GET: UserProjects/Create
        public IActionResult Create()
        {
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "ProjectDesc");
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email");
            return View();
        }

        // POST: UserProjects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,UserId,ProjectId")] UserProject userProject)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userProject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "ProjectDesc", userProject.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", userProject.UserId);
            return View(userProject);
        }

        // GET: UserProjects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProject = await _context.UserProjects.FindAsync(id);
            if (userProject == null)
            {
                return NotFound();
            }
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "ProjectDesc", userProject.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", userProject.UserId);
            return View(userProject);
        }

        // POST: UserProjects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,ProjectId")] UserProject userProject)
        {
            if (id != userProject.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(userProject);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UserProjectExists(userProject.Id))
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
            ViewData["ProjectId"] = new SelectList(_context.Projects, "Id", "ProjectDesc", userProject.ProjectId);
            ViewData["UserId"] = new SelectList(_context.Users, "Id", "Email", userProject.UserId);
            return View(userProject);
        }

        // GET: UserProjects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var userProject = await _context.UserProjects
                .Include(u => u.Project)
                .Include(u => u.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (userProject == null)
            {
                return NotFound();
            }

            return View(userProject);
        }

        // POST: UserProjects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var userProject = await _context.UserProjects.FindAsync(id);
            if (userProject != null)
            {
                _context.UserProjects.Remove(userProject);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UserProjectExists(int id)
        {
            return _context.UserProjects.Any(e => e.Id == id);
        }
    }
}

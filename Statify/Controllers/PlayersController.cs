﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StatTracker.Models;
using Statify.Data;
using Microsoft.AspNetCore.Authorization;

namespace Statify.Controllers
{
    public class PlayersController : Controller
    {
        private readonly ApplicationDbContext _context;

        public PlayersController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Players
        public async Task<IActionResult> Index()
        {
            return View("Index", await _context.Players.OrderBy(player => player.PlayerName).ToListAsync());
        }

        [AllowAnonymous]
        // GET: Players/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return View("Error");
            }

            return View("Details", player);
        }

        [Authorize]
        // GET: Players/Create
        public IActionResult Create()
        {
            return View("Create");
        }

        // POST: Players/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("PlayerId,Position,PlayerName,PlayerHeightCm")] Player player)
        {
            if (ModelState.IsValid)
            {
                _context.Add(player);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View("Create", player);
        }

        [Authorize]
        // GET: Players/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var player = await _context.Players.FindAsync(id);
            if (player == null)
            {
                return View("Error");
            }
            return View("Edit", player);
        }

        // POST: Players/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("PlayerId,Position,PlayerName,PlayerHeightCm")] Player player)
        {
            if (id != player.PlayerId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(player);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PlayerExists(player.PlayerId))
                    {
                        return View("Error");
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View("Edit", player);
        }

        // GET: Players/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return View("Error");
            }

            var player = await _context.Players
                .FirstOrDefaultAsync(m => m.PlayerId == id);
            if (player == null)
            {
                return View("Error");
            }

            return View("Delete", player);
        }

        // POST: Players/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var player = await _context.Players.FindAsync(id);
            _context.Players.Remove(player);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PlayerExists(int id)
        {
            return _context.Players.Any(e => e.PlayerId == id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Atividade_SAEP_3.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Atividade_SAEP_3.Controllers
{
    [Authorize]
    public class ConsultasController : Controller
    {
        private readonly AppDbContext _context;

        public ConsultasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: Consultas
        public async Task<IActionResult> Index()
        {
            var pacienteId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));

            var consultas = _context.Consulta
                .Include(c => c.medico)
                .Include(c => c.paciente)
                .Where(c => c.pacienteId == pacienteId);

            return View(await consultas.ToListAsync());
        }

        // GET: Consultas/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consulta
                .Include(c => c.medico)
                .Include(c => c.paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // GET: Consultas/Create
        public IActionResult Create()
        {
            ViewData["medicoId"] = new SelectList(_context.Medico, "Id", "Id");
            ViewData["pacienteId"] = new SelectList(_context.Paciente, "Id", "Id");
            return View();
        }

        // POST: Consultas/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,pacienteId,medicoId,DataHora,statusAtendimento")] Consulta consulta)
        {
            consulta.pacienteId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            consulta.statusAtendimento = "Agendado";
            ModelState.Remove("pacienteId");
            ModelState.Remove("pacienteId");
            ModelState.Remove("statusAtendimento");
            ModelState.Remove("paciente");
            ModelState.Remove("medico");
            var jaTemConsultaHoje = await _context.Consulta.AnyAsync(c =>
            c.pacienteId == consulta.pacienteId &&
            c.DataHora.Date == consulta.DataHora.Date);

            if (jaTemConsultaHoje)
            {
                ModelState.AddModelError("", "Você já possui uma consulta agendada para este dia.");
                ViewData["medicoId"] = new SelectList(_context.Medico, "Id", "Id", consulta.medicoId);
            }
            return View(consulta);
        }

        // GET: Consultas/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }
            ViewData["medicoId"] = new SelectList(_context.Medico, "Id", "Id", consulta.medicoId);
            ViewData["pacienteId"] = new SelectList(_context.Paciente, "Id", "Id", consulta.pacienteId);
            return View(consulta);
        }

        // POST: Consultas/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,pacienteId,medicoId,DataHora,statusAtendimento")] Consulta consultaForm)
        {
            if (id != consultaForm.Id)
            {
                return NotFound();
            }

            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta == null)
            {
                return NotFound();
            }

            // segurança extra: só o dono da consulta pode editar
            var pacienteId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
            if (consulta.pacienteId != pacienteId)
            {
                return Forbid();
            }

            consulta.DataHora = consultaForm.DataHora;
            consulta.statusAtendimento = consultaForm.statusAtendimento;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ConsultaExists(consulta.Id))
                {
                    return NotFound();
                }
                throw;
            }
            return RedirectToAction(nameof(Index));
        }

        // GET: Consultas/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var consulta = await _context.Consulta
                .Include(c => c.medico)
                .Include(c => c.paciente)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (consulta == null)
            {
                return NotFound();
            }

            return View(consulta);
        }

        // POST: Consultas/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var consulta = await _context.Consulta.FindAsync(id);
            if (consulta != null)
            {
                _context.Consulta.Remove(consulta);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ConsultaExists(int id)
        {
            return _context.Consulta.Any(e => e.Id == id);
        }
    }
}

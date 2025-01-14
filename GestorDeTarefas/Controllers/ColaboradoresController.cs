﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GestorDeTarefas.Data;
using GestorDeTarefas.Models;
using GestorDeTarefas.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace GestorDeTarefas.Controllers
{
    public class ColaboradoresController : Controller
    {
        private readonly GestorDeTarefasContext _context;
        private readonly UserManager<IdentityUser> _userManager;

        public ColaboradoresController(GestorDeTarefasContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Colaboradors
        [Authorize(Roles = "admin, gestor")]
        public async Task<IActionResult> Index(string nome, string cargo, int page = 1)
        {
            var colaboradorSearch = _context.Colaborador
                .Where(b=>(nome == null || b.Name.Contains(nome))
                &( cargo == null || b.Cargo.Nome_Cargo.Contains(cargo)));


            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = colaboradorSearch.Count()

            };

            if (pagingInfo.CurrentPage > pagingInfo.TotalPages)
            {
                pagingInfo.CurrentPage = pagingInfo.TotalPages;
            }

            if (pagingInfo.CurrentPage < 1)
            {
                pagingInfo.CurrentPage = 1;
            }

            var colaboradors = await colaboradorSearch
                             .Include(b => b.Cargo)
                            .OrderBy(b => b.Name)
                            .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                            .Take(pagingInfo.PageSize)
                            .ToListAsync();


            return View(
                new ColaboradorListViewModel
                {
                    Colaboradors = colaboradors,
                    PagingInfo = pagingInfo,
                    NomeSearched = nome,
                    CargoSearched = cargo
                }
            );
        }

        public async Task<IActionResult> AdicionarIdiomaColaborador(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Colaborador colaborador = _context.Colaborador.Find(id);

            if (colaborador == null)
            {
                return NotFound();
            }

            var Results = from b in _context.Idioma
                          select new
                          {
                              b.IdiomaId,
                              b.NomeIdioma,
                              Checked = ((from ab in _context.ColaboradorIdioma
                                          where (ab.ColaboradorId == id) & (ab.IdiomaId == b.IdiomaId)
                                          select ab).Count() > 0)
                          };

            var MyViewModel = new ColaboradorListViewModel();
            MyViewModel.ColaboradorId = id.Value;
            MyViewModel.NomeColaborador = colaborador.Name;

            var MyCheckBoxList = new List<CheckBoxViewModelColaboradorIdioma>();

            foreach (var item in Results)
            {
               MyCheckBoxList.Add(new CheckBoxViewModelColaboradorIdioma { Id = item.IdiomaId, Name = item.NomeIdioma, Checked = item.Checked });

                MyViewModel.Idiomas = MyCheckBoxList;
            }
            return View(MyViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> AdicionarIdiomaColaborador(ColaboradorListViewModel colaborador)
        {
            var MyColaborador = _context.Colaborador.Find(colaborador.ColaboradorId);
            MyColaborador.Name = colaborador.NomeColaborador;

            foreach (var item in _context.ColaboradorIdioma)
            {
                if (item.ColaboradorId == colaborador.ColaboradorId)
                {
                    _context.Entry(item).State = EntityState.Deleted;
                    ViewBag.Title = "Alteração do idioma do colaborador";/////////////
                    ViewBag.Message = "Idioma alterado no colaborador com sucesso!!!";/////////////
                }
            }

            foreach (var item in colaborador.Idiomas)
            {
                if (item.Checked)
                {
                    _context.ColaboradorIdioma.Add(new
                        ColaboradorIdioma()
                    {
                        ColaboradorId = colaborador.ColaboradorId,
                        IdiomaId = item.Id,
                    });


                    ViewBag.Title = "Idioma adicionado ao Colaborador";
                    ViewBag.Message = "Idioma adicionado ao Colaborador com sucesso!!!";
                }
            }

            _context.SaveChanges();


            return View("Success");
        }


        // GET: Colaboradors/Details/5
        [Authorize(Roles = "admin, gestor")]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colaborador = await _context.Colaborador
                .Include(b => b.Cargo)
                .FirstOrDefaultAsync(m => m.ColaboradorId == id);


            if (colaborador == null)
            {
                return NotFound();
            }

            return View(colaborador);
        }

        public async Task<IActionResult> VerIdiomas(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var Colaborador = await _context.Colaborador
                .FirstOrDefaultAsync(m => m.ColaboradorId == id);
            if (Colaborador == null)
            {
                return NotFound();
            }



            var ResultadoSearch = from b in _context.ColaboradorIdioma
                                  select new
                                  {
                                      b.ColaboradorId,
                                      b.Colaborador.Name,
                                      b.Colaborador.Email,
                                      b.Colaborador.Contacto,
                                      b.Colaborador.Cargo,
                                      Checked = ((from ab in _context.ColaboradorIdioma

                                                  where (ab.IdiomaId == id) & (ab.ColaboradorId == b.ColaboradorId)
                                                  select ab)
                                                  .Count() > 0)
                                  };


            var ResultadoSearch2 = from b in _context.ColaboradorIdioma
                                   select new
                                   {
                                       b.ColaboradorId,
                                       b.Colaborador.Name,
                                       b.Colaborador.Email,
                                       b.Colaborador.Contacto,
                                       b.Colaborador.Cargo,
                                       Checked = ((from ab in _context.ColaboradorIdioma

                                                   where (ab.IdiomaId == id) & (ab.ColaboradorId == b.ColaboradorId)
                                                   select ab)
                                                    .Count() > 0)
                                   };

            var Resultado = await ResultadoSearch2
                            .OrderBy(b => b.Name)
                            .ToListAsync();

            var MyViewModel = new ColaboradorListViewModel();
            var MyCheckBoxList = new List<CheckBoxViewModelColaboradorIdioma>();

            foreach (var item in Resultado)
            {
                MyCheckBoxList.Add(new CheckBoxViewModelColaboradorIdioma
                {
                    Id = item.ColaboradorId,
                    Name = item.Name,
                    Email = item.Email,
                    Contacto = item.Contacto,
                    Cargo = item.Cargo.Nome_Cargo,
                    Checked = item.Checked,
                });

                MyViewModel.Idiomas = MyCheckBoxList;
            }

            return View(new ColaboradorListViewModel
            {
                ColaboradorId = id.Value,
                NomeColaborador = Colaborador.Name,
                Idiomas = MyCheckBoxList,
                Contacto = Colaborador.Contacto,
                Cargo = Colaborador.Cargo.Nome_Cargo,
                Email = Colaborador.Email
            });
        }

        // GET: Colaboradors/Create
        public IActionResult Register()
        {
            Colaborador colaborador = new Colaborador();
            RegistarColaboradorViewModel r = new RegistarColaboradorViewModel();


            return View(new RegistarColaboradorViewModel
            {
                Nome = colaborador.Name,
                Email = colaborador.Email,
                Cargo = new SelectList(_context.Cargo, "CargoId", "Nome_Cargo"),
                Contacto = colaborador.Contacto
            });
        }

        //public IActionResult Create()
        //{
        //    ViewData["CargoId"] = new SelectList(_context.Cargo, "CargoId", "Nome_Cargo");
        //    return View();
        //}

        // POST: Colaboradors/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegistarColaboradorViewModel colaboradorInfo)
        {
            if (!ModelState.IsValid)
            {
                return View(colaboradorInfo);
            }

            string username = colaboradorInfo.Email;

            IdentityUser user = await _userManager.FindByNameAsync(username);

            if (user != null)
            {
                ModelState.AddModelError("Email", "There is allready a customer with that email.");
                return View(colaboradorInfo);
            }

            Colaborador colaborador = new Colaborador
            {
                Name = colaboradorInfo.Nome,
                Email = colaboradorInfo.Email,
                Contacto = colaboradorInfo.Contacto,
                CargoId = colaboradorInfo.CargoId
            };

            if (ModelState.IsValid)
            {
                _context.Add(colaborador);
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                ViewBag.Title = "Colaborador adicionado";
                ViewBag.Message = "Colaborador adicionado com sucesso.";
                //return View("Success");
            }
            return View("Success");
            //return View(colaborador);
        }

        // GET: Colaboradors/Edit/5
        [Authorize(Roles = "admin, gestor")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var colaborador = await _context.Colaborador.FindAsync(id);
            if (colaborador == null)
            {
                return NotFound();
            }
            return View(colaborador);
        }

        // POST: Colaboradors/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, gestor")]
        public async Task<IActionResult> Edit(int id, [Bind("ColaboradorId,Name,Email,Contacto,Cargo")] Colaborador colaborador)
        {
            if (id != colaborador.ColaboradorId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(colaborador);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ColaboradorExists(colaborador.ColaboradorId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                ViewBag.Title = "Colaborador editado";
                ViewBag.Message = "Colaborador alterado com sucesso.";
                return View("Success");
            }
            return View(colaborador);
        }

        // GET: Colaboradors/Delete/5
        [Authorize(Roles = "admin, gestor")]
        public async Task<IActionResult> Delete(int? id)
        {
            try
            {
                if (id == null)
                {
                    return NotFound();
                }

                var colaborador = await _context.Colaborador
                    .Include(b => b.Cargo)
                    .SingleOrDefaultAsync(m => m.ColaboradorId == id);
                if (colaborador == null)
                {
                    return NotFound();
                }

                return View(colaborador);
                }
            catch (DbUpdateException /* ex */)
            {

                return View("MensagemErro");
            }
        }

        // POST: Colaboradors/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin, gestor")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var colaborador = await _context.Colaborador.FindAsync(id);
                _context.Colaborador.Remove(colaborador);
                await _context.SaveChangesAsync();
                ViewBag.Title = "Colaboradores apagado";
                ViewBag.Message = "Colaborador apagado com sucesso.";
                return View("Success");
            }
            catch (DbUpdateException /* ex */)
            {
                ViewBag.Title = "Ups! Este Colaborador não pode ser apagado.";
                ViewBag.Message = "Verifique as ligações entre as tabelas!!!";
                return View("MensagemErro");
            }

        }

        private bool ColaboradorExists(int id)
        {
            return _context.Colaborador.Any(e => e.ColaboradorId == id);
        }
    }
}


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
using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace GestorDeTarefas.Controllers
{
    public class ContactoesController : Controller
    {
        private readonly GestorDeTarefasContext _context;

        public ContactoesController(GestorDeTarefasContext context)
        {
            _context = context;
        }

        // GET: Contactoes
        public async Task<IActionResult> Index(string nome, int page = 1)
        {
            var contactoSearch = _context.Contacto
                .Where(b => nome == null || b.Nome.Contains(nome) || b.Sobrenome.Contains(nome)
                || b.Email.Contains(nome));
            var pagingInfo = new PagingInfo
            {
                CurrentPage = page,
                TotalItems = contactoSearch.Count()
            };

            if (pagingInfo.CurrentPage > pagingInfo.TotalPages)
            {
                pagingInfo.CurrentPage = pagingInfo.TotalPages;
            }

            if (pagingInfo.CurrentPage < 1)
            {
                pagingInfo.CurrentPage = 1;
            }

            var contacto = await contactoSearch                      
                            .OrderBy(b => b.Nome)
                            .Skip((pagingInfo.CurrentPage - 1) * pagingInfo.PageSize)
                            .Take(pagingInfo.PageSize)
                            .ToListAsync();

            return View(
                new ContactoListViewModel
                {
                    Contacto = contacto,
                    PagingInfo = pagingInfo,
                    NomeSearched = nome
                }
            );
        }

        // GET: Contactoes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _context.Contacto
                .FirstOrDefaultAsync(m => m.ContactoId == id);
            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);
        }

        // GET: Contactoes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Contactoes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ContactoId,Nome,Sobrenome,Email,Assunto,Mensagem")] Contacto contacto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(contacto);
                await _context.SaveChangesAsync();

                ViewBag.Title = "Mensagem enviada com sucesso!!";
                ViewBag.type = "alert-success";
                ViewBag.Message = "Em breve entraremos em Contacto!";
                ViewBag.redirect = "/Contactoes/Create";
               
                return View("Success");
            
               // return RedirectToAction(nameof(Index));
            }
            return View(contacto);
        }

        // GET: Contactoes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _context.Contacto.FindAsync(id);
            if (contacto == null)
            {
                return NotFound();
            }
            return View(contacto);
        }

        // POST: Contactoes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]      
        public async Task<IActionResult> Edit(int id, [Bind("ContactoId,Nome,Sobrenome,Email,Assunto,Mensagem,Verificado,Resposta")] Contacto contacto)
        {
            if (id != contacto.ContactoId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    Contacto ContactoVerificarDados = await _context.Contacto.FindAsync(id);
                    ContactoVerificarDados.Verificado = true;
                    ContactoVerificarDados.Resposta = contacto.Resposta;
                    contacto = ContactoVerificarDados;

                    using (MailMessage message = new MailMessage("gestortarefa@gmail.com", contacto.Email))
                    {
                        message.Subject = contacto.Assunto;
                        message.Body = contacto.Resposta;
                        message.IsBodyHtml = false;

                        using (SmtpClient smtp = new SmtpClient())
                        {
                            smtp.Host = "smtp.gmail.com";
                            smtp.EnableSsl = true;
                            //NetworkCredential credencial =
                            smtp.UseDefaultCredentials = true;
                            smtp.Credentials = new NetworkCredential("gestortarefa@gmail.com", "Guarda@@1");
                            smtp.Port = 587;
                            smtp.Send(message);
                            //Permitir aplicações menos seguras: ATIVADO
                        }
                    }
                    contacto = ContactoVerificarDados;
                    _context.Update(contacto);
                    await _context.SaveChangesAsync();
                    ViewBag.title = "Contacto Respondido Com Sucesso!";
                    ViewBag.type = "alert-success";
                    ViewBag.message = "Em breve entraremos em Contacto!";
                    ViewBag.redirect = "//Contactoes/Index"; // Request.Path
                    return View("Mensagem");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ContactoExists(contacto.ContactoId))
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
            return View(contacto);
        }

        // GET: Contactoes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var contacto = await _context.Contacto
                .FirstOrDefaultAsync(m => m.ContactoId == id);
            if (contacto == null)
            {
                return NotFound();
            }

            return View(contacto);
        }

        // POST: Contactoes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var contacto = await _context.Contacto.FindAsync(id);
            _context.Contacto.Remove(contacto);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ContactoExists(int id)
        {
            return _context.Contacto.Any(e => e.ContactoId == id);
        }




        public IActionResult Responder()
        {

            return View();
        }
        [HttpPost]
        public IActionResult Responder(Contacto model)
        {
            /*using (MailAddress message = new MailAddress("gestortarefa@gmail.com", model.Email))
            {
                message.Subject = model.Assunto;
            }*/
            using (MailMessage message = new MailMessage("gestortarefa@gmail.com", model.Email))
            {
                message.Subject = model.Assunto;
                message.Body = model.Mensagem;
                message.IsBodyHtml = false;

                using (SmtpClient smtp = new SmtpClient())
                {
                    smtp.Host = "smtp.gmail.com";
                    smtp.EnableSsl = true;
                    //NetworkCredential credencial =
                    smtp.UseDefaultCredentials = true;
                    smtp.Credentials = new NetworkCredential("gestortarefa@gmail.com", "Guarda@@1");
                    smtp.Port = 587;
                    smtp.Send(message);
                    //smtp.S(message);
                    //Permitir aplicações menos seguras: ATIVADO
                    ViewBag.title = "Contacto Respondido Com Sucesso!";
                    ViewBag.type = "alert-success";
                    ViewBag.message = "Em breve entraremos em Contacto!";
                    ViewBag.redirect = "/Contactos/Index"; // Request.Path
                    return View("Mensagem");
                }

            }

            //return View(Contacto);
        }






    }
}
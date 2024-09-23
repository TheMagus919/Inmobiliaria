using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyecto.Models;

namespace proyecto.Controllers
{
    public class InquilinosController : Controller
    {
        // GET: Inquilinos
        [Authorize]
        public ActionResult Index()
        {   
            try{
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                List<Inquilino> inquilinos = repositorioInquilino.ObtenerTodos();
                ViewBag.Id = TempData["Id"];
                if(TempData.ContainsKey("Mensaje")){
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                return View(inquilinos);

            }catch(System.Exception){
                throw;
            }
        }

        // GET: Inquilinos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            try{
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                Inquilino inquilinos = repositorioInquilino.ObtenerPorId(id);
                ViewBag.IdInqui = inquilinos.IdInquilino;
                return View(inquilinos);

            }catch(System.Exception){
                throw;
            }
        }

        // GET: Inquilinos/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino inquilino)
        {
            try
            {   
                if (ModelState.IsValid){
                    RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                    List<Inquilino> rep = repositorioInquilino.ObtenerTodos();
                    foreach(var asd in rep){
                        if(asd.Dni == inquilino.Dni){
                            ModelState.AddModelError("", "El DNI ya esta en uso");
                            return View(inquilino);
                        }else if(asd.Email == inquilino.Email){
                            ModelState.AddModelError("", "El Email ya esta en uso");
                            return View(inquilino);
                        }
                    }
                    repositorioInquilino.Alta(inquilino);
                    TempData["Id"] = inquilino.IdInquilino;
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return View(inquilino);
                }
                
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inquilinos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                Inquilino inquilino = repositorioInquilino.ObtenerPorId(id);
                ViewBag.IdInqui = inquilino.IdInquilino;
                return View(inquilino);

            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
            try
            {   
                if (ModelState.IsValid){
                    RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                    List<Inquilino> rep = repositorioInquilino.ObtenerTodosParaEditar(inquilino.IdInquilino);
                        foreach(var asd in rep){
                            if(asd.Dni == inquilino.Dni){
                                ModelState.AddModelError("", "El DNI ya esta en uso");
                                return View(inquilino);
                            }else if(asd.Email == inquilino.Email){
                                ModelState.AddModelError("", "El Email ya esta en uso");
                                return View(inquilino);
                            }
                        }
                    repositorioInquilino.Modificacion(inquilino);
                    TempData["Edit"] = inquilino.IdInquilino;
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return View(inquilino);
                }

            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inquilinos/Delete/5
        [Authorize(Policy = "administrador")]
        public ActionResult Delete(int id)
        {   
            try
            {
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                Inquilino inquilino = repositorioInquilino.ObtenerPorId(id);
                ViewBag.IdInqui = inquilino.IdInquilino;
                return View(inquilino);

            }
            catch(System.Exception)
            {
                throw;
            }
            
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [Authorize(Policy = "administrador")]
        public ActionResult Delete(int id, Inquilino inquilino)
        {
            try
            {
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                repositorioInquilino.Baja(id);
                TempData["Delete"] = true;
                return Json(new { success = true });

            }
            catch(System.Exception)
            {
                throw;
            }
        }
    }
}
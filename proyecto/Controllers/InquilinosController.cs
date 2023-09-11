using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyecto.Models;

namespace proyecto.Controllers
{
    public class InquilinosController : Controller
    {
        // GET: Inquilinos
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
        public ActionResult Details(int id)
        {
            try{
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                Inquilino inquilinos = repositorioInquilino.ObtenerPorId(id);
                return View(inquilinos);

            }catch(System.Exception){
                throw;
            }
        }

        // GET: Inquilinos/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inquilinos/Create
        [HttpPost]
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
                    return View(inquilino);
                }
                
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inquilinos/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                Inquilino inquilino = repositorioInquilino.ObtenerPorId(id);
                return View(inquilino);

            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Inquilinos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino inquilino)
        {
            try
            {
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                List<Inquilino> rep = repositorioInquilino.ObtenerTodos();
                    foreach(var asd in rep){
                        if(asd.Dni == inquilino.Dni && asd.IdInquilino != inquilino.IdInquilino){
                            ModelState.AddModelError("", "El DNI ya esta en uso");
                            return View(inquilino);
                        }else if(asd.Email == inquilino.Email && asd.IdInquilino != inquilino.IdInquilino){
                            ModelState.AddModelError("", "El Email ya esta en uso");
                            return View(inquilino);
                        }
                    }
                repositorioInquilino.Modificacion(inquilino);
                return RedirectToAction(nameof(Index));

            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inquilinos/Delete/5
        public ActionResult Delete(int id)
        {   
            try
            {
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                Inquilino inquilino = repositorioInquilino.ObtenerPorId(id);
                return View(inquilino);

            }
            catch(System.Exception)
            {
                throw;
            }
            
        }

        // POST: Inquilinos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Inquilino inquilino)
        {
            try
            {
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                repositorioInquilino.Baja(id);
                return RedirectToAction(nameof(Index));

            }
            catch(System.Exception)
            {
                throw;
            }
        }
    }
}
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
        public ActionResult Delete(Inquilino inquilino)
        {
            try
            {
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                repositorioInquilino.Baja(inquilino.IdInquilino);
                return RedirectToAction(nameof(Index));

            }
            catch(System.Exception)
            {
                throw;
            }
        }
    }
}
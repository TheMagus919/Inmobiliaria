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
    public class ContratosController : Controller
    {
        // GET: Contratos
        [Authorize]
        public ActionResult Index()
        {
            try
            {   
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                List<Contrato> contratos = repositorioContrato.ObtenerTodos();
                ViewBag.Id = TempData["Id"];
                if(TempData.ContainsKey("Mensaje")){
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                return View(contratos);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Contratos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            try
            {
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                Contrato contrato = repositorioContrato.ObtenerPorId(id);
                return View(contrato);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Contratos/Create
        [Authorize]
        public ActionResult Create()
        {
            try{
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                return View();
            }catch(Exception ex){
                throw;
            }
            
        }

        // POST: Contratos/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Contrato contrato)
        {
            try
            {   
                if(ModelState.IsValid){
                    RepositorioContrato repositorioContrato = new RepositorioContrato();
                    List<Contrato> inm = repositorioContrato.ObtenerTodos();
                    foreach (var item in inm){
                        if(item.IdInmueble == contrato.IdInmueble){
                            ModelState.AddModelError("","El Contrato ya esta en uso");
                            TempData["Error"] = "El Contrato ya esta en uso";
                            return RedirectToAction(nameof(Create));
                        }
                    }
                    repositorioContrato.Alta(contrato);
                    TempData["Id"] = contrato.IdContrato;
                    return RedirectToAction(nameof(Index));
                }else{
                    return View(contrato);
                }
                
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Contratos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                Contrato contrato = repositorioContrato.ObtenerPorId(id);
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                return View(contrato);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Contratos/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Contrato contrato)
        {
            try
            {
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                repositorioContrato.Modificacion(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Contratos/Delete/5
        [Authorize(Policy = "administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                Contrato contrato = repositorioContrato.ObtenerPorId(id);
                return View(contrato);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Contratos/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "administrador")]
        public ActionResult Delete(int id, Contrato contrato)
        {
            try
            {
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                repositorioContrato.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception)
            {
                throw;
            }
        }
    }
}
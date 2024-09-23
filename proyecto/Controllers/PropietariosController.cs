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
    public class PropietariosController : Controller
    {
        // GET: Propietarios
        [Authorize]
        public ActionResult Index()
        {   
            try
            {   
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                List<Propietario> propietarios = repositorioPropietario.ObtenerLista();
                ViewBag.Id = TempData["Id"];
                if(TempData.ContainsKey("Mensaje")){
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                return View(propietarios);
            }
            catch(System.Exception)
            {
                throw;
            }
            
        }

        // GET: Propietarios/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {   
            try
            {
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                Propietario propietario = repositorioPropietario.ObtenerPorId(id);
                ViewBag.IdPropi = propietario.IdPropietario;
                return View(propietario);
            }
            catch(System.Exception)
            {
                throw;
            }
            
        }

        // GET: Propietarios/Create
        [Authorize]
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {   
                if(ModelState.IsValid){
                    RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                    List<Propietario> rep = repositorioPropietario.ObtenerTodos();
                    foreach(var asd in rep){
                        if(asd.Dni == propietario.Dni){
                            ModelState.AddModelError("", "El DNI ya esta en uso");
                            return View(propietario);
                        }else if(asd.Email == propietario.Email){
                            ModelState.AddModelError("", "El Email ya esta en uso");
                            return View(propietario);
                        }
                    }
                    repositorioPropietario.Alta(propietario);
                    TempData["Id"] = propietario.IdPropietario;
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    ViewBag.Propietarios = new RepositorioPropietario().ObtenerTodos();
                    return View(propietario);
                }
            }
            catch(System.Exception)
            {
                throw;
            }
        }
        [Authorize]
        // GET: Propietarios/Edit/5
        public ActionResult Edit(int id)
        {   
            try
            {
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                Propietario propietario = repositorioPropietario.ObtenerPorId(id);
                ViewBag.IdPropi = propietario.IdPropietario;
                return View(propietario);
            }
            catch(System.Exception)
            {
                throw;
            }
            
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {   
                if(ModelState.IsValid){
                    RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                    List<Propietario> rep = repositorioPropietario.ObtenerTodosParaEditar(propietario.IdPropietario);
                        foreach(var asd in rep){
                            if(asd.Dni == propietario.Dni){
                                ModelState.AddModelError("", "El DNI ya esta en uso");
                                return View(propietario);
                            }else if(asd.Email == propietario.Email){
                                ModelState.AddModelError("", "El Email ya esta en uso");
                                return View(propietario);
                            }
                        }
                    repositorioPropietario.Modificacion(propietario);
                    TempData["Edit"] = propietario.IdPropietario;
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return View(propietario);
                }
                
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Propietarios/Delete/5
        [Authorize(Policy = "administrador")]
        public ActionResult Delete(int id)
        {   
            try
            {
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                Propietario propietario = repositorioPropietario.ObtenerPorId(id);
                ViewBag.IdPropi = propietario.IdPropietario;
                return View(propietario);
            }
            catch(System.Exception)
            {
                throw;
            }
            
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [Authorize(Policy = "administrador")]
        public ActionResult Delete(int id, Propietario propietario)
        {
            try
            {
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                repositorioPropietario.Baja(id);
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
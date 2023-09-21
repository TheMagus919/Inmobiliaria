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
    public class InmueblesController : Controller
    {
        // GET: Inmuebles
        [Authorize]
        public ActionResult Index()
        {
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                List<Inmueble> inmuebles = repositorioInmueble.ObtenerTodos();
                ViewBag.Id = TempData["Id"];
                if(TempData.ContainsKey("Mensaje")){
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                return View(inmuebles);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inmuebles/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {   
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                Inmueble inmuebles = repositorioInmueble.ObtenerPorId(id);
                return View(inmuebles);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inmuebles/Create
        [Authorize]
        public ActionResult Create()
        {   try{
            RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
            ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
            return View();
        }catch(Exception ex){
            throw;
        }           
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {   
                if (ModelState.IsValid){
                    RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                    List<Inmueble> inm = new List<Inmueble>();
                    inm = repositorioInmueble.ObtenerTodos();
                    foreach(var item in inm){
                        if(item.Direccion == inmueble.Direccion){
                            ModelState.AddModelError("Direccion", "La dirección ya existe");
                            TempData["Error"] = "La dirección ya existe";
                            return RedirectToAction(nameof(Create));
                        }else{
                            repositorioInmueble.Alta(inmueble);
                        }
                    }
                    TempData["Id"] = inmueble.IdInmueble;
                    return RedirectToAction(nameof(Index));
                }else{
                    return View(inmueble);
                }
                
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Inmuebles/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                Inmueble inmuebles = repositorioInmueble.ObtenerPorId(id);
                ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                return View(inmuebles);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                repositorioInmueble.Modificacion(inmueble);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inmuebles/Delete/5
        [Authorize(Policy = "administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                Inmueble inmuebles = repositorioInmueble.ObtenerPorId(id);
                return View(inmuebles);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "administrador")]
        public ActionResult Delete(int id, Inmueble inmueble)
        {
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                repositorioInmueble.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception)
            {
                throw;
            }
        }
    }
}
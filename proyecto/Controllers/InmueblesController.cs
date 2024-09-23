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
        public ActionResult Details(int id, string returnUrl = null, string idPropietario = null)
        {   
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                Inmueble inmuebles = repositorioInmueble.ObtenerPorId(id);
                ViewBag.IdInmu = inmuebles.IdInmueble;
                ViewBag.IdPropi = idPropietario;
                ViewBag.ReturnUrl = returnUrl ?? "Index";
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
        [Authorize]
        [ValidateAntiForgeryToken]
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
                            return RedirectToAction(nameof(Index));
                        }
                    }
                    repositorioInmueble.Alta(inmueble);
                    TempData["Id"] = inmueble.IdInmueble;
                    return RedirectToAction(nameof(Index));
                }else{
                    RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                    ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
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
        public ActionResult Edit(int id, string returnUrl = null, string idPropietario = null)
        {
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                Inmueble inmuebles = repositorioInmueble.ObtenerPorId(id);
                ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                ViewBag.ReturnUrl = returnUrl ?? "Index";
                ViewBag.IdInmu = inmuebles.IdInmueble;
                ViewBag.IdPropi = idPropietario;
                return View(inmuebles);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble inmueble)
        {
            try
            {   if (ModelState.IsValid){
                    RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                    repositorioInmueble.Modificacion(inmueble);
                    TempData["Edit"] = inmueble.IdInmueble;
                    return RedirectToAction(nameof(Index));
                }else{
                    RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                    ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return View(inmueble);
                }
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inmuebles/Delete/5
        [Authorize(Policy = "administrador")]
        public ActionResult Delete(int id, string returnUrl = null, string idPropietario = null)
        {
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                Inmueble inmuebles = repositorioInmueble.ObtenerPorId(id);
                ViewBag.ReturnUrl = returnUrl ?? "Index";
                ViewBag.IdInmu = inmuebles.IdInmueble;
                ViewBag.IdPropi = idPropietario;
                return View(inmuebles);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Inmuebles/Delete/5
        [HttpPost]
        [Authorize(Policy = "administrador")]
        public ActionResult Delete(int id, Inmueble inmueble)
        {
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                repositorioInmueble.Baja(id);
                TempData["Delete"] = true;
                return Json(new { success = true });
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inmuebles/ListaDisponibles
        [Authorize]
        public ActionResult ListaDisponibles()
        {   
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                List<Inmueble> inmuebles = repositorioInmueble.ObtenerListaDisponibles();
                return View(inmuebles);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inmuebles/SeleccionarPropietario
        [Authorize]
        public ActionResult SeleccionarPropietario()
        {   
            try
            {
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                ViewBag.Propietarios = repositorioPropietario.ObtenerTodos();
                return View();
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Inmuebles/InmueblexPropietario/5
        [Authorize]
        [HttpPost]
        public ActionResult InmueblexPropietario(string IdPropietario)
        {   
            try
            {
                if (ModelState.IsValid){
                    RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                    RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                    int id = int.Parse(IdPropietario);
                    ViewBag.Propietario  = repositorioPropietario.ObtenerPorId(id);
                    ViewBag.IdPropi = id;
                    List<Inmueble> inmuebles = repositorioInmueble.ObtenerListaInmueblesXPropietario(IdPropietario);
                    return View(inmuebles);
                    
                }else{
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return RedirectToAction(nameof(SeleccionarPropietario));
                }
            }
            catch(System.Exception)
            {
                throw;
            }
        }
    }
}
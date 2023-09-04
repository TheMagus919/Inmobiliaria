using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyecto.Models;

namespace proyecto.Controllers
{
    public class InmueblesController : Controller
    {
        // GET: Inmuebles
        public ActionResult Index()
        {
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                List<Inmueble> inmuebles = repositorioInmueble.ObtenerTodos();
                var nombress = new List<string>();
                int contador=0;
                foreach(Inmueble nombre in inmuebles){
                    int id = nombre.IdPropietario;
                    Propietario propietarios=repositorioPropietario.ObtenerPorId(id);
                    nombress.Add(propietarios.Nombre.ToString());
                    contador++;
                }
                ViewBag.Id = TempData["Id"];
                if(TempData.ContainsKey("Mensaje")){
                    ViewBag.Mensaje = TempData["Mensaje"];
                }
                ViewBag.Contador = contador;
                ViewBag.Nombree = nombress;
                return View(inmuebles);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inmuebles/Details/5
        public ActionResult Details(int id)
        {   
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                Inmueble inmuebles = repositorioInmueble.ObtenerPorId(id);
                Propietario propietario=repositorioPropietario.ObtenerPorId(inmuebles.IdPropietario);
                var nombre = propietario.Nombre;
                ViewBag.Nombre = nombre;
                return View(inmuebles);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inmuebles/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Inmuebles/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble inmueble)
        {
            try
            {   
                if (ModelState.IsValid){
                    RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                    repositorioInmueble.Alta(inmueble);
                    TempData["Id"] = inmueble.IdInmueble;
                    return RedirectToAction(nameof(Index));
                }else{
                    return View(inmueble);
                }
                
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Inmuebles/Edit/5
        public ActionResult Edit(int id)
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

        // POST: Inmuebles/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
        public ActionResult Delete(int id, Inmueble inmueble)
        {
            try
            {
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                repositorioInmueble.Baja(inmueble.IdInmueble);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception)
            {
                throw;
            }
        }
    }
}
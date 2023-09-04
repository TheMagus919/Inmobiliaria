using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyecto.Models;

namespace proyecto.Controllers
{
    public class PropietariosController : Controller
    {
        // GET: Propietarios
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
        public ActionResult Details(int id)
        {   
            try
            {
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                Propietario propietario = repositorioPropietario.ObtenerPorId(id);
                return View(propietario);
            }
            catch(System.Exception)
            {
                throw;
            }
            
        }

        // GET: Propietarios/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Propietarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario propietario)
        {
            try
            {   
                if(ModelState.IsValid){
                    RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                repositorioPropietario.Alta(propietario);
                TempData["Id"] = propietario.IdPropietario;
                return RedirectToAction(nameof(Index));
                }else{
                    return View(propietario);
                }
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Propietarios/Edit/5
        public ActionResult Edit(int id)
        {   
            try
            {
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                Propietario propietario = repositorioPropietario.ObtenerPorId(id);
                return View(propietario);
            }
            catch(System.Exception)
            {
                throw;
            }
            
        }

        // POST: Propietarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Propietario propietario)
        {
            try
            {
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                repositorioPropietario.Modificacion(propietario);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Propietarios/Delete/5
        public ActionResult Delete(int id)
        {   
            try
            {
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                Propietario propietario = repositorioPropietario.ObtenerPorId(id);
                return View(propietario);
            }
            catch(System.Exception)
            {
                throw;
            }
            
        }

        // POST: Propietarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Propietario propietario)
        {
            try
            {
                RepositorioPropietario repositorioPropietario = new RepositorioPropietario();
                repositorioPropietario.Baja(propietario.IdPropietario);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception)
            {
                throw;
            }
        }
    }
}
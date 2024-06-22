using System;
using System.Collections.Generic;
using System.Formats.Asn1;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyecto.Models;

namespace proyecto.Controllers
{
    public class PagosController : Controller
    {
        // GET: Pagos
        [Authorize]
        public ActionResult Index()
        {   
            RepositorioPago repositorioPago = new RepositorioPago();
            List<Pago> pagos= repositorioPago.ObtenerTodos();
            return View(pagos);
        }

        // GET: Pagos/Details/5
        [Authorize]
        public ActionResult Details(int id)
        {
            try
            {
                RepositorioPago repositorioPago = new RepositorioPago();
                Pago pago = repositorioPago.ObtenerPorId(id);
                return View(pago);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Pagos/Create
        [Authorize]
        public ActionResult Create()
        {
            try{
            RepositorioContrato repositorioContrato = new RepositorioContrato();
            ViewBag.Contratos = repositorioContrato.ObtenerTodos();
            return View();
        }catch(Exception ex){
            throw;
        }     
        }

        // POST: Pagos/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Pago pago)
        {
            try
            {   
                if (ModelState.IsValid){
                    RepositorioPago repositorioPago = new RepositorioPago();
                    repositorioPago.Alta(pago);
                    TempData["Id"] = pago.IdPago;
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return View(pago);
                }
                
            }
            catch(Exception ex)
            {
                throw;
            }
        }

        // GET: Pagos/Edit/5
        [Authorize]
        public ActionResult Edit(int id)
        {
            try
            {
                RepositorioPago repositorioPago = new RepositorioPago();
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                Pago pago = repositorioPago.ObtenerPorId(id);
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View(pago);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Pagos/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago pago)
        {
            try
            {   
                if (ModelState.IsValid){
                    RepositorioPago repositorioPago = new RepositorioPago();
                    repositorioPago.Modificacion(pago);
                    TempData["Edit"] = pago.IdPago;
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return View(pago);
                }
                
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Pagos/Delete/5
        [Authorize(Policy = "administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                RepositorioPago repositorioPago = new RepositorioPago();
                Pago pago = repositorioPago.ObtenerPorId(id);
                return View(pago);
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Pagos/Delete/5
        [HttpPost]
        [Authorize(Policy = "administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Pago pago)
        {
            try
            {
                RepositorioPago repositorioPago = new RepositorioPago();
                TempData["Delete"] = "Eliminada";
                repositorioPago.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Pagos/FormContrato
        [Authorize]
        public ActionResult FormContrato()
        {
            try
            {
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                ViewBag.Contratos = repositorioContrato.ObtenerTodos();
                return View();
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Pagos/ListarPagos
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ListarPagos(int IdContrato)
        {
            try
            {   
                if (ModelState.IsValid){
                    RepositorioPago repositorioPago = new RepositorioPago();
                    List<Pago> listaPagos = repositorioPago.ListarPagos(IdContrato);
                    ViewBag.IdContrato = IdContrato;
                    return View(listaPagos);
                }else{
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return RedirectToAction(nameof(FormContrato));
                }
                
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Pagos/NuevoPago/5
        [Authorize]
        public ActionResult NuevoPago(int id)
        {
            try{
            RepositorioContrato repositorioContrato = new RepositorioContrato();
            ViewBag.Contrato = repositorioContrato.ObtenerPorId(id);
            return View();
        }
        catch(System.Exception)
        {
            throw;
        }     
        }

        // POST: Pagos/NuevoPago/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult NuevoPago(Pago pago)
        {
            try{
                if (ModelState.IsValid){
                    RepositorioPago repositorioPago = new RepositorioPago();
                    repositorioPago.Alta(pago);
                    TempData["Id"] = pago.IdPago;
                    return RedirectToAction(nameof(Index));
                }else{
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return View(pago);
                }
        }catch(System.Exception){
            throw;
        }     
        }

    }
}
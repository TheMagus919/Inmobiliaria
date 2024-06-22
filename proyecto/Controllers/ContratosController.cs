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
                DateTime fechaHoraActual = DateTime.Now;
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                Contrato contrato = repositorioContrato.ObtenerPorId(id);
                if(contrato.FechaHasta >= fechaHoraActual.Date && contrato.FechaDesde <= fechaHoraActual.Date){
                    if(!contrato.Cancelado){
                        ViewBag.Opcion = "Aprobado";
                    }
                }
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato contrato)
        {
            try
            {   
                if(ModelState.IsValid && contrato.FechaDesde.HasValue && contrato.FechaHasta.HasValue){
                    if(contrato.FechaDesde.Value.Date < contrato.FechaHasta.Value.Date){
                        RepositorioContrato repositorioContrato = new RepositorioContrato();
                        List<Contrato> inm = repositorioContrato.ObtenerTodos();
                        foreach (var item in inm){
                            if(item.IdInmueble == contrato.IdInmueble){
                                if(item.FechaDesde.Value.Date > contrato.FechaHasta.Value.Date || item.FechaHasta.Value.Date > contrato.FechaDesde.Value.Date){
                                    ModelState.AddModelError("","El Contrato ya esta en uso");
                                    TempData["ErrorMessage"] = "El Inmueble ya posee un contrato en esa fechas.";
                                    return RedirectToAction(nameof(Create));
                                }
                            }
                        }
                        repositorioContrato.Alta(contrato);
                        TempData["Id"] = contrato.IdContrato;
                        return RedirectToAction(nameof(Index));
                    }else{
                        RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                        RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                        ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                        ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                        TempData["ErrorMessage"] = "Las fechas ingresadas no son validas. Por favor, corrígelos y envíalos nuevamente.";
                        return View(contrato);
                    }
                }else{
                    RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                    RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                    ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                    ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
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
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato contrato)
        {
            try
            {   RepositorioContrato repositorioContrato = new RepositorioContrato();
                 if(ModelState.IsValid && contrato.FechaDesde.HasValue && contrato.FechaHasta.HasValue){
                    if(contrato.FechaDesde.Value.Date < contrato.FechaHasta.Value.Date){
                        List<Contrato> inm = repositorioContrato.ObtenerTodos();
                        foreach (var item in inm){
                            if(item.IdInmueble == contrato.IdInmueble){
                                if(item.FechaDesde.Value.Date < contrato.FechaHasta.Value.Date && item.FechaHasta.Value.Date > contrato.FechaDesde.Value.Date){
                                    if(!item.Cancelado){
                                        ModelState.AddModelError("","El Contrato ya esta en uso");
                                        TempData["ErrorMessage"] = "El Inmueble ya posee un contrato en esa fechas.";
                                        return RedirectToAction(nameof(Edit));
                                    }else{
                                        repositorioContrato.Modificacion(contrato);
                                        TempData["Edit"] = contrato.IdContrato;
                                        return RedirectToAction(nameof(Index));
                                    }
                                }
                            }
                        }
                        repositorioContrato.Modificacion(contrato);
                        TempData["Edit"] = contrato.IdContrato;
                        return RedirectToAction(nameof(Index));
                    }else{
                        RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                        RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                        TempData["ErrorMessage"] = "La fecha hasta ingresada no son validas. Por favor, corrígelos y envíalos nuevamente.";
                        ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                        ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                        return View(contrato);
                    }
                }else{
                    RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                    RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                    ViewBag.Inquilinos = repositorioInquilino.ObtenerTodos();
                    return View(contrato);
                }
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
        [Authorize(Policy = "administrador")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, Contrato contrato)
        {
            try
            {
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                TempData["Delete"] = "Eliminada";
                repositorioContrato.Baja(id);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception)
            {
                throw;
            }
        }

    // GET: Contratos/FormFecha
        [Authorize]
        public ActionResult FormFecha()
        {
            try
            {
                return View();
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Contratos/ContratosVigentes
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ContratosVigentes(DateTime? FechaDesde, DateTime? FechaHasta)
        {
            try
            {   
                if(ModelState.IsValid && FechaDesde.HasValue && FechaHasta.HasValue){
                    if(FechaDesde.Value.Date < FechaHasta.Value.Date){
                        RepositorioContrato repositorioContrato = new RepositorioContrato();
                        List<Contrato> contratosVigentes = repositorioContrato.ContratosVigentes(FechaDesde.Value,FechaHasta.Value);
                        ViewBag.FechaInicio = FechaDesde;
                        ViewBag.FechaFin = FechaHasta;
                        return View(contratosVigentes);
                    }else{
                        TempData["ErrorMessage"] = "Las Fechas ingresadas son incorrectas. Por favor, corrígelos y envíalos nuevamente.";
                        return RedirectToAction(nameof(FormFecha));
                    }
                }else{
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return RedirectToAction(nameof(FormFecha));
                }
                
            }
            catch(System.Exception)
            {
                throw;
            }
        }


        // GET: Contratos/FormInmueble
        [Authorize]
        public ActionResult FormInmueble()
        {
            try
            {   RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                return View();
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Contratos/ContratosxInmueble
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ContratosxInmueble(int IdInmueble)
        {
            try
            {   
                if(ModelState.IsValid){
                    RepositorioContrato repositorioContrato = new RepositorioContrato();
                    RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                    List<Contrato> contratos = repositorioContrato.ContratosxInmueble(IdInmueble);
                    ViewBag.IdInmueble = IdInmueble;
                    return View(contratos);
                }else{
                    RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                    ViewBag.Inmuebles = repositorioInmueble.ObtenerTodos();
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return RedirectToAction(nameof(FormInmueble));
                }
                
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // GET: Contratos/FormRangoFecha
        [Authorize]
        public ActionResult FormRangoFecha()
        {
            try
            {   
                return View();
            }
            catch(System.Exception)
            {
                throw;
            }
        }

        // POST: Contratos/ListarInmueblesDisponibles
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult ListarInmueblesDisponibles(DateTime? FechaDesde, DateTime? FechaHasta)
        {
            try
            {   if(ModelState.IsValid && FechaDesde.HasValue && FechaHasta.HasValue){
                    if(FechaDesde.Value.Date < FechaHasta.Value.Date){
                        RepositorioContrato repositorioContrato = new RepositorioContrato();
                        List<Inmueble> inmuebles = repositorioContrato.InmueblesDisponiblesxFecha(FechaDesde.Value,FechaHasta.Value);
                        ViewBag.fechaInicio = FechaDesde.Value.Date;
                        ViewBag.FechaFin = FechaHasta.Value.Date;
                        return View(inmuebles);
                    }else{
                        TempData["ErrorMessage"] = "Las fechas ingresadas no son validas. Por favor, corrígelos y envíalos nuevamente.";
                        return RedirectToAction(nameof(FormRangoFecha));
                    }
                }else{
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    return RedirectToAction(nameof(FormRangoFecha));
                }
            }
            catch(System.Exception)
            {
                throw;
            }
        }

    // GET: Contratos/Renovar/5
        [Authorize]
        public ActionResult Renovar(int id)
        {
            try
            {   
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                Contrato contrato = repositorioContrato.ObtenerPorId(id);
                DateTime fecha = DateTime.Now;
                ViewBag.FechaHasta = fecha;
                ViewBag.Inquilino = repositorioInquilino.ObtenerPorId(contrato.IdInquilino);
                ViewBag.Inmueble = repositorioInmueble.ObtenerPorId(contrato.IdInmueble);
                return View(contrato);
            }
            catch(System.Exception)
            {
                throw;
            }
        }
    // POST: Contratos/Renovar/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult Renovar(Contrato contrato)
        {
            try
            {   RepositorioContrato repositorioContrato = new RepositorioContrato();
                 if(ModelState.IsValid && contrato.FechaDesde.HasValue && contrato.FechaHasta.HasValue){
                    if(contrato.FechaDesde.Value.Date < contrato.FechaHasta.Value.Date){
                        List<Contrato> inm = repositorioContrato.ObtenerTodos();
                        foreach (var item in inm){
                            if(item.IdInmueble == contrato.IdInmueble){
                                if(item.FechaDesde.Value.Date > contrato.FechaHasta.Value.Date){
                                    ModelState.AddModelError("","El Contrato ya esta en uso");
                                    TempData["ErrorMessage"] = "El Inmueble ya posee un contrato en esa fechas.";
                                    return RedirectToAction(nameof(Create));
                                }
                            }
                        }
                        repositorioContrato.Modificacion(contrato);
                        TempData["Renovado"] = contrato.IdContrato;
                        return RedirectToAction(nameof(Index));
                    }else{
                        RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                        RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                        TempData["ErrorMessage"] = "La fecha hasta ingresada no son validas. Por favor, corrígelos y envíalos nuevamente.";
                        DateTime fecha = DateTime.Now;
                        ViewBag.FechaHasta = fecha;
                        ViewBag.Inquilino = repositorioInquilino.ObtenerPorId(contrato.IdInquilino);
                        ViewBag.Inmueble = repositorioInmueble.ObtenerPorId(contrato.IdInmueble);
                        return View(contrato);
                    }
                }else{
                    RepositorioInquilino repositorioInquilino = new RepositorioInquilino();
                    RepositorioInmueble repositorioInmueble = new RepositorioInmueble();
                    TempData["ErrorMessage"] = "Hay errores en el formulario. Por favor, corrígelos y envíalos nuevamente.";
                    DateTime fecha = DateTime.Now;
                    ViewBag.FechaHasta = fecha;
                    ViewBag.Inquilino = repositorioInquilino.ObtenerPorId(contrato.IdInquilino);
                    ViewBag.Inmueble = repositorioInmueble.ObtenerPorId(contrato.IdInmueble);
                    return View(contrato);
                }
            }
            catch(System.Exception)
            {
                throw;
            }
        }

    // GET: Contratos/Cancelar/5
        [Authorize]
        public ActionResult Cancelar(int id)
        {
            try
            {   RepositorioContrato repositorioContrato = new RepositorioContrato();
                Contrato contrato = repositorioContrato.ObtenerPorId(id);
                ViewBag.Contrato = contrato;
                DateTime fechaActual = DateTime.Now;
                DateTime fechaFin = contrato.FechaHasta.Value;
                TimeSpan diferencia = fechaFin - fechaActual;
                double multa = diferencia.Days * 5000;
                ViewBag.Multa = multa;
                return View();
            }
            catch(System.Exception)
            {
                throw;
            }
        }

    // POST: Contratos/CancelarContrato/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public ActionResult CancelarContrato(int id)
        {
            try
            {   
                RepositorioContrato repositorioContrato = new RepositorioContrato();
                Contrato contrato = repositorioContrato.ObtenerPorId(id);
                TempData["Cancelado"] = "cancelado";
                repositorioContrato.Cancelar(contrato);
                return RedirectToAction(nameof(Index));
            }
            catch(System.Exception)
            {
                throw;
            }
        }

    }
}
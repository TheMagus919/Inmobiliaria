using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using proyecto.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;

namespace proyecto.Controllers
{   
    public class UsuariosController : Controller
    {
        private readonly IConfiguration configuration;
        private readonly IWebHostEnvironment environment;
        private String[] roles = {"administrador", "empleado"};
		public UsuariosController(IConfiguration configuration, IWebHostEnvironment environment)
		{
			this.configuration = configuration;
			this.environment = environment;
		}

        [AllowAnonymous]
		// GET: Usuarios/Login/
		public ActionResult Login(string returnUrl)
		{
			TempData["Url"] = returnUrl;
			return View();
		}

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        // POST: Usuarios/Login/
        public async Task<IActionResult> Login(string email, string password){
            try{
				var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
				{
					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: password,
						salt: System.Text.Encoding.ASCII.GetBytes("Comerciante"),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8));
                    RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
                    Usuario us = repositorioUsuario.ObtenerPorEmail(email);
					if (us == null || us.Password != hashed)
					{
						ModelState.AddModelError("", "El email o la clave no son correctos");
						TempData["returnUrl"] = returnUrl;
						return View();
					}
                    
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, us.Email),
						new Claim("FullName", us.Nombre + " " + us.Apellido),
						new Claim(ClaimTypes.Role, us.Rol),
					};

					var claimsIdentity = new ClaimsIdentity(
							claims, CookieAuthenticationDefaults.AuthenticationScheme);

					await HttpContext.SignInAsync(
							CookieAuthenticationDefaults.AuthenticationScheme,
							new ClaimsPrincipal(claimsIdentity));

					TempData.Remove("returnUrl");
					return Redirect(returnUrl);
				}
				TempData["returnUrl"] = returnUrl;
				return View();
			}
			catch (Exception ex)
			{
				ModelState.AddModelError("", ex.Message);
				return View();
			}
        }

        // GET: Usuarios
		[Authorize(Policy = "administrador")]
		public ActionResult Index()
		{   
            RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
			var usuarios = repositorioUsuario.ObtenerTodos();
			return View(usuarios);
		}

		// GET: Usuarios/Details/5
		[Authorize(Policy = "administrador")]
		public ActionResult Details(int id)
		{   
			try{
                RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
				var u = repositorioUsuario.ObtenerPorId(id);
				ViewBag.IdUs = u.IdUsuario;
				return View(u);

            }catch(System.Exception){
                throw;
            }
            
		}

		// GET: Usuarios/Create
		[Authorize(Policy = "administrador")]
		public ActionResult Create()
		{
			ViewBag.Roles = roles;
			return View();
		}

		// POST: Usuarios/Create
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "administrador")]
		public ActionResult Create(Usuario u)
		{
				if (!ModelState.IsValid){
				return View();	}
			
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
								password: u.Password,
								salt: System.Text.Encoding.ASCII.GetBytes("Comerciante"),
								prf: KeyDerivationPrf.HMACSHA1,
								iterationCount: 1000,
								numBytesRequested: 256 / 8));
				u.Password = hashed;
				var nbreRnd = Guid.NewGuid();
                RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
				int res = repositorioUsuario.Alta(u);
				if (u.AvatarFile != null && u.IdUsuario > 0)
				{
					string wwwPath = environment.WebRootPath;
					string path = Path.Combine(wwwPath, "Uploads");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					string fileName = "avatar_" + u.IdUsuario + Path.GetExtension(u.AvatarFile.FileName);
					string pathCompleto = Path.Combine(path, fileName);
					u.Avatar = Path.Combine("/Uploads", fileName);
					using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
					{
						u.AvatarFile.CopyTo(stream);
					}
					repositorioUsuario.Modificacion(u);
				}
				return RedirectToAction(nameof(Index));
			}
			catch (Exception ex)
			{
				ViewBag.Roles = roles;
				return View();
			}
		}

		// GET: Usuarios/Perfil/5
		[Authorize]
		public ActionResult Perfil()
		{
			ViewData["Title"] = "Mi perfil";
            RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
			var u = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
			ViewBag.Roles = roles;
			ViewBag.Avatar = u.Avatar;
			ViewBag.IdUs = u.IdUsuario;
			return View("Perfil", u);
		}

		// GET: Usuarios/EditarPerfil/5
		[Authorize]
		public ActionResult EditarPerfil(int id, string returnUrl = null)
		{
			ViewData["Title"] = "Editar usuario";
            RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
			var u = repositorioUsuario.ObtenerPorId(id);
			ViewBag.Roles = roles;
			ViewBag.Avatar = u.Avatar;
			ViewBag.IdUs = u.IdUsuario;
			ViewBag.ReturnUrl = returnUrl ?? "Index";
			return View(u);
		}

		// POST: Usuarios/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult EditarPerfil(int id, Usuario u)
		{
			var vista = nameof(Edit);
			Usuario? us = null;
			try
			{
				if (!User.IsInRole("administrador"))
				{
					vista = nameof(Perfil);
                    RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
					var usuarioActual = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
					usuarioActual.Apellido = u.Apellido;
					usuarioActual.Nombre = u.Nombre;
					usuarioActual.Email = u.Email;
					usuarioActual.Rol = u.Rol;
					repositorioUsuario.Modificacion(usuarioActual);
				}else{
					RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
					var usuarioActual = repositorioUsuario.ObtenerPorId(id);
					usuarioActual.Apellido = u.Apellido;
					usuarioActual.Nombre = u.Nombre;
					usuarioActual.Email = u.Email;
					usuarioActual.Rol = u.Rol;
					repositorioUsuario.Modificacion(usuarioActual);
				}
				ViewBag.Id = u.IdUsuario;
				if(User.IsInRole("administrador")){
					return RedirectToAction(nameof(Index));
				}else{
					return RedirectToAction(nameof(Perfil));
				}
			}
			catch (Exception ex){
				throw;
			}
		}

		[Authorize]
		public ActionResult CambiarPassword(int id){
			RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
			var u = repositorioUsuario.ObtenerPorId(id);
			ViewBag.Roles = roles;
			return View("CambiarPassword",u);
		}
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize]
		public ActionResult CambiarPasswordd(int id, Usuario usuario, string ConfirmarPassword)
		{
			try
			{
				RepositorioUsuario repo = new RepositorioUsuario();
				var usuarioDB = repo.ObtenerPorId(usuario.IdUsuario);
				if (usuario.Password != ConfirmarPassword)
				{
					ViewBag.Error = "Las contraseñas no coinciden";
					return View("CambiarPassword",usuario);
				} else 
				{
					string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
						password: usuario.Password,
						salt: System.Text.Encoding.ASCII.GetBytes("Comerciante"),
						prf: KeyDerivationPrf.HMACSHA1,
						iterationCount: 1000,
						numBytesRequested: 256 / 8));
					usuario.Password = hashed;
					usuarioDB.Password = usuario.Password;
					repo.ModificarContraseña(usuarioDB);
				}
				return RedirectToAction("Index");
			} 
			catch (System.Exception)
			{
				throw;
			}
		}
		// GET: Usuarios/CambiarFoto/5
		[Authorize]
		public ActionResult CambiarFoto(int id)
		{	
			RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
			var u = repositorioUsuario.ObtenerPorId(id);
			ViewBag.IdUsuario = id;
			return View("CambiarFoto",u);
		}

		// POST: Usuarios/CambiarFoto/5
		[Authorize]
		public ActionResult CambiarFotoo(int id, Usuario u, IFormFile AvatarFile)
		{
            RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
			Usuario us = repositorioUsuario.ObtenerPorId(u.IdUsuario);
			if (AvatarFile != null && us.IdUsuario> 0)
				{
					string wwwPath = environment.WebRootPath;
					string path = Path.Combine(wwwPath, "Uploads");
					if (!Directory.Exists(path))
					{
						Directory.CreateDirectory(path);
					}
					string fileName = "avatar_" + us.IdUsuario + Path.GetExtension(AvatarFile.FileName);
					string pathCompleto = Path.Combine(path, fileName);
					us.Avatar = Path.Combine("/Uploads", fileName);
					using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
					{
						AvatarFile.CopyTo(stream);
					}
					repositorioUsuario.ModificarAvatar(us);
				}
			ViewBag.Id = us.IdUsuario;
			if(User.IsInRole("administrador")){
				return RedirectToAction(nameof(Index));
			}else{
				return RedirectToAction(nameof(Perfil));
			}
			
		}

		// GET: Usuarios/Edit/5
		[Authorize(Policy = "administrador")]
		public ActionResult Edit(int id, string returnUrl = null)
		{
			ViewData["Title"] = "Editar usuario";
            RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
			var u = repositorioUsuario.ObtenerPorId(id);
			ViewBag.Roles = roles;
			ViewBag.Avatar = u.Avatar;
			ViewBag.IdUs = u.IdUsuario;
			ViewBag.ReturnUrl = returnUrl ?? "Index";
			return View(u);
		}

		// POST: Usuarios/Edit/5
		[HttpPost]
		[ValidateAntiForgeryToken]
		[Authorize(Policy = "administrador")]
		public ActionResult Edit(int id, Usuario u)
		{
			var vista = nameof(Edit);
			Usuario? us = null;
			try
			{
				if (!User.IsInRole("administrador"))
				{
					vista = nameof(Perfil);
                    RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
					var usuarioActual = repositorioUsuario.ObtenerPorEmail(User.Identity.Name);
					usuarioActual.Apellido = u.Apellido;
					usuarioActual.Nombre = u.Nombre;
					usuarioActual.Email = u.Email;
					usuarioActual.Rol = u.Rol;
					repositorioUsuario.Modificacion(usuarioActual);
				}else{
					RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
					var usuarioActual = repositorioUsuario.ObtenerPorId(id);
					usuarioActual.Apellido = u.Apellido;
					usuarioActual.Nombre = u.Nombre;
					usuarioActual.Email = u.Email;
					usuarioActual.Rol = u.Rol;
					repositorioUsuario.Modificacion(usuarioActual);
				}
				ViewBag.Id = u.IdUsuario;
				if(User.IsInRole("administrador")){
					return RedirectToAction(nameof(Index));
				}else{
					return RedirectToAction(nameof(Perfil));
				}
			}
			catch (Exception ex){
				throw;
			}
		}

		// GET: Usuarios/Delete/5
		[Authorize(Policy = "administrador")]
		public ActionResult Delete(int id)
		{
			RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
			Usuario us = repositorioUsuario.ObtenerPorId(id);
			ViewBag.IdUs = us.IdUsuario;
			return View(us);
		}

		// POST: Usuarios/Delete/5
		[HttpPost]
		[Authorize(Policy = "administrador")]
		public ActionResult Delete(int id, Usuario usuario)
		{
			try
			{
				RepositorioUsuario repositorioUsuario = new RepositorioUsuario();
				repositorioUsuario.Baja(id);
				var ruta = Path.Combine(environment.WebRootPath, "Uploads", $"avatar_{id}" + Path.GetExtension(usuario.Avatar));
				if (System.IO.File.Exists(ruta))
					System.IO.File.Delete(ruta);
				TempData["Delete"] = true;
				return Json(new { success = true });
			}
			catch
			{
				return View();
			}
		}

		// GET: Usuarios/Logout
		public async Task<ActionResult> Logout()
		{
			await HttpContext.SignOutAsync(
					CookieAuthenticationDefaults.AuthenticationScheme);
			return RedirectToAction("Index", "Home");
		}
	}
}
using System.Data;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
namespace proyecto.Models;
public class RepositorioUsuario
	{
		protected readonly string connectionString;
		public RepositorioUsuario()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
		}

		public int Alta(Usuario u)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO usuarios 
					(Nombre, Apellido, Email, Password, Avatar, Rol)
					VALUES (@Nombre, @Apellido, @Email, @Password, @Avatar, @Rol);
					SELECT LAST_INSERT_ID()";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Nombre", u.Nombre);
					command.Parameters.AddWithValue("@Apellido", u.Apellido);
					command.Parameters.AddWithValue("@Email", u.Email);
					command.Parameters.AddWithValue("@Password", u.Password);
					if(String.IsNullOrEmpty(u.Avatar)){
						command.Parameters.AddWithValue("@Avatar",DBNull.Value);
					}else{
						command.Parameters.AddWithValue("@Avatar",u.Avatar);
					}
					command.Parameters.AddWithValue("@Rol", u.Rol);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					u.IdUsuario = res;
					connection.Close();
				}
			}
			return res;
		}
		public int Baja(int id)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = "DELETE FROM usuarios WHERE IdUsuario = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@id", id);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public int Modificacion(Usuario u)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE usuarios 
					SET Nombre=@nombre, Apellido=@apellido, Email=@Email, Rol=@Rol
					WHERE IdUsuario = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", u.Nombre);
					command.Parameters.AddWithValue("@apellido", u.Apellido);
					command.Parameters.AddWithValue("@Email", u.Email);
					command.Parameters.AddWithValue("@Rol", u.Rol);
					command.Parameters.AddWithValue("@id", u.IdUsuario);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public List<Usuario> ObtenerTodos()
		{
			List<Usuario> res = new List<Usuario>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
					IdUsuario, Nombre, Apellido, Email, Password, Rol
					FROM usuarios";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Usuario u = new Usuario
						{
							IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Email = reader.GetString("Email"),
							Password = reader.GetString("Password"),
							Rol = reader.GetString("Rol"),
						};
						res.Add(u);
					}
					connection.Close();
				}
			}
			return res;
		}

		public List<Usuario> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			List<Usuario> res = new List<Usuario>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT IdUsuario, Nombre, Apellido, Email, Password, Rol
					FROM usuarios
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Usuario u = new Usuario
						{
							IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),//más seguro
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Email = reader.GetString("Email"),
							Password = reader.GetString("Password"),
							Rol = reader.GetString("Rol"),
						};
						res.Add(u);
					}
					connection.Close();
				}
			}
			return res;
		}

		public Usuario ObtenerPorId(int id)
		{
			Usuario? u = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdUsuario, Nombre, Apellido, Email, Password, Avatar, Rol
					FROM usuarios
					WHERE IdUsuario=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					
					if (reader.Read())
					{	
						
						u = new Usuario{
									IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
									Nombre = reader.GetString("Nombre"),
									Apellido = reader.GetString("Apellido"),
									Email = reader.GetString("Email"),
									Password = reader.GetString("Password"),
									Avatar = reader["Avatar"] == DBNull.Value ? "" : reader.GetString("Avatar"),
									Rol = reader.GetString("Rol"),
								};
					}
					connection.Close();
				}
			}
			return u;
		}

		public int ModificarContraseña(Usuario usuario)
		{
			int res = 0;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE usuarios
					SET Password = @Password
					WHERE IdUsuario = @IdUsuario;";

				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@Password", usuario.Password);
					command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);

					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public int ModificarAvatar(Usuario usuario)
		{
			int res = 0;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE usuarios
					SET Avatar = @Avatar
					WHERE IdUsuario = @IdUsuario;";

				using (var command = new MySqlCommand(sql, connection))
				{
					command.Parameters.AddWithValue("@Avatar", usuario.Avatar);
					command.Parameters.AddWithValue("@IdUsuario", usuario.IdUsuario);

					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
		public Usuario ObtenerPorEmail(string email)
		{
			Usuario u = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdUsuario, Nombre, Apellido, Email, Password, Avatar, Rol
					FROM usuarios
					WHERE Email=@Email";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@Email", MySqlDbType.VarChar).Value = email;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						u = new Usuario
						{
							IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Email = reader.GetString("Email"),
							Avatar = reader["Avatar"] == DBNull.Value ? "" : reader.GetString("Avatar"),
							Password = reader.GetString("Password"),
							Rol = reader.GetString("Rol"),
						};
					}
					connection.Close();
				}
			}
			return u;
		}

		public List<Usuario> BuscarPorNombre(string nombre)
		{
			List<Usuario> res = new List<Usuario>();
			Usuario u = null;
			nombre = "%" + nombre + "%";
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdUsuario, Nombre, Apellido, Email, Password, Rol
					FROM usuarios
					WHERE Nombre LIKE @nombre OR Apellido LIKE @nombre";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = nombre;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						u = new Usuario
						{
							IdUsuario = reader.GetInt32(nameof(Usuario.IdUsuario)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Email = reader.GetString("Email"),
							Password = reader.GetString("Password"),
							Rol = reader.GetString("Rol"),
						};
						res.Add(u);
					}
					connection.Close();
				}
			}
			return res;
		}

	}
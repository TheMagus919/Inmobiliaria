using System.Data;
using MySql.Data.MySqlClient;
namespace proyecto.Models;
public class RepositorioPropietario
	{
		protected readonly string connectionString;
		public RepositorioPropietario()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
		}

		public int Alta(Propietario p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO propietarios 
					(Nombre, Apellido, Dni, Telefono, Email)
					VALUES (@Nombre, @Apellido, @Dni, @Telefono, @Email);
					SELECT LAST_INSERT_ID()";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Nombre", p.Nombre);
					command.Parameters.AddWithValue("@Apellido", p.Apellido);
					command.Parameters.AddWithValue("@Dni", p.Dni);
					command.Parameters.AddWithValue("@Telefono", p.Telefono);
					command.Parameters.AddWithValue("@Email", p.Email);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					p.IdPropietario = res;
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
				string sql = "DELETE FROM Propietarios WHERE IdPropietario = @id";
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
		public int Modificacion(Propietario p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE Propietarios 
					SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email
					WHERE IdPropietario = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", p.Nombre);
					command.Parameters.AddWithValue("@apellido", p.Apellido);
					command.Parameters.AddWithValue("@dni", p.Dni);
					command.Parameters.AddWithValue("@telefono", p.Telefono);
					command.Parameters.AddWithValue("@email", p.Email);
					command.Parameters.AddWithValue("@id", p.IdPropietario);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public List<Propietario> ObtenerTodos()
		{
			List<Propietario> res = new List<Propietario>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
					IdPropietario, Nombre, Apellido, Dni, Telefono, Email
					FROM propietarios";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Propietario p = new Propietario
						{
							IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public List<Propietario> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			List<Propietario> res = new List<Propietario>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email
					FROM Propietarios
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Propietario p = new Propietario
						{
							IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),//más seguro
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Propietario ObtenerPorId(int id)
		{
			Propietario p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email
					FROM Propietarios
					WHERE IdPropietario=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Propietario
						{
							IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
						};
					}
					connection.Close();
				}
			}
			return p;
		}

		public Propietario ObtenerPorEmail(string email)
		{
			Propietario p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email 
					FROM Propietarios
					WHERE Email=@email";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						p = new Propietario
						{
							IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
						};
					}
					connection.Close();
				}
			}
			return p;
		}

		public List<Propietario> BuscarPorNombre(string nombre)
		{
			List<Propietario> res = new List<Propietario>();
			Propietario p = null;
			nombre = "%" + nombre + "%";
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdPropietario, Nombre, Apellido, Dni, Telefono, Email
					FROM Propietarios
					WHERE Nombre LIKE @nombre OR Apellido LIKE @nombre";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = nombre;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						p = new Propietario
						{
							IdPropietario = reader.GetInt32(nameof(Propietario.IdPropietario)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

	}
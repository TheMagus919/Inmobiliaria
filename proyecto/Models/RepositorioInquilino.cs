using System.Data;
using MySql.Data.MySqlClient;
namespace proyecto.Models;
public class RepositorioInquilino
	{
		protected readonly string connectionString;
		public RepositorioInquilino()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
		}

		public int Alta(Inquilino i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO inquilinos
					(Nombre, Apellido, Dni, Telefono, Email)
					VALUES (@Nombre, @Apellido, @Dni, @Telefono, @Email);
					SELECT LAST_INSERT_ID()";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@Nombre", i.Nombre);
					command.Parameters.AddWithValue("@Apellido", i.Apellido);
					command.Parameters.AddWithValue("@Dni", i.Dni);
					command.Parameters.AddWithValue("@Telefono", i.Telefono);
					command.Parameters.AddWithValue("@Email", i.Email);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					i.IdInquilino = res;
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
				string sql = "DELETE FROM Inquilinos WHERE IdInquilino = @id";
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
		public int Modificacion(Inquilino i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE Inquilinos 
					SET Nombre=@nombre, Apellido=@apellido, Dni=@dni, Telefono=@telefono, Email=@email
					WHERE IdInquilino = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@nombre", i.Nombre);
					command.Parameters.AddWithValue("@apellido", i.Apellido);
					command.Parameters.AddWithValue("@dni", i.Dni);
					command.Parameters.AddWithValue("@telefono", i.Telefono);
					command.Parameters.AddWithValue("@email", i.Email);
					command.Parameters.AddWithValue("@id", i.IdInquilino);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public List<Inquilino> ObtenerTodos()
		{
			List<Inquilino> res = new List<Inquilino>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
					IdInquilino, Nombre, Apellido, Dni, Telefono, Email
					FROM inquilinos";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino i = new Inquilino
						{
							IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		public List<Inquilino> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			List<Inquilino> res = new List<Inquilino>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT IdInquilino, Nombre, Apellido, Dni, Telefono, Email
					FROM Inquilinos
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inquilino i = new Inquilino
						{
							IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),//más seguro
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Inquilino ObtenerPorId(int id)
		{
			Inquilino i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdInquilino, Nombre, Apellido, Dni, Telefono, Email
					FROM Inquilinos
					WHERE IdInquilino=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
					    i = new Inquilino
						{
							IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
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
			return i;
		}

		public Inquilino ObtenerPorEmail(string email)
		{
			Inquilino i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdInquilino, Nombre, Apellido, Dni, Telefono, Email 
					FROM Inquilinos
					WHERE Email=@email";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.Add("@email", MySqlDbType.VarChar).Value = email;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
						i = new Inquilino
						{
							IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
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
			return i;
		}

		public List<Inquilino> BuscarPorNombre(string nombre)
		{
			List<Inquilino> res = new List<Inquilino>();
			Inquilino i = null;
			nombre = "%" + nombre + "%";
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdInquilino, Nombre, Apellido, Dni, Telefono, Email
					FROM Inquilinos
					WHERE Nombre LIKE @nombre OR Apellido LIKE @nombre";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@nombre", MySqlDbType.VarChar).Value = nombre;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						i = new Inquilino
						{
							IdInquilino = reader.GetInt32(nameof(Inquilino.IdInquilino)),
							Nombre = reader.GetString("Nombre"),
							Apellido = reader.GetString("Apellido"),
							Dni = reader.GetString("Dni"),
							Telefono = reader.GetString("Telefono"),
							Email = reader.GetString("Email"),
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

	}
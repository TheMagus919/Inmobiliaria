using System.Data;
using MySql.Data.MySqlClient;
namespace proyecto.Models;
public class RepositorioInmueble
	{
		protected readonly string connectionString;
		public RepositorioInmueble()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
		}

		public int Alta(Inmueble i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO inmuebles
					(IdPropietario, CantidadDeAmbientes, Uso, Direccion, Tipo, Latitud, Longitud, Precio, Disponible)
					VALUES (@IdPropietario, @CantidadDeAmbientes, @Uso, @Direccion, @Tipo, @Latitud, @Longitud, @Precio, @Disponible);
					SELECT LAST_INSERT_ID()";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@IdPropietario", i.IdPropietario);
					command.Parameters.AddWithValue("@CantidadDeAmbientes", i.CantidadDeAmbientes);
					command.Parameters.AddWithValue("@Uso", i.Uso);
					command.Parameters.AddWithValue("@Direccion", i.Direccion);
					command.Parameters.AddWithValue("@Tipo", i.Tipo);
                    command.Parameters.AddWithValue("@Latitud", i.Latitud);
                    command.Parameters.AddWithValue("@Longitud", i.Longitud);
                    command.Parameters.AddWithValue("@Precio", i.Precio);
                    command.Parameters.AddWithValue("@Disponible", i.Disponible);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					i.IdInmueble = res;
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
				string sql = "DELETE FROM Inmuebles WHERE IdInmueble = @id";
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
		public int Modificacion(Inmueble i)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE Inmuebles 
					SET IdPropietario=@idPropietario, CantidadDeAmbientes=@cantidadDeAmbientes, Uso=@uso, Direccion=@direccion, Tipo=@tipo, Latitud=@latitud, Longitud=@longitud, Precio=@precio, Disponible=@disponible
					WHERE IdInmueble = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@idPropietario", i.IdPropietario);
					command.Parameters.AddWithValue("@cantidadDeAmbientes", i.CantidadDeAmbientes);
					command.Parameters.AddWithValue("@uso", i.Uso);
					command.Parameters.AddWithValue("@direccion", i.Direccion);
					command.Parameters.AddWithValue("@tipo", i.Tipo);
                    command.Parameters.AddWithValue("@latitud", i.Latitud);
                    command.Parameters.AddWithValue("@longitud", i.Longitud);
                    command.Parameters.AddWithValue("@precio", i.Precio);
                    command.Parameters.AddWithValue("@disponible", i.Disponible);
					command.Parameters.AddWithValue("@id", i.IdInmueble);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public List<Inmueble> ObtenerTodos()
		{
			List<Inmueble> res = new List<Inmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
					IdInmueble, i.IdPropietario, CantidadDeAmbientes, Uso, Direccion, Tipo, Latitud, Longitud, Precio, Disponible, p.Nombre, p.Apellido
					FROM inmuebles i INNER JOIN propietarios p ON i.IdPropietario = p.IdPropietario";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            IdPropietario = reader.GetInt32("IdPropietario"),
                            CantidadDeAmbientes = reader.GetInt32("CantidadDeAmbientes"),
                            Uso = reader.GetString("Uso"),
                            Direccion = reader.GetString("Direccion"),
                            Tipo = reader.GetString("Tipo"),
                            Latitud = reader.GetDecimal("Latitud"),
                            Longitud = reader.GetDecimal("Longitud"),
                            Precio = reader.GetDecimal("Precio"),
                            Disponible = reader.GetBoolean("Disponible"),
							Duenio = new Propietario{
								Nombre = reader.GetString("Nombre"),
								Apellido = reader.GetString("Apellido"),
							}
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		public List<Inmueble> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			List<Inmueble> res = new List<Inmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT IdInmueble, IdPropietario, CantidadDeAmbientes, Uso, Direccion, Tipo, Latitud, Longitud, Precio, Disponible
					FROM Inmuebles
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Inmueble i = new Inmueble
						{
							IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            IdPropietario = reader.GetInt32("IdPropietario"),
                            CantidadDeAmbientes = reader.GetInt32("CantidadDeAmbientes"),
                            Uso = reader.GetString("Uso"),
                            Direccion = reader.GetString("Direccion"),
                            Tipo = reader.GetString("Tipo"),
                            Latitud = reader.GetDecimal("Latitud"),
                            Longitud = reader.GetDecimal("Longitud"),
                            Precio = reader.GetDecimal("Precio"),
                            Disponible = reader.GetBoolean("Disponible"),
						};
						res.Add(i);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Inmueble ObtenerPorId(int id)
		{
			Inmueble i = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdInmueble, i.IdPropietario, CantidadDeAmbientes, Uso, Direccion, Tipo, Latitud, Longitud, Precio, Disponible, p.Nombre, p.Apellido
					FROM inmuebles i INNER JOIN propietarios p ON i.IdPropietario = p.IdPropietario
					WHERE IdInmueble=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
					    i = new Inmueble
						{
							IdInmueble = reader.GetInt32(nameof(Inmueble.IdInmueble)),
                            IdPropietario = reader.GetInt32("IdPropietario"),
                            CantidadDeAmbientes = reader.GetInt32("CantidadDeAmbientes"),
                            Uso = reader.GetString("Uso"),
                            Direccion = reader.GetString("Direccion"),
                            Tipo = reader.GetString("Tipo"),
                            Latitud = reader.GetDecimal("Latitud"),
                            Longitud = reader.GetDecimal("Longitud"),
                            Precio = reader.GetDecimal("Precio"),
                            Disponible = reader.GetBoolean("Disponible"),
							Duenio = new Propietario{
								Nombre = reader.GetString("Nombre"),
								Apellido = reader.GetString("Apellido"),
							},
						};
					}
					connection.Close();
				}
			}
			return i;
		}
	}
using System.Data;
using MySql.Data.MySqlClient;
namespace proyecto.Models;
public class RepositorioContrato
	{
		protected readonly string connectionString;
		public RepositorioContrato()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
		}

		public int Alta(Contrato c)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO contratos
					(IdInmueble, IdInquilino, FechaDesde, FechaHasta)
					VALUES (@IdInmueble, @IdInquilino, @FechaDesde, @FechaHasta);
					SELECT LAST_INSERT_ID()";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@IdInmueble", c.IdInmueble);
					command.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
					command.Parameters.AddWithValue("@FechaDesde", c.FechaDesde);
					command.Parameters.AddWithValue("@FechaHasta", c.FechaHasta);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					c.IdContrato = res;
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
				string sql = "DELETE FROM Contratos WHERE IdContrato = @id";
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
		public int Modificacion(Contrato c)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE Contratos 
					SET IdInmueble=@IdInmueble, IdInquilino=@IdInquilino, FechaDesde=@FechaDesde, FechaHasta=@FechaHasta
					WHERE IdContrato = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@IdInmueble", c.IdInmueble);
					command.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
					command.Parameters.AddWithValue("@FechaDesde", c.FechaDesde);
					command.Parameters.AddWithValue("@FechaHasta", c.FechaHasta);
					command.Parameters.AddWithValue("@id", c.IdContrato);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public List<Contrato> ObtenerTodos()
		{
			List<Contrato> res = new List<Contrato>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
					IdContrato, c.IdInmueble, c.IdInquilino, FechaDesde, FechaHasta, inq.Nombre, inq.Apellido, inm.IdPropietario
					FROM contratos c INNER JOIN inquilinos inq ON c.IdInquilino = inq.IdInquilino INNER JOIN inmuebles inm ON c.IdInmueble = inm.IdInmueble";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            IdInmueble = reader.GetInt32("IdInmueble"),
                            IdInquilino = reader.GetInt32("IdInquilino"),
                            FechaDesde = reader.GetDateTime("FechaDesde"),
                            FechaHasta = reader.GetDateTime("FechaHasta"),
							Vive = new Inquilino{
								Nombre = reader.GetString("Nombre"),
								Apellido = reader.GetString("Apellido"),
							},
							Lugar = new Inmueble{
								IdPropietario = reader.GetInt32("IdPropietario"),
							},
						};
						res.Add(c);
					}
					connection.Close();
				}
			}
			return res;
		}

		public List<Contrato> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			List<Contrato> res = new List<Contrato>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT IdContrato, IdInmueble, IdInquilino, FechaDesde, FechaHasta
					FROM Contratos
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Contrato c = new Contrato
						{
							IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            IdInmueble = reader.GetInt32("IdInmueble"),
                            IdInquilino = reader.GetInt32("IdInquilino"),
                            FechaDesde = reader.GetDateTime("FechaDesde"),
                            FechaHasta = reader.GetDateTime("FechaHasta"),
						};
						res.Add(c);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Contrato ObtenerPorId(int id)
		{
			Contrato c = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdContrato, IdInmueble, c.IdInquilino, FechaDesde, FechaHasta, inq.Nombre, inq.Apellido
					FROM contratos c INNER JOIN inquilinos inq ON c.IdInquilino = inq.IdInquilino
					WHERE IdContrato=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
					    c = new Contrato
						{
							IdContrato = reader.GetInt32(nameof(Contrato.IdContrato)),
                            IdInmueble = reader.GetInt32("IdInmueble"),
                            IdInquilino = reader.GetInt32("IdInquilino"),
                            FechaDesde = reader.GetDateTime("FechaDesde"),
                            FechaHasta = reader.GetDateTime("FechaHasta"),
							Vive = new Inquilino{
								Nombre = reader.GetString("Nombre"),
								Apellido = reader.GetString("Apellido"),
							}
						};
					}
					connection.Close();
				}
			}
			return c;
		}
	}
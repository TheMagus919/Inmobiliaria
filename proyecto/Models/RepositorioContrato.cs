using System.Data;
using Google.Protobuf;
using Microsoft.CodeAnalysis.Elfie.Serialization;
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
					IdContrato, c.IdInmueble, c.IdInquilino, FechaDesde, FechaHasta, Cancelado, inq.Nombre, inq.Apellido, inm.Direccion
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
							Cancelado = reader.GetBoolean("Cancelado"),
							Vive = new Inquilino{
								Nombre = reader.GetString("Nombre"),
								Apellido = reader.GetString("Apellido"),
							},
							Lugar = new Inmueble{
								Direccion = reader.GetString("Direccion"),
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
				string sql = @"SELECT IdContrato, c.IdInmueble, c.IdInquilino, FechaDesde, FechaHasta, Cancelado, inq.Nombre, inq.Apellido, inm.Direccion
					FROM contratos c INNER JOIN inquilinos inq ON c.IdInquilino = inq.IdInquilino INNER JOIN inmuebles inm ON c.IdInmueble = inm.IdInmueble
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
							Cancelado = reader.GetBoolean("Cancelado"),
							Lugar = new Inmueble{
								Direccion = reader.GetString("Direccion"),
							},
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
		public List<Contrato> ContratosVigentes(DateTime FechaDesde, DateTime FechaHasta, int paginaNro = 1, int tamPagina = 10)
		{
			List<Contrato> res = new List<Contrato>();
			DateTime hoy = DateTime.Today;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT IdContrato, c.IdInmueble, c.IdInquilino, FechaDesde, FechaHasta, Cancelado, inq.Nombre, inq.Apellido, inm.Direccion
					FROM contratos c INNER JOIN inquilinos inq ON c.IdInquilino = inq.IdInquilino INNER JOIN inmuebles inm ON c.IdInmueble = inm.IdInmueble
					WHERE FechaDesde <= @FechaHoy AND FechaHasta >= @FechaHoy AND Cancelado = 0
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@FechaHoy", hoy);
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
							Lugar = new Inmueble{
								Direccion = reader.GetString("Direccion"),
							},
							Vive = new Inquilino{
								Nombre = reader.GetString("Nombre"),
								Apellido = reader.GetString("Apellido"),
							}
						};
						res.Add(c);
					}
					connection.Close();
				}
			}
			return res;
		}

		public List<Contrato> ContratosxInmueble(int inmueble, int paginaNro = 1, int tamPagina = 10)
		{	
			List<Contrato> res = new List<Contrato>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT IdContrato, c.IdInmueble, c.IdInquilino, FechaDesde, FechaHasta, inq.Nombre, inq.Apellido, inm.Direccion
					FROM contratos c INNER JOIN inquilinos inq ON c.IdInquilino = inq.IdInquilino INNER JOIN inmuebles inm ON c.IdInmueble = inm.IdInmueble
					WHERE c.IdInmueble = @IdInmueble
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@IdInmueble", inmueble);
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
							Lugar = new Inmueble{
								Direccion = reader.GetString("Direccion"),
							},
							Vive = new Inquilino{
								Nombre = reader.GetString("Nombre"),
								Apellido = reader.GetString("Apellido"),
							}
						};
						res.Add(c);
					}
					connection.Close();
				}
			}
			return res;
		}

		public List<Inmueble> InmueblesDisponiblesxFecha(DateTime fechaInicio, DateTime fechaFin, int paginaNro = 1, int tamPagina = 10)
		{
			List<Inmueble> res = new List<Inmueble>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"
					SELECT *
					FROM Inmuebles i
					WHERE i.IdInmueble NOT IN(
						SELECT c.IdInmueble
						FROM Contratos c
						WHERE (c.FechaDesde >= @FechaInicio AND c.FechaHasta <= @FechaFin OR c.Cancelado = 0)
					)
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@FechaInicio", fechaInicio);
					command.Parameters.AddWithValue("@FechaFin", fechaFin);
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

		public int Cancelar(Contrato c)
		{	
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE Contratos 
					SET IdInmueble=@IdInmueble, IdInquilino=@IdInquilino, FechaDesde=@FechaDesde, FechaHasta=@FechaHasta, Cancelado=@cancelado
					WHERE IdContrato = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@IdInmueble", c.IdInmueble);
					command.Parameters.AddWithValue("@IdInquilino", c.IdInquilino);
					command.Parameters.AddWithValue("@FechaDesde", c.FechaDesde);
					command.Parameters.AddWithValue("@FechaHasta", c.FechaHasta);
					command.Parameters.AddWithValue("@id", c.IdContrato);
					command.Parameters.AddWithValue("@cancelado", 1);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}
	}
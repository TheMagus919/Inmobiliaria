using System.Data;
using MySql.Data.MySqlClient;
namespace proyecto.Models;
public class RepositorioPago
	{
		protected readonly string connectionString;
		public RepositorioPago()
		{
			connectionString = "Server=localhost;User=root;Password=;Database=inmobiliaria;SslMode=none";
		}

		public int Alta(Pago p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"INSERT INTO pagos
					(NumeroDePago, FechaDePago, Importe, IdContrato)
					VALUES (@NumeroDePago, @FechaDePago, @Importe, @IdContrato);
					SELECT LAST_INSERT_ID()";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@NumeroDePago", p.NumeroDePago);
					command.Parameters.AddWithValue("@FechaDePago", p.FechaDePago);
					command.Parameters.AddWithValue("@Importe", p.Importe);
					command.Parameters.AddWithValue("@IdContrato", p.IdContrato);
					connection.Open();
					res = Convert.ToInt32(command.ExecuteScalar());
					p.IdContrato = res;
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
				string sql = "DELETE FROM pagos WHERE IdPago = @id";
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
		public int Modificacion(Pago p)
		{
			int res = -1;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"UPDATE pagos
					SET NumeroDePago=@NumeroDePago, FechaDePago=@FechaDePago, Importe=@Importe, IdContrato=@IdContrato
					WHERE IdPago = @id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					command.Parameters.AddWithValue("@NumeroDePago", p.NumeroDePago);
					command.Parameters.AddWithValue("@FechaDePago", p.FechaDePago);
					command.Parameters.AddWithValue("@Importe", p.Importe);
					command.Parameters.AddWithValue("@IdContrato", p.IdContrato);
					command.Parameters.AddWithValue("@id", p.IdPago);
					connection.Open();
					res = command.ExecuteNonQuery();
					connection.Close();
				}
			}
			return res;
		}

		public List<Pago> ObtenerTodos()
		{
			List<Pago> res = new List<Pago>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT 
					IdPago, NumeroDePago, FechaDePago, Importe, p.IdContrato, c.IdInquilino, c.IdInmueble, inm.Direccion
					FROM pagos p INNER JOIN contratos c ON p.IdContrato = c.IdContrato INNER JOIN inmuebles inm ON c.IdInmueble = inm.IdInmueble";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago p = new Pago
						{
							IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                            NumeroDePago = reader.GetInt32("NumeroDePago"),
                            FechaDePago = reader.GetDateTime("FechaDePago"),
                            Importe = reader.GetDecimal("Importe"),
                            IdContrato = reader.GetInt32("IdContrato"),
							contrato = new Contrato{
                                IdContrato = reader.GetInt32("IdContrato"),
								IdInquilino = reader.GetInt32("IdInquilino"),
								IdInmueble = reader.GetInt32("IdInmueble"),
								Lugar = new Inmueble{
									Direccion = reader.GetString("Direccion"),
								}
							}
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		public List<Pago> ObtenerLista(int paginaNro = 1, int tamPagina = 10)
		{
			List<Pago> res = new List<Pago>();
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @$"SELECT 
					IdPago, NumeroDePago, FechaDePago, Importe, p.IdContrato
					FROM pagos p INNER JOIN contratos c ON p.IdContrato = c.IdContrato
					LIMIT {tamPagina} OFFSET {(paginaNro - 1) * tamPagina}
				";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					while (reader.Read())
					{
						Pago p = new Pago
						{
							IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                            NumeroDePago = reader.GetInt32("NumeroDePago"),
                            FechaDePago = reader.GetDateTime("FechaDePago"),
                            Importe = reader.GetDecimal("Importe"),
                            IdContrato = reader.GetInt32("IdContrato"),
							contrato = new Contrato{
                                IdContrato = reader.GetInt32("IdContrato"),
								IdInquilino = reader.GetInt32("IdInquilino"),
								IdInmueble = reader.GetInt32("IdInmueble"),
							}
						};
						res.Add(p);
					}
					connection.Close();
				}
			}
			return res;
		}

		virtual public Pago ObtenerPorId(int id)
		{
			Pago p = null;
			using (MySqlConnection connection = new MySqlConnection(connectionString))
			{
				string sql = @"SELECT IdPago, NumeroDePago, FechaDePago, Importe, p.IdContrato, c.IdInquilino, c.IdInmueble, inm.Direccion
					FROM pagos p INNER JOIN contratos c ON p.IdContrato = c.IdContrato INNER JOIN inmuebles inm ON c.IdInmueble = inm.IdInmueble
					WHERE IdPago=@id";
				using (MySqlCommand command = new MySqlCommand(sql, connection))
				{
					command.Parameters.Add("@id", MySqlDbType.Int32).Value = id;
					command.CommandType = CommandType.Text;
					connection.Open();
					var reader = command.ExecuteReader();
					if (reader.Read())
					{
					    p = new Pago
						{
							IdPago = reader.GetInt32(nameof(Pago.IdPago)),
                            NumeroDePago = reader.GetInt32("NumeroDePago"),
                            FechaDePago = reader.GetDateTime("FechaDePago"),
                            Importe = reader.GetDecimal("Importe"),
                            IdContrato = reader.GetInt32("IdContrato"),
							contrato = new Contrato{
                                IdContrato = reader.GetInt32("IdContrato"),
								IdInquilino = reader.GetInt32("IdInquilino"),
								IdInmueble = reader.GetInt32("IdInmueble"),
								Lugar = new Inmueble{
									Direccion = reader.GetString("Direccion"),
								}
							}
						};
					}
					connection.Close();
				}
			}
			return p;
		}
	}
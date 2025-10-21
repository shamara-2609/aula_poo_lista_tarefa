using System.ComponentModel.Design;
using System.Net;
using MySql.Data.MySqlClient;

public class Operacoes
{
    private string connectionString = 
    @"server=phpmyadmin.uni9.marize.us;User ID=user_poo;password=S3nh4!F0rt3;database=user_poo;";
    public int Criar(Tarefa tarefa)
    {
        using(var conexao = new MySqlConnection(connectionString))
        {
            conexao.Open();
            string sql = @"INSERT INTO tarefa (nome, descricao, dataCriacao, status, dataExecucao) 
                           VALUES (@nome, @descricao, @dataCriacao, @status, @dataExecucao);
                           SELECT LAST_INSERT_ID();";
            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@nome", tarefa.Nome);
                cmd.Parameters.AddWithValue("@descricao", tarefa.Descricao);
                cmd.Parameters.AddWithValue("@dataCriacao", tarefa.DataCriacao);
                cmd.Parameters.AddWithValue("@status", tarefa.Status);
                cmd.Parameters.AddWithValue("@dataExecucao", tarefa.DataExecucao);

                return Convert.ToInt32(cmd.ExecuteScalar());
            }
        }
    }
public Tarefa? Buscar(int id)
    {
        Tarefa? tarefa = null; 
        
        using (var conexao = new MySqlConnection(connectionString))
        {
            var sql = "SELECT id, nome, descricao, dataCriacao, dataExecucao, status FROM tarefa WHERE id = @id";

            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@id", id);
                
                conexao.Open();
                
                using (var reader = cmd.ExecuteReader())
                {
                    if (reader.Read()) 
                    {
                        tarefa = new Tarefa
                        {
                            
                            Id = reader.GetInt32("Id"),
                            Nome = reader.GetString("Nome"),
                            Descricao = reader.GetString("Descricao"),
                            DataCriacao = reader.GetDateTime("DataCriacao"),
                            DataExecucao = reader.IsDBNull(reader.GetOrdinal("DataExecucao"))
                                             ? (DateTime?)null
                                             : reader.GetDateTime("DataExecucao"),
                            Status = reader.GetInt32("Status")
                        };
                    }
                }
            }
        }
        
        return tarefa;
    }

    public IList<Tarefa> Listar()
    {
        var tarefas = new List<Tarefa>();
        using (var conexao = new MySqlConnection(connectionString))
        {
            var sql = "SELECT id, nome, descricao, dataCriacao, dataExecucao, status FROM tarefa";
            
            conexao.Open();

            using (var cmd = new MySqlCommand(sql, conexao))
            using (var reader = cmd.ExecuteReader())
            {
                while (reader.Read()) 
                {
                    var tarefa = new Tarefa
                    {
                        Id = reader.GetInt32("id"),
                        Nome = reader.GetString("nome"),
                        Descricao = reader.GetString("descricao"),
                        DataCriacao = reader.GetDateTime("dataCriacao"),
                        DataExecucao = reader.IsDBNull(reader.GetOrdinal("dataExecucao"))
                                             ? (DateTime?)null
                                             : reader.GetDateTime("dataExecucao"),
                        Status = reader.GetInt32("status")
                    };

                    tarefas.Add(tarefa);
                }
            }
        }

        return tarefas;
    }

    public void Alterar(Tarefa tarefa)
    {
        using (var conexao = new MySqlConnection(connectionString))
        {
            var sql = @"UPDATE tarefa 
                        SET nome = @nome, 
                            descricao = @descricao, 
                            dataExecucao = @dataExecucao, 
                            status = @status 
                        WHERE id = @id";

            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@nome", tarefa.Nome);
                cmd.Parameters.AddWithValue("@descricao", tarefa.Descricao);
                cmd.Parameters.AddWithValue("@dataExecucao", tarefa.DataExecucao ?? (object)DBNull.Value);
                cmd.Parameters.AddWithValue("@status", tarefa.Status);
                cmd.Parameters.AddWithValue("@id", tarefa.Id);

                conexao.Open();
                
                cmd.ExecuteNonQuery();
            }
        }
    }

    public void Excluir(int id)
    {
        using (var conexao = new MySqlConnection(connectionString))
        {
            var sql = "DELETE FROM tarefa WHERE id = @id";

            using (var cmd = new MySqlCommand(sql, conexao))
            {
                cmd.Parameters.AddWithValue("@id", id);

                conexao.Open();
                
                cmd.ExecuteNonQuery();
            }
        }
    }
}
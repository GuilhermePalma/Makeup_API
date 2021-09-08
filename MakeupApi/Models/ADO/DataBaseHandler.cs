using MySql.Data.MySqlClient;
using System;
using System.Configuration;
using System.Data;

namespace MakeupApi.Models.ADO
{
    public class DatabaseHandler : IDisposable
    {
        private const int ERROR = -1;
        private const int NOT_FOUND = 0;

        private readonly MySqlConnection mysqlConnection;
        private MySqlDataReader dataReader;

        public bool Has_connectionAvailable { get; set; }
        public string Error_operation { get; set; }

        // Inicia a Conexão com o Banco de Dados
        public DatabaseHandler()
        {
            // Inicia a Conexão e Informa que a Conexão foi Bem Sucedida
            try
            {
                // Obtem a Cadeia de Conexão do Banco de Dados
                mysqlConnection = new MySqlConnection(
                    ConfigurationManager.ConnectionStrings["connection"].ConnectionString);

                // Inicia uma conexão com o Banco de Dados
                mysqlConnection.Open();

                if (mysqlConnection.State == ConnectionState.Open)
                {
                    Has_connectionAvailable = true;
                }
            }
            catch (NullReferenceException ex)
            {
                Has_connectionAvailable = false;
                Error_operation = "Não foi possivel Obter a " +
                    "Conexão com o Banco de Dados";
                System.Diagnostics.Debug.WriteLine("Não foi Possivel obter a " +
                    "conexão com o Banco de Dados. Exception: \n" + ex);
            }
            catch (InvalidOperationException ex)
            {
                Has_connectionAvailable = false;
                Error_operation = "Não foi possivel iniciar a " +
                    "Conexão com o Banco de Dados";
                System.Diagnostics.Debug.WriteLine("Não foi Possivel iniciar o " +
                    "Banco de Dados. Exception: \n" + ex);
            }
            catch (MySqlException ex)
            {
                Has_connectionAvailable = false;
                Error_operation = "Houve uma Exceção na Conexão com o Banco de Dados";
                System.Diagnostics.Debug.WriteLine("Exceção no Banco de Dados." +
                    " Exception: \n" + ex);
            }
        }

        // Executa um comando SQL no Banco de Dados (Create, Insert, Update, Delete)
        public int executeCommand(string querySql)
        {
            // Verifica o comando recebido e Monta um Comando SQL
            if (string.IsNullOrEmpty(querySql)) return ERROR;
            MySqlCommand commandSql = new MySqlCommand
            {
                CommandText = querySql,
                CommandType = CommandType.Text,
                Connection = mysqlConnection
            };

            if (commandSql == null) return ERROR;
            int row_affected = 0;

            try
            {
                // Executa a query no Banco e Obtem o n° de afetados
                row_affected = commandSql.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                // Erro na Execução
                Error_operation = "Houve um Erro na Execução da Solicitação";
                System.Diagnostics.Debug.WriteLine("Não foi Possivel Executar o Comando." +
                    "Exceção: " + ex);
                return ERROR;
            }

            // Validação e Retorno da Execução da Query
            if (row_affected == -1)
            {
                Error_operation = "Metodo de Execução Invalido";
                return ERROR;
            }
            else if (row_affected == 0)
            {
                Error_operation = "Dados Solicitados não Encontrados";
                return NOT_FOUND;
            }
            else return row_affected;
        }

        // Realiza a Letirua de Dados do Banco de Dados
        public MySqlDataReader readerTable(string query)
        {
            // Verifica o comando recebido e Monta um Comando SQL
            if (string.IsNullOrEmpty(query))
            {
                Error_operation = "Solicitação Invalida/Vazia";
                return null;
            }

            MySqlCommand commandSql = new MySqlCommand
            {
                CommandText = query,
                CommandType = CommandType.Text,
                Connection = mysqlConnection
            };

            if (commandSql == null)
            {
                Error_operation = "Erro ao Criar a Solicitação para o Banco de Dados";
                return null;
            }

            try
            {
                dataReader = commandSql.ExecuteReader();
            }
            catch (Exception e)
            {
                // Erro na Execução
                Error_operation = "Houve um Erro na Execução da Leitura da Solicitação";
                System.Diagnostics.Debug.WriteLine("Não foi Possivel Ler os dados Solicitados" + e);
                return null;
            }

            if (dataReader == null || !dataReader.HasRows)
            {
                Error_operation = "Registros não Encontrados no Banco de Dados";
                return null;
            }

            return dataReader;
        }

        // Responsavel por Finalizar a Conexão e Reader após o Usign
        public void Dispose()
        {
            if (mysqlConnection != null || mysqlConnection.State == ConnectionState.Open)
                mysqlConnection.Close();

            if (dataReader != null) dataReader.Close();
        }
    }
}
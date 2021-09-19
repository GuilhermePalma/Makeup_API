using MakeupApi.Models.ADO;
using MySql.Data.MySqlClient;
using System;

namespace MakeupApi.Models.DAO
{
    public class TypeDAO
    {
        public static string TABLE_TYPE = "type";
        public static string ID_TYPE = "id_type";
        public static string NAME_TYPE = "name_type";

        private const string NAME_FOREIGN_ID = "id_type";

        MySqlDataReader dataReader;
        public const int ERROR = -1;
        public const int NOT_FOUND = 0;

        // Mensagem de Erro caso dê algum erro nas Operações
        public string Error_Operation { get; set; }

        // Validação da String Type
        public bool ValidationType(string type)
        {
            if (string.IsNullOrEmpty(type))
            {
                Error_Operation = "Campo 'Tipo da Maquiagem' é Obrigatorio";
                return false;
            }
            else return true;
        }

        // Retorna o ID do Tipo atraves de um Tipo (Nome do Tipo)
        public int ReturnIdType(string type)
        {
            // Valida o Tipo
            if (!ValidationType(type)) return ERROR;

            // Formação e Validação da Query
            string sql = "SELECT {0} FROM {1} WHERE {2}='{3}'";
            string[] data_sql = new string[]
            {
                ID_TYPE, TABLE_TYPE, NAME_TYPE, type
            };

            string command = StringFormat(sql, data_sql);
            if (string.IsNullOrEmpty(command)) return ERROR;

            // Operações no Banco de Dados
            using (DatabaseHandler database = new DatabaseHandler())
            {
                if (!database.Has_connectionAvailable)
                {
                    Error_Operation = database.Error_operation;
                    return ERROR;
                }

                // Obtem o reader do Banco de Dados
                dataReader = database.readerTable(command);
                if (dataReader == null)
                {
                    Error_Operation = database.Error_operation;
                    return NOT_FOUND;
                }

                // Tratamento de Exceções da Leitura do Reader
                try
                {
                    int code = NOT_FOUND;
                    dataReader.Read();
                    code = dataReader.GetInt32(dataReader.GetOrdinal(ID_TYPE));

                    if (code <= 0) Error_Operation = "Registro não Encontrado no Banco de Dados";
                    
                    // Retorna o Codigo ou 0 (não encontrado)
                    return code;
                }
                catch (IndexOutOfRangeException ex)
                {
                    Error_Operation = "Não foi possivel Obter os Dados.";
                    System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                    return ERROR;
                }
                catch (Exception ex)
                {
                    Error_Operation = "Não foi possivel obter o Codigo do Tipo.";
                    System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                    return ERROR;
                }
                finally
                {
                    CloseItens();
                }
            }
        }

        // Insere um Novo Tipo no Banco de Dados
        public bool InsertType(string type)
        {
            // Valida o Tipo Passado e Busca se Existe no Banco de Dados
            if (!ValidationType(type)) return false;
            else if (ReturnIdType(type) > 0)
            {
                Error_Operation = "Tipo Já Existente no Banco de Dados";
                return false;
            }

            // Criação e Validação do Comando SQL
            string sql = "INSERT INTO {0}({1}) VALUE('{2}')";
            string[] data_sql = new string[]
            {
                TABLE_TYPE, NAME_TYPE, type
            };

            string command = StringFormat(sql, data_sql);
            if (string.IsNullOrEmpty(command)) return false;

            // Valida e Realiza a Operação no Banco de Dados
            using (DatabaseHandler database = new DatabaseHandler())
            {
                if (!database.Has_connectionAvailable)
                {
                    // MysqlConnection não Disponivel
                    Error_Operation = database.Error_operation;
                    return false;
                }
                else if (database.executeCommand(command) <= 0)
                {
                    // Insert não Realizado
                    Error_Operation = database.Error_operation;
                    return false;
                }
                else return true;
            }
        }

        // Obtem o Nome de um Tipo a partir do ID
        public string SelectType(int id_type)
        {
            // Formação e Validação do SQL
            string sql = "SELECT {0} FROM {1} WHERE {2}={3}";
            string[] data_sql = new string[]
            {
                NAME_TYPE, TABLE_TYPE, ID_TYPE ,id_type.ToString()
            };

            string command = StringFormat(sql, data_sql);
            if (string.IsNullOrEmpty(command)) return null;

            // Operação e Validações com o Banco de Dados
            using (DatabaseHandler database = new DatabaseHandler())
            {
                if (!database.Has_connectionAvailable)
                {
                    // Conexão do Banco de Dados não Disponivel
                    Error_Operation = database.Error_operation;
                    return null;
                }
                else dataReader = database.readerTable(command);
                
                if (dataReader == null)
                {
                    Error_Operation = database.Error_operation;
                    return null;
                }

                // Tratamento de Exceção da Leitura do DataReader
                try
                {
                    dataReader.Read();
                    
                    string name_type = string.Empty;
                    name_type = dataReader.GetString(dataReader.GetOrdinal(NAME_TYPE));
                    return name_type;
                }
                catch (IndexOutOfRangeException ex)
                {
                    Error_Operation = "Não foi possivel Obter os Dados.";
                    System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                    return null;
                }
                catch (Exception ex)
                {
                    Error_Operation = "Não foi possivel Excluir o Tipo.";
                    System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                    return null;
                }
                finally
                {
                    CloseItens();
                }
            }
        }

        // Obtem o ID de um Tipo Valido
        public int IdTypeValid(string type)
        {
            // Obtem o Id do Tipo
            int id_type = ReturnIdType(type);

            if (id_type == ERROR)
            {
                // Erro na Validação ou Execução SQL
                return ERROR;
            }
            else if (id_type == NOT_FOUND)
            {
                // Caso não exista no Banco de Dados ---> Insere
                if (!InsertType(type)) return ERROR;

                // Obtem o codigo do Registro Inserido
                id_type = ReturnIdType(type);
            }

            // Retorna um Erro ou o Codigo Valido
            return id_type <= 0 ? ERROR : id_type;
        }

        // Verifica se a Makeup é a unica usando o Tipo 
        public bool IsOnlyType(string type)
        {
            // Obtem o ID do Tipo (No metodo ReturnIdType é feito a Validação do Tipo)
            int id_type = ReturnIdType(type);
            if (id_type <= 0) return false;
            
            // Formatação e Validação do Comando SQL
            string count_formmated = StringFormat("COUNT({0})", new string[] { NAME_FOREIGN_ID });
            string sql = "SELECT {0} FROM {1} WHERE {2}={3}";
            string[] parameters_sql = new string[]
            {
                count_formmated, MakeupDAO.TABLE_MAKEUP, NAME_FOREIGN_ID, id_type.ToString()
            };

            string command = StringFormat(sql, parameters_sql);
            if (string.IsNullOrEmpty(command) || string.IsNullOrEmpty(count_formmated)) return false;

            using (DatabaseHandler database = new DatabaseHandler())
            {
                if (!database.Has_connectionAvailable)
                {
                    // VErifica se a Conexão do Banco de Dados foi bem sucedida
                    Error_Operation = database.Error_operation;
                    return false;
                }
                else dataReader = database.readerTable(command);

                if (dataReader == null)
                {
                    Error_Operation = database.Error_operation;
                    return false;
                }

                // Tratamento de Exceções da Leitura do DataReader
                try
                {
                    int quantity_register = NOT_FOUND;

                    dataReader.Read();
                    quantity_register = dataReader.GetInt32(dataReader.GetOrdinal(count_formmated));

                    return quantity_register == 1;
                }
                catch (IndexOutOfRangeException ex)
                {
                    Error_Operation = "Não foi possivel Obter os Dados.";
                    System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                    return false;
                }
                catch (Exception ex)
                {
                    Error_Operation = "Não foi possivel obter Verificar o Tipo.";
                    System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                    return false;
                }
                finally
                {
                    CloseItens();
                }
            }
        }

        // Exclui se a Makeup é a unica usando o Tipo 
        public bool DeleteOnlyType(string type)
        {
            // Verificar se o Usuario é o unico usando aquele tipo
            if (IsOnlyType(type))
            {
                // Obtem e Valida o ID do Tipo passado
                int id_type = ReturnIdType(type);
                if (id_type < 1) return false;

                // Formação e Validação do Comando SQL
                string sql = "DELETE FROM {0} WHERE {1}={2}";
                string[] parameters_sql = new string[]
                {
                    TABLE_TYPE, ID_TYPE, id_type.ToString()
                };

                string command = StringFormat(sql, parameters_sql);
                if (string.IsNullOrEmpty(command)) return false;
                
                using (DatabaseHandler database = new DatabaseHandler())
                {
                    if (!database.Has_connectionAvailable)
                    {
                        // Verifica a disponibilidade da COnexão com o Banco de Dados
                        Error_Operation = database.Error_operation;
                        return false;
                    }
                    else if (database.executeCommand(command) <= 0)
                    {
                        // Execução do Comando não Afetou nenhum Registro = Erro
                        Error_Operation = "Não Foi Possivel Excluir o Tipo. "
                            + database.Error_operation;
                        return false;
                    }
                    else return true;
                }
            }
            else
            {
                // Não Exlcui o usuario pois não é o unico usando o Registro
                Error_Operation = string.Empty;
                return false;
            }
        }

        // Formata as Strings e Trata as Possiveis Exceptions
        private string StringFormat(string format, string[] parameters)
        {
            try
            {
                string stringFormated = string.Format(format, parameters);
                return stringFormated;
            }
            catch (ArgumentNullException ex)
            {
                Error_Operation = "Erro: Argumento Nulo na Criação da Query";
                System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                return string.Empty;
            }
            catch (FormatException ex)
            {
                Error_Operation = "Erro: Formação da String SQL Invalida";
                System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                return string.Empty;
            }
        }

        // Fecha os Itens (somente os instanciados)
        private void CloseItens()
        {
            if (dataReader != null) dataReader.Close();
        }
    }
}
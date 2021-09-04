using MakeupApi.Models.ADO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MakeupApi.Models.DAO
{
    public class UserDAO
    {
        // Get/Set da Mensagem de Erro em alguma Operação
        public string Error_operation { get; set; }

        public static string TABLE_USER = "user";
        public static string ID_USER = "id_user";
        public static string NAME = "name";
        public static string NICKNAME = "nickname";
        public static string EMAIL = "email";
        public static string PASSWORD = "password";
        public static string THEME = "theme_is_night";
        public static string IDIOM = "idiom";

        MySqlDataReader dataReader;
        public const int ERROR = -1;
        public const int NOT_FOUND = 0;
        
        // Retorna o Codigo do Usuario ou Informa que ele não Existe
        public int ReturnCodeUser(User user)
        {
            // Valida o Usuario e os Dados Utilizados
            string validationEmail = User.ValidationEmail(user.Email);
            string validationPassword = User.ValidationPassword(user.Password);
            if (validationEmail != User.OK)
            {
                Error_operation = validationEmail;
                return ERROR;
            } else if (validationPassword != User.OK)
            {
                Error_operation = validationPassword;
                return ERROR;
            }

            using (DatabaseHandler database = new DatabaseHandler())
            {
                // Valida se há Conexão Disponivel
                if (!database.Has_connectionAvailable)
                {
                    Error_operation = database.Error_operation;
                    return ERROR;
                }

                // Consulta no Banco de Dadoso Usuario
                string commandSql = string.Format("SELECT {0} FROM {2} WHERE {3}={4} AND {5}={5}",
                    ID_USER, TABLE_USER, EMAIL, user.Email, PASSWORD, user.Password);

                // Consulta se o Usuario existe no Banco de Dados
                int returnSelect = database.executeCommand(commandSql);
                if (returnSelect == NOT_FOUND)
                {
                    Error_operation = database.Error_operation;
                    return NOT_FOUND;
                }
                else if (returnSelect == ERROR)
                {
                    Error_operation = database.Error_operation;
                    return ERROR;
                }

                try
                {
                    // Obtem o ID do Usuario do Banco de Dados
                    dataReader = database.readerTable(commandSql);
                    if (dataReader == null)
                    {
                        Error_operation = "Não foi possivel Ler os dados do Banco de Dados. Erro:" +
                            database.Error_operation;
                        return ERROR;
                    }

                    // Executa a leitura dos dados
                    dataReader.Read();
                    return dataReader.GetInt32(dataReader.GetOrdinal(ID_USER));
                }
                catch (Exception ex)
                {
                    Error_operation = "Não foi possivel Ler os dados do Banco de Dados." +
                        " Exceção: " + ex + "Erro:" + database.Error_operation;
                    return ERROR;
                }
                finally 
                {
                    closeItens();
                }
            }
        }

        // Insere um Usuario no Banco de Dados se não existir
        public bool InsertUser(User user)
        {
            string validationUser = User.ValidationUser(user);
            if (validationUser != User.OK)
            {
                Error_operation = validationUser;
                return false;
            }

            // Verifica se o Usuario existe no Banco de Dados
            if (ReturnCodeUser(user) == ERROR) return false;
            if (ReturnCodeUser(user) != NOT_FOUND)
            {
                Error_operation = "Usuario já esta cadastrado no Banco de Dados";
            }

            using (DatabaseHandler database = new DatabaseHandler())
            {
                // Valida se há Conexão Disponivel
                if (!database.Has_connectionAvailable)
                {
                    Error_operation = database.Error_operation;
                    return false;
                }

                string commandSql = string.Format("INSERT INTO {0}({1},{2},{3},{4},{5},{6}) " +
                    "VALUE('{7}','{8}','{9}','{10}','{11}',{12})", TABLE_USER, NICKNAME, EMAIL,
                    NAME, PASSWORD, IDIOM, THEME, user.Nickname, user.Email, user.Name,
                    user.Password, user.Idioms, user.Theme_is_night);

                // Insere o Usuario no Sistema e Valida
                int row_affected = database.executeCommand(commandSql);
                if (row_affected != 1)
                {
                    Error_operation = database.Error_operation;
                    return false;
                }
                else return true;
            }
        }

        // Exclui um Usuario do Banco de Dados
        public bool DeleteUser(int id_user)
        {
            // Valida o Codigo do Usuario
            if (id_user <= 0)
            {
                Error_operation = string.Format("Codigo de Usuario ({0}) Invalido", id_user);
                return false;
            } else if (!IsValidCode(id_user)) return false;

            using (DatabaseHandler database = new DatabaseHandler())
            {
                // Valida se há Conexão Disponivel
                if (!database.Has_connectionAvailable)
                {
                    Error_operation = database.Error_operation;
                    return false;
                }

                string deleteSQL = string.Format("DELETE FROM {0} WHERE {1}={2}",
                    TABLE_USER, ID_USER, id_user);

                // Caso delete apenos o Registro Infromado = retorna True
                if (database.executeCommand(deleteSQL) != 1)
                {
                    Error_operation = database.Error_operation;
                    return false;
                }
                else return true;
            }
        }

        // Atualiza um Usuario do Banco de Dados
        public bool UpdateUser(User user)
        {
            // Valida o Usuario e o Codigo Passado
            string validationUser = User.ValidationUser(user);
            if(validationUser != User.OK)
            {
                Error_operation = validationUser;
                return false;
            } else if (!IsValidCode(user.Id)) return false;

            using (DatabaseHandler database = new DatabaseHandler())
            {
                // Valida se há Conexão Disponivel
                if (!database.Has_connectionAvailable)
                {
                    Error_operation = database.Error_operation;
                    return false;
                }

                string updateSQL = string.Format("UPDATE {0} SET {1}='{2}', {2}='{3}', " +
                    "{4}='{5}', {6}='{7}', {8}='{9}', {10}='{11}', {12}='{13}', " +
                    "WHERE {14}={25}", TABLE_USER, EMAIL, user.Email, PASSWORD, user.Password,
                    NAME, user.Name, NICKNAME, user.Nickname, IDIOM, user.Idioms, THEME,
                    user.Theme_is_night, ID_USER, user.Id);

                // Caso altere o Usuario especificado ---> Retorna True
                if (database.executeCommand(updateSQL) != 1)
                {
                    Error_operation = database.Error_operation;
                    return false;
                }
                else return true;
            }
        }

        // Obtem um Usuario do Banco de Dados
        public User SelectUser(int id_user)
        {
            // Verifica se o Codigo Informado Existe no Banco de Dados
            if (!IsValidCode(id_user)) return null;

            using (DatabaseHandler database = new DatabaseHandler())
            {
                // Valida se há Conexão Disponivel
                if (!database.Has_connectionAvailable)
                {
                    Error_operation = database.Error_operation;
                    return null;
                }

                string selectUser = string.Format("SELECT {0},{1},{2},{3},{4} " +
                    "FROM {5} WHERE {6}={7}", EMAIL, NAME, NICKNAME, IDIOM, THEME,
                    TABLE_USER, ID_USER, id_user);

                // Realiza a Leitura do SELECT
                MySqlDataReader dataReader = database.readerTable(selectUser);
                if (dataReader == null)
                {
                    Error_operation = database.Error_operation;
                    return null;
                }

                try
                {
                    // Executa a leitura dos dados
                    dataReader.Read();
                    return new User()
                    {
                        Id = id_user,
                        Name = dataReader.GetString(dataReader.GetOrdinal(NAME)),
                        Nickname = dataReader.GetString(dataReader.GetOrdinal(NICKNAME)),
                        Email = dataReader.GetString(dataReader.GetOrdinal(EMAIL)),
                        Idioms = dataReader.GetString(dataReader.GetOrdinal(IDIOM)),
                        Theme_is_night = dataReader.GetBoolean(dataReader.GetOrdinal(THEME))
                    };
                }
                catch (Exception ex)
                {
                    Error_operation = "Não foi possivel Ler os dados do Banco de Dados." +
                        " Exceção: " + ex + "Erro:" + database.Error_operation;
                    return null;
                }
                finally
                {
                    closeItens();
                }
            }
        }

        // Verifica se o Codigo do Usuario existe no Banco de Dados
        public bool IsValidCode(int code_user)
        {
            if(code_user <= 0) 
            {
                Error_operation = "Codigo de Usuario Invalido.";
                return false;
            }

            using (DatabaseHandler database = new DatabaseHandler())
            {
                // Valida se há Conexão Disponivel
                if (!database.Has_connectionAvailable)
                {
                    Error_operation = database.Error_operation;
                    return false;
                }

                // Verifica se o Codigo é Valido
                string selectSQL = string.Format("SELECT {0} FROM {1} WHERE {0}={2}",
                    ID_USER, TABLE_USER, code_user);

                MySqlDataReader dataReader = database.readerTable(selectSQL);
                if (dataReader == null)
                {
                    Error_operation = database.Error_operation;
                    return false;
                }

                // Lê a tabela e obtem o ID do registro com o ID passado
                dataReader.Read();
                int amount_register = dataReader.GetInt32(dataReader.GetOrdinal(ID_USER));
                return amount_register > 0;
            }
        }

        private void closeItens()
        {
            if (dataReader != null) dataReader.Close();
        }
    }
}
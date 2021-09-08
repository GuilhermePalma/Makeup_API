using MakeupApi.Models.ADO;
using MySql.Data.MySqlClient;
using System;

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

        // Verifica se um Usuario Existe no banco de Dados
        public bool ExistsUser(string email, string password)
        {
            // Valida o Usuario e os Dados Utilizados
            string validationEmail = User.ValidationEmail(email);
            string validationPassword = User.ValidationPassword(password);
            if (validationEmail != User.OK)
            {
                Error_operation = validationEmail;
                return false;
            }
            else if (validationPassword != User.OK)
            {
                Error_operation = validationPassword;
                return false;
            }

            // Formação e Tratamento de Erros nos comandos SQL
            string count_formatted, countSQL;
            try
            {
                count_formatted = string.Format("COUNT({0})", ID_USER);
                countSQL = string.Format("SELECT {0} FROM {1} WHERE {2}='{3}' AND {4}='{5}'",
                    count_formatted, TABLE_USER, EMAIL, email, PASSWORD, password);
            }
            catch (ArgumentNullException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Parametros Invalidos. Exceção: " + ex;
                return false;
            }
            catch (FormatException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Formação da Solicitação. Exceção: " + ex;
                return false;
            }

            // Operações no banco de Dados
            using (DatabaseHandler database = new DatabaseHandler())
            {
                try
                {
                    dataReader = database.readerTable(countSQL);

                    if (dataReader == null)
                    {
                        Error_operation = database.Error_operation;
                        return false;
                    }

                    // Executa a leitura dos dados
                    dataReader.Read();
                    int return_quantity = dataReader.GetInt32(dataReader.GetOrdinal(count_formatted));

                    if (return_quantity <= 0)
                    {
                        Error_operation = "Usuario não Encontrado no Banco de Dados";
                        return false;
                    }
                    else return true;
                }
                catch (Exception ex)
                {
                    Error_operation = "Não foi possivel Ler os dados do Banco de Dados." +
                        " Exceção: " + ex + "Erro:" + database.Error_operation;
                    return false;
                }
                finally
                {
                    closeItens();
                }
            }
        }

        // Retorna o Codigo do Usuario a partir do Email e Senha
        public int ReturnIdUser(string email, string password)
        {
            // Valida o Usuario e os Dados Utilizados
            string validationEmail = User.ValidationEmail(email);
            string validationPassword = User.ValidationPassword(password);
            if (validationEmail != User.OK)
            {
                Error_operation = validationEmail;
                return ERROR;
            }
            else if (validationPassword != User.OK)
            {
                Error_operation = validationPassword;
                return ERROR;
            }
            else if (!ExistsUser(email, password)) return NOT_FOUND;

            // Formação e Tratamento de Erros nos comandos SQL
            string sellectUser;
            try
            {
                sellectUser = string.Format("SELECT {0} FROM {1} WHERE {2}='{3}' AND {4}='{5}'",
                ID_USER, TABLE_USER, EMAIL, email, PASSWORD, password);
            }
            catch (ArgumentNullException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Parametros Invalidos. Exceção: " + ex;
                return ERROR;
            }
            catch (FormatException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Formação da Solicitação. Exceção: " + ex;
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

                try
                {
                    // Obtem o ID do Usuario do Banco de Dados
                    dataReader = database.readerTable(sellectUser);
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
            if (ExistsUser(user.Email, user.Password))
            {
                Error_operation = "Usuario já esta cadastrado no Banco de Dados";
                return false;
            }

            // Formação e Tratamento de Erros nos comandos SQL
            string commandSql;
            try
            {
                commandSql = string.Format("INSERT INTO {0}({1},{2},{3},{4},{5},{6}) " +
                "VALUE('{7}','{8}','{9}','{10}','{11}',{12})", TABLE_USER, NICKNAME, EMAIL,
                NAME, PASSWORD, IDIOM, THEME, user.Nickname, user.Email, user.Name,
                user.Password, user.Idioms, user.Theme_is_night);
            }
            catch (ArgumentNullException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Parametros Invalidos. Exceção: " + ex;
                return false;
            }
            catch (FormatException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Formação da Solicitação. Exceção: " + ex;
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
        public bool DeleteUser(User user)
        {
            // Valida o Codigo do Usuario
            if (user.Id <= 0)
            {
                Error_operation = string.Format("Codigo de Usuario ({0}) Invalido", user.Id);
                return false;
            }
            else if (ReturnIdUser(user.Email, user.Password) != user.Id)
            {
                Error_operation = "Codigo de Usuario não Correspondente ao " +
                    "Login. Tente Novamente";
                return false;
            }
            else if (!ExistsUser(user.Email, user.Password)) return false;

            // Formação e Tratamento de Erros nos comandos SQL
            string deleteSQL;
            try
            {
                deleteSQL = string.Format("DELETE FROM {0} WHERE {1}={2}",
                    TABLE_USER, ID_USER, user.Id);
            }
            catch (ArgumentNullException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Parametros Invalidos. Exceção: " + ex;
                return false;
            }
            catch (FormatException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Formação da Solicitação. Exceção: " + ex;
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
        // Atualiza somente Nome, Senha, Idioma e Tema
        public bool UpdateUser(User user)
        {
            // Valida o Usuario e o Codigo Passado
            string validationUser = User.ValidationUser(user);
            if (validationUser != User.OK)
            {
                Error_operation = validationUser;
                return false;
            }

            // Formação e Tratamento de Erros nos comandos SQL
            string updateSQL;
            try
            {
                updateSQL = string.Format("UPDATE {0} SET {1}='{2}', {3}='{4}', " +
                    "{5}='{6}', {7}={8} WHERE {9}={10}", TABLE_USER, PASSWORD,
                    user.Password, NAME, user.Name, IDIOM, user.Idioms, THEME,
                    user.Theme_is_night, ID_USER, user.Id);
            }
            catch (ArgumentNullException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Parametros Invalidos. Exceção: " + ex;
                return false;
            }
            catch (FormatException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Formação da Solicitação. Exceção: " + ex;
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

                // Caso altere o Usuario especificado ---> Retorna True
                if (database.executeCommand(updateSQL) != 1)
                {
                    if (string.IsNullOrEmpty(database.Error_operation))
                    {
                        Error_operation = "Houve um Problema no Banco de Dados. " +
                            "Houve mais de 1 Atualização de Registro";
                    }
                    else Error_operation = database.Error_operation;
                    return false;
                }
                else return true;
            }
        }

        // Atualiza o Nickname do Usuario
        public bool UpdateNickname(User user)
        {
            // Valida o Usuario e o Codigo Passado
            string validationNickname = User.ValidationNickname(user.Nickname);
            if (validationNickname != User.OK)
            {
                Error_operation = validationNickname;
                return false;
            }
            else if (ExistsNickname(user.Nickname))
            {
                Error_operation = "Nickname (Nome de Usuario) já existente " +
                    "no Banco de Dados. Nickname não podem ser iguais";
                return false;
            }
            else if (ReturnIdUser(user.Email, user.Password) != user.Id)
            {
                Error_operation = "Codigo de Usuario não Correspondente ao " +
                    "Login. Tente Novamente";
                return false;
            }
            else if (!ExistsUser(user.Email, user.Password)) return false;

            // Formação e Tratamento de Erros nos comandos SQL
            string updateSQL;
            try
            {
                updateSQL = string.Format("UPDATE {0} SET {1}='{2}' WHERE {3}={4}",
                    TABLE_USER, NICKNAME, user.Nickname, ID_USER, user.Id);
            }
            catch (ArgumentNullException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Parametros Invalidos. Exceção: " + ex;
                return false;
            }
            catch (FormatException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Formação da Solicitação. Exceção: " + ex;
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

                // Caso altere o Usuario especificado ---> Retorna True
                if (database.executeCommand(updateSQL) != 1)
                {
                    if (string.IsNullOrEmpty(database.Error_operation))
                    {
                        Error_operation = "Houve um Problema no Banco de Dados. " +
                            "Houve mais de 1 Atualização de Registro";
                    }
                    else
                    {
                        Error_operation = database.Error_operation;
                    }
                    return false;
                }
                else return true;
            }
        }

        // Atualiza o Email do Usuario
        public bool UpdateEmail(User user)
        {
            // Valida o Usuario e o Codigo Passado
            string validationEmail = User.ValidationEmail(user.Email);
            string validationPassword = User.ValidationPassword(user.Password);
            if (validationEmail != User.OK)
            {
                Error_operation = validationEmail;
                return false;
            }
            else if (validationPassword != User.OK)
            {
                Error_operation = validationPassword;
                return false;
            }
            else if (ExistsEmail(user.Email))
            {
                Error_operation = "Email já cadastrado no Banco de Dados. " +
                    " Não é possivel ter Emails iguais.";
                return false;
            }

            // Formação e Tratamento de Erros nos comandos SQL
            string checkPassword, updateSQL;
            try
            {
                checkPassword = string.Format("SELECT {0} FROM {1} WHERE {2}={3}",
                    PASSWORD, TABLE_USER, ID_USER, user.Id);
                updateSQL = string.Format("UPDATE {0} SET {1}='{2}' WHERE {3}={4}",
                    TABLE_USER, EMAIL, user.Email, ID_USER, user.Id);
            }
            catch (ArgumentNullException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Parametros Invalidos. Exceção: " + ex;
                return false;
            }
            catch (FormatException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Formação da Solicitação. Exceção: " + ex;
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

                dataReader = database.readerTable(checkPassword);
                if (dataReader == null) Error_operation = "Não foi Possivel Ler os Dados";

                try
                {
                    dataReader.Read();
                    string passwordDatabase = dataReader.GetString(dataReader.GetOrdinal(PASSWORD));

                    if (passwordDatabase != user.Password)
                    {
                        Error_operation = "Dados do usuario não Autenticados. Senha Invalida";
                        return false;
                    }
                }
                catch (Exception ex)
                {
                    Error_operation = "Houve um erro ao Ler os Dados. Exceção: " + ex;
                }
                finally
                {
                    closeItens();
                }

                // Caso altere o Usuario especificado ---> Retorna True
                if (database.executeCommand(updateSQL) != 1)
                {
                    if (string.IsNullOrEmpty(database.Error_operation))
                    {
                        Error_operation = "Houve um Problema no Banco de Dados. " +
                            "Houve mais de 1 Atualização de Registro";
                    }
                    else
                    {
                        Error_operation = database.Error_operation;
                    }
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

            // Formação e Tratamento de Erros nos comandos SQL
            string selectUser;
            try
            {
                selectUser = string.Format("SELECT {0},{1},{2},{3},{4} " +
                    "FROM {5} WHERE {6}={7}", EMAIL, NAME, NICKNAME, IDIOM, THEME,
                    TABLE_USER, ID_USER, id_user);
            }
            catch (ArgumentNullException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Parametros Invalidos. Exceção: " + ex;
                return null;
            }
            catch (FormatException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Formação da Solicitação. Exceção: " + ex;
                return null;
            }

            using (DatabaseHandler database = new DatabaseHandler())
            {
                // Valida se há Conexão Disponivel
                if (!database.Has_connectionAvailable)
                {
                    Error_operation = database.Error_operation;
                    return null;
                }

                // Realiza a Leitura do SELECT
                dataReader = database.readerTable(selectUser);
                if (dataReader == null)
                {
                    if (string.IsNullOrEmpty(database.Error_operation))
                        Error_operation = "Erro na Leitura do Banco de Dados";
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
            if (code_user <= 0)
            {
                Error_operation = "Codigo de Usuario Invalido.";
                return false;
            }

            // Formação e Tratamento de Erros nos comandos SQL
            string selectSQL;
            try
            {
                selectSQL = string.Format("SELECT {0} FROM {1} WHERE {0}={2}",
                    ID_USER, TABLE_USER, code_user);
            }
            catch (ArgumentNullException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Parametros Invalidos. Exceção: " + ex;
                return false;
            }
            catch (FormatException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Formação da Solicitação. Exceção: " + ex;
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

                dataReader = database.readerTable(selectSQL);
                if (dataReader == null)
                {
                    Error_operation = database.Error_operation;
                    return false;
                }

                // Lê a tabela e obtem o ID do registro com o ID passado
                dataReader.Read();
                int amount_register = dataReader.GetInt32(dataReader.GetOrdinal(ID_USER));

                if (amount_register <= 0)
                {
                    Error_operation = "Usuario não Cadastrado";
                    return false;
                }
                else return true;
            }
        }

        // Verifica se o Nickname já existe no Banco de Dados
        public bool ExistsNickname(string nickname)
        {

            // Formação e Tratamento de Erros nos comandos SQL
            string formattedCount, countNickname;
            try
            {
                formattedCount = string.Format("COUNT({0})", NICKNAME);
                countNickname = string.Format("SELECT {0} FROM {1} WHERE {2}='{3}'",
                    formattedCount, TABLE_USER, NICKNAME, nickname);
            }
            catch (ArgumentNullException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Parametros Invalidos. Exceção: " + ex;
                return false;
            }
            catch (FormatException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Formação da Solicitação. Exceção: " + ex;
                return false;
            }

            using (DatabaseHandler database = new DatabaseHandler())
            {
                dataReader = database.readerTable(countNickname);
                if (dataReader == null)
                {
                    Error_operation = database.Error_operation;
                    return false;
                }

                int quantityNickname;
                try
                {
                    // Executa a leitura dos dados
                    dataReader.Read();
                    quantityNickname = dataReader.GetInt32(dataReader.GetOrdinal(formattedCount));

                    if (quantityNickname >= 1) return true;
                    return false;
                }
                catch
                {
                    string errorDatabase = database.Error_operation;
                    if (string.IsNullOrEmpty(errorDatabase)) errorDatabase = "Leitura de Dados";

                    Error_operation = "Não foi possivel Ler os dados do Banco de Dados." +
                       "Erro:" + database.Error_operation;
                    return false;
                }
                finally
                {
                    closeItens();
                }
            }
        }

        // Verifica se o Email já existe no Banco de Dados
        public bool ExistsEmail(string email)
        {
            // Formação e Tratamento de Erros nos comandos SQL
            string formattedCount, countEmail;
            try
            {
                formattedCount = string.Format("COUNT({0})", EMAIL);
                countEmail = string.Format("SELECT {0} FROM {1} WHERE {2}='{3}'",
                    formattedCount, TABLE_USER, EMAIL, email);
            }
            catch (ArgumentNullException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Parametros Invalidos. Exceção: " + ex;
                return false;
            }
            catch (FormatException ex)
            {
                Error_operation = "Erro na Formação da Solicitação. " +
                    "Erro: Formação da Solicitação. Exceção: " + ex;
                return false;
            }

            using (DatabaseHandler database = new DatabaseHandler())
            {
                dataReader = database.readerTable(countEmail);
                if (dataReader == null)
                {
                    Error_operation = database.Error_operation;
                    return false;
                }

                int quantityEmail;

                try
                {
                    // Executa a leitura dos dados
                    dataReader.Read();
                    quantityEmail = dataReader.GetInt32(dataReader.GetOrdinal(formattedCount));

                    if (quantityEmail >= 1) return true;
                    return false;
                }
                catch
                {
                    string errorDatabase = database.Error_operation;
                    if (string.IsNullOrEmpty(errorDatabase)) errorDatabase = "Leitura de Dados";

                    Error_operation = "Não foi possivel Ler os dados do Banco de Dados." +
                       "Erro: " + database.Error_operation;
                    return false;
                }
                finally
                {
                    closeItens();
                }
            }
        }

        // todo: integrar com Update/Delete de Country
        public bool OnlyWithCountry(int id_country)
        {
            return false;
        }

        // todo: integrar com Update/Delete de StateCity
        public bool OnlyWithStateCity(int id_stateCity)
        {
            return false;
        }

        private void closeItens()
        {
            if (dataReader != null) dataReader.Close();
        }
    }
}
using MakeupApi.Models.ADO;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;

namespace MakeupApi.Models.DAO
{
    public class FavoriteDAO
    {
        public static string TABLE_FAVORITE = "favorites";
        public static string ID_FAVORITE = "id_favorite";
        public static string ID_USER = "id_user";
        public static string ID_MAKEUP = "id_makeup";

        public const int ERROR = -1;
        public const int NOT_FOUND = 0;

        public string Error_Operation { get; set; }

        // Retorna o ID de um Favorito atraves de uma Makeup e Email/Senha (User)
        // todo: implementar na classe UserDAO um metodo p/ retornar o ID pelo Nickname
        public int ReturnIdFavorite(Makeup makeup, User user)
        {
            string validationEmail = User.ValidationEmail(user.Email);
            string validationPassword = User.ValidationPassword(user.Password);

            // Valida os Campos
            if (!makeup.ValidationMakeup())
            {
                Error_Operation = makeup.Error_validation;
                return ERROR;
            } 
            else if(validationEmail != User.OK)
            {
                Error_Operation = validationEmail;
                return ERROR;
            }
            else if (validationPassword != User.OK)
            {
                Error_Operation = validationPassword;
                return ERROR;
            }

            // Obtem os IDs da Makeup e User
            MakeupDAO makeupDAO = new MakeupDAO();
            UserDAO userDAO = new UserDAO();
            makeup.Id_makeup = makeupDAO.ReturnIdMakeup(makeup);
            user.Id = userDAO.ReturnIdUser(user.Email, user.Password);

            // Valida os IDs Obtidos
            if (makeup.Id_makeup < 1)
            {
                Error_Operation = makeupDAO.Error_Operation;
                return ERROR;
            }
            else if (user.Id < 1)
            {
                Error_Operation = userDAO.Error_operation;
                return ERROR;
            }

            // Formata e Valida o Comando SQL
            string sql = "SELECT {0} FROM {1} WHERE {2}='{3}' AND {4}='{5}'";
            string[] parameters_sql = new string[]
            {
                ID_FAVORITE, TABLE_FAVORITE, ID_MAKEUP, makeup.Id_makeup.ToString(),
                ID_USER, user.Id.ToString()
            };

            string command = StringFormat(sql, parameters_sql);
            if (string.IsNullOrEmpty(command)) return ERROR;

            // Operações no banco de Dados
            using (DatabaseHandler database = new DatabaseHandler())
            {
                // Valida a Conexão e Tenta Obter o DataReader do command
                if (!database.Has_connectionAvailable)
                {
                    Error_Operation = database.Error_operation;
                    return ERROR;
                }
                
                MySqlDataReader dataReader = database.readerTable(command);

                if (dataReader == null)
                {
                    Error_Operation = database.Error_operation;
                    return NOT_FOUND;
                }

                // Tratamento das possiveis excções da leitura do DataReader
                try
                {
                    int id_favorite = NOT_FOUND;

                    dataReader.Read();
                    id_favorite = dataReader.GetInt32(dataReader.GetOrdinal(ID_FAVORITE));

                    if (id_favorite <= 0)
                        Error_Operation = "Favorito não Encontrado no Banco de Dados";

                    return id_favorite;
                }
                catch (Exception ex)
                {
                    Error_Operation = "Não foi possivel Ler os dados do Banco de Dados." +
                        " Exceção: " + ex + "Erro:" + database.Error_operation;
                    return ERROR;
                }
                finally
                {
                    if (dataReader != null) dataReader.Close();
                }
            }

        }

        // Insere um Favorito Atraves de uma Makeup e Email/Senha (User)
        public bool InsertFavorite(Makeup makeup, User user)
        {
            makeup.Id_makeup = ReturnIdFavorite(makeup, user);

            if(ReturnIdFavorite(makeup, user) > 0)
            {
                Error_Operation = "Favorito Já Cadastrado no Banco de Dados";
                return false;
            }

            // Obtem os IDs da Makeup e User
            MakeupDAO makeupDAO = new MakeupDAO();
            UserDAO userDAO = new UserDAO();
            makeup.Id_makeup = makeupDAO.IdMakeupValid(makeup);
            user.Id = userDAO.ReturnIdUser(user.Email, user.Password);

            // Valida os IDs Obtidos
            if (makeup.Id_makeup < 1)
            {
                Error_Operation = makeupDAO.Error_Operation;
                return false;
            }
            else if (user.Id < 1)
            {
                Error_Operation = userDAO.Error_operation;
                return false;
            }

            // Criação e Validação do Comando SQL
            string sql = "INSERT INTO {0}({1},{2}) VALUE ({3},{4})";
            string[] parameters_sql = new string[]
            {
                TABLE_FAVORITE, ID_USER, ID_MAKEUP, user.Id.ToString(), 
                makeup.Id_makeup.ToString()
            };

            string command = StringFormat(sql, parameters_sql);
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

        // Obtem uma Makeup a partir de um Id_favorite(Id_makeup + Email e Senha)
        public Makeup SelectFavorite(int id_favorite)
        {
            // Formação e Validação do SQL
            string sql = "SELECT {0} FROM {1} WHERE {2}={3}";
            string[] data_sql = new string[]
            {
                ID_MAKEUP, TABLE_FAVORITE, ID_FAVORITE, id_favorite.ToString()
            };

            string command = StringFormat(sql, data_sql);
            if (string.IsNullOrEmpty(command)) return null;

            // Operação e Validações com o Banco de Dados
            using (DatabaseHandler database = new DatabaseHandler())
            {
                // Obtem o DataReader se a conexão estiver disponivel
                if (!database.Has_connectionAvailable)
                {
                    Error_Operation = database.Error_operation;
                    return null;
                }

                MySqlDataReader dataReader = database.readerTable(command);

                if (dataReader == null)
                {
                    Error_Operation = database.Error_operation;
                    return null;
                }

                // Tratamento de Exceção da Leitura do DataReader
                try
                {
                    dataReader.Read();

                    Makeup makeup = new Makeup()
                    {
                        Id_makeup = dataReader.GetInt32(dataReader.GetOrdinal(ID_MAKEUP))
                    };

                    MakeupDAO makeupDAO = new MakeupDAO();
                    makeup = makeupDAO.SelectMakeup(makeup.Id_makeup);
                    
                    if(makeup == null)
                    {
                        Error_Operation = makeupDAO.Error_Operation;
                        return null;
                    }
                    else if (!makeup.ValidationMakeup())
                    {
                        Error_Operation = makeup.Error_validation;
                        return null;
                    }
                    else return makeup;
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
                    if (dataReader != null) dataReader.Close();
                }
            }
        }
        
        // Remove a Maquiagem ligada ao Usuario da tabela 'favorites'
        public bool DeleteFavorite(int id_favorite)
        {
            // Obtem a makeup do Id_favorite p/ Validar
            Makeup makeup = SelectFavorite(id_favorite);
            if (makeup == null) return false;

            // Exclui a Makeup se ela for a Unica usada na Tabela 'favorites'
            MakeupDAO makeupDAO = new MakeupDAO();
            if (!makeupDAO.DeleteOnlyMakeup(makeup)
                    && !string.IsNullOrEmpty(makeupDAO.Error_Operation))
            {
                Error_Operation = makeupDAO.Error_Operation;
                return false;
            }

            // Formação e Validação do Comando SQL
            string sql = "DELETE FROM {0} WHERE {1}={2}";
            string[] parameters_sql = new string[]
            {
                TABLE_FAVORITE, ID_FAVORITE, id_favorite.ToString()
            };

            string command = StringFormat(sql, parameters_sql);
            if (string.IsNullOrEmpty(command)) return false;

            using (DatabaseHandler database = new DatabaseHandler())
            {
                if (!database.Has_connectionAvailable)
                {
                    Error_Operation = database.Error_operation;
                    return false;
                }
                else if (database.executeCommand(command) <= 0)
                {
                    // Exclusão não foi bem Sucedida (Nenhum registro foi alterado)
                    Error_Operation = "Não Foi Possivel Excluir a Marca. "
                        + database.Error_operation;
                    return false;
                }
                else return true;
            }
        }

        // Retorna uma Lista de Maquiagens Favoritadas de um Usuario (Email+Senha)
        public List<Makeup> ListFavoritesUser(User user)
        {
            string validationEmail = User.ValidationEmail(user.Email);
            string validationPassword = User.ValidationPassword(user.Password);

            // Valida os Campos
            if (validationEmail != User.OK)
            {
                Error_Operation = validationEmail;
                return null;
            }
            else if (validationPassword != User.OK)
            {
                Error_Operation = validationPassword;
                return null;
            }

            // Obtem e Valida o ID do User
            UserDAO userDAO = new UserDAO();
            user.Id = userDAO.ReturnIdUser(user.Email, user.Password);
            if (user.Id < 1)
            {
                Error_Operation = userDAO.Error_operation;
                return null;
            }

            // Criação e Validação do Comando SQL
            string sql = "SELECT {0} FROM {1} WHERE {2}={3}";
            string[] parameters_sql = new string[]
            {
                ID_FAVORITE, TABLE_FAVORITE, ID_USER, user.Id.ToString()
            };

            string command = StringFormat(sql, parameters_sql);
            if (string.IsNullOrEmpty(command)) return null;

            // Operação e Validações com o Banco de Dados
            using (DatabaseHandler database = new DatabaseHandler())
            {
                // Obtem o DataReader se a conexão estiver disponivel
                if (!database.Has_connectionAvailable)
                {
                    Error_Operation = database.Error_operation;
                    return null;
                }
                
                MySqlDataReader dataReader = database.readerTable(command);
                if (dataReader == null)
                {
                    Error_Operation = database.Error_operation;
                    return null;
                }

                // Tratamento de Exceção da Leitura do DataReader
                try
                {
                    List<Makeup> listMakeupUser = new List<Makeup>();
                    int id_favorite = NOT_FOUND;

                    // Obtem os Dados do DataReder enquanto existirem
                    while (dataReader.Read())
                    {
                        // Obtem o Id_favorite com o Id_user informado/obtido
                        id_favorite = dataReader.GetInt32(dataReader.GetOrdinal(ID_FAVORITE));

                        // Serializa e Valida os Dados do Id_favorite
                        Makeup makeup = SelectFavorite(id_favorite);
                        if (makeup == null) return null;

                        listMakeupUser.Add(makeup);
                    }
                    
                    return listMakeupUser;
                }
                catch (IndexOutOfRangeException ex)
                {
                    Error_Operation = "Não foi possivel Obter os Dados.";
                    System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                    return null;
                }
                catch (Exception ex)
                {
                    Error_Operation = "Não foi possivel Listar os Favoritos.";
                    System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                    return null;
                }
                finally
                {
                    if (dataReader != null) dataReader.Close();
                }
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

    }
}
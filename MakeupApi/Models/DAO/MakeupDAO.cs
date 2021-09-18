using MakeupApi.Models.ADO;
using MySql.Data.MySqlClient;
using System;

namespace MakeupApi.Models.DAO
{
    public class MakeupDAO
    {

        // Get/Set da Mensagem de Erro em alguma Operação
        public string Error_Operation { get; set; }

        // Constantes do Banco de Dados
        public static string TABLE_MAKEUP = "makeup";
        public static string ID_MAKEUP = "id_makeup";
        public static string ID_BRAND = "id_brand";
        public static string ID_TYPE = "id_type";
        public static string NAME_MAKEUP = "name";

        private const string NAME_FOREIGN_ID = "id_makeup";

        MySqlDataReader dataReader;
        public const int ERROR = -1;
        public const int NOT_FOUND = 0;

        // Recebe uma Makeup Instanciada e retorna seu id_makeup
        public int ReturnIdMakeup(Makeup makeup)
        {
            // Valida os Campos
            if (!makeup.ValidationMakeup())
            {
                Error_Operation = makeup.Error_validation;
                return ERROR;
            }

            // Obtem os IDs da Marca e Tipo 
            BrandDAO brandDAO = new BrandDAO();
            TypeDAO typeDAO = new TypeDAO();
            makeup.Id_brand = brandDAO.ReturnIdBrand(makeup.Brand);
            makeup.Id_type = typeDAO.ReturnIdType(makeup.Type);

            // Valida os IDs Obtidos
            if (makeup.Id_brand < 1)
            {
                Error_Operation = brandDAO.Error_Operation;
                return ERROR;
            }
            else if (makeup.Id_type < 1)
            {
                Error_Operation = typeDAO.Error_Operation;
                return ERROR;
            }

            // Formata e Valida o Comando SQL
            string sql = "SELECT {0} FROM {1} WHERE {2}='{3}' AND {4}='{5}' AND {6}='{7}'";
            string[] parameters_sql = new string[]
            {
                ID_MAKEUP, TABLE_MAKEUP, ID_BRAND, makeup.Id_brand.ToString(),
                ID_TYPE, makeup.Id_type.ToString(), NAME_MAKEUP, makeup.Name
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
                else dataReader = database.readerTable(command);
                
                if (dataReader == null)
                {
                    Error_Operation = database.Error_operation;
                    return NOT_FOUND;
                }

                // Tratamento ~das possiveis excções da leitura do DataReader
                try
                {
                    makeup.Id_makeup = NOT_FOUND;
                    
                    dataReader.Read();
                    makeup.Id_makeup = dataReader.GetInt32(dataReader.GetOrdinal(ID_MAKEUP));

                    if (makeup.Id_makeup <= 0) 
                        Error_Operation = "Maquiagem não Encontrado no Banco de Dados";
                    
                    return makeup.Id_makeup;
                }
                catch (Exception ex)
                {
                    Error_Operation = "Não foi possivel Ler os dados do Banco de Dados." +
                        " Exceção: " + ex + "Erro:" + database.Error_operation;
                    return ERROR;
                }
                finally
                {
                    CloseItens();
                }
            }
        }

        // Recebe uma Makeup Instanciada e Insere no Banco de Dados
        public bool InsertMakeup(Makeup makeup)
        {
            if (!makeup.ValidationMakeup())
            {
                Error_Operation = makeup.Error_validation;
                return false;
            }
            else if (ReturnIdMakeup(makeup) > 0)
            {
                Error_Operation = "Maquiagem já Cadastrada no Banco de Dados";
                return false;
            }

            // Obtem os IDs da Marca e Tipo 
            BrandDAO brandDAO = new BrandDAO();
            TypeDAO typeDAO = new TypeDAO();
            makeup.Id_brand = brandDAO.IdBrandValid(makeup.Brand);
            makeup.Id_type = typeDAO.IdTypeValid(makeup.Type);

            // Valida os IDs Obtidos
            if (makeup.Id_brand < 1)
            {
                Error_Operation = brandDAO.Error_Operation;
                return false;
            }
            else if (makeup.Id_type < 1)
            {
                Error_Operation = typeDAO.Error_Operation;
                return false;
            }

            string sql = "INSERT INTO {0}({1},{2},{3}) VALUE ('{4}',{5},{6})";
            string[] parameters_sql = new string[]
            {
                TABLE_MAKEUP, NAME_MAKEUP, ID_BRAND, ID_TYPE, makeup.Name,
                makeup.Id_brand.ToString(), makeup.Id_type.ToString()
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

        // Obtem uma Makeup a partir de um Id_makeup
        public Makeup SelectMakeup(int id_makeup)
        {
            // Formação e Validação do SQL
            string sql = "SELECT {0}, {1}, {3} FROM {4} WHERE {5}={6}";
            string[] data_sql = new string[]
            {
                NAME_MAKEUP, ID_TYPE, ID_BRAND, TABLE_MAKEUP, ID_MAKEUP,
                id_makeup.ToString()
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
                    Makeup makeup = new Makeup()
                    {
                        Name = dataReader.GetString(dataReader.GetOrdinal(NAME_MAKEUP)),
                        Id_brand = dataReader.GetInt32(dataReader.GetOrdinal(ID_BRAND)),
                        Id_type = dataReader.GetInt32(dataReader.GetOrdinal(ID_TYPE))
                    };

                    if (makeup == null || makeup.Id_brand < 1 || makeup.Id_type < 1)
                    {
                        Error_Operation = "Não foi possivel Obter a Maquiagem";
                        return null;
                    }

                    // Obtem e Valida os Valores/Nome (string) da Marca e Tipo 
                    BrandDAO brandDAO = new BrandDAO();
                    TypeDAO typeDAO = new TypeDAO();
                    makeup.Brand = brandDAO.SelectBrand(makeup.Id_brand);
                    makeup.Type = typeDAO.SelectType(makeup.Id_type);

                    if (!makeup.ValidationMakeup())
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
                    CloseItens();
                }
            }
        }

        // Obtem o ID de uma Makeup Valido
        public int IdMakeupValid(Makeup makeup)
        {
            // Obtem o Id da Maquiagem
            makeup.Id_makeup = ReturnIdMakeup(makeup);

            // Erro na Validação ou Execução SQL
            if (makeup.Id_makeup == ERROR) return ERROR;
            else if (makeup.Id_makeup == NOT_FOUND)
            {
                // Caso não exista no Banco de Dados ---> Insere
                if (!InsertMakeup(makeup)) return ERROR;

                // Obtem o codigo do Registro Inserido
                makeup.Id_makeup = ReturnIdMakeup(makeup);
            }

            // Retorna um Erro ou o Codigo Valido
            return makeup.Id_makeup < 1 ? ERROR : makeup.Id_makeup;
        }

        // Verifica se o Favorite é a unica usando aquela Makeup
        public bool IsOnlyMakeup(Makeup makeup)
        {
            // Obtem e Valida o Nome,Marca e Tipo
            makeup.Id_makeup = ReturnIdMakeup(makeup);
            if (makeup.Id_makeup <= 0) return false;

            // Formação e Validação do Comando SQL
            string count_formmated = StringFormat("COUNT({0})", new string[] { NAME_FOREIGN_ID });
            string sql = "SELECT {0} FROM {1} WHERE {2}={3}";
            string[] parameters_sql = new string[]
            {
                count_formmated, FavoriteDAO.TABLE_FAVORITE, NAME_FOREIGN_ID, makeup.Id_makeup.ToString()
            };

            string command = StringFormat(sql, parameters_sql);
            if (string.IsNullOrEmpty(command) || string.IsNullOrEmpty(count_formmated)) return false;

            // Endereço existe no Banco de Dados
            using (DatabaseHandler database = new DatabaseHandler())
            {
                if (!database.Has_connectionAvailable)
                {
                    Error_Operation = database.Error_operation;
                    return false;
                }
                else dataReader = database.readerTable(command);

                if (dataReader == null)
                {
                    Error_Operation = database.Error_operation;
                    return false;
                }

                // Tratamento de possiveis Exceções da Leitura do DataReader
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
                    Error_Operation = "Não foi possivel obter Verificar a Marca.";
                    System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                    return false;
                }
                finally
                {
                    CloseItens();
                }
            }
        }

        // Exclui se a Makeup é a unica usando a Marca
        public bool DeleteOnlyMakeup(Makeup makeup)
        {
            // Verificar se o Usuario é o unico usando aquela Makeup
            if (IsOnlyMakeup(makeup))
            {
                // Obtem e Valida o ID da Makeup
                makeup.Id_makeup = ReturnIdMakeup(makeup);
                if (makeup.Id_makeup < 1) return false;

                // Verifica se é Preciso Excluir o Tipo e Marca
                // Só serão Excludios caso a makeup deletada seja a unica usando-os
                TypeDAO typeDAO = new TypeDAO();
                BrandDAO brandDAO = new BrandDAO();

                if (!typeDAO.DeleteOnlyType(makeup.Type)
                    && !string.IsNullOrEmpty(typeDAO.Error_Operation))
                {
                    Error_Operation = typeDAO.Error_Operation;
                    return false;
                }
                else if (!brandDAO.DeleteOnlyBrand(makeup.Brand)
                    && !string.IsNullOrEmpty(brandDAO.Error_Operation))
                {
                    Error_Operation = brandDAO.Error_Operation;
                    return false;
                }

                // Formação e Validação do Comando SQL
                string sql = "DELETE FROM {0} WHERE {1}={2}";
                string[] parameters_sql = new string[]
                {
                    TABLE_MAKEUP, ID_MAKEUP, makeup.Id_makeup.ToString()
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

        // Fecha os Itens se Necessario
        private void CloseItens()
        {
            if (dataReader != null) dataReader.Close();
        }

    }
}
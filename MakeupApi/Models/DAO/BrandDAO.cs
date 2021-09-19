using MakeupApi.Models.ADO;
using MySql.Data.MySqlClient;
using System;

namespace MakeupApi.Models.DAO
{
    public class BrandDAO
    {
        // Constantes dos Campos no Banco de Dados e Variaveis Usadas
        public static string TABLE_BRAND = "brand";
        public static string ID_BRAND = "id_brand";
        public static string NAME_BRAND = "name_brand";

        private const string NAME_FOREIGN_ID = "id_brand";

        MySqlDataReader dataReader;
        public const int ERROR = -1;
        public const int NOT_FOUND = 0;

        // Mensagem de Erro de alguma etapa nos metodos
        public string Error_Operation { get; set; }

        // Valida a Marca Passada
        public bool ValidationBrand(string brand)
        {
            if (string.IsNullOrEmpty(brand))
            {
                Error_Operation = "Campo 'Marca da Maquiagem' é Obrigatorio";
                return false;
            }
            else return true;
        }

        // Retorna o ID da marca por meio do seu nome
        public int ReturnIdBrand(string brand)
        {
            if (!ValidationBrand(brand)) return ERROR;
            
            // Criação e Validação do Comando SQL
            string sql = "SELECT {0} FROM {1} WHERE {2}='{3}'";
            string[] data_sql = new string[]
            {
                ID_BRAND, TABLE_BRAND, NAME_BRAND, brand
            };

            string command = StringFormat(sql, data_sql);
            if (string.IsNullOrEmpty(command)) return ERROR;

            // Operação no Banco de Dados
            using (DatabaseHandler database = new DatabaseHandler())
            {
                if (!database.Has_connectionAvailable)
                {
                    Error_Operation = database.Error_operation;
                    return ERROR;
                }

                // Obtem e Valida o DataReader
                dataReader = database.readerTable(command);
                if (dataReader == null)
                {
                    Error_Operation = database.Error_operation;
                    return NOT_FOUND;
                }

                // Tratamento de Exceções na Leitura do DataReader
                try
                {
                    int code = NOT_FOUND;
                    dataReader.Read();
                    code = dataReader.GetInt32(dataReader.GetOrdinal(ID_BRAND));

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
                    Error_Operation = "Não foi possivel obter o Codigo da Marca.";
                    System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                    return ERROR;
                }
                finally
                {
                    CloseItens();
                }
            }
        }

        // Insere a Marca no Banco de Dados
        public bool InsertBrand(string brand)
        {
            // Valida a marca passada
            if (!ValidationBrand(brand)) return false;
            else if (ReturnIdBrand(brand) > 0)
            {
                Error_Operation = "Marca Já Existente no Banco de Dados";
                return false;
            }

            // Criação e Validação do Comando SQL
            string sql = "INSERT INTO {0}({1}) VALUE('{2}')";
            string[] data_sql = new string[]
            {
                TABLE_BRAND, NAME_BRAND, brand
            };

            string command = StringFormat(sql, data_sql);
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
                    // Inserção no Banco de Dados não foi bem sucedida
                    Error_Operation = database.Error_operation;
                    return false;
                }
                else return true;
            }
        }
        
        // Obtem o nome da marca atraves do ID
        public string SelectBrand(int id_brand)
        {
            // Criação e Validação do Comando SQL
            string sql = "SELECT {0} FROM {1} WHERE {2}={3}";
            string[] data_sql = new string[]
            {
                NAME_BRAND, TABLE_BRAND, ID_BRAND ,id_brand.ToString()
            };

            string command = StringFormat(sql, data_sql);
            if (string.IsNullOrEmpty(command)) return null;

            using (DatabaseHandler database = new DatabaseHandler())
            {
                // Se houver Conexão com o Banco de Dados disponivel, obtem o DataReader
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

                // Tratamento de Exceções da Leitura do DataReader
                try
                {
                    dataReader.Read();

                    string name_type = string.Empty;
                    name_type = dataReader.GetString(dataReader.GetOrdinal(NAME_BRAND));

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
                    Error_Operation = "Não foi possivel Excluir a Marca.";
                    System.Diagnostics.Debug.WriteLine(Error_Operation + " Exceção: " + ex);
                    return null;
                }
                finally
                {
                    CloseItens();
                }
            }
        }

        // Obtem o ID valido atraves do nome da marca
        public int IdBrandValid(string brand)
        {
            // Obtem o Id da Marca
            int id_type = ReturnIdBrand(brand);

            if (id_type == ERROR)
            {
                // Erro na Validação ou Execução SQL
                return ERROR;
            }
            else if (id_type == NOT_FOUND)
            {
                // Caso não exista no Banco de Dados ---> Insere
                if (!InsertBrand(brand)) return ERROR;

                // Obtem o codigo do Registro Inserido
                id_type = ReturnIdBrand(brand);
            }

            // Retorna um Erro ou o Codigo Valido
            return id_type <= 0 ? ERROR : id_type;
        }

        // Verifica se a Makeup é a unica usando aquela Marca
        public bool IsOnlyBrand(string brand)
        {
            // Obtem e Valida o ID
            int id_brand = ReturnIdBrand(brand);
            if (id_brand <= 0) return false;

            // Formação e Validação do Comando SQL
            string count_formmated = StringFormat("COUNT({0})", new string[] { NAME_FOREIGN_ID });
            string sql = "SELECT {0} FROM {1} WHERE {2}={3}";
            string[] parameters_sql = new string[]
            {
                count_formmated, MakeupDAO.TABLE_MAKEUP, NAME_FOREIGN_ID, id_brand.ToString()
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
        public bool DeleteOnlyBrand(string brand)
        {
            // Verificar se o Usuario é o unico usando aquela marca 
            if (IsOnlyBrand(brand))
            {
                // Obtem e Valida o ID da marca
                int id_type = ReturnIdBrand(brand);
                if (id_type < 1) return false;

                // Formação e Validação do Comando SQL
                string sql = "DELETE FROM {0} WHERE {1}={2}";
                string[] parameters_sql = new string[]
                {
                    TABLE_BRAND, ID_BRAND, id_type.ToString()
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

        // Formata e Trata as Possiveis Exceções do string.Format
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

        // Fecha os Itens necessarios
        private void CloseItens()
        {
            if (dataReader != null) dataReader.Close();
        }
    }
}
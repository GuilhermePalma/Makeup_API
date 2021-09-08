namespace MakeupApi.Models
{
    public class User
    {
        public static string OK = "Campo Validado";
        public static string INPUT_NULL = "Campo Obrigatorio.";
        public static string INPUT_MIN_LENGTH = "%1$s deve ter no Minimo %2$s Caracteres";
        public static string INPUT_MAX_LENGTH = "%1$s deve ter no Maximo %2$s Caracteres";
        public static string INPUT_NOT_FORMAT = "%1$s deve ter conter apenas %2$s";

        private int id;
        private string name;
        private string nickname;
        private string email;
        private string password;
        private bool theme_is_night;
        private string idioms;

        // Contrutor Vazio
        public User() { }

        // Contrutor do Login
        public User(string email, string password)
        {
            this.email = email;
            this.password = password;
        }

        public static string ValidationUser(User user)
        {
            if (user == null) return INPUT_NULL;

            string status_name = ValidationName(user.Name);
            string status_nickname = ValidationNickname(user.Nickname);
            string status_email = ValidationEmail(user.Email);
            string status_password = ValidationPassword(user.Password);

            if (status_name != OK)
            {
                return status_name;
            }
            else if (status_nickname != OK)
            {
                return status_nickname;
            }
            else if (status_email != OK)
            {
                return status_email;
            }
            else if (status_password != OK)
            {
                return status_password;
            }
            else if (!ValidationIdioms(user.Idioms))
            {
                return "Idioma não Cadastrado";
            }
            else return OK;
        }

        public static string ValidationName(string name)
        {
            if (string.IsNullOrEmpty(name) || string.IsNullOrWhiteSpace(name))
            {
                return INPUT_NULL;
            }
            else if (name.Length < 3)
            {
                return string.Format(INPUT_MIN_LENGTH, "Nome", 3);
            }
            else if (name.Length > 120)
            {
                return string.Format(INPUT_MAX_LENGTH, "Nome", 120);
            }
            else if (!name.Contains("^[A-ZÀ-úà-úa-zçÇ\\s]*"))
            {
                return string.Format(INPUT_NOT_FORMAT, "Nome", "Letras");
            }
            else return OK;
        }

        public static string ValidationNickname(string nickname) {
            if (string.IsNullOrEmpty(nickname) || string.IsNullOrWhiteSpace(nickname))
            {
                return INPUT_NULL;
            }
            else if (nickname.Length < 3)
            {
                return string.Format(INPUT_MIN_LENGTH, "Nome de Usuario", 3);
            }
            else if (nickname.Length > 40)
            {
                return string.Format(INPUT_MAX_LENGTH, "Nome de Usuario", 40);
            }
            else if (!nickname.Contains("^[A-Za-z._\\s]*"))
            {
                return string.Format(INPUT_NOT_FORMAT, "Nome de Usuario", 
                    "Letras, Pontos ou Hifen (Sem Espaço em Branco)");
            }
            else return OK;
        }

        public static string ValidationEmail(string email)
        {
            if (string.IsNullOrEmpty(email) || string.IsNullOrWhiteSpace(email))
            {
                return INPUT_NULL;
            }
            else if (email.Length < 8)
            {
                return string.Format(INPUT_MIN_LENGTH, "Email", 8);
            }
            else if (email.Length > 150)
            {
                return string.Format(INPUT_MAX_LENGTH, "Email", 150);
            }
            else if (!email.Contains("^[A-Za-z\\S\\d._\\-@#]*"))
            {
                return string.Format(INPUT_NOT_FORMAT, "Email",
                    "Letras, Numeros, e alguns caracteres (Hifen, " +
                    "Ponto, Underline, # e @)");
            }
            else return OK;
        }

        public static string ValidationPassword(string password)
        {
            if (string.IsNullOrEmpty(password) || string.IsNullOrWhiteSpace(password))
            {
                return INPUT_NULL;
            }
            else if (password.Length < 3)
            {
                return string.Format(INPUT_MIN_LENGTH, "Senha", 3);
            }
            else if (password.Length > 40)
            {
                return string.Format(INPUT_MAX_LENGTH, "Senha", 40);
            }
            else if (!password.Contains("^[A-Za-z\\S\\d´`^~.,_\\-?@!*&+=#/|]*"))
            {
                return string.Format(INPUT_NOT_FORMAT, "Senha", 
                    "Letras, Numeros, Acentos e Caracteres Especiais Validos");
            }
            else return OK;
        }

        public static bool ValidationIdioms(string idioms)
        {
            if (string.IsNullOrEmpty(idioms)) return false;

            // Obtem um Array dos Idiomas Validos
            string[] idiomsValid = ListIdioms();
            foreach (string itemArray in idiomsValid)
            {
                if (idioms == itemArray) return true;
            }

            return false;
        }
        
        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Nickname { get => nickname; set => nickname = value; }
        public string Email { get => email; set => email = value; }
        public string Password { get => password; set => password = value; }
        public bool Theme_is_night { get => theme_is_night; set => theme_is_night = value; }
        public string Idioms { get => idioms; set => idioms = value; }

        private static string[] ListIdioms()
        {
            string[] idioms = new string[4];
            idioms[0] = "Portugues";
            idioms[1] = "Ingles";
            idioms[2] = "Espanhol";
            idioms[3] = "Frances";

            return idioms;
        }

    }
}
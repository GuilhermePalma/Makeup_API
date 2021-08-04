using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;

namespace MakeupApi.Models
{
    public class User
    {

        private int id;
        private string name;
        private string last_name;
        private string nickname;
        private string login;
        private string password;
        private bool isChangedLogin;


        // Contrutor Vazio
        public User() { }

        // Contrutor para o Login
        public User(string login, string password)
        {
            this.Login = login;
            this.Password = password;
        }

        // Contrutor para o Cadastro
        public User(string name, string last_name, string nickname, string login, string password)
        { 
            this.Name = name;
            this.Last_name = last_name;
            this.Nickname = nickname;
            this.Login = login;
            this.Password = password;
        }

        public int Id { get => id; set => id = value; }
        public string Name { get => name; set => name = value; }
        public string Last_name { get => last_name; set => last_name = value; }
        public string Nickname { get => nickname; set => nickname = value; }
        public string Login { get => login; set => login = value; }
        public string Password { get => password; set => password = value; }
        public bool IsChangedLogin { get => isChangedLogin; set => isChangedLogin = value; }

    }
}
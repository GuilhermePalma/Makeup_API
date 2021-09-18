namespace MakeupApi.Models
{
    public class Makeup
    {
        private string ERRO_INPUT = "Campo {0} é Obrigatorio";

        private string name;
        private string type;
        private string brand;
        private int id_makeup;
        private int id_type;
        private int id_brand;

        public Makeup() { }

        public Makeup(string name, string brand, string type)
        {
            Name = name;
            Brand = brand;
            Type = type;
        }

        public string Name { get => name; set => name = value; }
        public string Type { get => type; set => type = value; }
        public string Brand { get => brand; set => brand = value; }
        public int Id_makeup { get => id_makeup; set => id_makeup = value; }
        public int Id_type { get => id_type; set => id_type = value; }
        public int Id_brand { get => id_brand; set => id_brand = value; }
        public string Error_validation { get; set; }

        public bool ValidationMakeup()
        {
            if (!ValidationName() || !ValidationBrand() || !ValidationType())
                return false;
            else 
                return true;
        }

        public bool ValidationName()
        {
            if (string.IsNullOrEmpty(name)) {
                Error_validation = string.Format(ERRO_INPUT, "Nome");
                return false;
            } 
            else return true;
        }

        public bool ValidationBrand()
        {
            if (string.IsNullOrEmpty(name))
            {
                Error_validation = string.Format(ERRO_INPUT, "Marca");
                return false;
            }
            else return true;
        }

        public bool ValidationType()
        {
            if (string.IsNullOrEmpty(name))
            {
                Error_validation = string.Format(ERRO_INPUT, "Tipo");
                return false;
            }
            else return true;
        }

    }
}
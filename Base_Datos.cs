using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows.Forms;

namespace KineticsBD
{
    [Serializable()]
    public class Base_Datos{
         bool interno;
        string Depurador;

        public List<Molecule> Moleculas;
         public Base_Datos(String Fuente, bool interno) {
            Depurador  = "oK";
            this.interno = interno;
             if (interno) {
                using (StreamReader reader = new StreamReader(Path.Combine(Application.StartupPath, "Data/Database.json")))
                {
                    string json = reader.ReadToEnd();
                    Moleculas = JsonConvert.DeserializeObject<List<Molecule>>(json);
                    foreach (Molecule a in Moleculas)
                    {
                        Depurador += a;  
                    }
                    Depurador += json;
                }
            }
            else {

                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(Fuente + "/getMolecules");
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                using (Stream stream = response.GetResponseStream())
                using (StreamReader reader = new StreamReader(stream)) {
                    var json = reader.ReadToEnd();
                    Moleculas = JsonConvert.DeserializeObject<List<Molecule>>(json);
                }
                foreach (Molecule a in Moleculas) {
                    int id = a.ID;
                    request = (HttpWebRequest)WebRequest.Create(Fuente + "/KOverals/" + id);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream)) {
                        var json = reader.ReadToEnd();
                        a.K_Ovs = JsonConvert.DeserializeObject<List<Kinetic_constant>>(json);
                    }
                    request = (HttpWebRequest)WebRequest.Create(Fuente + "/PK_S/" + id);
                    using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                    using (Stream stream = response.GetResponseStream())
                    using (StreamReader reader = new StreamReader(stream)) {
                        var json = reader.ReadToEnd();
                        a.pKs = JsonConvert.DeserializeObject<List<Dissociaton_Constants>>(json);
                    }
                }
                Depurador += " ";

                String Valores = JsonConvert.SerializeObject(Moleculas);
                System.IO.Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Data"));
                System.IO.File.WriteAllText(Path.Combine(Application.StartupPath, "Data\\Database.json"), Valores);
                WebClient client = new WebClient();
                foreach (Molecule a in Moleculas) {
                    Depurador += a;
                    System.IO.Directory.CreateDirectory(Path.Combine(Application.StartupPath, "Data\\images\\" + a.ID));
                    client.DownloadFile(Fuente + "/files/data-base-img/"+a.ID+"/"+a.Imagen, Path.Combine(Application.StartupPath, "Data\\images\\"+a.ID+"\\" + a.Imagen));
                }
            }

        }
        public string getDep() {
            return Depurador;

        }
        
    }
    [Serializable()]
    public class Molecule
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string RIS { get; set; }
        public string SMILE { get; set; }
        public string Imagen { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public string updated_at { get; set; }
        public string created_at { get; set; }

  
        public override string ToString() {
            String regresar = "ID " + ID + " Name "+Name + "\r\n Kinetic";
            if(K_Ovs != null)
            foreach (Kinetic_constant A in K_Ovs)
                regresar += A + " \r\n";
             
            regresar += " pK ";
            if(pKs != null)
            foreach (Dissociaton_Constants A in pKs) 
                regresar += A + " \r\n"; 
            return regresar; 
        }
        public List<Kinetic_constant> K_Ovs { get; set; }
        public List<Dissociaton_Constants> pKs { get; set; }
    }
    [Serializable()]
    public class Kinetic_constant {
        public int ID_K_OVERALL { get; set; }
        public int ID_Molecule { get; set; }
        public int radical { get; set; }
        public int Solvent { get; set; }
        public float Valor { get; set; }
        public string pH { get; set; }
        public string Tipo { get; set; }
        public object Descripcion { get; set; }
 
        public object id_reference { get; set; }
        public object updated_at { get; set; }
        public object created_at { get; set; }
        public int ID { get; set; }
        public string Name { get; set; }
        public string RIS { get; set; }
        public string SMILE { get; set; }
        public string Imagen { get; set; }
        public string Description { get; set; }
        public string Active { get; set; }
        public int ID_Radical { get; set; }
        public string Name_Radical { get; set; }
        public int ID_Solvent { get; set; }
        public string Name_Solvent { get; set; }
        public object Reference { get; set; }
        public object Coments { get; set; }

        public override string ToString()
        {
            String regresar = "Value: "+Valor+" "+Coments+" \r\n";
            return regresar;
        }


    }
    public class Dissociaton_Constants
    {
        public int id_pks { get; set; }
        public int ID { get; set; }
        public string Site { get; set; }
        public string Tipo_Exp_te{ get; set; }
        public double Value { get; set; }
 
        public object id_reference { get; set; }
        public object Description { get; set; }
        public object created_at { get; set; }
        public object updated_at { get; set; }
        public string Name { get; set; }
        public string RIS { get; set; }
        public string Imagen { get; set; }
        public object Reference { get; set; }
        public object Coments { get; set; }
        public override string ToString()
        {
            String regresar = "Value: " + Value + " " + Coments + " \r\n";
            return regresar;
        }
    }

}
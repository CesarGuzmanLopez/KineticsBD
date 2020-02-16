using System;
using System.Drawing;
using System.IO; 
using System.Windows.Forms;

namespace KineticsBD
{
    public partial class prin : Form
    {
        String Page = "http://chemistry.agalano.com/";
        Base_Datos Completa;
        public prin()
        {
            
            InitializeComponent();
           
            if (!Directory.Exists(Application.StartupPath + "/Data"))
            {
                DialogResult dialogResult = MessageBox.Show("Download Data Base", "Database not found", MessageBoxButtons.YesNo);
                if (dialogResult == DialogResult.Yes)    
                    Completa = new Base_Datos(Page, false);
                else
                    Application.Exit();
            }
            else
                Completa = new Base_Datos(Page, true);
            Data_Mole.AutoGenerateColumns = true;
            Data_Mole.DataSource = Completa.Moleculas;
        }
        private void Prin_Load(object sender, EventArgs e)
        {
            Kinetics_grid.Columns["radicalDataGridViewTextBoxColumn"].DisplayIndex = 0;
        }


        private void Update_Click(object sender, EventArgs e)
        {
            Image_Box.Image = null;
            Completa = new Base_Datos(Page, false);
        }

        private void Data_Mole_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex >= 0)
            {
                llenaGraficas(e.RowIndex);
            }
           
        }
        void llenaGraficas(int select)
        { 
            DataGridViewRow row = this.Data_Mole.Rows[select];
            String Nombr_mol = row.Cells["nameDataGridViewTextBoxColumn"].Value.ToString();
            int ID = int.Parse(row.Cells["iDDataGridViewTextBoxColumn"].Value.ToString());
            if (row.Cells["imagenDataGridViewTextBoxColumn"] != null)
                Image_Box.Image = Image.FromFile(Application.StartupPath + "/Data/images/" + ID+ "/" + row.Cells["imagenDataGridViewTextBoxColumn"].Value.ToString());
            Label_mol.Text = "Molecule: " + Nombr_mol;
            TextBox_Details.Text = (row.Cells["descriptionDataGridViewTextBoxColumn"].Value != null)
                                    ? row.Cells["descriptionDataGridViewTextBoxColumn"].Value.ToString().Replace("\n", "\r\n") : "";
            TextBox_RIS.Text = (row.Cells["risDataGridViewTextBoxColumn"].Value != null)
                                       ? row.Cells["risDataGridViewTextBoxColumn"].Value.ToString().Replace("\n", "\r\n") : "";
            foreach (Molecule a in Completa.Moleculas)
                if (Nombr_mol == a.Name && ID == a.ID)
                {
                    Dissociation_grid.DataSource = a.pKs;
                    Kinetics_grid.DataSource = a.K_Ovs;
                    if (a.K_Ovs.Count > 0 && a.pKs.Count > 0)
                    {
                        button1.Visible = true;
                        button2.Visible = true;
                        button1.Enabled = true;
                        button2.Enabled = false;
                        Dissociation_grid.Visible = false;
                        Kinetics_grid.Visible = true;
                         
                    }
                    else if (a.K_Ovs.Count <= 0 && a.pKs.Count > 0)
                    {
                        button1.Visible = true;
                        button2.Visible = false;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        Dissociation_grid.Visible = true;
                        Kinetics_grid.Visible = false;
                    }
                    else if (a.K_Ovs.Count > 0 && a.pKs.Count <= 0)
                    {
                        button1.Visible = false;
                        button2.Visible = true;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        Dissociation_grid.Visible = false;
                        Kinetics_grid.Visible = true;

                    }
                    else {
                        button1.Visible = false;
                        button2.Visible = false;
                        button1.Enabled = false;
                        button2.Enabled = false;
                        Dissociation_grid.Visible = false;
                        Kinetics_grid.Visible = false;

                    }
                    break;
                } 
        }  
        private void button1_Click_1(object sender, EventArgs e)
        { 
                button1.Enabled = false;
                button2.Enabled = true;
                Dissociation_grid.Visible = true;
                Kinetics_grid.Visible = false; 
        }

        private void button2_Click(object sender, EventArgs e)
        { 
                button1.Enabled = true;
                button2.Enabled = false;
                Dissociation_grid.Visible = false;
                Kinetics_grid.Visible = true;
        }

        private void eToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }
    }
}

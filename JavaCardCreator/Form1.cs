using System;
using System.IO;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using JavaCardCreator.BO;

namespace JavaCardCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            lblTeste.Text = " ";
            string RID = edtRID.Text;
            string ApplicationCode = edtApplicationCode.Text;
            string ContryCode = edtContryCode.Text;
            string IndustryCode = edtIndustryCode.Text;
            string OpecacionalArea = edtOpecacionalArea.Text;
            string TAR = edtTAR.Text;
            string ApplicationIdArea = cbxApplicationIdArea.Text;
            int idArea = 0;
            string AppID;
            int version = (int)nbrVersion.Value;
            string hexValue = version.ToString("X");
            int idIgual = 0;
            
            
            
            switch (ApplicationIdArea)
            {
                case "Operator Services":
                    idArea = 1;
                    break;
                case "Mobile Banking":
                    idArea = 2;
                    break;
                case "Entretainment":
                    idArea = 3;
                    break;
                case "Prepaid Services":
                    idArea = 4;
                    break;
                default:
                    idArea = 0;
                    break;
            }
            if ((RID.Length != 10) ||
                (ApplicationCode.Length != 4) ||
                (ContryCode.Length != 4) ||
                (IndustryCode.Length != 2) ||
                (OpecacionalArea.Length != 2) ||
                (TAR.Length != 6) ||
                (idArea == 0)||
                (NameCap.Text=="")||
                (NameCap.Text==null) || 
                (edtCapContryCode.Text.Length != 2) || 
                (edtCapCustumerId.Text.Length <2) || 
                (edtCapAppletOriginator.Text.Length <1) || 
                (edtCapAppletName.Text.Length < 1) ||
                (edtCapMinorVersion.Text.Length < 1)
                )
            {
                MessageBox.Show("Some ID OR .Cap Name parameters was not configured", "Missing parameter",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Asterisk);
            }
            else
            {
                string pacoteID = RID + ApplicationCode + ContryCode + OpecacionalArea + idArea + hexValue + IndustryCode + TAR + "00";
                AppID = RID + ApplicationCode + ContryCode + OpecacionalArea + idArea + hexValue + IndustryCode + TAR + "01";
                AppletID.Text = AppID;
                PackageID.Text = pacoteID;

                try
                {
                   string[] lines = System.IO.File.ReadAllLines(SourcePath.Text + @"\Config.txt");
          
                    foreach (string line in lines)
                    {
                        if (line == AppletID.Text)
                        {
                            string message = "This Id alredy exist. so you wana to procege?";
                            string caption = "Error Detected in Input";
                            MessageBoxButtons buttons = MessageBoxButtons.YesNo;
                            DialogResult result;

                            // Displays the MessageBox.

                            result = MessageBox.Show(message, caption, buttons);

                            if (result == System.Windows.Forms.DialogResult.Yes)
                            {
                                idIgual = 1;
                            }
                            else if (result == System.Windows.Forms.DialogResult.No)
                            {
                                idIgual = 2;
                            }
                        }
                    }
                }
                catch (Exception)
                {

                    lblStatus.Text = "wrong path";
                }
                try
                {
                    if (idIgual!=2)
                    {
                        ArquivosBo bo = new ArquivosBo
                        {
                            path = ProjectPath.Text,
                            SourcePath = SourcePath.Text,
                            NameCap = NameCap.Text,
                            AppletID = AppletID.Text
                        };
                        bo.CreateFolders();
                        bo.Copyfolders();
                        bo.createCompile();
                        bo.CriateConvert();
                        bo.CriateProd();
                        bo.CriateDebug();
                        lblStatus.Text = "Sucesso";
                        if (idIgual != 1)
                        {
                            using (System.IO.StreamWriter file =
                            new System.IO.StreamWriter(SourcePath.Text + @"\Config.txt", true))
                            {
                                file.WriteLine(AppletID.Text);
                            }
                        }
                        
                    }
                    
                }
                catch (Exception)
                {
                    lblStatus.Text = "Falhou";
                }
            }
        }

        private void cap_CheckedChanged(object sender, EventArgs e)
        {
            GeraNome();
        }

        public void PopulaID(object sender, EventArgs e) {
            lblTeste.Text = " ";
            string RID = edtRID.Text;
            string ApplicationCode = edtApplicationCode.Text;
            string ContryCode = edtContryCode.Text;
            string IndustryCode = edtIndustryCode.Text;
            string OpecacionalArea = edtOpecacionalArea.Text;
            string TAR = edtTAR.Text;
            string ApplicationIdArea = cbxApplicationIdArea.Text;
            int idArea = 0;
            string AppID;
            int version = (int)nbrVersion.Value;
            string hexValue = version.ToString("X");
            
            switch (ApplicationIdArea)
            {
                case "Operator Services":
                    idArea = 1;
                    break;
                case "Mobile Banking":
                    idArea = 2;
                    break;
                case "Entretainment":
                    idArea = 3;
                    break;
                case "Prepaid Services":
                    idArea = 4;
                    break;
                default:
                    idArea = 0;
                    break;
            }
            string pacoteID = RID + ApplicationCode + ContryCode + OpecacionalArea + idArea + hexValue + IndustryCode + TAR + "00";
            AppID = RID + ApplicationCode + ContryCode + OpecacionalArea + idArea + hexValue + IndustryCode + TAR + "01";
            AppletID.Text = AppID;
            PackageID.Text = pacoteID;
            if ((RID.Length != 10) ||
                (ApplicationCode.Length != 4) ||
                (ContryCode.Length != 4) ||
                (IndustryCode.Length != 2) ||
                (OpecacionalArea.Length != 2) ||
                (TAR.Length != 6) ||
                (idArea == 0))
            {
                lblIDStatus.Text = "ID Invalid";
            }
            else
            {
                if (onlyHexa())
                {
                   lblIDStatus.Text = "ID Valid";
                }
                
            }
            lblTAR.Text = ArquivosBo.HEX2ASCII(edtTAR.Text);
        }

        public void GeraNome() {
            string LL = edtCapContryCode.Text.ToUpper();
            string cccc = edtCapCustumerId.Text.ToUpper();
            string oo = edtCapAppletOriginator.Text.ToUpper();
            string nnnnnnnnnn = edtCapAppletName.Text;
            string xxx = edtCapMinorVersion.Text.ToUpper();
            
            string aux = "";
            if (cccc.Length<4)
            {
                for (int i = 0; i < (4- cccc.Length ); i++)
                {
                    aux += "_";
                }
            }
                NameCap.Text = "C" + LL + cccc +aux+ "_A" + oo + nnnnnnnnnn + "V" + xxx;
            if((edtCapContryCode.Text.Length != 2) ||
               (edtCapCustumerId.Text.Length < 2) ||
               (edtCapAppletOriginator.Text.Length < 1) ||
               (edtCapAppletName.Text.Length < 1) ||
               (edtCapMinorVersion.Text.Length < 1))
            {
                lblNameStatus.Text = "Name Invalid";
            }
            else
            {
                lblNameStatus.Text = "Name Valid";
            }
        }

        private void btnGetPath_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();
            using (FileDialog fileDialog = new OpenFileDialog())
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    ProjectPath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        private void btnGetPathSource_Click(object sender, EventArgs e)
        {
            var folderBrowserDialog1 = new FolderBrowserDialog();
            using (FileDialog fileDialog = new OpenFileDialog())
            {
                if (folderBrowserDialog1.ShowDialog() == DialogResult.OK)
                {
                    SourcePath.Text = folderBrowserDialog1.SelectedPath;
                }
            }
        }

        public bool onlyHexa() {
            try
            {
                int testeHexa = int.Parse(edtOpecacionalArea.Text, System.Globalization.NumberStyles.HexNumber);
                testeHexa = int.Parse(edtIndustryCode.Text, System.Globalization.NumberStyles.HexNumber);
                testeHexa = int.Parse(edtTAR.Text, System.Globalization.NumberStyles.HexNumber);
                testeHexa = int.Parse(edtContryCode.Text, System.Globalization.NumberStyles.HexNumber);
                lblTAR.Text = ArquivosBo.HEX2ASCII(edtTAR.Text);
                return true;
            }
            catch (Exception)
            {
                lblIDStatus.Text = "ID Fora do padrao hexa";
                return false;
            }
        }

        private void groupBox1_Enter(object sender, EventArgs e)
        {
            edtTAR.Text= ArquivosBo.ASCIITOHex(lblTAR.Text);
        }

        /*erro*/
        private void panel3_Paint(object sender, PaintEventArgs e)
        {

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        

        /*/erro*/
    }
}

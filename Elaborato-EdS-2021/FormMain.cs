using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Elaborato_EdS_2021
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
        }

        FormConvertImg form_img;
        FormTesto form_txt;
        
        private void btn_img_Click(object sender, EventArgs e)
        {
            
            form_img = new FormConvertImg();
            form_img.ShowDialog();

        }

        private void FormMain_Load(object sender, EventArgs e)
        {
            Text = "Menù Iniziale";
        }

        private void Testobutton_Click(object sender, EventArgs e)
        {
            form_txt = new FormTesto();
            form_txt.ShowDialog();
        }
    }
}

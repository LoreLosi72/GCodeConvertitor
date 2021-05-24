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
        
        private void btn_img_Click(object sender, EventArgs e)
        {
            form_img = new FormConvertImg();
            form_img.ShowDialog();
        }
    }
}

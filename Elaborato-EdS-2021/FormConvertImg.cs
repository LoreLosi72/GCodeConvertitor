using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Elaborato_EdS_2021
{
    public partial class FormConvertImg : Form
    {
        Bitmap ImgOrig;
        Bitmap ImgRev;
        float UltimateValue;
        float Ratio; //proporzione dell'immagine, rapporto d'aspetto (tra altezza e larghezza). Esempio: 3:4, 16:9, 32:54. In questo caso lo utilizziamo per bloccare il valore ratio quando l'opzione è selezionata.
        public FormConvertImg()
        {
            InitializeComponent();
        }

        private void SaveSettings() //salviamo le impostazioni, all'avvio verrano presi i valori iniziali, quando l'utente modificherà i valori saranno modificate le impostazioni
        {
            try
            {
                string set;
                Properties.Settings.Default.autoZoom = VisualZoomtoolStripMenuItem1.Checked; //zoom immagine
                Properties.Settings.Default.width = LarghezzatextBox.Text; //larghezza immagine
                Properties.Settings.Default.height = AltezzatextBox2.Text; //altezza immagine
                Properties.Settings.Default.risoluzione = RisoluzionetextBox3.Text; //risoluzione immagine
                Properties.Settings.Default.minPower = MinPowertextBox.Text; //min power profilo laser
                Properties.Settings.Default.maxPower = MaxPowertextBox.Text; //max power profilo laser
                Properties.Settings.Default.feedRate = FeedRatetextBox.Text; //velocità di avanzamento 
                if (ZradioButton2.Checked) set = "Z";
                else set = "S";
                Properties.Settings.Default.powercmd = set; //comando power Z o S
                Properties.Settings.Default.modello = IncisionecomboBox.Text; //modalità di incisione: orizzontale o diagonale
                Properties.Settings.Default.bordoLinea = IncisioneBordocheckBox1.Text; //incisione anche del bordo oppuro no

                Properties.Settings.Default.Save(); //mantiene le modifiche tra le varie sessioni dell'applicazione.
            
            }catch (Exception e)
            {
                MessageBox.Show("Si è verificato un errore nel salvataggio:\n" + e.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

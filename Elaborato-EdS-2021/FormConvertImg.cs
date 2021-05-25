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
                //Properties.Settings.Default.autoZoom = VisualZoomtoolStripMenuItem1.Checked; //zoom immagine
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
                Properties.Settings.Default.incisioneBordo = IncisioneBordocheckBox1.Checked;

                Properties.Settings.Default.Save(); //mantiene le modifiche tra le varie sessioni dell'applicazione.
            
            }catch (Exception e)
            {
                MessageBox.Show("Si è verificato un errore nel salvataggio:\n" + e.Message, "error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void LoadSettings() //carica le impostazioni iniziali predefinite
        {
            try
            {
                //VisualZoomtoolStripMenuItem1.Checked = (bool)Properties.Settings.Default.autoZoom;
                VisualZoomtoolStripMenuItem1_Click(this, null);
                LarghezzatextBox.Text = Properties.Settings.Default.width;
                AltezzatextBox2.Text = Properties.Settings.Default.height;
                RisoluzionetextBox3.Text = Properties.Settings.Default.risoluzione;
                MinPowertextBox.Text = Properties.Settings.Default.minPower;
                MaxPowertextBox.Text = Properties.Settings.Default.maxPower;
                FeedRatetextBox.Text = Properties.Settings.Default.feedRate;
                if (Properties.Settings.Default.powercmd == "Z")
                    ZradioButton2.Checked = true;
                else SradioButton1.Checked = true;
                IncisionecomboBox.Text = Properties.Settings.Default.modello;
                IncisioneBordocheckBox1.Checked = Properties.Settings.Default.incisioneBordo;

            } catch(Exception e){

                MessageBox.Show("Si è verificato un errore nel salvataggio: " + e.Message, "errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VisualZoomtoolStripMenuItem1_Click(object sender, EventArgs e) //Auto zoom dell'immagine all'avvio del programma. L'auto zoom dell'impostazione visualizza è predefinito poi è possibile disattivarlo
        {
            if (VisualZoomtoolStripMenuItem1.Checked)
            {
                ImgpictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                ImgpictureBox1.Width = panel1.Width;
                ImgpictureBox1.Height = panel1.Height;
                ImgpictureBox1.Top = 0;
                ImgpictureBox1.Left = 0;
            }
        }

        private void FormConvertImg_Load(object sender, EventArgs e) //operazioni di partenza al caricamento del form
        {
            Text = "Immagine in GCODE per stampante 3D";
            StatotoolStripStatusLabel.Text = "Pronto";
            //LoadSettings();
            VisualZoomtoolStripMenuItem1_Click(this, null); //settaggio iniziale dello zoom dell'immagine.
        }

        private Int32 InterpolazioneImg(Int32 grigio, Int32 min, Int32 max) //interpolazione dell'immagine con scala di grigio (0,255) con valori tra minimo e massimo. Interpolazione dell'immagine significa andare a modificare artificialmente il numero dei pixel e quindi la risoluzioni dell'immagine (diminuzione o aumento dei pixel).
        {
            Int32 differenza = max - min;
            Int32 interpolazione = (min + ((grigio * differenza) / 255));
            return interpolazione;
        }

        private Bitmap ScalaGrigiImg(Bitmap img) // funzione che ritorna una versione a scala di grigi dell'immagine inserita
        {
            StatotoolStripStatusLabel.Text = "Applicazione scala di grigi all'immagine....";
            Refresh();
            Bitmap newimg = new Bitmap(img.Width, img.Height); //creazione di un'immagine vuota con le stesse dimensioni dell'immagine originale
            Graphics g = Graphics.FromImage(newimg); //prendo l'oggetto graphics (che ci fornisce metodi per disegnare oggetti da visualizzare) dalla nuova immagine
            ColorMatrix colorMatrix = new ColorMatrix( //creazione della colorMatrix in scala di grigi. Definisce una matrice 5x5.
                new float[][]
                {
                    new float[] {.299f, .299f, .299f, 0, 0},
                    new float[] {.587f, .587f, .587f, 0, 0},
                    new float[] {.114f, .114f, .114f, 0, 0},
                    new float[] {0, 0, 0, 1, 0},
                    new float[] {0, 0, 0, 0, 1}
                });
            ImageAttributes attributi = new ImageAttributes(); //Creazione di un ImageAttributes oggetto e passo il ColorMatrix al SetColorMatrix metodo dell' ImageAttributes oggetto. Imageattributes contiene una serie di informazioni relative alla manipolazione dei colori delle immagini
            attributi.SetColorMatrix(colorMatrix); //passo la matrice di grigi all'imageattributes
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 0, 0, img.Width, img.Height, GraphicsUnit.Pixel, attributi); //disegno l'immagine originale inserita nella nuova immagine utilizzando la matrice scala di grigi.
            g.Dispose();
            StatotoolStripStatusLabel.Text = "Immagine pronta";
            Refresh();
            return newimg;
        }

        private Bitmap ResizeImg(Bitmap imgInput, Int32 xSize, Int32 ySize) //funzione che ridimensiona l'immagine con altezza e larghezza desiderati
        {
            Bitmap imgOutput;
            StatotoolStripStatusLabel.Text = "Ridimensionamento dell'immagine....";
            Refresh();
            imgOutput = new Bitmap(imgInput, new Size(xSize, ySize));
            StatotoolStripStatusLabel.Text = "Immagine pronta";
            Refresh();
            return imgOutput;
        }

        private Bitmap BalanceImg(Bitmap img, int lum, int contr, int gamm)//funzione che sistema l'immagine attraverso i parametri di luminosità, contrasto e gamma
        {
            StatotoolStripStatusLabel.Text = "Bilanciamento dell'immagine....";
            Refresh();
            ImageAttributes attributiImg;
            float luminosità = (lum / 100.0f) + 1.0f;
            float contrasto = (contr / 100.0f) + 1.0f;
            float gamma = 1 / (gamm / 100.0f);
            float luminositàAgg = luminosità - 1.0f;
            Bitmap imgOutput;
            float[][] Array = //creazione della matrice per la luminosità e contrasto dell'immagine
            {
                new float[] {contrasto, 0, 0, 0, 0}, // scala rosso
                new float[] {0, contrasto, 0, 0, 0}, // scala verde
                new float[] {0, 0, contrasto, 0, 0}, // scala blu
                new float[] {0, 0, 0, 1.0f, 0}, //
                new float[] {luminositàAgg, luminositàAgg, luminositàAgg, 0, 1}
            };
            imgOutput = new Bitmap(img);
            attributiImg = new ImageAttributes();
            attributiImg.ClearColorMatrix(); //cancella la matrice di regolazione del colore
            attributiImg.SetColorMatrix(new ColorMatrix(Array), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);//passo la matrice creata prima
            attributiImg.SetGamma(gamma, ColorAdjustType.Bitmap); //imposta valore di gamma per una categoria specifica
            Graphics g = Graphics.FromImage(imgOutput); //oggetto graphics dall'immagine di output (bilanciata)
            g.DrawImage(imgOutput, new Rectangle(0, 0, imgOutput.Width, imgOutput.Height), 0, 0, imgOutput.Width, imgOutput.Height, GraphicsUnit.Pixel, attributiImg);
            StatotoolStripStatusLabel.Text = "Immagine Pronta";
            Refresh();
            return imgOutput;
        }
        private void userAdjust() //funzione che viene chiamata quando l'utente inserisce in input qualche valore per aggiustare l'immagine
        {
            try
            {
                if (ImgRev == null) return; //se non c'è l'immagine la funzione non fa niente.
                Int32 xSize; // X pixels totali dell'immagine risultanto che serviranno per la generazione GCODE
                Int32 ySize; // Y pixels totali dell'immagine risultanto che serviranno per la generazione GCODE
                xSize = Convert.ToInt32(float.Parse(LarghezzatextBox.Text, CultureInfo.InvariantCulture.NumberFormat) / float.Parse(RisoluzionetextBox3.Text, CultureInfo.InvariantCulture.NumberFormat)); //invariantculture: operazione eseguita indipendentemente dal setting dell'user local che utilizza il programma
                ySize = Convert.ToInt32(float.Parse(AltezzatextBox2.Text, CultureInfo.InvariantCulture.NumberFormat) / float.Parse(RisoluzionetextBox3.Text, CultureInfo.InvariantCulture.NumberFormat));
                ImgRev = ResizeImg(ImgOrig, xSize, ySize); //applicazione del ridimensionamento all'immagine
                ImgRev = BalanceImg(ImgRev, LuminositàTrackBar.Value, ContrastoTrackBar.Value, GammaTrackBar.Value); //applicazione del bilanciamento tramite parametri (lum,contrasto,gamma) inseriti dall'utente. Applicazione all'immagine ridimensionata
                ImgpictureBox1.Image = ImgRev; //display dell'immagine "aggiustata"
                Refresh();
                VisualZoomtoolStripMenuItem1_Click(this, null); //set zoom
            }
            catch (Exception e)
            {
                MessageBox.Show("Errore nel ridimensionamento/bilanciamento dell'immagine: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void VisualOriginalebutton_MouseUp(object sender, MouseEventArgs e) //rivisualizza l'immagine corrente modificata
        {
            if (ImgRev == null) return; //se non c'è l'immagine la funzione non fa niente
            if (!File.Exists(openFileDialog1.FileName)) return;
            ImgpictureBox1.Image = ImgRev; //immagine revisionata, modificata dall'utente attraverso ridimensionamento e bilanciamneto
        }

        private void VisualOriginalebutton_MouseDown(object sender, MouseEventArgs e) //preview dell'immagine originale inserita
        {
            if (ImgRev == null) return; //se non c'è l'immagine, la funzione non fa niente
            if (!File.Exists(openFileDialog1.FileName)) return;
            StatotoolStripStatusLabel.Text = "Caricamento Immagine originale....";
            Refresh();
            ImgpictureBox1.Image = ImgOrig; //immagine originale
            StatotoolStripStatusLabel.Text = "Immagine Pronta";
        }

        private bool checkDigitFloat(char ch) //funzione che controlla se il carattere inserito è un float valido, altrimenti mostra messaggio di errore.
        {
            if ((ch != '.') & (ch != '0') & (ch != '1') & (ch != '2') & (ch != '3') & (ch != '4') & (ch != '5') & (ch != '6') & (ch != '7') & (ch != '8') & (ch != '9') & (Convert.ToByte(ch) != 8) & (Convert.ToByte(ch) != 13))
            {
                MessageBox.Show("Parametri ammessi da 0-9 e il punto (.) è il separatore decimale", "valore non valido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private bool checkDigitInteger(char ch) //funzione che controlla se il carattere inserito è un int valido, altrimenti mostra messaggio di errore
        {
            if ((ch != '0') & (ch != '1') & (ch != '2') & (ch != '3') & (ch != '4') & (ch != '5') & (ch != '6') & (ch != '7') & (ch != '8') & (ch != '9') & (Convert.ToByte(ch) != 8) & (Convert.ToByte(ch) != 13))
            {
                MessageBox.Show("Parametri ammessi da 0-9'", "valore non valido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }

        private void InvalidValue() //messaggio quando i valori inseriti non sono validi
        {
            StatotoolStripStatusLabel.Text = "Valore non valido! Controlla";
        }

        private void LuminositàTrackBar_Scroll(object sender, EventArgs e) //luminosità aggiustata dall'utente
        {
            LumtextBox.Text = Convert.ToString(LuminositàTrackBar.Value);
            Refresh();
            userAdjust();
        }

        private void ContrastoTrackBar_Scroll(object sender, EventArgs e) //contrasto aggiustato dall'utente
        {
            ContrtextBox.Text = Convert.ToString(ContrastoTrackBar.Value);
            Refresh();
            userAdjust();
        }

        private void GammaTrackBar_Scroll(object sender, EventArgs e) //gamma aggiustato dall'utente
        {
            GammatextBox.Text = Convert.ToString(GammaTrackBar.Value);
            Refresh();
            userAdjust();
        }
    }
}

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
        
        public FormConvertImg()
        {
            InitializeComponent();
        }

        float Ratio; //proporzione dell'immagine, rapporto d'aspetto (tra altezza e larghezza). Esempio: 3:4, 16:9, 32:54. In questo caso lo utilizziamo per bloccare il valore ratio quando l'opzione è selezionata.
        //FormMain form_main;
        private void SaveSettings() //salviamo le impostazioni, all'avvio verrano presi i valori iniziali, quando l'utente modificherà i valori saranno modificate le impostazioni
        {
            try
            {
                //string set;
                //Properties.Settings.Default.autoZoom = VisualZoomtoolStripMenuItem1.Checked; //zoom immagine
                //Properties.Settings.Default.width = LarghezzatextBox.Text; //larghezza immagine
                //Properties.Settings.Default.height = AltezzatextBox2.Text; //altezza immagine
                //Properties.Settings.Default.risoluzione = RisoluzionetextBox3.Text; //risoluzione immagine
                //Properties.Settings.Default.minPower = MinPowertextBox.Text; //min power profilo laser
                //Properties.Settings.Default.maxPower = MaxPowertextBox.Text; //max power profilo laser
                //Properties.Settings.Default.feedRate = FeedRatetextBox.Text; //velocità di avanzamento 
                /*if (ZradioButton2.Checked) set = "Z";
                else set = "S";*/
                //Properties.Settings.Default.powercmd = set; //comando power Z o S
                //Properties.Settings.Default.modello = IncisionecomboBox.Text; //modalità di incisione: orizzontale o diagonale
                //Properties.Settings.Default.incisioneBordo = IncisioneBordocheckBox1.Checked;

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
                //VisualZoomtoolStripMenuItem1.Checked = Properties.Settings.Default.autoZoom;
                //VisualZoomtoolStripMenuItem1_Click(this, null);
                //LarghezzatextBox.Text = Properties.Settings.Default.width;
                //AltezzatextBox2.Text = Properties.Settings.Default.height;
                //RisoluzionetextBox3.Text = Properties.Settings.Default.risoluzione;
                //MinPowertextBox.Text = Properties.Settings.Default.minPower;
                //MaxPowertextBox.Text = Properties.Settings.Default.maxPower;
                //FeedRatetextBox.Text = Properties.Settings.Default.feedRate;
                /*if (Properties.Settings.Default.powercmd == "Z")
                    ZradioButton2.Checked = true;
                else SradioButton1.Checked = true;*/
                //IncisionecomboBox.Text = Properties.Settings.Default.modello;
                //IncisioneBordocheckBox1.Checked = Properties.Settings.Default.incisioneBordo;

            } catch(Exception e){

                MessageBox.Show("Si è verificato un errore nel salvataggio: " + e.Message, "errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //AUTO-ZOOM IMMAGINE OK
    
        /*private void AutoZoomtoolStripMenuItem1_Click(object sender, EventArgs e) //Auto zoom dell'immagine all'avvio del programma. L'auto zoom dell'impostazione visualizza è predefinito poi è possibile disattivarlo
        {
            if (AutoZoomtoolStripMenuItem2.Checked)
            {
                ImgpictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                ImgpictureBox1.Width = panel1.Width;
                ImgpictureBox1.Height = panel1.Height;
                ImgpictureBox1.Top = 0;
                ImgpictureBox1.Left = 0;
            }
            else
            {
                ImgpictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
                if (ImgpictureBox1.Width > panel1.Width) ImgpictureBox1.Left = 0; else ImgpictureBox1.Left = (panel1.Width / 2) - (ImgpictureBox1.Width / 2);
                if (ImgpictureBox1.Height > panel1.Height) ImgpictureBox1.Top = 0; else ImgpictureBox1.Top = (panel1.Height / 2) - (ImgpictureBox1.Height / 2);
            }
        }*/

        private void FormConvertImg_Load(object sender, EventArgs e) //operazioni di partenza al caricamento del form
        {
            Text = "Immagine in GCODE per stampante 3D";
            StatotoolStripStatusLabel.Text = "Pronto";
            //LoadSettings();
            //AutoZoomtoolStripMenuItem1_Click(this, null); //settaggio iniziale dello zoom dell'immagine.
        }

        //INTERPOLAZIONE OK
        private Int32 InterpolazioneImg(Int32 grigio, Int32 min, Int32 max) //interpolazione dell'immagine con scala di grigio (0,255) con valori tra minimo e massimo. Interpolazione dell'immagine significa andare a modificare artificialmente il numero dei pixel e quindi la risoluzioni dell'immagine (diminuzione o aumento dei pixel).
        {
            Int32 dif = max - min;
            return (min + ((grigio * dif) / 255));
        }

        //SCALA DI GRIGI OK
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

            ImageAttributes attributes = new ImageAttributes(); //Creazione di un ImageAttributes oggetto e passo il ColorMatrix al SetColorMatrix metodo dell' ImageAttributes oggetto. Imageattributes contiene una serie di informazioni relative alla manipolazione dei colori delle immagini
            attributes.SetColorMatrix(colorMatrix); //passo la matrice di grigi all'imageattributes
            g.DrawImage(img, new Rectangle(0, 0, img.Width, img.Height), 
            0, 0, img.Width, img.Height, 
            GraphicsUnit.Pixel, attributes); //disegno l'immagine originale inserita nella nuova immagine utilizzando la matrice scala di grigi.
            
            g.Dispose();
            StatotoolStripStatusLabel.Text = "Immagine pronta";
            Refresh();
            return (newimg);
        }

        //RIDIMENSIONAMENTO IMMAGINE OK
        private Bitmap ResizeImg(Bitmap imgInput, Int32 xSize, Int32 ySize) //funzione che ridimensiona l'immagine con altezza e larghezza desiderati
        {
            Bitmap imgOutput;
            StatotoolStripStatusLabel.Text = "Ridimensionamento dell'immagine....";
            Refresh();
            imgOutput = new Bitmap(imgInput, new Size(xSize, ySize));
            StatotoolStripStatusLabel.Text = "Immagine pronta";
            Refresh();
            return (imgOutput);
        }

        //BILANCIAMENTO IMMAGINE CON LUMINOSITA, CONTRASTO, GAMMA OK
        private Bitmap BalanceImg(Bitmap img, int lum, int contr, int gamm)//funzione che sistema l'immagine attraverso i parametri di luminosità, contrasto e gamma
        {
            StatotoolStripStatusLabel.Text = "Bilanciamento dell'immagine....";
            Refresh();
            ImageAttributes attributiImg;
            float luminosit = (lum / 100.0f) + 1.0f;
            float contrasto = (contr / 100.0f) + 1.0f;
            float gamma = 1/(gamm / 100.0f);
            float luminositAgg = luminosit - 1.0f;
            Bitmap imgOutput;
            float[][] Array ={ //creazione della matrice per la luminosità e contrasto dell'immagine
            new float[] {contrasto, 0, 0, 0, 0}, // scala rosso
            new float[] {0, contrasto, 0, 0, 0}, // scala verde
            new float[] {0, 0, contrasto, 0, 0}, // scala blu
            new float[] {0, 0, 0, 1.0f, 0}, //
            new float[] {luminositAgg, luminositAgg, luminositAgg, 0, 1}};

            imgOutput = new Bitmap(img);
            attributiImg = new ImageAttributes();
            attributiImg.ClearColorMatrix(); //cancella la matrice di regolazione del colore
            attributiImg.SetColorMatrix(new ColorMatrix(Array), ColorMatrixFlag.Default, ColorAdjustType.Bitmap);//passo la matrice creata prima
            attributiImg.SetGamma(gamma, ColorAdjustType.Bitmap); //imposta valore di gamma per una categoria specifica
            
            Graphics g = Graphics.FromImage(imgOutput); //oggetto graphics dall'immagine di output (bilanciata)
            g.DrawImage(imgOutput, new Rectangle(0, 0, imgOutput.Width, imgOutput.Height)
            , 0, 0, imgOutput.Width, imgOutput.Height, 
            GraphicsUnit.Pixel, attributiImg);

            StatotoolStripStatusLabel.Text = "Immagine Pronta";
            Refresh();
            return imgOutput;
        }
        //MODIFICHE UTENTE OK
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
                //AutoZoomtoolStripMenuItem1_Click(this, null); //set zoom
            }
            catch (Exception e)
            {
                MessageBox.Show("Errore nel ridimensionamento/bilanciamento dell'immagine: " + e.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //OK
        /*private void VisualOriginalebutton_MouseUp(object sender, MouseEventArgs e) //rivisualizza l'immagine corrente modificata
        {
            if (ImgRev == null) return; //se non c'è l'immagine la funzione non fa niente
            if (!File.Exists(openFileDialog1.FileName)) return;
            ImgpictureBox1.Image = ImgRev; //immagine revisionata, modificata dall'utente attraverso ridimensionamento e bilanciamneto
        }*/

        //OK
        /*private void VisualOriginalebutton_MouseDown(object sender, MouseEventArgs e) //preview dell'immagine originale inserita
        {
            if (ImgRev == null) return; //se non c'è l'immagine, la funzione non fa niente
            if (!File.Exists(openFileDialog1.FileName)) return;
            StatotoolStripStatusLabel.Text = "Caricamento Immagine originale....";
            Refresh();
            ImgpictureBox1.Image = ImgOrig; //immagine originale
            StatotoolStripStatusLabel.Text = "Immagine Pronta";
        }*/

        //OK
        private bool checkDigitFloat(char ch) //funzione che controlla se il carattere inserito è un float valido, altrimenti mostra messaggio di errore.
        {
            if ((ch != '.') & (ch != '0') & (ch != '1') & (ch != '2') & (ch != '3') & (ch != '4') & (ch != '5') & (ch != '6') & (ch != '7') & (ch != '8') & (ch != '9') & (Convert.ToByte(ch) != 8) & (Convert.ToByte(ch) != 13))
            {
                MessageBox.Show("Parametri ammessi da 0-9 e il punto (.) è il separatore decimale", "valore non valido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        //OK
        private bool checkDigitInteger(char ch) //funzione che controlla se il carattere inserito è un int valido, altrimenti mostra messaggio di errore
        {
            if ((ch != '0') & (ch != '1') & (ch != '2') & (ch != '3') & (ch != '4') & (ch != '5') & (ch != '6') & (ch != '7') & (ch != '8') & (ch != '9') & (Convert.ToByte(ch) != 8) & (Convert.ToByte(ch) != 13))
            {
                MessageBox.Show("Parametri ammessi da 0-9'", "valore non valido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            return true;
        }
        //OK
        private void InvalidValue() //messaggio quando i valori inseriti non sono validi
        {
            StatotoolStripStatusLabel.Text = "Valore non valido! Controlla";
        }

        //CONVERSIONE VALORE IN STRINGA LUMINOSITA TRACKBAR OK
        private void LuminositàTrackBar_Scroll(object sender, EventArgs e) //luminosità aggiustata dall'utente
        {
            LumtextBox.Text = Convert.ToString(LuminositàTrackBar.Value);
            Refresh();
            userAdjust();
        }

        //CONVERSIONE VALORE IN STRINGA CONTRASTO TRACKBAR OK
        private void ContrastoTrackBar_Scroll(object sender, EventArgs e) //contrasto aggiustato dall'utente
        {
            ContrtextBox.Text = Convert.ToString(ContrastoTrackBar.Value);
            Refresh();
            userAdjust();
        }

        //CONVERSIONE VALORE IN STRINGA GAMMA TRACKBAR OK
        private void GammaTrackBar_Scroll(object sender, EventArgs e) //gamma aggiustato dall'utente
        {
            GammatextBox.Text = Convert.ToString(GammaTrackBar.Value/100.0f);
            Refresh();
            userAdjust();
        }

        //LARGHEZZA CAMBIATA DALL'UTENTE OK
        private void WidthChanged() // controlla se la nuova larghezza è stata confermata dall'utente e la processa
        {
            try
            {
                if (ImgRev == null) return; //se non c'è l'immagine la funzione non fa niente
                float newValue = Convert.ToSingle(LarghezzatextBox.Text, CultureInfo.InvariantCulture.NumberFormat); //prende la larghezza inserita dall'utente in unput
                if (newValue == UltimateValue) return; //se la larghezza in input è uguale alla larghezza corrente dell'immagine, la funzione non fa niente
                UltimateValue = Convert.ToSingle(LarghezzatextBox.Text, CultureInfo.InvariantCulture.NumberFormat);
                if (RatiocheckBox.Checked)
                {
                    AltezzatextBox2.Text = Convert.ToString((newValue / Ratio), CultureInfo.InvariantCulture.NumberFormat);
                }
                userAdjust();
            }
            catch
            {
                MessageBox.Show("Controlla la larghezza inserita", "valore non valido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //ALTEZZA CAMBIATA DALL'UTENTE OK
        private void HeightChanged() // controlla se la nuova altezza è stata confermata dall'utente e la processa
        {
            try
            {
                if (ImgRev == null) return; //se non c'è l'immagine la funzione non fa niente
                float newValue = Convert.ToSingle(AltezzatextBox2, CultureInfo.InvariantCulture.NumberFormat); //prende l'altezza inserita dall'utente in unput
                if (newValue == UltimateValue) return; //se l'altezza in input è uguale all'altezza corrente dell'immagine, la funzione non fa niente
                UltimateValue = Convert.ToSingle(AltezzatextBox2, CultureInfo.InvariantCulture.NumberFormat);
                if (RatiocheckBox.Checked)
                {
                    LarghezzatextBox.Text = Convert.ToString((newValue * Ratio), CultureInfo.InvariantCulture.NumberFormat);
                }
                userAdjust();
            }
            catch
            {
                MessageBox.Show("Controlla l'altezza inserita", "valore non valido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //RISOLUZIONE CAMBIATA DALL'UTENTE OK
        private void ResolutionChanged() // controlla se la nuova risoluzione è stata confermata dall'utente e la processa
        {
            try
            {
                if (ImgRev == null) return; //se non c'è l'immagine la funzione non fa niente
                float newValue = Convert.ToSingle(RisoluzionetextBox3.Text, CultureInfo.InvariantCulture.NumberFormat); //prende la risoluzione inserita dall'utente in unput
                if (newValue == UltimateValue) return; //se la risoluzione in input è uguale alla risoluzione corrente dell'immagine, la funzione non fa niente
                UltimateValue = Convert.ToSingle(RisoluzionetextBox3.Text, CultureInfo.InvariantCulture.NumberFormat);
           
                userAdjust();
            }
            catch
            {
                MessageBox.Show("Controlla Risoluzione inserita", "valore non valido", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //ASPECT RATIO CHECKED OK
        private void RatiocheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (RatiocheckBox.Checked)
            {
                AltezzatextBox2.Text = Convert.ToString((Convert.ToSingle(LarghezzatextBox.Text, CultureInfo.InvariantCulture.NumberFormat) / Ratio),CultureInfo.InvariantCulture.NumberFormat); //inizializzazione dimensione Y
                if (ImgRev == null) return; //se non c'è l'immagine non fa niente
                userAdjust();
            }
        }

        //LARGHEZZA KEY PRESS OK
        private void LarghezzatextBox_KeyPress(object sender, KeyPressEventArgs e) //Si verifica quando si preme un tasto carattere, la barra spaziatrice o il tasto backspace mentre il controllo ha lo stato attivo.
        {
            if (!checkDigitFloat(e.KeyChar)) //controlla il carattere inserito attraverso la funzione di controllo per i float creata in precedenza
            {
                e.Handled = true; //ferma il carattere inserito perchè non valido
                return;
            }

            if (e.KeyChar == Convert.ToChar(13))
            {
                WidthChanged();
            }
        }

        //ALTEZZA KEY PRESS OK
        private void AltezzatextBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!checkDigitFloat(e.KeyChar)) //controlla il carattere inserito attraverso la funzione di controllo per i float creata in precedenza
            {
                e.Handled = true; //ferma il carattere inserito perchè non valido
                return;
            }

            if (e.KeyChar == Convert.ToChar(13))
            {
                HeightChanged();
            }
        }

        //RISOLUZIONE KEY PRESS OK
        private void RisoluzionetextBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!checkDigitFloat(e.KeyChar)) //controlla il carattere inserito attraverso la funzione di controllo per i float creata in precedenza
            {
                e.Handled = true; //ferma il carattere inserito perchè non valido
                return;
            }

            if (e.KeyChar == Convert.ToChar(13))
            {
                ResolutionChanged();
            }
        }
        //OK
        private void LarghezzatextBox_Leave(object sender, EventArgs e) //si verifica quando lo stato attivo esce dall'area di controllo
        {
            WidthChanged();
        }
        //OK
        private void AltezzatextBox2_Leave(object sender, EventArgs e) 
        {
            HeightChanged();
        }
        //OK
        private void RisoluzionetextBox3_Leave(object sender, EventArgs e) //risoluzione
        {
            ResolutionChanged(); 
        }

        //OK
        private void LarghezzatextBox_Enter(object sender, EventArgs e) //si verifica quando si entra nel'area di controllo
        {
            try
            {
                UltimateValue = Convert.ToSingle(LarghezzatextBox.Text, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch { }
        }
        //OK
        private void AltezzatextBox2_Enter(object sender, EventArgs e)
        {
            try
            {
                UltimateValue = Convert.ToSingle(AltezzatextBox2.Text, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch { }
        }

        //OK
        private void RisoluzionetextBox3_Enter(object sender, EventArgs e)
        {
            try
            {
                UltimateValue = Convert.ToSingle(RisoluzionetextBox3.Text, CultureInfo.InvariantCulture.NumberFormat);
            }
            catch { }
        }

        //APERTURA FILE IMMAGINE OK
        private void FileApritoolStripMenuItem_Click(object sender, EventArgs e) // Apre il fle (immagine), salva l'immagine con la scala di grigi dall'immagine originale e salva il ratio originale in Ratio
        {
            try
            {
                if (openFileDialog1.ShowDialog() == DialogResult.Cancel) return; //se l'immagine non viene inserita la funzione non fa niente
                if (!File.Exists(openFileDialog1.FileName)) return;
                StatotoolStripStatusLabel.Text = "Apertura del file in corso....";
                Refresh();
                LuminositàTrackBar.Value = 0;
                ContrastoTrackBar.Value = 0;
                GammaTrackBar.Value = 100;
                LumtextBox.Text = Convert.ToString(LuminositàTrackBar.Value);
                ContrtextBox.Text = Convert.ToString(ContrastoTrackBar.Value);
                GammatextBox.Text = Convert.ToString(GammaTrackBar.Value / 100.0f);
                ImgOrig = new Bitmap(Image.FromFile(openFileDialog1.FileName)); //l'immagine originale diventa l'immagine appena inserita dall'utente
                ImgOrig = ScalaGrigiImg(ImgOrig); //l'immagine originale la trasformo in un'immagine basata sulla scala di grigi attraverso la funzione con la matrice creata in precedenza
                ImgRev = new Bitmap(ImgOrig); //l'immagine originale modifica diventa la nuova immagine revisionata
                Ratio = (ImgOrig.Width + 0.0f) / ImgOrig.Height; //salva il ratio dell'immagine così è pronto se ne avremmo bisogno
                if (RatiocheckBox.Checked) AltezzatextBox2.Text = Convert.ToString((Convert.ToSingle(LarghezzatextBox.Text) / Ratio), CultureInfo.InvariantCulture.NumberFormat);
                userAdjust();
                StatotoolStripStatusLabel.Text = "Immagine Pronta";
            }
            catch (Exception err)
            {
                MessageBox.Show("Errore nell'apertura del file:  " + err.Message, "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        //ESCI DAL PROGRAMMA CONVERTI IMMAGINE IN GCODE OK
        private void EscitoolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void VisualOriginalebutton_Click(object sender, EventArgs e)
        {

        }
        //DICHIARAZIONE VARIABILI PER SCRITTURA GCODE OK
        string line;
        float coordinateX; // X
        float coordinateY; // Y
        Int32 SZ; // se utilizzare S o Z
        float lastX; // ultima coordinata X per confrontarla
        float lastY; // ultima coordinata Y per confrontarla
        Int32 lastSZ; // ultimo valore S per confrontarlo
        string coordinataXstr; //formato stringa della X
        string coordinataYstr; //formato stringa della Y
        char SZchar; //formato carattere S o Z
        string SZstr; //formato stringa S

        //GENERA LINEA GCODE OK
        private void GeneraLineaGCODE() //genera la linea di Gcode
        {
            line = "";

            if (coordinateX != lastX) // aggiunge la coordinata X alla linea se è diversa dalla coordinata X precedente
            {
                coordinataXstr = string.Format(CultureInfo.InvariantCulture.NumberFormat,"{0:0.###}", coordinateX);
                line += 'X' + coordinataXstr;

            }
            if (coordinateY != lastY) // aggiunge la coordinata Y alla linea se è diversa dalla coordinata Y precedente
            {
                coordinataYstr = string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:0.###}", coordinateY);
                line += 'Y' + coordinataYstr;
            }
            if (SZ != lastSZ) // aggiunge valore power (potenza) alla linea se è differente dal valore precedente
            {
                SZstr = SZchar + Convert.ToString(SZ);
                line += SZstr;
            }


        }
        private void GCODEbutton_Click(object sender, EventArgs e) //Genera il file GCODE dell'immagine con il GCODE
        {
            if (ImgRev == null) return; //se non cè l'immagine non fa niente
            float altezza = Convert.ToSingle(AltezzatextBox2.Text, CultureInfo.InvariantCulture.NumberFormat); //altezza in input dall'utente per controllare se il parametro è valido
            float larghezza = Convert.ToSingle(LarghezzatextBox.Text, CultureInfo.InvariantCulture.NumberFormat); //larghezza in input per controllare se è valida
            float risoluzione = Convert.ToSingle(RisoluzionetextBox3.Text, CultureInfo.InvariantCulture.NumberFormat); //risoluzione in input per controllare se è valida
        
            if((ImgRev.Width<1) | (ImgRev.Height<1) | (larghezza<1) | (altezza<1) | (risoluzione <=0)) //controlla se i valori altezza, larghezza e risoluzione dell'immagine sono validi per poter generare GCODE
            {
                MessageBox.Show("Controlla Altezza, Larghezza e Risoluzione.", "valori non validi", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (Convert.ToInt32(FeedRatetextBox.Text) < 1) //controlla se il valore FeedRate è valido
            {
                MessageBox.Show("Controlla il valore FeedRate", "valore non valido", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;

            Int32 linea; //pixels dell'immagine della parte superiore ed inferiore 
            Int32 colonna; //pixels dell'immagine della parte sinistra e destra

            StatotoolStripStatusLabel.Text = "Generazione del file contenente il GCODE....";
            Refresh();

            List<string> LineeFile;
            LineeFile = new List<string>(); //lista linee file GCODE
            if (SradioButton1.Checked) SZchar = 'S'; //S o Z comando power
            else SZchar = 'Z';

            //////////////////////////////////////
            ///INIZIO SCRITTURA GCODE SU FILE////
            ////////////////////////////////////

            line = "M5\r"; //comando che ci permette di assicurarsi che il Laser è spento. Mandrino spento
            LineeFile.Add(line);

            lastX = -1; //reset delle ultime posizioni X, Y , S o Z
            lastY = -1;
            lastSZ = -1;

            line = "G90\r"; //comando che ci permette di utilizzare coordinate assolute 
            LineeFile.Add(line);

            line = "G21\r"; //comando che imposta l'unita di misura in Millimetri
            //line = "G20\r"; se si volesse l'unita di misura in Pollici

            line = "F" + FeedRatetextBox.Text + "\r"; //comando che imposta la velocità di avanzamento mm/min
            LineeFile.Add(line);

            Int32 pixelTot = ImgRev.Height * ImgRev.Width;
            Int32 pixelBurned = 0; //pixel scoloriti

            //Generazione GCODE per scanning orizzontale

            if (Scansionelabel.Text=="Scansione Orizzontale") //verifico se l'utente ha scelto la scansione orizzontale
            {
                // comando G0: movimento rapido lineare, in questo caso movimento rapido nell'angolo in alto a sinistra. Quindi movimento 0 mm sull'asse X e movimento altezza img * risoluzione mm sull'asse Y
                line = "G0X0Y" + string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:0.###}", ImgRev.Height * Convert.ToSingle(RisoluzionetextBox3.Text, CultureInfo.InvariantCulture.NumberFormat));
                LineeFile.Add(line);

                // comando G1: movimento lineare
                line = "G1\r";
                LineeFile.Add(line);

                // comando M3: accensione laser/mandrino 
                line = "M3\r";
                LineeFile.Add(line);

                //INIZIO IMMAGINE
                linea = ImgRev.Height - 1; // top 
                colonna = 0; //pixel snistra

                while (linea >= 0)
                {
                    //Coordinata Y
                    coordinateY = risoluzione * linea;
                    while (colonna < ImgRev.Width) // da SINISTRA A DESTRA. Movimento in larghezza
                    {
                        //Coordinata X
                        coordinateX = risoluzione * colonna;

                        //Valore power
                        Color colore = ImgRev.GetPixel(colonna, (ImgRev.Height - 1) - linea); //prendo pixel colore
                        SZ = 255 - colore.R; //rosso
                        SZ = InterpolazioneImg(SZ, Convert.ToInt32(MinPowertextBox.Text), Convert.ToInt32(MaxPowertextBox.Text));
                        GeneraLineaGCODE();
                        pixelBurned++;

                        if (!string.IsNullOrEmpty(line))
                            LineeFile.Add(line);
                        lastX = coordinateX; //assegno alle variabili last le ultime coordinate X,Y,SZ
                        lastY = coordinateY;
                        lastSZ = SZ;
                        colonna++;
                    }

                    colonna--;
                    linea--;
                    coordinateY = risoluzione * (float)linea;

                    while ((colonna >= 0) & (linea >= 0)) // da DESTRA A SINISTRA
                    {
                        //Coordinata X
                        coordinateX = risoluzione * (float)colonna;

                        //Valore power
                        Color colore = ImgRev.GetPixel(colonna, (ImgRev.Height - 1) - linea); //prendo pixel colore
                        SZ = 255 - colore.R; //rosso
                        SZ = InterpolazioneImg(SZ, Convert.ToInt32(MinPowertextBox.Text), Convert.ToInt32(MaxPowertextBox.Text));
                        GeneraLineaGCODE();
                        pixelBurned++;

                        if (!string.IsNullOrEmpty(line))
                            LineeFile.Add(line);
                        lastX = coordinateX; //assegno alle variabili last le ultime coordinate X,Y,SZ
                        lastY = coordinateY;
                        lastSZ = SZ;
                        colonna--;
                    }
                    colonna++;
                    linea--;
                    StatotoolStripStatusLabel.Text = "Generazione del File in corso.... " + Convert.ToString((pixelBurned * 100) / pixelTot) + "%";
                    Refresh();
                }
            }

            //linee bordo
            if (IncisioneBordocheckBox1.Checked)
            {
                line = "M5\r";
                LineeFile.Add(line);

                line = "G0X0Y0\r";
                LineeFile.Add(line);

                line = "M3S" + MaxPowertextBox.Text + "\r";
                LineeFile.Add(line);

                line = "G1X0Y" + string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:0.###}", (ImgRev.Height - 1) * risoluzione) + "\r";
                LineeFile.Add(line);

                line = "G1X" + string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:0.###}", (ImgRev.Width - 1) * risoluzione) + "Y" + string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:0.###}", (ImgRev.Height - 1) * risoluzione) + "\r";
                LineeFile.Add(line);

                line = "G1X" + string.Format(CultureInfo.InvariantCulture.NumberFormat, "{0:0.###}", (ImgRev.Width - 1) * risoluzione) + "Y0\r";
                LineeFile.Add(line);

                line = "G1X0Y0\r";
                LineeFile.Add(line);
                    
            }
            //Spengo il Laser/Mandrino
            line = "M5\r";
            LineeFile.Add(line);

            StatotoolStripStatusLabel.Text = "Salvataggio del file in corso....";
            Refresh();

            //Salvataggio del file con il GCODE dell'immagine
            File.WriteAllLines(saveFileDialog1.FileName, LineeFile);
            StatotoolStripStatusLabel.Text = "Fatto (" + Convert.ToString(pixelBurned) + "/" + Convert.ToString(pixelTot) + ")";         

            
        }

        
        //OK 

        private void FormConvertImg_FormClosing(object sender, FormClosingEventArgs e)
        {
            //SaveSettings();
        }

        //OK
        private void FeedRatetextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!checkDigitInteger(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        private void MinPowertextBox_TextChanged(object sender, EventArgs e)
        {
           
        }

        //OK
        private void MinPowertextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!checkDigitInteger(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }

        //OK
        private void MaxPowertextBox_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!checkDigitInteger(e.KeyChar))
            {
                e.Handled = true;
                return;
            }
        }
    }
}

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

namespace Elaborato_EdS_2021
{
    public partial class FormTesto : Form
    {
        List<List<string>> GLista = new List<List<string>>(); //Lista di liste di Gcode  
        float GcodeX = 0; //valore X Gcode
        float GcodeY = 0; //valore Y Gcode
        float NewX = 0; //valore nuovo X
        float NewY = 0; //valore nuovo Y
        float OffsetX = 0; //spostamento rispetto alla X
        float FlyAltezza = 5;
        float ProfonditàAltezza = -1; //profondità di taglio
        float FontAltezza = 12; //altezza font
        string FlyAltezzaStr;
        string ProfonditàAltezzaStr;
        string FontAltezzaStr;
        float ScaleFactor;
        
        public FormTesto()
        {
            InitializeComponent();
        }

        //PRENDO COORDINATE DALLA STRINGA OK
        private void CoordinateDaStringa(string stringaCoo) //funzione che analizza una stringa nel formato "Xnumero Ynumero" e assegna i valori X e Y alle variabile GcodeX e GcodeY
        {
            string stringa = stringaCoo.Trim().ToUpper(); //vengono rimossi tutti gli spazi con trim e con toupper viene tutto trasformato in maiuscolo
            int posizioneX = stringa.LastIndexOf("X"); //con lastindex trovo la posizione di X nella stringa
            int posizioneY = stringa.LastIndexOf("Y"); //trovo la posizione della Y

            if ((posizioneX == -1) | (posizioneY == -1)) //se tutti e due hanno valore -1 (vuol dire che lastindex non ha trovato il carattere specificato nella stringa) allora genero errore
            {
                MessageBox.Show("Linea Input non ha coordinate ne X ne Y!!", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else //se invece ce le ha allora convertiamo
            {
                if (posizioneY > posizioneX)//verifico se la Y nella stringa è dopo la X
                {
                    string bitY = stringa.Substring(posizioneY + 1); //attraverso substring prendo la stringa subito dopo la Y
                    string bitX = stringa.Substring(1, posizioneY - 1).Trim(); // attraverso substring prendo la stringa da dopo la X fino a prima dell Y e poi tolgo spazi attraverso trim

                    //memorizzo le coordinate e le trasformo in numeri da stringhe
                    GcodeX = (float)Convert.ToDouble(bitX);
                    GcodeY = Convert.ToSingle(bitY);
                }
            }
        }

        //LISTA GCODE CARATTERI (LISTA DI LISTE DI GCODE CARATTERI)
        private void ListaGcodeCaratteri()
        {
            GLista.Add(new List<string>
            {
             "0",
             "ZF",
             "X1 Y2",
             "ZC",
             "X11 Y16",
             "ZF",
             "X12 Y12",
             "ZC",
             "X12 Y6",
             "X9 Y0",
             "X3 Y0",
             "X0 Y6",
             "X0 Y12",
             "X3 Y18",
             "X9 Y18",
             "X12 Y12",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "1",
             "ZF",
             "X3 Y0",
             "ZC",
             "X9 Y0",
             "X6 Y0",
             "X6 Y18",
             "X3 Y15",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "2",
             "ZF",
             "X0 Y15",
             "ZC",
             "X3 Y18",
             "X9 Y18",
             "X12 Y15",
             "X12 Y11",
             "X2 Y5",
             "X0 Y0",
             "X12 Y0",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "3",
             "ZF",
             "X0 Y2",
             "ZC",
             "X3 Y0",
             "X9 Y0",
             "X12 Y3",
             "X12 Y7",
             "X9 Y9",
             "X3 Y9",
             "X9 Y9",
             "X12 Y11",
             "X12 Y15",
             "X9 Y18",
             "X3 Y18 ",
             "X0 Y16",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "4",
             "ZF",
             "X9 Y0",
             "ZC",
             "X9 Y18",
             "X0 Y6",
             "X12 Y6",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "5",
             "ZF",
             "X0 Y2",
             "ZC",
             "X3 Y0",
             "X9 Y0",
             "X12 Y2",
             "X12 Y8",
             "X9 Y10",
             "X3 Y10",
             "X0 Y9",
             "X2 Y18",
             "X12 Y18",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "6",
             "ZF",
             "X0 Y7",
             "ZC",
             "X3 Y10",
             "X9 Y10",
             "X12 Y7",
             "X12 Y3",
             "X9 Y0",
             "X3 Y0",
             "X0 Y3",
             "X0 Y10",
             "X3 Y15",
             "X7 Y18",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "7",
             "ZF",
             "X0 Y18",
             "ZC",
             "X12 Y18",
             "X4 Y0",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "8",
             "ZF",
             "X9 Y10",
             "ZC",
             "X12 Y13",
             "X12 Y15",
             "X9 Y18",
             "X3 Y18",
             "X0 Y15",
             "X0 Y13",
             "X3 Y10",
             "X9 Y10",
             "X12 Y7",
             "X12 Y3",
             "X9 Y0",
             "X3 Y0",
             "X0 Y3",
             "X0 Y7",
             "X3 Y10",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "9",
             "ZF",
             "X5 Y0",
             "ZC",
             "X9 Y3",
             "X12 Y8",
             "X12 Y15",
             "X9 Y18",
             "X3 Y18",
             "X0 Y15",
             "X0 Y11",
             "X3 Y8",
             "X9 Y8",
             "X12 Y11",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             ".",
             "ZF",
             "X6 Y0",
             "ZC",
             "ZF"
            });


            GLista.Add(new List<string>
            {
             "a",
             "ZF",
             "X0 Y10",
             "ZC",
             "X5 Y12",
             "X11 Y10",
             "X11 Y2",
             "X8 Y0",
             "X4 Y0",
             "X0 Y2",
             "X0 Y5",
             "X11 Y6",
             "X11 Y2",
             "X13 Y0",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "b",
             "ZF",
             "X0 Y18",
             "ZC",
             "X0 Y0",
             "X0 Y2",
             "X6 Y0",
             "X12 Y2",
             "X12 Y9",
             "X6 Y11",
             "X0 Y9",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "c",
             "ZF",
             "X11 Y9",
             "ZC",
             "X6 Y11",
             "X0 Y9",
             "X0 Y2",
             "X6 Y0",
             "X11 Y2",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "d",
             "ZF",
             "X12 Y9",
             "ZC",
             "X6 Y11",
             "X0 Y9",
             "X0 Y2",
             "X6 Y0",
             "X12 Y2",
             "X12 Y0",
             "X12 Y18",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "e",
             "ZF",
             "X0 Y6",
             "ZC",
             "X12 Y7",
             "X9 Y12",
             "X3 Y12",
             "X0 Y9",
             "X0 Y2",
             "X3 Y0",
             "X9 Y0",
             "X12 Y2",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "f",
             "ZF",
             "X0 Y9",
             "ZC",
             "X8 Y9",
             "ZF",
             "X12 Y16",
             "ZC",
             "X8 Y18",
             "X4 Y16",
             "X4 Y0",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "g",
             "ZF",
             "X11 Y2",
             "ZC",
             "X6 Y0",
             "X0 Y2",
             "X0 Y9",
             "X6 Y11",
             "X11 Y9",
             "X11 Y11",
             "X11 Y-5",
             "X6 Y-7",
             "X0 Y-5",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "h",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y18",
             "ZF",
             "X0 Y9",
             "ZC",
             "X6 Y11",
             "X12 Y9",
             "X12 Y0",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "i",
             "ZF",
             "X7 Y0",
             "ZC",
             "X7 Y11",
             "X4 Y11",
             "ZF",
             "X7 Y18",
             "ZC",
             "X6 Y18",
             "X6 Y17",
             "X7 Y17",
             "X7 Y18",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "j",
             "ZF",
             "X0 Y-5",
             "ZC",
             "X4 Y-7",
             "X8 Y-5",
             "X8 Y11",
             "ZF",
             "X8 Y18",
             "ZC",
             "X7 Y18",
             "X7 Y17",
             "X8 Y17",
             "X8 Y18",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "k",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y18",
             "ZF",
             "X0 Y5",
             "ZC",
             "X12 Y11",
             "ZF",
             "X4 Y7",
             "ZC",
             "X12 Y0",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "l",
             "ZF",
             "X3 Y18",
             "ZC",
             "X6 Y18",
             "X6 Y0",
             "X3 Y0",
             "X9 Y0",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "m",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y12",
             "X0 Y9",
             "X4 Y12",
             "X6 Y9",
             "X6 Y0",
             "ZF",
             "X6 Y9",
             "ZC",
             "X10 Y12",
             "X12 Y9",
             "X12 Y0",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "n",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y11",
             "X0 Y8",
             "X6 Y11",
             "X12 Y8",
             "X12 Y0",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "o",
             "ZF",
             "X6 Y0",
             "ZC",
             "X12 Y2",
             "X12 Y9",
             "X6 Y11",
             "X0 Y9",
             "X0 Y2",
             "X6 Y0",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "p",
             "ZF",
             "X0 Y-7",
             "ZC",
             "X0 Y11",
             "X0 Y9",
             "X6 Y11",
             "X12 Y9",
             "X12 Y2",
             "X6 Y0",
             "X0 Y2",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "q",
             "ZF",
             "X11 Y2",
             "ZC",
             "X6 Y0",
             "X0 Y2",
             "X0 Y9",
             "X6 Y11",
             "X11 Y9",
             "X11 Y11",
             "X11 Y-6",
             "X13 Y-8",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "r",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y11",
             "X0 Y8",
             "X6 Y11",
             "X12 Y8",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "s",
             "ZF",
             "X0 Y2",
             "ZC",
             "X6 Y0",
             "X12 Y2",
             "X12 Y5",
             "X0 Y7",
             "X0 Y10",
             "X6 Y12",
             "X12 Y10",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "t",
             "ZF",
             "X0 Y11",
             "ZC",
             "X8 Y11",
             "ZF",
             "X4 Y18",
             "ZC",
             "X4 Y2",
             "X8 Y0",
             "X12 Y2",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "u",
             "ZF",
             "X0 Y11",
             "ZC",
             "X0 Y2",
             "X6 Y0",
             "X12 Y2",
             "X12 Y11",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "v",
             "ZF",
             "X0 Y11",
             "ZC",
             "X6 Y0",
             "X12 Y11",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "w",
             "ZF",
             "X0 Y11",
             "ZC",
             "X3 Y0",
             "X6 Y8",
             "X9 Y0",
             "X12 Y11",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "x",
             "ZF",
             "X0 Y0",
             "ZC",
             "X11 Y11",
             "ZF",
             "X0 Y11",
             "ZC",
             "X11 Y0",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "y",
             "ZF",
             "X0 Y11",
             "ZC",
             "X7 Y1",
             "ZF",
             "X3 Y-7",
             "ZC",
             "X12 Y11",
             "ZF"
            });
            GLista.Add(new List<string>
            {
             "z",
             "ZF",
             "X0 Y11",
             "ZC",
             "X12 Y11",
             "X0 Y0",
             "X12 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             " ",
             "ZF"
            });


            GLista.Add(new List<string>
            {
             "A",
             "ZF",
             "X0 Y0",
             "ZC",
             "X6 Y18",
             "X12 Y0",
             "ZF",
             "X3 Y9",
             "ZC",
             "X9 Y9",
             "ZF"

            });


            GLista.Add(new List<string>
            {
             "B",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y18",
             "X9 Y18",
             "X12 Y15",
             "X12 Y12",
             "X9 Y9",
             "X0 Y9",
             "X9 Y9",
             "X12 Y6",
             "X12 Y3",
             "X9 Y0",
             "X0 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "C",
             "ZF",
             "X12 Y3",
             "ZC",
             "X9 Y0",
             "X3 Y0",
             "X0 Y3",
             "X0 Y15",
             "X3 Y18",
             "X9 Y18",
             "X12 Y15",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "D",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y18",
             "X9 Y18",
             "X12 Y15",
             "X12 Y3",
             "X9 Y0",
             "X0 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "E",
             "ZF",
             "X9 Y9",
             "ZC",
             "X0 Y9",
             "ZF",
             "X12 Y18",
             "ZC",
             "X0 Y18",
             "X0 Y0",
             "X12 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "F",
             "ZF",
             "X9 Y9",
             "ZC",
             "X0 Y9",
             "ZF",
             "X12 Y18",
             "ZC",
             "X0 Y18",
             "X0 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "G",
             "ZF",
             "X12 Y15",
             "ZC",
             "X9 Y18",
             "X3 Y18",
             "X0 Y15",
             "X0 Y3",
             "X3 Y0",
             "X9 Y0",
             "X12 Y3",
             "X12 Y8",
             "X5 Y8",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "H",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y18",
             "ZF",
             "X12 Y0",
             "ZC",
             "X12 Y18",
             "ZF",
             "X0 Y9",
             "ZC",
             "X12 Y9",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "I",
             "ZF",
             "X2 Y0",
             "ZC",
             "X10 Y0",
             "X6 Y0",
             "X6 Y18",
             "X2 Y18",
             "X10 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "J",
             "ZF",
             "X0 Y2",
             "ZC",
             "X3 Y0",
             "X5 Y0",
             "X8 Y2",
             "X8 Y18",
             "X4 Y18",
             "X12 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "K",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y18",
             "ZF",
             "X12 Y18",
             "ZC",
             "X0 Y6",
             "X3 Y9",
             "X12 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "L",
             "ZF",
             "X0 Y18",
             "ZC",
             "X0 Y0",
             "X12 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "M",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y18",
             "X6 Y5",
             "X12 Y18",
             "X12 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "N",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y18",
             "X12 Y0",
             "X12 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "O",
             "ZF",
             "X3 Y0",
             "ZC",
             "X9 Y0",
             "X12 Y3",
             "X12 Y15",
             "X9 Y18",
             "X3 Y18",
             "X0 Y15",
             "X0 Y3",
             "X3 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "P",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y18",
             "X9 Y18",
             "X12 Y15",
             "X12 Y11",
             "X9 Y8",
             "X0 Y8",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "Q",
             "ZF",
             "X3 Y0",
             "ZC",
             "X9 Y0",
             "X12 Y3",
             "X12 Y15",
             "X9 Y18",
             "X3 Y18",
             "X0 Y15",
             "X0 Y3",
             "X3 Y0",
             "ZF",
             "X7 Y5",
             "ZC",
             "X14 Y-2",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "R",
             "ZF",
             "X0 Y0",
             "ZC",
             "X0 Y18",
             "X9 Y18",
             "X12 Y15",
             "X12 Y11",
             "X9 Y8",
             "X0 Y8",
             "X7 Y8",
             "X12 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "S",
             "ZF",
             "X0 Y2",
             "ZC",
             "X3 Y0",
             "X9 Y0",
             "X12 Y3",
             "X12 Y6",
             "X9 Y9",
             "X3 Y9",
             "X0 Y12",
             "X0 Y15",
             "X3 Y18",
             "X9 Y18",
             "X12 Y16",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "T",
             "ZF",
             "X6 Y0",
             "ZC",
             "X6 Y18",
             "X0 Y18",
             "X12 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "U",
             "ZF",
             "X0 Y18",
             "ZC",
             "X0 Y3",
             "X3 Y0",
             "X9 Y0",
             "X12 Y3",
             "X12 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "V",
             "ZF",
             "X0 Y18",
             "ZC",
             "X6 Y0",
             "X12 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "W",
             "ZF",
             "X0 Y18",
             "ZC",
             "X3 Y0",
             "X6 Y14",
             "X9 Y0",
             "X12 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "X",
             "ZF",
             "X0 Y0",
             "ZC",
             "X12 Y18",
             "ZF",
             "X0 Y18",
             "ZC",
             "X12 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "Y",
             "ZF",
             "X0 Y18",
             "ZC",
             "X6 Y8",
             "X6 Y0",
             "X6 Y8",
             "X12 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "Z",
             "ZF",
             "X0 Y18",
             "ZC",
             "X12 Y18",
             "X0 Y0",
             "X12 Y0",
             "ZF"
            });


            GLista.Add(new List<string>
            {
             "&",
             "ZF",
             "X12 Y5",
             "ZC",
             "X8 Y0",
             "X2 Y0",
             "X0 Y4",
             "X9 Y14",
             "X7 Y18",
             "X3 Y18",
             "X1 Y14",
             "X12 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "'",
             "ZF",
             "X5 Y18",
             "ZC",
             "X5 Y18",
             "X7 Y14",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "@",
             "ZF",
             "X12 Y2",
             "ZC",
             "X10 Y0",
             "X3 Y0",
             "X0 Y3",
             "X0 Y15",
             "X3 Y18",
             "X9 Y18",
             "X12 Y15",
             "X12 Y6",
             "X5 Y6",
             "X5 Y13",
             "X12 Y13",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "\\",
             "ZF",
             "X0 Y18",
             "ZC",
             "X12 Y0",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "}",
             "ZF",
             "X0 Y-2",
             "ZC",
             "X5 Y1",
             "X5 Y6",
             "X8 Y9",
             "X5 Y12",
             "X5 Y17",
             "X0 Y20",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             ")",
             "ZF",
             "X0 Y-2",
             "ZC",
             "X6 Y4",
             "X6 Y14",
             "X0 Y20",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "]",
             "ZF",
             "X0 Y-2",
             "ZC",
             "X6 Y-2",
             "X6 Y20",
             "X0 Y20",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             ":",
             "ZF",
             "X6 Y14",
             "ZC",
             "ZF",
             "X6 Y4",
             "ZC",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             ",",
             "ZF",
             "X4 Y-4",
             "ZC",
             "X6 Y1",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "$",
             "ZF",
             "X0 Y3",
             "ZC",
             "X3 Y1",
             "X9 Y1",
             "X12 Y3",
             "X12 Y7",
             "X9 Y9",
             "X3 Y9",
             "X0 Y11",
             "X0 Y15",
             "X3 Y17",
             "X9 Y17",
             "X12 Y15",
             "ZF",
             "X6 Y19",
             "ZC",
             "X6 Y-1",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "\"",
             "ZF",
             "X3 Y14",
             "ZC",
             "X4 Y18",
             "ZF",
             "X7 Y14",
             "ZC",
             "X8 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "=",
             "ZF",
             "X0 Y4",
             "ZC",
             "X12 Y4",
             "ZF",
             "X0 Y14",
             "ZC",
             "X12 Y14",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "!",
             "ZF",
             "X6 Y18",
             "ZC",
             "X6 Y5",
             "ZF",
             "X6 Y0",
             "ZC",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "/",
             "ZF",
             "X0 Y0",
             "ZC",
             "X12 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             ">",
             "ZF",
             "X0 Y0",
             "ZC",
             "X12 Y9",
             "X0 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "#",
             "ZF",
             "X2 Y0",
             "ZC",
             "X4 Y18",
             "ZF",
             "X10 Y18",
             "ZC",
             "X8 Y0",
             "ZF",
             "X12 Y5",
             "ZC",
             "X0 Y5",
             "ZF",
             "X0 Y13",
             "ZC",
             "X12 Y13",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "<",
             "ZF",
             "X12 Y0",
             "ZC",
             "X0 Y9",
             "X12 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "-",
             "ZF",
             "X0 Y9",
             "ZC",
             "X12 Y9",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "{",
             "ZF",
             "X12 Y-2",
             "ZC",
             "X7 Y1",
             "X7 Y6",
             "X4 Y9",
             "X7 Y12",
             "X7 Y17",
             "X12 Y20",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "(",
             "ZF",
             "X12 Y-2",
             "ZC",
             "X6 Y4",
             "X6 Y14",
             "X12 Y20",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "[",
             "ZF",
             "X12 Y-2",
             "ZC",
             "X6 Y-2",
             "X6 Y20",
             "X12 Y20",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "|",
             "ZF",
             "X6 Y0",
             "ZC",
             "X6 Y6",
             "ZF",
             "X6 Y12",
             "ZC",
             "X6 Y18",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "%",
             "ZF",
             "X0 Y0",
             "ZC",
             "X12 Y18",
             "ZF",
             "X6 Y14",
             "ZC",
             "X3 Y18",
             "X0 Y14",
             "X3 Y10",
             "X6 Y14",
             "ZF",
             "X9 Y8",
             "ZC",
             "X12 Y4",
             "X9 Y0",
             "X6 Y4",
             "X9 Y8",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "+",
             "ZF",
             "X6 Y2",
             "ZC",
             "X6 Y16",
             "ZF",
             "X0 Y9",
             "ZC",
             "X12 Y9",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "?",
             "ZF",
             "X6 Y0",
             "ZC",
             "ZF",
             "X6 Y4",
             "ZC",
             "X6 Y7",
             "X12 Y11",
             "X12 Y15",
             "X9 Y18",
             "X3 Y18",
             "X0 Y15",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "^",
             "ZF",
             "X0 Y7",
             "ZC",
             "X6 Y16",
             "X12 Y7",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             ";",
             "ZF",
             "X5 Y-4",
             "ZC",
             "X7 Y0",
             "ZF",
             "X7 Y10",
             "ZC",
             "ZF"
            });

            GLista.Add(new List<string>
            {
             "*",
             "ZF",
             "X3 Y2",
             "ZC",
             "X9 Y16",
             "ZF",
             "X3 Y16",
             "ZC",
             "X9 Y2",
             "ZF",
             "X12 Y9",
             "ZC",
             "X0 Y9",
             "ZF"
            });
        }
        private void EscitoolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        //GENERAZIONE GCODE
        private void GeneraGcodebutton1_Click(object sender, EventArgs e)
        {
            OffsetX = 0; //reset dell'offset orizzontale

            //CONTROLLI

            try //Fly Altezza
            {
                FlyAltezza = Convert.ToSingle(FlyAltezzatextBox2.Text);
            }
            catch
            {
                MessageBox.Show("Valore non valido in Fly Altezza", "errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try //Profondità di taglio
            {
                ProfonditàAltezza = Convert.ToSingle(ProfonditatextBox3.Text);
            }
            catch
            {
                MessageBox.Show("Valore non valido in Profondità di taglio", "errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            try //Font Altezza
            {
                FontAltezza = Convert.ToSingle(FontAltezza);
            }
            catch
            {
                MessageBox.Show("Valore non valido in Font Altezza", "errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (FlyAltezza > 0.5) FlyAltezzaStr = "Z" + FlyAltezza.ToString("0.000");
            else FlyAltezzaStr = "Z5.0";

            ScaleFactor = FontAltezza / 18.0f;

            if (Testotxt.Text != "") //controlla se l'utente ha inserito il testo
            {
                StringBuilder sb = new StringBuilder();

                List<string> Gcode;
                Gcode = new List<string>(); //Lista Gcode linee File in Output

                sb.AppendLine("G21"); //unità di misura in mm
                sb.AppendLine("F1000"); //Feedrate predefinita. Velocità di avanzamento

                string Testo = Testotxt.Text; //prendo il testo da convertire inserito dall'utente

                foreach(char carattere in Testo)
                {
                    //Faccio passare ogni singolo carattere del testo per poi trovare il suo GCode all'interno della lista
                    string tempstringa = GLista[1][3];

                    foreach(List<string> GcodeListaCaratteri in GLista) 
                    {
                        if(GcodeListaCaratteri[0][0] == carattere) //se il primo carattere del testo corrisponde al primo elemento della lista di liste
                        {
                            //Elaboro la lista GCODE corrispondente al carattere
                            int NItemsLista = GcodeListaCaratteri.Count(); //prendo il numero di elementi della lista

                            for(int i = 0; i < NItemsLista; i++)
                            {
                                string elem = GcodeListaCaratteri[i]; // prendo ogni elemento della lista

                                if(elem.Trim() == "ZF") //quando l'elemento è ZF inserisco Fly Altezza specificata dall'utente
                                {
                                    sb.AppendLine(FlyAltezzaStr);
                                }
                                if(elem.Trim() == "ZC") //quando l'elemento è ZC inserisco Profondità
                                {
                                    sb.AppendLine(ProfonditàAltezzaStr);
                                }
                                else //l'elemento corrisponde a coordinate GCODE
                                {
                                    CoordinateDaStringa(elem); //converto XY coordinata e prendo i valori X e Y

                                    NewX = GcodeX + OffsetX;
                                    NewY = GcodeY;

                                    //dimensione Font richiede scaling delle due coordinate X e Y
                                    NewX = NewX * ScaleFactor;
                                    NewY = NewY * ScaleFactor;

                                    //genero stringa con le due coordinate
                                    string NewCoordinataX = NewX.ToString("0.000");
                                    string NewCoordinataY = NewY.ToString("0.000");
                                    string stringaCoordinate = "X" + NewCoordinataX + " Y" + NewCoordinataY;
                                    sb.AppendLine(stringaCoordinate);
                                }
                            }

                            OffsetX = OffsetX + 18; // incremento offset X per il movimento del prossimo carattere
                        }
                    }
                
                }

                sb.Replace(',', '.'); //sostituisco le istruzioni GCODE con le virgole in punti. Es: X4,178-->X4.178
                Gcode.Add(sb.ToString());
                saveFileDialog1.FileName = Testotxt.Text + "_GCODE.nc";
                File.WriteAllLines(saveFileDialog1.FileName, Gcode);
                
            }
            else
            {
                MessageBox.Show("Testo non inserito!");
            }
        }

        private void FormTesto_Load(object sender, EventArgs e)
        {
            ListaGcodeCaratteri();
        }
    }


}

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

        //VARIABILI  OK

        List<List<string>> GCodeLista = new List<List<string>>(); //Lista di liste GCode dei caratteri
        
        float GcodeX = 0; //valore X Gcode
        float GcodeY = 0; //valore Y Gcode

        float NewX = 0;
        float NewY = 0;

        float OffsetX = 0; //movimento sull'asse X

        float FlyAltezza = 5;
        float FontAltezza = 12; //altezza font
        float ProfonditàAltezza = -1; //profondità di taglio
        
        string FlyAltezzaStr;
        //string FontAltezzaStr;
        string ProfonditàAltezzaStr;

        float ScaleFactor;
        public FormTesto()
        {
            InitializeComponent();
            CaricaGCodeLista(); //Carico la lista di liste di coordinate XY dei caratteri
        }

       private void EscitoolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        //GENERAZIONE GCODE 
        private void GeneraGcodebutton1_Click(object sender, EventArgs e)
        {

            OffsetX = 0; //reset dell'offset
            
            try //provo a convertire FlyAltezza
            {
                FlyAltezza = Convert.ToSingle(FlyAltezzatextBox2.Text); 
            }
            catch
            {
                MessageBox.Show("Valore Fly Altezza non valido!");
                Application.Exit();
            }

            try //provo a convertire Profondità di Taglio
            {
                ProfonditàAltezza = Convert.ToSingle(ProfonditatextBox3.Text);
            }
            catch
            {
                MessageBox.Show("Valore Profondità non valido!");
                Application.Exit();
            }

            try //provo a convertire Altezza Font
            {
                FontAltezza = Convert.ToSingle(AltezzatextBox1.Text);
            }
            catch
            {
                MessageBox.Show("Valore Font Altezza non valido!");
                Application.Exit();
            }
            
            if (FlyAltezza > 0.5)
            {
                FlyAltezzaStr = "Z" + FlyAltezza.ToString("0.000");
            }
            else
            {
                FlyAltezzaStr = "Z5.0";
            }
            
            ProfonditàAltezzaStr = "Z" + ProfonditàAltezza.ToString("0.000");

            if (FontAltezza < 0.5)
            {
                FontAltezza = 0.5f;
            }

            ScaleFactor = FontAltezza / 18.0f;  

            if (Testotxt.Text != "") //controllo se la textbox del testo non è vuota
            {

                if (saveFileDialog1.ShowDialog() == DialogResult.Cancel) return;
                List<string> Gcode = new List<string>();

                StringBuilder sb = new StringBuilder();

                sb.AppendLine("G21"); //imposto unità in mm
                sb.AppendLine("F1000"); //FeedRate (velocità di avanzamento) predefinita

                Application.DoEvents();

                string Testo = Testotxt.Text; //memorizzo testi inserito in una variabile
                
                foreach (char carattere in Testo)  //faccio passare ogni carattere del testo
                {
                    string tempstring = GCodeLista[1][3];

                    foreach (List<string> GcodeListaCarattere in GCodeLista) //lista delle righe di Gcode dei caratteri
                    {

                        string atempstring = GcodeListaCarattere[0];

                        if (GcodeListaCarattere[0][0] == carattere) //quando e se il primo carattere del testo sarà uguale al carattere rappresentativo del suo Gcode allora vado avanti
                        {
                            
                            int numero_elem = GcodeListaCarattere.Count(); //memorizzo numero di elementi nella lista Gcode del carattere

                            for (int i = 1; i < (numero_elem); i++)
                            {
                                string elem = GcodeListaCarattere[i];

                                if (elem.Trim() == "ZF") //se la linea di Gcode del carattere contiene ZF allora inserico nella stessa linea FlyAltezza
                                {
                                    sb.AppendLine(FlyAltezzaStr);
                                }
                                else if (elem.Trim() == "ZC") //se la linea di Gcode del carattere contiene ZC allora inserisco nella stessa linea la Profondità di Taglio
                                {
                                    sb.AppendLine(ProfonditàAltezzaStr);
                                }
                                else
                                {
                                    PrendiCoordinate(elem);

                                    NewX = GcodeX + OffsetX;
                                    NewY = GcodeY;

                                    NewX = NewX * ScaleFactor;
                                    NewY = NewY * ScaleFactor;

                                    string NewXcoordinata = NewX.ToString("0.000");
                                    string NewYcoordinata = NewY.ToString("0.000");

                                    string lineaGcode = "X" + NewXcoordinata + " Y" + NewYcoordinata;
                                    sb.AppendLine(lineaGcode);
                                }
                                
                            }

                            OffsetX = OffsetX + 18; //incremento Offset
                        }

                    }
                    
                }
                sb.Replace(',', '.'); //sostituisco le , con i punti.Esempio: X4,718-->X4.718
                Gcode.Add(sb.ToString());//inserisco il Gcode nella lista
               
                File.WriteAllLines(saveFileDialog1.FileName, Gcode);//salvo il file
            }
            else
            {
                MessageBox.Show("Testo da convertire non inserito, riprova!");
            }
        }

        public void CaricaGCodeLista()
        {
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
            {
             "7",
             "ZF",
             "X0 Y18",
             "ZC",
             "X12 Y18",
             "X4 Y0",
             "ZF"
            });
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
            {
             ".",
             "ZF",
             "X6 Y0",
             "ZC",
             "ZF"
            });


            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
            {
             "v",
             "ZF",
             "X0 Y11",
             "ZC",
             "X6 Y0",
             "X12 Y11",
             "ZF"
            });
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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
            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
            {
             " ",
             "ZF"
            });


            GCodeLista.Add(new List<string>
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


            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
            {
             "L",
             "ZF",
             "X0 Y18",
             "ZC",
             "X0 Y0",
             "X12 Y0",
             "ZF"
            });

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
            {
             "V",
             "ZF",
             "X0 Y18",
             "ZC",
             "X6 Y0",
             "X12 Y18",
             "ZF"
            });

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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


            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
            {
             "'",
             "ZF",
             "X5 Y18",
             "ZC",
             "X5 Y18",
             "X7 Y14",
             "ZF"
            });

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
            {
             "\\",
             "ZF",
             "X0 Y18",
             "ZC",
             "X12 Y0",
             "ZF"
            });

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
            {
             ",",
             "ZF",
             "X4 Y-4",
             "ZC",
             "X6 Y1",
             "ZF"
            });

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
            {
             "/",
             "ZF",
             "X0 Y0",
             "ZC",
             "X12 Y18",
             "ZF"
            });

            GCodeLista.Add(new List<string>
            {
             ">",
             "ZF",
             "X0 Y0",
             "ZC",
             "X12 Y9",
             "X0 Y18",
             "ZF"
            });

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
            {
             "<",
             "ZF",
             "X12 Y0",
             "ZC",
             "X0 Y9",
             "X12 Y18",
             "ZF"
            });

            GCodeLista.Add(new List<string>
            {
             "-",
             "ZF",
             "X0 Y9",
             "ZC",
             "X12 Y9",
             "ZF"
            });

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
            {
             "^",
             "ZF",
             "X0 Y7",
             "ZC",
             "X6 Y16",
             "X12 Y7",
             "ZF"
            });

            GCodeLista.Add(new List<string>
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

            GCodeLista.Add(new List<string>
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

        public void PrendiCoordinate(string stringaCoordinate)
        {
            
            string Newstringa = stringaCoordinate.Trim().ToUpper(); //tolgo gli spazi dalla stringa contenente le coordinate
            int Xpos = Newstringa.LastIndexOf("X"); //controllo se è presente la coordinata X e memorizzo la sua posizione
            int Ypos = Newstringa.LastIndexOf("Y"); //controllo anche sulla Y
            if ((Xpos == -1) | (Ypos == -1)) //se non sono presenti genero un errore
            {
                MessageBox.Show("Errore!");
                Application.Exit();
            }
            else
            {
                
                if (Ypos > Xpos) //se la Y è dopo la X allora vado avanti
                {
                    string theYbit = Newstringa.Substring(Ypos + 1);   //prendo coordinate dopo la Y
                    string theXbit = Newstringa.Substring(1, Ypos - 1).Trim();   //prendo coordinato dopo la X e prima della Y
                    GcodeX = (float)Convert.ToDouble(theXbit); //converto in istruzioni GCode
                    GcodeY = Convert.ToSingle(theYbit);
                    
                }

            }

            
        }

        private void FormTesto_Load(object sender, EventArgs e)
        {
            Text = "Testo in GCode per macchine utensili";
        }
    }




}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Kw.Combinatorics;

namespace DaneZPlikuOkienko
{
    public partial class Form2 : Form
    {
        public Form2(string[][] systemDecyzyjny)
        {
            InitializeComponent();

            var Lista = new List<Regula>();
            int[][][] macierz = Regula.MacierzNieodroznialnosci(systemDecyzyjny);//Tworze macierz nieordóżnialności
            for (int j = 1; j < systemDecyzyjny[0].Length - 1; j++)//pętla po rzędach aka covering
                for (int i = 0; i < macierz[0].Length; i++)//pętla po obiektach
                {
                    int[][] wiersz = Regula.Fdwuwymiarowywiersz(macierz, i);//wyciagamy wiersz i z macierzy nieodwracalnosci
                    string[] obiekt = Regula.Fwiersz(systemDecyzyjny, i);//wyciagamy obiekt i
                    foreach (Combination combo in new Combination(obiekt.Length - 1, j).GetRows())
                    {
                        if ((Regula.CzyZawieraWWierszu(wiersz, combo.ToArray())) == (false))//sprawdzamy czy dana kombinacja zawiera sie w wierszu jezeli nie to 
                        {
                            string[] wiersz_prawdziwy = Regula.Fwiersz(systemDecyzyjny, i);
                            Regula r = new Regula(wiersz_prawdziwy, combo.ToArray());//tworze regule

                            if (Lista.Count == 0)//jezeli lista jest pusta to
                            {
                                r.support = r.Fsupport(systemDecyzyjny);//lcize support
                                rtbGlowne.Text += r.ToString() + Environment.NewLine;//wypisuje
                                Lista.Add(r);//dodaje regule do listy         
                            }
                            else
                            {
                                int pomocnicza = 0;
                                foreach (var kvp in Lista)
                                    if (r.CzyRegulaZawieraInnaRegule(kvp) == false)//sprawdzam czy regula zaweira sie w liscie reguł (pomocnicza mówi mi czy reguła nie zawiera sie w kazdym elemencie listy czytaj wszystkei reguly)
                                    {
                                        pomocnicza++;//wiadomo bardzo wazna zmienna

                                    }
                                if (pomocnicza == Lista.Count)//jesli pomocnicza ma taka sama wartosc co wielkosc listy regul to oznacza ze moge użyć tej reguły
                                {
                                    r.support = r.Fsupport(systemDecyzyjny);//licze support
                                    rtbGlowne.Text += r.ToString() + Environment.NewLine;//wypisuje
                                    Lista.Add(r);//dodaje do listy
                                }

                            }
                        }
                    }
                }
        }
        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

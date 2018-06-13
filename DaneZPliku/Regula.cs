using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DaneZPlikuOkienko
{
    class Regula
    {
        public bool CzyObiektSpelniaRegule(string[] ob)
        {
            foreach (var desk in this.deskryptory)
            {
                if (ob[desk.Key] != desk.Value)
                    return false;
            }
            return true;
        }

        public static string[] Fwiersz(string[][] dane, int numerwiersza)//z wczytanych danych zwraca wybraną kolumne 
        {
            string[] wiersz = new string[dane[0].Length];
            for (int i = 0; i < wiersz.Length; i++)
            {
                wiersz[i] = dane[numerwiersza][i];
            }
            return wiersz;
        }
        public static int[][] Fdwuwymiarowywiersz(int[][][] dane, int numerwiersza)
        {
            int[][] wiersz = new int[dane[0].Length][];

            for (int i = 0; i < wiersz.Length; i++)
            {
                wiersz[i] = dane[numerwiersza][i];
                wiersz[i] = new int[dane[numerwiersza][i].Length];
                for (int j = 0; j < dane[numerwiersza][i].Length; j++)
                {
                    wiersz[i][j] = dane[numerwiersza][i][j];
                }
            }
            return wiersz;
        }

        public Regula(string[] obiekt, int[] kombinacja)//odniesienie do klasy
        {
            this.decyzja = obiekt.Last();
            for (int i = 0; i < kombinacja.Length; i++)
            {
                int nrAtrybutu = kombinacja[i];
                this.deskryptory.Add(nrAtrybutu, obiekt[nrAtrybutu]);
            }
        }
        public  int Fsupport(string[][] dane)
        {
            int support = 0;
            for (int i = 0; i < dane.Length; i++) 
            {
                if(CzyObiektSpelniaRegule(dane[i]) && decyzja == dane[i].Last())
                {
                    support++;
                }
            }
            return support;
        }

        public bool CzyRegulaNieSprzeczna(Regula r, string[][] obiekty)
        {
            foreach (var ob in obiekty)
            {
                if (r.CzyObiektSpelniaRegule(ob) && ob.Last() != r.decyzja)
                    return false;
            }
            return true;
        }


        // 1. Macierz nieodroznialnosci:
        //   a) komorka macierzy
        // 2. Kombinacje.
        // 3. Czy kombinacja zawiera sie:
        //   a) w komorce,
        //   b) w kolumnie/wierszu
        // 4. Czy regula zawiera regule.

        // 1. macierz
        // 2. petla po obiektach
        // 3. petla po kombinacjach
        // 4. czy zawiera sie w wierszu
        // 5. tworzenie reguly
        // 6. czy zawiera regule nizszego rzedu

        public static int[] TworzKomorke(string[] obiekt1, string[] obiekt2)
        {
            List<int> komorka = new List<int>();
            if (obiekt1.Last() == obiekt2.Last())
                return komorka.ToArray();

            for (int i = 0; i < obiekt1.Length - 1; i++)
            {
                if (obiekt1[i] == obiekt2[i])
                    komorka.Add(i);
            }
            return komorka.ToArray();
        }
        public static int[][][] MacierzNieodroznialnosci(string[][] obiekty)
        {
            int[][][] macierz = new int[obiekty.Length][][];
            for (int i = 0; i < obiekty.Length; i++)
            {
                macierz[i] = new int[obiekty.Length][];
                for (int j = 0; j < obiekty.Length; j++)
                {
                    macierz[i][j] = TworzKomorke(obiekty[i], obiekty[j]);
                }
            }
            return macierz;
        }

        public static bool CzyKombinacjaWKomorce(int[] komorka, int[] kombinacja)
        {
            for (int i = 0; i < kombinacja.Length; i++)
            {
                if (!komorka.Contains(kombinacja[i]))
                    return false;
            }
            return true;
        }

        public static bool CzyZawieraWWierszu(int[][] wiersz, int[] kombinacja)
        {
            for (int i = 0; i < wiersz.Length; i++)
            {
                if (CzyKombinacjaWKomorce(wiersz[i], kombinacja))
                    return true;
            }
            return false;
        }

        public bool CzyRegulaZawieraInnaRegule(Regula r2)
        {
            foreach (var desk in r2.deskryptory)
            {
                if (!this.deskryptory.ContainsKey(desk.Key) || this.deskryptory[desk.Key] != desk.Value)
                    return false;
            }
            return true;
        }

      //  public bool CzyRegulaZawieraReguleInnegoRzedu(List<Regula> lista, Regula r1)
      ///  {
      //      foreach (var desk in lista)
      //      {
     //           if (!r1.deskryptory.ContainsKey(desk.Key) || r1.deskryptory[desk.Key] != desk.Value)
     //               return false;
    //        }
    //        return true;
    //   }

        public  static bool CzyRegulaZawieraReguleZListy(List<Regula> lista, Regula r)
        {
            for (int i = 0; i < lista.Count; i++)
            {
                if(!lista.Contains(r))
                {
                   return false;
                }
            }
            return true;
        }
        public Dictionary<int, string> deskryptory = new Dictionary<int, string>();
        public string decyzja;
        public int support;

        public override string ToString()
        {
            string wynik = string.Format("(a{0}={1})", this.deskryptory.First().Key + 1, this.deskryptory.First().Value);

            for (int i = 1; i < this.deskryptory.Count; i++)
            {
                var kvp = this.deskryptory.ElementAt(i);
                wynik += string.Format(" ^ (a{0}={1})", kvp.Key + 1, kvp.Value);
            }

            wynik += string.Format("=>(D={0})", this.decyzja);
            if (this.support > 1)
                wynik += $"[{this.support}]";

            return wynik;
        }
    }
}

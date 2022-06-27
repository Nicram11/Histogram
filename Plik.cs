using System.IO;
using System.Windows;

namespace Histogram
{
    public static class Plik //Klasa Statyczna, nie trzeba tworzyć obiektów tej klasy aby używac metod zawartych w nich.
    {
        public static DrzewoBinarne Czytaj(string sciezka, DrzewoBinarne drzewo) //Odczytuje z pliku txt wartości oraz umieszcza je w drzewie BST, zwraca Drzewo
        {
            int SkutecznieZapisaneLinijki = 0;
            int put;
            bool parse;
            StreamReader sr = new StreamReader(sciezka);
            using (sr)
            {
                while (!sr.EndOfStream)
                {
                    try
                    {
                        string line = sr.ReadLine();
                        line = line.Remove(line.IndexOf(";"));


                        parse = int.TryParse(line, out put);

                        if (parse)
                        {
                            SkutecznieZapisaneLinijki++;
                            drzewo.Put(put);
                        }
                    }
                    catch { }
                    
                }
            }
            if(SkutecznieZapisaneLinijki == 0)
            {
                MessageBox.Show("Nie odczytano ani jednej linijki, upewnij się że plik jest dobrze sformatowany");
                return null;
            }
            return drzewo;
        }

    }
}

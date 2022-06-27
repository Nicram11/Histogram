using LiveCharts.Wpf;
using System;
using System.ComponentModel;
using System.Windows.Media;

namespace Histogram
{
    public class Produkt : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;
        private void OnPropertyChanged(string s)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(s));

        }
        public string Nazwa { get; set; }

        public int LiczbaOcen { get; set; }

        public int ZakresOcen { get; set; }

        public string Zrodlo { get; set; }

        public Boolean CzyUkryty { get; set; } = false;

        public SolidColorBrush Kolor { get; set; }

        public Produkt(string nazwa, string zrodlo, DrzewoBinarne drzewo)
        {
            Nazwa = nazwa;
            LiczbaOcen = drzewo.LiczbaOcen();
            ZakresOcen = drzewo.Max();
            Zrodlo = zrodlo;
           
             Oceny = drzewo.ZamienNaTablice();

        }

        public Produkt(string nazwa, string zrodlo)
        {
            Nazwa = nazwa;
            DrzewoBinarne drzewo = new DrzewoBinarne();
            drzewo = Plik.Czytaj(zrodlo, drzewo);
            LiczbaOcen = drzewo.LiczbaOcen();
            ZakresOcen = drzewo.Max();
            Zrodlo = zrodlo;

            Oceny = drzewo.ZamienNaTablice();

        }

        public int[] Oceny { get; set; }
        public ColumnSeries seria { get; set; }
       public void UpdateKolory()
        {
            OnPropertyChanged(nameof(Kolor));
        }
    }
}

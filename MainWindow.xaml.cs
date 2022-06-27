using LiveCharts;
using LiveCharts.Wpf;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Histogram
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {



         Kolory kolor = new Kolory();
        bool czyRozdzielone = false;
        public SeriesCollection Series { get; set; } = new SeriesCollection();

        public ObservableCollection<Produkt> produkty { get; set; } = new ObservableCollection<Produkt>();

        public string NazwaNowegoProduktu { get; set; }

        private string nazwaPlikuTxt;

        public MainWindow()
        {
            
            InitializeComponent();

            DataContext = this;
            Czytaj_Xml();  //Wczytaj stan poprzednio zamknietej aplikacji

        }

        #region ImplementacjaInterfejsów

        // -- Interfejs pozwalający powiadomić widok Okna o zmianie wartości właściwości -- // 
        public event PropertyChangedEventHandler? PropertyChanged; 
        private void OnPropertyChanged(string s)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(s));

        }

        #endregion ImplementacjaInterfejsów



        private T FindVisualParent<T>(DependencyObject potomek) // Zwraca rodzica podanego w argumencie Elementu, Wywołuje sama siebie, do czasu znalezienia podanego-poszukiwanego T Elementu
          where T : DependencyObject
        {
            var rodzic = VisualTreeHelper.GetParent(potomek);
            if (rodzic == null)
                return null;
            T parent = rodzic as T;
            if (parent != null)
                return parent;
            return FindVisualParent<T>(rodzic);
        }

        private void Czytaj_Xml() //Metoda Wywoływana Przy stracie programu, Wczytująca stan aplikacji sprzed zamknięcia
        {

            produkty = Xml.Czytaj();
            foreach (Produkt produkt in produkty)
            {
                DodajDoSerii(produkt);
            }
            OnPropertyChanged(nameof(produkty));

        }


        private void DodajDoSerii(Produkt p) //Dodawanie stworzonego produktu do Serii(Wykresu)
        {
            ColumnSeries seria;
            // -- Ustawienie właściwości Serii
            if (czyRozdzielone)
                seria = new ColumnSeries
                {
                    Title = p.Nazwa,
                    Values = new ChartValues<int>(p.Oceny),
                    Fill = kolor.NastepnyKolor(),
                    ColumnPadding = 0

                };
            else
                seria =
                    new ColumnSeries
                    {
                        Title = p.Nazwa,
                        Values = new ChartValues<int>(p.Oceny),
                        Fill = kolor.NastepnyKolor(),


                    };

            Series.Add(seria); //dodanie serii do kolekcji Serii.
            p.Kolor = (SolidColorBrush)seria.Fill;
            p.seria = seria;
        }



        #region Zdarzenia


        private void Dodawanie_Produktu(object sender, RoutedEventArgs e) //Zdarzenie kliknięcia na przycisk Dodania Produktu
        {

            // -- Zapisz do BST dane odczytane z wybranego pliku -- //
            DrzewoBinarne drzewo = new DrzewoBinarne();
            drzewo = Plik.Czytaj(nazwaPlikuTxt, drzewo);

            // -- Jeżeli skuteczenie odczytano plik, utwórz nowy produkt, dodaj go do listy produktów oraz Serii wykresu -- //
            if (drzewo != null)
            {
                Produkt p = new Produkt(NazwaNowegoProduktu, nazwaPlikuTxt, drzewo);
                DodajDoSerii(p);
                produkty.Add(p);


            }
            NazwaNowegoProduktu = ""; // zresetuj zawartość textBlocka z wyborem nazwy produktu
            OnPropertyChanged(nameof(NazwaNowegoProduktu)); //poinformuj widok o zmianie(resecie) zawartości textBlocka z wyborem nazwy produktu
            PrzyciskDodaj.IsEnabled = false; //zablokuj przycisk do czasu wybrania nowego pliku 

        }


        public void Ukrywanie(object sender, RoutedEventArgs e) //Zdarzenie kliknięcia na przycisk ukrycia
        {
            // -- Znajdź produkt i serię powiązaną z klikniętym Buttonem, ukryj ją lub pokaż -- //
            var item = FindVisualParent<ListBoxItem>((DependencyObject)sender);
            Produkt produkt = item.Content as Produkt;
            produkt.seria.Visibility = produkt.seria.Visibility == Visibility.Visible ? Visibility.Hidden : Visibility.Visible;

            // -- Zmiana Obrazka przycisku ukryj/pokaż w zależności czy seria jest już ukryta czy nie -- //
            produkt.CzyUkryty = !produkt.CzyUkryty;
            Button przycisk = sender as Button;
            Image imgUkryj = new Image();
            imgUkryj.Source = new BitmapImage(new Uri(@"img/hide2.png", UriKind.RelativeOrAbsolute)); 
            Image imgOdkryj = new Image();
            imgOdkryj.Source = new BitmapImage(new Uri(@"img/unhide.png", UriKind.RelativeOrAbsolute));
            przycisk.Content = produkt.CzyUkryty ? imgOdkryj : imgUkryj;

        }

        private void Zapisz_Do_Pliku(object sender, RoutedEventArgs e) //Zdarzenie kliknięcia na przycisk zapisu produktu
        {
            // -- Znajdź produkt powiązany z klikniętym przyciskiem -- //
            var item = FindVisualParent<ListBoxItem>((DependencyObject)sender);
            Produkt produkt = item.Content as Produkt;

            // -- Zapisz dane o wybranym produkcie do pliku --//
            string path = produkt.Nazwa + ".txt";  // ścieżka w której produkt się zapisze (Opcjonalnie aby zapisać w dokumentach Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments))
            StreamWriter sw = new StreamWriter(path);
            sw.WriteLine("Nazwa Produktu: "+produkt.Nazwa);
            sw.WriteLine("źródło obliczeń: "+produkt.Zrodlo);
            sw.WriteLine("Łączna liczba wszystkich ocen: "+produkt.LiczbaOcen);
            for (int i = 0; i < produkt.Oceny.Length; i++)
                sw.WriteLine("Liczba Ocen o wyniku: " + i + " wynosiła:" + produkt.Oceny[i]);
            sw.Flush();
            MessageBox.Show("Wynik Zapisano w pliku tekstowym o nazwie "+produkt.Nazwa+" w folderze z Projektem");

        }
        private void Zmień_Kolor(object sender, RoutedEventArgs e) //Zdarzenie kliknięcia na przycisk zmiany koloru
        {
            // -- Otwórz okno Wyboru koloru -- //
            OknoWyboruKoloru oknoWyboruKoloru = new OknoWyboruKoloru();
            oknoWyboruKoloru.Show();
            // -- Znajdź produkt i serię powiązaną z klikniętym przyciskiem -- //
            var item = FindVisualParent<ListBoxItem>((DependencyObject)sender);
            Produkt produkt = item.Content as Produkt;
           
            // -- Do okna Wyboru Koloru dodaj zachowanie na zamykanie(Closing) okna zapisujące wybrany kolor --//
            void wybranie_Koloru(object sender, CancelEventArgs e)
            {
                Brush br = oknoWyboruKoloru.GetBrush();
                if (br != null) // Jeżeli kolor jest różny niż null, czyli wybranie koloru zakączono powodzeniem, Zmień kolory wybranego produktu i serii na wybrany
                {

                   
                    produkt.seria.Fill = br;  
                    produkt.Kolor = (SolidColorBrush)br;
                    produkt.UpdateKolory();
                }
            }
            oknoWyboruKoloru.Closing += wybranie_Koloru; //Podpięcie do okna zachowania;
            
        }

        private void Usuwanie(object sender, RoutedEventArgs e) //Zdarzenie kliknięcia na przycisk usuwania
        {
            // -- Znajdź produkt i serię powiązaną z klikniętym przyciskiem oraz je usuń-- //
            var item = FindVisualParent<ListBoxItem>((DependencyObject)sender);
            Produkt produkt = item.Content as Produkt;
            Series.Remove(produkt.seria);
            produkty.Remove(produkt);

        }


      


      
 
        private void Wybieranie_PLiku(object sender, RoutedEventArgs e) //zdarzenie na kliknięcie przycisku wyboru pliku
        {
            // -- otwórz okno dialogowe wyboru pliku, zapisz scieżkę do wybranego pliku --//
            var dialog = new Microsoft.Win32.OpenFileDialog();
            dialog.DefaultExt = ".txt";
            dialog.Filter = "Text documents (.txt)|*.txt";

            if (dialog.ShowDialog() == true) 
            {
                nazwaPlikuTxt = dialog.FileName;
                PrzyciskDodaj.IsEnabled = true;      //wybieranie pliku zakończono powodzeniem, odblokuj przycisk dodania produktu
                //  MessageBox.Show(nazwaPlikuTxt);
            }
            else
                MessageBox.Show("Wybrano niepoprawny plik");

        }


        private void Nowe_Okno_wykresu(object sender, RoutedEventArgs e) //zdarzenie na podwójnie klikniecie na wykres
        {
            // -- Otwórz okno nowego wykresu -- //
            OsobnyWykres window = new OsobnyWykres();
            window.Show();
            // -- Przenieś dataContext na nowe okno -- // 
            window.DataContext = this;
            // -- Ukryj wykres w głównym oknie oraz zmniejsz je aby wyświetlał się jedynie ListBox z produktami -- //
            Histogram.Visibility = Visibility.Collapsed; 
            MainWindow2.Width = 280;
            czyRozdzielone = true;
            // -- Zmień Odlegość pomiędzy kolumnami, dla każdej serii,  aby umożliwić wyświetlenie wiekszej liczby Danych -- //
            foreach (ColumnSeries column in Series)
            {
                column.ColumnPadding = 0;
            }

        } 
 
        void Closing_Event(object sender, CancelEventArgs e) //Zdarzenie na zamykanie okna
        {
            Xml.Zapisz(produkty); // Zapisz stan aplikacji w pliku Xml
        }

        #endregion
    }
}

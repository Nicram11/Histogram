using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Xml.Linq;

namespace Histogram
{
    public static class Xml //Klasa Statyczna, nie trzeba tworzyć obiektów tej klasy aby używac metod zawartych w nich.
    {
        private static string ścieżkaPliku = "produkty.xml";
        private static readonly IFormatProvider formatProvider = CultureInfo.InvariantCulture;
        public static void Zapisz(this ObservableCollection<Produkt> produkty) //Metoda serializująca informacje o produktach po zamknięciu Aplikacji
        {

            try
            {

                XDocument xml = new XDocument(
                    new XDeclaration("1.0", "utf-8", "yes"),
                    new XComment("Data zapisania: " + DateTime.Now.ToString(formatProvider)),
                    new XElement("Produkty",
                        from Produkt produkt in produkty
                        select new XElement("Produkt",
                            new XElement("Zrodlo", produkt.Zrodlo),
                            new XElement("Nazwa", produkt.Nazwa))));

                xml.Save(ścieżkaPliku);

            }
            catch (Exception exc)
            {
                MessageBox.Show("Błąd przy zapisie danych do pliku XML");
                throw new Exception("Błąd przy zapisie danych do pliku XML", exc);
            }

        }

        public static ObservableCollection<Produkt> Czytaj() //Metoda przywracająca stan kolekcji z produktami po ponownym otwarciu aplikacji
        {
            try
            {
                XDocument xml = XDocument.Load(ścieżkaPliku);
                IEnumerable<Produkt> dane =
                    from produkt in xml.Root.Descendants("Produkt")
                    select new Produkt(
                        produkt.Element("Nazwa").Value,
                        produkt.Element("Zrodlo").Value);

                ObservableCollection<Produkt> produkty = new ObservableCollection<Produkt>();
                foreach (Produkt produkt in dane) 
                    produkty.Add(produkt);
                return produkty;
            }
            catch (Exception exc)
            {
                throw new Exception("Błąd przy odczycie danych z pliku XML", exc);
            }
        }

    }
}

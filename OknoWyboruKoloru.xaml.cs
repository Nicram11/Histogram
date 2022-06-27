using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Histogram
{
    /// <summary>
    /// Logika interakcji dla klasy Kolor.xaml
    /// </summary>
    public partial class OknoWyboruKoloru : Window
    {
        public OknoWyboruKoloru()
        {
            InitializeComponent();
        }
        public Brush color;

        public Brush GetBrush()
        {
            return color;
        }    
        public void Wybranie_Koloru(object sender, RoutedEventArgs e)
        {
            Button b = sender as Button;
            color = b.Background;
           // MessageBox.Show(color.ToString());
            this.Close();   
        }

     
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Histogram
{
    public class Kolory
    {

        private int index = -1;

        public List<SolidColorBrush> kolory = new List<SolidColorBrush>()
        {
            (SolidColorBrush)new BrushConverter().ConvertFrom("#2195f2"),
            (SolidColorBrush)new BrushConverter().ConvertFrom("#f34336"),
            (SolidColorBrush)new BrushConverter().ConvertFrom("#fec007"),
            (SolidColorBrush)new BrushConverter().ConvertFrom("#607d8a"),
            (SolidColorBrush)new BrushConverter().ConvertFrom("#00bbd3"),
            (SolidColorBrush)new BrushConverter().ConvertFrom("#e81e63"),
            (SolidColorBrush)new BrushConverter().ConvertFrom("#fe5722"),
            (SolidColorBrush)new BrushConverter().ConvertFrom("#3f51b4"),
            (SolidColorBrush)new BrushConverter().ConvertFrom("#ccdb39"),
            (SolidColorBrush)new BrushConverter().ConvertFrom("#009587"),
            (SolidColorBrush)new BrushConverter().ConvertFrom("#4cae50"),
            (SolidColorBrush)new BrushConverter().ConvertFrom("#4cae50")

        };

        public SolidColorBrush NastepnyKolor()
        {

            if (index < 11)
                index++;
            else
                index = 0;


            return kolory[index];
       
        }


    }

}
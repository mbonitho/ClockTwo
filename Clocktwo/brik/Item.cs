using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;

namespace WPFBricks
{
    public interface IItem
    {
        //Propriétés
        double CoteGauche { get; }

        double CoteHaut { get; }

        double CoteDroit { get; }

        double CoteBas { get; }
    }
}

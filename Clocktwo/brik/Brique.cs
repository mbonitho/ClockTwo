using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows;
using System.Threading;

namespace WPFBricks
{
    public class Brique : IItem
    {
        //Champs privés
        private Rectangle _forme;

        //Propriétés
        public double CoteGauche
        {
            get { return this._forme.Margin.Left; }
        }

        public double CoteHaut
        {
            get { return this._forme.Margin.Top; }
        }

        public double CoteDroit
        {
            get { return this._forme.Margin.Left + this._forme.Width; }
        }

        public double CoteBas
        {
            get { return this._forme.Margin.Top + this._forme.Height; }
        }


        public Rectangle Forme
        {
            get { return this._forme; }
        }

        public bool Visible
        {
            get { return this._forme.Visibility == System.Windows.Visibility.Visible; }
            set
            {
                if (value)
                    this._forme.Visibility = System.Windows.Visibility.Visible;
                else
                    this._forme.Visibility = System.Windows.Visibility.Hidden;
            }
        }

        public bool Cassable { get; set; }

        //Constructeur
        public Brique(Grid parent, double posX, double posY, bool cassable)
        {
            //Création de l'ellipse
            this._forme = new Rectangle();

            //Ajout au parent
            parent.Children.Add(this._forme);

            //Taille de la Brique
            this._forme.Height = 20;
            this._forme.Width = 60;

            //Position de la Brique
            this._forme.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this._forme.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this._forme.Margin = new System.Windows.Thickness(posX, posY, 0, 0);

            this.Cassable = cassable;

            //Couleur de la Brique
            //Couleur de fond
            Thread.Sleep(1);
            Random R = new Random(unchecked((int)DateTime.Now.Ticks));
            SolidColorBrush couleurFond = new SolidColorBrush();
            byte Red = (byte)(R.Next(253) + 1);
            byte Green = (byte)(R.Next(253) + 1);
            byte Blue = (byte)(R.Next(253) + 1);
            //Noire si incassable
            couleurFond.Color = cassable ? Color.FromRgb(Red, Green, Blue) : Color.FromRgb(0, 0, 0);
            this._forme.Fill = couleurFond;
            //Couleur de bordure
            SolidColorBrush couleurBord = new SolidColorBrush();
            couleurBord.Color = Color.FromRgb(255, 255, 255);
            this._forme.Stroke = couleurBord;
        }
    }
}

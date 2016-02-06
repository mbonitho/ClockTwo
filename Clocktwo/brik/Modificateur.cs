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
    public class Modificateur : IItem
    {
        //Champs privés
        private Rectangle _forme;
        private int _vitesse;

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

        public bool Visible
        {
            get { return this._forme.Visibility == Visibility.Visible; }
            set { this._forme.Visibility = value ? Visibility.Visible : Visibility.Hidden; }
        }

        public Rectangle Forme
        {
            get { return this._forme; }
        }

        //Constructeur standard
        public Modificateur(Grid parent, double posX, double posY)
        {
            this._vitesse = 7;

            //Création de l'ellipse
            this._forme = new Rectangle();

            //Ajout au parent
            parent.Children.Add(this._forme);

            //Taille du modificateur
            this._forme.Height = 30;
            this._forme.Width = 30;

            //Position du modificateur
            this._forme.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this._forme.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this._forme.Margin = new System.Windows.Thickness(posX, posY, 0, 0);

            //Couleur du modificateur
            //Couleur de fond
            Random R = new Random(unchecked((int)DateTime.Now.Ticks));
            SolidColorBrush couleurFond = new SolidColorBrush();
            byte Red = (byte)R.Next(255);
            byte Green = (byte)R.Next(255);
            byte Blue = (byte)R.Next(255);
            Thread.Sleep(1);
            couleurFond.Color = Color.FromRgb(Red, Green, Blue);
            this._forme.Fill = couleurFond;

            //Couleur de bordure
            SolidColorBrush couleurBord = new SolidColorBrush();
            couleurBord.Color = Color.FromRgb(255, 255, 255);
            this._forme.Stroke = couleurBord;
        }

        public static Modificateur GetNewModificateur(Grid parent, Brique laBrique)
        {
            return new Modificateur(parent, laBrique.CoteGauche, laBrique.CoteBas);
        }

        public void Deplace()
        {
            this._forme.Margin = new Thickness(this._forme.Margin.Left, this._forme.Margin.Top + _vitesse, this._forme.Margin.Right, this._forme.Margin.Bottom);
        }
    }
}

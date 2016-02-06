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
    public class Raquette : IItem
    {
        //Champs privés
        private Rectangle _forme;

        //Propriétés
        public int Vitesse { get; set; }
        public bool touchesInversees { get; set; }

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

        public double Width
        {
            get { return this._forme.Width; }
            set { this._forme.Width = value; }
        }

        public Rectangle Forme
        {
            get { return this._forme; }
        }

        //Constructeur
        public Raquette(Grid parent, double ScreenSizeX, double ScreenSizeY)
        {
            //<Rectangle Height="20" HorizontalAlignment="Left" Margin="100,285,0,0" Name="raquette" Stroke="Black" 
            //VerticalAlignment="Top" Width="100" Fill="#FF0000E1" />

            //Création du rectangle
            this._forme = new Rectangle();

            //Ajout au parent
            parent.Children.Add(this._forme);

            //Taille de la Raquette
            this._forme.Height = 20;
            this._forme.Width = 100;

            //Initialisation de la vitesse
            this.Vitesse = 5;

            //Touches non inversées par défaut
            this.touchesInversees = false;

            //Position de la Raquette
            this._forme.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this._forme.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this._forme.Margin = new System.Windows.Thickness(ScreenSizeX / 2 - this._forme.Width / 2, ScreenSizeY - 3 * this._forme.Height, 0, 0);

            //Couleur de la Raquette
            //Couleur de fond
            SolidColorBrush couleurFond = new SolidColorBrush();
            Random R = new Random(unchecked((int)DateTime.Now.Ticks));
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

        public void Bouge(int direction)
        {
            if (direction < 0) //Gauche
                this._forme.Margin = new Thickness(this._forme.Margin.Left - this.Vitesse, this._forme.Margin.Top, this._forme.Margin.Right, this._forme.Margin.Bottom);
            else //Droite
                this._forme.Margin = new Thickness(this._forme.Margin.Left + this.Vitesse, this._forme.Margin.Top, this._forme.Margin.Right, this._forme.Margin.Bottom);

        }
    }
}

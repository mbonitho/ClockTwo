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
    public class Balle : IItem
    {
        //Champs privés
        private Ellipse _forme;

        //Propriétés
        public double VitesseX { get; set; }
        public double VitesseY { get; set; }
        public bool Percante { get; set; }

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

        public Ellipse Forme
        {
            get { return this._forme; }
        }

        //Constructeur
        public Balle(Grid parent, double posX, double posY)
        {
            //Height="24" HorizontalAlignment="Left" Margin="116,120,0,0" Name="balle" Stroke="Black" VerticalAlignment="Top" Width="24" Fill="#FFFA0000"

            //Création de l'ellipse
            this._forme = new Ellipse();

            //Ajout au parent
            parent.Children.Add(this._forme);

            //Taille de la balle
            this._forme.Height = 24;
            this._forme.Width = 24;

            //Position de la balle
            this._forme.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this._forme.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this._forme.Margin = new System.Windows.Thickness(posX, posY, 0, 0);

            //Couleur de la balle
            //Couleur de fond
            Random R = new Random(unchecked((int)DateTime.Now.Ticks));
            SolidColorBrush couleurFond = new SolidColorBrush();
            Thread.Sleep(1);
            byte Red = (byte)R.Next(255);
            byte Green = (byte)R.Next(255);
            byte Blue = (byte)R.Next(255);
            couleurFond.Color = Color.FromRgb(Red, Green, Blue);
            this._forme.Fill = couleurFond;
            //Couleur de bordure
            SolidColorBrush couleurBord = new SolidColorBrush();
            couleurBord.Color = Color.FromRgb(255, 255, 255);
            this._forme.Stroke = couleurBord;

            //Initialisation de la vitesse de la balle
            this.VitesseX = 5;
            this.VitesseY = -5;

            //De base, la balle n'est pas percante
            this.Percante = false;
        }

        //Déplace la balle selon sa vitesse
        public void Deplace()
        {
            this._forme.Margin = new Thickness(this._forme.Margin.Left - VitesseX, this._forme.Margin.Top - VitesseY, this._forme.Margin.Right, this._forme.Margin.Bottom);
        }


        public void Reinit(double posX, double posY)
        {
            //vitesse descendante
            if (this.VitesseY > 0)
                this.VitesseY *= -1;

            //Réinitialisation de la position de la balle
            this._forme.Margin = new Thickness(posX, posY, 0, 0);

            //Initialisation de la vitesse de la balle
            this.VitesseX = 5;
            this.VitesseY = -5;

            //De base, la balle n'est pas percante
            this.Percante = false;

            //Réinitialisation d'une couleur
            SolidColorBrush couleurFond = new SolidColorBrush();
            Thread.Sleep(1);
            Random R = new Random(unchecked((int)DateTime.Now.Ticks));
            byte Red = (byte)R.Next(255);
            byte Green = (byte)R.Next(255);
            byte Blue = (byte)R.Next(255);
            couleurFond.Color = Color.FromRgb(Red, Green, Blue);
            this._forme.Fill = couleurFond;
        }

        internal void ReplaceEnY(double nouveauY)
        {
            this._forme.Margin = new Thickness(this._forme.Margin.Left, nouveauY, this._forme.Margin.Right, this._forme.Margin.Bottom);
        }
    }
}

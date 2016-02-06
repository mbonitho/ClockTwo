using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using System.Threading;

namespace AssistantPersonnel
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Point mousePosDown = new Point();
        Point mousePosMove = new Point();

        UserControl mainComponent;

        public Clocktwo clockComp { get; set; }

        public WPFBricks.BrikControl brikComp { get; set; }

        public MainWindow()
        {
            InitializeComponent();

            //Ajout du composant clocktwo
            clockComp = new Clocktwo();
            this.mainGrid.Children.Add(clockComp);
            this.mainComponent = clockComp;
        }

        private void Window_MouseDown(object sender, MouseButtonEventArgs e)
        {
            //récupération de la position de la souris
            mousePosDown.X = e.GetPosition(this.mainGrid).X;
            mousePosDown.Y = e.GetPosition(this.mainGrid).Y;
        }

        private void Window_MouseMove(object sender, MouseEventArgs e)
        {
            //si la souris bouge
            if (e.LeftButton == MouseButtonState.Pressed)
            {
                mousePosMove.X = e.GetPosition(this.mainGrid).X;
                mousePosMove.Y = e.GetPosition(this.mainGrid).Y;

                double deplX = mousePosDown.X - mousePosMove.X;

                //Déplacement du mainComponent
                this.mainComponent.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
                this.mainComponent.Margin = new Thickness(-deplX, 0, 0, 0);
                this.mainComponent.Width = 500;

                //déplacement de la souris vers la gauche avec bouton gauche enfoncé
                if (this.mainComponent.Margin.Left <= -350)
                {
                    for (int i = 0; i < 250; i++)
                    {
                        this.mainComponent.Margin = new Thickness(this.mainComponent.Margin.Left - 1, 0, 0, 0);
                    }
                    this.mainGrid.Children.Remove(mainComponent);

                    //ajout brikComponent
                    ajoutBrikComponent();
                }
            }
        }

        private void ajoutBrikComponent()
        {
            if (this.brikComp == null)
            {
                //Ajout du composant brik
                brikComp = new WPFBricks.BrikControl(this);
            }

            this.mainGrid.Children.Add(brikComp);
            this.mainComponent = brikComp;

            deplacementMainComponent();
        }

        private void Window_MouseUp(object sender, MouseButtonEventArgs e)
        {
            deplacementMainComponent();
        }

        private void deplacementMainComponent()
        {
            //Déplacement du mainComponent
            this.mainComponent.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            this.mainComponent.Margin = new Thickness(0, 0, 0, 0);
            this.mainComponent.Width = 500;
            this.mainComponent.Height = 500;
        }

    }
}

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
using System.Windows.Threading;
using System.Threading;

namespace WPFBricks
{
    /// <summary>
    /// Logique d'interaction pour MainWindow.xaml
    /// </summary>
    public partial class BrikControl : UserControl
    {
        //Contrôle parent du composant
        public Window FenParent { get; set; }
        
        //Propriétés
        public Raquette laRaquette { get; set; }
        public List<Balle> lesBalles { get; set; }
        public List<Brique> lesBriques { get; set; }
        public int nbBriquesRestantes { get; set; }
        public List<Modificateur> lesModificateurs { get; set; }

        public int niveau { get; set; }
        public int score { get; set; }
        public int nbVies { get; set; }
        public DispatcherTimer IHMTimer { get; set; }

        //Attributs privés
        private Label lblGo;
        private Label lblTitre;
        private Label lblAuteur;
        private Label lblChoixNiv;
        private TextBox txtChoixNiv;

        //*** AJOUT DE FONCTIONNALITES
        //TODO effets sonores + mute on/off ==>  midis dans dossier data
        //TODO choix de résolutions
        //TODO limiter la raquette dans le terrain
        //TODO déplacer le label score dans le code et non dans le concepteur de vues pour pouvoir clear facilement l'affichage

        //*** DEBOGGAGE
        //TODO Passage au niveau suivant : balle sur raquette

        //Constructeur
        public BrikControl(Window fenParent)
        {
            InitializeComponent();

            this.FenParent = fenParent;

            //Initialisation de la liste des modificateurs pour eviter une nullref exc
            lesModificateurs = new List<Modificateur>();

            InitialiseMenu();
        }

        //Initialise le menu
        private void InitialiseMenu()
        {
            //Ajout du clavier
            this.FenParent.KeyDown += this.BrikCompOnMenuKeyDown;

            //Couleur de texte des labels
            SolidColorBrush couleurPolice = new SolidColorBrush();
            couleurPolice.Color = Color.FromRgb(255, 255, 255);

            lblScore.Visibility = System.Windows.Visibility.Hidden;

            //Propriétés du label titre
            lblTitre = new Label();
            lblTitre.Content = "WPF BREAKOUT";
            lblTitre.FontSize = 42;
            lblTitre.FontWeight = FontWeights.ExtraBlack;
            lblTitre.Foreground = couleurPolice;
            lblTitre.Margin = new Thickness(70, 0, 0, 0);

            //Propriétés du label go
            lblGo = new Label();
            lblGo.Content = "GO !";
            lblGo.FontSize = 56;
            lblGo.FontWeight = FontWeights.Black;
            lblGo.Foreground = couleurPolice;
            lblGo.Margin = new Thickness(160, 150, 0, 0);
            lblGo.MouseUp += this.lblGo_MouseUp;
            lblGo.MouseEnter += this.lblGo_MouseEnter;
            lblGo.MouseLeave += this.lblGo_MouseLeave;

            //Propriétés du label auteur
            lblAuteur = new Label();
            lblAuteur.Content = "2014, Mathieu BONITHON";
            lblAuteur.FontSize = 16;
            lblAuteur.FontWeight = FontWeights.Light;
            lblAuteur.Foreground = couleurPolice;
            lblAuteur.Margin = new Thickness(280, 310, 0, 0);

            //Propriétés du label choix du niveau
            lblChoixNiv = new Label();
            lblChoixNiv.Content = "Choix du niveau :";
            lblChoixNiv.FontSize = 16;
            lblChoixNiv.FontWeight = FontWeights.Light;
            lblChoixNiv.Foreground = couleurPolice;
            lblChoixNiv.Margin = new Thickness(130, 210, 0, 0);

            //Propriétés de la textBox choix du niveau
            txtChoixNiv = new TextBox();
            txtChoixNiv.Text = "1";
            txtChoixNiv.FontSize = 16;
            txtChoixNiv.FontWeight = FontWeights.Black;
            txtChoixNiv.Width = 30;
            txtChoixNiv.Height = 30;
            txtChoixNiv.Margin = new Thickness(50, 100, 0, 0);

            //Ajouts des contrôles à l'image
            MainGrid.Children.Add(lblTitre);
            MainGrid.Children.Add(lblGo);
            MainGrid.Children.Add(lblAuteur);
            MainGrid.Children.Add(lblChoixNiv);
            MainGrid.Children.Add(txtChoixNiv);
        }

        //Initialise les différents objets
        private void InitialisePartie()
        {
            //Avant toute chose, on enlève le handler de démarrage de partie
            this.FenParent.KeyDown -= this.BrikCompOnMenuKeyDown;

            //Retrait des labels de l'image (maingrid.children.clear ?)
            MainGrid.Children.Remove(lblTitre);
            MainGrid.Children.Remove(lblGo);
            MainGrid.Children.Remove(lblAuteur);
            MainGrid.Children.Remove(lblChoixNiv);
            MainGrid.Children.Remove(txtChoixNiv);

            //Score visible
            lblScore.Visibility = System.Windows.Visibility.Visible;

            //Branchement du clavier
            this.FenParent.KeyDown += new KeyEventHandler(this.BrikCompOnGameKeyDown);

            //Initialisation de la raquette
            this.laRaquette = new Raquette(this.MainGrid, this.Width / 2, this.Height);

            //Initialisation de la balle
            this.lesBalles = new List<Balle>();
            Balle b = new Balle(this.MainGrid, this.laRaquette.CoteGauche + this.laRaquette.Forme.Width / 2, this.laRaquette.CoteHaut);
            b.ReplaceEnY(this.laRaquette.CoteHaut - b.Forme.Height);
            lesBalles.Add(b);

            //Attributs
            try
            {
                this.niveau = int.Parse(txtChoixNiv.Text);
            }
            catch (Exception)
            {
                this.niveau = 1;
            }

            this.nbVies = 3;

            //Création des différentes Briques
            CreeBriques(this.niveau.ToString());

            //Création du timer d'IHM
            IHMTimer = new DispatcherTimer();
            IHMTimer.Tick += new EventHandler(IHMTimer_Tick);
            IHMTimer.Interval = new TimeSpan(0, 0, 0, 0, 50);
            IHMTimer.Start();
        }

        //Clic sur le lblGo
        private void lblGo_MouseUp(object sender, MouseButtonEventArgs e)
        {
            InitialisePartie(); //Passe en mode normal
        }

        //Survol du Go
        private void lblGo_MouseEnter(object sender, MouseEventArgs e)
        {
            //Couleur aléatoire
            SolidColorBrush couleurFond = new SolidColorBrush();
            Random R = new Random(unchecked((int)DateTime.Now.Ticks));
            Thread.Sleep(1);
            byte Red = (byte)R.Next(255);
            byte Green = (byte)R.Next(255);
            byte Blue = (byte)R.Next(255);
            couleurFond.Color = Color.FromRgb(Red, Green, Blue);
            lblGo.Foreground = couleurFond;
        }

        //Quitte le go
        private void lblGo_MouseLeave(object sender, MouseEventArgs e)
        {
            //Couleur blanche
            SolidColorBrush couleurFond = new SolidColorBrush();
            couleurFond.Color = Color.FromRgb(255, 255, 255);
            lblGo.Foreground = couleurFond;
        }

        //Génère les différentes briques selon fichier txt specifié
        private void CreeBriques(string numNiveau)
        {
            nbBriquesRestantes = 0;
            lesBriques = new List<Brique>();

            try
            {
                //ligne lue
                string ligne;

                //Marge X et Y (en haut et à gauche)
                int margeX = 25;
                int margeY = 5;

                System.IO.StreamReader fichier = new System.IO.StreamReader(@"data\" + numNiveau);

                // Lecture du fichier ligne par ligne
                int i = 0;
                int j = 0;
                while ((ligne = fichier.ReadLine()) != null)
                {
                    i = 0;
                    var charTab = ligne.ToCharArray();
                    foreach (char c in charTab)
                    {
                        switch (c)
                        {
                            case '-':
                                //Positionnement d'une brique normale
                                Brique b1 = new Brique(MainGrid, i * 60 + margeX, j * 20 + margeY, true);
                                //Ajout des briques à la liste
                                lesBriques.Add(b1);
                                nbBriquesRestantes++;
                                break;

                            case '*':
                                //Positionnement d'une brique incassable
                                Brique b2 = new Brique(MainGrid, i * 60 + margeX, j * 20 + margeY, false);
                                //Ajout des briques à la liste
                                lesBriques.Add(b2);
                                break;

                            default:
                                break;
                        }
                        i++;
                    }
                    j++;
                }

                //Fermeture du fichier
                fichier.Close();
            }
            catch (Exception ex) 
            {
                throw new Exception("Pas cool : " + ex.ToString());
            }
        }

        private void BrikCompOnMenuKeyDown(object sender, KeyEventArgs e)
        {
            //Espace ou enter : commencer
            if (e.Key == Key.Enter || e.Key == Key.Space)
            {
                InitialisePartie(); //Passe en mode normal
            }
        }

        //Gère la saisie clavier on game
        private void BrikCompOnGameKeyDown(object sender, KeyEventArgs e)
        {
            //Flèches : déplacement de la raquette
            if (e.Key == Key.Left && this.IHMTimer.IsEnabled)
                this.laRaquette.Bouge(this.laRaquette.touchesInversees ? 1 : -1);

            if (e.Key == Key.Right && this.IHMTimer.IsEnabled)
                this.laRaquette.Bouge(this.laRaquette.touchesInversees ? -1 : 1);

            //Espace : mettre en pause
            if (e.Key == Key.Space)
                this.IHMTimer.IsEnabled = !this.IHMTimer.IsEnabled;
        }

        //Applique un effet aléatoire à la raquette ou à la balle
        private void AppliqueEffetModificateur()
        {
            Random R = new Random();
            double hasard = R.NextDouble();

            if (hasard < 0.1)
                this.laRaquette.Width += 10;
            else if (hasard < 0.2)
                this.laRaquette.Width -= 10;
            else if (hasard < 0.3)
                this.laRaquette.Vitesse += 2;
            else if (hasard < 0.4)
                this.laRaquette.Vitesse -= 2;
            else if (hasard < 0.5)
            {
                Balle b = new Balle(this.MainGrid, this.laRaquette.CoteGauche + this.laRaquette.Forme.Width / 2, this.laRaquette.CoteHaut);
                b.ReplaceEnY(this.laRaquette.CoteHaut - b.Forme.Height);
                this.lesBalles.Add(b);
            }
            else if (hasard < 0.6)
                foreach (Balle b in this.lesBalles)
                    b.Percante = true;
            else if (hasard < 0.7)
                foreach (Balle b in this.lesBalles)
                {
                    b.VitesseX -= 0.5;
                    b.VitesseY -= 0.5;
                }
            else if (hasard < 0.8)
                foreach (Balle b in this.lesBalles)
                {
                    b.VitesseX += 0.5;
                    b.VitesseY += 0.5;
                }
            else if (hasard < 0.9)
                this.nbVies++;
            else
                this.laRaquette.touchesInversees = !this.laRaquette.touchesInversees;
            //feu
        }

        //Détecte si les 2 rectangles passés en paramètre sont en collision
        private bool CollisionEntreItems(IItem brique1, IItem brique2)
        {
            ////le code ci-dessous teste l'absorption de brique1 par brique2
            //if (brique1.CoteGauche >= brique2.CoteGauche
            // && brique1.CoteDroit <= brique2.CoteDroit
            // && brique1.CoteBas >= brique2.CoteHaut
            // && brique1.CoteHaut <= brique2.CoteBas)
            //    return true;
            //else
            //    return false;

            if (brique1.CoteGauche > brique2.CoteDroit
                || brique1.CoteDroit < brique2.CoteGauche
                || brique1.CoteHaut > brique2.CoteBas
                || brique1.CoteBas < brique2.CoteHaut)
                return false;
            else
                return true;
        }

        private bool CollisionParLeHautOuLeBas(IItem brique1, IItem brique2)
        {
            return (CollisionEntreItems(brique1, brique2) && (brique1.CoteBas > brique2.CoteBas) || brique1.CoteHaut < brique2.CoteHaut);
        }

        private bool CollisionParLaGaucheOuLaDroite(Balle balle, IItem brique)
        {
            return (CollisionEntreItems(balle, brique) && (balle.CoteGauche < brique.CoteGauche) || balle.CoteDroit > brique.CoteDroit);
        }

        //Méthode de rafraichissement d'IHM : boucle principale
        private void IHMTimer_Tick(object sender, EventArgs e)
        {
            //***************************************************************
            //Détection du nombre de vies pour GO
            //***************************************************************
            if (this.nbVies < 0)
            {
                //On cache la balle
                foreach (Balle b in this.lesBalles)
                {
                    b.Forme.Visibility = System.Windows.Visibility.Hidden;
                }

                //Propriétés du label
                Label lblGO = new Label();
                lblGO.Content = "GAME OVER";
                lblGO.FontSize = 36;
                lblGO.FontWeight = FontWeights.ExtraBlack;

                //Couleur de texte du label
                SolidColorBrush couleurFond = new SolidColorBrush();
                couleurFond.Color = Color.FromRgb(255, 255, 255);
                lblGO.Foreground = couleurFond;

                //Positionnement
                lblGO.Margin = new Thickness(150, 150, 0, 0);

                MainGrid.Children.Add(lblGO);

                //désactivation du timer
                this.IHMTimer.IsEnabled = false;
                return;
            }

            //***************************************************************
            //Déplacement des bonus vers le bas et suppression une fois le bas de l'écran dépassé
            //***************************************************************
            List<Modificateur> copieListeModificateurs = lesModificateurs;
            for (int i = 0; i < copieListeModificateurs.Count; i++)
            {
                Modificateur item = copieListeModificateurs[i];
                item.Deplace();

                if (item.CoteHaut > this.Height)
                    lesModificateurs.RemoveAt(i);
            }

            //***************************************************************
            //Déplacement de la balle à chaque tick
            //***************************************************************
            List<Balle> copieListeBalles = this.lesBalles;
            for (int cpt = 0; cpt < copieListeBalles.Count; cpt++)
            {
                Balle b = copieListeBalles[cpt];

                b.Deplace();

                //détection du bord gauche de l'écran
                if (b.CoteGauche <= 0)
                    b.VitesseX *= -1;

                //détection du bord haut de l'écran
                if (b.CoteHaut <= 0)
                    b.VitesseY *= -1.1;

                //détection du bord droit de l'écran
                if (b.CoteDroit >= this.Width)
                    b.VitesseX *= -1;

                //***************************************************************
                //détection du bord bas de l'écran
                //***************************************************************
                if (b.CoteHaut >= this.Height)
                {
                    //baisse du score
                    this.score -= 1000;

                    //Réinitialisation de la balle (juste la 1ere)
                    if (cpt < 1)
                    {
                        b.Reinit(this.Width / 2, this.laRaquette.CoteHaut - b.Forme.Height);

                        //Décrémentation d'une vie
                        this.nbVies--;

                        //Réinitialisation de la raquette (pour annuler tous les effets)
                        MainGrid.Children.Remove(laRaquette.Forme);
                        this.laRaquette = new Raquette(this.MainGrid, this.Width, this.Height);
                    }
                    else
                    {
                        //Suppression de cette balle qui était issue d'un modificateur
                        MainGrid.Children.Remove(b.Forme);
                        lesBalles.Remove(b);
                    }
                }

                //***************************************************************
                //détection d'une collision avec la raquette
                //***************************************************************
                if (CollisionEntreItems(b, this.laRaquette))
                {
                    //Déplacement en Y de la balle pour qu'elle ne soit pas captive de la raquette
                    b.ReplaceEnY(laRaquette.Forme.Margin.Top - b.Forme.Height);
                    b.VitesseY *= -1.1;
                }

                //***************************************************************
                //détection d'une collision avec une brique
                //***************************************************************
                int nbCollisionsVerticales = 0;
                int nbCollisionsHorizontales = 0;
                for (int i = 0; i < lesBriques.Count; i++)
                {
                    Brique briq = lesBriques[i];

                    if (briq.Visible && CollisionEntreItems(b, briq))
                    {
                        if (briq.Cassable)
                        {
                            //On casse la brique
                            MainGrid.Children.Remove(briq.Forme);
                            lesBriques.Remove(briq);
                            nbBriquesRestantes--;

                            //On augmente le score
                            this.score += 500;

                            //Obtention possible d'un modificateur
                            Random r = new Random();
                            if (r.NextDouble() > 0.1)
                                lesModificateurs.Add(Modificateur.GetNewModificateur(MainGrid, briq));

                            //Vérification du nombre de briques restantes
                            if (nbBriquesRestantes == 0)
                            {
                                //******************************************************
                                //S'il n'y a plus de briques, passage au niveau suivant
                                //******************************************************

                                //Vidage du tableau, qui ne contient plus que des briques noires
                                for (int k = 0; k < lesBriques.Count; k++)
                                {
                                    Brique b2 = lesBriques[k];
                                    MainGrid.Children.Remove(b2.Forme);
                                }
                                lesBriques.Clear();

                                this.niveau++;
                                CreeBriques(this.niveau.ToString());
                            }
                        }

                        //On inverse la vitesse de la balle (sauf si elle est perçante)
                        if (CollisionParLeHautOuLeBas(b, briq) && !b.Percante)
                            nbCollisionsVerticales++;

                        if (CollisionParLaGaucheOuLaDroite(b, briq) && !b.Percante)
                            nbCollisionsHorizontales++;
                    }
                }

                if (nbCollisionsVerticales > 0)
                    b.VitesseY *= -1.1;

                if (nbCollisionsHorizontales > 0)
                    b.VitesseX *= -1;

                //***************************************************************
                //Détection d'une collision avec un modificateur
                //***************************************************************
                copieListeModificateurs = lesModificateurs;
                for (int i = 0; i < copieListeModificateurs.Count; i++)
                {

                    Modificateur unModificateur = copieListeModificateurs[i];

                    if (CollisionEntreItems(unModificateur, laRaquette))
                    {
                        //On augmente le score
                        this.score += 500;

                        //Obtention d'un effet aléatoire
                        this.AppliqueEffetModificateur();

                        //après la collision, suppression du modificateur de la grille
                        MainGrid.Children.Remove(unModificateur.Forme);
                        lesModificateurs.RemoveAt(i);
                    }
                }
            }

            //***************************************************************
            //Mise à jour de l'affichage du score, des vies et du niveau
            //***************************************************************
            lblScore.Content = String.Format("Score : {0}\nVies  : {1}\nNiv.  : {2}", this.score, Math.Max(this.nbVies, 0), this.niveau);
        }
    }
}

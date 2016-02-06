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

namespace AssistantPersonnel
{
    /// <summary>
    /// Logique d'interaction pour clocktwo.xaml
    /// </summary>
    public partial class Clocktwo : UserControl
    {
        //Constantes de mots
        private Label[] IL;
        private Label[] EST;
        private Label[] UNE_HEURE;
        private Label[] DEUX_HEURES;
        private Label[] TROIS_HEURES;
        private Label[] QUATRE_HEURES;
        private Label[] CINQ_HEURES;
        private Label[] SIX_HEURES;
        private Label[] SEPT_HEURES;
        private Label[] HUIT_HEURES;
        private Label[] NEUF_HEURES;
        private Label[] DIX_HEURES;
        private Label[] ONZE_HEURES;
        private Label[] MIDI;
        private Label[] MINUIT;
        private Label[] HEURE;
        private Label[] HEURES;

        private Label[] MOINS;
        private Label[] LE;
        private Label[] ET1;
        private Label[] CINQ;
        private Label[] DIX;
        private Label[] QUART;
        private Label[] VINGT;
        private Label[] VINGTCINQ;
        private Label[] ET2;
        private Label[] DEMIE;

        private Label[] AM;
        private Label[] PM;

        private List<Label[]> labelsAllumes;

        public DispatcherTimer IHMTimer { get; set; }

        public Clocktwo()
        {
            InitializeComponent();

            //initialisation de la list de tableaux de labels à allumer
            labelsAllumes = new List<Label[]>();

            InitialiseRaccourcis();


            //création du timer d'ihm
            IHMTimer = new DispatcherTimer();
            IHMTimer.Tick += new EventHandler(IHMTimer_Tick);
            IHMTimer.Interval = new TimeSpan(0, 0, 40);
            IHMTimer.Start();

            afficheHeure();
        }


        private void InitialiseRaccourcis()
        {
            //Initialisation des raccourcis
            IL = new Label[] { lbl0, lbl1 };
            EST = new Label[] { lbl3, lbl4, lbl5 };
            UNE_HEURE = new Label[] { lbl26, lbl27, lbl28 };
            DEUX_HEURES = new Label[] { lbl7, lbl8, lbl9, lbl10 };
            TROIS_HEURES = new Label[] { lbl17, lbl18, lbl19, lbl20, lbl21 };
            QUATRE_HEURES = new Label[] { lbl11, lbl12, lbl13, lbl14, lbl15, lbl16 };
            CINQ_HEURES = new Label[] { lbl40, lbl41, lbl42, lbl43 };
            SIX_HEURES = new Label[] { lbl37, lbl38, lbl39 };
            SEPT_HEURES = new Label[] { lbl29, lbl30, lbl31, lbl32 };
            HUIT_HEURES = new Label[] { lbl33, lbl34, lbl35, lbl36 };
            NEUF_HEURES = new Label[] { lbl22, lbl23, lbl24, lbl25 };
            DIX_HEURES = new Label[] { lbl46, lbl47, lbl48 };
            ONZE_HEURES = new Label[] { lbl55, lbl56, lbl57, lbl58 };
            MIDI = new Label[] { lbl44, lbl45, lbl46, lbl47 };
            MINUIT = new Label[] { lbl49, lbl50, lbl51, lbl52, lbl53, lbl54 };
            HEURE = new Label[] { lbl60, lbl61, lbl62, lbl63, lbl64 };
            HEURES = new Label[] { lbl60, lbl61, lbl62, lbl63, lbl64, lbl65 };

            MOINS = new Label[] { lbl66, lbl67, lbl68, lbl69, lbl70 };
            LE = new Label[] { lbl72, lbl73 };
            ET1 = new Label[] { lbl77, lbl78 };
            CINQ = new Label[] { lbl94, lbl95, lbl96, lbl97 };
            DIX = new Label[] { lbl74, lbl75, lbl76 };
            QUART = new Label[] { lbl80, lbl81, lbl82, lbl83, lbl84 };
            VINGT = new Label[] { lbl88, lbl89, lbl90, lbl91, lbl92 };
            VINGTCINQ = new Label[] { lbl88, lbl89, lbl90, lbl91, lbl92, lbl93, lbl94, lbl95, lbl96, lbl97 };
            ET2 = new Label[] { lbl99, lbl100 };
            DEMIE = new Label[] { lbl102, lbl103, lbl104, lbl105, lbl106 };

            AM = new Label[] { lbl108, lbl109 };
            PM = new Label[] { lbl86, lbl87 };
        }

        private void IHMTimer_Tick(object sender, EventArgs e)
        {
            afficheHeure();
        }

        private void afficheHeure()
        {
            //Extinction des labels allumés
            foreach (Label[] labels in labelsAllumes)
            {
                foreach (Label label in labels)
                {
                    SolidColorBrush scb = new SolidColorBrush(Color.FromRgb(56, 56, 56));
                    label.Foreground = scb;
                }
            }

            //vidage de la liste de labels
            labelsAllumes.Clear();

            int minutes = (int)(DateTime.Now.Minute / 5) * 5;
            int heure = minutes > 30 ? DateTime.Now.Hour + 1 : DateTime.Now.Hour;

            string ampm;
            if (heure > 12)
            {
                heure = heure - 12;
                ampm = "pm";
                labelsAllumes.Add(PM);
            }
            else
            {
                ampm = "am";
                labelsAllumes.Add(AM);
            }

            //remplissage de la liste de labels allumés          
            switch (heure)
            {
                case 1:
                    labelsAllumes.Add(UNE_HEURE);
                    labelsAllumes.Add(HEURE);
                    break;

                case 2:
                    labelsAllumes.Add(DEUX_HEURES);
                    labelsAllumes.Add(HEURES);
                    break;

                case 3:
                    labelsAllumes.Add(TROIS_HEURES);
                    labelsAllumes.Add(HEURES);
                    break;

                case 4:
                    labelsAllumes.Add(QUATRE_HEURES);
                    labelsAllumes.Add(HEURES);
                    break;

                case 5:
                    labelsAllumes.Add(CINQ_HEURES);
                    labelsAllumes.Add(HEURES);
                    break;

                case 6:
                    labelsAllumes.Add(SIX_HEURES);
                    labelsAllumes.Add(HEURES);
                    break;

                case 7:
                    labelsAllumes.Add(SEPT_HEURES);
                    labelsAllumes.Add(HEURES);
                    break;

                case 8:
                    labelsAllumes.Add(HUIT_HEURES);
                    labelsAllumes.Add(HEURES);
                    break;

                case 9:
                    labelsAllumes.Add(NEUF_HEURES);
                    labelsAllumes.Add(HEURES);
                    break;

                case 10:
                    labelsAllumes.Add(DIX_HEURES);
                    labelsAllumes.Add(HEURES);
                    break;

                case 11:
                    labelsAllumes.Add(ONZE_HEURES);
                    labelsAllumes.Add(HEURES);
                    break;

                case 12:
                    if (ampm == "am")
                        labelsAllumes.Add(MIDI);
                    else
                        labelsAllumes.Add(MINUIT);
                    break;

                default:
                    break;
            }

            switch (minutes)
            {
                case 5:
                    labelsAllumes.Add(CINQ);
                    break;

                case 10:
                    labelsAllumes.Add(DIX);
                    break;

                case 15:
                    labelsAllumes.Add(ET1);
                    labelsAllumes.Add(QUART);
                    break;

                case 20:
                    labelsAllumes.Add(VINGT);
                    break;

                case 25:
                    labelsAllumes.Add(VINGTCINQ);
                    break;

                case 30:
                    labelsAllumes.Add(ET2);
                    labelsAllumes.Add(DEMIE);
                    break;

                case 35:
                    labelsAllumes.Add(MOINS);
                    labelsAllumes.Add(VINGTCINQ);
                    break;

                case 40:
                    labelsAllumes.Add(MOINS);
                    labelsAllumes.Add(VINGT);
                    break;

                case 45:
                    labelsAllumes.Add(MOINS);
                    labelsAllumes.Add(LE);
                    labelsAllumes.Add(QUART);
                    break;

                case 50:
                    labelsAllumes.Add(MOINS);
                    labelsAllumes.Add(DIX);
                    break;

                case 55:
                    labelsAllumes.Add(MOINS);
                    labelsAllumes.Add(CINQ);
                    break;

                default:
                    break;
            }


            //Allumage des labels à allumer
            foreach (Label[] labels in labelsAllumes)
            {
                foreach (Label label in labels)
                {
                    SolidColorBrush scb = new SolidColorBrush(Color.FromRgb(255, 255, 255));
                    label.Foreground = scb;
                }
            }
        }
    }
}

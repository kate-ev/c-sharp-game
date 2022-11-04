using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyGame
{
    // spēles Memory Training Game forma
    // spēles būtība: atmiņas spēle, kurā vajag atrast pārīšus ar vienādiem simboliem
    public partial class Game : Form
    {
        // privātais lauks paziņojumam Lietotājam, ja viņš ir uzvarējis
        private string messageCongratulations = "Woohoo!! You've matched everything!!!!";

        // saraksts ar 16 simboliem, kurus mēs izmantosim spēlē, piesaistot katru simbolu vienai Label 
        // katrai Label (Game.Designer.cs) es norādīju Font : Wedbings
        // informāciju par to ņemu no avota: https: //en.wikipedia.org/wiki/Webdings
        // šis Fonts burtu vietā izdruka simbolus
        // es esmu izvēlējusies 8 manuprāt visuzskatamākos simbolu pārus 
        List<string> symbols = new List<string>()
        {
            "l", "l", 
            "o", "o", 
            "?", "?", 
            "e", "e", 
            "p", "p", 
            "V", "V", 
            "Q", "Q", 
            "Y", "Y"
        };

        // vienmēr kontrolēsim, kuras Label tika nospiestas 
        // vienlaikus var būt noklikšķinātas tikai divas !
        Label firstSymbolClicked;
        Label secondSymbolClicked;

        Random random = new Random();
        public Game()
        {
            InitializeComponent();
            
            // metode piesaistīs simbolu katrai Label
            PlaceIconsIntoLabels();
        }

        // metode, kas piesaistīs simbolu katrai Label 
        // izmantojot Random,
        // lai katru reizi, atvērot spēli, simbolu pārīši būtu dažādās vietās
        private void PlaceIconsIntoLabels()
        {
            int randomNumber;

            // https: //docs.microsoft.com/en-us/dotnet/api/system.web.ui.controlcollection.count?view=netframework-4.8
            int labelCount = tableLayoutPanel1.Controls.Count;

            Label label;
            
            for (int i = 0; i < labelCount; i++)
            {
                // pārbaudām, vai dotais Control ir Label
                // šī pārbaude var būt svarīga, ja izdomāsim paplašināt spēli, pievienojot Controls 

                // informāciju par Control tipa pārbaudi ņemu no avota : 
                // https: //stackoverflow.com/questions/11459135/checking-for-the-control-type
                if (tableLayoutPanel1.Controls[i] is Label)
                {
                    label = (Label)tableLayoutPanel1.Controls[i];
                }
                else
                {
                    continue;
                }

                // izvēlēsimies jebkuru skailti starp 0 un simbolu skaitu - tā būs simbola pozīcija sarakstā symbols
                // un piesaistām to simbolu pie Label
                randomNumber = random.Next(0, symbols.Count);
                label.Text = symbols[randomNumber];

                // kontrolējam, lai nebūtu atkārtošanos, izdzēšot simbolu tajā pozīcijā
                symbols.RemoveAt(randomNumber);
            }
        }


        // metode, kas tiek izsaukta, kad lietotājs noklikšķina uz jebkuru Label 
        // nodrošina spēles galveno funkcionalitāti 
        private void labelClicked(object sender, EventArgs e)
        {
            // ja neviena Label nebija noklikšķināta, tālāk neiesim 
            if(firstSymbolClicked != null && secondSymbolClicked != null)
            {
                return;
            }

            Label clicked = sender as Label;
            if (clicked == null)
            {
                return;
            }

            // ja tiek nospiesta Label, kas jau ir "redzama" (t.i., teksta (simbola) krāsa ir melna), tālāk neiesim,
            // jo, nospiežot uz jau "redzamas" Label, nekam nevajadzētu notikt 
            if (clicked.ForeColor == Color.Black)
            {
                return;
            }

            // tika nospiesta Label, un mums pirms tam vēl nebija pirmās uzklikšķinātās Label 
            // tālāk neiesim 
            if (firstSymbolClicked == null)
            {
                firstSymbolClicked = clicked;
                firstSymbolClicked.ForeColor = Color.Black;
                return;
            }

            // šeit nonāksim, saņemot otro uzklikšķinātu Label = TIKAI ja firstSymbolClicked nav null 
            secondSymbolClicked = clicked;
            secondSymbolClicked.ForeColor = Color.Black;

            // pārbaudām, vai lietotājs nav uzvarējis
            checkIfUserWon();

            // ja abu Label teksti (=simboli) sakrīt, mēs "atbrīvojam" mainīgos firstSymbolClicked un secondSymbolClicked
            // abu Label teksti paliek redzami (jo pāris ir atrasts)
            if (firstSymbolClicked.Text == secondSymbolClicked.Text)
            {
                firstSymbolClicked = null;
                secondSymbolClicked = null;
            }

            // ja abu Label teksti (simboli) nesakrīt, mēs darbinām taimeri
            else
            {
                timer1.Start();
            }

        }

        // taimeris, kas atbild par to, cik ilgi abu Label simboli paliek "redzami", gadījumā, ja tie nesakrīt 
        // (lai lietotājs spētu atcerēties / pamanīt simbolus)
        // informāciju par taimeriem ņemu no avotiem:
        // https: //codescratcher.com/wpf/create-timer-wpf/
        // https: //docs.microsoft.com/en-us/dotnet/api/system.timers.timer?view=net-5.0
        private void timer1_Tick(object sender, EventArgs e)
        {
            // radīsim tekstu tikai 1 'Tick' ilgumā 
            timer1.Stop();

            // abu Label teksti (simboli) atkal paliek "neredzami"
            firstSymbolClicked.ForeColor = firstSymbolClicked.BackColor;
            secondSymbolClicked.ForeColor = secondSymbolClicked.BackColor;

            // "atbrīvojam" mainīgos, lai tālāk konrolētu Label, uz kurām noklikšķinās Lietotājs, 
            // un lai vienmēr zinātu, kura bija pirmā un kura - otrā
            firstSymbolClicked = null;
            secondSymbolClicked = null; 
        }

        private void Game_Load(object sender, EventArgs e)
        {

        }

        // metode, kas pārbaudīs, vai visu Label teksti (simboli) ir "redzami" = vai teksta krāsa ir melna
        private void checkIfUserWon()
        {
            // mainīgais katras Label pārbaudei
            Label current;

            for (int i=0; i < tableLayoutPanel1.Controls.Count; i++)
            {
                current = tableLayoutPanel1.Controls[i] as Label;

                // ja teksts nav redzams = teksta krāsa sakrīt ar fona krāsu, tālāk neiesim 
                if (current != null && current.ForeColor == current.BackColor)
                {
                    return;
                }
            }

            // šeit nonāksim tikai ja visu Label teksti (simboli) ir "redzami" = Lietotājs uzvarēja
            MessageBox.Show(messageCongratulations);
            this.Close();
        }
    }
}

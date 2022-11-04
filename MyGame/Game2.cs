using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace MyGame
{
    // spēle: Don't Get Hit
    // spēles būtība: Lietotājam, kustinot melno taisnstūri, ir jāizvairās no dzeltenajiem kvadrātiem, kas krīt no augšas
    public partial class Game2 : Form
    {
        // privātie lauki paziņojumiem Lietotājam
        private string messageUserWon = "Woohoo, you won!!! " + Environment.NewLine + "Your score: ";
        private string messageHasBeenHit = "You've been hit! Please try again!" + Environment.NewLine + "Your score: ";

        // mainīgais, kurā glabāsim Lietotāja rezultātu
        // rezultāts reprezentē, no cik dzeltenajiem kvadrātiem Lietotājam ir izdevies izvairīties
        int score = 0;
        int scoreToWin = 50; // rezultāts, kad spēle beidzas

        // attālums, par cik (px) pārvietosies melnais taisnstūris 
        int movement = 20;

        // cik mums sākotnēji būs dzelteno kvadrātu
        int enemiesCount = 10;

        // masīvs ar int vērtībām, kas reprezentēs dzelteno kvadrātu "ātrumu" (attālumu, par cik pārvietosies laika vienībā)
        int[] enemiesMovement;

        // masīvs no PictureBox, kur glabāsies dzeltenie kvadrāti
        // (mēs tos veidosim dināmiski)
        // informāciju par PictureBox ņemu no avota :
        // https: //docs.microsoft.com/en-us/dotnet/api/system.windows.forms.picturebox?view=net-5.0
        PictureBox[] enemiesToAvoid;

        Random random = new Random(); 
        public Game2()
        {
            InitializeComponent();
        }

        // informāciju par KeyDown notikumu ņemu no avota : 
        // https: //docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.keydown?view=net-5.0
        private void Game2_KeyDown(object sender, KeyEventArgs e)
        {   
            // nodrošinām spēles kontroli ar taustatūras bultām 

            // ja tika nospiesta bultiņa pa labi 
            // UN ja attālums no melnā taisnstūra (Player) labās malas līdz formas kreisajai malai ir mazāks par formas platumu
            // (neļausim Lietotājam iet "aiz" labās formas malas) 
                // informāciju par ClientRectangle ņemu no avotiem : 
                // https: //social.msdn.microsoft.com/Forums/vstudio/en-US/a02e57a5-af75-4862-ac4b-fdc18b5e01ac/how-to-get-the-width-of-form?forum=csharpgeneral
                // https: //docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.clientrectangle?view=net-5.0 
            if (e.KeyCode == Keys.Right && Player.Right < this.ClientRectangle.Width)
            {
                // kustinām melno taisnstūri 
                // informāciju par Point un Location ņemu no avota: 
                // https: //docs.microsoft.com/en-us/dotnet/desktop/winforms/controls/how-to-position-controls-on-windows-forms?view=netframeworkdesktop-4.8
                Player.Location = new Point(Player.Location.X + movement, Player.Location.Y);
            }

            // tas pats, ja tika nospiesta kreisā bulta
            // UN ja attālums no melnā taisnstūra (Player) kreisās malas līdz formas kreisajai malai ir lielāks/vienāds par 10
            // (pietiek, lai neļautu Lietotājam "paslēpties" aiz formas kreisās malas)
            if (e.KeyCode == Keys.Left && Player.Left >= 10)
            {
                Player.Location = new Point(Player.Location.X - movement, Player.Location.Y);
            }
        }

        // metode, kas tiek automātiski izsaukta, kad spēle (forma) ielādās
        // informāciju par Load notikumu ņemu no avota: 
        // https: //docs.microsoft.com/en-us/dotnet/api/system.windows.forms.form.load?view=net-5.0
        private void Game2_Load(object sender, EventArgs e)
        {
            // taimeris, kas atbildēs par dzelteno taisnstūru kustību
            timer1.Start();

            // informāciju par taimera intervālu ņemu no avota:
            // https: //docs.microsoft.com/en-us/dotnet/api/system.timers.timer.interval?view=net-5.0
            timer1.Interval = 10;

            enemiesToAvoid = new PictureBox[enemiesCount];
            enemiesMovement = new int[enemiesCount];

            // aizpildām abus masīvus
            for(int i = 0; i < enemiesCount; i++)
            {
                // dināmiski veidotā PictureBox
                enemiesToAvoid[i] = new PictureBox();
                enemiesToAvoid[i].BackColor = Color.Yellow;
                
                // dzelteno kvadrātu "ātrums" - nejauši izvēlētais skaitlis robežās no 1 līdz 10,
                // lai spēle būtu dinamiskāka
                enemiesMovement[i] = random.Next(1, 10);

                // dzeltenā kvadrāta malas garums -  nejauši izvēlētais skaitlis robežās no 10 līdz 50 px
                enemiesToAvoid[i].Height = random.Next(10,50);
                enemiesToAvoid[i].Width = enemiesToAvoid[i].Height;

                // dināmiski veidotā dzeltenā kvadrāta pozīcija : 
                // attālums līdz kreisajai formas malai - nejauši izvēlētais skailis starp 0 un formas platumu,
                // ņemot vērā arī dotā kvadrāta platumu
                // kvadrāts vienmēr tiks veidots pašā formas augšā
                enemiesToAvoid[i].Left = random.Next(0, this.ClientRectangle.Width - enemiesToAvoid[i].Width);
                enemiesToAvoid[i].Top = 0;

                // informāciju par Controls.Add(control) ņemu no avota: 
                // https: //docs.microsoft.com/en-us/dotnet/api/system.windows.forms.control.controlcollection.add?view=net-5.0
                Controls.Add(enemiesToAvoid[i]);
            }
        }

        // taimeris, kas kontrolēs dzelteno kvadrātu kustību, kā arī Lietotāja rezultātu 
        // metode nodrošina galveno spēles funkcionalitāti
        private void timer1_Tick(object sender, EventArgs e)
        {
            // label, kas parāda Lietotāja rezultātu (dinamiski) 
            label2.Text = score.ToString();

            // ja jau ir sasniegts maksimāli iespējams rezultāts 
            if (score >= scoreToWin)
            {
                // spēle apstājas, jo Lietotājs ir uzvarējis
                timer1.Stop();
                MessageBox.Show(messageUserWon + score.ToString());
                score = 0;
                // spēle beidzas
                this.Close();
            }

            // ja rezultāts vēl nav pietiekoši augsts
            for (int i = 0; i < enemiesCount; i++)
            {
                // dzelteno kvadrātu kustība
                // katram kvadrātam ir savs ātrums enemiesMovement[i], ko mēs pieskaitām Y koordinātai 
                enemiesToAvoid[i].Location = new Point(enemiesToAvoid[i].Location.X, 
                    enemiesToAvoid[i].Location.Y + enemiesMovement[i]);

                // ja melnais taisnstūris un dzeltenais kvadrāts saduras (spēle apstājas)
                // koda fragmentu if nosacījumam ņemu no avota : 
                // https: //forums.codeguru.com/showthread.php?528763-Collision-with-a-picturebox
                if (Player.Bounds.IntersectsWith(enemiesToAvoid[i].Bounds))
                {
                    timer1.Stop();
                    MessageBox.Show(messageHasBeenHit + score.ToString());
                    // spēle beidzas
                    this.Close();
                }
                
                // ja dzeltenais kvadrāts ir nonācis (ir "paslēpies" aiz) formas zemākai robežas
                // ( = kvadrāta attālums līdz augšai ir lielāks par formas augstumu) 
                if (enemiesToAvoid[i].Top > this.ClientRectangle.Height)
                {
                    // pieskaitām +1 pie rezultāta 
                    score++;
                    
                    // it kā pārvietojam šo pašu kvadrātu augšā, bet dodam citus paramentrus un "ātrumu",
                    // lai spēle būtu dinamiskāka
                    // (t.i., lai dzeltenie kvadrāti vienmēr tiktu ģenerēti dažādās pozīcijās)
                    enemiesToAvoid[i].Top = 0;
                    enemiesToAvoid[i].Left = random.Next(0, this.ClientRectangle.Width - enemiesToAvoid[i].Width);
                    enemiesMovement[i] = random.Next(1, 10);
                }
            }

        }
    }
}

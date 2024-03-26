using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

//"Izjavljam, da sem nalogo opravil samostojno in da sem njen avtor. Zavedam se, da v primeru, če izjava prvega stavka ni resnična, kršim disciplinska pravila."

namespace Elektronska_posta
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            PrikaziShranjenePodatke();
            DodajVeljavneStreznike();
        }

        private void Form_Shown(object sender, EventArgs e)
        {
            // Fokus na pošiljatelja
            label1.Focus();

            // Set the default sender email address if the "Remember sender" option is checked and SMTP server is selected
            if (string.IsNullOrEmpty(textBox1.Text))
            {
                textBox1.Text = Properties.Settings.Default.Privzeti_Pošiljatelj;
            }
        }
        private void PrikaziShranjenePodatke()
        {
            // Preveri, ali so podatki shranjeni
            if (Properties.Settings.Default.ZapomniSiMe)
            {
                // Če so shranjeni, prikaži shranjene podatke na obrazcu
                checkBox1.Checked = true;
                comboBox1.Text = Properties.Settings.Default.Prejemnik;
                comboBox2.Text = Properties.Settings.Default.SMTPStreznik;
                textBox5.Text = Properties.Settings.Default.UporabniskoIme;
            }
        }

        private void checkBoxZapomniSiMe_CheckedChanged(object sender, EventArgs e)
        {
            // Shrani stanje checkbox-a 'Zapomni si me' v nastavitve
            Properties.Settings.Default.ZapomniSiMe = checkBox1.Checked;
            Properties.Settings.Default.Save();
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Shrani vnesene podatke, če je checkbox 'Zapomni si me' označen
            if (checkBox1.Checked)
            {
                Properties.Settings.Default.Prejemnik = comboBox1.Text;
                Properties.Settings.Default.SMTPStreznik = comboBox2.Text;
                Properties.Settings.Default.UporabniskoIme = textBox5.Text;
                Properties.Settings.Default.Save();
            }
            else
            {
                // Če checkbox ni označen, pobriši shranjene podatke
                Properties.Settings.Default.Prejemnik = "";
                Properties.Settings.Default.SMTPStreznik = "";
                Properties.Settings.Default.UporabniskoIme = "";
                Properties.Settings.Default.Save();
            }
        }

        private void DodajVeljavneStreznike()
        {
            string[] veljavniStrezniki = { "smtp.t-2.net", "mail.arnes.si", "mail.siol.net", "smtp.gmail.com" };
            foreach (string streznik in veljavniStrezniki)
            {
                comboBox2.Items.Add(streznik);
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            if (Properties.Settings.Default.ZapomniSiMe)
            {
                comboBox1.Text = Properties.Settings.Default.Prejemnik;
                comboBox2.Text = Properties.Settings.Default.SMTPStreznik;
                textBox5.Text = Properties.Settings.Default.UporabniskoIme;
            }
        }

        private void tableLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void label1_Click_1(object sender, EventArgs e)
        {

        }

        private void tableLayoutPanel1_Paint_1(object sender, PaintEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
                try
                {
                    // Preverite, ali so vsi obvezni podatki izpolnjeni
                    if (string.IsNullOrWhiteSpace(textBox1.Text))
                    {
                        throw new Exception("Vnesite veljavnega pošiljatelja.");
                    }
                    if (string.IsNullOrWhiteSpace(comboBox1.Text))
                    {
                        throw new Exception("Vnesite veljavnega prejemnika.");
                    }
                    if (string.IsNullOrWhiteSpace(textBox4.Text))
                    {
                        throw new Exception("Vnesite zadevo sporočila.");
                    }
                    if (string.IsNullOrWhiteSpace(richTextBox1.Text))
                    {
                        throw new Exception("Vnesite besedilo sporočila.");
                    }
                    if (string.IsNullOrWhiteSpace(comboBox2.Text))
                    {
                        throw new Exception("Izberite ali vnesite SMTP strežnik.");
                    }

                    // Če so vsa polja izpolnjena, lahko pošljemo sporočilo
                    PosljiSporocilo();
                }
                catch (Exception ex)
                {
                    // Če pride do napake, prikažite sporočilo o napaki
                    MessageBox.Show(ex.Message, "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        private void DodajPodatkeOPoslanihSporocilih(string vsebinaSporocila)
        {
            string potDoDatoteke = "PoslanaSporocila.txt";
            // Preverite, ali datoteka že obstaja
            if (!File.Exists(potDoDatoteke))
            {
                // Če datoteka ne obstaja, jo ustvarite
                File.Create(potDoDatoteke).Close();
            }

            // Dodajte vsebino sporočila v datoteko
            using (StreamWriter writer = File.AppendText(potDoDatoteke))
            {
                writer.WriteLine(vsebinaSporocila);
            }
        }
        private void PregledPoslanihSporocil()
        {
            string potDoDatoteke = "PoslanaSporocila.txt";
            string urejevalnik = "notepad.exe"; // Urejevalnik besedila, ki ga želite uporabiti

            // Preverite, ali datoteka obstaja
            if (File.Exists(potDoDatoteke))
            {
                // Odprite datoteko z ustreznim urejevalnikom
                Process.Start(urejevalnik, potDoDatoteke);
            }
            else
            {
                MessageBox.Show("Datoteka s poslanimi sporočili ne obstaja.", "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }


        private void toolStripButton1_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {

        }

        private void toolStripLabel2_Click(object sender, EventArgs e)
        {

        }

        private void toolStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void toolStripButton1_Click_2(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            
        }

        private void PosljiSporocilo()
        {
            try
            {
                // Preverite, ali so vsi obvezni podatki izpolnjeni
                if (string.IsNullOrWhiteSpace(textBox1.Text))
                {
                    throw new Exception("Vnesite veljavnega pošiljatelja.");
                }
                if (string.IsNullOrWhiteSpace(comboBox1.Text))
                {
                    throw new Exception("Vnesite veljavnega prejemnika.");
                }
                if (string.IsNullOrWhiteSpace(textBox4.Text))
                {
                    throw new Exception("Vnesite zadevo sporočila.");
                }
                if (string.IsNullOrWhiteSpace(richTextBox1.Text))
                {
                    throw new Exception("Vnesite besedilo sporočila.");
                }
                if (string.IsNullOrWhiteSpace(comboBox2.Text))
                {
                    throw new Exception("Izberite ali vnesite SMTP strežnik.");
                }

                // Preverjanje veljavnosti elektronskega naslova pošiljatelja
                if (!String.IsNullOrEmpty(textBox1.Text))
                {
                    try
                    {
                        // Za preverjanje veljavnega el. naslova lahko uporabimo razred MailAddress
                        MailAddress m = new MailAddress(textBox1.Text);
                    }
                    catch (FormatException)
                    {
                        throw new Exception("Neveljaven elektronski naslov pošiljatelja!");
                    }
                }
                else
                {
                    throw new Exception("Vnesi elektronski naslov pošiljatelja!");
                }

                // Ustvarjanje novega poštnega sporočila
                MailMessage mailMessage = new MailMessage();

                mailMessage.To.Add(comboBox1.Text); // Prejemnik
                mailMessage.From = new MailAddress(textBox1.Text); // Pošiljatelj

                // Če je vnesen veljaven CC, ga dodamo
                if (!string.IsNullOrWhiteSpace(textBox2.Text))
                {
                    try
                    {
                        mailMessage.CC.Add(textBox2.Text);
                    }
                    catch (FormatException)
                    {
                        throw new Exception("Neveljaven elektronski naslov CC!");
                    }
                }

                // Če je vnesen veljaven Bcc, ga dodamo
                if (!string.IsNullOrWhiteSpace(textBox3.Text))
                {
                    try
                    {
                        mailMessage.Bcc.Add(textBox3.Text);
                    }
                    catch (FormatException)
                    {
                        throw new Exception("Neveljaven elektronski naslov Bcc!");
                    }
                }

                mailMessage.Subject = textBox4.Text; // Zadeva sporočila
                mailMessage.Body = richTextBox1.Text; // Vsebina sporočila

                // Nastavimo format sporočila
                mailMessage.IsBodyHtml = false; // Tekstovni format

                // Ustvarimo SMTP klienta in pošljemo sporočilo
                SmtpClient smtp = new SmtpClient();
                smtp.Host = comboBox2.Text; // SMTP strežnik
                smtp.Credentials = new NetworkCredential(textBox5.Text, textBox6.Text); // Uporabniško ime in geslo
                smtp.EnableSsl = true; // Omogočimo SSL
                smtp.Port = 587; // Port za SSL

                // Pošljemo sporočilo
                smtp.Send(mailMessage);

                // Dodajte podatke o poslanem sporočilu v datoteko
                string vsebinaSporocila = $"Prejemnik: {comboBox1.Text}, Zadeva: {textBox4.Text}, Vsebina: {richTextBox1.Text}";
                DodajPodatkeOPoslanihSporocilih(vsebinaSporocila);
            }
            catch (Exception ex)
            {
                // Če pride do napake, prikažemo sporočilo o napaki
                MessageBox.Show(ex.Message, "Napaka", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void toolStripButton3_Click_1(object sender, EventArgs e)
        {
            try
            {
                textBox1.Text = ""; // Pošiljatelj
                comboBox1.Text = ""; // Prejemnik
                textBox2.Text = ""; // CC
                textBox3.Text = ""; // BCC
                textBox4.Text = ""; // Zadeva sporočila
                richTextBox1.Text = ""; // Vsebina sporočila
                comboBox1.SelectedIndex = -1; // Prejemnik
                comboBox2.SelectedIndex = -1; // SMTP strežnik
                textBox5.Text = ""; // Uporabniško ime
                textBox6.Text = ""; // Geslo
                radioButton1.Checked = false; // For radio button 1
                radioButton2.Checked = false; // For radio button 2
                radioButton3.Checked = false; // For radio button 3
                checkBox1.Checked = false; // For checkbox
                // Set focus to the sender field
                textBox1.Focus();
            }
            catch (Exception ex)
            {
                // Handle any exceptions
                MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}

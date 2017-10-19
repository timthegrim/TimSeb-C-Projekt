﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using Logic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;

namespace WindowsFormsApp1
{
    public partial class Window : Form
    {
        PodFeed podfeed = new PodFeed();
        Kategori kategori = new Kategori();
        Validering validering = new Validering();
        public List<String> listaPod = new List<String>();
        List<String> listaUppdatering = new List<String>();

        public List<String> Podcast
        {
            get
            {
                return listaPod;
            }
        }

        public Window()
        {
            try
            {
                InitializeComponent();
                listaPod = new List<String>();
                fyllListaKategori();
                fyllCbUppdatering();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void fyllCbUppdatering()
        {
            try
            {
                cbUppdatering.Items.Add("2000");
                cbUppdatering.Items.Add("10000");
                cbUppdatering.Items.Add("60000");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        public void fyllListaKategori()
        {
            String[] kategori = Directory.GetDirectories(Directory.GetCurrentDirectory());
            foreach (String kat in kategori)
            {
                String[] splitta = kat.Split('\\');
                int längd = splitta.Length - 1;
                String fixadLängd = splitta[längd];
                lbKategori.Items.Add(fixadLängd);
                cbKategori.Items.Add(fixadLängd);
            }
        }

        private async void btnLäggTillPod_Click(object sender, EventArgs e)
        {
            await btnLäggTillPod_ClickAsync();
        }

        public async Task btnLäggTillPod_ClickAsync()
        {
            if (Validering.kollaTextFält(txtURL, "URL") && Validering.kollaTextFält(txtNamn, "Namn") && Validering.KollaUrl(txtURL.Text))
            {
                if (cbKategori.SelectedItem == null)
                {
                    MessageBox.Show("Välj kategori från komboboxen");
                    return;
                }
                else
                {
                    PodFeed xmlPodFeed = new PodFeed();
                    xmlPodFeed.skapaPod(txtNamn.Text, txtURL.Text, cbKategori.SelectedItem.ToString(), cbUppdatering.SelectedItem.ToString());
                    lbPodcast.Items.Clear();
                    lbKategori.Items.Clear();
                    fyllListaKategori();
                    MessageBox.Show(txtNamn.Text + " har lagts till.");
                }
            }
            await Task.Delay(1000);
        }

        private void btnLäggTillKategori_Click_1(object sender, EventArgs e)
        {
            if (Validering.kollaTextFält(txtLäggTillKategori, "Kategori"))
            {
                kategori.nyMapp(txtLäggTillKategori.Text);
                lbKategori.Items.Clear();
                cbKategori.Items.Clear();
                fyllListaKategori();
                MessageBox.Show("Kategorin: '" + txtLäggTillKategori.Text + "' har lagts till.");
                txtLäggTillKategori.Clear();
            }
        }

        public void fyllListaPodcast(String kategori, ListBox lista)
        {
            String[] podcast = Directory.GetFiles(kategori);
            foreach (String pod in podcast)
            {
                String[] splitta = pod.Split('\\');
                String splitta2 = splitta[splitta.Length - 1];
                String[] fixadLängd = splitta2.Split('.');
                String fixadSplitt = fixadLängd[0];
                lbPodcast.Items.Add(fixadSplitt);
            }
        }
        //knas
        private void lbKategori_MouseClick(object sender, EventArgs e)
        {
            if (lbKategori.SelectedItem != null)
            {
                lbPodcast.Items.Clear();
                fyllListaPodcast(lbKategori.Text, lbPodcast);
            }
        }






        private void Window_Load(object sender, EventArgs e)
        {

        }

        private void label5_Click(object sender, EventArgs e)
        {

        }

        private void textBox3_TextChanged(object sender, EventArgs e)
        {

        }

        private void btnTabortKategori_Click(object sender, EventArgs e)
        {

            if (Validering.KollaPodLista(lbKategori))
            {
                podfeed.taBortKategori(lbKategori.Text);
                cbKategori.Items.Clear();
                lbKategori.Items.Clear();
                fyllListaKategori();
                clbAvsnitt.Items.Clear();
                lbPodcast.Items.Clear();
                MessageBox.Show("Kategorin är borttagen.");
            }

        }

        private void lbPodcast_MouseClick(object sender, MouseEventArgs e)
        {
            if (lbPodcast.SelectedItem != null)
            {
                clbAvsnitt.Items.Clear();
                podfeed.hämtaAvsnitt(lbKategori.SelectedItem.ToString(), lbPodcast.SelectedItem.ToString(), clbAvsnitt);
                podfeed.omSpelad(lbKategori.SelectedItem.ToString(), lbPodcast.SelectedItem.ToString(), clbAvsnitt);
            }
        }

        private void btnTabortPod_Click(object sender, EventArgs e)
        {
            if (Validering.KollaPodLista(lbPodcast))
            {
                podfeed.taBortPod(lbKategori.Text, lbPodcast.Text);
                clbAvsnitt.Items.Clear();
                lbPodcast.Items.Clear();
                tbOm.Clear();
                fyllListaPodcast(lbKategori.Text, lbPodcast);
                MessageBox.Show("Poddcasten är borttagen.");
            }
        }

        //KNAS
        private void btnÄndraKategori_Click(object sender, EventArgs e)
        {
            if (Validering.kollaTextFält(txtNamn, "Namn"))
            {
                if (cbKategori.SelectedItem == null)
                {
                    MessageBox.Show("Välj en kategori du vill ändra.");
                }
                else
                {
                    podfeed.ändraKategori(lbKategori.Text, txtNamn.Text, lbKategori, cbKategori);
                    lbKategori.Items.Clear();
                    cbKategori.Items.Clear();
                    clbAvsnitt.Items.Clear();
                    lbPodcast.Items.Clear();
                    tbOm.Clear();
                    fyllListaKategori();
                }
            }

        }

        private void clbAvsnitt_MouseClick(object sender, MouseEventArgs e)
        {

            if (clbAvsnitt.SelectedItem != null)
            {
                podfeed.hamtaOmAvsnitt(lbKategori.SelectedItem.ToString(), lbPodcast.SelectedItem.ToString(), clbAvsnitt.SelectedItem.ToString(), tbOm);
            }
        }
    }
}


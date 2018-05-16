using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SprintBoard
{
    public partial class AboutForm : Form
    {
        public AboutForm()
        {
            InitializeComponent();
        }

        private void AboutForm_Load(object sender, EventArgs e)
        {
           richTextBox1.Text+=@" MANİSA CELAL BAYAR ÜNİVERSİTESİ"+Environment.NewLine+
                               "HASAN FERDİ TURGUTLU TEKNOLOJİ FAKÜLTESİ"+Environment.NewLine+
                                "YAZILIM MÜHENDİSLİĞİ BÖLÜMÜ"+Environment.NewLine+
                                "YZM 2122 Yazılım Yapımı Dersi  (MAYIS-2018)"+Environment.NewLine+
                                "SCRUM TABLE PROJESİ" + Environment.NewLine +
                                "Danışman : Yrd. Doç. Dr. Emin BORANDAĞ"+Environment.NewLine+
                                "Hazırlayan: Orçun ÖZDİL 172803065";
           richTextBox1.SelectAll();
           richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

           webBrowser1.Navigate(new Uri(@"file:///C:\Users\orçun\Desktop\Odev-Proje\YzmYpmOdevSon\SprintBoard\SprintBoard\bin\Debug\Odev.pdf"));
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            System.Diagnostics.Process.Start(linkLabel1.Text);
        }
    }
}

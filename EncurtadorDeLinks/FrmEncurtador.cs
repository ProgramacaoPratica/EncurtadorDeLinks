using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EncurtadorDeLinks
{
    public partial class FrmEncurtador : Form
    {
        public FrmEncurtador()
        {
            InitializeComponent();
        }

        private async void btnEncurtar_Click(object sender, EventArgs e)
        {
            if (txtUrlLonga.Text.Trim().Equals(string.Empty))
                return;

            string longUrl = txtUrlLonga.Text.Trim();

            BitLyAPI.BitLyAPI api = new BitLyAPI.BitLyAPI();
            
            txtUrlEncurtada.Text = await api.ShortenAsync(longUrl);
        }

        private async void btnEncurtarListaDeLinks_Click(object sender, EventArgs e)
        {

            string strEntrada = @"c:\users\gilse\documents\visual studio 2015\Projects\EncurtadorDeLinks\EncurtadorDeLinks\Arquivos\Entrada.txt";
            string strSaida = @"c:\users\gilse\documents\visual studio 2015\Projects\EncurtadorDeLinks\EncurtadorDeLinks\Arquivos\Saida.txt";

            if (File.Exists(strEntrada))
            {
                using (StreamReader sr = new StreamReader(strEntrada))
                {
                    var listLinks = new List<Link>();
                    string linha = string.Empty;
                    while ((linha = sr.ReadLine()) != null)
                    {
                        if (!linha.Equals(string.Empty))
                        {
                            var oLink = new Link();
                            oLink.LongUrl = linha;
                            listLinks.Add(oLink);
                        }
                    }

                    BitLyAPI.BitLyAPI api = new BitLyAPI.BitLyAPI();

                    foreach (var item in listLinks)
                    {
                        item.ShortUrl = await api.ShortenAsync(item.LongUrl);
                    }

                    using (StreamWriter wr = new StreamWriter(strSaida, false))
                    {
                        foreach (var item in listLinks)
                        {
                            wr.WriteLine(item.ShortUrl);
                        }
                    }
                }
            }
            MessageBox.Show("Processamento Finalizado!", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
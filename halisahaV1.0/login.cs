using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace halisahaV1._0
{
    public partial class login : Form
    {
        public login()
        {
            InitializeComponent();
        }
        public static SqlConnection con;
        public static SqlCommand cmd;
        public static SqlDataReader dr;
        public static string kuadi;
        public static string sifresi;
        veribilgi veri = new veribilgi();
        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            kuadi = kua.Text;
            sifresi = sifre.Text;
            con = new SqlConnection(veri.source);
            cmd = new SqlCommand();
            con.Open();
            cmd.Connection = con;
            cmd.CommandText = "SELECT * FROM hesap where kuadi='" + kuadi + "' AND sifre='" + sifresi + "'";
            dr = cmd.ExecuteReader();
            if (dr.Read())
            {
                menu yeni = new menu();
                yeni.Show();
                this.Hide();
            }
            else
            {
                MessageBox.Show("Kullanıcı adını ve şifrenizi kontrol ediniz.");
            }
            con.Close();
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            DialogResult sonuc;
            sonuc = MessageBox.Show("Çıkmak İstediğinizden Emin misiniz ?", "Çıkış", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (sonuc == DialogResult.Yes)
            {
                Application.Exit();
            }
        }

        private void kua_Enter(object sender, EventArgs e)
        {
            kua.Text = "";
        }
        private void sifre_Enter(object sender, EventArgs e)
        {

            sifre.Text = "";
        }
        Point İlkkonum;
        bool durum = false;
        private void login_Load(object sender, EventArgs e)
        {
            this.Opacity = 0.80;
        }

        private void login_MouseDown(object sender, MouseEventArgs e)
        {
            durum = true;
            this.Cursor = Cursors.SizeAll; // Fareyi taşıma şeklinde seçim yapılmış ikon halini almasını sağladık.
            İlkkonum = e.Location;
        }

        private void login_MouseMove(object sender, MouseEventArgs e)
        {
            if (durum)
            {
                this.Left = e.X + this.Left - (İlkkonum.X);
                this.Top = e.Y + this.Top - (İlkkonum.Y);
            }
        }

        private void login_MouseUp(object sender, MouseEventArgs e)
        {
            durum = false;
            this.Cursor = Cursors.Default;
        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            durum = true;
            this.Cursor = Cursors.SizeAll; // Fareyi taşıma şeklinde seçim yapılmış ikon halini almasını sağladık.
            İlkkonum = e.Location;
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {

            durum = false;
            this.Cursor = Cursors.Default;
        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (durum)
            {
                this.Left = e.X + this.Left - (İlkkonum.X);
                this.Top = e.Y + this.Top - (İlkkonum.Y);
            }
        }

        private void sifre_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Enter)
            {
                kuadi = kua.Text;
                sifresi = sifre.Text;
                con = new SqlConnection(veri.source);
                cmd = new SqlCommand();
                con.Open();
                cmd.Connection = con;
                cmd.CommandText = "SELECT * FROM hesap where kuadi='" + kuadi + "' AND sifre='" + sifresi + "'";
                dr = cmd.ExecuteReader();
                if (dr.Read())
                {
                    menu yeni = new menu();
                    yeni.ShowDialog();
                }
                else
                {
                    MessageBox.Show("Kullanıcı adını ve şifrenizi kontrol ediniz.");
                }
                con.Close();
            }
        }
    }
}

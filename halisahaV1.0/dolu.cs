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
    public partial class dolu : Form
    {
        public dolu()
        {
            InitializeComponent();
        }
        veribilgi veri = new veribilgi();
        private void dolu_Load(object sender, EventArgs e)
        {
            if (menu.dinamikButon.Appearance.BackColor == Color.Yellow)
            {
                textBox9.Enabled = false;
                textBox2.Enabled = false;
                textBox3.Enabled = false;
                maskedTextBox1.Enabled = false;
                maskedTextBox2.Enabled = false;
                textBox1.Enabled = false;
                textBox8.Enabled = false;
                button1.Enabled = false;
                button5.Enabled = false;
                button6.Enabled = false;
            }
            this.Text = menu.dinamikButon.Text;
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                
                ///
                SqlCommand komut = new SqlCommand();
                komut.CommandText = "SELECT * FROM tliste";
                komut.Connection = conn;
                komut.CommandType = CommandType.Text;
                SqlDataReader dr;
                conn.Open();
                dr = komut.ExecuteReader();
                while (dr.Read())
                {
                    char[] ayrac = { ' ' };
                    string tarih = menu.tarih.ToString() ;
                    string[] parcalar = tarih.Split(ayrac);
                    string a = DateTime.Parse(parcalar[0]).ToString("yyyy.MM.dd");
                    if (menu.dinamikButon.Text == "  " + dr["takim_adi"].ToString())
                    {
                        textBox9.Text = dr["ad_soyad"].ToString().Trim();
                        textBox2.Text = dr["takim_adi"].ToString().Trim();
                        textBox3.Text = dr["mail"].ToString().Trim();
                        maskedTextBox2.Text = dr["tc"].ToString().Trim();
                        maskedTextBox1.Text = dr["telefon"].ToString().Trim();
                        label12.Text = dr["idno"].ToString().Trim();
                        dateTimePicker2.Value = menu.tarih;
                    }


                }
                SqlCommand komut1 = new SqlCommand();
                komut1.CommandText = "SELECT * FROM rezervasyon";
                komut1.Connection = conn;
                komut1.CommandType = CommandType.Text;
                dr.Close();
                SqlDataReader dr1;
                dr1 = komut1.ExecuteReader();
                while (dr1.Read())
                {
                    if (menu.dinamikButon.Text == "  " + dr1["takim_adi"].ToString())
                    {
                        dateTimePicker2.Text = dr1["tarih"].ToString();
                        comboBox1.Text = dr1["saat"].ToString();
                        textBox1.Text = dr1["fiyat"].ToString().Trim();
                    }

                }
                dr1.Close();
                komut.CommandText = "SELECT * FROM takim_bilgi";
                dr1 = komut.ExecuteReader();
                while (dr1.Read())
                {
                    if (menu.dinamikButon.Text == "  " + dr1["takim_adi"].ToString())
                    {
                        listBox1.Items.Add(dr1["oyuncu1"].ToString().Trim());
                        listBox1.Items.Add(dr1["oyuncu2"].ToString().Trim());
                        listBox1.Items.Add(dr1["oyuncu3"].ToString().Trim());
                        listBox1.Items.Add(dr1["oyuncu4"].ToString().Trim());
                        listBox1.Items.Add(dr1["oyuncu5"].ToString().Trim());
                        listBox1.Items.Add(dr1["oyuncu6"].ToString().Trim());
                        listBox1.Items.Add(dr1["oyuncu7"].ToString().Trim());
                        listBox1.Items.Add(dr1["oyuncu8"].ToString().Trim());
                    }

                }

                conn.Close();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                conn.Open();
                string kayit = "UPDATE tliste SET ad_soyad=@adsoyad,takim_adi=@takimadi,telefon=@telefon,mail=@mail,tc=@tc where idno=@idno";
                SqlCommand komut = new SqlCommand(kayit, conn);
                string kayit1 = "UPDATE rezervasyon SET takim_adi=@takimadi1,tarih=@tarih,saat=@saat where idno=@idno";
                SqlCommand komut1 = new SqlCommand(kayit1, conn);
                string kayit2 = "UPDATE takim_bilgi SET takim_adi=@takimadi2,oyuncu1=@oyuncu1,oyuncu2=@oyuncu2,oyuncu3=@oyuncu3,oyuncu4=@oyuncu4,oyuncu5=@oyuncu5,oyuncu6=@oyuncu6,oyuncu7=@oyuncu7,oyuncu8=@oyuncu8,fiyat=@fiyat where idno=@idno";
                SqlCommand komut2 = new SqlCommand(kayit2, conn);
                komut.Parameters.AddWithValue("@adsoyad", textBox9.Text);
                komut.Parameters.AddWithValue("@takimadi", textBox2.Text);
                komut.Parameters.AddWithValue("@telefon", maskedTextBox1.Text);
                komut.Parameters.AddWithValue("@mail", textBox3.Text);
                komut.Parameters.AddWithValue("@tc", maskedTextBox2.Text);
                komut.Parameters.AddWithValue("@fiyat", textBox1.Text);
                komut.Parameters.AddWithValue("@idno", label12.Text);
                //
                komut1.Parameters.AddWithValue("@takimadi1", textBox2.Text);
                komut1.Parameters.AddWithValue("@tarih", dateTimePicker2.Value);
                komut1.Parameters.AddWithValue("@saat", comboBox1.Text);
                komut1.Parameters.AddWithValue("@idno", label12.Text);
                //
                komut2.Parameters.AddWithValue("@takimadi2", textBox2.Text);
                komut2.Parameters.AddWithValue("@oyuncu1", listBox1.Items[0].ToString());
                komut2.Parameters.AddWithValue("@oyuncu2", listBox1.Items[1].ToString());
                komut2.Parameters.AddWithValue("@oyuncu3", listBox1.Items[2].ToString());
                komut2.Parameters.AddWithValue("@oyuncu4", listBox1.Items[3].ToString());
                komut2.Parameters.AddWithValue("@oyuncu5", listBox1.Items[4].ToString());
                komut2.Parameters.AddWithValue("@oyuncu6", listBox1.Items[5].ToString());
                komut2.Parameters.AddWithValue("@oyuncu7", listBox1.Items[6].ToString());
                komut2.Parameters.AddWithValue("@oyuncu8", listBox1.Items[7].ToString());
                komut2.Parameters.AddWithValue("@idno", label12.Text);
                komut2.ExecuteNonQuery();
                komut1.ExecuteNonQuery();
                komut.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Müşteri Bilgileri Güncellendi.");
                menu frm1 = (menu)Application.OpenForms["menu"];
                frm1.tarihgoster();
            }
        }
        int sayac = 0;
        private void button5_Click(object sender, EventArgs e)
        {
            if (sayac < 9)
            {
                sayac++;
                listBox1.Items.Add(textBox8.Text);
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
            sayac--;
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}

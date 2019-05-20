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
using System.Text.RegularExpressions;

namespace halisahaV1._0
{
    public partial class bos : Form
    {
        public bos()
        {
            InitializeComponent();
        }
        veribilgi veri = new veribilgi();
        private const string MatchEmailPattern =
                  @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
                                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
           + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
                                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                bool retVal = false;
                retVal = Regex.IsMatch(textBox3.Text, MatchEmailPattern);
                if (retVal && maskedTextBox2.TextLength == 11 && maskedTextBox1.TextLength == 14)
                {
                    using (SqlConnection conn = new SqlConnection(veri.source))
                    {
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        string kayit = "insert into tliste(ad_soyad,takim_adi,telefon,mail,tc) values (@adsoyad,@takimadi,@telefon,@mail,@tc)";
                        SqlCommand komut = new SqlCommand(kayit, conn);
                        komut.Parameters.AddWithValue("@adsoyad", comboBox2.Text);
                        komut.Parameters.AddWithValue("@takimadi", textBox2.Text);
                        komut.Parameters.AddWithValue("@telefon", maskedTextBox1.Text);
                        komut.Parameters.AddWithValue("@mail", textBox3.Text);
                        komut.Parameters.AddWithValue("@tc", maskedTextBox2.Text);
                        //Parametrelerimize Form üzerinde ki kontrollerden girilen verileri aktarıyoruz.
                        komut.ExecuteNonQuery();
                        //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
                        conn.Close();
                        if (conn.State == ConnectionState.Closed)
                        { conn.Open(); }
                        // Bağlantımızı kontrol ediyoruz, eğer kapalıysa açıyoruz.
                        kayit = "insert into rezervasyon(takim_adi,tarih,saat,fiyat) values (@takimadi,@tarih,@saat,@fiyat)";
                        // müşteriler tablomuzun ilgili alanlarına kayıt ekleme işlemini gerçekleştirecek sorgumuz.
                        SqlCommand komut1 = new SqlCommand(kayit, conn);
                        //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
                        komut1.Parameters.AddWithValue("@takimadi", textBox2.Text);
                        komut1.Parameters.AddWithValue("@tarih", dateTimePicker2.Value);
                        komut1.Parameters.AddWithValue("@saat", comboBox1.Text);
                        komut1.Parameters.AddWithValue("@fiyat", textBox1.Text);
                        //Parametrelerimize Form üzerinde ki kontrollerden girilen verileri aktarıyoruz.
                        komut1.ExecuteNonQuery();
                        //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
                        conn.Close();
                        if (conn.State == ConnectionState.Closed) conn.Open();
                        // Bağlantımızı kontrol ediyoruz, eğer kapalıysa açıyoruz.
                        kayit = "insert into takim_bilgi(takim_adi,oyuncu1,oyuncu2,oyuncu3,oyuncu4,oyuncu5,oyuncu6,oyuncu7,oyuncu8) values (@takimadi,@oyuncu1,@oyuncu2,@oyuncu3,@oyuncu4,@oyuncu5,@oyuncu6,@oyuncu7,@oyuncu8)";
                        // müşteriler tablomuzun ilgili alanlarına kayıt ekleme işlemini gerçekleştirecek sorgumuz.
                        SqlCommand komut2 = new SqlCommand(kayit, conn);
                        //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
                        komut2.Parameters.AddWithValue("@takimadi", textBox2.Text);
                        komut2.Parameters.AddWithValue("@oyuncu1", listBox1.Items[0].ToString());
                        komut2.Parameters.AddWithValue("@oyuncu2", listBox1.Items[1].ToString());
                        komut2.Parameters.AddWithValue("@oyuncu3", listBox1.Items[2].ToString());
                        komut2.Parameters.AddWithValue("@oyuncu4", listBox1.Items[3].ToString());
                        komut2.Parameters.AddWithValue("@oyuncu5", listBox1.Items[4].ToString());
                        komut2.Parameters.AddWithValue("@oyuncu6", listBox1.Items[5].ToString());
                        komut2.Parameters.AddWithValue("@oyuncu7", listBox1.Items[6].ToString());
                        komut2.Parameters.AddWithValue("@oyuncu8", listBox1.Items[7].ToString());
                        //Parametrelerimize Form üzerinde ki kontrollerden girilen verileri aktarıyoruz.
                        komut2.ExecuteNonQuery();
                        //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
                        conn.Close();
                        MessageBox.Show("Kayıt işlemi tamamlandı.");
                        menu frm1 = (menu)Application.OpenForms["menu"];
                        frm1.tarihgoster();
                    }
                }
                else
                {
                    MessageBox.Show("Mail Adresini ve diğer bilgileri düzgün girdiginizden emin olun");
                }
            }
            catch (Exception hata )
            {

                MessageBox.Show("Lütfen tüm bilgileri girdiğinizden emin olun.Eğer bir hata olduğunu düşünüyorsanız programcınız ile iletişime geçiniz.");
                MessageBox.Show(hata.ToString());
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

        private void bos_Load(object sender, EventArgs e)
        {
            dateTimePicker2.Value = menu.tarih;
            comboBox1.Text = menu.dinamikButon.Text;
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                SqlCommand komut2 = new SqlCommand();
                komut2.CommandText = "SELECT *FROM tliste";
                komut2.Connection = conn;
                komut2.CommandType = CommandType.Text;

                SqlDataReader dr2;
                conn.Open();
                dr2 = komut2.ExecuteReader();
                while (dr2.Read())
                {
                    comboBox2.Items.Add(dr2["ad_soyad"]);
                }
                conn.Close();
            }
        }

        private void comboBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = comboBox2.SelectedIndex;
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                SqlDataAdapter adaptor = new SqlDataAdapter();
                DataSet ds = new DataSet();
                DataRow oku;
                SqlCommand komut = new SqlCommand("SELECT * FROM tliste", conn);
                adaptor.SelectCommand = komut;
                komut.Connection = conn;

                adaptor.Fill(ds, "Yeni");
                oku = ds.Tables["Yeni"].Rows[index];
                textBox2.Text = oku["takim_adi"].ToString().Trim();
                textBox3.Text = oku["mail"].ToString().Trim();
                maskedTextBox1.Text = oku["telefon"].ToString().Trim();
                maskedTextBox2.Text = oku["tc"].ToString().Trim();

                komut.Dispose();
                conn.Close();
            }
        }
    }
}

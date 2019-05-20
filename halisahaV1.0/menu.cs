using System;
using System.Data;
using System.Drawing;
using System.Data.SqlClient;
using System.Windows.Forms;
using System.Text.RegularExpressions;
using DevExpress.XtraEditors;
using DevExpress.XtraReports.UI;
namespace halisahaV1._0
{
    public partial class menu : Form
    {
        public menu()
        {
            InitializeComponent();
        }
        veribilgi veri = new veribilgi();
        DataTable tbl;
        private void menu_Load(object sender, EventArgs e)
        {
            xtraTabControl2.SelectedTabPageIndex = 0;
            DateTime input = dateNavigator1.DateTime;
            int delta = DayOfWeek.Monday - input.DayOfWeek;
            DateTime monday = input.AddDays(delta);
            simpleButton1.Text = monday.ToShortDateString();
            simpleButton2.Text = monday.AddDays(+1).ToShortDateString();
            simpleButton3.Text = monday.AddDays(+2).ToShortDateString();
            simpleButton4.Text = monday.AddDays(+3).ToShortDateString();
            simpleButton5.Text = monday.AddDays(+4).ToShortDateString();
            simpleButton6.Text = monday.AddDays(+5).ToShortDateString();
            simpleButton7.Text = monday.AddDays(+6).ToShortDateString();
            this.dataGridView2.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
            xtraTabPage1.BackColor = System.Drawing.ColorTranslator.FromHtml("#70D6BC");
            dateTimePicker2.CustomFormat = "dd/mm/yyyy";
            dateTimePicker3.Value = DateTime.Now.AddMonths(-1);
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(@"SELECT r.idno,t.ad_soyad,t.takim_adi,t.telefon,t.mail,t.tc,r.tarih,r.saat,r.ucret,r.fiyat,b.oyuncu1,b.oyuncu2,b.oyuncu3,b.oyuncu4,b.oyuncu5,b.oyuncu6,b.oyuncu7,b.oyuncu8 FROM tliste t INNER JOIN rezervasyon r ON t.idno = r.idno INNER JOIN takim_bilgi b ON t.idno=b.idno", conn)))
                {
                    tbl = new DataTable();
                    da.Fill(tbl);
                    dataGridView2.DataSource = tbl;
                    dataGridView2.AutoGenerateColumns = true;
                    da.Dispose();
                }
            }
            dataGridView2.BorderStyle = BorderStyle.None;
            dataGridView2.AlternatingRowsDefaultCellStyle.BackColor = Color.FromArgb(238, 239, 249);
            dataGridView2.CellBorderStyle = DataGridViewCellBorderStyle.SingleHorizontal;
            dataGridView2.DefaultCellStyle.SelectionBackColor = Color.DarkTurquoise;
            dataGridView2.DefaultCellStyle.SelectionForeColor = Color.WhiteSmoke;
            dataGridView2.BackgroundColor = Color.White;
            dataGridView2.EnableHeadersVisualStyles = false;
            dataGridView2.ColumnHeadersBorderStyle = DataGridViewHeaderBorderStyle.None;
            dataGridView2.ColumnHeadersDefaultCellStyle.BackColor = Color.FromArgb(20, 25, 72);
            dataGridView2.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
            tarihgoster();
            int toplam = 0;
            for (int i = 0; i < dataGridView2.Rows.Count - 1; ++i)
            {
                if (dataGridView2.Rows[i].Cells["ucret"].Value.ToString()== "Ödendi         ")
                {
                    toplam += Convert.ToInt32(dataGridView2.Rows[i].Cells["fiyat"].Value);
                }
            }
            label30.Text = toplam.ToString() + " TL";
        }
        private void button2_Click_1(object sender, EventArgs e)
        {
            
                using (SqlConnection conn = new SqlConnection(veri.source))
            {
                try
                {

                    char[] ayrac = { ' ' };
                    string tarih = dateTimePicker2.Value.ToString();
                    conn.Open();
                    string[] parcalar = tarih.Split(ayrac);
                    string a = DateTime.Parse(parcalar[0]).ToString("yyyy.MM.dd");
                    SqlCommand komut1 = new SqlCommand();
                    komut1.CommandText = String.Format("SELECT saat FROM dbo.rezervasyon WHERE saat = '{0}'", comboBox1.Text);
                    komut1.Connection = conn;
                    SqlCommand komut2 = new SqlCommand();
                    komut2.CommandText = String.Format("SELECT saat FROM dbo.rezervasyon WHERE tarih = '{0}'", a.ToString());
                    komut2.Connection = conn;

                    SqlDataReader sorgu1 = komut1.ExecuteReader();
                    SqlDataReader sorgu2 = komut2.ExecuteReader();
                    sorgu1.Read();
                    sorgu2.Read();
                    if (sorgu1.HasRows == true && sorgu2.HasRows == true)
                    {
                        MessageBox.Show("tarih dolu");
                    }
                    else if (comboBox3.Text != string.Empty)
                    {
                        if (conn.State == ConnectionState.Closed)
                        { conn.Open(); }
                        // Bağlantımızı kontrol ediyoruz, eğer kapalıysa açıyoruz.
                        string kayit = "insert into rezervasyon(takim_adi,tarih,saat,fiyat) values (@takimadi,@tarih,@saat,@fiyat)";
                        // müşteriler tablomuzun ilgili alanlarına kayıt ekleme işlemini gerçekleştirecek sorgumuz.
                        SqlCommand komut = new SqlCommand(kayit, conn);
                        //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
                        komut.Parameters.AddWithValue("@takimadi", textBox2.Text);
                        komut.Parameters.AddWithValue("@tarih", dateTimePicker2.Value);
                        komut.Parameters.AddWithValue("@saat", comboBox1.Text);
                        komut.Parameters.AddWithValue("@fiyat", textBox4.Text);
                        //Parametrelerimize Form üzerinde ki kontrollerden girilen verileri aktarıyoruz.
                        komut.ExecuteNonQuery();
                        //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
                        conn.Close();
                        MessageBox.Show("Kayıt İşlemine devam ediniz.");
                        if (conn.State == ConnectionState.Closed)
                            conn.Open();
                        using (SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(@"SELECT r.idno,t.ad_soyad,t.takim_adi,t.telefon,t.mail,t.tc,r.tarih,r.saat,r.ucret,r.fiyat,b.oyuncu1,b.oyuncu2,b.oyuncu3,b.oyuncu4,b.oyuncu5,b.oyuncu6,b.oyuncu7,b.oyuncu8 FROM tliste t INNER JOIN rezervasyon r ON t.idno = r.idno INNER JOIN takim_bilgi b ON t.idno=b.idno", conn)))
                        {
                            tbl = new DataTable();
                            da.Fill(tbl);
                            dataGridView2.DataSource = tbl;
                            dataGridView2.AutoGenerateColumns = true;
                            da.Dispose();
                        }
                    }
                    sorgu1.Close();
                    sorgu2.Close();
                    conn.Close();
                    comboBox3.Enabled = true;
                    comboBox3.Text = "";
                    comboBox1.Text = "";
                    textBox4.Text = "";
                }
                catch (Exception)
                {
                    MessageBox.Show("İşlem Sırasında Hata Oluştu. Lütfen programcınız ile irtibata geçiniz.");
                }
            }
        }
        private void dataGridView2_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            comboBox1.Text = dataGridView2.CurrentRow.Cells["saat"].Value.ToString().Trim();
            comboBox3.Text = dataGridView2.CurrentRow.Cells["takim_adi"].Value.ToString().Trim();
            textBox9.Text = dataGridView2.CurrentRow.Cells["ad_soyad"].Value.ToString().Trim();
            textBox2.Text = dataGridView2.CurrentRow.Cells["takim_adi"].Value.ToString().Trim();
            textBox3.Text = dataGridView2.CurrentRow.Cells["mail"].Value.ToString().Trim();
            maskedTextBox2.Text = dataGridView2.CurrentRow.Cells["tc"].Value.ToString().Trim();
            maskedTextBox1.Text = dataGridView2.CurrentRow.Cells["telefon"].Value.ToString().Trim();
            combobox2.Text = dataGridView2.CurrentRow.Cells["takim_adi"].Value.ToString().Trim();
        }
        private void textBox7_TextChanged(object sender, EventArgs e)
        {
            DataView dv = tbl.DefaultView;
            dv.RowFilter = "takim_adi like '" + textBox7.Text + "%'";
            dataGridView2.DataSource = dv;
            int toplam = 0;
            for (int i = 0; i < dataGridView2.Rows.Count - 1; ++i)
            {
                if (dataGridView2.Rows[i].Cells["ucret"].Value.ToString() == "Ödendi         ")
                {
                    toplam += Convert.ToInt32(dataGridView2.Rows[i].Cells["fiyat"].Value);
                }
            }
            label30.Text = toplam.ToString() + " TL";
        }
        string cmb1 = "";
        private void comboBox2_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == " ")  //Eğer kişi space tuşuna basar ise cmb1 boşalıyor ve combo temizleniyor
            {
                e.Handled = true;
                combobox2.Text = "";
                cmb1 = "";
                return;
            }
            cmb1 = cmb1 + e.KeyChar.ToString();
            int index = combobox2.FindString(cmb1);
            combobox2.SelectedIndex = index;
            e.Handled = true;
            if (combobox2.Text == "") //Eğer girilen değer comboda kayıtlı değil ise combo ve cmb1 boşalıyor.
            {
                cmb1 = "";
            }
        }
        private void combobox2_Enter(object sender, EventArgs e)
        {
            cmb1 = "";  //Kontrol comboBox2'e gelince cmb1 boşalıyor.
        }
        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                SqlCommand komut = new SqlCommand();
                komut.CommandText = "SELECT * FROM tliste";
                komut.Connection = conn;
                komut.CommandType = CommandType.Text;

                SqlDataReader dr;
                conn.Open();
                dr = komut.ExecuteReader();
                combobox2.Items.Clear();
                while (dr.Read())
                {
                    combobox2.Items.Add(dr["takim_adi"]);
                }
                conn.Close();
            }
        }
        private void button3_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                conn.Open();
                string secmeSorgusu = "SELECT * from rezervasyon where idno=@id";
                //musterino parametresine bağlı olarak müşteri bilgilerini çeken sql kodu
                SqlCommand secmeKomutu = new SqlCommand(secmeSorgusu, conn);
                secmeKomutu.Parameters.AddWithValue("@id", dataGridView2.CurrentRow.Cells["idno"].Value.ToString());
                //musterino parametremize textbox'dan girilen değeri aktarıyoruz.
                SqlDataAdapter da = new SqlDataAdapter(secmeKomutu);
                SqlDataReader dr = secmeKomutu.ExecuteReader();
                //DataReader ile müşteri verilerini veritabanından belleğe aktardık.
                if (dr.Read()) //Datareader herhangi bir okuma yapabiliyorsa aşağıdaki kodlar çalışır.
                {
                    string isim = dr["takim_adi"].ToString().Trim();
                    string id = dr[0].ToString().Trim();
                    dr.Close();
                    //Datareader ile okunan müşteri ad ve soyadını isim değişkenine atadım.
                    //Datareader açık olduğu sürece başka bir sorgu çalıştıramayacağımız için dr nesnesini kapatıyoruz.
                    DialogResult durum = MessageBox.Show(isim + " kaydını silmek istediğinizden emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo);
                    //Kullanıcıya silme onayı penceresi açıp, verdiği cevabı durum değişkenine aktardık.
                    if (DialogResult.Yes == durum) // Eğer kullanıcı Evet seçeneğini seçmişse, veritabanından kaydı silecek kodlar çalışır.
                    {
                        string silmeSorgusu = "DELETE from rezervasyon where idno=@id";
                        string silmesorgusu = "DELETE FROM tliste where idno=@id";
                        string Silmesorgusu = "DELETE FROM takim_bilgi where idno=@id";
                        //musterino parametresine bağlı olarak müşteri kaydını silen sql sorgusu
                        SqlCommand silKomutu = new SqlCommand(silmeSorgusu, conn);
                        silKomutu.Parameters.AddWithValue("@id", dataGridView2.CurrentRow.Cells["idno"].Value.ToString());
                        SqlCommand silKomutu1 = new SqlCommand(silmesorgusu, conn);
                        silKomutu1.Parameters.AddWithValue("@id", dataGridView2.CurrentRow.Cells["idno"].Value.ToString());
                        SqlCommand silKomutu2 = new SqlCommand(Silmesorgusu, conn);
                        silKomutu2.Parameters.AddWithValue("@id", dataGridView2.CurrentRow.Cells["idno"].Value.ToString());

                        silKomutu.ExecuteNonQuery();
                        silKomutu2.ExecuteNonQuery();
                        silKomutu1.ExecuteNonQuery();
                        MessageBox.Show("Kayıt Silindi...");
                        //Silme işlemini gerçekleştirdikten sonra kullanıcıya mesaj verdik.
                        tarihgoster();
                    }
                }
                else
                {
                    MessageBox.Show("Müşteri Bulunamadı.");
                }
                using (da = new SqlDataAdapter(new SqlCommand(@"SELECT r.idno,t.ad_soyad,t.takim_adi,t.telefon,t.mail,t.tc,r.tarih,r.saat,r.ucret,r.fiyat,b.oyuncu1,b.oyuncu2,b.oyuncu3,b.oyuncu4,b.oyuncu5,b.oyuncu6,b.oyuncu7,b.oyuncu8 FROM tliste t INNER JOIN rezervasyon r ON t.idno = r.idno INNER JOIN takim_bilgi b ON t.idno=b.idno", conn)))
                {
                    tbl = new DataTable();
                    da.Fill(tbl);
                    dataGridView2.DataSource = tbl;
                    dataGridView2.AutoGenerateColumns = true;
                    da.Dispose();
                }
                conn.Close();
            }
        }
        public void tarihgoster()
        {
            DateTime input = (dateNavigator1.DateTime);
            int delta = DayOfWeek.Monday - input.DayOfWeek;
            DateTime monday = input.AddDays(delta);
            simpleButton1.Text = monday.ToShortDateString();
            simpleButton2.Text = monday.AddDays(+1).ToShortDateString();
            simpleButton3.Text = monday.AddDays(+2).ToShortDateString();
            simpleButton4.Text = monday.AddDays(+3).ToShortDateString();
            simpleButton5.Text = monday.AddDays(+4).ToShortDateString();
            simpleButton6.Text = monday.AddDays(+5).ToShortDateString();
            simpleButton7.Text = monday.AddDays(+6).ToShortDateString();
            simpleButton8.Text = "12:00 - 13:00";
            simpleButton9.Text = "13:00 - 14:00";
            simpleButton10.Text = "14:00 - 15:00";
            simpleButton11.Text = "15:00 - 16:00";
            simpleButton12.Text = "16:00 - 17:00";
            simpleButton13.Text = "17:00 - 18:00";
            simpleButton14.Text = "18:00 - 19:00";
            simpleButton15.Text = "19:00 - 20:00";
            simpleButton16.Text = "20:00 - 21:00";
            simpleButton17.Text = "21:00 - 22:00";
            simpleButton18.Text = "22:00 - 23:00";
            simpleButton19.Text = "23:00 - 24:00";
            simpleButton31.Text = "12:00 - 13:00";
            simpleButton30.Text = "13:00 - 14:00";
            simpleButton29.Text = "14:00 - 15:00";
            simpleButton28.Text = "15:00 - 16:00";
            simpleButton27.Text = "16:00 - 17:00";
            simpleButton26.Text = "17:00 - 18:00";
            simpleButton25.Text = "18:00 - 19:00";
            simpleButton24.Text = "19:00 - 20:00";
            simpleButton23.Text = "20:00 - 21:00";
            simpleButton22.Text = "21:00 - 22:00";
            simpleButton21.Text = "22:00 - 23:00";
            simpleButton20.Text = "23:00 - 24:00";
            simpleButton43.Text = "12:00 - 13:00";
            simpleButton42.Text = "13:00 - 14:00";
            simpleButton41.Text = "14:00 - 15:00";
            simpleButton40.Text = "15:00 - 16:00";
            simpleButton39.Text = "16:00 - 17:00";
            simpleButton38.Text = "17:00 - 18:00";
            simpleButton37.Text = "18:00 - 19:00";
            simpleButton36.Text = "19:00 - 20:00";
            simpleButton35.Text = "20:00 - 21:00";
            simpleButton34.Text = "21:00 - 22:00";
            simpleButton33.Text = "22:00 - 23:00";
            simpleButton32.Text = "23:00 - 24:00";
            simpleButton55.Text = "12:00 - 13:00";
            simpleButton54.Text = "13:00 - 14:00";
            simpleButton53.Text = "14:00 - 15:00";
            simpleButton52.Text = "15:00 - 16:00";
            simpleButton51.Text = "16:00 - 17:00";
            simpleButton50.Text = "17:00 - 18:00";
            simpleButton49.Text = "18:00 - 19:00";
            simpleButton48.Text = "19:00 - 20:00";
            simpleButton47.Text = "20:00 - 21:00";
            simpleButton46.Text = "21:00 - 22:00";
            simpleButton45.Text = "22:00 - 23:00";
            simpleButton44.Text = "23:00 - 24:00";
            simpleButton67.Text = "12:00 - 13:00";
            simpleButton66.Text = "13:00 - 14:00";
            simpleButton65.Text = "14:00 - 15:00";
            simpleButton64.Text = "15:00 - 16:00";
            simpleButton63.Text = "16:00 - 17:00";
            simpleButton62.Text = "17:00 - 18:00";
            simpleButton61.Text = "18:00 - 19:00";
            simpleButton60.Text = "19:00 - 20:00";
            simpleButton59.Text = "20:00 - 21:00";
            simpleButton58.Text = "21:00 - 22:00";
            simpleButton57.Text = "22:00 - 23:00";
            simpleButton56.Text = "23:00 - 24:00";
            simpleButton79.Text = "12:00 - 13:00";
            simpleButton78.Text = "13:00 - 14:00";
            simpleButton77.Text = "14:00 - 15:00";
            simpleButton76.Text = "15:00 - 16:00";
            simpleButton75.Text = "16:00 - 17:00";
            simpleButton74.Text = "17:00 - 18:00";
            simpleButton73.Text = "18:00 - 19:00";
            simpleButton72.Text = "19:00 - 20:00";
            simpleButton71.Text = "20:00 - 21:00";
            simpleButton70.Text = "21:00 - 22:00";
            simpleButton69.Text = "22:00 - 23:00";
            simpleButton68.Text = "23:00 - 24:00";
            simpleButton91.Text = "12:00 - 13:00";
            simpleButton90.Text = "13:00 - 14:00";
            simpleButton89.Text = "14:00 - 15:00";
            simpleButton88.Text = "15:00 - 16:00";
            simpleButton87.Text = "16:00 - 17:00";
            simpleButton86.Text = "17:00 - 18:00";
            simpleButton85.Text = "18:00 - 19:00";
            simpleButton84.Text = "19:00 - 20:00";
            simpleButton83.Text = "20:00 - 21:00";
            simpleButton82.Text = "21:00 - 22:00";
            simpleButton81.Text = "22:00 - 23:00";
            simpleButton80.Text = "23:00 - 24:00";
            simpleButton8.Appearance.BackColor = Color.White;
            simpleButton9.Appearance.BackColor = Color.White;
            simpleButton10.Appearance.BackColor = Color.White;
            simpleButton11.Appearance.BackColor = Color.White;
            simpleButton12.Appearance.BackColor = Color.White;
            simpleButton13.Appearance.BackColor = Color.White;
            simpleButton14.Appearance.BackColor = Color.White;
            simpleButton15.Appearance.BackColor = Color.White;
            simpleButton16.Appearance.BackColor = Color.White;
            simpleButton17.Appearance.BackColor = Color.White;
            simpleButton18.Appearance.BackColor = Color.White;
            simpleButton19.Appearance.BackColor = Color.White;
            simpleButton20.Appearance.BackColor = Color.White;
            simpleButton21.Appearance.BackColor = Color.White;
            simpleButton22.Appearance.BackColor = Color.White;
            simpleButton23.Appearance.BackColor = Color.White;
            simpleButton24.Appearance.BackColor = Color.White;
            simpleButton25.Appearance.BackColor = Color.White;
            simpleButton26.Appearance.BackColor = Color.White;
            simpleButton27.Appearance.BackColor = Color.White;
            simpleButton28.Appearance.BackColor = Color.White;
            simpleButton29.Appearance.BackColor = Color.White;
            simpleButton30.Appearance.BackColor = Color.White;
            simpleButton31.Appearance.BackColor = Color.White;
            simpleButton32.Appearance.BackColor = Color.White;
            simpleButton33.Appearance.BackColor = Color.White;
            simpleButton34.Appearance.BackColor = Color.White;
            simpleButton35.Appearance.BackColor = Color.White;
            simpleButton36.Appearance.BackColor = Color.White;
            simpleButton37.Appearance.BackColor = Color.White;
            simpleButton38.Appearance.BackColor = Color.White;
            simpleButton39.Appearance.BackColor = Color.White;
            simpleButton40.Appearance.BackColor = Color.White;
            simpleButton41.Appearance.BackColor = Color.White;
            simpleButton42.Appearance.BackColor = Color.White;
            simpleButton43.Appearance.BackColor = Color.White;
            simpleButton44.Appearance.BackColor = Color.White;
            simpleButton45.Appearance.BackColor = Color.White;
            simpleButton46.Appearance.BackColor = Color.White;
            simpleButton47.Appearance.BackColor = Color.White;
            simpleButton48.Appearance.BackColor = Color.White;
            simpleButton49.Appearance.BackColor = Color.White;
            simpleButton50.Appearance.BackColor = Color.White;
            simpleButton51.Appearance.BackColor = Color.White;
            simpleButton52.Appearance.BackColor = Color.White;
            simpleButton53.Appearance.BackColor = Color.White;
            simpleButton54.Appearance.BackColor = Color.White;
            simpleButton55.Appearance.BackColor = Color.White;
            simpleButton56.Appearance.BackColor = Color.White;
            simpleButton57.Appearance.BackColor = Color.White;
            simpleButton58.Appearance.BackColor = Color.White;
            simpleButton59.Appearance.BackColor = Color.White;
            simpleButton60.Appearance.BackColor = Color.White;
            simpleButton61.Appearance.BackColor = Color.White;
            simpleButton62.Appearance.BackColor = Color.White;
            simpleButton63.Appearance.BackColor = Color.White;
            simpleButton64.Appearance.BackColor = Color.White;
            simpleButton65.Appearance.BackColor = Color.White;
            simpleButton66.Appearance.BackColor = Color.White;
            simpleButton67.Appearance.BackColor = Color.White;
            simpleButton68.Appearance.BackColor = Color.White;
            simpleButton69.Appearance.BackColor = Color.White;
            simpleButton70.Appearance.BackColor = Color.White;
            simpleButton71.Appearance.BackColor = Color.White;
            simpleButton72.Appearance.BackColor = Color.White;
            simpleButton73.Appearance.BackColor = Color.White;
            simpleButton74.Appearance.BackColor = Color.White;
            simpleButton75.Appearance.BackColor = Color.White;
            simpleButton76.Appearance.BackColor = Color.White;
            simpleButton77.Appearance.BackColor = Color.White;
            simpleButton78.Appearance.BackColor = Color.White;
            simpleButton79.Appearance.BackColor = Color.White;
            simpleButton80.Appearance.BackColor = Color.White;
            simpleButton81.Appearance.BackColor = Color.White;
            simpleButton82.Appearance.BackColor = Color.White;
            simpleButton83.Appearance.BackColor = Color.White;
            simpleButton84.Appearance.BackColor = Color.White;
            simpleButton85.Appearance.BackColor = Color.White;
            simpleButton86.Appearance.BackColor = Color.White;
            simpleButton87.Appearance.BackColor = Color.White;
            simpleButton88.Appearance.BackColor = Color.White;
            simpleButton89.Appearance.BackColor = Color.White;
            simpleButton90.Appearance.BackColor = Color.White;
            simpleButton91.Appearance.BackColor = Color.White;
            simpleButton8.Appearance.ForeColor = Color.Black;
            simpleButton9.Appearance.ForeColor = Color.Black;
            simpleButton10.Appearance.ForeColor = Color.Black;
            simpleButton11.Appearance.ForeColor = Color.Black;
            simpleButton12.Appearance.ForeColor = Color.Black;
            simpleButton13.Appearance.ForeColor = Color.Black;
            simpleButton14.Appearance.ForeColor = Color.Black;
            simpleButton15.Appearance.ForeColor = Color.Black;
            simpleButton16.Appearance.ForeColor = Color.Black;
            simpleButton17.Appearance.ForeColor = Color.Black;
            simpleButton18.Appearance.ForeColor = Color.Black;
            simpleButton19.Appearance.ForeColor = Color.Black;
            simpleButton20.Appearance.ForeColor = Color.Black;
            simpleButton21.Appearance.ForeColor = Color.Black;
            simpleButton22.Appearance.ForeColor = Color.Black;
            simpleButton23.Appearance.ForeColor = Color.Black;
            simpleButton24.Appearance.ForeColor = Color.Black;
            simpleButton25.Appearance.ForeColor = Color.Black;
            simpleButton26.Appearance.ForeColor = Color.Black;
            simpleButton27.Appearance.ForeColor = Color.Black;
            simpleButton28.Appearance.ForeColor = Color.Black;
            simpleButton29.Appearance.ForeColor = Color.Black;
            simpleButton30.Appearance.ForeColor = Color.Black;
            simpleButton31.Appearance.ForeColor = Color.Black;
            simpleButton32.Appearance.ForeColor = Color.Black;
            simpleButton33.Appearance.ForeColor = Color.Black;
            simpleButton34.Appearance.ForeColor = Color.Black;
            simpleButton35.Appearance.ForeColor = Color.Black;
            simpleButton36.Appearance.ForeColor = Color.Black;
            simpleButton37.Appearance.ForeColor = Color.Black;
            simpleButton38.Appearance.ForeColor = Color.Black;
            simpleButton39.Appearance.ForeColor = Color.Black;
            simpleButton40.Appearance.ForeColor = Color.Black;
            simpleButton41.Appearance.ForeColor = Color.Black;
            simpleButton42.Appearance.ForeColor = Color.Black;
            simpleButton43.Appearance.ForeColor = Color.Black;
            simpleButton44.Appearance.ForeColor = Color.Black;
            simpleButton45.Appearance.ForeColor = Color.Black;
            simpleButton46.Appearance.ForeColor = Color.Black;
            simpleButton47.Appearance.ForeColor = Color.Black;
            simpleButton48.Appearance.ForeColor = Color.Black;
            simpleButton49.Appearance.ForeColor = Color.Black;
            simpleButton50.Appearance.ForeColor = Color.Black;
            simpleButton51.Appearance.ForeColor = Color.Black;
            simpleButton52.Appearance.ForeColor = Color.Black;
            simpleButton53.Appearance.ForeColor = Color.Black;
            simpleButton54.Appearance.ForeColor = Color.Black;
            simpleButton55.Appearance.ForeColor = Color.Black;
            simpleButton56.Appearance.ForeColor = Color.Black;
            simpleButton57.Appearance.ForeColor = Color.Black;
            simpleButton58.Appearance.ForeColor = Color.Black;
            simpleButton59.Appearance.ForeColor = Color.Black;
            simpleButton60.Appearance.ForeColor = Color.Black;
            simpleButton61.Appearance.ForeColor = Color.Black;
            simpleButton62.Appearance.ForeColor = Color.Black;
            simpleButton63.Appearance.ForeColor = Color.Black;
            simpleButton64.Appearance.ForeColor = Color.Black;
            simpleButton65.Appearance.ForeColor = Color.Black;
            simpleButton66.Appearance.ForeColor = Color.Black;
            simpleButton67.Appearance.ForeColor = Color.Black;
            simpleButton68.Appearance.ForeColor = Color.Black;
            simpleButton69.Appearance.ForeColor = Color.Black;
            simpleButton70.Appearance.ForeColor = Color.Black;
            simpleButton71.Appearance.ForeColor = Color.Black;
            simpleButton72.Appearance.ForeColor = Color.Black;
            simpleButton73.Appearance.ForeColor = Color.Black;
            simpleButton74.Appearance.ForeColor = Color.Black;
            simpleButton75.Appearance.ForeColor = Color.Black;
            simpleButton76.Appearance.ForeColor = Color.Black;
            simpleButton77.Appearance.ForeColor = Color.Black;
            simpleButton78.Appearance.ForeColor = Color.Black;
            simpleButton79.Appearance.ForeColor = Color.Black;
            simpleButton80.Appearance.ForeColor = Color.Black;
            simpleButton81.Appearance.ForeColor = Color.Black;
            simpleButton82.Appearance.ForeColor = Color.Black;
            simpleButton83.Appearance.ForeColor = Color.Black;
            simpleButton84.Appearance.ForeColor = Color.Black;
            simpleButton85.Appearance.ForeColor = Color.Black;
            simpleButton86.Appearance.ForeColor = Color.Black;
            simpleButton87.Appearance.ForeColor = Color.Black;
            simpleButton88.Appearance.ForeColor = Color.Black;
            simpleButton89.Appearance.ForeColor = Color.Black;
            simpleButton90.Appearance.ForeColor = Color.Black;
            simpleButton91.Appearance.ForeColor = Color.Black;
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(@"SELECT * From rezervasyon", conn)))
                {
                    SqlDataReader oku;
                    oku = da.SelectCommand.ExecuteReader();
                    while (oku.Read())
                    {
                        char[] ayrac = { ' ' };
                        string tarih = oku[2].ToString();
                        string[] parcalar = tarih.Split(ayrac);
                        if (parcalar[0] == simpleButton1.Text)
                        {
                            switch (oku[3].ToString().Trim())
                            {
                                case "23:00 - 24:00":
                                    {
                                        simpleButton19.Text = "  " + oku[1].ToString();
                                        simpleButton19.ForeColor = System.Drawing.Color.Red;
                                        simpleButton19.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton19.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "22:00 - 23:00":
                                    {
                                        simpleButton18.Text = "  " + oku[1].ToString();
                                        simpleButton18.ForeColor = System.Drawing.Color.Red;
                                        simpleButton18.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton18.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "21:00 - 22:00":
                                    {
                                        simpleButton17.Text = "  " + oku[1].ToString();
                                        simpleButton17.ForeColor = System.Drawing.Color.Red;
                                        simpleButton17.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton17.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "20:00 - 21:00":
                                    {
                                        simpleButton16.Text = "  " + oku[1].ToString();
                                        simpleButton16.ForeColor = System.Drawing.Color.Red;
                                        simpleButton16.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton16.Appearance.BackColor = Color.Yellow;
                                        }

                                        break;
                                    }
                                case "19:00 - 20:00":
                                    {
                                        simpleButton15.Text = "  " + oku[1].ToString();
                                        simpleButton15.ForeColor = System.Drawing.Color.Red;
                                        simpleButton15.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton15.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "18:00 - 19:00":
                                    {
                                        simpleButton14.Text = "  " + oku[1].ToString();
                                        simpleButton14.ForeColor = System.Drawing.Color.Red;
                                        simpleButton14.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton14.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "17:00 - 18:00":
                                    {
                                        simpleButton13.Text = "  " + oku[1].ToString();
                                        simpleButton13.ForeColor = System.Drawing.Color.Red;
                                        simpleButton13.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton13.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "16:00 - 17:00":
                                    {
                                        simpleButton12.Text = "  " + oku[1].ToString();
                                        simpleButton12.ForeColor = System.Drawing.Color.Red;
                                        simpleButton12.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton12.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "15:00 - 16:00":
                                    {
                                        simpleButton11.Text = "  " + oku[1].ToString();
                                        simpleButton11.ForeColor = System.Drawing.Color.Red;
                                        simpleButton11.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton11.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "14:00 - 15:00":
                                    {
                                        simpleButton10.Text = "  " + oku[1].ToString();
                                        simpleButton10.ForeColor = System.Drawing.Color.Red;
                                        simpleButton10.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton10.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "13:00 - 14:00":
                                    {
                                        simpleButton9.Text = "  " + oku[1].ToString();
                                        simpleButton9.ForeColor = System.Drawing.Color.Red;
                                        simpleButton9.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton9.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "12:00 - 13:00":
                                    {
                                        simpleButton8.Text = "  " + oku[1].ToString();
                                        simpleButton8.ForeColor = System.Drawing.Color.Red;
                                        simpleButton8.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton8.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                            }
                        }
                        if (parcalar[0] == simpleButton2.Text)
                        {
                            switch (oku[3].ToString().Trim())
                            {
                                case "23:00 - 24:00":
                                    {
                                        simpleButton20.Text = "  " + oku[1].ToString();
                                        simpleButton20.ForeColor = System.Drawing.Color.Red;
                                        simpleButton20.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton20.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "22:00 - 23:00":
                                    {
                                        simpleButton21.Text = "  " + oku[1].ToString();
                                        simpleButton21.ForeColor = System.Drawing.Color.Red;
                                        simpleButton21.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton21.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "21:00 - 22:00":
                                    {
                                        simpleButton22.Text = "  " + oku[1].ToString();
                                        simpleButton22.ForeColor = System.Drawing.Color.Red;
                                        simpleButton22.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton22.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "20:00 - 21:00":
                                    {
                                        simpleButton23.Text = "  " + oku[1].ToString();
                                        simpleButton23.ForeColor = System.Drawing.Color.Red;
                                        simpleButton23.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton23.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "19:00 - 20:00":
                                    {
                                        simpleButton24.Text = "  " + oku[1].ToString();
                                        simpleButton24.ForeColor = System.Drawing.Color.Red;
                                        simpleButton24.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton24.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "18:00 - 19:00":
                                    {
                                        simpleButton25.Text = "  " + oku[1].ToString();
                                        simpleButton25.ForeColor = System.Drawing.Color.Red;
                                        simpleButton25.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton25.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "17:00 - 18:00":
                                    {
                                        simpleButton26.Text = "  " + oku[1].ToString();
                                        simpleButton26.ForeColor = System.Drawing.Color.Red;
                                        simpleButton26.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton26.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "16:00 - 17:00":
                                    {
                                        simpleButton27.Text = "  " + oku[1].ToString();
                                        simpleButton27.ForeColor = System.Drawing.Color.Red;
                                        simpleButton27.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton27.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "15:00 - 16:00":
                                    {
                                        simpleButton28.Text = "  " + oku[1].ToString();
                                        simpleButton28.ForeColor = System.Drawing.Color.Red;
                                        simpleButton28.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton28.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "14:00 - 15:00":
                                    {
                                        simpleButton29.Text = "  " + oku[1].ToString();
                                        simpleButton29.ForeColor = System.Drawing.Color.Red;
                                        simpleButton29.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton29.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "13:00 - 14:00":
                                    {
                                        simpleButton30.Text = "  " + oku[1].ToString();
                                        simpleButton30.ForeColor = System.Drawing.Color.Red;
                                        simpleButton30.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton30.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "12:00 - 13:00":
                                    {
                                        simpleButton31.Text = "  " + oku[1].ToString();
                                        simpleButton31.ForeColor = System.Drawing.Color.Red;
                                        simpleButton31.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton31.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                            }
                        }
                        if (parcalar[0] == simpleButton3.Text)
                        {
                            switch (oku[3].ToString().Trim())
                            {
                                case "23:00 - 24:00":
                                    {
                                        simpleButton32.Text = "  " + oku[1].ToString();
                                        simpleButton32.ForeColor = System.Drawing.Color.Red;
                                        simpleButton32.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton32.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "22:00 - 23:00":
                                    {
                                        simpleButton33.Text = "  " + oku[1].ToString();
                                        simpleButton33.ForeColor = System.Drawing.Color.Red;
                                        simpleButton33.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton33.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "21:00 - 22:00":
                                    {
                                        simpleButton34.Text = "  " + oku[1].ToString();
                                        simpleButton34.ForeColor = System.Drawing.Color.Red;
                                        simpleButton34.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton34.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "20:00 - 21:00":
                                    {
                                        simpleButton35.Text = "  " + oku[1].ToString();
                                        simpleButton35.ForeColor = System.Drawing.Color.Red;
                                        simpleButton35.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton35.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "19:00 - 20:00":
                                    {
                                        simpleButton36.Text = "  " + oku[1].ToString();
                                        simpleButton36.ForeColor = System.Drawing.Color.Red;
                                        simpleButton36.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton36.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "18:00 - 19:00":
                                    {
                                        simpleButton37.Text = "  " + oku[1].ToString();
                                        simpleButton37.ForeColor = System.Drawing.Color.Red;
                                        simpleButton37.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton37.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "17:00 - 18:00":
                                    {
                                        simpleButton38.Text = "  " + oku[1].ToString();
                                        simpleButton38.ForeColor = System.Drawing.Color.Red;
                                        simpleButton38.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton38.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "16:00 - 17:00":
                                    {
                                        simpleButton39.Text = "  " + oku[1].ToString();
                                        simpleButton39.ForeColor = System.Drawing.Color.Red;
                                        simpleButton39.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton39.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "15:00 - 16:00":
                                    {
                                        simpleButton40.Text = "  " + oku[1].ToString();
                                        simpleButton40.ForeColor = System.Drawing.Color.Red;
                                        simpleButton40.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton40.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "14:00 - 15:00":
                                    {
                                        simpleButton41.Text = "  " + oku[1].ToString();
                                        simpleButton41.ForeColor = System.Drawing.Color.Red;
                                        simpleButton41.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton41.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "13:00 - 14:00":
                                    {
                                        simpleButton42.Text = "  " + oku[1].ToString();
                                        simpleButton42.ForeColor = System.Drawing.Color.Red;
                                        simpleButton42.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton42.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "12:00 - 13:00":
                                    {
                                        simpleButton43.Text = "  " + oku[1].ToString();
                                        simpleButton43.ForeColor = System.Drawing.Color.Red;
                                        simpleButton43.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton43.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                            }
                        }
                        if (parcalar[0] == simpleButton4.Text)
                        {
                            switch (oku[3].ToString().Trim())
                            {
                                case "23:00 - 24:00":
                                    {
                                        simpleButton44.Text = "  " + oku[1].ToString();
                                        simpleButton44.ForeColor = System.Drawing.Color.Red;
                                        simpleButton44.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton44.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "22:00 - 23:00":
                                    {
                                        simpleButton45.Text = "  " + oku[1].ToString();
                                        simpleButton45.ForeColor = System.Drawing.Color.Red;
                                        simpleButton45.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton45.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "21:00 - 22:00":
                                    {
                                        simpleButton46.Text = "  " + oku[1].ToString();
                                        simpleButton46.ForeColor = System.Drawing.Color.Red;
                                        simpleButton46.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton46.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "20:00 - 21:00":
                                    {
                                        simpleButton47.Text = "  " + oku[1].ToString();
                                        simpleButton47.ForeColor = System.Drawing.Color.Red;
                                        simpleButton47.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton47.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "19:00 - 20:00":
                                    {
                                        simpleButton48.Text = "  " + oku[1].ToString();
                                        simpleButton48.ForeColor = System.Drawing.Color.Red;
                                        simpleButton48.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton48.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "18:00 - 19:00":
                                    {
                                        simpleButton49.Text = "  " + oku[1].ToString();
                                        simpleButton49.ForeColor = System.Drawing.Color.Red;
                                        simpleButton49.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton49.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "17:00 - 18:00":
                                    {
                                        simpleButton50.Text = "  " + oku[1].ToString();
                                        simpleButton50.ForeColor = System.Drawing.Color.Red;
                                        simpleButton50.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton50.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "16:00 - 17:00":
                                    {
                                        simpleButton51.Text = "  " + oku[1].ToString();
                                        simpleButton51.ForeColor = System.Drawing.Color.Red;
                                        simpleButton51.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton51.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "15:00 - 16:00":
                                    {
                                        simpleButton52.Text = "  " + oku[1].ToString();
                                        simpleButton52.ForeColor = System.Drawing.Color.Red;
                                        simpleButton52.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton15.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "14:00 - 15:00":
                                    {
                                        simpleButton53.Text = "  " + oku[1].ToString();
                                        simpleButton53.ForeColor = System.Drawing.Color.Red;
                                        simpleButton53.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton53.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "13:00 - 14:00":
                                    {
                                        simpleButton54.Text = "  " + oku[1].ToString();
                                        simpleButton54.ForeColor = System.Drawing.Color.Red;
                                        simpleButton54.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton54.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "12:00 - 13:00":
                                    {
                                        simpleButton55.Text = "  " + oku[1].ToString();
                                        simpleButton55.ForeColor = System.Drawing.Color.Red;
                                        simpleButton55.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton55.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                            }
                        }
                        if (parcalar[0] == simpleButton5.Text)
                        {
                            switch (oku[3].ToString().Trim())
                            {
                                case "23:00 - 24:00":
                                    {
                                        simpleButton56.Text = "  " + oku[1].ToString();
                                        simpleButton56.ForeColor = System.Drawing.Color.Red;
                                        simpleButton56.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton56.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "22:00 - 23:00":
                                    {
                                        simpleButton57.Text = "  " + oku[1].ToString();
                                        simpleButton57.ForeColor = System.Drawing.Color.Red;
                                        simpleButton57.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton57.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "21:00 - 22:00":
                                    {
                                        simpleButton58.Text = "  " + oku[1].ToString();
                                        simpleButton58.ForeColor = System.Drawing.Color.Red;
                                        simpleButton58.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton58.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "20:00 - 21:00":
                                    {
                                        simpleButton59.Text = "  " + oku[1].ToString();
                                        simpleButton59.ForeColor = System.Drawing.Color.Red;
                                        simpleButton59.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton59.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "19:00 - 20:00":
                                    {
                                        simpleButton60.Text = "  " + oku[1].ToString();
                                        simpleButton60.ForeColor = System.Drawing.Color.Red;
                                        simpleButton60.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton60.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "18:00 - 19:00":
                                    {
                                        simpleButton61.Text = "  " + oku[1].ToString();
                                        simpleButton61.ForeColor = System.Drawing.Color.Red;
                                        simpleButton61.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton61.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "17:00 - 18:00":
                                    {
                                        simpleButton62.Text = "  " + oku[1].ToString();
                                        simpleButton62.ForeColor = System.Drawing.Color.Red;
                                        simpleButton62.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton62.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "16:00 - 17:00":
                                    {
                                        simpleButton63.Text = "  " + oku[1].ToString();
                                        simpleButton63.ForeColor = System.Drawing.Color.Red;
                                        simpleButton63.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton63.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "15:00 - 16:00":
                                    {
                                        simpleButton64.Text = "  " + oku[1].ToString();
                                        simpleButton64.ForeColor = System.Drawing.Color.Red;
                                        simpleButton64.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton64.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "14:00 - 15:00":
                                    {
                                        simpleButton65.Text = "  " + oku[1].ToString();
                                        simpleButton65.ForeColor = System.Drawing.Color.Red;
                                        simpleButton65.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton65.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "13:00 - 14:00":
                                    {
                                        simpleButton66.Text = "  " + oku[1].ToString();
                                        simpleButton66.ForeColor = System.Drawing.Color.Red;
                                        simpleButton66.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton66.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "12:00 - 13:00":
                                    {
                                        simpleButton67.Text = "  " + oku[1].ToString();
                                        simpleButton67.ForeColor = System.Drawing.Color.Red;
                                        simpleButton67.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton67.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                            }
                        }
                        if (parcalar[0] == simpleButton6.Text)
                        {
                            switch (oku[3].ToString().Trim())
                            {
                                case "23:00 - 24:00":
                                    {
                                        simpleButton68.Text = "  " + oku[1].ToString();
                                        this.simpleButton68.BackColor = Color.Red;
                                        simpleButton68.ForeColor = System.Drawing.Color.Red;
                                        simpleButton68.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton68.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "22:00 - 23:00":
                                    {
                                        simpleButton69.Text = "  " + oku[1].ToString();
                                        simpleButton69.ForeColor = System.Drawing.Color.Red;
                                        simpleButton69.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton69.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "21:00 - 22:00":
                                    {
                                        simpleButton69.Text = "  " + oku[1].ToString();
                                        simpleButton69.ForeColor = System.Drawing.Color.Red;
                                        simpleButton69.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton69.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "20:00 - 21:00":
                                    {
                                        simpleButton70.Text = "  " + oku[1].ToString();
                                        simpleButton70.ForeColor = System.Drawing.Color.Red;
                                        simpleButton70.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton70.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "19:00 - 20:00":
                                    {
                                        simpleButton71.Text = "  " + oku[1].ToString();
                                        simpleButton71.ForeColor = System.Drawing.Color.Red;
                                        simpleButton71.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton71.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "18:00 - 19:00":
                                    {
                                        simpleButton72.Text = "  " + oku[1].ToString();
                                        simpleButton72.ForeColor = System.Drawing.Color.Red;
                                        simpleButton72.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton72.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "17:00 - 18:00":
                                    {
                                        simpleButton73.Text = "  " + oku[1].ToString();
                                        simpleButton73.ForeColor = System.Drawing.Color.Red;
                                        simpleButton73.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton73.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "16:00 - 17:00":
                                    {
                                        simpleButton74.Text = "  " + oku[1].ToString();
                                        simpleButton74.ForeColor = System.Drawing.Color.Red;
                                        simpleButton74.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton74.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "15:00 - 16:00":
                                    {
                                        simpleButton75.Text = "  " + oku[1].ToString();
                                        simpleButton75.ForeColor = System.Drawing.Color.Red;
                                        simpleButton75.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton75.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "14:00 - 15:00":
                                    {
                                        simpleButton76.Text = "  " + oku[1].ToString();
                                        simpleButton76.ForeColor = System.Drawing.Color.Red;
                                        simpleButton76.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton76.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "13:00 - 14:00":
                                    {
                                        simpleButton77.Text = "  " + oku[1].ToString();
                                        simpleButton77.ForeColor = System.Drawing.Color.Red;
                                        simpleButton77.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton77.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "12:00 - 13:00":
                                    {
                                        simpleButton78.Text = "  " + oku[1].ToString();
                                        simpleButton78.ForeColor = System.Drawing.Color.Red;
                                        simpleButton78.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton78.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                            }
                        }
                        if (parcalar[0] == simpleButton7.Text)
                        {
                            switch (oku[3].ToString().Trim())
                            {
                                case "23:00 - 24:00":
                                    {
                                        simpleButton79.Text = "  " + oku[1].ToString();
                                        simpleButton79.ForeColor = System.Drawing.Color.Red;
                                        simpleButton79.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton79.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "22:00 - 23:00":
                                    {
                                        simpleButton80.Text = "  " + oku[1].ToString();
                                        simpleButton80.ForeColor = System.Drawing.Color.Red;
                                        simpleButton80.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton80.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "21:00 - 22:00":
                                    {
                                        simpleButton81.Text = "  " + oku[1].ToString();
                                        simpleButton81.ForeColor = System.Drawing.Color.Red;
                                        simpleButton81.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton81.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "20:00 - 21:00":
                                    {
                                        simpleButton82.Text = "  " + oku[1].ToString();
                                        simpleButton82.ForeColor = System.Drawing.Color.Red;
                                        simpleButton82.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton82.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "19:00 - 20:00":
                                    {
                                        simpleButton83.Text = "  " + oku[1].ToString();
                                        simpleButton83.ForeColor = System.Drawing.Color.Red;
                                        simpleButton83.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton83.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "18:00 - 19:00":
                                    {
                                        simpleButton84.Text = "  " + oku[1].ToString();
                                        simpleButton84.ForeColor = System.Drawing.Color.Red;
                                        simpleButton84.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton84.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "17:00 - 18:00":
                                    {
                                        simpleButton85.Text = "  " + oku[1].ToString();
                                        simpleButton85.ForeColor = System.Drawing.Color.Red;
                                        simpleButton85.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton85.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "16:00 - 17:00":
                                    {
                                        simpleButton86.Text = "  " + oku[1].ToString();
                                        simpleButton86.ForeColor = System.Drawing.Color.Red;
                                        simpleButton86.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton86.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "15:00 - 16:00":
                                    {
                                        simpleButton87.Text = "  " + oku[1].ToString();
                                        simpleButton87.ForeColor = System.Drawing.Color.Red;
                                        simpleButton87.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton87.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "14:00 - 15:00":
                                    {
                                        simpleButton88.Text = "  " + oku[1].ToString();
                                        simpleButton88.ForeColor = System.Drawing.Color.Red;
                                        simpleButton88.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton88.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "13:00 - 14:00":
                                    {
                                        simpleButton89.Text = "  " + oku[1].ToString();
                                        simpleButton89.ForeColor = System.Drawing.Color.Red;
                                        simpleButton89.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton89.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                                case "12:00 - 13:00":
                                    {
                                        simpleButton90.Text = "  " + oku[1].ToString();
                                        simpleButton90.ForeColor = System.Drawing.Color.Red;
                                        simpleButton90.Appearance.BackColor = Color.MediumSeaGreen;
                                        if (oku[4].ToString().Trim() == "Ödendi")
                                        {
                                            simpleButton90.Appearance.BackColor = Color.Yellow;
                                        }
                                        break;
                                    }
                            }
                        }
                    }
                }
                conn.Close();
            }
        }
        private void dateTimePicker3_ValueChanged(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(@"SELECT r.idno,t.ad_soyad,t.takim_adi,t.telefon,t.mail,t.tc,r.tarih,r.saat,r.ucret,r.fiyat,b.oyuncu1,b.oyuncu2,b.oyuncu3,b.oyuncu4,b.oyuncu5,b.oyuncu6,b.oyuncu7,b.oyuncu8 FROM tliste t INNER JOIN rezervasyon r ON t.idno = r.idno INNER JOIN takim_bilgi b ON t.idno=b.idno WHERE tarih >= @STarih AND tarih < @ETarih", conn)))
                {
                    
                    da.SelectCommand.Parameters.AddWithValue("@STarih", dateTimePicker3.Value.Date);
                    da.SelectCommand.Parameters.AddWithValue("@ETarih", dateTimePicker4.Value.Date.AddDays(1));
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    dataGridView2.DataSource = tbl;
                    dataGridView2.AutoGenerateColumns = true;
                    int toplam = 0;
                    for (int i = 0; i < dataGridView2.Rows.Count - 1; ++i)
                    {
                        if (dataGridView2.Rows[i].Cells["ucret"].Value.ToString() == "Ödendi         ")
                        {
                            toplam += Convert.ToInt32(dataGridView2.Rows[i].Cells["fiyat"].Value);
                        }
                    }
                    label30.Text = toplam.ToString() + " TL";
                    da.Dispose();
                }
                conn.Close();
            }
        }
        private void dateTimePicker4_ValueChanged(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                if (conn.State == ConnectionState.Closed) conn.Open();
                using (SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(@"SELECT r.idno,t.ad_soyad,t.takim_adi,t.telefon,t.mail,t.tc,r.tarih,r.saat,r.ucret,r.fiyat,b.oyuncu1,b.oyuncu2,b.oyuncu3,b.oyuncu4,b.oyuncu5,b.oyuncu6,b.oyuncu7,b.oyuncu8 FROM tliste t INNER JOIN rezervasyon r ON t.idno = r.idno INNER JOIN takim_bilgi b ON t.idno=b.idno WHERE tarih >= @STarih AND tarih < @ETarih", conn)))
                {
                    da.SelectCommand.Parameters.AddWithValue("@STarih", dateTimePicker3.Value.Date);
                    da.SelectCommand.Parameters.AddWithValue("@ETarih", dateTimePicker4.Value.Date.AddDays(1));
                    DataTable tbl = new DataTable();
                    da.Fill(tbl);
                    dataGridView2.DataSource = tbl;
                    dataGridView2.AutoGenerateColumns = true;
                    int toplam = 0;
                    for (int i = 0; i < dataGridView2.Rows.Count - 1; ++i)
                    {
                        if (dataGridView2.Rows[i].Cells["ucret"].Value.ToString() == "Ödendi         ")
                        {
                            toplam += Convert.ToInt32(dataGridView2.Rows[i].Cells["fiyat"].Value);
                        }
                    }
                    label30.Text = toplam.ToString() + " TL";
                    da.Dispose();
                }
                conn.Close();
            }
        }
        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            tarihgoster();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            textBox2.Text = "";
            comboBox1.Text = "";
            maskedTextBox1.Text = "";
            dateTimePicker2.Text = null;
        }
        private const string MatchEmailPattern =
                  @"^(([\w-]+\.)+[\w-]+|([a-zA-Z]{1}|[\w-]{2,}))@"
           + @"((([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
                                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])\."
           + @"([0-1]?[0-9]{1,2}|25[0-5]|2[0-4][0-9])\.([0-1]?
                                                [0-9]{1,2}|25[0-5]|2[0-4][0-9])){1}|"
           + @"([a-zA-Z]+[\w-]+\.)+[a-zA-Z]{2,4})$";
        string takimadi;
        private void button8_Click(object sender, EventArgs e)
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
                        komut.Parameters.AddWithValue("@adsoyad", textBox9.Text);
                        komut.Parameters.AddWithValue("@takimadi", textBox2.Text);
                        komut.Parameters.AddWithValue("@telefon", maskedTextBox1.Text);
                        komut.Parameters.AddWithValue("@mail", textBox3.Text);
                        komut.Parameters.AddWithValue("@tc", maskedTextBox2.Text);
                        //Parametrelerimize Form üzerinde ki kontrollerden girilen verileri aktarıyoruz.
                        komut.ExecuteNonQuery();
                        //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
                        conn.Close();
                        MessageBox.Show("Kayıt İşlemine devam ediniz.");
                        using (SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(@"SELECT r.idno,t.ad_soyad,t.takim_adi,t.telefon,t.mail,t.tc,r.tarih,r.saat,r.ucret,r.fiyat,b.oyuncu1,b.oyuncu2,b.oyuncu3,b.oyuncu4,b.oyuncu5,b.oyuncu6,b.oyuncu7,b.oyuncu8 FROM tliste t INNER JOIN rezervasyon r ON t.idno = r.idno INNER JOIN takim_bilgi b ON t.idno=b.idno", conn)))
                        {
                            tbl = new DataTable();
                            da.Fill(tbl);
                            dataGridView2.DataSource = tbl;
                            dataGridView2.AutoGenerateColumns = true;
                            da.Dispose();
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Mail Adresini ve diğer bilgileri düzgün girdiginizden emin olun");
                }
                takimadi = textBox2.Text;
                comboBox3.Text = takimadi;
                comboBox3.Enabled = false;
                combobox2.Text = takimadi;
                combobox2.Enabled = false;
            }
            catch (Exception)
            {

                MessageBox.Show("Takım adi alınmıştır. Lütfen farklı bir takım adi giriniz.");
            }
        }
        int sayac = 0;
        private void button5_Click(object sender, EventArgs e)
        {
            combobox2.Enabled = false;
            if (sayac < 9)
            {
                sayac++;
                listBox1.Items.Add(textBox8.Text);
            }
        }
        private void button7_Click(object sender, EventArgs e)
        {
            try
            {
                using (SqlConnection conn = new SqlConnection(veri.source))
                {
                    if (conn.State == ConnectionState.Closed) conn.Open();
                    // Bağlantımızı kontrol ediyoruz, eğer kapalıysa açıyoruz.
                    string kayit = "insert into takim_bilgi(takim_adi,oyuncu1,oyuncu2,oyuncu3,oyuncu4,oyuncu5,oyuncu6,oyuncu7,oyuncu8) values (@takimadi,@oyuncu1,@oyuncu2,@oyuncu3,@oyuncu4,@oyuncu5,@oyuncu6,@oyuncu7,@oyuncu8)";
                    // müşteriler tablomuzun ilgili alanlarına kayıt ekleme işlemini gerçekleştirecek sorgumuz.
                    SqlCommand komut = new SqlCommand(kayit, conn);
                    //Sorgumuzu ve baglantimizi parametre olarak alan bir SqlCommand nesnesi oluşturuyoruz.
                    komut.Parameters.AddWithValue("@takimadi", combobox2.Text);
                    komut.Parameters.AddWithValue("@oyuncu1", listBox1.Items[0].ToString());
                    komut.Parameters.AddWithValue("@oyuncu2", listBox1.Items[1].ToString());
                    komut.Parameters.AddWithValue("@oyuncu3", listBox1.Items[2].ToString());
                    komut.Parameters.AddWithValue("@oyuncu4", listBox1.Items[3].ToString());
                    komut.Parameters.AddWithValue("@oyuncu5", listBox1.Items[4].ToString());
                    komut.Parameters.AddWithValue("@oyuncu6", listBox1.Items[5].ToString());
                    komut.Parameters.AddWithValue("@oyuncu7", listBox1.Items[6].ToString());
                    komut.Parameters.AddWithValue("@oyuncu8", listBox1.Items[7].ToString());
                    //Parametrelerimize Form üzerinde ki kontrollerden girilen verileri aktarıyoruz.
                    komut.ExecuteNonQuery();
                    //Veritabanında değişiklik yapacak komut işlemi bu satırda gerçekleşiyor.
                    conn.Close();
                    MessageBox.Show("Kayıt İşlemi Gerçekleşti.");

                    using (SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(@"SELECT r.idno,t.ad_soyad,t.takim_adi,t.telefon,t.mail,t.tc,r.tarih,r.saat,r.ucret,r.fiyat,b.oyuncu1,b.oyuncu2,b.oyuncu3,b.oyuncu4,b.oyuncu5,b.oyuncu6,b.oyuncu7,b.oyuncu8 FROM tliste t INNER JOIN rezervasyon r ON t.idno = r.idno INNER JOIN takim_bilgi b ON t.idno=b.idno", conn)))
                    {
                        tbl = new DataTable();
                        da.Fill(tbl);
                        dataGridView2.DataSource = tbl;
                        dataGridView2.AutoGenerateColumns = true;
                        da.Dispose();
                    }
                    combobox2.Enabled = true;
                    combobox2.Text = "";
                    textBox8.Text = "";
                    listBox1.Items.Clear();
                }
            }
            catch (Exception)
            {

                MessageBox.Show("Takim adini düzgün girdiğinizden emin olun.");
            }
        }
        private void button6_Click(object sender, EventArgs e)
        {
            listBox1.Items.Remove(listBox1.SelectedItem);
            sayac--;
        }
        string cmb2 = "";
        private void comboBox3_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == " ")  //Eğer kişi space tuşuna basar ise cmb1 boşalıyor ve combo temizleniyor
            {
                e.Handled = true;
                comboBox3.Text = "";
                cmb2 = "";
                return;
            }
            cmb2 = cmb2 + e.KeyChar.ToString();
            int index = comboBox3.FindString(cmb2);
            comboBox3.SelectedIndex = index;
            e.Handled = true;
            if (comboBox3.Text == "") //Eğer girilen değer comboda kayıtlı değil ise combo ve cmb1 boşalıyor.
            {
                cmb2 = "";
            }
        }

        private void comboBox3_Enter(object sender, EventArgs e)
        {
            cmb2 = "";
        }

        private void comboBox3_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                SqlCommand komut = new SqlCommand();
                komut.CommandText = "SELECT * FROM tliste";
                komut.Connection = conn;
                komut.CommandType = CommandType.Text;
                SqlDataReader dr;
                conn.Open();
                dr = komut.ExecuteReader();
                comboBox3.Items.Clear();
                while (dr.Read())
                {
                    comboBox3.Items.Add(dr["takim_adi"]);
                }
                conn.Close();
            }
        }
        string cmb3 = "";
        private void comboBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar.ToString() == " ")  //Eğer kişi space tuşuna basar ise cmb1 boşalıyor ve combo temizleniyor
            {
                e.Handled = true;
                comboBox1.Text = "";
                cmb3 = "";
                return;
            }
            cmb3 = cmb3 + e.KeyChar.ToString();
            int index = comboBox1.FindString(cmb3);
            comboBox1.SelectedIndex = index;
            e.Handled = true;
            if (comboBox1.Text == "") //Eğer girilen değer comboda kayıtlı değil ise combo ve cmb1 boşalıyor.
            {
                cmb3 = "";
            }
        }
        public static SimpleButton dinamikButon;
        public static DateTime tarih;
        private void simpleButton80_MouseClick(object sender, MouseEventArgs e)
        {
            contextMenuStrip1.Visible = false;
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                dinamikButon = (sender as SimpleButton);
                SqlCommand komut = new SqlCommand();
                komut.CommandText = "SELECT * FROM rezervasyon";
                komut.Connection = conn;
                komut.CommandType = CommandType.Text;
                SqlDataReader dr;
                conn.Open();
                dr = komut.ExecuteReader();
                combobox2.Items.Clear();
                bos yeni1 = new bos();
                dolu yeni = new dolu();
                if (dinamikButon.Name == simpleButton21.Name || dinamikButon.Name == simpleButton22.Name || dinamikButon.Name == simpleButton20.Name || dinamikButon.Name == simpleButton23.Name || dinamikButon.Name == simpleButton24.Name || dinamikButon.Name == simpleButton25.Name || dinamikButon.Name == simpleButton26.Name || dinamikButon.Name == simpleButton27.Name || dinamikButon.Name == simpleButton28.Name || dinamikButon.Name == simpleButton29.Name || dinamikButon.Name == simpleButton30.Name || dinamikButon.Name == simpleButton31.Name)
                {
                    tarih = Convert.ToDateTime(simpleButton2.Text);
                }
                if (dinamikButon.Name == simpleButton8.Name || dinamikButon.Name == simpleButton9.Name || dinamikButon.Name == simpleButton10.Name || dinamikButon.Name == simpleButton11.Name || dinamikButon.Name == simpleButton12.Name || dinamikButon.Name == simpleButton13.Name || dinamikButon.Name == simpleButton14.Name || dinamikButon.Name == simpleButton15.Name || dinamikButon.Name == simpleButton16.Name || dinamikButon.Name == simpleButton17.Name || dinamikButon.Name == simpleButton18.Name || dinamikButon.Name == simpleButton19.Name)
                {
                    tarih = Convert.ToDateTime(simpleButton1.Text);
                }
                if (dinamikButon.Name == simpleButton43.Name || dinamikButon.Name == simpleButton42.Name || dinamikButon.Name == simpleButton41.Name || dinamikButon.Name == simpleButton40.Name || dinamikButon.Name == simpleButton39.Name || dinamikButon.Name == simpleButton38.Name || dinamikButon.Name == simpleButton37.Name || dinamikButon.Name == simpleButton36.Name || dinamikButon.Name == simpleButton35.Name || dinamikButon.Name == simpleButton34.Name || dinamikButon.Name == simpleButton33.Name || dinamikButon.Name == simpleButton32.Name)
                {
                    tarih = Convert.ToDateTime(simpleButton3.Text);
                }
                if (dinamikButon.Name == simpleButton44.Name || dinamikButon.Name == simpleButton45.Name || dinamikButon.Name == simpleButton46.Name || dinamikButon.Name == simpleButton47.Name || dinamikButon.Name == simpleButton48.Name || dinamikButon.Name == simpleButton49.Name || dinamikButon.Name == simpleButton50.Name || dinamikButon.Name == simpleButton51.Name || dinamikButon.Name == simpleButton52.Name || dinamikButon.Name == simpleButton53.Name || dinamikButon.Name == simpleButton54.Name || dinamikButon.Name == simpleButton55.Name)
                {
                    tarih = Convert.ToDateTime(simpleButton4.Text);
                }
                if (dinamikButon.Name == simpleButton67.Name || dinamikButon.Name == simpleButton66.Name || dinamikButon.Name == simpleButton65.Name || dinamikButon.Name == simpleButton64.Name || dinamikButon.Name == simpleButton63.Name || dinamikButon.Name == simpleButton62.Name || dinamikButon.Name == simpleButton61.Name || dinamikButon.Name == simpleButton60.Name || dinamikButon.Name == simpleButton59.Name || dinamikButon.Name == simpleButton58.Name || dinamikButon.Name == simpleButton57.Name || dinamikButon.Name == simpleButton56.Name)
                {
                    tarih = Convert.ToDateTime(simpleButton5.Text);
                }
                if (dinamikButon.Name == simpleButton68.Name || dinamikButon.Name == simpleButton69.Name || dinamikButon.Name == simpleButton70.Name || dinamikButon.Name == simpleButton71.Name || dinamikButon.Name == simpleButton72.Name || dinamikButon.Name == simpleButton73.Name || dinamikButon.Name == simpleButton74.Name || dinamikButon.Name == simpleButton75.Name || dinamikButon.Name == simpleButton78.Name || dinamikButon.Name == simpleButton79.Name || dinamikButon.Name == simpleButton76.Name || dinamikButon.Name == simpleButton77.Name)
                {
                    tarih = Convert.ToDateTime(simpleButton6.Text);
                }
                if (dinamikButon.Name == simpleButton80.Name || dinamikButon.Name == simpleButton81.Name || dinamikButon.Name == simpleButton82.Name || dinamikButon.Name == simpleButton83.Name || dinamikButon.Name == simpleButton84.Name || dinamikButon.Name == simpleButton85.Name || dinamikButon.Name == simpleButton86.Name || dinamikButon.Name == simpleButton87.Name || dinamikButon.Name == simpleButton88.Name || dinamikButon.Name == simpleButton89.Name || dinamikButon.Name == simpleButton90.Name || dinamikButon.Name == simpleButton91.Name)
                {
                    tarih = Convert.ToDateTime(simpleButton7.Text);
                }
                while (dr.Read())
                {
                    if (dinamikButon.Text == "  " + dr["takim_adi"].ToString())
                    {
                        yeni.Name = "dolu";
                        yeni.Show();
                    }

                }
                if (Application.OpenForms["dolu"] == null)
                {
                    yeni1.Show();
                }
                conn.Close();
            }
        }
        private XtraReport rapor = new XtraReport();
        private void dateNavigator1_DateTimeChanged(object sender, EventArgs e)
        {
            tarihgoster();
        }
        private void simpleButton92_Click(object sender, EventArgs e)
        {
            rapor.LoadLayout(Application.StartupPath + @"\XtraReport.repx");
            rapor.DataSource = dataGridView2.DataSource;
            rapor.CreateDocument();
            rapor.ShowPreview();
        }
        private void simpleButton93_Click(object sender, EventArgs e)
        {
            rapor.LoadLayout(Application.StartupPath + @"\XtraReport.repx");
            rapor.DataSource = dataGridView2.DataSource;
            rapor.ShowDesigner();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            DataView dv = tbl.DefaultView;
            dv.RowFilter = "tc like '" + textBox1.Text + "%'";
            dataGridView2.DataSource = dv;
            int toplam = 0;
            for (int i = 0; i < dataGridView2.Rows.Count - 1; ++i)
            {
                if (dataGridView2.Rows[i].Cells["ucret"].Value.ToString() == "Ödendi         ")
                {
                    toplam += Convert.ToInt32(dataGridView2.Rows[i].Cells["fiyat"].Value);
                }
            }
            label30.Text = toplam.ToString() + " TL";
        }

        private void yenileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MessageBox.Show("Yenilendi.");
            tarihgoster();
        }
        SimpleButton btn;
        private void simpleButton80_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)  //Farenin sağ tuşuna tıklandığını
            {
                btn = (SimpleButton)sender;  //hangi buttonun tıkladığını
                if (btn.Appearance.BackColor != Color.MediumSeaGreen)
                {
                    contextMenuStrip1.Items[0].Enabled = false;
                    contextMenuStrip1.Items[1].Enabled = false;
                }
                if (btn.Appearance.BackColor == Color.MediumSeaGreen)
                {
                    contextMenuStrip1.Items[0].Enabled = true;
                    contextMenuStrip1.Items[1].Enabled = true;
                }
                contextMenuStrip1.Show(simpleButton80, simpleButton80.PointToClient(Cursor.Position));

            }
        }

        private void ödemeAlındıToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SqlConnection conn = new SqlConnection(veri.source))
            {
                conn.Open();
                string kayit = "UPDATE rezervasyon SET ucret=@ucret where takim_adi=@takimadi";
                SqlCommand komut = new SqlCommand(kayit, conn);
                komut.Parameters.AddWithValue("@ucret", "Ödendi");
                komut.Parameters.AddWithValue("@takimadi", btn.Text.Trim());
                komut.ExecuteNonQuery();
                conn.Close();
            }
            btn.Appearance.BackColor = Color.Yellow;
        }

        private void rezervasyonİptaliToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (SqlConnection baglanti = new SqlConnection(veri.source))
            {
                baglanti.Open();
                string secmeSorgusu = "SELECT * from rezervasyon where takim_adi=@takimadi";
                //musterino parametresine bağlı olarak müşteri bilgilerini çeken sql kodu
                SqlCommand secmeKomutu = new SqlCommand(secmeSorgusu, baglanti);
                secmeKomutu.Parameters.AddWithValue("@takimadi", btn.Text.Trim());
                //musterino parametremize textbox'dan girilen değeri aktarıyoruz.
                SqlDataAdapter da = new SqlDataAdapter(secmeKomutu);
                SqlDataReader dr = secmeKomutu.ExecuteReader();
                //DataReader ile müşteri verilerini veritabanından belleğe aktardık.
                if (dr.Read()) //Datareader herhangi bir okuma yapabiliyorsa aşağıdaki kodlar çalışır.
                {
                    string isim = dr["takim_adi"].ToString() + " ";
                    dr.Close();
                    //Datareader ile okunan müşteri ad ve soyadını isim değişkenine atadım.
                    //Datareader açık olduğu sürece başka bir sorgu çalıştıramayacağımız için dr nesnesini kapatıyoruz.
                    DialogResult durum = MessageBox.Show(isim + " kaydını silmek istediğinizden emin misiniz?", "Silme Onayı", MessageBoxButtons.YesNo);
                    //Kullanıcıya silme onayı penceresi açıp, verdiği cevabı durum değişkenine aktardık.
                    if (DialogResult.Yes == durum) // Eğer kullanıcı Evet seçeneğini seçmişse, veritabanından kaydı silecek kodlar çalışır.
                    {


                        string silmeSorgusu = "DELETE from rezervasyon where takim_adi=@takimadi";
                        string silmesorgusu = "DELETE FROM tliste where takim_adi=@takimadi";
                        string Silmesorgusu = "DELETE FROM takim_bilgi where takim_adi=@takimadi";
                        //musterino parametresine bağlı olarak müşteri kaydını silen sql sorgusu
                        SqlCommand silKomutu = new SqlCommand(silmeSorgusu, baglanti);
                        silKomutu.Parameters.AddWithValue("@takimadi", btn.Text.Trim());
                        SqlCommand silKomutu1 = new SqlCommand(silmesorgusu, baglanti);
                        silKomutu1.Parameters.AddWithValue("@takimadi", btn.Text.Trim());
                        SqlCommand silKomutu2 = new SqlCommand(Silmesorgusu, baglanti);
                        silKomutu2.Parameters.AddWithValue("@takimadi", btn.Text.Trim());

                        silKomutu.ExecuteNonQuery();
                        silKomutu2.ExecuteNonQuery();
                        silKomutu1.ExecuteNonQuery();
                    }
                }
                else
                    MessageBox.Show("Müşteri Bulunamadı.");
                baglanti.Close();
                tarihgoster();
            }
        }
        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            using (SqlConnection baglanti = new SqlConnection(veri.source))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(new SqlCommand(@"SELECT r.idno,t.ad_soyad,t.takim_adi,t.telefon,t.mail,t.tc,r.tarih,r.saat,r.ucret,r.fiyat,b.oyuncu1,b.oyuncu2,b.oyuncu3,b.oyuncu4,b.oyuncu5,b.oyuncu6,b.oyuncu7,b.oyuncu8 FROM tliste t INNER JOIN rezervasyon r ON t.idno = r.idno INNER JOIN takim_bilgi b ON t.idno=b.idno", baglanti)))
                {
                    tbl = new DataTable();
                    da.Fill(tbl);
                    dataGridView2.DataSource = tbl;
                    dataGridView2.AutoGenerateColumns = true;
                    da.Dispose();
                }
            }
            dateTimePicker3.Value = DateTime.Now.AddMonths(-1);
            dateTimePicker4.Value = DateTime.Now.AddMonths(+1);
        }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace CrudApps
{
    public partial class Form1 : Form
    {
        // konfigurasi database
        static string konfig = "Server=localhost;Username=root;database=crud_db";
        MySqlConnection koneksi = new MySqlConnection(konfig);
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            ambilData();
        }

        private void btnSimpan_Click(object sender, EventArgs e)
        {
            // koleksi nilai dari input user
            string nama = textNama.Text;
            string alamat = textAlamat.Text;
            string no_hp = textNoTelpon.Text;
            string jenis_kelamin = radioLaki.Checked ? "L" : "P";
            string jabatan = comboJabatan.SelectedItem.ToString();

            // proses simpan data ke tabel database
            try
            {
                // buka koneksi 
                koneksi.Open();
                // siapkan query simpan data ke tabel
                string query = $"insert into pegawai(nama,alamat,no_hp,jenis_kelamin,jabatan) values('{nama}','{alamat}','{no_hp}','{jenis_kelamin}','{jabatan}')";
                // buat objek command
                MySqlCommand komando = new MySqlCommand(query, koneksi);
                komando.ExecuteNonQuery();
                // ambil ID terakhir dari tabel pegawai
                long idAkhir = komando.LastInsertedId;
                // tambahkan data ke datagrid
                dataPegawai.Rows.Add(idAkhir,nama,alamat,no_hp,jenis_kelamin,jabatan);
            }
            catch (Exception error)
            {

                MessageBox.Show($"Error cuy :{error.Message}");
            }
            finally
            {
                koneksi.Close();
            }

        }
        // method untuk membaca table
        private void ambilData()
        {
            try
            {
                // buka koneksi 
                koneksi.Open();
                // siapkan query simpan data ke tabel
                string query = "select * from pegawai";
                // buat objek command
                MySqlCommand komando = new MySqlCommand(query, koneksi);
                MySqlDataReader hasil = komando.ExecuteReader();
                // kosongkan datagridview
                dataPegawai.RowCount = 0;
                // isi datagridview 
                while (hasil.Read())
                {
                    dataPegawai.Rows.Add(hasil["id"], hasil["nama"], hasil["alamat"], hasil["no_hp"], hasil["jenis_kelamin"], hasil["jabatan"]);
                }
                kosongkanForm();
            }
            catch (Exception error)
            {

                MessageBox.Show($"Error cuy :{error.Message}");
            }
            finally
            {
                koneksi.Close();
            }
        }

        private void dataPegawai_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            // ambil posisi baris
            int pilihBaris = dataPegawai.CurrentCell.RowIndex;
            // isikan data ke form
            textNama.Text = dataPegawai.Rows[pilihBaris].Cells[1].Value.ToString();
            textAlamat.Text = dataPegawai.Rows[pilihBaris].Cells[2].Value.ToString();
            textNoTelpon.Text = dataPegawai.Rows[pilihBaris].Cells[3].Value.ToString();
            comboJabatan.SelectedItem = dataPegawai.Rows[pilihBaris].Cells[5].Value.ToString();

            // pilih jenis kelamin
            if(dataPegawai.Rows[pilihBaris].Cells[4].Value.ToString() == "L")
            {
                radioLaki.Checked = true;
            }
            else
            {
                radioPerempuan.Checked = true;
            }
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            // koleksi nilai dari input user
            string nama = textNama.Text;
            string alamat = textAlamat.Text;
            string no_hp = textNoTelpon.Text;
            string jenis_kelamin = radioLaki.Checked ? "L" : "P";
            string jabatan = comboJabatan.SelectedItem.ToString();

            // proses simpan data ke tabel database
            try
            {
                // ambil posisi baris
                int pilihBaris = dataPegawai.CurrentCell.RowIndex;
                string id = dataPegawai.Rows[pilihBaris].Cells[0].Value.ToString();
                // buka koneksi 
                koneksi.Open();
                // siapkan query simpan data ke tabel
                string query = "update pegawai set nama=@nama , alamat = @alamat , no_hp= @no_hp,jenis_kelamin = @jenis_kelamin , jabatan = @jabatan where id=@id";
                // buat objek command
                MySqlCommand komando = new MySqlCommand(query, koneksi);
                komando.Parameters.AddWithValue("nama",nama);
                komando.Parameters.AddWithValue("alamat",alamat);
                komando.Parameters.AddWithValue("no_hp",no_hp);
                komando.Parameters.AddWithValue("jenis_kelamin",jenis_kelamin);
                komando.Parameters.AddWithValue("jabatan",jabatan);
                komando.Parameters.AddWithValue("id",id);
                komando.ExecuteNonQuery();
                // update data datagridview
                dataPegawai.Rows[pilihBaris].Cells[1].Value = nama;
                dataPegawai.Rows[pilihBaris].Cells[2].Value = alamat;
                dataPegawai.Rows[pilihBaris].Cells[3].Value = no_hp;
                dataPegawai.Rows[pilihBaris].Cells[4].Value = jenis_kelamin;
                dataPegawai.Rows[pilihBaris].Cells[5].Value = jabatan;
                kosongkanForm();
            }
            catch (Exception error)
            {

                MessageBox.Show($"Error cuy :{error.Message}");
            }
            finally
            {
                koneksi.Close();
            }
        }

        private void btnHapus_Click(object sender, EventArgs e)
        {
            // ambil posisi baris
            int pilihBaris = dataPegawai.CurrentCell.RowIndex;
            string id = dataPegawai.Rows[pilihBaris].Cells[0].Value.ToString();
            try
            {
                koneksi.Open();
                string query = $"delete from pegawai where id={id}";
                MySqlCommand komando = new MySqlCommand(query,koneksi);
                komando.ExecuteNonQuery();

                // hapus data datagridview
                dataPegawai.Rows.RemoveAt(pilihBaris);
                kosongkanForm();
            }
            catch (Exception error)
            {

                MessageBox.Show($"Error cuy : {error.Message}");
            }
            finally { koneksi.Close(); }
        }
        private void kosongkanForm()
        {
            textNama.Clear();
            textAlamat.Clear();
            textNoTelpon.Clear();
            radioLaki.Checked = true;
            comboJabatan.SelectedIndex = 0;
        }
    }
}

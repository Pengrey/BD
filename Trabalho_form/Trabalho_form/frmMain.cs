using System;
using System.Windows.Forms;
using System.Data.SqlClient;
using System.Data;

namespace Trabalho_form
{
    public partial class frmMain : Form
    {
        private SqlConnection cn;
        private int currentAnime;
        private bool adding;
        public frmMain()
        {
            InitializeComponent();
        }
        //btn_LogOut Click Event
        private void btn_LogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 fl = new Form1();
            fl.Show();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cn = getSGBDConnection();
        }

        private SqlConnection getSGBDConnection()
        {
            return new SqlConnection("Data Source = tcp:mednat.ieeta.pt\\SQLSERVER,8101; Initial Catalog = p5g1; uid = p5g1; password = r0*7rFeu03Z");
        }

        private bool verifySGBDConnection()
        {
            if (cn == null)
                cn = getSGBDConnection();

            if (cn.State != ConnectionState.Open)
                cn.Open();

            return cn.State == ConnectionState.Open;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > 0)
            {
                currentAnime = listBox1.SelectedIndex;
                ShowAnime();
            }
        }

        private void loadAnimesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("SELECT * FROM Customers", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();

            while (reader.Read())
            {
                Anime A = new Anime();
                A.Name = reader["Nome_unico"].ToString();
                A.Description = reader["Descricao"].ToString();
                A.Avaliation = reader["Avaliacao"].ToString();
                A.ReleaseDate = reader["Data_lancamento"].ToString();
                listBox1.Items.Add(A);
            }

            cn.Close();


            currentAnime = 0;
            ShowAnime();
        }

        private void SubmitAnime(Anime A)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "TODO : command to add anime and rating";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Nome_unico", A.Name);
            cmd.Parameters.AddWithValue("@Avaliacao", A.Avaliation);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update anime in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private void UpdateAnime(Anime A)
        {
            int rows = 0;

            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "TODO: muda a avaliação";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Nome_unico", A.Name);
            cmd.Parameters.AddWithValue("@Avaliacao", A.Avaliation);
            cmd.Connection = cn;

            try
            {
                rows = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update contact in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                if (rows == 1)
                    MessageBox.Show("Update OK");
                else
                    MessageBox.Show("Update NOT OK");

                cn.Close();
            }
        }

        private void RemoveAnime(string AnimeID)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "TODO: remove anime do watched ";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@animeID", AnimeID);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete anime in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        // Helper routines
        public void LockControls()
        {
            textName.ReadOnly = true;
            numericUpDown1.ReadOnly = true;
        }

        public void UnlockControls()
        {
            textName.ReadOnly = true;
            numericUpDown1.ReadOnly = true;
        }

        public void ShowButtons()
        {
            LockControls();
            btn_Add.Visible = true;
            btn_delete.Visible = true;
            btn_edit.Visible = true;
            btn_ok.Visible = false;
            btn_cancel.Visible = false;
        }

        public void ClearFields()
        {
            textName.Text = "";
            numericUpDown1.Text = "";
        }

        public void ShowAnime()
        {
            if (listBox1.Items.Count == 0 | currentAnime < 0)
                return;
            Anime anime = new Anime();
            anime = (Anime)listBox1.Items[currentAnime];
            textName.Text = anime.Name;
            numericUpDown1.Text = anime.Avaliation;

        }

        public void HideButtons()
        {
            UnlockControls();
            btn_Add.Visible = false;
            btn_delete.Visible = false;
            btn_edit.Visible = false;
            btn_ok.Visible = true;
            btn_cancel.Visible = true;
        }

        private bool SaveAnime()
        {
            Anime anime = new Anime();
            try
            {
                anime.Name = textName.Text;
                anime.Avaliation = numericUpDown1.Text;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            if (adding)
            {
                SubmitAnime(anime);
                listBox1.Items.Add(anime);
            }
            else
            {
                UpdateAnime(anime);
                listBox1.Items[currentAnime] = anime;
            }
            return true;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            adding = true;
            ClearFields();
            HideButtons();
            listBox1.Enabled = false;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            listBox1.Enabled = true;
            if (listBox1.Items.Count > 0)
            {
                currentAnime = listBox1.SelectedIndex;
                if (currentAnime < 0)
                    currentAnime = 0;
                ShowAnime();
            }
            else
            {
                ClearFields();
                LockControls();
            }
            ShowButtons();
        }

        private void btn_edit_Click(object sender, EventArgs e)
        {
            currentAnime = listBox1.SelectedIndex;
            if (currentAnime <= 0)
            {
                MessageBox.Show("Please select a contact to edit");
                return;
            }
            adding = false;
            HideButtons();
            listBox1.Enabled = false;
        }

        private void btn_ok_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAnime();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            listBox1.Enabled = true;
            int idx = listBox1.FindString(textName.Text);
            listBox1.SelectedIndex = idx;
            ShowButtons();
        }

        private void btn_delete_Click(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex > -1)
            {
                try
                {
                    RemoveAnime(((Anime)listBox1.SelectedItem).Name);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                listBox1.Items.RemoveAt(listBox1.SelectedIndex);
                if (currentAnime == listBox1.Items.Count)
                    currentAnime = listBox1.Items.Count - 1;
                if (currentAnime == -1)
                {
                    ClearFields();
                    MessageBox.Show("There are no more contacts");
                }
                else
                {
                    ShowAnime();
                }
            }
        }

        private void frmMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            Application.Exit();
        }
    }
}
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
        private int currentAnime2;
        private bool adding;
        private int userID;
        public frmMain(int userID)
        {
            InitializeComponent();
            this.userID = userID;
        }

        private void frmMain_Load(object sender, EventArgs e)
        {
            cn = getSGBDConnection();
            loadVeAnimes();
        }

        //btn_LogOut Click Event
        private void btn_LogOut_Click_1(object sender, EventArgs e)
        {
            this.Hide();
            Form1 fl = new Form1();
            fl.Show();
        }

        private SqlConnection getSGBDConnection()
        {
            return new SqlConnection("Data Source = tcp:mednat.ieeta.pt\\SQLSERVER,8101; Initial Catalog = p5g1; uid = p5g1; password = r0*7rFeu03Z"); // Mudar aqui para o server a conectar
        }

        private bool verifySGBDConnection()
        {
            if (cn == null)
                cn = getSGBDConnection();

            if (cn.State != ConnectionState.Open)
                cn.Open();

            return cn.State == ConnectionState.Open;
        }

        private void listBox1_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            if (listBox1.SelectedIndex >= 0)
            {
                currentAnime = listBox1.SelectedIndex;
                ShowAnime();
            }
        }

        private void listBox2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (listBox2.SelectedIndex >= 0)
            {
                currentAnime2 = listBox2.SelectedIndex;
                ShowAnime2();
            }
        }

        private void loadVeAnimes()
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("Select * from AnimeDB.ve where u_id=@userID", cn);
            cmd.Parameters.AddWithValue("@userID", this.userID);
            SqlDataReader reader = cmd.ExecuteReader();
            listBox1.Items.Clear();

            while (reader.Read())
            {
                Anime A = new Anime();
                A.Name = reader["Anime_nome"].ToString();
                A.Progresso = reader["progresso"].ToString();
                A.Estado = reader["estado"].ToString();
                listBox1.Items.Add(A);
            }

            cn.Close();


            currentAnime = 0;
            ShowAnime();
        }

        private void loadAnimes()
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("Select * from AnimeDB.Anime", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            listBox2.Items.Clear();

            while (reader.Read())
            {
                Anime A = new Anime();
                A.Name = reader["Nome_unico"].ToString();
                listBox2.Items.Add(A);
            }

            cn.Close();


            currentAnime2 = 0;
            ShowAnime2();
        }

        private void SubmitAnime(Anime A)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO AnimeDB.ve (u_id,Anime_nome,progresso,estado) VALUES ( @u_id, @Nome_unico, @progresso, @estado)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@u_id",this.userID.ToString());
            cmd.Parameters.AddWithValue("@Nome_unico", A.Name);
            cmd.Parameters.AddWithValue("@progresso", "0.00");
            cmd.Parameters.AddWithValue("@estado", A.Estado);
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


        private void RemoveAnime(string AnimeNome)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();
            
            cmd.CommandText = "EXEC AnimeDB.delLineVe  @animeNome, @userID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@animeNome", AnimeNome);
            cmd.Parameters.AddWithValue("@userID", this.userID);
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
            State_box.Enabled = false;
        }

        public void UnlockControls()
        {
            textName.ReadOnly = false;
            State_box.Enabled = true;
        }

        public void ShowButtons()
        {
            LockControls();
            btn_Add.Visible = true;
            btn_delete.Visible = true;
            btn_ok.Visible = false;
            btn_cancel.Visible = false;
            label2.Visible = false;
            listBox2.Visible = false;
        }

        public void ClearFields()
        {
            textName.Text = "";
            State_box.Text = "";
        }

        public void ShowAnime()
        {
            if (listBox1.Items.Count == 0 | currentAnime < 0)
                return;
            Anime anime = new Anime();
            anime = (Anime)listBox1.Items[currentAnime];
            textName.Text = anime.Name;
            State_box.Text = anime.Estado;
            decimal progress;
            AnimeProgressBar.Value = int.Parse(anime.Progresso.Replace("0,", ""));
        }

        public void ShowAnime2()
        {
            if (listBox2.Items.Count == 0 | currentAnime2 < 0)
                return;
            Anime anime = new Anime();
            anime = (Anime)listBox2.Items[currentAnime2];
            textName.Text = anime.Name;
        }

        public void HideButtons()
        {
            UnlockControls();
            btn_Add.Visible = false;
            btn_delete.Visible = false;
            btn_ok.Visible = true;
            btn_cancel.Visible = true;
            label2.Visible = true;
            listBox2.Visible = true;
        }

        private bool SaveAnime()
        {
            Anime anime = (Anime)listBox2.Items[currentAnime2];
            try
            {
                anime.Name = textName.Text;
                anime.Estado = State_box.Text;
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
                loadVeAnimes();
            }
            return true;
        }

        private void btn_Add_Click(object sender, EventArgs e)
        {
            adding = true;
            ClearFields();
            HideButtons();
            loadAnimes();
            ShowAnime2();
            listBox1.Enabled = false;
            State_box.Enabled = true;
        }

        private void btn_cancel_Click(object sender, EventArgs e)
        {
            listBox1.Enabled = true;
            State_box.Enabled = false;
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
            State_box.Enabled = false;
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
                    MessageBox.Show("There are no more animes");
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
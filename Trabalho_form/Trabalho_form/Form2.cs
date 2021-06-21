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
using System.Globalization;

namespace Trabalho_form
{
    public partial class Form2 : Form
    {

        CultureInfo provider = CultureInfo.InvariantCulture;
        private SqlConnection cn;
        private int currentAnime;
        private int currentStudio;
        private int currentAuthor;
        private int currentSeason;
        private int currentEpisode;

        public Form2()
        {
            InitializeComponent();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            cn = getSGBDConnection();
            loadAnimes();
        }

        private void btn_LogOut_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form1 fl = new Form1();
            fl.Show();
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

        private void loadAnimes()
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("Select * from AnimeDB.Anime", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            Anime_list_box.Items.Clear();

            while (reader.Read())
            {
                Anime A = new Anime();
                A.Name = reader["Nome_unico"].ToString();
                A.Description = reader["Descricao"].ToString();
                A.Avaliation = (decimal) reader["Avaliacao"];
                A.Estudio = reader["Est_email"].ToString();
                A.Autor = reader["Autor_id"].ToString();
                A.Estado = reader["estado"].ToString();
                A.ReleaseDate = reader["Data_lancamento"].ToString();
                Anime_list_box.Items.Add(A);
            }

            cn.Close();

            currentAnime = 0;
            ShowAnime();
        }

        private void loadStudios()
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("Select * from AnimeDB.Estudio", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            Anime_Est_email_box.Items.Clear();

            while (reader.Read())
            {
                Studio S = new Studio();
                S.Email = reader["Email"].ToString();
                S.nome = reader["nome"].ToString();
                S.WebSite = reader["WebSite"].ToString();
                S.localizacao = reader["localizacao"].ToString();
                S.fundacao = reader["fundacao"].ToString();
                Anime_Est_email_box.Items.Add(S);
            }

            cn.Close();

            currentStudio = 0;
        }

        private void loadAuthors()
        {
            if (!verifySGBDConnection())
                return;

            SqlCommand cmd = new SqlCommand("Select * from AnimeDB.Autor", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            Anime_Author_box.Items.Clear();

            while (reader.Read())
            {
                Author A = new Author();
                A.ID = reader["ID"].ToString();
                A.city = reader["city"].ToString();
                Anime_Author_box.Items.Add(A);
            }

            cn.Close();

            currentAuthor = 0;
        }

        public void ShowAnime()
        {
            if (Anime_list_box.Items.Count == 0 | currentAnime < 0)
                return;
            Anime anime = new Anime();
            anime = (Anime)Anime_list_box.Items[currentAnime];
            Anime_Name_box.Text = anime.Name;
            Anime_Description_box.Text = anime.Description;
            Anime_Evaluation.Value = (decimal) anime.Avaliation;
            Anime_date.Value = DateTime.ParseExact(anime.ReleaseDate.Split()[0], "dd/mm/yyyy", provider);
            Anime_State_box.Text = anime.Estado;
        }

        private void Anime_list_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Anime_list_box.SelectedIndex >= 0)
            {
                currentAnime = Anime_list_box.SelectedIndex;
                ShowAnime();
            }
        }

        private void Anime_Est_email_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Anime_Est_email_box.SelectedIndex >= 0)
            {
                currentStudio = Anime_Est_email_box.SelectedIndex;
            }
        }

        private void Anime_Author_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Anime_Author_box.SelectedIndex >= 0)
            {
                currentAuthor = Anime_Author_box.SelectedIndex;
            }
        }

        private void Anime_Add_btn_Click(object sender, EventArgs e)
        {
            AnimeClearFields();
            AnimeShowButtons();
            loadStudios();
            loadAuthors();
        }

        private void Anime_Cancel_btn_Click(object sender, EventArgs e)
        {
            AnimeHideButtons();
            if (Anime_list_box.Items.Count > 0)
            {
                currentAnime = Anime_list_box.SelectedIndex;
                if (currentAnime < 0)
                    currentAnime = 0;
                ShowAnime();
            }
            else
            {
                AnimeLockControls();
            }
        }

        private void Anime_Delete_btn_Click(object sender, EventArgs e)
        {
            if (Anime_list_box.SelectedIndex > -1)
            {
                try
                {
                    RemoveAnime(((Anime)Anime_list_box.SelectedItem).Name);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                Anime_list_box.Items.RemoveAt(Anime_list_box.SelectedIndex);
                if (currentAnime == Anime_list_box.Items.Count)
                    currentAnime = Anime_list_box.Items.Count - 1;
                if (currentAnime == -1)
                {
                    AnimeClearFields();
                    MessageBox.Show("There are no more contacts");
                }
                else
                {
                    ShowAnime();
                }
            }
        }

        private void SubmitAnime(Anime A)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO AnimeDB.Anime(Nome_unico,Descricao,Avaliacao,Est_email,Autor_id,estado,Data_lancamento) VALUES ( @Nome_unico, @Descricao, @Avaliacao, @Est_email, @Autor_id, @estado, @Data_lancamento)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Nome_unico", A.Name);
            cmd.Parameters.AddWithValue("@Descricao", A.Description);
            cmd.Parameters.AddWithValue("@Avaliacao", A.Avaliation);
            cmd.Parameters.AddWithValue("@Est_email", A.Estudio);
            cmd.Parameters.AddWithValue("@Autor_id", A.Autor);
            cmd.Parameters.AddWithValue("@estado", A.Estado);
            cmd.Parameters.AddWithValue("@Data_lancamento", A.ReleaseDate);
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

            cmd.CommandText = "EXEC AnimeDB.DeleteAnim @animeNome";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@animeNome", AnimeNome);
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

        private bool SaveAnime()
        {
            Anime anime = (Anime)Anime_list_box.Items[currentAnime];
            try
            {
                anime.Name = Anime_Name_box.Text;
                anime.Description = Anime_Description_box.Text;
                anime.Avaliation = Anime_Evaluation.Value;
                anime.Estudio = Anime_Est_email_box.Items[currentStudio].ToString();
                anime.Autor = Anime_Author_box.Items[currentAuthor].ToString();
                anime.Estado = Anime_State_box.Text;
                anime.ReleaseDate = Anime_date.Value.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            SubmitAnime(anime);
            Anime_list_box.Items.Add(anime);
            loadAnimes();
            return true;
        }

        private void Anime_Ok_btn_Click(object sender, EventArgs e)
        {
            try
            {
                SaveAnime();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            int idx = Anime_list_box.FindString(Anime_Name_box.Text);
            Anime_list_box.SelectedIndex = idx;
            AnimeHideButtons();
        }

        // Helper routines
        public void AnimeLockControls()
        {
            Anime_Name_box.ReadOnly = true;
            Anime_Description_box.ReadOnly = false;
            Anime_Evaluation.ReadOnly = false;
            Anime_date.Enabled = false;
            Anime_State_box.Enabled = false;
            Anime_list_box.Enabled = true;
        }

        public void AnimeUnlockControls()
        {
            Anime_Name_box.ReadOnly = false;
            Anime_Description_box.ReadOnly = false;
            Anime_Evaluation.ReadOnly = true;
            Anime_date.Enabled = true;
            Anime_State_box.Enabled = true;
            Anime_list_box.Enabled = false;
        }

        public void AnimeShowButtons()
        {
            AnimeUnlockControls();
            Est_email_label.Visible = true;
            Anime_Est_email_box.Visible = true;
            Author_label.Visible = true;
            Anime_Author_box.Visible = true;
            Anime_Add_btn.Visible = false;
            Anime_Cancel_btn.Visible = true;
            Anime_Ok_btn.Visible = true;
            Anime_Delete_btn.Visible = false;
        }

        public void AnimeHideButtons()
        {
            AnimeLockControls();
            Est_email_label.Visible = false;
            Anime_Est_email_box.Visible = false;
            Author_label.Visible = false;
            Anime_Author_box.Visible = false;
            Anime_Add_btn.Visible = true;
            Anime_Cancel_btn.Visible = false;
            Anime_Ok_btn.Visible = false;
            Anime_Delete_btn.Visible = true;
        }

        public void AnimeClearFields()
        {
            Anime_Name_box.Text = "";
            Anime_Description_box.Text = "";
            Anime_Evaluation.Value = 0.0M;
            Anime_State_box.Text = "";
        }
    }
}

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
            loadSeasons();
            loadEpisodes();
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

        // Animes
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

            SqlCommand cmd = new SqlCommand("SELECT * FROM AnimeDB.id_autor", cn);
            SqlDataReader reader = cmd.ExecuteReader();
            Anime_Author_box.Items.Clear();

            while (reader.Read())
            {
                Author A = new Author();
                A.ID = reader["id"].ToString();
                A.Nome = reader["nome"].ToString();
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
            loadSeasons();
            loadEpisodes();
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
                anime.Autor = ((Author)Anime_Author_box.Items[currentAuthor]).ID;
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

        // Anime Helper routines
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

        // Seasons

        private void loadSeasons()
        {
            if (!verifySGBDConnection())
                return;


            SqlCommand cmd = new SqlCommand("Select * from AnimeDB.Temporada WHERE Anime_nome = @Anime_nome", cn);
            cmd.Parameters.AddWithValue("@Anime_nome", ((Anime) Anime_list_box.Items[currentAnime]).Name);
            SqlDataReader reader = cmd.ExecuteReader();
            Season_list_box.Items.Clear();

            while (reader.Read())
            {
                Season S = new Season();
                S.Name = reader["Nome"].ToString();
                S.AnimeName = reader["Anime_nome"].ToString();
                S.Description = reader["Descricao"].ToString();
                S.Avaliation = (decimal)reader["Avaliacao"];
                S.Estudio = reader["Est_email"].ToString();
                S.ReleaseDate = reader["Data_lancamento"].ToString();
                Season_list_box.Items.Add(S);
            }

            cn.Close();
            if (Season_list_box.Items.Count == 0)
            {
                SeasonClearFields();
                return;
            }
            currentSeason = 0;
            ShowSeason();
        }

        public void ShowSeason()
        {
            if (Season_list_box.Items.Count == 0 | currentSeason < 0)
                return;
            Season season = new Season();
            season = (Season)Season_list_box.Items[currentSeason];
            Season_Name_box.Text = season.Name;
            Season_Description_box.Text = season.Description;
            Season_Evaluation.Value = (decimal)season.Avaliation;
            Season_date.Value = DateTime.ParseExact(season.ReleaseDate.Split()[0], "dd/mm/yyyy", provider);
        }

        private void Season_list_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Season_list_box.SelectedIndex >= 0)
            {
                currentSeason = Season_list_box.SelectedIndex;
                ShowSeason();
                loadEpisodes();
            }
        }

        private void Season_Add_btn_Click(object sender, EventArgs e)
        {
            SeasonClearFields();
            SeasonShowButtons();
        }

        private void Season_Cancel_btn_Click(object sender, EventArgs e)
        {
            SeasonHideButtons();
            if (Season_list_box.Items.Count > 0)
            {
                currentSeason = Season_list_box.SelectedIndex;
                if (currentSeason < 0)
                    currentSeason = 0;
                ShowSeason();
            }
            else
            {
                SeasonLockControls();
            }
        }

        private void Season_Ok_btn_Click(object sender, EventArgs e)
        {
            try
            {
                SaveSeason();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            int idx = Season_list_box.FindString(Season_Name_box.Text);
            Season_list_box.SelectedIndex = idx;
            SeasonHideButtons();
        }

        private bool SaveSeason()
        {
            Season season = new Season();
            Anime anime = (Anime)Anime_list_box.Items[currentAnime];
            try
            {
                season.Name = Season_Name_box.Text;
                season.AnimeName = anime.Name;
                season.Description = Season_Description_box.Text;
                season.Avaliation = Season_Evaluation.Value;
                season.Estudio = anime.Estudio;
                season.ReleaseDate = Season_date.Value.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            SubmitSeason(season);
            Season_list_box.Items.Add(season);
            loadSeasons();
            return true;
        }

        private void SubmitSeason(Season S)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO AnimeDB.Temporada(Nome,Anime_nome,Descricao,Avaliacao,Est_email,Data_lancamento) VALUES ( @Nome, @Anime_nome, @Descricao, @Avaliacao, @Est_email,  @Data_lancamento)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Nome", S.Name);
            cmd.Parameters.AddWithValue("@Anime_nome", S.AnimeName);
            cmd.Parameters.AddWithValue("@Descricao", S.Description);
            cmd.Parameters.AddWithValue("@Avaliacao", S.Avaliation);
            cmd.Parameters.AddWithValue("@Est_email", S.Estudio);
            cmd.Parameters.AddWithValue("@Data_lancamento", S.ReleaseDate);
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

        private void Season_Delete_btn_Click(object sender, EventArgs e)
        {
            if (Season_list_box.SelectedIndex > -1)
            {
                try
                {
                    Season season = (Season)Season_list_box.SelectedItem;
                    RemoveSeason(season.Name, season.AnimeName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                Season_list_box.Items.RemoveAt(Season_list_box.SelectedIndex);
                if (currentSeason == Season_list_box.Items.Count)
                    currentSeason = Season_list_box.Items.Count - 1;
                if (currentSeason == -1)
                {
                    SeasonClearFields();
                    MessageBox.Show("There are no more seasons");
                }
                else
                {
                    ShowSeason();
                }
            }
        }

        private void RemoveSeason(string SeasonNome, string SeasonAnime)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "EXEC AnimeDB.DeleteTemp @seasonAnime, @seasonNome";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@seasonAnime", SeasonAnime);
            cmd.Parameters.AddWithValue("@seasonNome", SeasonNome);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete season in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        // Season Helper routines
        public void SeasonClearFields()
        {
            Season_Name_box.Text = "";
            Season_Description_box.Text = "";
            Season_Evaluation.Value = 0.0M;
        }

        public void SeasonLockControls()
        {
            Season_Name_box.ReadOnly = true;
            Season_Description_box.ReadOnly = false;
            Season_Evaluation.ReadOnly = false;
            Season_date.Enabled = false;
            Season_list_box.Enabled = true;
        }

        public void SeasonUnlockControls()
        {
            Season_Name_box.ReadOnly = false;
            Season_Description_box.ReadOnly = false;
            Season_Evaluation.ReadOnly = true;
            Season_date.Enabled = true;
            Season_list_box.Enabled = false;
        }

        public void SeasonShowButtons()
        {
            SeasonUnlockControls();
            Season_Add_btn.Visible = false;
            Season_Cancel_btn.Visible = true;
            Season_Ok_btn.Visible = true;
            Season_Delete_btn.Visible = false;
        }

        public void SeasonHideButtons()
        {
            SeasonLockControls();
            Season_Add_btn.Visible = true;
            Season_Cancel_btn.Visible = false;
            Season_Ok_btn.Visible = false;
            Season_Delete_btn.Visible = true;
        }

        // Episodes

        private void loadEpisodes()
        {
            if (!verifySGBDConnection())
                return;

            if (Season_list_box.Items.Count == 0)
            {
                EpisodeClearFields();
                Episode_list_box.Items.Clear();
                return;
            }
            
            SqlCommand cmd = new SqlCommand("Select * from AnimeDB.Episodio WHERE Anime_nome = @Anime_nome and Temp_Nome = @Season_nome", cn);
            cmd.Parameters.AddWithValue("@Anime_nome", ((Anime)Anime_list_box.Items[currentAnime]).Name);
            cmd.Parameters.AddWithValue("@Season_nome", ((Season)Season_list_box.Items[currentSeason]).Name);
            SqlDataReader reader = cmd.ExecuteReader();
            Episode_list_box.Items.Clear();

            while (reader.Read())
            {
                Episode E = new Episode();
                E.Name = reader["Titulo"].ToString();
                E.SeasonName = reader["Temp_Nome"].ToString();
                E.AnimeName = reader["Anime_nome"].ToString();
                E.Description = reader["Descricao"].ToString();
                E.Avaliation = (decimal)reader["Avaliacao"];
                E.Estudio = reader["Est_email"].ToString();
                E.ReleaseDate = reader["Data_lancamento"].ToString();
                Episode_list_box.Items.Add(E);
            }

            cn.Close();
            if (Episode_list_box.Items.Count == 0)
            {
                EpisodeClearFields();
                return;
            }
            currentEpisode = 0;
            ShowEpisode();
        }

        public void ShowEpisode()
        {
            if (Episode_list_box.Items.Count == 0 | currentEpisode < 0)
                return;
            Episode episode = new Episode();
            episode = (Episode)Episode_list_box.Items[currentEpisode];
            Episode_Name_box.Text = episode.Name;
            Episode_Description_box.Text = episode.Description;
            Episode_Evaluation.Value = (decimal)episode.Avaliation;
            Episode_date.Value = DateTime.ParseExact(episode.ReleaseDate.Split()[0], "dd/mm/yyyy", provider);
        }

        private void Episode_list_box_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (Episode_list_box.SelectedIndex >= 0)
            {
                currentEpisode = Episode_list_box.SelectedIndex;
                ShowEpisode();
            }
        }

        private void Episode_Add_btn_Click(object sender, EventArgs e)
        {
            EpisodeClearFields();
            EpisodeShowButtons();
        }

        private void Episode_Cancel_btn_Click(object sender, EventArgs e)
        {
            EpisodeHideButtons();
            if (Episode_list_box.Items.Count > 0)
            {
                currentEpisode = Episode_list_box.SelectedIndex;
                if (currentEpisode < 0)
                    currentEpisode = 0;
                ShowEpisode();
            }
            else
            {
                EpisodeLockControls();
            }
        }

        private void Episode_Ok_btn_Click(object sender, EventArgs e)
        {
            try
            {
                SaveEpisode();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            int idx = Episode_list_box.FindString(Episode_Name_box.Text);
            Episode_list_box.SelectedIndex = idx;
            EpisodeHideButtons();
        }

        private bool SaveEpisode()
        {
            Episode episode = new Episode();
            Season season = (Season)Season_list_box.Items[currentSeason];
            Anime anime = (Anime)Anime_list_box.Items[currentAnime];
            try
            {
                episode.Name = Episode_Name_box.Text;
                episode.SeasonName = season.Name;
                episode.AnimeName = anime.Name;
                episode.Description = Episode_Description_box.Text;
                episode.Avaliation = Episode_Evaluation.Value;
                episode.Estudio = anime.Estudio;
                episode.ReleaseDate = Episode_date.Value.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                return false;
            }
            SubmitEpisode(episode);
            Episode_list_box.Items.Add(episode);
            loadEpisodes();
            return true;
        }

        private void SubmitEpisode(Episode E)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "INSERT INTO AnimeDB.Episodio(Titulo,Temp_Nome,Anime_nome,Descricao,Avaliacao,Est_email,Data_lancamento) VALUES ( @Titulo, @Temp_Nome, @Anime_nome, @Descricao, @Avaliacao, @Est_email,  @Data_lancamento)";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@Titulo", E.Name);
            cmd.Parameters.AddWithValue("@Temp_Nome", E.SeasonName);
            cmd.Parameters.AddWithValue("@Anime_nome", E.AnimeName);
            cmd.Parameters.AddWithValue("@Descricao", E.Description);
            cmd.Parameters.AddWithValue("@Avaliacao", E.Avaliation);
            cmd.Parameters.AddWithValue("@Est_email", E.Estudio);
            cmd.Parameters.AddWithValue("@Data_lancamento", E.ReleaseDate);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to update episode in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        private void Episode_Delete_btn_Click(object sender, EventArgs e)
        {
            if (Episode_list_box.SelectedIndex > -1)
            {
                try
                {
                    Episode episode = (Episode)Episode_list_box.SelectedItem;
                    RemoveEpisode(episode.Name, episode.SeasonName, episode.AnimeName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                    return;
                }
                Episode_list_box.Items.RemoveAt(Episode_list_box.SelectedIndex);
                if (currentEpisode == Episode_list_box.Items.Count)
                    currentEpisode = Episode_list_box.Items.Count - 1;
                if (currentEpisode == -1)
                {
                    EpisodeClearFields();
                    MessageBox.Show("There are no more episodes");
                }
                else
                {
                    ShowEpisode();
                }
            }
        }

        private void RemoveEpisode(string EpisodeNome, string SeasonNome, string SeasonAnime)
        {
            if (!verifySGBDConnection())
                return;
            SqlCommand cmd = new SqlCommand();

            cmd.CommandText = "EXEC AnimeDB.DeleteEp @seasonAnime, @seasonNome, @seasonEpisodio";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@seasonEpisodio", EpisodeNome);
            cmd.Parameters.AddWithValue("@seasonAnime", SeasonAnime);
            cmd.Parameters.AddWithValue("@seasonNome", SeasonNome);
            cmd.Connection = cn;

            try
            {
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to delete episode in database. \n ERROR MESSAGE: \n" + ex.Message);
            }
            finally
            {
                cn.Close();
            }
        }

        // Episode Helper routines
        public void EpisodeClearFields()
        {
            Episode_Name_box.Text = "";
            Episode_Description_box.Text = "";
            Episode_Evaluation.Value = 0.0M;
        }

        public void EpisodeLockControls()
        {
            Episode_Name_box.ReadOnly = true;
            Episode_Description_box.ReadOnly = false;
            Episode_Evaluation.ReadOnly = false;
            Episode_date.Enabled = false;
            Episode_list_box.Enabled = true;
        }

        public void EpisodeUnlockControls()
        {
            Episode_Name_box.ReadOnly = false;
            Episode_Description_box.ReadOnly = false;
            Episode_Evaluation.ReadOnly = true;
            Episode_date.Enabled = true;
            Episode_list_box.Enabled = false;
        }

        public void EpisodeShowButtons()
        {
            EpisodeUnlockControls();
            Episode_Add_btn.Visible = false;
            Episode_Cancel_btn.Visible = true;
            Episode_Ok_btn.Visible = true;
            Episode_Delete_btn.Visible = false;
        }

        public void EpisodeHideButtons()
        {
            EpisodeLockControls();
            Episode_Add_btn.Visible = true;
            Episode_Cancel_btn.Visible = false;
            Episode_Ok_btn.Visible = false;
            Episode_Delete_btn.Visible = true;
        }
    }
}

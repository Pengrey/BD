 using System;
using System.Data;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace Trabalho_form
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        //Connection String
        string cs = "Data Source = tcp:mednat.ieeta.pt\\SQLSERVER,8101; Initial Catalog = p5g1; uid = p5g1; password = r0*7rFeu03Z";

        //btn_Submit Click event
        private void btn_Submit_Click(object sender, EventArgs e)
        {
            if (text_UserName.Text == "" || text_Password.Text == "")
            {
                MessageBox.Show("Please provide UserName and Password");
                return;
            }
            try
            {
                //Create SqlConnection
                SqlConnection con = new SqlConnection(cs);
                SqlCommand cmd = new SqlCommand("Select * from AnimeDB.Utilizador where email=@username and Palavra_Passe=@password", con);
                cmd.Parameters.AddWithValue("@username", text_UserName.Text);
                cmd.Parameters.AddWithValue("@password", GetStringSha256Hash(text_Password.Text));
                con.Open();
                SqlDataAdapter adapt = new SqlDataAdapter(cmd);
                DataSet ds = new DataSet();
                adapt.Fill(ds);
                con.Close();
                int count = ds.Tables[0].Rows.Count;
                //If count is equal to 1, than show frmMain form
                if (count == 1)
                {
                    MessageBox.Show("Login Successful!");
                    this.Hide();
                    String isAdmin = "T";
                    if (String.Equals(ds.Tables[0].Rows[0][3].ToString(), isAdmin)) { 
                        Form2 fm = new Form2();
                        fm.Show();
                    }
                    else
                    {
                        int userID = (int) ds.Tables[0].Rows[0][0];
                        frmMain fm = new frmMain(userID);
                        fm.Show();
                    }

                }
                else
                {
                    MessageBox.Show("Login Failed!");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private string GetStringSha256Hash(string text)
        {
            if (String.IsNullOrEmpty(text))
                return String.Empty;

            using (var sha = new System.Security.Cryptography.SHA256Managed())
            {
                byte[] textData = System.Text.Encoding.UTF8.GetBytes(text);
                byte[] hash = sha.ComputeHash(textData);
                return BitConverter.ToString(hash).Replace("-", String.Empty);
            }
        }
    }
}


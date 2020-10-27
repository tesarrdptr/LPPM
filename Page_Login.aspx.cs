using System;
using System.Data;
using System.Configuration;
using System.Web.Security;
using System.Web;
using System.Data.SqlClient;
using System.Web.Configuration;

public partial class Page_Login : System.Web.UI.Page
    {

    //PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
    SqlConnection Sqlconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (Context.User.Identity.IsAuthenticated)
            {
                Response.Redirect("Default");
            }
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
        Login();
        //divAlert.InnerHtml = "Nama akun atau kata sandi salah atau Anda belum terdaftar dalam aplikasi";
        //divAlert.Visible = true;

    }

    private void Login()
    {
        DataSet myDataSet = new DataSet();
        string tempRole = "";
        //int idMember;
        //string id = "";

        try
        {

            //membuat objek dengan nama myCommand, inisialisasi dari class SqlCommand
            SqlCommand sqlCmd = new SqlCommand("stp_Login", Sqlconn);

            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.AddWithValue("@p1", txtUsername.Text);
            sqlCmd.Parameters.AddWithValue("@p2", txtPassword.Text);
            Sqlconn.Open();
            SqlDataReader reader = sqlCmd.ExecuteReader();
            try
            {
                while (reader.Read())
                {

                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, txtUsername.Text, DateTime.Now, DateTime.Now.AddHours(10), true, "testng", FormsAuthentication.FormsCookiePath);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    //cookie.Domain = "127.0.0.1";
                    Response.Cookies.Add(cookie);
                    Response.Redirect("Default");
                }
                Sqlconn.Close();



            }
            catch (Exception ex)
            {
                divAlert.InnerHtml = ex.ToString();
                divAlert.Visible = true;
                
            }
        }
        catch (Exception ex)
        {
            divAlert.InnerHtml = ex.ToString();
            divAlert.Visible = true;
        }

        //return tempRole;
    }
     /*   protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                bool isAuthenticated = false;
                if (true == adAuthStaff.IsAuthenticated(Server.HtmlEncode(txtUsername.Text.Trim()), Server.HtmlEncode(txtPassword.Text.Trim())))
                {
                    dt = lib.CallProcedure("stp_getAuthentication", new String[] { Server.HtmlEncode(txtUsername.Text.Trim()) });
                    isAuthenticated = (dt.Rows.Count != 0 ? true : false);
                }
                else
                {
                    if (true == adAuthStudent.IsAuthenticated(Server.HtmlEncode(txtUsername.Text.Trim()), Server.HtmlEncode(txtPassword.Text.Trim())))
                    {
                        dt = lib.CallProcedure("stp_getAuthentication", new String[] { Server.HtmlEncode(txtUsername.Text.Trim()) });
                        isAuthenticated = (dt.Rows.Count != 0 ? true : false);
                    }
                }
                if (isAuthenticated)
                {
                    FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(1, Server.HtmlEncode(txtUsername.Text.Trim()), DateTime.Now, DateTime.Now.AddHours(10), true, dt.Rows[0][0].ToString() + "@" + dt.Rows[0][1].ToString(), FormsAuthentication.FormsCookiePath);
                    string encTicket = FormsAuthentication.Encrypt(ticket);
                    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
                    cookie.Domain = "application.polman.astra.ac.id";
                    Response.Cookies.Add(cookie);
                    Response.Redirect("Default");
                }
                else
                {
                    divAlert.InnerHtml = "Nama akun atau kata sandi salah atau Anda belum terdaftar dalam aplikasi";
                    divAlert.Visible = true;
                }
            }
            catch (Exception ex)
            {
                divAlert.InnerHtml = ex.Message + " " + ex.StackTrace;
                divAlert.Visible = true;
            }
        }*/
    }

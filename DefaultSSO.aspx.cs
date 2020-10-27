using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI;


public partial class _Default : Page
    {
    SqlConnection Sqlconn = new SqlConnection(WebConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString);
    DataTable dt = new DataTable();
    public PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());

    protected void Page_Load(object sender, EventArgs e)
        {
        fillData();
        //literal1.Text = Context.User.Identity.Name;
        if (Context.User.Identity.IsAuthenticated)
            {
            //dt = lib.CallProcedure("stp_getAppByUser", new string[] { Context.User.Identity.Name });
            String oldData = "";
            String content = "";
            content += "<table class='table' style='width:100%;'>";
            for (int i = 1; i < dt.Rows.Count; i++)
            {
                //literal1.Text = "aaaaaa";
                if (!oldData.Equals(dt.Rows[i][0].ToString()))
                {
                    if (!content.Equals(""))
                    {
                        content += "</div></td></tr>";
                    }
                    content += "<tr><td colspan='2' style='border-top-style:none;'><h4>" + dt.Rows[i][3].ToString() + "</h4></td></tr>";
                    content += "<tr><td style='border-top-style:none;width:15%;padding-right:10px;'><img src='Images/" + dt.Rows[i][2].ToString() + "'></td>";
                    content += "<td style='border-top-style:none;width:85%;'>Masuk sebagai:<br><br><div class='list-group'><a href='Redirect?token=" + lib.Encrypt(dt.Rows[i][1].ToString() + "@" + Context.User.Identity.Name + "@" + dt.Rows[i][5].ToString() + "@" + dt.Rows[i][6].ToString() + "@" + dt.Rows[i][7].ToString() + "@" + DateTime.Now.ToShortTimeString(), "SSOPolmanAstra") + "' class='list-group-item'>" + dt.Rows[i][4].ToString() + " (" + dt.Rows[i][8].ToString() + ")</a>";
                    //content += "<a href='Redirect?token=" + dt.Rows[i][1].ToString() + "@" + Context.User.Identity.Name + "@" + dt.Rows[i][5].ToString() + "@" + dt.Rows[i][6].ToString() + "@" + dt.Rows[i][7].ToString() + "@" + DateTime.Now.ToShortTimeString()+ "SSOPolmanAstra" + "' class='list-group-item'>" + dt.Rows[i][4].ToString() + " (" + dt.Rows[i][8].ToString() + ")</a>";
                    oldData = dt.Rows[i][0].ToString();
                }
                else
                {
                   // literal1.Text = "bbbbb";
                    content += "<a href='Redirect?token=" + lib.Encrypt(dt.Rows[i][1].ToString() + "@" + Context.User.Identity.Name + "@" + dt.Rows[i][5].ToString() + "@" + dt.Rows[i][6].ToString() + "@" + dt.Rows[i][7].ToString() + "@" + DateTime.Now.ToShortTimeString(), "SSOPolmanAstra") + "' class='list-group-item'>" + dt.Rows[i][4].ToString() + " (" + dt.Rows[i][8].ToString() + ")</a>";
                }
            }
            content += "</tr></table><br>";
            literalContent.Text = content;
        }
            else
            {
                Response.Redirect("Page_Login");
            }
        }

    private void fillData()
    {
        try
        {

            //membuat objek dengan nama myCommand, inisialisasi dari class SqlCommand
            SqlCommand sqlCmd = new SqlCommand("stp_getAppByUser", Sqlconn);

            sqlCmd.CommandType = CommandType.StoredProcedure;

            sqlCmd.Parameters.AddWithValue("@p1", Context.User.Identity.Name);
            Sqlconn.Open();
            SqlDataAdapter da = new SqlDataAdapter(sqlCmd);

           // SqlDataReader reader = sqlCmd.ExecuteReader();
            try
            {

                da.Fill(dt);


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
    }
    }

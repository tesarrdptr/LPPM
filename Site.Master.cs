using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using ProposalPolmanAstra.Classes;
using System.Web;


public partial class SiteMaster : MasterPage
{
    public PolmanAstraLibrary.PolmanAstraLibrary sso = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["SSOConnection"].ToString());
    public PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    LDAPAuthenticationStaff lastaff = new LDAPAuthenticationStaff();
    LDAPAuthenticationStudent lastudent = new LDAPAuthenticationStudent();
    DataTable dtMenu = new DataTable();
    DataTable dtSubMenu = new DataTable();

    public string Title
    {
        get
        {
            return lblTitle.Text;
        }
        set
        {
            lblTitle.Text = value;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {

        //if(Context.User.Identity.IsAuthenticated)
        //{
        //    literalMenu.Text = Context.User.Identity.Name;
        //}
        //else
        //{
        //    literalMenu.Text = "kosong";
        //}
        if (Context.User.Identity.IsAuthenticated)
        {
            FormsIdentity id = (FormsIdentity)Context.User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            String content = "";

            literalMenu.Text = ticket.UserData.Split('@')[1].ToString();
            if (ticket.UserData.Split('@')[1].Equals("APP07"))
            {
                try
                {
                    dtMenu = sso.CallProcedure("stp_getListMenu", new string[] { ticket.UserData.Split('@')[0], ticket.UserData.Split('@')[1] });
                   // literalMenu.Text = dtMenu.Rows[0][0].ToString();
                }
                catch (Exception ex)
                {
                    literalMenu.Text = ex.ToString();
                }

                for (int i = 0; i < dtMenu.Rows.Count; i++)
                {
                    if (dtMenu.Rows[i][1].ToString().Equals("#"))
                    {
                        var total_1 = lib.CallProcedure("stp_GetPengajuanPublikasiNotif", new string[] { }).Rows[0][0].ToString();
                        var total_2 = lib.CallProcedure("stp_GetProgressPengajuanTotal", new string[] { }).Rows[0][0].ToString();
                        var total_3 = lib.CallProcedure("stp_GetProposalPengajuanTotal", new string[] { }).Rows[0][0].ToString();
                        int total = (Convert.ToInt32(total_1) + Convert.ToInt32(total_2) + Convert.ToInt32(total_3));
                        if (dtMenu.Rows[i][2].ToString().Equals("Notifikasi") && total > 0)
                        {
                            content += "<li class='dropdown'><a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>" + dtMenu.Rows[i][2].ToString() + "&nbsp;<span class='caret'></span> <span class='badge'>" + total + "</span></a><ul class='dropdown-menu'>";
                        }
                        else
                        {
                            content += "<li class='dropdown'><a href='#' class='dropdown-toggle' data-toggle='dropdown' role='button' aria-haspopup='true' aria-expanded='false'>" + dtMenu.Rows[i][2].ToString() + "&nbsp;<span class='caret'></span></a><ul class='dropdown-menu'>";
                        }
                        dtSubMenu = sso.CallProcedure("stp_getListSubMenu", new string[] { dtMenu.Rows[i][0].ToString() });
                        for (int j = 0; j < dtSubMenu.Rows.Count; j++)
                        {
                            if (dtSubMenu.Rows[j][1].ToString() == "Page_NotifPublikasi")
                            {
                                if (total_1 == "0") content += "<li><a href='" + dtSubMenu.Rows[j][1].ToString() + "'>" + dtSubMenu.Rows[j][2].ToString() + "</a></li>";
                                else
                                    content += "<li><a href='" + dtSubMenu.Rows[j][1].ToString() + "'>" + dtSubMenu.Rows[j][2].ToString() + " <span class='badge'>" + total_1 + "</span></a></li>";
                            }
                            else if (dtSubMenu.Rows[j][1].ToString() == "Page_NotifProgress")
                            {
                                if (total_2 == "0") content += "<li><a href='" + dtSubMenu.Rows[j][1].ToString() + "'>" + dtSubMenu.Rows[j][2].ToString() + "</a></li>";
                                else
                                    content += "<li><a href='" + dtSubMenu.Rows[j][1].ToString() + "'>" + dtSubMenu.Rows[j][2].ToString() + " <span class='badge'>" + total_2 + "</span></a></li>";
                            }
                            else if (dtSubMenu.Rows[j][1].ToString() == "Page_LProposalDiajukan")
                            {
                                if (total_3 == "0") content += "<li><a href='" + dtSubMenu.Rows[j][1].ToString() + "'>" + dtSubMenu.Rows[j][2].ToString() + "</a></li>";
                                else
                                    content += "<li><a href='" + dtSubMenu.Rows[j][1].ToString() + "'>" + dtSubMenu.Rows[j][2].ToString() + " <span class='badge'>" + total_3 + "</span></a></li>";
                            }

                            else
                            {
                                content +=
                                    "<li><a href='" + dtSubMenu.Rows[j][1].ToString() + "'>" + dtSubMenu.Rows[j][2].ToString() + "</a></li>";
                            }
                        }
                        content += "</ul></li>";
                    }
                    else
                    {
                        if (dtMenu.Rows[i][1].ToString() == "Page_Trans_AuditPublikasi")
                        {
                            var total = lib.CallProcedure("stp_GetPengajuanPublikasiNotif", new string[] { }).Rows[0][0].ToString();
                            if (total == "0") content += "<li><a href='" + dtMenu.Rows[i][1].ToString() + "'>" + dtMenu.Rows[i][2].ToString() + "</a></li>";
                            else content += "<li><a href='" + dtMenu.Rows[i][1].ToString() + "'>" + dtMenu.Rows[i][2].ToString() + " <span class='badge'>" + total + "</span></a></li>";
                        }
                        else
                        {
                            content += "<li><a href='" + dtMenu.Rows[i][1].ToString() + "'>" + dtMenu.Rows[i][2].ToString() + "</a></li>";
                        }
                    }
                }
                literalMenu.Text = content;
            }

            welcome.InnerHtml = "Hai, <b>" + (Context.User.Identity.Name[0].Equals('0') ? lastudent.GetDisplayName(Context.User.Identity.Name) : lastaff.GetDisplayName(Context.User.Identity.Name)) + "</b> ";
            //welcome.InnerHtml = "Hai, <b>" + Context.User.Identity.Name + "</b> ";
        }
        else linkGoToSSO_Click(sender, e);
        setTitle(Page.Title);
    }
    protected void linkLogout_Click(object sender, EventArgs e)
    {
        Response.Redirect("Page_Logout");
    }

    protected void linkGoToSSO_Click(object sender, EventArgs e)
    {
        Response.Redirect("http://10.5.0.123/sso/");
    }

    public void goToChangePass()
    {
        //Response.Redirect("http://application.polman.astra.ac.id/sso");


        //Response.Redirect("http://localhost:1234/sso/Page_UbahPassword.aspx");
    }

    public void showAlertDanger(string message)
    {
        divAlert.Visible = true;
        divAlert.InnerHtml = message;
    }

    public void showAlertSuccess(string message)
    {
        divSuccess.Visible = true;
        divSuccess.InnerHtml = message;
    }

    public void hideAlert()
    {
        divAlert.Visible = false;
        divSuccess.Visible = false;
    }

    public void setTitle(string text)
    {
        Title = text;
        Page.Title = text;
    }

    protected void LinkChangePass_Click(object sender, EventArgs e)
    {
        FormsIdentity id = (FormsIdentity)Context.User.Identity;
        FormsAuthenticationTicket ticket = id.Ticket;
        ticket.UserData.Replace("APP07", "APP01");
        Session.Abandon();
        FormsAuthentication.SignOut();

        try
        {
            if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
            {
                var myCookie = new HttpCookie(FormsAuthentication.FormsCookieName);
                myCookie.Expires = DateTime.Now.AddDays(-1d);
                myCookie.Domain = "application.polman.astra.ac.id";
                Response.Cookies.Add(myCookie);
            }
        }
        catch { }

        string encTicket = FormsAuthentication.Encrypt(ticket);
        var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encTicket);
        cookie.Domain = "application.polman.astra.ac.id";
        Response.Cookies.Add(cookie);
        goToChangePass();
    }

    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        Response.Redirect("Page_UbahPassword");
    }


    protected void LinkProfil_Click(object sender, EventArgs e)
    {
        Response.Redirect("Page_Profil_Dosen");
    }
}

using System;
using System.Web;
using System.Web.Security;
using System.Web.UI;

public partial class Page_Logout : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Session.Abandon();
            FormsAuthentication.SignOut();

            try
            {
                if (Request.Cookies[FormsAuthentication.FormsCookieName] != null)
                {
                    var myCookie = new HttpCookie(FormsAuthentication.FormsCookieName);
                    myCookie.Expires = DateTime.Now.AddDays(-1d);
                    Response.Cookies.Add(myCookie);
                }
            }
            catch { }

            Response.Redirect("http://10.5.0.123/sso");
        }
}
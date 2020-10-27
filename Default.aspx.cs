using ProposalPolmanAstra.Classes;
using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

    public partial class _Default : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
        //if (Context.User.Identity.IsAuthenticated)
        //{
        //    Response.Redirect("Page_Login");
        //}
        panelWelcome.Visible = true;
            
        }
    }

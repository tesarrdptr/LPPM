using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;


    public partial class Page_LProposalDiajukan : System.Web.UI.Page
    {
        SiteMaster master;

        protected void Page_Load(object sender, EventArgs e)
        {
            master = this.Master as SiteMaster;
            if (!IsPostBack)
            {
                try
                {
                    FormsIdentity id = (FormsIdentity)Context.User.Identity;
                    FormsAuthenticationTicket ticket = id.Ticket;
                    if (!Context.User.Identity.IsAuthenticated || !ticket.UserData.Split('@')[1].Equals("APP07") || master.sso.CallProcedure("stp_getMenuByRole", new string[] { ticket.UserData.Split('@')[0], ticket.UserData.Split('@')[1], Request.Path.Split('/')[Request.Path.Split('/').Length - 1].Replace(".aspx", "") }).Rows.Count == 0)
                    {
                        Response.Redirect("http://application.polman.astra.ac.id/sso");
                    }
                }
                catch
                {
                    Response.Redirect("http://application.polman.astra.ac.id/sso");
                }
            }
           
            loadData();
            gridData.Width = Unit.Percentage(100);
        }

        protected void loadData()
        {
            gridData.DataSource = master.lib.CallProcedure("stp_getProposalDiajukan", new String[] { });
            gridData.DataBind();
            master.hideAlert();
        }
        protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridData.PageIndex = e.NewPageIndex;
            loadData();
        }
        protected void gridData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell abstraks = e.Row.Cells[2];
                abstraks.Text = abstraks.Text.Length > 30 ? abstraks.Text.Substring(0, 30) + "..." : abstraks.Text;
            }
        }
    }
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;


    public partial class Page_History : System.Web.UI.Page
    {
        SiteMaster master;
        DataTable dt = new DataTable();

        protected void Page_Load(object sender, EventArgs e)
        {
            master = this.Master as SiteMaster;
            master.setTitle("Daftar Riwayat");
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

                cariHistory.DataSource = master.lib.CallProcedure("stp_getHistoryScopes", new string[] { });
                cariHistory.DataValueField = "hsc_id";
                cariHistory.DataTextField = "hsc_title";
                cariHistory.DataBind();
                cariHistory.Items.Insert(0, new ListItem("---Semua Scope---", ""));
                
            }
            loadData();
            gridData.Width = Unit.Percentage(100);
        }

        protected void loadData()
        {
            string hsc_id = cariHistory.SelectedValue.ToString();
            string user = cariUser.Text;
            if(hsc_id == "")
            {
                if(user == "") gridData.DataSource = master.lib.CallProcedure("stp_getHistories", new String[] { });
                else gridData.DataSource = master.lib.CallProcedure("stp_getHistories", new String[] { user });
            }
            else
            {
                if(user == "") gridData.DataSource = master.lib.CallProcedure("stp_getHistoriesByScope", new String[] { hsc_id });
                else gridData.DataSource = master.lib.CallProcedure("stp_getHistoriesByScope", new String[] { hsc_id, user });
            }
            gridData.DataBind();

            master.hideAlert();
        }

        protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridData.PageIndex = e.NewPageIndex;
            loadData();
        }

        protected void btnCari_Click(object sender, EventArgs e)
        {
            loadData();
        }

        protected void gridData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell icon = e.Row.Cells[0];
                TableCell created_by = e.Row.Cells[1];
                TableCell msg = e.Row.Cells[2];
                TableCell created_date = e.Row.Cells[3];
                icon.Text = HttpUtility.HtmlDecode("<i class='glyphicon glyphicon-" + icon.Text + "'></i>");
                created_by.Text = HttpUtility.HtmlDecode("<b>" + created_by.Text + "</b>");
                created_date.Text = HttpUtility.HtmlDecode("<i>"+created_date.Text+"</i>");
                msg.Text = HttpUtility.HtmlDecode(msg.Text).Replace("-->", "<i class='glyphicon glyphicon-arrow-right'></i>");
            }
        }

        protected void gridData_PreRender(object sender, EventArgs e)
        {
            var gridView = (GridView)sender;
            var header = (GridViewRow)gridView.Controls[0].Controls[0];

            header.Cells[0].Visible = false;
            header.Cells[1].Visible = false;
            header.Cells[2].Visible = false;
            header.Cells[3].ColumnSpan = 4;
            header.Cells[3].Text = "Riwayat";
        }
    }

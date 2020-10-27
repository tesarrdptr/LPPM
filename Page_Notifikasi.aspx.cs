using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Web.Security;

    public partial class Page_Notifikasi : System.Web.UI.Page
    { 
        PolmanAstraLibrary.PolmanAstraLibrary lib2 = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
        //LDAPAuthenticationStaff lastaff = new LDAPAuthenticationStaff();
        DataTable dt = new DataTable();
        static CDO.Message message = new CDO.Message();
        static CDO.IConfiguration configuration = message.Configuration;
        static ADODB.Fields fields = configuration.Fields;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            { 
             ddStatus.Items.Add(new ListItem("--- Semua ---", "ALL"));
             ddStatus.Items.Add(new ListItem("Belum Dibaca", "0"));
             ddStatus.Items.Add(new ListItem("Terbaca", "1"));
            }
            loadData();
            gridData.Width = Unit.Percentage(100);
        }

        protected void loadData()
        {
            gridData.DataSource = lib2.CallProcedure("stp_getDataNotifikasi", new string[] { ddStatus.SelectedValue, Context.User.Identity.Name });
            gridData.DataBind();
            hideAlert();
            for (int i = 0; i < gridData.Rows.Count; i++)
            {
                if (gridData.Rows[i].Cells[4].Text.Equals("Terbaca"))
                {
                    ((LinkButton)gridData.Rows[i].FindControl("linkRead")).Visible = false;
                }
            }
        }

        protected void btnCari_Click(object sender, EventArgs e)
        {
            loadData();
        }

        protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridData.PageIndex = e.NewPageIndex;
            loadData();
        }

        protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
                if (e.CommandName == "Baca")
                {
                    lib2.CallProcedure("stp_setReadNotifikasi", new string[] { id, Context.User.Identity.Name });
                    loadData();
                }
            }
        }

        protected void showAlertDanger(string message)
        {
            divAlert.Visible = true;
            divAlert.InnerHtml = message;
        }

        protected void showAlertSuccess(string message)
        {
            divSuccess.Visible = true;
            divSuccess.InnerHtml = message;
        }

        protected void hideAlert()
        {
            divAlert.Visible = false;
            divSuccess.Visible = false;
        }

        protected void gridData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //TableCell abstraks = e.Row.Cells[2];
                TableCell status = e.Row.Cells[4];
                //abstraks.Text = abstraks.Text.Length > 30 ? abstraks.Text.Substring(0, 30) + "..." : abstraks.Text;
               
                if (status.Text == "Belum Dibaca")
                {
                    status.Text = HttpUtility.HtmlDecode("<span class='label label-danger'>Belum Dibaca</span>");

                }
                else if (status.Text == "Terbaca")
                {
                    status.Text = HttpUtility.HtmlDecode("<span class='label label-success'>Terbaca</span>");

                }
                
                //else if (status.Text == "-1") status.Text = HttpUtility.HtmlDecode("<span class='label label-danger'>Ditolak</span>");
                //else
                //{
                //    status.Text = HttpUtility.HtmlDecode("<span class='label label-warning' title='Menunggu konfirmasi anggota'>Konfirmasi</span>");
                //}

            }
        }
    }
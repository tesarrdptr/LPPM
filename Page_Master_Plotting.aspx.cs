using ProposalPolmanAstra.Classes;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Web.UI;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;

public partial class Page_KelolaProposal : System.Web.UI.Page
{
    SiteMaster master;
    System.Data.SqlClient.SqlConnection connection =
    new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);
    public PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    MailFunction mail = new MailFunction();

    //   LDAPAuthentication adAuth = new LDAPAuthentication();

    protected void Page_Load(object sender, EventArgs e)
    {
        master = this.Master as SiteMaster;
        if (!IsPostBack)
        {
            userPenelitian();
            FillDDLEdit();
            try
            {
                FormsIdentity id = (FormsIdentity)Context.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                int index = -1;
                for (int i = 0; i < ticket.UserData.Split('|').Length - 1; i++)
                {
                    if (ticket.UserData.Split('|')[i].Contains("APP01"))
                    {
                        index = i;
                    }
                }
                if (index != -1)
                {
                    if (!Context.User.Identity.IsAuthenticated || lib.CallProcedure("stp_getMenuByRole", new string[] { ticket.UserData.Split('|')[index].Split('@')[0], ticket.UserData.Split('|')[index].Split('@')[1], Request.Path.Split('/')[Request.Path.Split('/').Length - 1].Replace(".aspx", "") }).Rows.Count == 0)
                    {
                        Response.Redirect("Default");
                    }
                }
            }
            catch
            {
                Response.Redirect("Default");
            }
            //Role();
            //RoleEdit();
        }
        loadData();
        hideAlert();
        gridData.Width = Unit.Percentage(100);
    }

    protected void userPenelitian()
    {
        ddlAddReviewer.DataSource = lib.CallProcedure("stp_geUserReviewer", new String[] {});
        ddlAddReviewer.DataValueField = "rev_id";
        ddlAddReviewer.DataTextField = "iduser";
        ddlAddReviewer.DataBind();
        ddlAddReviewer.Items.Insert(0, new ListItem("--- Pilih Anggota ---", ""));
        ddlAddProposal.DataSource = lib.CallProcedure("stp_getProposalDDL", new String[] { });
        ddlAddProposal.DataValueField = "prp_id";
        ddlAddProposal.DataTextField = "prp_judul";
        ddlAddProposal.DataBind();
        ddlAddProposal.Items.Insert(0, new ListItem("--- Pilih Proposal ---", ""));

    }

    private void FillDDLEdit()
    {
        ddlEditReviewer.DataSource = lib.CallProcedure("stp_geUserReviewer", new String[] { });
        ddlEditReviewer.DataValueField = "rev_id";
        ddlEditReviewer.DataTextField = "iduser";
        ddlEditReviewer.DataBind();
        ddlEditReviewer.Items.Insert(0, new ListItem("--- Pilih Reviewer ---", ""));
        ddlEditProposal.DataSource = lib.CallProcedure("stp_getProposalDDL", new String[] { });
        ddlEditProposal.DataValueField = "prp_id";
        ddlEditProposal.DataTextField = "prp_judul";
        ddlEditProposal.DataBind();
        ddlEditProposal.Items.Insert(0, new ListItem("--- Pilih Jenis ---", ""));
    }

    protected void loadData()
    {
        gridData.DataSource = lib.CallProcedure("stp_getPlotting", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
        gridData.DataBind();
        hideAlert();

    }

    protected void hideAlert()
    {
        divAlert.Visible = false;
        divSuccess.Visible = false;
    }
  

    protected void linkTambah_Click(object sender, EventArgs e)
    {
        string act = Request.QueryString["action"];
        if (act != "new") Response.Redirect("Page_Master_Plotting?action=new");
        else
        {
            master.setTitle("Tambah Plotting Proposal");
            master.hideAlert();
            panelData.Visible = false;
            panelAdd.Visible = true;
            panelEdit.Visible = false;
        }
    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        ddlAddProposal.SelectedValue = "";
        ddlAddReviewer.SelectedValue = "";
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
        panelDetil.Visible = false;
    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
        String[] param = new String[3];
        param[0] = Server.HtmlEncode(ddlAddReviewer.SelectedValue.Trim());
        param[1] = Server.HtmlEncode(ddlAddProposal.SelectedValue.Trim());
        param[2] = Context.User.Identity.Name;

        try
        {
            lib.CallProcedure("stp_createPlotting", param);
            btnCancelAdd_Click(sender, e);
            loadData();
            //showAlertSuccess(param[0]+ param[1]+ param[2]);
            //showAlertSuccess("Tambah Plotting Proposal Berhasil");
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
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


    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        master.hideAlert();
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;

    }

    //protected void btnSubmitEdit_Click(object sender, EventArgs e)
    //{
    //    try
    //    {
    //        lib.CallProcedure("stp_editPlotting", new string[] { editPlotId.Text, ddlEditReviewer.SelectedValue, ddlEditProposal.SelectedValue });
    //        btnCancelEdit_Click(sender, e);
    //        loadData();
    //        showAlertSuccess("Ubah plotting proposal berhasil");

    //    }
    //    catch (Exception ex)
    //    {
    //        master.showAlertDanger(ex.Message + " " + ex.StackTrace);
    //    }
    //}

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

            if (e.CommandName == "Ubah")
            {
                int ind = Convert.ToInt32(e.CommandArgument);
                connection.Open();
                SqlCommand sqlCmd = new SqlCommand("stp_detailPlotting", connection);
                sqlCmd.Parameters.AddWithValue("@p1", gridData.Rows[ind].Cells[1].Text);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    ddlEditProposal.SelectedItem.Text = dr["proposal"].ToString();
                    ddlEditReviewer.SelectedItem.Text = dr["reviewer"].ToString();
                    //ddlEditTanggal.SelectedValue = dr["jenis"].ToString();
                    editPlotId.Text = gridData.Rows[ind].Cells[1].Text;
                    //showAlertSuccess(gridData.Rows[ind].Cells[1].Text);
                }
                connection.Close();

                //ddlEditProposal.SelectedValue = dt.Rows[0][1].ToString();
                //ddlEditReviewer.SelectedValue = dt.Rows[0][2].ToString();
                //ddlEditReviewer.Items.FindByText(gridData.Rows[i].Cells[2].Text).Selected = true;
                //ddlEditProposal.Items.FindByText(gridData.Rows[i].Cells[1].Text).Selected = true;
                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;
                panelDetil.Visible = false;


            }
           
            else if (e.CommandName == "Detil")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string tempid = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();

                dt = lib.CallProcedure("stp_detailPlotting", new string[] { id });
                //editKode.Text = id;
                //dtime = Convert.ToDateTime(dt.Rows[0][2].ToString());
                //dtime2 = Convert.ToDateTime(dt.Rows[0][3].ToString());

                TextBox1.Text = dt.Rows[0][1].ToString();
                TextBox2.Text = dt.Rows[0][2].ToString();
                TextBox4.Text = dt.Rows[0][3].ToString();
                TextBox5.Text = dt.Rows[0][4].ToString();
                TextBox6.Text = dt.Rows[0][5].ToString();
                TextBox7.Text = dt.Rows[0][6].ToString();
                //TextBox4.Text = GetDataItem.
                //ddlRoleEdit.SelectedValue = dt.Rows[0][2].ToString();
                panelAdd.Visible = false;
                //panelEdit.Visible = true;
                panelData.Visible = false;
                panelDetil.Visible = true;
            }
        }
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib.CallProcedure("stp_deletePlotting", new string[] { txtConfirmDelete.Text, Context.User.Identity.Name });
        loadData();
        master.showAlertSuccess("Hapus Proposal berhasil");
    }



    protected void grdAnggotaProposal_RowDataBound(object sender, GridViewRowEventArgs e)
    {

    }

    protected void btnCari_Click(object sender, EventArgs e)
    {

    }

    protected void btnSubmitEdit_Click1(object sender, EventArgs e)
    {
        try
        {
            lib.CallProcedure("stp_editPlotting", new string[] { editPlotId.Text, ddlEditProposal.SelectedValue, ddlEditReviewer.SelectedValue, Context.User.Identity.Name });
            btnCancelEdit_Click(sender, e);
            loadData();
            showAlertSuccess("Ubah plotting proposal berhasil");

        }
        catch (Exception ex)
        {
            master.showAlertDanger(ex.Message + " " + ex.StackTrace);
        }
    }
}

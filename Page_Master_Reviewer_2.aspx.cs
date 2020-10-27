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

public partial class Page_Master_Reviewer : System.Web.UI.Page
{
    SiteMaster master;
    System.Data.SqlClient.SqlConnection connection =
    new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);
    public PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        master = this.Master as SiteMaster;
        if (!IsPostBack)
        {
            
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
            userReviewer();
            //userReviewerEdit();
        }
        loadData();
        hideAlert();
        gridData.Width = Unit.Percentage(100);
    }

    protected void loadData()
    {
        gridData.DataSource = lib.CallProcedure("stp_getReviewer", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
        gridData.DataBind();
        hideAlert();
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
        DateTime dtime, dtime2;
        if (e.CommandName != "Page")
        {
            String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
            if (e.CommandName == "Ubah")
            {
                userReviewerEdit();
                dt = lib.CallProcedure("stp_detailReviewer", new string[] { id });
                editKode.Text = id;
                dtime = Convert.ToDateTime(dt.Rows[0][2].ToString());
                dtime2 = Convert.ToDateTime(dt.Rows[0][3].ToString());

                ddlBidfokEdit.SelectedValue= dt.Rows[0][5].ToString();
                ddlReviewerEdit.SelectedValue = dt.Rows[0][1].ToString();
                ddlJenisEdit.SelectedValue= dt.Rows[0][4].ToString();
                editTanggalMulai.Text = dtime.ToString("yyyy-MM-dd");
                editTanggalSelesai.Text = dtime2.ToString("yyyy-MM-dd");
                //editTanggalSelesai.Text = dtime2.ToString("yyyy-MM-dd");
                //ddlBidfokEdit.SelectedValue = dt.Rows[0][5].ToString();
                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;
                panelDetil.Visible = false;
            }
            else if (e.CommandName == "Detil")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string tempid = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();

                dt = lib.CallProcedure("stp_detailReviewer", new string[] { id });
                editKode.Text = id;
                dtime = Convert.ToDateTime(dt.Rows[0][2].ToString());
                dtime2 = Convert.ToDateTime(dt.Rows[0][3].ToString());

                TextBox1.Text = dt.Rows[0][1].ToString();
                TextBox2.Text = dtime.ToString("yyyy-MM-dd");
                TextBox3.Text = dtime2.ToString("yyyy-MM-dd");
                TextBox4.Text = dt.Rows[0][4].ToString();
                TextBox5.Text = dt.Rows[0][5].ToString();
                TextBox6.Text = dt.Rows[0][6].ToString();
                TextBox7.Text = dt.Rows[0][7].ToString();
                TextBox8.Text = dt.Rows[0][8].ToString();
                //TextBox4.Text = GetDataItem.
                //ddlRoleEdit.SelectedValue = dt.Rows[0][2].ToString();
                panelAdd.Visible = false;
                //panelEdit.Visible = true;
                panelData.Visible = false;
                panelDetil.Visible = true;
            }
        }
        else
        {
            showAlertSuccess("gagal masuk");
        }
    }

    protected void userReviewer()
    {
        ddlReviewer.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        ddlReviewer.DataValueField = "iduser";
        ddlReviewer.DataTextField = "iduser";
        ddlReviewer.DataBind();
        ddlReviewer.Items.Insert(0, new ListItem("--- Pilih Reviewer ---", ""));
        ddlJenis.DataSource = lib.CallProcedure("stp_getJenisProposal", new String[] { });
        ddlJenis.DataValueField = "jns_id";
        ddlJenis.DataTextField = "jns_title";
        ddlJenis.DataBind();
        ddlJenis.Items.Insert(0, new ListItem("--- Pilih Jenis ---", ""));
        ddlBidfok.DataSource = lib.CallProcedure("stp_getBidangFokus", new String[] { });
        ddlBidfok.DataValueField = "bidfok_id";
        ddlBidfok.DataTextField = "bidfok_name";
        ddlBidfok.DataBind();
        ddlBidfok.Items.Insert(0, new ListItem("--- Pilih Bidang Fokus ---", ""));
    }

    protected void userReviewerEdit()
    {
        ddlReviewerEdit.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        ddlReviewerEdit.DataValueField = "iduser";
        ddlReviewerEdit.DataTextField = "iduser";
        ddlReviewerEdit.DataBind();
        ddlReviewerEdit.Items.Insert(0, new ListItem("--- Pilih Reviewer ---", ""));
        ddlJenisEdit.DataSource = lib.CallProcedure("stp_getJenisProposal", new String[] { });
        ddlJenisEdit.DataValueField = "jns_id";
        ddlJenisEdit.DataTextField = "jns_title";
        ddlJenisEdit.DataBind();
        ddlJenisEdit.Items.Insert(0, new ListItem("--- Pilih Jenis ---", ""));
        ddlBidfokEdit.DataSource = lib.CallProcedure("stp_getBidangFokus", new String[] { });
        ddlBidfokEdit.DataValueField = "bidfok_id";
        ddlBidfokEdit.DataTextField = "bidfok_name";
        ddlBidfokEdit.DataBind();
        ddlBidfokEdit.Items.Insert(0, new ListItem("--- Pilih Bidang Fokus ---", ""));
    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
        panelDetil.Visible = false;
    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
        if (cekReviewer())
        {
            showAlertDanger("ada datanya");
        }
        else
        {
            //    showAlertSuccess("ga ada datanya");
            String[] param = new String[6];
            param[0] = Server.HtmlEncode(AddTanggalMulai.Text.Trim());
            param[1] = Server.HtmlEncode(AddTanggalSelesai.Text.Trim());
            param[2] = Context.User.Identity.Name;
            param[3] = Server.HtmlEncode(ddlReviewer.SelectedValue.Trim());
            param[4] = Server.HtmlEncode(ddlJenis.SelectedValue.Trim());
            param[5] = Server.HtmlEncode(ddlBidfok.SelectedValue.Trim());

            try
            {
                lib.CallProcedure("stp_createReviewer", param);
                master.sso.CallProcedure("stp_createReviewer", new string[] { ddlReviewer.SelectedValue });
                btnCancelAdd_Click(sender, e);
                loadData();
                showAlertSuccess("Berhasil Menambah Data");
            }
            catch (Exception ex)
            {
                showAlertDanger(ex.Message + " " + ex.StackTrace);
            }
        }

    }

    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
    }

    protected void btnSubmitEdit_Click(object sender, EventArgs e)
    {
        try
        {

            string[] param = new string[8];
            param[0] = editKode.Text;
            param[1] = Server.HtmlEncode(ddlReviewerEdit.SelectedValue.Trim());
            param[2] = Server.HtmlEncode(ddlJenisEdit.SelectedValue.Trim());
            param[3] = Server.HtmlEncode(editTanggalMulai.Text);
            param[4] = Server.HtmlEncode(editTanggalSelesai.Text);
            param[5] = Server.HtmlEncode(editRevId.Text);
            param[6] = Server.HtmlEncode(ddlBidfokEdit.SelectedValue.Trim());
            param[7] = Context.User.Identity.Name;

            lib.CallProcedure("stp_editReviewer", param);

            //lib.CallProcedure("stp_editReviewer", new string[] { editKode.Text,
            //    editTanggalMulai.Text,
            //    editTanggalSelesai.Text, Context.User.Identity.Name,
            //    ddlReviewerEdit.SelectedValue, ddlJenisEdit.SelectedValue,
            //    ddlBidfokEdit.SelectedValue });
            btnCancelEdit_Click(sender, e);
            loadData();
            showAlertSuccess("Ubah reviewer berhasil");
            //showAlertSuccess(param[0] + param[1] + param[2] + param[3]
            //    + param[4] + param[5] + param[6] + param[7]);
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
        }
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib.CallProcedure("stp_deleteReviewer", new string[] { txtConfirmDelete.Text });
        master.sso.CallProcedure("stp_deleteReviewer", new string[] { txtConfirmDelete.Text });
        loadData();
        showAlertSuccess("Hapus reviewer berhasil");
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

    protected void linkTambah_Click(object sender, EventArgs e)
    {
        //master.setTitle("Tambah Reviewer");
        hideAlert();
        panelData.Visible = false;
        panelAdd.Visible = true;
        panelEdit.Visible = false;
    }

    protected bool cekReviewer()
    {
        bool auth=false;
        DataTable dtrev;
        dtrev=master.sso.CallProcedure("stp_getStatusReviewer", new string[] { ddlReviewer.SelectedValue });
        int row = dtrev.Rows.Count;
        if(row==0)
        {
            auth = false;
        }
        else
        {
            auth = true;
        }
        return auth;
    }


}
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
            //Role();
            //RoleEdit();
        }
        userReviewer();
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
        if (e.CommandName != "Page")
        {
            String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
            if (e.CommandName == "Ubah")
            {
                dt = lib.CallProcedure("stp_detailReviewer", new string[] { id });
                //editKode.Text = id;

                ddlJenisEdit.DataSource = lib.CallProcedure("stp_getJenisProposal", new String[] { });
                ddlJenisEdit.DataValueField = "jns_id";
                ddlJenisEdit.DataTextField = "jns_title";
                ddlJenisEdit.DataBind();
                ddlReviewerEdit.SelectedValue = dt.Rows[0][1].ToString();
                //ddlJenisEdit.SelectedValue = dt.Rows[0][4].ToString();
                editTanggalMulai.Text = dt.Rows[0][2].ToString();
                editTanggalSelesai.Text = dt.Rows[0][3].ToString();
                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;
            }
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
    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
        try
        {
            if (ddlReviewer.SelectedValue == null || ddlJenis.SelectedValue == null || AddTanggalMulai.Text == null || AddTanggalSelesai.Text == null)
            {
                showAlertSuccess("Gagal");
            }
            else 
            {
                
                SqlCommand insertcmd = new SqlCommand("stp_createReviewer", connection);
                insertcmd.CommandType = CommandType.StoredProcedure;

                insertcmd.Parameters.AddWithValue("@p1", AddTanggalMulai.Text.ToUpper().ToString());
                insertcmd.Parameters.AddWithValue("@p2", AddTanggalSelesai.Text.ToUpper().ToString());
                insertcmd.Parameters.AddWithValue("@p3", Context.User.Identity.Name);
                insertcmd.Parameters.AddWithValue("@p4", ddlReviewer.SelectedValue.ToUpper().ToString());
                insertcmd.Parameters.AddWithValue("@p5", ddlJenis.SelectedValue.ToUpper().ToString());

                //System.Data.SqlClient.SqlParameter param;

                //param = insertcmd.Parameters.Add("@p1", SqlDbType.VarChar, 50);
                //param.Direction = ParameterDirection.Input;
                //param.Value = AddTanggalMulai.Text;

                //param = insertcmd.Parameters.Add("@p2", SqlDbType.VarChar, 50);
                //param.Direction = ParameterDirection.Input;
                //param.Value = AddTanggalSelesai.Text;

                //param = insertcmd.Parameters.Add("@p3", SqlDbType.VarChar, 50);
                //param.Direction = ParameterDirection.Input;
                //param.Value = Context.User.Identity.Name;

                //param = insertcmd.Parameters.Add("@p4", SqlDbType.VarChar, 50);
                //param.Direction = ParameterDirection.Input;
                //param.Value = ddlReviewer.SelectedValue;

                //param = insertcmd.Parameters.Add("@p5", SqlDbType.VarChar, 50);
                //param.Direction = ParameterDirection.Input;
                //param.Value = ddlJenis.SelectedValue;

                connection.Open();
                insertcmd.ExecuteNonQuery();
                connection.Close();

                //String[] param = new String[5];
                //var rev = lib.CallProcedure("stp_createReviewer", param);
                //param[0] = Server.HtmlEncode(AddTanggalMulai.Text);
                //param[1] = Server.HtmlEncode(AddTanggalSelesai.Text); 
                //param[2] = Context.User.Identity.Name;
                //param[3] = Server.HtmlEncode(ddlReviewer.SelectedValue);
                //param[4] = Server.HtmlEncode(ddlJenis.SelectedValue);

                //var prp = lib.CallProcedure("stp_createReviewer", param);

                lib.CallProcedure("stp_createReviewer", new string[]  { Server.HtmlEncode(AddTanggalMulai.Text), AddTanggalSelesai.Text, Context.User.Identity.Name, ddlReviewer.SelectedValue, ddlJenis.SelectedValue });
                //btnCancelAdd_Click(sender, e);
                loadData();
                showAlertSuccess("Tambah reviewer berhasil");
            }
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
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

            //string[] param = new string[8];
            //param[0] = Context.User.Identity.Name;
            //param[1] = Server.HtmlEncode(ddlReviewerEdit.SelectedValue);
            //param[2] = Server.HtmlEncode(ddlJenisEdit.SelectedValue);
            //param[3] = Server.HtmlEncode(editTanggalMulai.Text);
            //param[4] = Server.HtmlEncode(editTanggalSelesai.Text);
            //param[5] = Server.HtmlEncode(editRevId.Text);

            lib.CallProcedure("stp_editReviewer", new string[] { editKode.Text, editTanggalMulai.Text, editTanggalSelesai.Text, Context.User.Identity.Name, ddlReviewer.SelectedValue, ddlJenisEdit.SelectedValue });
            btnCancelEdit_Click(sender, e);
            loadData();
            showAlertSuccess("Ubah reviewer berhasil");
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
        }
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib.CallProcedure("stp_deleteReviewer", new string[] { txtConfirmDelete.Text });
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
}
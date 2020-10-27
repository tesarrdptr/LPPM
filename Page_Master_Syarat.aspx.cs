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

public partial class Page_Master_Syarat : System.Web.UI.Page
{
    SiteMaster master;
    System.Data.SqlClient.SqlConnection connection =
    new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);
    public PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    MailFunction mail = new MailFunction();
    protected void Page_Load(object sender, EventArgs e)
    {
        master = this.Master as SiteMaster;
        if (!IsPostBack)
        {
            ddlAdd();
            ddlEdit();
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

    protected void ddlAdd()
    {
        ddlAddPP.DataSource = lib.CallProcedure("stp_getJenisPP", new String[] { Context.User.Identity.Name });
        ddlAddPP.DataValueField = "jpp_id";
        ddlAddPP.DataTextField = "jpp_nama";
        ddlAddPP.DataBind();
        ddlAddPP.Items.Insert(0, new ListItem("--- Pilih Jenis ---", ""));
        ddlAddNIDN.Items.Add(new ListItem("Memiliki", "0"));
        ddlAddNIDN.Items.Add(new ListItem("Tidak Memiliki", "1"));
        ddlAddNIDN.Items.Insert(0, new ListItem("--- Pilih Status ---", ""));
        ddlAddJabatan.Items.Add(new ListItem("-", "-"));
        ddlAddJabatan.Items.Add(new ListItem("Asisten Ahli", "Asisten Ahli"));
        ddlAddJabatan.Items.Add(new ListItem("Lektor", "Lektor"));
        ddlAddJabatan.Items.Add(new ListItem("Lektor Kepala", "Lektor Kepala"));
        ddlAddJabatan.Items.Add(new ListItem("Profesor ", "Profesor "));
        ddlAddJabatan.Items.Insert(0, new ListItem("--- Pilih Jabatan ---", ""));
        ddlAddPendidikan.Items.Add(new ListItem("D-3", "D-3"));
        ddlAddPendidikan.Items.Add(new ListItem("S-1", "S-1"));
        ddlAddPendidikan.Items.Add(new ListItem("S-2", "S-2"));
        ddlAddPendidikan.Items.Add(new ListItem("S-3", "S-3"));
        ddlAddPendidikan.Items.Insert(0, new ListItem("--- Pilih Pendidikan Terakhir ---", ""));
    }

    protected void ddlEdit()
    {
        ddlEditPP.DataSource = lib.CallProcedure("stp_getJenisPP", new String[] { Context.User.Identity.Name });
        ddlEditPP.DataValueField = "jpp_id";
        ddlEditPP.DataTextField = "jpp_nama";
        ddlEditPP.DataBind();
        ddlEditPP.Items.Insert(0, new ListItem("--- Pilih Jenis ---", ""));
        ddlEditNIDN.Items.Add(new ListItem("Memiliki", "0"));
        ddlEditNIDN.Items.Add(new ListItem("Tidak Memiliki", "1"));
        ddlEditNIDN.Items.Insert(0, new ListItem("--- Pilih Status ---", ""));
        ddlEditJabatan.Items.Add(new ListItem("-", "-"));
        ddlEditJabatan.Items.Add(new ListItem("Asisten Ahli", "Asisten Ahli"));
        ddlEditJabatan.Items.Add(new ListItem("Lektor", "Lektor"));
        ddlEditJabatan.Items.Add(new ListItem("Lektor Kepala", "Lektor Kepala"));
        ddlEditJabatan.Items.Add(new ListItem("Profesor ", "Profesor "));
        ddlEditJabatan.Items.Insert(0, new ListItem("--- Pilih Jabatan ---", ""));
        ddlEditPendidikan.Items.Add(new ListItem("D-3", "D-3"));
        ddlEditPendidikan.Items.Add(new ListItem("S-1", "S-1"));
        ddlEditPendidikan.Items.Add(new ListItem("S-2", "S-2"));
        ddlEditPendidikan.Items.Add(new ListItem("S-3", "S-3"));
        ddlEditPendidikan.Items.Insert(0, new ListItem("--- Pilih Pendidikan Terakhir ---", ""));
    }

    protected void gridData_Sorting(object sender, GridViewSortEventArgs e)
    {

    }

    protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

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
                SqlCommand sqlCmd = new SqlCommand("stp_detailSyarat", connection);
                sqlCmd.Parameters.AddWithValue("@p1", id);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    ddlEditNIDN.SelectedItem.Text = dr["syr_nidn"].ToString();
                    ddlEditJabatan.SelectedValue = dr["syr_job"].ToString();
                    ddlEditPendidikan.SelectedValue = dr["syr_edu"].ToString();
                    ddlEditPP.SelectedValue = dr["jpp_id"].ToString();
                    editPlotId.Text = id;
                }
                connection.Close();

                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;


            }
        }
    }

    protected void loadData()
    {
        if (txtCari.Text != "")
        {
            tglMulaiCari.Text = null;
            tglSelesaiCari.Text = null;
            //query = "SELECT ROW_NUMBER() over (order by bidfok_created_date desc) as no, bidfok_name, CONVERT(varchar(12), bidfok_created_date, 113) as bidfok_created_date, CONVERT(varchar(12), bidfok_updated_date, 113) as bidfok_updated_date, bidfok_created_by, bidfok_updated_by from  MSBIDANGFOKUS";

        }
        if  (tglMulaiCari.Text == String.Empty || tglSelesaiCari.Text == String.Empty)
        {
            gridData.DataSource = lib.CallProcedure("stp_getSyarat", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
            gridData.DataBind();
        }
        else
        {
            gridData.DataSource = lib.CallProcedure("stp_searchsyaratbydate", new string[] { Server.HtmlEncode(tglMulaiCari.Text), Server.HtmlEncode(tglSelesaiCari.Text) });
            gridData.DataBind();
        }
        hideAlert();
    }

    protected void hideAlert()
    {
        divAlert.Visible = false;
        divSuccess.Visible = false;
    }

    protected void linkTambah_Click(object sender, EventArgs e)
    {

        panelData.Visible = false;
        panelAdd.Visible = true;
        //panelEdit.Visible = false;
    }

    protected void btnCari_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        panelAdd.Visible = false;
        panelData.Visible = true;
        ddlAddPP.SelectedValue = "";
        ddlAddPendidikan.SelectedValue = "";
        ddlAddJabatan.SelectedValue = "";
        ddlAddNIDN.SelectedValue = "";
    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
        try
        {
            String[] param = new String[5];
            //param[0] = editKode.Text;
            param[0] = Server.HtmlEncode(ddlAddNIDN.SelectedValue.Trim());
            param[1] = Server.HtmlEncode(ddlAddJabatan.SelectedValue.Trim());
            param[2] = Server.HtmlEncode(ddlAddPendidikan.SelectedValue.Trim());
            param[3] = Server.HtmlEncode(ddlAddPP.SelectedValue.Trim());
            param[4] = Context.User.Identity.Name;

            try
            {
                lib.CallProcedure("stp_createSyarat", param);
                btnCancelAdd_Click(sender, e);
                loadData();
                showAlertSuccess("Berhasil Menambah Data");
                //showAlertSuccess(param[0] + " " + param[1] + " " + param[2] + " " + param[3] + " " + param[4] + " " + param[5] +
                //    " " + param[6] + " " + param[7] + " " + param[8] +
                //   " " + param[9] + " " + param[10] + " " + param[11] );
            }
            catch (Exception ex)
            {
                showAlertDanger(ex.Message + " " + ex.StackTrace);
            }
        }
        catch
        {
            showAlertSuccess("GAGAL");
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
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
    }

    protected void btnSubmitEdit_Click(object sender, EventArgs e)
    {
        try
        {

            String[] param = new String[6];
            param[0] = editPlotId.Text;
            param[1] = Server.HtmlEncode(ddlEditNIDN.SelectedValue.Trim());
            param[2] = Server.HtmlEncode(ddlEditJabatan.SelectedValue.Trim());
            param[3] = Server.HtmlEncode(ddlEditPendidikan.SelectedValue.Trim());
            param[4] = Server.HtmlEncode(ddlEditPP.SelectedValue.Trim());
            param[5] = Context.User.Identity.Name;

            lib.CallProcedure("stp_editSyarat", param);
            btnCancelEdit_Click(sender, e);
            loadData();
            showAlertSuccess("Ubah syarat berhasil");
            //showAlertSuccess(param[0] + " " + param[1] + " " + param[2] + " " + param[3]
            //    + " " + param[4] + " " + param[5]);
            //+ " " + param[6] + " " + param[7] + " " + param[8]
            //    + " " + param[9] + " " + param[10] + " " + param[11] + " " + param[12]);
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
        }
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib.CallProcedure("stp_deleteSyarat", new string[] { txtConfirmDelete.Text, Context.User.Identity.Name });
        loadData();
        master.showAlertSuccess("Hapus Syarat berhasil");
    }
}
using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web.Security;
public partial class Page_Master_Tanggal : System.Web.UI.Page
{
    PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        //AddTanggalMulai.StartDate = DateTime.Today;
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

        }
        loadData();
        hideAlert();
        gridData.Width = Unit.Percentage(100);
    }

    protected void loadData()
    {
        gridData.DataSource = lib.CallProcedure("stp_getTanggal", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
        gridData.DataBind();
        hideAlert();
    }

    protected void hideAlert()
    {
        divAlert.Visible = false;
        divSuccess.Visible = false;
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
        DateTime dtime,dtime2;
        if (e.CommandName != "Page")
        {
            String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
            if (e.CommandName == "Ubah")
            {
                dt = lib.CallProcedure("stp_detailTanggal", new string[] { id });
                editKode.Text = id;
                dtime = Convert.ToDateTime(dt.Rows[0][2].ToString());
                dtime2 = Convert.ToDateTime(dt.Rows[0][3].ToString());

                editTanggal.Text = dt.Rows[0][1].ToString();
                editTanggalMulai.Text = dtime.ToString("yyyy-MM-dd");
                editTanggalSelesai.Text = dtime2.ToString("yyyy-MM-dd");
                //ddlRoleEdit.SelectedValue = dt.Rows[0][2].ToString();
                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;
                panelDetil.Visible = false;
            }
            else if (e.CommandName == "Detil")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string tempid = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();

                dt = lib.CallProcedure("stp_detailTanggal", new string[] { id });
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
                //TextBox4.Text = GetDataItem.
                //ddlRoleEdit.SelectedValue = dt.Rows[0][2].ToString();
                panelAdd.Visible = false;
                //panelEdit.Visible = true;
                panelData.Visible = false;
                panelDetil.Visible = true;
            }
            else
            {
                
            }
        }
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
        try
        {
            lib.CallProcedure("stp_createTanggal", new string[] { Server.HtmlEncode(AddTanggal.Text.Trim()), AddTanggalMulai.Text, AddTanggalSelesai.Text, Context.User.Identity.Name, });
            btnCancelAdd_Click(sender, e);
            loadData();
            showAlertSuccess("Tambah tanggal berhasil");
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

            lib.CallProcedure("stp_editTanggal", new string[] { editKode.Text, editTanggal.Text, editTanggalMulai.Text, editTanggalSelesai.Text, Context.User.Identity.Name });
            btnCancelEdit_Click(sender, e);
            loadData();
            showAlertSuccess("Ubah tanggal berhasil");
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
        }
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib.CallProcedure("stp_deleteTanggal", new string[] { txtConfirmDelete.Text });
        loadData();
        showAlertSuccess("Hapus tanggal berhasil");
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

    protected void linkTambah_Click(object sender, EventArgs e)
    {
        panelData.Visible = false;
        panelAdd.Visible = true;
        panelEdit.Visible = false;
    }
}
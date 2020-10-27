using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class Page_Master_BidangFokus : System.Web.UI.Page
{
    PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();

    protected void Page_Load(object sender, EventArgs e)
    {
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
        gridData.DataSource = lib.CallProcedure("stp_getDataBidangFokus", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
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
        panelData.Visible = false;
        panelAdd.Visible = true;
        panelEdit.Visible = false;
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
                dt = lib.CallProcedure("stp_detailBidangFokus", new string[] { id });
                editKode.Text = id;
                editPub.Text = dt.Rows[0][1].ToString();
                //ddlRoleEdit.SelectedValue = dt.Rows[0][2].ToString();
                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;
            }
            else if (e.CommandName == "Detil")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string tempid = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();

                dt = lib.CallProcedure("stp_detailBidangFokus", new string[] { id });
                editKode.Text = id;

                TextBox1.Text = dt.Rows[0][1].ToString();
                TextBox4.Text = dt.Rows[0][2].ToString();
                TextBox5.Text = dt.Rows[0][3].ToString();
                TextBox6.Text = dt.Rows[0][4].ToString();
                TextBox7.Text = dt.Rows[0][5].ToString();
                panelAdd.Visible = false;
                panelData.Visible = false;
                panelDetil.Visible = true;
            }
        }
    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
        panelDetil.Visible = false;

        AddJenisPub.Text = " ";
    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
        try
        {
            lib.CallProcedure("stp_createBidangFokus", new string[] { Server.HtmlEncode(AddJenisPub.Text.Trim()), Context.User.Identity.Name });
            btnCancelAdd_Click(sender, e);
            loadData();
            showAlertSuccess("Tambah bidang fokus berhasil");
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
            lib.CallProcedure("stp_editBidangFokus", new string[] { editKode.Text, editPub.Text, Context.User.Identity.Name });
            btnCancelEdit_Click(sender, e);
            loadData();
            showAlertSuccess("Ubah bidang fokus berhasil");
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
        }
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib.CallProcedure("stp_deleteBidangFokus", new string[] { txtConfirmDelete.Text });
        loadData();
        showAlertSuccess("Hapus bidang fokus berhasil");
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
    
}
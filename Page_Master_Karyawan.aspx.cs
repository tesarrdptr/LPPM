using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;

public partial class Default2 : System.Web.UI.Page
{
    //PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    PolmanAstraLibrary.PolmanAstraLibrary sso = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["SSOConnection"].ToString());
    DataTable dt = new DataTable();
    SiteMaster master;

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
                    if (!Context.User.Identity.IsAuthenticated || sso.CallProcedure("stp_getMenuByRole", new string[] { ticket.UserData.Split('|')[index].Split('@')[0], ticket.UserData.Split('|')[index].Split('@')[1], Request.Path.Split('/')[Request.Path.Split('/').Length - 1].Replace(".aspx", "") }).Rows.Count == 0)
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
        }
        loadData();
        hideAlert();
        gridData.Width = Unit.Percentage(100);
    }

    protected void loadData()
    {
        gridData.DataSource = sso.CallProcedure("stp_getDataDosen", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
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

    protected void userReviewer()
    {
        ddlKaryawan.DataSource = sso.CallProcedure("stp_getUser", new String[] { Context.User.Identity.Name });
        ddlKaryawan.DataValueField = "id";
        ddlKaryawan.DataTextField = "iduser";
        ddlKaryawan.DataBind();
        ddlKaryawan.Items.Insert(0, new ListItem("--- Pilih Reviewer ---", ""));
        ddlProdi.Items.Add(new ListItem("P4", "P4"));
        ddlProdi.Items.Add(new ListItem("TPM", "TPM"));
        ddlProdi.Items.Add(new ListItem("MI", "MI"));
        ddlProdi.Items.Add(new ListItem("PMO ", "PMO"));
        ddlProdi.Items.Add(new ListItem("MK", "MK"));
        ddlProdi.Items.Add(new ListItem("TKBG", "TKBG"));
        ddlProdi.Items.Insert(0, new ListItem("--- Pilih Program Studi ---", ""));
        ddlPosisi.Items.Add(new ListItem("Asisten Ahli", "Asisten Ahli"));
        ddlPosisi.Items.Add(new ListItem("Lektor", "Lektor"));
        ddlPosisi.Items.Add(new ListItem("Lektor Kepala", "Lektor Kepala"));
        ddlPosisi.Items.Add(new ListItem("Profesor ", "Profesor "));
        ddlPosisi.Items.Insert(0, new ListItem("--- Pilih Jabatan ---", ""));
        ddlPendidikan.Items.Add(new ListItem("D-3", "D-3"));
        ddlPendidikan.Items.Add(new ListItem("S-1", "S-1"));
        ddlPendidikan.Items.Add(new ListItem("S-2", "S-2"));
        ddlPendidikan.Items.Add(new ListItem("S-3", "S-3"));
        ddlPendidikan.Items.Insert(0, new ListItem("--- Pilih Pendidikan Terakhir ---", ""));
    }

    protected void userReviewerEdit()
    {
        ddlKaryawanEdit.DataSource = sso.CallProcedure("stp_getUser", new String[] { Context.User.Identity.Name });
        ddlKaryawanEdit.DataValueField = "id";
        ddlKaryawanEdit.DataTextField = "iduser";
        ddlKaryawanEdit.DataBind();
        ddlKaryawanEdit.Items.Insert(0, new ListItem("--- Pilih Karyawan ---", ""));
        ddlProdiEdit.Items.Add(new ListItem("P4", "P4"));
        ddlProdiEdit.Items.Add(new ListItem("TPM", "TPM"));
        ddlProdiEdit.Items.Add(new ListItem("MI", "MI"));
        ddlProdiEdit.Items.Add(new ListItem("PMO ", "PMO"));
        ddlProdiEdit.Items.Add(new ListItem("MK", "MK"));
        ddlProdiEdit.Items.Add(new ListItem("TKBG", "TKBG"));
        ddlProdiEdit.Items.Insert(0, new ListItem("--- Pilih Program Studi ---", ""));
        ddlPosisiEdit.Items.Add(new ListItem("Asisten Ahli", "Asisten Ahli"));
        ddlPosisiEdit.Items.Add(new ListItem("Lektor", "Lektor"));
        ddlPosisiEdit.Items.Add(new ListItem("Lektor Kepala", "Lektor Kepala"));
        ddlPosisiEdit.Items.Add(new ListItem("Profesor ", "Profesor "));
        ddlPosisiEdit.Items.Insert(0, new ListItem("--- Pilih Jabatan ---", ""));
        ddlPendidikanEdit.Items.Add(new ListItem("D-3", "D-3"));
        ddlPendidikanEdit.Items.Add(new ListItem("S-1", "S-1"));
        ddlPendidikanEdit.Items.Add(new ListItem("S-2", "S-2"));
        ddlPendidikanEdit.Items.Add(new ListItem("S-3", "S-3"));
        ddlPendidikanEdit.Items.Insert(0, new ListItem("--- Pilih Pendidikan Terakhir ---", ""));
    }

    protected void userReviewerDetil()
    {
        ddlKaryawanDt.DataSource = sso.CallProcedure("stp_getUser", new String[] { Context.User.Identity.Name });
        ddlKaryawanDt.DataValueField = "id";
        ddlKaryawanDt.DataTextField = "iduser";
        ddlKaryawanDt.DataBind();
        ddlKaryawanDt.Items.Insert(0, new ListItem("--- Pilih Karyawan ---", ""));
        ddlProdiDt.Items.Add(new ListItem("P4", "P4"));
        ddlProdiDt.Items.Add(new ListItem("TPM", "TPM"));
        ddlProdiDt.Items.Add(new ListItem("MI", "MI"));
        ddlProdiDt.Items.Add(new ListItem("PMO ", "PMO"));
        ddlProdiDt.Items.Add(new ListItem("MK", "MK"));
        ddlProdiDt.Items.Add(new ListItem("TKBG", "TKBG"));
        ddlProdiDt.Items.Insert(0, new ListItem("--- Pilih Program Studi ---", ""));
        ddlPosisiDt.Items.Add(new ListItem("-", "-"));
        ddlPosisiDt.Items.Add(new ListItem("Asisten Ahli", "Asisten Ahli"));
        ddlPosisiDt.Items.Add(new ListItem("Lektor", "Lektor"));
        ddlPosisiDt.Items.Add(new ListItem("Lektor Kepala", "Lektor Kepala"));
        ddlPosisiDt.Items.Add(new ListItem("Profesor ", "Profesor "));
        ddlPosisiDt.Items.Insert(0, new ListItem("--- Pilih Jabatan ---", ""));
    }

    protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            DateTime dtime;
            String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
            if (e.CommandName == "Ubah")
            {
                userReviewerEdit();
                dt = sso.CallProcedure("stp_detailDosen", new string[] { id });
                editKode.Text = id;

                dtime = Convert.ToDateTime(dt.Rows[0][7].ToString());
                //showAlertSuccess(id + dt.Rows[0][1].ToString());
                EditNIK.Text = dt.Rows[0][8].ToString();
                EditNama.Text = dt.Rows[0][1].ToString();
                ddlProdiEdit.SelectedValue = dt.Rows[0][2].ToString();
                ddlPendidikanEdit.SelectedValue = dt.Rows[0][3].ToString();
                ddlPosisiEdit.SelectedValue = dt.Rows[0][4].ToString();
                ddlKaryawanEdit.SelectedValue = dt.Rows[0][11].ToString();
                EditAlamat.Text = dt.Rows[0][5].ToString();
                EditTempatLahir.Text = dt.Rows[0][6].ToString();
                EditTanggalLahir.Text = dtime.ToString("yyyy-MM-dd");
                EditTelepon.Text = dt.Rows[0][9].ToString();
                EditEmail.Text = dt.Rows[0][10].ToString();

                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;
            }
            else if (e.CommandName == "Detil")
            {
                userReviewerDetil();
                dt = sso.CallProcedure("stp_detailDosen", new string[] { id });
                editKode.Text = id;

                dtime = Convert.ToDateTime(dt.Rows[0][7].ToString());
                //showAlertSuccess(id + dt.Rows[0][1].ToString());
                DetilNIK.Text = dt.Rows[0][8].ToString();
                DetilNama.Text = dt.Rows[0][1].ToString();
                ddlProdiDt.SelectedValue = dt.Rows[0][2].ToString();
                DetilPendidikan.Text = dt.Rows[0][3].ToString();
                ddlPosisiDt.SelectedValue = dt.Rows[0][4].ToString();
                ddlKaryawanDt.SelectedValue = dt.Rows[0][11].ToString();
                DetilAlamat.Text = dt.Rows[0][5].ToString();
                DetilTempatLahir.Text = dt.Rows[0][6].ToString();
                DetilTanggalLahir.Text = dtime.ToString("yyyy-MM-dd");
                DetilTelepon.Text = dt.Rows[0][9].ToString();
                DetilEmail.Text = dt.Rows[0][10].ToString();
                DetilCreaby.Text = dt.Rows[0][12].ToString();
                DetilCreadate.Text = dt.Rows[0][13].ToString();
                DetilUpdBy.Text = dt.Rows[0][14].ToString();
                DetilUptdate.Text = dt.Rows[0][15].ToString();

                panelAdd.Visible = false;
                panelEdit.Visible = false;
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
    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
        try
        {
            String[] param = new String[12];
            //param[0] = editKode.Text;
            param[0] = Server.HtmlEncode(AddNama.Text.Trim());
            param[1] = Server.HtmlEncode(ddlProdi.SelectedValue.Trim());
            param[2] = Server.HtmlEncode(ddlPendidikan.SelectedValue.Trim());
            param[3] = Server.HtmlEncode(ddlPosisi.SelectedValue.Trim());
            param[4] = Server.HtmlEncode(AddAlamat.Text.Trim());
            param[5] = Server.HtmlEncode(AddTempatLahir.Text.Trim());
            param[6] = Server.HtmlEncode(AddTanggalLahir.Text.Trim());
            param[7] = Server.HtmlEncode(AddNIK.Text.Trim());
            param[8] = Server.HtmlEncode(AddTelepon.Text.Trim());
            param[9] = Server.HtmlEncode(AddEmail.Text.Trim());
            param[10] = Server.HtmlEncode(ddlKaryawan.SelectedValue.Trim());
            param[11] = Context.User.Identity.Name;

            try
            {
                sso.CallProcedure("stp_createDosen", param);
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
    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
    }   

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        sso.CallProcedure("stp_deleteDosen", new string[] { txtConfirmDelete.Text });
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

    protected void btnCancelEdit_Click1(object sender, EventArgs e)
    {

    }



    protected void btnSubmitEdit_Click(object sender, EventArgs e)
    {
        try
        {

            String[] param = new String[13];
            param[0] = editKode.Text;
            param[1] = Server.HtmlEncode(EditNama.Text.Trim());
            param[2] = Server.HtmlEncode(ddlProdiEdit.SelectedValue.Trim());
            param[3] = Server.HtmlEncode(ddlPendidikanEdit.SelectedValue.Trim());
            param[4] = Server.HtmlEncode(ddlPosisiEdit.SelectedValue.Trim());
            param[5] = Server.HtmlEncode(EditAlamat.Text.Trim());
            param[6] = Server.HtmlEncode(EditTempatLahir.Text.Trim());
            param[7] = Server.HtmlEncode(EditTanggalLahir.Text.Trim());
            param[8] = Server.HtmlEncode(EditNIK.Text.Trim());
            param[9] = Server.HtmlEncode(EditTelepon.Text.Trim());
            param[10] = Server.HtmlEncode(EditEmail.Text.Trim());
            param[11] = Server.HtmlEncode(ddlKaryawanEdit.SelectedValue.Trim());
            param[12] = Context.User.Identity.Name;

            sso.CallProcedure("stp_editDosen", param);
            btnCancelEdit_Click(sender, e);
            loadData();
            showAlertSuccess("Ubah karyawan berhasil");
            //showAlertSuccess(param[0] + " " +param[1] + " " + param[2] + " " + param[3]
            //    + " " + param[4] + " " + param[5] + " " + param[6] + " " + param[7] + " " + param[8]
            //    + " " + param[9] + " " + param[10] + " " + param[11] + " " + param[12]);
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
        }
    }

    protected void btnDetilCancel_Click(object sender, EventArgs e)
    {
        panelAdd.Visible = false;
        panelData.Visible = true;
        panelDetil.Visible = false;
        panelDetil.Visible = false;
    }
}
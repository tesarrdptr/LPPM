using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;


    public partial class Page_Master_Akun : System.Web.UI.Page
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
            gridData.DataSource = lib.CallProcedure("stp_getDataUser", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
            gridData.DataBind();
            hideAlert();
        }

        protected void linkTambah_Click(object sender, EventArgs e)
        {
            addJabatan.DataSource = lib.CallProcedure("stp_getListRole", new string[] { });
            addJabatan.DataValueField = "idrole";
            addJabatan.DataTextField = "deskripsi";
            addJabatan.DataBind();
            addJabatan.Items.Insert(0, new ListItem("--- Pilih Jabatan ---", ""));
            addBagian.DataSource = lib.CallProcedure("stp_getListArea", new string[] { });
            addBagian.DataValueField = "idarea";
            addBagian.DataTextField = "deskripsi";
            addBagian.DataBind();
            addBagian.Items.Insert(0, new ListItem("--- Pilih Bagian ---", ""));
            addAplikasi.DataSource = lib.CallProcedure("stp_getListApplication", new string[] { });
            addAplikasi.DataValueField = "idapplication";
            addAplikasi.DataTextField = "deskripsi";
            addAplikasi.DataBind();
            addAplikasi.Items.Insert(0, new ListItem("--- Pilih Aplikasi ---", ""));
            panelData.Visible = false;
            panelAdd.Visible = true;
            panelEdit.Visible = false;
        }

        protected void btnCari_Click(object sender, EventArgs e)
        {
            loadData();
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
                lib.CallProcedure("stp_createUser", new string[] { Server.HtmlEncode(addNamaAkun.Text.Trim()), addJabatan.SelectedValue, addBagian.SelectedValue, addAplikasi.SelectedValue, (addStatus.Checked ? "1" : "0"), Context.User.Identity.Name });
                btnCancelAdd_Click(sender, e);
                loadData();
                showAlertSuccess("Tambah akun berhasil");
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
                lib.CallProcedure("stp_editUser", new string[] { editKode.Text, editJabatan.SelectedValue, editBagian.SelectedValue, editAplikasi.SelectedValue, (editStatus.Checked ? "1" : "0"), Context.User.Identity.Name });
                btnCancelEdit_Click(sender, e);
                loadData();
                showAlertSuccess("Ubah akun berhasil");
            }
            catch (Exception ex)
            {
                showAlertDanger(ex.Message + " " + ex.StackTrace);
            }
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
                    editJabatan.DataSource = lib.CallProcedure("stp_getListRole", new string[] { });
                    editJabatan.DataValueField = "idrole";
                    editJabatan.DataTextField = "deskripsi";
                    editJabatan.DataBind();
                    editJabatan.Items.Insert(0, new ListItem("--- Pilih Jabatan ---", ""));
                    editBagian.DataSource = lib.CallProcedure("stp_getListArea", new string[] { });
                    editBagian.DataValueField = "idarea";
                    editBagian.DataTextField = "deskripsi";
                    editBagian.DataBind();
                    editBagian.Items.Insert(0, new ListItem("--- Pilih Bagian ---", ""));
                    editAplikasi.DataSource = lib.CallProcedure("stp_getListApplication", new string[] { });
                    editAplikasi.DataValueField = "idapplication";
                    editAplikasi.DataTextField = "deskripsi";
                    editAplikasi.DataBind();
                    editAplikasi.Items.Insert(0, new ListItem("--- Pilih Aplikasi ---", ""));
                    dt = lib.CallProcedure("stp_detailUser", new String[] { id });
                    editKode.Text = dt.Rows[0][0].ToString();
                    editNamaAkun.Text = dt.Rows[0][1].ToString();
                    editJabatan.SelectedValue = dt.Rows[0][2].ToString();
                    editBagian.SelectedValue = dt.Rows[0][3].ToString();
                    editAplikasi.SelectedValue = dt.Rows[0][4].ToString();
                    editStatus.Checked = (dt.Rows[0][5].ToString().Equals("1") ? true : false);
                    panelAdd.Visible = false;
                    panelEdit.Visible = true;
                    panelData.Visible = false;
                }
            }
        }

        protected void linkConfirmDelete_Click(object sender, EventArgs e)
        {
            lib.CallProcedure("stp_deleteUser", new string[] { txtConfirmDelete.Text });
            loadData();
            showAlertSuccess("Hapus akun berhasil");
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
    }
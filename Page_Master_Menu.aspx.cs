using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;


    public partial class Page_Master_Menu : System.Web.UI.Page
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
                cariAplikasi.DataSource = lib.CallProcedure("stp_getListApplication", new string[] { });
                cariAplikasi.DataValueField = "idapplication";
                cariAplikasi.DataTextField = "deskripsi";
                cariAplikasi.DataBind();
                cariAplikasi.Items.Insert(0, new ListItem("--- Pilih Aplikasi ---", ""));
                cariJabatan.DataSource = lib.CallProcedure("stp_getListRole", new string[] { });
                cariJabatan.DataValueField = "idRole";
                cariJabatan.DataTextField = "deskripsi";
                cariJabatan.DataBind();
                cariJabatan.Items.Insert(0, new ListItem("--- Pilih Jabatan ---", ""));
            }
            loadData();
            hideAlert();
            gridData.Width = Unit.Percentage(100);
            gridDetail.Width = Unit.Percentage(100);
        }

        protected void loadData()
        {
            gridData.DataSource = lib.CallProcedure("stp_getDataMenu", new string[] { cariAplikasi.SelectedValue, cariJabatan.SelectedValue });
            gridData.DataBind();
            try
            {
                ((LinkButton)gridData.Rows[0].FindControl("linkUp")).Visible = false;
                ((LinkButton)gridData.Rows[gridData.Rows.Count - 1].FindControl("linkDown")).Visible = false;
            }
            catch { }
            hideAlert();
        }

        protected void loadDataDetail(string idmenu)
        {
            gridDetail.DataSource = lib.CallProcedure("stp_getDataSubMenu", new string[] { idmenu });
            gridDetail.DataBind();
            try
            {
                ((LinkButton)gridDetail.Rows[0].FindControl("linkUp")).Visible = false;
                ((LinkButton)gridDetail.Rows[gridDetail.Rows.Count - 1].FindControl("linkDown")).Visible = false;
            }
            catch { }
            hideAlert();
        }

        protected void linkTambah_Click(object sender, EventArgs e)
        {
            addJabatan.DataSource = lib.CallProcedure("stp_getListRole", new string[] { });
            addJabatan.DataValueField = "idrole";
            addJabatan.DataTextField = "deskripsi";
            addJabatan.DataBind();
            addJabatan.Items.Insert(0, new ListItem("--- Pilih Jabatan ---", ""));
            addAplikasi.DataSource = lib.CallProcedure("stp_getListApplication", new string[] { });
            addAplikasi.DataValueField = "idapplication";
            addAplikasi.DataTextField = "deskripsi";
            addAplikasi.DataBind();
            addAplikasi.Items.Insert(0, new ListItem("--- Pilih Aplikasi ---", ""));
            panelData.Visible = false;
            panelAdd.Visible = true;
            panelEdit.Visible = false;
            panelDetail.Visible = false;
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
            panelDetail.Visible = false;
        }

        protected void btnSubmitAdd_Click(object sender, EventArgs e)
        {
            try
            {
                lib.CallProcedure("stp_createMenu", new string[] { addAplikasi.SelectedValue, Server.HtmlEncode(addNamaMenu.Text.Trim()), (Server.HtmlEncode(addTautan.Text.Trim()).Equals("") ? "#" : Server.HtmlEncode(addTautan.Text.Trim())), (addStatus.Checked ? "1" : "0"), addJabatan.SelectedValue, Context.User.Identity.Name });
                btnCancelAdd_Click(sender, e);
                loadData();
                showAlertSuccess("Tambah menu berhasil");
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
            panelDetail.Visible = false;
        }

        protected void btnSubmitEdit_Click(object sender, EventArgs e)
        {
            try
            {
                lib.CallProcedure("stp_editMenu", new string[] { editKode.Text, Server.HtmlEncode(editNamaMenu.Text.Trim()), (Server.HtmlEncode(editTautan.Text.Trim()).Equals("") ? "#" : Server.HtmlEncode(editTautan.Text.Trim())), editAplikasi.SelectedValue, editJabatan.SelectedValue, (editStatus.Checked ? "1" : "0"), Context.User.Identity.Name });
                btnCancelEdit_Click(sender, e);
                loadData();
                showAlertSuccess("Ubah menu berhasil");
            }
            catch (Exception ex)
            {
                showAlertDanger(ex.Message + " " + ex.StackTrace);
            }
        }

        protected void btnCancelDetail_Click(object sender, EventArgs e)
        {
            panelData.Visible = true;
            panelAdd.Visible = false;
            panelEdit.Visible = false;
            panelDetail.Visible = false;
        }

        protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
                if (e.CommandName == "Atas")
                {
                    lib.CallProcedure("stp_setOrderMenu", new String[] { id, "Up" });
                    loadData();
                    showAlertSuccess("Urutan menu berhasil diubah");
                }
                else if (e.CommandName == "Bawah")
                {
                    lib.CallProcedure("stp_setOrderMenu", new String[] { id, "Down" });
                    loadData();
                    showAlertSuccess("Urutan menu berhasil diubah");
                }
                else if (e.CommandName == "Detail")
                {
                    detailKode.Text = id;
                    detailMenu.Text = gridData.Rows[Convert.ToInt32(e.CommandArgument.ToString())].Cells[0].Text;
                    detailAplikasi.Text = cariAplikasi.SelectedItem.Text;
                    detailJabatan.Text = cariJabatan.SelectedItem.Text;
                    loadDataDetail(id);
                    panelAdd.Visible = false;
                    panelEdit.Visible = false;
                    panelData.Visible = false;
                    panelDetail.Visible = true;
                }
                else if (e.CommandName == "Ubah")
                {
                    editJabatan.DataSource = lib.CallProcedure("stp_getListRole", new string[] { });
                    editJabatan.DataValueField = "idrole";
                    editJabatan.DataTextField = "deskripsi";
                    editJabatan.DataBind();
                    editJabatan.Items.Insert(0, new ListItem("--- Pilih Jabatan ---", ""));
                    editAplikasi.DataSource = lib.CallProcedure("stp_getListApplication", new string[] { });
                    editAplikasi.DataValueField = "idapplication";
                    editAplikasi.DataTextField = "deskripsi";
                    editAplikasi.DataBind();
                    editAplikasi.Items.Insert(0, new ListItem("--- Pilih Aplikasi ---", ""));
                    dt = lib.CallProcedure("stp_detailMenu", new String[] { id });
                    editKode.Text = dt.Rows[0][0].ToString();
                    editNamaMenu.Text = dt.Rows[0][1].ToString();
                    editTautan.Text = dt.Rows[0][2].ToString();
                    editAplikasi.SelectedValue = dt.Rows[0][3].ToString();
                    editJabatan.SelectedValue = dt.Rows[0][4].ToString();
                    editStatus.Checked = (dt.Rows[0][5].ToString().Equals("1") ? true : false);
                    panelAdd.Visible = false;
                    panelEdit.Visible = true;
                    panelData.Visible = false;
                    panelDetail.Visible = false;
                }
            }
        }

        protected void gridDetail_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName != "Page")
            {
                String id = gridDetail.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
                if (e.CommandName == "Atas")
                {
                    lib.CallProcedure("stp_setOrderSubMenu", new String[] { id, "Up" });
                    loadDataDetail(detailKode.Text);
                    showAlertSuccess("Urutan submenu berhasil diubah");
                }
                else if (e.CommandName == "Bawah")
                {
                    lib.CallProcedure("stp_setOrderSubMenu", new String[] { id, "Down" });
                    loadDataDetail(detailKode.Text);
                    showAlertSuccess("Urutan submenu berhasil diubah");
                }
            }
        }

        protected void linkConfirmDelete_Click(object sender, EventArgs e)
        {
            lib.CallProcedure("stp_deleteMenu", new string[] { txtConfirmDelete.Text });
            loadData();
            showAlertSuccess("Hapus menu berhasil");
        }

        protected void linkConfirmDeleteDetail_Click(object sender, EventArgs e)
        {
            lib.CallProcedure("stp_deleteSubMenu", new string[] { txtConfirmDeleteDetail.Text });
            loadDataDetail(detailKode.Text);
            showAlertSuccess("Hapus submenu berhasil");
        }

        protected void linkConfirmAddDetail_Click(object sender, EventArgs e)
        {
            try
            {
                lib.CallProcedure("stp_createSubMenu", new string[] { detailKode.Text, Server.HtmlEncode(addDetailNamaSubMenu.Text.Trim()), Server.HtmlEncode(addDetailTautan.Text.Trim()), (addDetailStatus.Checked ? "1" : "0"), Context.User.Identity.Name });
                loadDataDetail(detailKode.Text);
                showAlertSuccess("Tambah submenu berhasil");
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

        protected void hideAlert()
        {
            divAlert.Visible = false;
            divSuccess.Visible = false;
        }
    }
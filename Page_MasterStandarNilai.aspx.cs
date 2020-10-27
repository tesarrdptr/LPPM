using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;


    public partial class Page_MasterStandarNilai : System.Web.UI.Page
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
            gridData.DataSource = lib.CallProcedure("stp_getDataStandarNilai", new string[] { });
            gridData.DataBind();
            hideAlert();
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
                lib.CallProcedure("stp_createStandarNilai", new string[] { Server.HtmlEncode(AddJenisPub.Text.Trim()), Context.User.Identity.Name });
                btnCancelAdd_Click(sender, e);
                loadData();
                showAlertSuccess("Tambah Standar berhasil");
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
                lib.CallProcedure("stp_editStandarNilai", new string[] { editKode.Text, editPub.Text, Context.User.Identity.Name });
                btnCancelEdit_Click(sender, e);
                loadData();
                showAlertSuccess("Ubah standar nilai berhasil");
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
                    dt = lib.CallProcedure("stp_detailStandarNilai", new string[] { id });
                    editKode.Text = id;
                    editPub.Text = dt.Rows[0][1].ToString();
                    panelAdd.Visible = false;
                    panelEdit.Visible = true;
                    panelData.Visible = false;
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
    }
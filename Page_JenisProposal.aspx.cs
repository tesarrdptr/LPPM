using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;


    public partial class Page_JenisProposal : System.Web.UI.Page
    {
        SiteMaster master;
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
                    if (!Context.User.Identity.IsAuthenticated || !ticket.UserData.Split('@')[1].Equals("APP04") || master.sso.CallProcedure("stp_getMenuByRole", new string[] { ticket.UserData.Split('@')[0], ticket.UserData.Split('@')[1], Request.Path.Split('/')[Request.Path.Split('/').Length - 1].Replace(".aspx", "") }).Rows.Count == 0)
                    {
                        Response.Redirect("http://application.polman.astra.ac.id/sso");
                    }
                }
                catch
                {
                    Response.Redirect("http://application.polman.astra.ac.id/sso");
                }
            }
            loadData();
            gridData.Width = Unit.Percentage(100);
        }

        protected void loadData()
        {
            gridData.DataSource = master.lib.CallProcedure("stp_getJenisProposal", new String[] { });
            gridData.DataBind();
            master.hideAlert();         
        }

        protected void linkTambah_Click(object sender, EventArgs e)
        {
            master.hideAlert();
            panelData.Visible = false;
            panelAdd.Visible = true;
            panelEdit.Visible = false;
        }

        protected void btnCancelAdd_Click(object sender, EventArgs e)
        {
            master.hideAlert();
            panelData.Visible = true;
            panelAdd.Visible = false;
            panelEdit.Visible = false;
            addJenisName.Text = "";
        }

        protected void btnSubmitAdd_Click(object sender, EventArgs e)
        {
            try
            {
                var dt = master.lib.CallProcedure("stp_addJenisProposal", new string[] { Server.HtmlEncode(addJenisName.Text.Trim()) });
                master.showAlertSuccess(dt.Rows[0][0].ToString());
                btnCancelAdd_Click(sender, e);
                loadData();
            }
            catch (Exception ex)
            {
                master.showAlertDanger(ex.Message + " " + ex.StackTrace);
            }
        }

        protected void btnCancelEdit_Click(object sender, EventArgs e)
        {
            master.hideAlert();
            panelData.Visible = true;
            panelAdd.Visible = false;
            panelEdit.Visible = false;
            editJenisName.Text = "";
        }

        protected void btnSubmitEdit_Click(object sender, EventArgs e)
        {
            try
            {
                master.lib.CallProcedure("stp_editJenisProposal", new string[] { editJenisID.Text, Server.HtmlEncode(editJenisName.Text.Trim()),Context.User.Identity.Name });
                btnCancelEdit_Click(sender, e);
                loadData();
            }
            catch (Exception ex)
            {
                master.showAlertDanger(ex.Message + " " + ex.StackTrace);
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
                    dt = master.lib.CallProcedure("stp_getJenisProposalByID", new String[] { id });
                    editJenisID.Text = dt.Rows[0][0].ToString();
                    editJenisName.Text = dt.Rows[0][1].ToString();
                    panelAdd.Visible = false;
                    panelEdit.Visible = true;
                    panelData.Visible = false;
                }
            }
        }

        protected void linkConfirmDelete_Click(object sender, EventArgs e)
        {
            master.lib.CallProcedure("stp_deleteJenisProposal", new string[] { txtConfirmDelete.Text, Context.User.Identity.Name });
            loadData();
            master.showAlertSuccess("Hapus Jenis Proposal berhasil");
        }

    }

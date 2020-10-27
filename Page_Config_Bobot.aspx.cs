using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;

    public partial class Page_Config_Bobot : System.Web.UI.Page
    {
        SiteMaster master;
        DataTable dt = new DataTable();
        int count = 0;

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
            count = Convert.ToInt32(master.lib.CallProcedure("stp_getBobotTotal", new String[] { }).Rows[0][0].ToString());
            gridData.DataSource = master.lib.CallProcedure("stp_getBobots", new String[] { });
            gridData.DataBind();
            master.hideAlert();
            //if (count < 100) master.showAlertDanger("Total persentase bobot kurang dari 100%");
            //else 
            if (count > 100) master.showAlertDanger("Total persentase bobot lebih dari 100%");
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
            addBobotName.Text = "";
            addBobotPersentase.Text = "";
            loadData();
        }

        protected void btnSubmitAdd_Click(object sender, EventArgs e)
        {
            try
            {
                int total = Convert.ToInt32(addBobotPersentase.Text.Trim()) + count;
                if(total > 100) master.showAlertDanger("Tambah bobot gagal, Bobot melebihi 100%");
                else
                {
                    master.lib.CallProcedure("stp_addBobot", new string[] { Server.HtmlEncode(addBobotName.Text.Trim()), Server.HtmlEncode(addBobotPersentase.Text.Trim()), Context.User.Identity.Name });
                    btnCancelAdd_Click(sender, e);
                    loadData();
                    master.showAlertSuccess("Tambah bobot berhasil");
                }
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
            editBobotName.Text = "";
            editBobotPercent.Text = "";
            loadData();
        }

        protected void btnSubmitEdit_Click(object sender, EventArgs e)
        {
            try
            {
                int total = Convert.ToInt32(editBobotPercent.Text) + count - Convert.ToInt32(editBobotPercent.Attributes["origin"]);
                if(total > 100)
                {
                    master.showAlertDanger("Ubah bobot gagal, bobot melebihi 100%");
                }
                else
                {
                    master.lib.CallProcedure("stp_editBobot", new string[] { editBobotID.Text, Server.HtmlEncode(editBobotName.Text.Trim()), Server.HtmlEncode(editBobotPercent.Text), Context.User.Identity.Name });
                    btnCancelEdit_Click(sender, e);
                    loadData();
                    master.showAlertSuccess("Ubah bobot berhasil");
                }
                
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
                    dt = master.lib.CallProcedure("stp_getBobotByID", new String[] { id });
                    editBobotID.Text = dt.Rows[0][0].ToString();
                    editBobotName.Text = dt.Rows[0][1].ToString();
                    editBobotPercent.Text = dt.Rows[0][2].ToString();
                    editBobotPercent.Attributes.Add("origin", editBobotPercent.Text);
                    panelAdd.Visible = false;
                    panelEdit.Visible = true;
                    panelData.Visible = false;
                }
            }
        }

        protected void linkConfirmDelete_Click(object sender, EventArgs e)
        {
            master.lib.CallProcedure("stp_deleteBobot", new string[] { txtConfirmDelete.Text, Context.User.Identity.Name });
            loadData();
            master.showAlertSuccess("Hapus bobot berhasil");
        }

    }
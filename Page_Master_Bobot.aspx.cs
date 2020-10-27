using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI.WebControls;


public partial class Page_Master_Bobot : System.Web.UI.Page
{
    PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    public int _tmp;

    public int Tmp
    {
        get
        {
            return _tmp;
        }
        set
        {
            this._tmp = value;
        }
    }


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
        gridData.DataSource = lib.CallProcedure("stp_getDataBobot", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
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
        editBobotPercentBaru.Text = "";
    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
        int x = Convert.ToInt32(lib.CallProcedure("stp_checkBobotPercent", new String[] { }).Rows[0][0].ToString());

        if (AddBobotTitle.Text != "" && AddBobotPercent.Text != "")
        {
            try
            {
                int y = Convert.ToInt32(AddBobotPercent.Text);
                int z = y + x;

                /*if (z != 100)
                {
                    showAlertDanger("Tambah bobot gagal, bobot yang anda masukan menjadi " + z + " .Total bobot harus 100");
                    btnCancelAdd_Click(sender, e);
                }
                else */
                if (z > 100)
                {
                    showAlertDanger("Tambah bobot gagal, persentase tidak boleh lebih dari 100 dan maksimal bobot harus 100");
                    btnCancelAdd_Click(sender, e);
                }
                else
                {
                    lib.CallProcedure("stp_createBobot", new string[] { Server.HtmlEncode(AddBobotTitle.Text.Trim()), AddBobotPercent.Text, Context.User.Identity.Name });
                    btnCancelAdd_Click(sender, e);
                    loadData();
                    if (z < 100)
                    {
                        showAlertDanger("Bobot yang anda masukan menjadi " + z + " .Total bobot harus 100");
                        showAlertSuccess("Tambah bobot berhasil");
                    }
                    else
                    {
                        showAlertSuccess("Tambah bobot berhasil");
                    }

                }
                //if (x == 100)
                //{
                //    lib.CallProcedure("stp_createBobot", new string[] { Server.HtmlEncode(AddBobotTitle.Text.Trim()), AddBobotPercent.Text, Context.User.Identity.Name });
                //    btnCancelAdd_Click(sender, e);
                //    loadData();
                //    showAlertSuccess("Tambah bobot berhasil");
                //}
                //else
                //{
                //    loadData();
                //    showAlertSuccess("Total bobot harus 100");
                //}
            }
            catch
            {
                showAlertDanger("Presentase tidak valid, tolong masukan angka");
            }
        }
        else
        {
            showAlertDanger("Textbox harus diisi semua !");
        }
    }

    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
        editBobotPercentBaru.Text = "";
    }

    protected void btnSubmitEdit_Click(object sender, EventArgs e)
    {

        int x = Convert.ToInt32(lib.CallProcedure("stp_checkBobotPercent", new String[] { }).Rows[0][0].ToString());

        try
        {
            int c = Convert.ToInt32(editBobotPercentBaru.Text);
            int y = Convert.ToInt32(editBobotPercent.Text);
            int z = x - y + c;
            btnCancelEdit_Click(sender, e);
            /*if (z != 100)
            {
                showAlertDanger("Edit Bobot Gagal, Total bobot harus 100");
                btnCancelEdit_Click(sender, e);
            }*/
            if (c == y)
            {
                btnCancelEdit_Click(sender, e);
            }
            else
            {
                lib.CallProcedure("stp_editBobot", new string[] { editKode.Text, editBobotTitle.Text, c.ToString(), Context.User.Identity.Name });
                btnCancelEdit_Click(sender, e);
                loadData();
                showAlertSuccess("Ubah bobot berhasil");
            }
        }
        catch
        {
            showAlertDanger("Presentase tidak valid, tolong masukan angka");
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
                dt = lib.CallProcedure("stp_detailBobot", new string[] { id });
                editKode.Text = id;
                editBobotTitle.Text = dt.Rows[0][1].ToString();
                editBobotPercent.Text = dt.Rows[0][2].ToString();
                Tmp = Convert.ToInt32(dt.Rows[0][2].ToString());
                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;
                panelDetil.Visible = false;
            }
            else if (e.CommandName == "Detil")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                string tempid = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();

                dt = lib.CallProcedure("stp_detailBobot", new string[] { id });
                editKode.Text = id;

                TextBox1.Text = dt.Rows[0][1].ToString();
                TextBox2.Text = dt.Rows[0][2].ToString();
                Tmp = Convert.ToInt32(dt.Rows[0][2].ToString());
                TextBox4.Text = dt.Rows[0][3].ToString();
                TextBox5.Text = dt.Rows[0][4].ToString();
                TextBox6.Text = dt.Rows[0][5].ToString();
                TextBox7.Text = dt.Rows[0][6].ToString();
                panelAdd.Visible = false;
                panelData.Visible = false;
                panelDetil.Visible = true;
            }
        }
    }


    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib.CallProcedure("stp_deleteBobot", new string[] { txtConfirmDelete.Text });
        loadData();

        int x = Convert.ToInt32(lib.CallProcedure("stp_checkBobotPercent", new String[] { }).Rows[0][0].ToString());
        if (x != 100)
        {
            showAlertSuccess("Hapus bobot berhasil");
            showAlertDanger("Bobot kurang dari 100, bobot harus 100");
        }
        else if (x == 100)
        {
            showAlertSuccess("Hapus bobot berhasil");
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
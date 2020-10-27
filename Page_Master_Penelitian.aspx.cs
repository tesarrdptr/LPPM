using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.IO;
using System.Text;
using System.Security.Cryptography;
using ProposalPolmanAstra.Classes;
using System.Web.Configuration;

public partial class Page_Master_Penelitian : System.Web.UI.Page
{
    PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    SiteMaster master;
    System.Data.SqlClient.SqlConnection connection =
     new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);
    protected void Page_Load(object sender, EventArgs e)
    {
        master = this.Master as SiteMaster;
        if (!IsPostBack)
        {
            userPenelitian();
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
            int year = DateTime.Now.Year;
            for (int i = year - 10; i <= year + 10; i++)
            {
                ListItem li = new ListItem(i.ToString());
                tglAnggaran.Items.Add(li);
                //ddlAnggaranEdit.Items.Add(li);
            }
            tglAnggaran.Items.FindByText(year.ToString()).Selected = true;

        }
        loadData();
        hideAlert();
        gridData.Width = Unit.Percentage(100);
    }

    protected void userPenelitian()
    {
        ddlAddSumDana.Items.Add(new ListItem("--- Pilih Sumber Dana ---", ""));
        ddlAddSumDana.Items.Add(new ListItem("Polman", "Polman"));
        ddlAddSumDana.Items.Add(new ListItem("Non-Polman", "Non-Polman"));
        AddKetua.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        AddKetua.DataValueField = "iduser";
        AddKetua.DataTextField = "iduser";
        AddKetua.DataBind();
        AddKetua.Items.Insert(0, new ListItem("--- Pilih Ketua ---", ""));
        addAnggota.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        addAnggota.DataValueField = "iduser";
        addAnggota.DataTextField = "iduser";
        addAnggota.DataBind();
        addAnggota.Items.Insert(0, new ListItem("--- Pilih Anggota ---", ""));
    }

    

    protected void loadData()
    {
        gridData.DataSource = lib.CallProcedure("stp_getHistoryPP", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
        gridData.DataBind();
        hideAlert();
    }

    protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
        if (e.CommandName == "download")
        {
            string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand com = new SqlCommand("select * from  TRATTACHMENT where atc_id=@id", con);
            com.Parameters.AddWithValue("id", e.CommandArgument.ToString());
            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                string fileName = dr["atc_name"].ToString();
                string fileLength = dr["atc_size"].ToString();
                string filePath = dr["atc_path"].ToString();
                string fileExtension = dr["atc_contentType"].ToString();
                if (File.Exists(filePath))
                {
                    Response.Clear();
                    Response.BufferOutput = false;
                    Response.ContentType = dr["atc_contentType"].ToString();
                    Response.AddHeader("Content-Length", fileLength);
                    Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                    Response.TransmitFile(filePath);
                    Response.Flush();
                }
                else
                {
                    showAlertDanger("Gagal: File tidak ditemukan!");
                }
            }
        }
        else if (e.CommandName == "Ubah")
        {
            String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
            panelData.Visible = false;
            panelAdd.Visible = false;
            panelDetil.Visible = true;

            int ind = Convert.ToInt32(e.CommandArgument);
            connection.Open();
            SqlCommand sqlCmd = new SqlCommand("stp_detailPP", connection);
            sqlCmd.Parameters.AddWithValue("@p1", id);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            //showAlertSuccess(id);
            while (dr.Read())
            {
                dtJudul.Text = dr["pp_judul"].ToString();
                dtKetua.Text = dr["pp_ketua"].ToString();
                dtAnggota.Text = dr["pp_anggota"].ToString();
                dtSumDana.Text = dr["pp_sumber_dana"].ToString();
                dtDana.Text = dr["pp_totaldana"].ToString();
                dtTglMulai.Text = dr["pp_tgl_mulai"].ToString();
                dtTglSelesai.Text = dr["pp_tgl_selesai"].ToString();
                dtThnAnggaran.Text = dr["pp_tahun_anggaran"].ToString();
                creaby.Text = dr["pp_creaby"].ToString();
                creadate.Text = dr["pp_creadate"].ToString();
                dtJenis.Text = dr["pp_jenis"].ToString();

            }
            connection.Close();
        }
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
        panelDetil.Visible = false;
    }

    protected void btnCari_Click(object sender, EventArgs e)
    {

    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        panelAdd.Visible = false;
        panelData.Visible = true;
        panelDetil.Visible = false;
        AddJudul.Text = "";
        AddKetua.SelectedValue = "";
        addAnggota.SelectedValue = "";
        ddlAddSumDana.SelectedValue = "";
        addDana.Text = "";
        addJenis.Text = "";
        tglMulai.Text = "";
        tglSelesai.Text = "";
    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
        string tempLinkBerkas = "";
        string saveDir = @"\Uploaded\";
        string appPath = Request.PhysicalApplicationPath;
        string fileSavePath = "";
        string extension = "";

        string fileExtension = Path.GetExtension(addUpload.PostedFile.FileName);
        if (addUpload.PostedFile != null && addUpload.PostedFile.ContentLength > 0 && fileExtension == ".pdf")
        {
            string fileName = Path.GetFileName(addUpload.PostedFile.FileName);
            string contenttype = String.Empty;

            //tempLinkBerkas = "Buku" + lib.Encrypt(addUpload.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + addUpload.FileName.Split('.')[addUpload.FileName.Split('.').Length - 1];

            extension = Path.GetExtension(addUpload.PostedFile.FileName).ToString();

            tempLinkBerkas = ddlAddSumDana.SelectedItem + "_" + lib.Encrypt(addUpload.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + addUpload.FileName.Split('.')[addUpload.FileName.Split('.').Length - 1];
            addUpload.SaveAs(appPath + saveDir + tempLinkBerkas);
            fileSavePath = Server.MapPath("Uploaded");
            fileSavePath = fileSavePath + "//" + tempLinkBerkas;

            FileInfo fileInfo = new FileInfo(fileSavePath);
            switch (fileExtension)
            {
                case ".doc":
                    contenttype = "application/vnd.ms-word";
                    break;
                case ".docx":
                    contenttype = "application/vnd.ms-word";
                    break;
                case ".pdf":
                    contenttype = "application/pdf";
                    break;
            }
            if (contenttype != String.Empty)
            {
                String[] param_atc = new String[6];
                param_atc[0] = tempLinkBerkas;
                param_atc[1] = addUpload.FileBytes.Length.ToString();
                param_atc[2] = fileSavePath;
                param_atc[3] = extension;
                var atc = lib.CallProcedure("stp_newAttachment", param_atc);

                String[] param = new String[12];
                param[0] = Server.HtmlEncode(AddJudul.Text.Trim());
                param[1] = Server.HtmlEncode(tglAnggaran.SelectedValue.Trim());
                param[2] = Server.HtmlEncode(tglMulai.Text.Trim());
                param[3] = Server.HtmlEncode(tglSelesai.Text.Trim());
                param[4] = Server.HtmlEncode(ddlAddSumDana.SelectedValue.Trim());
                param[5] = Server.HtmlEncode(addDana.Text.Trim());
                param[6] = Server.HtmlEncode(addJenis.Text.Trim());
                param[7] = Server.HtmlEncode(AddKetua.SelectedValue.Trim());
                param[8] = Server.HtmlEncode(addAnggota.SelectedValue.Trim());
                param[9] = Server.HtmlEncode("0");
                param[10] = Context.User.Identity.Name;
                param[11] = Server.HtmlEncode(atc.Rows[0][0].ToString());

                try
                {
                    lib.CallProcedure("stp_createHistoryPenelitian", param);
                    loadData();
                    showAlertSuccess("Tambah History Penelitian Berhasil.");
                    hideAlert();
                    btnCancelAdd_Click(sender, e);
                }
                catch (Exception ex)
                {
                    showAlertDanger(ex.Message + " " + ex.StackTrace);
                }

            }

            else
            {
                showAlertDanger("Format file tidak valid." +
             " Unggah file PDF.");
            }
        }
        else if (addUpload.PostedFile == null || addUpload.PostedFile.ContentLength == 0)
        {
            showAlertDanger("File belum diunggah, unggah file PDF.");
        }
        else if (addUpload.PostedFile != null && addUpload.PostedFile.ContentLength > 0 && fileExtension != ".pdf")
        {
            showAlertDanger("Format file tidak valid." +
             " Unggah file PDF.");
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

    protected void gridData_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortexpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortexpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortexpression, ASCENDING);
        }
    }
    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
            {
                ViewState["sortDirection"] = SortDirection.Ascending;

            }

            return (SortDirection)ViewState["sortDirection"];
        }
        set
        {
            ViewState["sortDirection"] = value;
        }
    }
    private void SortGridView(string sortExpression, string direction)
    {
        DataTable dt;

        dt = LoadData();



        DataView dv = new DataView(dt);
        dv.Sort = sortExpression + direction;
        gridData.DataSource = dv;
        gridData.DataBind();

    }

    public DataTable LoadData()
    {
        return lib.CallProcedure("stp_getHistoryPP", new String[] { Context.User.Identity.Name, Server.HtmlEncode(txtCari.Text.Trim()) });
    }

}
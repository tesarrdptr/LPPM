using System;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Configuration;
using System.Web.Security;
using System.IO;
using System.Web.UI.WebControls;

public partial class Page_Master_Dokumen_Monev : System.Web.UI.Page
{
    PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    System.Data.SqlClient.SqlConnection connection =
     new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddlAdd();
            editAdd();
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

    protected void ddlAdd()
    {
        ddlAddJenis.DataSource = lib.CallProcedure("stp_getJenisProposal", new String[] { Context.User.Identity.Name });
        ddlAddJenis.DataValueField = "jns_id";
        ddlAddJenis.DataTextField = "jns_title";
        ddlAddJenis.DataBind();
        ddlAddJenis.Items.Insert(0, new ListItem("--- Pilih Jenis ---", ""));
    }

    protected void editAdd()
    {
        ddlEditJenis.DataSource = lib.CallProcedure("stp_getJenisProposal", new String[] { Context.User.Identity.Name });
        ddlEditJenis.DataValueField = "jns_id";
        ddlEditJenis.DataTextField = "jns_title";
        ddlEditJenis.DataBind();
        ddlEditJenis.Items.Insert(0, new ListItem("--- Pilih Jenis ---", ""));
    }

    protected void loadData()
    {
        if (txtCari.Text != "")
        {
            tglMulaiCari.Text = null;
            tglSelesaiCari.Text = null;
            //query = "SELECT ROW_NUMBER() over (order by bidfok_created_date desc) as no, bidfok_name, CONVERT(varchar(12), bidfok_created_date, 113) as bidfok_created_date, CONVERT(varchar(12), bidfok_updated_date, 113) as bidfok_updated_date, bidfok_created_by, bidfok_updated_by from  MSBIDANGFOKUS";

        }
        if (tglMulaiCari.Text == String.Empty || tglSelesaiCari.Text == String.Empty)
        {
            gridData.DataSource = lib.CallProcedure("stp_getDokumenMonev", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
            gridData.DataBind();
        }
        else
        {
            gridData.DataSource = lib.CallProcedure("stp_searchdokumenmonevbydate", new string[] { Server.HtmlEncode(tglMulaiCari.Text), Server.HtmlEncode(tglSelesaiCari.Text) });
            gridData.DataBind();
        }
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
    }

    protected void btnCari_Click(object sender, EventArgs e)
    {

    }

    protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            
            if (e.CommandName == "download")
            {
                string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                SqlCommand com = new SqlCommand("select * from  MSDOKUMENMONEV where monevdoc_id=@id", con);
                com.Parameters.AddWithValue("id", e.CommandArgument.ToString());
                showAlertSuccess(e.CommandArgument.ToString());
                SqlDataReader dr = com.ExecuteReader();

                if (dr.Read())
                {
                    string fileName = dr["monevdoc_nama"].ToString();
                    string fileLength = dr["monevdoc_ukuran"].ToString();
                    string filePath = dr["monevdoc_path"].ToString();
                    string fileExtension = dr["monevdoc_ekstensi"].ToString();
                    if (File.Exists(filePath))
                    {
                        Response.Clear();
                        Response.BufferOutput = false;
                        Response.ContentType = dr["monevdoc_ekstensi"].ToString();
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
                panelEdit.Visible = true;
                int ind = Convert.ToInt32(e.CommandArgument);
                connection.Open();
                SqlCommand sqlCmd = new SqlCommand("stp_detailDokumenMonev", connection);
                sqlCmd.Parameters.AddWithValue("@p1", id);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
                
                while (dr.Read())
                {
                    editNama.Text = dr["jenis"].ToString();
                    ddlEditJenis.SelectedValue = dr["untuk"].ToString();
                    editKode.Text = id;
                }
                connection.Close();
            }
            else
            {

            }
        }
        
    }


    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        panelAdd.Visible = false;
        panelData.Visible = true;
        panelEdit.Visible = false;
        ddlAddJenis.SelectedValue = null;
        addNama.Text = "";
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

            tempLinkBerkas = ddlAddJenis.SelectedItem + "_" + lib.Encrypt(addUpload.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + addUpload.FileName.Split('.')[addUpload.FileName.Split('.').Length - 1];
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

                String[] param = new String[7];
                param[0] = tempLinkBerkas; //nama
                param[1] = Server.HtmlEncode(addNama.Text.Trim());  //jenis
                param[2] = Server.HtmlEncode(ddlAddJenis.SelectedValue.Trim()); //untuk
                param[3] = addUpload.FileBytes.Length.ToString();
                param[4] = fileSavePath;
                param[5] = extension;
                param[6] = Context.User.Identity.Name;

                try
                {
                    lib.CallProcedure("stp_createDokumenMonev", param);
                    loadData();
                    showAlertSuccess("Tambah Dokumen Monev Berhasil.");
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
        return lib.CallProcedure("stp_getDokumenMonev", new String[] { Context.User.Identity.Name, Server.HtmlEncode(txtCari.Text.Trim()) });
    }

    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
    }

    protected void btnSubmitEdit_Click(object sender, EventArgs e)
    {
        string tempLinkBerkas = "";
        string saveDir = @"\Uploaded\";
        string appPath = Request.PhysicalApplicationPath;
        string fileSavePath = "";
        string extension = "";

        string fileExtension = Path.GetExtension(editUpload.PostedFile.FileName);
        if (editUpload.PostedFile != null && editUpload.PostedFile.ContentLength > 0 && fileExtension == ".pdf")
        {
            string fileName = Path.GetFileName(editUpload.PostedFile.FileName);
            string contenttype = String.Empty;

            //tempLinkBerkas = "Buku" + lib.Encrypt(addUpload.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + addUpload.FileName.Split('.')[addUpload.FileName.Split('.').Length - 1];

            extension = Path.GetExtension(editUpload.PostedFile.FileName).ToString();

            tempLinkBerkas = ddlEditJenis.SelectedItem + "_" + lib.Encrypt(editUpload.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + editUpload.FileName.Split('.')[editUpload.FileName.Split('.').Length - 1];
            editUpload.SaveAs(appPath + saveDir + tempLinkBerkas);
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

                String[] param = new String[8];
                param[0] = editKode.Text; //nama
                param[1] = tempLinkBerkas; //nama
                param[2] = Server.HtmlEncode(editNama.Text.Trim());  //jenis
                param[3] = Server.HtmlEncode(ddlEditJenis.SelectedValue.Trim()); //untuk
                param[4] = editUpload.FileBytes.Length.ToString();
                param[5] = fileSavePath;
                param[6] = extension;
                param[7] = Context.User.Identity.Name;

                try
                {
                    lib.CallProcedure("stp_editDokumenMonev ", param);
                    loadData();
                    showAlertSuccess("Ubah Dokumen Monev Berhasil.");
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

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib.CallProcedure("stp_deleteDokumenMonev", new string[] { txtConfirmDelete.Text });
        loadData();
        showAlertSuccess("Hapus dokumen monev berhasil");
    }
}
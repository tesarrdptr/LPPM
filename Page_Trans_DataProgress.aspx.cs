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


public partial class Page_Trans_ProgressPenelitian : System.Web.UI.Page
{
    public PolmanAstraLibrary.PolmanAstraLibrary lib2 = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    public PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    SiteMaster master;
    DataTable dt = new DataTable();
    MailFunction mail = new MailFunction();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    //LDAPAuthentication adAuth = new LDAPAuthentication();

    private string conn = ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString();
    protected void Page_Load(object sender, EventArgs e)
    {
        master = this.Master as SiteMaster;
        if (!IsPostBack)
        {
            ProposalPenelitian();
            
            loadJenisDokumenMonev();
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
                    if (!Context.User.Identity.IsAuthenticated || lib2.CallProcedure("stp_getMenuByRole", new string[] { ticket.UserData.Split('|')[index].Split('@')[0], ticket.UserData.Split('|')[index].Split('@')[1], Request.Path.Split('/')[Request.Path.Split('/').Length - 1].Replace(".aspx", "") }).Rows.Count == 0)
                    {
                        Response.Redirect("Default");
                    }
                }
            }
            catch
            {
                Response.Redirect("Default");
            }
            ddStatus.Items.Add(new ListItem("--- Semua ---", "ALL"));
            ddStatus.Items.Add(new ListItem("Menunggu Konfirmasi", "0"));
            ddStatus.Items.Add(new ListItem("Ditolak", "1"));
            ddStatus.Items.Add(new ListItem("Diterima", "2"));

        }
        loadData();
        hideAlert();
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        gridData.Width = Unit.Percentage(100);
    }

    
    protected void ProposalPenelitian()
    {
        ddlAddProgress.DataSource = lib2.CallProcedure("stp_GetNamaPropPenelitian", new string[] { Context.User.Identity.Name });
        ddlAddProgress.DataTextField = "prp_judul";
        ddlAddProgress.DataValueField = "prp_id";
        ddlAddProgress.DataBind();
        ddlAddProgress.Items.Insert(0, new ListItem("--- Proposal Penelitian ---", ""));
    }
    
    protected void loadJenisDokumenMonev()
    {
        ddlJenisDokumen.DataSource = lib2.CallProcedure("stp_getJenisDokumenMonev", new string[] { Context.User.Identity.Name });
        ddlJenisDokumen.DataTextField = "monevdoc_jenis";
        ddlJenisDokumen.DataValueField = "monevdoc_id";
        ddlJenisDokumen.DataBind();
        ddlJenisDokumen.Items.Insert(0, new ListItem("--- Jenis Dokumen Progress ---", ""));
    }
    protected void loadData()
    {
        String id = Request.QueryString["id"];
        gridData.DataSource = lib2.CallProcedure("stp_GetProgressPenelitianSaya", new string[] { Context.User.Identity.Name, Server.HtmlEncode(txtCari.Text.Trim()), ddStatus.SelectedValue, id });
        gridData.DataBind();
        ddlAddProgress.SelectedValue = id;
    }


    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gridData.EditIndex = e.NewEditIndex;
        this.loadData();
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
                SqlCommand com = new SqlCommand("select * from  TRATTACHMENT where atc_name=@id", con);
                com.Parameters.AddWithValue("id", e.CommandArgument.ToString());
                SqlDataReader dr = com.ExecuteReader();

                if (dr.Read())
                {
                    string fileName = dr["atc_name"].ToString();
                    string fileLength = dr["atc_size"].ToString();
                    string filePath = dr["atc_path"].ToString();
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

            else if (e.CommandName == "Lihat_Nilai")
            {
                GridNilai.DataSource = lib2.CallProcedure("stp_getLihatNilai", new string[] { e.CommandArgument.ToString() });
                GridNilai.DataBind();
                dt = lib2.CallProcedure("stp_getTotalNilai", new string[] { e.CommandArgument.ToString() });
                TotalScore.Text = dt.Rows[0][0].ToString();

                panelLihatNilai.Visible = true;
                panelData.Visible = false;
            }
            else if (e.CommandName == "Ubah")
            {
                String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
                dt = lib2.CallProcedure("stp_getProgressPenelitianByID", new String[] { id });

                ddlEditProgress.DataSource = lib2.CallProcedure("stp_GetNamaPropPenelitian", new string[] { Context.User.Identity.Name });
                ddlEditProgress.DataTextField = "prp_judul";
                ddlEditProgress.DataValueField = "prp_id";
                ddlEditProgress.DataBind();
                ddlEditProgress.SelectedValue = dt.Rows[0][4].ToString();
                editPrgID.Text = dt.Rows[0][0].ToString();

                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;
            }
            else if (e.CommandName == "Kirim")
            {
                String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
                lib.CallProcedure("stp_kirimProgressPenelitian", new String[] { id,Context.User.Identity.Name });
                loadData();
                Response.Redirect("Page_Trans_DataProgress?id=" + Request.QueryString["id"]);
            }

        }
    }

    protected void linkTambah_Click(object sender, EventArgs e)
    {
        panelData.Visible = false;
        panelAdd.Visible = true;
    }

    protected void btnCari_Click(object sender, EventArgs e)
    {
        loadData();
    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        ddlAddProgress.SelectedIndex = 0;
        panelAdd.Visible = false;
        panelLihatNilai.Visible = false;
        panelData.Visible = true;
    }

    protected void gridData_PageIndexChanged(object sender, EventArgs e)
    {
    }

    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }



    protected void Upload(object sender, EventArgs e)
    {
        string tempLinkBerkas = "";
        string saveDir = @"\Uploaded\";
        string appPath = Request.PhysicalApplicationPath;
        string fileSavePath = "";

        string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName).ToString();
        if (FileUpload1.PostedFile != null && FileUpload1.PostedFile.ContentLength > 0 && fileExtension == ".pdf")
        {
            string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
            string contenttype = String.Empty;

            tempLinkBerkas = "ProgressPeneltian_" + lib2.Encrypt(FileUpload1.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + FileUpload1.FileName.Split('.')[FileUpload1.FileName.Split('.').Length - 1];
            FileUpload1.SaveAs(appPath + saveDir + tempLinkBerkas);
            fileSavePath = Server.MapPath("Uploaded");
            fileSavePath = fileSavePath + "//" + tempLinkBerkas;

            FileInfo fileInfo = new FileInfo(fileSavePath);

            try
            {
                lib.CallProcedure("stp_create_progress", new String[] {
                        Server.HtmlEncode(ddlAddProgress.SelectedValue.Trim()),
                        Server.HtmlEncode(tempLinkBerkas),
                        Server.HtmlEncode(FileUpload1.FileBytes.Length.ToString()),
                        Server.HtmlEncode(fileSavePath.Trim()),
                        fileExtension,
                        Context.User.Identity.Name,ddlJenisDokumen.SelectedValue});
                btnCancelAdd_Click(sender, e);
                loadData();
                master.showAlertSuccess("Berhasil Menambah Proggres Penelitian");
            }
            catch (Exception ex)
            {
                master.showAlertDanger(ex.ToString());
            }






        }
        else if (FileUpload1.PostedFile == null || FileUpload1.PostedFile.ContentLength == 0)
        {
            showAlertDanger("File belum diunggah, unggah file PDF.");
        }
        else if (FileUpload1.PostedFile != null && FileUpload1.PostedFile.ContentLength > 0 && fileExtension != ".pdf")
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

    protected void hideAlert()
    {
        divAlert.Visible = false;
        divSuccess.Visible = false;
    }

    protected void gridData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TableCell status = e.Row.Cells[4];
            if (status.Text == "Menunggu Konfirmasi")
            {
                status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Menunggu Konfirmasi</span>");
            }

            else if (status.Text == "Ditolak")
            {
                status.Text = HttpUtility.HtmlDecode("<span class='label label-danger'>Ditolak</span>");

            }
            else if (status.Text == "Diterima")
            {
                status.Text = HttpUtility.HtmlDecode("<span class='label label-success'>Diterima</span>");

            }
            else if (status.Text == "Belum Dikirim")
            {
                status.Text = HttpUtility.HtmlDecode("<span class='label label-warning'>Belum Dikirim</span>");

            }
        }
    }

    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        hideAlert();
        panelLihatNilai.Visible = false;
        panelData.Visible = false;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
        ddlEditProgress.SelectedIndex = 0;
    }

    protected void btnSubmitEdit_Click(object sender, EventArgs e)
    {
        string tempLinkBerkas = "";
        string saveDir = @"\Uploaded\";
        string appPath = Request.PhysicalApplicationPath;
        string fileSavePath = "";

        try
        {
            string fileExtension = Path.GetExtension(FileUpload2.PostedFile.FileName);
            if (FileUpload2.PostedFile != null && FileUpload2.PostedFile.ContentLength > 0 && fileExtension == ".pdf")
            {
                string fileName = Path.GetFileName(FileUpload2.PostedFile.FileName);
                string contenttype = String.Empty;
                fileSavePath = Server.MapPath("Uploaded");
                fileSavePath = fileSavePath + "//" + fileName;
                tempLinkBerkas = "ProgressPeneltian_" + lib2.Encrypt(FileUpload1.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + FileUpload1.FileName.Split('.')[FileUpload1.FileName.Split('.').Length - 1];
                FileUpload1.SaveAs(appPath + saveDir + tempLinkBerkas);

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
                    lib2.CallProcedure("stp_getProgressPenelitianAttachment", new string[] {
                        Server.HtmlEncode(editPrgID.Text.Trim()),
                        Server.HtmlEncode(ddlEditProgress.SelectedValue.Trim()),
                        Server.HtmlEncode(Context.User.Identity.Name),
                        Server.HtmlEncode(tempLinkBerkas),
                        Server.HtmlEncode(fileInfo.Length.ToString().Trim()),
                        Server.HtmlEncode(contenttype.Trim()),
                        Server.HtmlEncode(fileSavePath.Trim())});
                    btnCancelEdit_Click(sender, e);
                    loadData();
                    showAlertSuccess("Edit Progress Berhasil");
                    hideAlert();

                }
                else
                {
                    showAlertDanger("Format file tidak valid." +
                 " Unggah Word/PDF");
                }
            }

            else
            {

                showAlertDanger("Format file tidak valid." +
                    " Unggah PDF");
            }
        }
        catch
        {
        }
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib2.CallProcedure("stp_hapusProgressPenelitian", new string[] { txtConfirmDelete.Text });
        loadData();
        showAlertSuccess("Hapus Progress Penelitian Berhasil");
    }

    protected void gridproposal_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dtStatus;
        if (e.CommandName != "Page")
        {
            if (e.CommandName == "progress")
            {
                String id = e.CommandArgument.ToString();

                ddlAddProgress.SelectedValue = id;
                gridData.DataSource = lib2.CallProcedure("stp_GetProgressPenelitianSaya", new string[] { Context.User.Identity.Name, Server.HtmlEncode(txtCari.Text.Trim()), ddStatus.SelectedValue, id });
                gridData.DataBind();
                dtStatus = lib2.CallProcedure("stp_getstatusowner", new string[] { id,Context.User.Identity.Name });
                if(dtStatus.Rows[0][3].ToString()=="0")
                {
                    linkTambahpProgress.Visible = false;
                }
                panelData.Visible = true;
                panelAdd.Visible = false;
                panelEdit.Visible = false;
                master.hideAlert();
            }

        }

    }
    protected void LinkButton1_Click(object sender, EventArgs e)
    {
        lib2.CallProcedure("stp_kirimProgressPenelitian", new string[] { txtKirim.Text, Context.User.Identity.Name });
        try
        {
            dt = master.lib.CallProcedure("stp_detailProgress", new String[] { txtKirim.Text });
            String htmlBody = "";

            //GENERATE MAIL BODY HERE
            htmlBody += "Dear Bapak/Ibu Reviewer,<br><br>";
            htmlBody += "Terdapat pengajuan progress penelitian baru dengan judul " + dt.Rows[0][0].ToString() + ".<br><br>";
            htmlBody += "Silahkan lakukan approval pada progress penelitian tersebut.<br><br>";
            htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_Trans_AuditProgressPenelitian.aspx'>[LIHAT DETAIL]</a></b><br><br>";
            htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
            htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
            //END GENERATE MAIL BODY HERE

            mail.SendMail(
                "[SISFO LPPM] - Pengajuan Progress Pengabdian Baru: " + dt.Rows[0][0].ToString(),
                "rachmadrizky9@gmail.com",
                htmlBody
            );
        }
        catch { }

        loadData();
        showAlertSuccess("Kirim Progress Penelitian Berhasil");
    }

    protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridData.PageIndex = e.NewPageIndex;
        loadData();
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
        return lib2.CallProcedure("stp_GetProgressPenelitianSaya", new string[] { Context.User.Identity.Name, Server.HtmlEncode(txtCari.Text.Trim()), ddStatus.SelectedValue });
    }

    protected void gridData_Sorting1(object sender, GridViewSortEventArgs e)
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

    protected void gridproposal_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        //gridproposal.PageIndex = e.NewPageIndex;
        LoadDataSearching();
    }

    

    protected void gridproposal_Sorting(object sender, GridViewSortEventArgs e)
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
    public DataTable LoadDataSearching()
    {
        return lib.CallProcedure("stp_getPenelitianProposalsByOwner", new String[] { Context.User.Identity.Name, ddStatus.SelectedValue, Server.HtmlEncode(txtCari.Text.Trim()) });
    }


    protected void btnKembali_Click(object sender, EventArgs e)
    {
        Response.Redirect("Page_Trans_ProgressPenelitian");
    }
}
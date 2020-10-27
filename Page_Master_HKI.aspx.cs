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

public partial class Page_Master_HKI : System.Web.UI.Page
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
            addDDL();
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
                ddlTahun.Items.Add(li);
                //ddlAnggaranEdit.Items.Add(li);
            }
            ddlTahun.Items.FindByText(year.ToString()).Selected = true;

        }
        loadData();
        hideAlert();
        gridData.Width = Unit.Percentage(100);
    }

    protected void loadData()
    {
        gridData.DataSource = lib.CallProcedure("stp_getHistoryHKI", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
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
        panelDetil.Visible = false;
    }

    protected void btnCari_Click(object sender, EventArgs e)
    {

    }

    protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        
        if (e.CommandName == "download")
        {
            try
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
            catch
            {
                showAlertDanger("Maaf ya");
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
            SqlCommand sqlCmd = new SqlCommand("stp_detailHKI", connection);
            sqlCmd.Parameters.AddWithValue("@p1", id);
            sqlCmd.CommandType = CommandType.StoredProcedure;
            SqlDataReader dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
            //showAlertSuccess(id);
            while (dr.Read())
            {
                dtJudul.Text = dr["hki_judul"].ToString();
                dtNama.Text = dr["hki_nama"].ToString();
                dtJenis.Text = dr["hki_jenis"].ToString();
                ddldtTahun.Text = dr["hki_tahun"].ToString();
                dtPendaftaran.Text = dr["hki_no_pendaftaran"].ToString();
                dtStatus.Text = dr["hki_status"].ToString();
                dtnoHKI.Text = dr["hki_no"].ToString();
                dtURL.Text = dr["hki_url"].ToString();
                creaby.Text = dr["hki_creaby"].ToString();
                creadate.Text = dr["hki_creadate"].ToString();
                updby.Text = dr["hki_updateby"].ToString();
                upddate.Text = dr["hki_updatedate"].ToString();
                //ddlAnggaranEdit.SelectedValue = Convert.ToDateTime(dr["anggaran"].ToString()).ToString("yyyy");
                //editTanggalMulai.Text = Convert.ToDateTime(dr["mulai"].ToString()).ToString("yyyy-MM-dd");
                //editTanggalSelesai.Text = Convert.ToDateTime(dr["selesai"].ToString()).ToString("yyyy-MM-dd");
                //ddlEditTanggal.SelectedValue = dr["jenis"].ToString();
                //editKode.Text = id;

            }
            connection.Close();
        }

    }

    protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {

    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        panelAdd.Visible = false;
        panelData.Visible = true;
        panelDetil.Visible = false;
        AddJudul.Text = "";
        addNama.SelectedValue = "";
        addJenis.Text = "";
         addNama.SelectedValue = "";
        addPendaftaran.Text = "";
        ddlStatus.SelectedValue = "";
        addNoHKI.Text = "";
        addUrl.Text = "";
    }

    protected void addDDL()
    {
        addNama.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        addNama.DataValueField = "iduser";
        addNama.DataTextField = "iduser";
        addNama.DataBind();
        addNama.Items.Insert(0, new ListItem("--- Pilih Nama ---", ""));
        addJenis.Items.Add(new ListItem("--- Pilih Jenis ---", ""));
        addJenis.Items.Add(new ListItem("Paten", "Paten"));
        addJenis.Items.Add(new ListItem("Paten Sederhana", "Paten Sederhana"));
        addJenis.Items.Add(new ListItem("Hak Cipta", "Hak Cipta"));
        addJenis.Items.Add(new ListItem("Merek Dagang", "Merek Dagang"));
        addJenis.Items.Add(new ListItem("Rahasia Dagang", "Rahasia Dagang"));
        addJenis.Items.Add(new ListItem("Desain Produk Industri", "Desain Produk Industri"));
        addJenis.Items.Add(new ListItem("Indikasi Geografis", "Indikasi Geografis"));
        addJenis.Items.Add(new ListItem("Perlindungan Varietas Tanaman", "Perlindungan Varietas Tanaman"));
        addJenis.Items.Add(new ListItem("Perlindungan Topografi Sirkuit Terpadu", "Perlindungan Topografi Sirkuit Terpadu"));
        ddlStatus.Items.Add(new ListItem("--- Pilih Status ---", ""));
        ddlStatus.Items.Add(new ListItem("Terdaftar", "Terdaftar"));
        ddlStatus.Items.Add(new ListItem("Granted", "Granted"));

    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
        string tempLinkBerkas = "";
        string saveDir = @"\Uploaded\";
        string appPath = Request.PhysicalApplicationPath;
        string fileSavePath = "";
        string extension = "";

        //string fileExtension = Path.GetExtension(addUpload.PostedFile.FileName);
        //if (addUpload.PostedFile != null && addUpload.PostedFile.ContentLength > 0 && fileExtension == ".pdf")
        //{
        //    string fileName = Path.GetFileName(addUpload.PostedFile.FileName);
        //    string contenttype = String.Empty;

        //    //tempLinkBerkas = "History HKI" + lib.Encrypt(addUpload.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + addUpload.FileName.Split('.')[addUpload.FileName.Split('.').Length - 1];


        //    extension = Path.GetExtension(addUpload.PostedFile.FileName).ToString();
        //    tempLinkBerkas = ddlTahun.SelectedItem + "_" + lib.Encrypt(addUpload.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + addUpload.FileName.Split('.')[addUpload.FileName.Split('.').Length - 1];
        //    addUpload.SaveAs(appPath + saveDir + tempLinkBerkas);
        //    fileSavePath = Server.MapPath("Uploaded");
        //    fileSavePath = fileSavePath + "//" + fileName;

        //    FileInfo fileInfo = new FileInfo(fileSavePath);
        //    switch (fileExtension)
        //    {
        //        case ".doc":
        //            contenttype = "application/vnd.ms-word";
        //            break;
        //        case ".docx":
        //            contenttype = "application/vnd.ms-word";
        //            break;
        //        case ".pdf":
        //            contenttype = "application/pdf";
        //            break;
        //    }
        string fileExtension = Path.GetExtension(addUpload.PostedFile.FileName);
        if (addUpload.PostedFile != null && addUpload.PostedFile.ContentLength > 0 && fileExtension == ".pdf")
        {
            string fileName = Path.GetFileName(addUpload.PostedFile.FileName);
            string contenttype = String.Empty;

            //tempLinkBerkas = "Buku" + lib.Encrypt(addUpload.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + addUpload.FileName.Split('.')[addUpload.FileName.Split('.').Length - 1];

            extension = Path.GetExtension(addUpload.PostedFile.FileName).ToString();

            tempLinkBerkas = ddlTahun.SelectedItem + "_" + lib.Encrypt(addUpload.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + addUpload.FileName.Split('.')[addUpload.FileName.Split('.').Length - 1];
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
                param_atc[3] = Server.HtmlEncode(contenttype.Trim());
                var atc = lib.CallProcedure("stp_newAttachment", param_atc);

                String[] param = new String[10];
                param[0] = Server.HtmlEncode(AddJudul.Text.Trim());
                param[1] = Server.HtmlEncode(addNama.SelectedValue.Trim());
                param[2] = Server.HtmlEncode(addJenis.SelectedValue.Trim());
                param[3] = Server.HtmlEncode(ddlTahun.Text.Trim());
                param[4] = Server.HtmlEncode(addPendaftaran.Text.Trim());
                param[5] = Server.HtmlEncode(ddlStatus.SelectedValue.Trim());
                param[6] = Server.HtmlEncode(addNoHKI.Text.Trim());
                param[7] = Server.HtmlEncode(addUrl.Text.Trim());
                param[8] = Context.User.Identity.Name;
                param[9] = Server.HtmlEncode(atc.Rows[0][0].ToString());

                try
                {
                    lib.CallProcedure("stp_createHistoryHKI", param);
                    loadData();
                    showAlertSuccess("Tambah History HKI Berhasil.");
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
        return lib.CallProcedure("stp_getHistoryHKI", new String[] { Context.User.Identity.Name, Server.HtmlEncode(txtCari.Text.Trim()) });
    }
}
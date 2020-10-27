using System;
using System.Web;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.IO;
using System.Text;
using ProposalPolmanAstra.Classes;
using System.Web.Configuration;


    public partial class Page_Trans_PublikasiSaya : System.Web.UI.Page
    {
        SiteMaster master;
        PolmanAstraLibrary.PolmanAstraLibrary lib2 = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
        DataTable dt = new DataTable();
        MailFunction mail = new MailFunction();
        //LDAPAuthentication adAuth = new LDAPAuthentication();

        public int year;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JenisPublikasi();
                ddStatus.Items.Add(new ListItem("--- Semua ---", "ALL"));
                ddStatus.Items.Add(new ListItem("Menunggu Konfirmasi", "0"));
                ddStatus.Items.Add(new ListItem("Ditolak", "1"));
                ddStatus.Items.Add(new ListItem("Diterima", "2"));
            }
            loadData();
            hideAlert();
            this.Page.Form.Enctype = "multipart/form-data";
            gridData.Width = Unit.Percentage(100);
        }

        protected void JenisPublikasi()
        {
            ddlJnsPublikasi.DataSource = lib2.CallProcedure("stp_GetNamaJenisPub", new string[] { });
            ddlJnsPublikasi.DataTextField = "jpb_title";
            ddlJnsPublikasi.DataValueField = "jpb_id";
            ddlJnsPublikasi.DataBind();
            ddlJnsPublikasi.Items.Insert(0, new ListItem("--- Jenis Publikasi ---", ""));
        }
        protected void loadData()
        {
            gridData.DataSource = lib2.CallProcedure("stp_GetPublikasiSaya", new string[] { Server.HtmlEncode(txtCari.Text.Trim()), Context.User.Identity.Name, ddStatus.SelectedValue });
            gridData.DataBind();
            hideAlert();
        }

        protected void linkTambah_KLIK(object sender, EventArgs e)
        {
            panelData.Visible = false;
            panelAdd.Visible = true;
        }

        protected void btnCari_Click(object sender, EventArgs e)
        {
            loadData();
        }

        protected void btnCancelAd_Click(object sender, EventArgs e)
        {
            panelData.Visible = true;
            panelAdd.Visible = false;
            txtJudulPublikasi.Text = "";
            JenisPublikasi();
        }
        protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridData.PageIndex = e.NewPageIndex;
            loadData();
        }

        protected void ddlJnsPublikasi_SelectedIndexChanged(object sender, EventArgs e)
        {
        }

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string tempLinkBerkas = "";
            string saveDir = @"\Uploaded\";
            string appPath = Request.PhysicalApplicationPath;
            string errormessage = "Error: ";
            string fileSavePath = "";
            string extension = "";
            string lenght = "";

            try
            {
                if (FileUpload1.HasFile)
                {
                    if (FileUpload1.PostedFile.ContentLength > 5242880)
                        master.showAlertDanger("Berkas terlalu besar. Maksimum adalah 5 MB");
                    else if (!Path.GetExtension(FileUpload1.FileName).ToLower().Equals(".pdf"))
                        errormessage += "<br>- Berkas harus berformat .pdf";
                    else
                    {
                        extension = Path.GetExtension(FileUpload1.PostedFile.FileName).ToString();

                        tempLinkBerkas = "Publikasi_" + lib2.Encrypt(FileUpload1.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + FileUpload1.FileName.Split('.')[FileUpload1.FileName.Split('.').Length - 1];
                        FileUpload1.SaveAs(appPath + saveDir + tempLinkBerkas);
                        fileSavePath = Server.MapPath("Uploaded");
                        fileSavePath = fileSavePath + "//" + tempLinkBerkas;

                        FileInfo fileInfo = new FileInfo(fileSavePath);
                        lenght = fileInfo.Length.ToString().Trim();
                    }
                }
            }
            catch
            {
                if (errormessage.Equals("Error: "))
                    errormessage += "<br>- Terjadi kesalahan sistem. Mohon hubungi bagian MIS";
            }
            if (errormessage.Equals("Error: "))
            {
                lib2.CallProcedure("stp_create_publikasi", new string[] {
                        Server.HtmlEncode(ddlJnsPublikasi.SelectedValue.Trim()),
                        Server.HtmlEncode(txtJudulPublikasi.Text.Trim()),
                        Server.HtmlEncode(txtYear.Text.Trim()),
                        Server.HtmlEncode(Context.User.Identity.Name.Trim()),
                        Context.User.Identity.Name,
                        Server.HtmlEncode(tempLinkBerkas.Trim()),
                        Server.HtmlEncode(lenght),
                        Server.HtmlEncode(extension.Trim()),
                        Server.HtmlEncode(fileSavePath.Trim()) });
                btnCancelAd_Click(sender, e);
                loadData();
                showAlertSuccess("Pengajuan Publikasi berhasil");

                txtJudulPublikasi.Text = "";
                txtYear.Text = "";
            }
            else master.showAlertDanger(errormessage);
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
                dt = lib2.CallProcedure("stp_getPublikasiByID", new String[] { e.CommandArgument.ToString() });

                ddlEditJenisPublikasi.DataSource = lib2.CallProcedure("stp_GetNamaJenisPub", new string[] { });
                ddlEditJenisPublikasi.DataTextField = "jpb_title";
                ddlEditJenisPublikasi.DataValueField = "jpb_id";
                ddlEditJenisPublikasi.DataBind();
                ddlEditJenisPublikasi.SelectedValue = dt.Rows[0][4].ToString();
                txtEditYear.Text = dt.Rows[0][7].ToString();
                txtEditJudulPublikasi.Text = dt.Rows[0][6].ToString();
                TextBox4.Text = dt.Rows[0][0].ToString();

                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;
            }
        }

        protected void gridData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell status = e.Row.Cells[6];
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

        protected void linkConfirmDelete_Click(object sender, EventArgs e)
        {
            lib2.CallProcedure("stp_hapusPublikasi", new string[] { TextBox1.Text });
            loadData();
            showAlertSuccess("Hapus Publikasi Berhasil");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            lib2.CallProcedure("stp_kirimPublikasi", new string[] { txtKirim.Text, Context.User.Identity.Name });
            try
            {
                dt = master.lib.CallProcedure("stp_detailPublikasi", new String[] { txtKirim.Text });
                String htmlBody = "";

                //GENERATE MAIL BODY HERE
                htmlBody += "Dear Bapak/Ibu Reviewer,<br><br>";
                htmlBody += "Terdapat pengajuan publikasi baru dengan judul " + dt.Rows[0][0].ToString() + ".<br><br>";
                htmlBody += "Silahkan lakukan approval pada publikasi tersebut.<br><br>";
                htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_Trans_AuditPublikasi.aspx'>[LIHAT DETAIL]</a></b><br><br>";
                htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
                htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
                //END GENERATE MAIL BODY HERE

                mail.SendMail(
                    "[SISFO LPPM] - Pengajuan Publikasi Baru: " + dt.Rows[0][0].ToString(),
                    "rachmadrizky9@gmail.com",
                    htmlBody
                );

            }
            catch { }
            loadData();
            showAlertSuccess("Kirim Publikasi Berhasil");
        }

        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            hideAlert();
            panelData.Visible = true;
            panelAdd.Visible = false;
            panelEdit.Visible = false;
            ddlEditJenisPublikasi.SelectedIndex = 0;
        }

        protected void LinkButton3_Click(object sender, EventArgs e)
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

                    tempLinkBerkas = "Publikasi_" + lib2.Encrypt(FileUpload1.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + FileUpload1.FileName.Split('.')[FileUpload1.FileName.Split('.').Length - 1];
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
                        lib2.CallProcedure("stp_getPublikasiAttachment", new string[] {
                        Server.HtmlEncode(TextBox4.Text.Trim()),
                        Server.HtmlEncode(ddlEditJenisPublikasi.SelectedValue.Trim()),
                        Server.HtmlEncode(Context.User.Identity.Name),
                        Server.HtmlEncode(tempLinkBerkas),
                        Server.HtmlEncode(fileInfo.Length.ToString().Trim()),
                        Server.HtmlEncode(contenttype.Trim()),
                        Server.HtmlEncode(fileSavePath.Trim()),
                        Server.HtmlEncode(txtEditJudulPublikasi.Text.Trim()),
                        Server.HtmlEncode(txtEditYear.Text.Trim())});
                        LinkButton2_Click(sender, e);
                        loadData();
                        showAlertSuccess("Edit Publikasi Berhasil");
                        hideAlert();
                        panelData.Visible = true;
                        panelEdit.Visible = false;
                        panelAdd.Visible = false;
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
    }
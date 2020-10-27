using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.IO;
using System.Web;
using System.Data.SqlClient;
using ProposalPolmanAstra.Classes;
using System.Web.Configuration;


    public partial class Page_PublikasiMahasiswa : System.Web.UI.Page
    {
        PolmanAstraLibrary.PolmanAstraLibrary lib2 = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
        DataTable dt = new DataTable();
        static CDO.Message message = new CDO.Message();
        static CDO.IConfiguration configuration = message.Configuration;
        static ADODB.Fields fields = configuration.Fields;
        LDAPAuthentication adAuth = new LDAPAuthentication();

        public int year;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                JenisPublikasi();
                //tahun();
                ddStatus.Items.Add(new ListItem("--- Semua ---", "ALL"));
                ddStatus.Items.Add(new ListItem("Menunggu Konfirmasi", "0"));
                ddStatus.Items.Add(new ListItem("Ditolak", "1"));
                ddStatus.Items.Add(new ListItem("Diterima", "2"));
            }
            initializeMailConfig();
            loadData();
            hideAlert();
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            gridData.Width = Unit.Percentage(100);
        }

        protected void JenisPublikasi()
        {
            ddlJnsPublikasi.DataSource = lib2.CallProcedure("stp_GetJenisPubMahasiswa", new string[] { });
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

        //protected void linkTambah_Click(object sender, EventArgs e)
        //{
        //    panelData.Visible = false;
        //    panelAdd.Visible = true;   
        //}

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
            txtYear.Text = "";
            JenisPublikasi();
            //tahun();
        }
        protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridData.PageIndex = e.NewPageIndex;
            loadData();
        }

        protected void ddlJnsPublikasi_SelectedIndexChanged(object sender, EventArgs e)
        {
            /*using (SqlConnection connection = new SqlConnection(conn))
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand();
                cmd = new SqlCommand("Select * from TRPROPOSAL WHERE jns_id = " + ddlJnsPublikasi.SelectedValue, connection);
                ddlProposal.DataSource = cmd.ExecuteReader();
                ddlProposal.DataTextField = "prp_judul";
                ddlProposal.DataValueField = "prp_id";
                ddlProposal.DataBind();
            }*/
        }

        //protected void tahun()
        //{
        //    ddlYear.Items.Add(new ListItem("Pilih Tahun Penulisan", "-1"));
        //    for (year = DateTime.Now.Year; year >= 1998; year--)
        //    {
        //        ddlYear.Items.Add(new ListItem(Convert.ToString(year)));
        //    }
        //}

        protected void btnUpload_Click(object sender, EventArgs e)
        {
            string tempLinkBerkas = "";
            string saveDir = @"\Uploaded\";
            string appPath = Request.PhysicalApplicationPath;
            string fileSavePath = "";

            if (FileUpload1.PostedFile != null && FileUpload1.PostedFile.ContentLength > 0)
            {
                try
                {
                    string fileName = Path.GetFileName(FileUpload1.PostedFile.FileName);
                    string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                    string contenttype = String.Empty;
                    //first check if "uploads" folder exist or not, if not create it
                    fileSavePath = Server.MapPath("Uploaded");

                    //after checking or creating folder it's time to save the file
                    fileSavePath = fileSavePath + "//" + fileName;

                    tempLinkBerkas = "Publikasi_" + lib2.Encrypt(FileUpload1.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + FileUpload1.FileName.Split('.')[FileUpload1.FileName.Split('.').Length - 1];
                    FileUpload1.SaveAs(appPath + saveDir + tempLinkBerkas);

                    FileInfo fileInfo = new FileInfo(fileSavePath);
                    switch (fileExtension)
                    {
                        case ".pdf":
                            contenttype = "application/pdf";
                            break;
                    }
                    if (contenttype != String.Empty)
                    {
                        lib2.CallProcedure("stp_create_publikasi", new string[] {
                        Server.HtmlEncode(ddlJnsPublikasi.SelectedValue.Trim()),
                        Server.HtmlEncode(txtJudulPublikasi.Text.Trim()),
                        Server.HtmlEncode(txtYear.Text.Trim()),
                        Server.HtmlEncode(Context.User.Identity.Name.Trim()),
                        Context.User.Identity.Name,
                        Server.HtmlEncode(tempLinkBerkas),
                        Server.HtmlEncode(fileInfo.Length.ToString().Trim()),
                        Server.HtmlEncode(contenttype.Trim()),
                        Server.HtmlEncode(fileSavePath.Trim()) });
                        btnCancelAd_Click(sender, e);
                        loadData();
                        showAlertSuccess("Pengajuan Publikasi berhasil");

                        txtJudulPublikasi.Text = "";
                        txtYear.Text = "";
                    }

                    else
                    {
                        showAlertDanger("Format file tidak sesuai. Unggah file dengan format PDF");
                    }
                }
                catch { }
                {
                    showAlertDanger("Format file tidak sesuai. Unggah file dengan format PDF");
                }
            }
            else
            {
                showAlertDanger("Harus unggah File !");
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
                //TableCell abstraks = e.Row.Cells[2];
                TableCell status = e.Row.Cells[5];
                //abstraks.Text = abstraks.Text.Length > 30 ? abstraks.Text.Substring(0, 30) + "..." : abstraks.Text;
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
                //else if (status.Text == "-1") status.Text = HttpUtility.HtmlDecode("<span class='label label-danger'>Ditolak</span>");
                //else
                //{
                //    status.Text = HttpUtility.HtmlDecode("<span class='label label-warning' title='Menunggu konfirmasi anggota'>Konfirmasi</span>");
                //}

            }
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
                //String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
                dt = lib2.CallProcedure("stp_getPublikasiByID", new String[] { e.CommandArgument.ToString() });


                //string id1 = dt.Rows[0][4].ToString();
                //string id2 = dt.Rows[0][1].ToString();
                ddlEditJenisPublikasi.DataSource = lib2.CallProcedure("stp_GetJenisPubMahasiswa", new string[] { });
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
        protected void linkConfirmDelete_Click(object sender, EventArgs e)
        {
            lib2.CallProcedure("stp_hapusPublikasi", new string[] { TextBox1.Text });
            loadData();
            showAlertSuccess("Hapus Publikasi Berhasil");
        }

        protected void LinkButton1_Click(object sender, EventArgs e)
        {
            lib2.CallProcedure("stp_kirimPublikasi", new string[] { txtKirim.Text, Context.User.Identity.Name });
            loadData();
            showAlertSuccess("Kirim Publikasi Berhasil");
        }
        protected void LinkButton2_Click(object sender, EventArgs e)
        {
            hideAlert();
            panelData.Visible = true;
            panelAdd.Visible = false;
            panelEdit.Visible = false;
            //editAtcID.Text = "";
            //ddlEditProgress.Text = "";
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
                //DataTable data = lib2.CallProcedure("stp_getProgressPenelitianAttachment", new string[] { editPrgID.Text });

                string fileExtension = Path.GetExtension(FileUpload2.PostedFile.FileName);
                if (FileUpload2.PostedFile != null && FileUpload2.PostedFile.ContentLength > 0 && fileExtension == ".pdf")
                {
                    string fileName = Path.GetFileName(FileUpload2.PostedFile.FileName);
                    //string fileExtension = Path.GetExtension(FileUpload1.PostedFile.FileName);
                    string contenttype = String.Empty;
                    //first check if "uploads" folder exist or not, if not create it
                    fileSavePath = Server.MapPath("Uploaded");

                    //after checking or creating folder it's time to save the file
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
                        //string[] param = new string[8];
                        //param[0] = Server.HtmlEncode(editPrgID.Text);
                        //param[1] = Context.User.Identity.Name;

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
                //param[2] = Server.HtmlEncode(editJudul.Text);
                //param[3] = Server.HtmlEncode(editJenisID.SelectedValue);
                //param[4] = Server.HtmlEncode(editAbstrak.Text);
                //param[5] = Server.HtmlEncode(editKeyword.Text);
                //param[6] = "";
                ////JANGAN LUPA DIGANTI KALO ATCNYA UDAH JADI
                //param[7] = Server.HtmlEncode(data.Rows[0][0].ToString());

                //lib.CallProcedure("stp_editProposal", param);
                //btnCancelEdit_Click(sender, e);
                //loadData();
                //master.showAlertSuccess("Edit Proposal Berhasil");
            }
            catch { }
            {
                //master.showAlertDanger(ex.Message + " " + ex.StackTrace);
            }
        }

        protected void initializeMailConfig()
        {
            ADODB.Field field = fields["http://schemas.microsoft.com/cdo/configuration/smtpserver"];
            field.Value = WebConfigurationManager.AppSettings["linkSMTPServer"];
            field = fields["http://schemas.microsoft.com/cdo/configuration/smtpserverport"];
            field.Value = WebConfigurationManager.AppSettings["linkPort"];
            field = fields["http://schemas.microsoft.com/cdo/configuration/sendusing"];
            field.Value = CDO.CdoSendUsing.cdoSendUsingPort;
            field = fields["http://schemas.microsoft.com/cdo/configuration/smtpauthenticate"];
            field.Value = CDO.CdoProtocolsAuthentication.cdoBasic;
            field = fields["http://schemas.microsoft.com/cdo/configuration/sendusername"];
            field.Value = WebConfigurationManager.AppSettings["linkUserMail"];
            field = fields["http://schemas.microsoft.com/cdo/configuration/sendpassword"];
            field.Value = WebConfigurationManager.AppSettings["linkPasswordMail"];
            field = fields["http://schemas.microsoft.com/cdo/configuration/smtpusessl"];
            field.Value = "true";
            fields.Update();
        }
    }
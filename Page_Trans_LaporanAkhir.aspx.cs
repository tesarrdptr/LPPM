using ProposalPolmanAstra.Classes;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Web.UI;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using ClosedXML.Excel;

public partial class Page_KelolaProposal : System.Web.UI.Page
{
    SiteMaster master;
    System.Data.SqlClient.SqlConnection connection =
    new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);
    public PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    MailFunction mail = new MailFunction();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    //   LDAPAuthentication adAuth = new LDAPAuthentication();
    private string[] idAnggota = new string[5];

    protected void Page_Load(object sender, EventArgs e)
    {
        master = this.Master as SiteMaster;

        if (!IsPostBack)
        {
            master.setTitle("Laporan Akhir Saya");
            ddStatus.Items.Add(new ListItem("--- Semua ---", "ALL"));
            ddStatus.Items.Add(new ListItem("Draft", "0"));
            ddStatus.Items.Add(new ListItem("Menunggu Konfirmasi", "1"));
            ddStatus.Items.Add(new ListItem("Diajukan", "2"));
            ProposalPenelitian();


            try
            {
                FormsIdentity id = (FormsIdentity)Context.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                if (!Context.User.Identity.IsAuthenticated || !ticket.UserData.Split('@')[1].Equals("APP07") || master.sso.CallProcedure("stp_getMenuByRole", new string[] { ticket.UserData.Split('@')[0], ticket.UserData.Split('@')[1], Request.Path.Split('/')[Request.Path.Split('/').Length - 1].Replace(".aspx", "") }).Rows.Count == 0)
                {
                    Response.Redirect("http://localhost/sso");
                }
            }
            catch
            {
                Response.Redirect("http://localhost/sso");
            }
        }
        if (!String.IsNullOrEmpty(Request.QueryString["action"]))
        {
            string act = Request.QueryString["action"];
            if (act == "new") linkTambah_Click(sender, e);
        }

        this.Page.Form.Enctype = "multipart/form-data";

        loadData();
        //gridData.Width = Unit.Percentage(100);
    }
    protected void ProposalPenelitian()
    {
        ddlAddProgress.DataSource = lib.CallProcedure("stp_GetNamaPropPenelitian", new string[] { Context.User.Identity.Name });
        ddlAddProgress.DataTextField = "prp_judul";
        ddlAddProgress.DataValueField = "prp_id";
        ddlAddProgress.DataBind();
        ddlAddProgress.Items.Insert(0, new ListItem("--- Proposal Penelitian ---", ""));
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

            tempLinkBerkas = "LaporanAkhir_" + lib.Encrypt(FileUpload1.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + FileUpload1.FileName.Split('.')[FileUpload1.FileName.Split('.').Length - 1];
            FileUpload1.SaveAs(appPath + saveDir + tempLinkBerkas);
            fileSavePath = Server.MapPath("Uploaded");
            fileSavePath = fileSavePath + "//" + tempLinkBerkas;

            FileInfo fileInfo = new FileInfo(fileSavePath);

            try
            {
                lib.CallProcedure("stp_create_LaporanAkhir", new String[] {
                        Server.HtmlEncode(ddlAddProgress.SelectedValue.Trim()),
                        Server.HtmlEncode(tempLinkBerkas),
                        Server.HtmlEncode(FileUpload1.FileBytes.Length.ToString()),
                        Server.HtmlEncode(fileSavePath.Trim()),
                        fileExtension,
                        Context.User.Identity.Name});
                btnCancelAdd_Click(sender, e);
                loadData();
                master.showAlertSuccess("Berhasil Menambah Laporan Akhir");
            }
            catch (Exception ex)
            {
                master.showAlertDanger(ex.ToString());
            }






        }
        else if (FileUpload1.PostedFile == null || FileUpload1.PostedFile.ContentLength == 0)
        {
            //showAlertDanger("File belum diunggah, unggah file PDF.");
        }
        else if (FileUpload1.PostedFile != null && FileUpload1.PostedFile.ContentLength > 0 && fileExtension != ".pdf")
        {
            //showAlertDanger("Format file tidak valid." +
            // " Unggah file PDF.");
        }
    }
    
    
    

    protected void loadData()
    {
        gridData.DataSource = lib.CallProcedure("stp_getLaporanAKhir", new String[] { Context.User.Identity.Name });
        gridData.DataBind();
        //if (tglMulai.Text == String.Empty || tglSelesai.Text == String.Empty)
        //{
        //    gridData.DataSource = lib.CallProcedure("stp_getProposalsByOwner", new String[] { Context.User.Identity.Name, ddStatus.SelectedValue, Server.HtmlEncode(txtCari.Text.Trim()) });
        //    gridData.DataBind();
        //}
        //else
        //{
        //    gridData.DataSource = lib.CallProcedure("stp_getProposalsByDate", new String[] { Context.User.Identity.Name, ddStatus.SelectedValue, Server.HtmlEncode(tglMulai.Text.Trim()), Server.HtmlEncode(tglSelesai.Text.Trim()) });
        //    gridData.DataBind();
        //}

        //master.hideAlert();
        //GridView1.DataSource = lib.CallProcedure("stp_getProposalsByOwner", new String[] { Context.User.Identity.Name });
        //GridView1.DataBind();
    }

    public void fillDgAnggota(String id)
    {

        SqlCommand insertCmd = new SqlCommand("stp_getProposalownerAnggota", connection);
        insertCmd.CommandType = CommandType.StoredProcedure;

        insertCmd.Parameters.AddWithValue("@p1", id);

        connection.Open();
        insertCmd.ExecuteNonQuery();
        connection.Close();

        SqlDataAdapter dataAdapter = new SqlDataAdapter(insertCmd);
        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);
        grdAnggotaProposal.DataSource = dt;
        grdAnggotaProposal.DataBind();

    }

    protected void linkTambah_Click(object sender, EventArgs e)
    {
        string act = Request.QueryString["action"];
        if (act != "new") Response.Redirect("Page_Trans_LaporanAkhir?action=new");
        else
        {
            master.setTitle("Tambah Laporan AKhir");
            master.hideAlert();
            panelData.Visible = false;
            panelAdd.Visible = true;
            panelEdit.Visible = false;
        }
    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        string act = Request.QueryString["action"];
        if (act == "new") Response.Redirect("Page_Trans_LaporanAkhir");
        else
        {
            master.setTitle("Laporan Akhir Saya");
            master.hideAlert();
            panelData.Visible = true;
            panelAdd.Visible = false;
            panelEdit.Visible = false;

        }
    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
       
    }


    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        master.hideAlert();
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
        editJudul.Text = "";
        editJenisID.SelectedIndex = 0;
        editAbstrak.Text = "";
        editKeyword.Text = "";
    }

    protected void btnSubmitEdit_Click(object sender, EventArgs e)
    {
        string[] anggota = idAnggota;
        if (editMemberDL1.SelectedValue == "")
        {
            master.showAlertDanger("Anggota 1 tidak boleh kosong ");
        }
        else
        {

            try
            {
                DataTable data = lib.CallProcedure("stp_getProposalAttachment", new string[] { editPrpID.Text });
                string[] param = new string[8];
                param[0] = Server.HtmlEncode(editPrpID.Text);
                param[1] = Context.User.Identity.Name;
                param[2] = Server.HtmlEncode(editJudul.Text);
                param[3] = Server.HtmlEncode(editJenisID.SelectedValue);
                param[4] = Server.HtmlEncode(editAbstrak.Text);
                param[5] = Server.HtmlEncode(editKeyword.Text);
                param[6] = "";
                //JANGAN LUPA DIGANTI KALO ATCNYA UDAH JADI
                param[7] = Server.HtmlEncode(data.Rows[0][0].ToString());
                lib.CallProcedure("stp_editProposal", param);
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (editMemberDL1.SelectedValue != idAnggota1.Text)
                            {
                                if (editMemberDL1.SelectedValue != "")
                                {
                                    if(idAnggota1.Text=="Anggota")
                                    {
                                        lib.CallProcedure("stp_addProposalMember", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL1.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota1", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL1.SelectedValue });

                                    }
                                    else
                                    {
                                        lib.CallProcedure("stp_editProposalownerAnggota", new String[] { Server.HtmlEncode(editPrpID.Text),idnya1.Text, editMemberDL1.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota1", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL1.SelectedValue });
                                    }
                                    
                                }
                                else
                                    lib.CallProcedure("stp_edit_proposalAnggota1", new String[] { Server.HtmlEncode(editPrpID.Text), null });
                                lib.CallProcedure("stp_DeleteownerAnggotaProposal", new string[] { idAnggota1.Text, Server.HtmlEncode(editPrpID.Text) });

                            }
                            break;
                        case 1:
                            if (editMemberDL2.SelectedValue != idAnggota2.Text)
                            {
                                if (editMemberDL2.SelectedValue != "")
                                {
                                    if (idAnggota2.Text == "Anggota")
                                    {
                                        lib.CallProcedure("stp_addProposalMember", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL2.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota2", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL2.SelectedValue });

                                    }
                                    else
                                    {
                                        lib.CallProcedure("stp_editProposalownerAnggota", new String[] { Server.HtmlEncode(editPrpID.Text), idnya2.Text, editMemberDL2.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota2", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL2.SelectedValue });
                                    }
                                }
                            else
                                {
                                    lib.CallProcedure("stp_edit_proposalAnggota2", new String[] { Server.HtmlEncode(editPrpID.Text), null });
                                    lib.CallProcedure("stp_DeleteownerAnggotaProposal", new string[] { idAnggota2.Text, Server.HtmlEncode(editPrpID.Text) });
                                }
                                

                            }
                            break;
                        case 2:
                            if (editMemberDL3.SelectedValue != idAnggota3.Text)
                            {
                                if (editMemberDL3.SelectedValue != "") {
                                    if (idAnggota3.Text == "Anggota")
                                    {
                                        lib.CallProcedure("stp_addProposalMember", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL3.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota3", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL3.SelectedValue });

                                    }
                                    else
                                    {
                                        lib.CallProcedure("stp_editProposalownerAnggota", new String[] { Server.HtmlEncode(editPrpID.Text), idnya3.Text, editMemberDL3.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota3", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL3.SelectedValue });
                                    }
                                }
                                else
                                {
                                    lib.CallProcedure("stp_edit_proposalAnggota3", new String[] { Server.HtmlEncode(editPrpID.Text), null });
                                    lib.CallProcedure("stp_DeleteownerAnggotaProposal", new string[] { idAnggota3.Text, Server.HtmlEncode(editPrpID.Text) });
                                }
                                    


                            }
                            break;
                        case 3:
                            if (editMemberDL4.SelectedValue != idAnggota4.Text)
                            {
                                if (editMemberDL4.SelectedValue != "")
                                {
                                    if (idAnggota4.Text == "Anggota")
                                    {
                                        lib.CallProcedure("stp_addProposalMember", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL4.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota4", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL4.SelectedValue });
                                    }
                                    else
                                    {
                                        lib.CallProcedure("stp_editProposalownerAnggota", new String[] { Server.HtmlEncode(editPrpID.Text), idnya4.Text, editMemberDL4.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota4", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL4.SelectedValue });
                                    }
                                }
                                else
                                {
                                    lib.CallProcedure("stp_edit_proposalAnggota4", new String[] { Server.HtmlEncode(editPrpID.Text), null });
                                lib.CallProcedure("stp_DeleteownerAnggotaProposal", new string[] { idAnggota4.Text, Server.HtmlEncode(editPrpID.Text) });
                                }

                            }
                            break;
                    }
                    btnCancelEdit_Click(sender, e);
                    loadData();
                    master.showAlertSuccess("Edit Proposal Berhasil");
                }
            }
            catch (Exception ex)
            {
                master.showAlertDanger(ex.Message + " " + ex.StackTrace);
            }
        }

    }

    protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridData.PageIndex = e.NewPageIndex;
        loadData();
    }

    protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
       
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib.CallProcedure("stp_deleteProposal", new string[] { txtConfirmDelete.Text, Context.User.Identity.Name });
        loadData();
        master.showAlertSuccess("Hapus Proposal berhasil");
    }

    //protected void gridData_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        TableCell abstraks = e.Row.Cells[2]; // sebelum ditambahkan nomor proposal index = 2
    //        TableCell status = e.Row.Cells[4]; // sebelum ditambahkan nomor proposal index = 4
    //        abstraks.Text = abstraks.Text.Length > 30 ? abstraks.Text.Substring(0, 30) + "..." : abstraks.Text;
    //        if (status.Text == "1")
    //        {
    //            status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Diajukan</span>");
    //        }

    //        else if (status.Text == "2")
    //        {
    //            status.Text = HttpUtility.HtmlDecode("<span class='label label-danger'>Ditolak</span>");

    //        }
    //        else if (status.Text == "3")
    //        {
    //            status.Text = HttpUtility.HtmlDecode("<span class='label label-success'>Diterima</span>");

    //        }
    //        else
    //        {
    //            status.Text = HttpUtility.HtmlDecode("<span class='label label-warning' title='Menunggu konfirmasi anggota'>Konfirmasi</span>");
    //        }

    //    }
    //}

   
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
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    public void WriteExcel(DataTable dt, String extension)
    {
        IWorkbook workbook;

        if (extension == "xlsx")
        {
            workbook = new XSSFWorkbook();
        }
        else if (extension == "xls")
        {
            workbook = new HSSFWorkbook();
        }
        else
        {
            throw new Exception("This format is not supported");
        }

        ISheet sheet1 = workbook.CreateSheet("Sheet 1");

        //make a header row
        IRow row1 = sheet1.CreateRow(0);

        for (int j = 0; j < dt.Columns.Count; j++)
        {

            ICell cell = row1.CreateCell(j);
            String columnName = dt.Columns[j].ToString();
            cell.SetCellValue(columnName);
        }

        //loops through data
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            IRow row = sheet1.CreateRow(i + 1);
            for (int j = 0; j < dt.Columns.Count; j++)
            {

                ICell cell = row.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(dt.Rows[i][columnName].ToString());
            }
        }

        using (var exportData = new MemoryStream())
        {
            workbook.Write(exportData);
            if (extension == "xlsx") //xlsx file format
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ContactNPOI.xlsx"));
                Response.BinaryWrite(exportData.ToArray());
            }
            else if (extension == "xls")  //xls file format
            {
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ContactNPOI.xls"));
                Response.BinaryWrite(exportData.GetBuffer());
            }
            Response.End();
        }
    }

    private int WordCount(string s)
    {
        int c = 0;
        string[] split = { " " };


        string[] list = s.Split(split, StringSplitOptions.RemoveEmptyEntries);

        char[] ch = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};


        string val = string.Empty;

        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].IndexOfAny(ch) >= 0) c++;
        }

        return c;
    }

    public string ProcessNomorProposal(object myValue)
    {
        if (Convert.ToInt32(myValue) < 10)
            return "000";
        else if (Convert.ToInt32(myValue) < 100)
            return "00";
        else if (Convert.ToInt32(myValue) < 1000)
            return "0";
        else
            return "";

    }
    public string ProcessNomorProposalString(string myValue)
    {
        if (Convert.ToInt32(myValue) < 10)
            return "000";
        else if (Convert.ToInt32(myValue) < 100)
            return "00";
        else if (Convert.ToInt32(myValue) < 1000)
            return "0";
        else
            return "";

    }

    protected void grdAnggotaProposal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            grdAnggotaProposal.Columns[2].Visible = false;
            TableCell nama = e.Row.Cells[0];
            TableCell status = e.Row.Cells[1];
            //   TableCell conf = e.Row.Cells[2];

            if (status.Text == "0") status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Belum konfirmasi</span>");
            else if (status.Text == "1") status.Text = HttpUtility.HtmlDecode("<span class='label label-success'>Sudah Konfirmasi</span>");

            // else status.Text = HttpUtility.HtmlDecode("<span class='label label-warning'>Leader</span>");
        }
    }

        
    protected void editMemberDL1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void editMemberDL2_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void editMemberDL3_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void editMemberDL4_SelectedIndexChanged(object sender, EventArgs e)
    {

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
        return lib.CallProcedure("stp_getProposalsByOwner", new String[] { Context.User.Identity.Name, ddStatus.SelectedValue, Server.HtmlEncode(txtCari.Text.Trim()) });
    }
    protected void ExportExcel(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("Select DISTINCT ROW_NUMBER() over (order by A.prp_updated_date desc) as rownum, A.prp_id as prp_id, A.prp_judul as judul, A.atc_id , B.atc_name ,A.prp_created_by as buatby, CONVERT(varchar(12), A.prp_updated_date, 113) as creadate, CONVERT(varchar(12), A.prp_submit, 113) as submitdate, CONVERT(varchar(12), A.prp_approve, 113) as approveddate, (case A.prp_status when 2 then 'Diajukan' when 4 then 'Ditolak' when 3 then 'Diterima' end) as status, A.prp_nomor as prp_nomor FROM TRPROPOSAL AS A JOIN TRATTACHMENT AS B ON A.atc_id = B.atc_id where A.prp_status = 2"))
            {
                using (SqlDataAdapter sda = new SqlDataAdapter())
                {
                    cmd.Connection = con;
                    sda.SelectCommand = cmd;
                    using (DataTable dt = new DataTable())
                    {
                        sda.Fill(dt);
                        using (XLWorkbook wb = new XLWorkbook())
                        {
                            wb.Worksheets.Add(dt, "Customers");

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx");
                            using (MemoryStream MyMemoryStream = new MemoryStream())
                            {
                                wb.SaveAs(MyMemoryStream);
                                MyMemoryStream.WriteTo(Response.OutputStream);
                                Response.Flush();
                                Response.End();
                            }
                        }
                    }
                }
            }
        }
    }
    

}

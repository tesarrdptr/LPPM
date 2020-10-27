using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.IO;

using ClosedXML.Excel;

using System.Web.Configuration;

public partial class Page_Trans_AuditProposal : System.Web.UI.Page
{
    System.Data.SqlClient.SqlConnection connection =
new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);

    PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    DataTable dt1 = new DataTable();
    private string conn = ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString();
    private const string ASCENDING = " ASC ";
    private const string DESCENDING = " DESC ";
    String idProp;
    String aa, bb;
    String query = "Select  ROW_NUMBER() over(order by A.prp_updated_date desc) as No, A.prp_nomor as [No. Proposal] ,A.prp_judul as [Judul Proposal], A.prp_created_by as [Ketua Pengusul], ap.anggota1 AS [Anggota 1], ap.anggota2 AS [Anggota 2], ap.anggota3 AS [Anggota 3],ap.anggota4 AS [Anggota 4], A.prp_abstrak as [Abstrak], A.prp_keyword as Keyword, A.prp_total_rab as [Total Dana],  CONVERT(varchar(12), A.prp_submit, 113) as [Tanggal Diajukan], (case A.prp_status when 2 then 'Belum direview' when 4 then 'Ditolak' when 3 then 'Diterima' end) as Status FROM TRPROPOSAL AS A JOIN TRATTACHMENT AS B ON A.atc_id = B.atc_id JOIN MSANGGOTAPROPOSAL ap on AP.prp_id = A.prp_id WHERE A.prp_status = 2";


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
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        gridData.Width = Unit.Percentage(100);
    }

    protected void loadData()
    {
        panelDetail.Visible = false;

        if (txtCari.Text != "")
        {
            tglMulai.Text = null;
            tglSelesai.Text = null;
            query = "Select  ROW_NUMBER() over(order by A.prp_updated_date desc) as No, A.prp_nomor as [No. Proposal] ,A.prp_judul as [Judul Proposal], A.prp_created_by as [Ketua Pengusul], ap.anggota1 AS [Anggota 1], ap.anggota2 AS [Anggota 2], ap.anggota3 AS [Anggota 3],ap.anggota4 AS [Anggota 4], A.prp_abstrak as [Abstrak], A.prp_keyword as Keyword, A.prp_total_rab as [Total Dana],  CONVERT(varchar(12), A.prp_submit, 113) as [Tanggal Diajukan], (case A.prp_status when 2 then 'Belum direview' when 4 then 'Ditolak' when 3 then 'Diterima' end) as Status FROM TRPROPOSAL AS A JOIN TRATTACHMENT AS B ON A.atc_id = B.atc_id JOIN MSANGGOTAPROPOSAL ap on AP.prp_id = A.prp_id WHERE (A.prp_judul like '%" + txtCari.Text + "%' OR A.prp_created_by like '%" + txtCari.Text + "%') AND A.prp_status = 2";

        } 

        if (tglMulai.Text == String.Empty || tglSelesai.Text == String.Empty)
        {
            gridData.DataSource = lib.CallProcedure("stp_GetProgressProposal", new string[] { Server.HtmlEncode(txtCari.Text.Trim()),Context.User.Identity.Name });
            gridData.DataBind();
           
        }
        else
        {
            gridData.DataSource = lib.CallProcedure("stp_searchproposalnilaibydate", new string[] { Server.HtmlEncode(tglMulai.Text), Server.HtmlEncode(tglSelesai.Text) });
            gridData.DataBind();


            query = "Select ROW_NUMBER() over(order by A.prp_updated_date desc) as No, A.prp_nomor as [No. Proposal] ,A.prp_judul as [Judul Proposal], A.prp_created_by as [Ketua Pengusul],ap.anggota1 AS [Anggota 1], ap.anggota2 AS [Anggota 2], ap.anggota3 AS [Anggota 3],ap.anggota4 AS [Anggota 4],A.prp_abstrak as [Abstrak], A.prp_keyword as Keyword, A.prp_total_rab as [Total Dana],  CONVERT(varchar(12), A.prp_submit, 113) as [Tanggal Diajukan], (case A.prp_status when 2 then 'Belum direview' when 4 then 'Ditolak' when 3 then 'Diterima' end) as Status FROM TRPROPOSAL AS A JOIN TRATTACHMENT AS B ON A.atc_id = B.atc_id JOIN MSANGGOTAPROPOSAL AP on AP.prp_id = A.prp_id where((CONVERT(date,A.prp_submit) >= CONVERT(date,'" + tglMulai.Text + "') AND CONVERT(date,A.prp_submit) <=  CONVERT(date,'" + tglSelesai.Text + "')) OR   (CONVERT(date, A.prp_approve) >= CONVERT(date, '" + tglMulai.Text + "') AND CONVERT(date, A.prp_approve) <= CONVERT(date, '" + tglSelesai.Text + "'))) AND A.prp_status = 2";

        }
    }

    protected void ExportExcel(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand(query))
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



    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gridData.EditIndex = e.NewEditIndex;
        this.loadData();
    }

    protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            if (e.CommandName == "lihat")
            {
                int ind = Convert.ToInt32(e.CommandArgument);
                connection.Open();
                SqlCommand sqlCmd = new SqlCommand("stp_getDataProposalByNomor", connection);
                sqlCmd.Parameters.AddWithValue("@p1", gridData.Rows[ind].Cells[2].Text);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                // SqlDataReader reader = sqlCmd.ExecuteReader();
                SqlDataReader dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    int idprp = Convert.ToInt32(dr["id"].ToString());
                    idProp = (dr["id"].ToString());
                }
                connection.Close();

                List<string> mylist = JSONDataAll(idProp);
                int o, i;
                string title = "Nama Anggota";
                int no = 1;
                for (i = 0; i < mylist.Count; i++)
                {

                    aa += "<tr><td>" + no + "</td>" + "<td>" + mylist[i] + "</td></tr>";
                    no++;
                }
                string body = "<style>table, th, td {border: 0px solid black;}</style><table " + "style=width:100%" + "><tr><th>No</th><th>Nama Anggota</th></tr>" + aa + "</table>";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);


            }
            else
            if (e.CommandName == "ReviewProposal")
            {
                String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
                dt = lib.CallProcedure("stp_getDataBobotMonev", new string[] { id });
                //beriNilai.InnerHtml = "";
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    String input = "<div class=\"form-group\" style=\"margin-bottom: -10px; \">" +
                        " <label class=\"control-label\">" + dt.Rows[i][0] + "</label> " +
                        " <label Style=\"color: red; \"> *</label> " +
                        " <input type=\"text\" name=\"bobot\" class=\"form-control\" maxlength=\"2\" /> " +
                        " <input type='hidden' name='bobotID' value='" + dt.Rows[i][1] + "'/>" +
                        " </div><br/><br/>";
                    //beriNilai.InnerHtml += input;
                }
                //panelNilai.Visible = true;
                panelData.Visible = false;
                //editKode.Text = id;
                //ed.Text = dt.Rows[0][1].ToString();
                //panelAdd.Visible = false;
                //panelEdit.Visible = true;
                //panelData.Visible = false;
            }
            else if (e.CommandName == "download")
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
            else if (e.CommandName == "Detail")
            {
                Title = "Detail Proposal";
                int i = Convert.ToInt32(e.CommandArgument);
                connection.Open();
                SqlCommand sqlCmd = new SqlCommand("stp_getDataProposalByNomor", connection);
                sqlCmd.Parameters.AddWithValue("@p1", gridData.Rows[i].Cells[2].Text);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                // SqlDataReader reader = sqlCmd.ExecuteReader();
                SqlDataReader dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
                panelDetail.Visible = true;
                panelData.Visible = false;

                while (dr.Read())
                {
                    int idprp = Convert.ToInt32(dr["id"].ToString());
                    idProp = (dr["id"].ToString());
                    txtJudul.Text = dr["judul"].ToString();
                    txtKeyword.Text = dr["keyword"].ToString();
                    txtDana.Text = "Rp " + dr["dana"].ToString();
                    txtAbstrak.Text = dr["abstrak"].ToString();
                    txtNoProposal.Text = gridData.Rows[i].Cells[2].Text;
                    tgldiajukan.Text = dr["submitdate"].ToString();

                    txtBidangFokus.Text = dr["bidfok_nama"].ToString();

                }

                connection.Close();
               fillDgAnggota(idProp);


            }

        }
    }

    protected void Unnamed_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelDetail.Visible = false;
        loadData();
    }

    public void fillDgAnggota(String idProp)
    {
        SqlCommand insertCmd = new SqlCommand("stp_getOwnerProposalById", connection);
        insertCmd.CommandType = CommandType.StoredProcedure;

        insertCmd.Parameters.AddWithValue("@p1", idProp);

        connection.Open();
        insertCmd.ExecuteNonQuery();
        connection.Close();

        SqlDataAdapter dataAdapter = new SqlDataAdapter(insertCmd);
        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);
        grdAnggotaProposal.DataSource = dt;
        grdAnggotaProposal.DataBind();
    }



    public List<String> JSONDataAll(String id)
    {
        List<String> Users = new List<String>();
        string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            con.Open();
            using (SqlCommand cmd = new SqlCommand("stp_getProposalownerAnggota", con))
            {
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p1", id);
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        Users.Add(reader.GetString(1)); //Specify column index 
                    }
                }
            }
        }
        return Users;
    }

    protected void linkTambah_Click(object sender, EventArgs e)
    {
        panelData.Visible = false;
    }

    protected void btnCari_Click(object sender, EventArgs e)
    {
        loadData();
    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
    }

    protected void gridData_PageIndexChanged(object sender, EventArgs e)
    {
        //string constr = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        //SqlConnection con = new SqlConnection(constr);
        //con.Open();
        //SqlCommand com = new SqlCommand("select * from  TRATTACHMENT where atc_name=@id", con);
        //com.Parameters.AddWithValue("@id", gridData.SelectedRow.Cells[2].Text);
        //SqlDataReader dr = com.ExecuteReader();

        //if (dr.Read())
        //{
        //    string fileName = dr["atc_name"].ToString();
        //    string fileLength = dr["atc_size"].ToString();
        //    string filePath = dr["atc_path"].ToString();
        //    if (File.Exists(filePath))
        //    {
        //        Response.Clear();
        //        Response.BufferOutput = false;
        //        Response.ContentType = dr["atc_contentType"].ToString();
        //        Response.AddHeader("Content-Length", fileLength);
        //        Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
        //        Response.TransmitFile(filePath);
        //        Response.Flush();
        //    }
        //    else
        //    {
        //        //  showAlertDanger("Error: File not found!");
        //    }
        //}
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

    protected void btnCancelMonev_Click(object sender, EventArgs e)
    {
        //panelNilai.Visible = false;
        panelData.Visible = true;
    }
    public class bobot
    {
        public string id;
        public string name;
        public string percent;
        public bobot(string _id, string _name, string _percent)
        {
            id = _id;
            name = _name;
            percent = _percent + "%";
        }
    }

    public List<bobot> getBobots()
    {
        List<bobot> bobots = new List<bobot>();
        var data = lib.CallProcedure("stp_getBobots", new String[] { });
        for (int i = 0; i < data.Rows.Count; i++)
        {
            bobots.Add(new bobot(
                data.Rows[i][0].ToString(),
                data.Rows[i][1].ToString(),
                data.Rows[i][2].ToString()
            ));
        }
        return bobots;
    }

    protected void linkConfirmInsert_Click(object sender, EventArgs e)
    {
        //String[] bobots = Request.Form["bobot"].ToString().Split(',');
        //String[] idbobot = Request.Form["bobotID"].ToString().Split(',');
        //string nama = "arif";
        //string progress = "Lihat Nilai Progress";

        //if (bobots != null && idbobot != null)
        //{
        //    for (int i = 0; i < idbobot.Count(); i++)
        //    {
        //        lib.CallProcedure("stp_create_monev", new string[] { editKode.Text, idbobot[i], bobots[i], Context.User.Identity.Name });
        //    }
        //    lib.CallProcedure("stp_create_notifikasi", new string[] { Context.User.Identity.Name, nama, progress });

        //    btnCancelMonev_Click(sender, e);
        //    loadData();
        //    showAlertSuccess("Tambah Nilai Berhasil");
        //}
        //else
        //{
        //    showAlertDanger("Textbox tidak boleh kosong dan harus angka");
        //}
    }
    public static string readableSize(string size)
    {
        string[] sizes = { "B", "KB", "MB", "GB" };
        double len = Convert.ToDouble(size);
        int order = 0;
        while (len >= 1024 && ++order < sizes.Length) len = len / 1024;
        return String.Format("{0:0.##} {1}", len, sizes[order]);
    }
    protected void confirmGrade_Click(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
        SqlConnection con = new SqlConnection(constr);
        con.Open();
        SqlCommand com = new SqlCommand("select std_score from  MSSTANDARSCORE", con);
        //com.Parameters.AddWithValue("@id", gridData.SelectedRow.Cells[2].Text);
        SqlDataReader dr = com.ExecuteReader();

        if (dr.Read())
        {
            String score = dr["std_score"].ToString();
            int scr = int.Parse(score);
        }

        List<String> bobot_key = new List<String>();

        String[] param = new String[3];
        param[0] = txtID.Text;
        param[1] = Context.User.Identity.Name;
        param[2] = komentar.Text;
        //param[3] = aksi.SelectedValue;
        var dt = lib.CallProcedure("stp_newGrade", param);

        string grd_id = dt.Rows[0][0].ToString();

        foreach (var post in HttpContext.Current.Request.Form)
        {
            if (post.ToString().Contains("bobot"))
            {
                var a = post.ToString().Split('_');
                bobot_key.Add(a[1]);
            }

        }
        foreach (string bob_id in bobot_key)
        {

            lib.CallProcedure("stp_newScore", new String[] {
                        grd_id,
                        bob_id,
                        HttpContext.Current.Request.Form["bobot_" + bob_id]
                    });
        }
        dt = lib.CallProcedure("stp_getDataStandarNilai", new string[] { });
        dt1 = lib.CallProcedure("stp_getDataRataRataNilai", new string[] { grd_id });

        string nilairata = dt1.Rows[0][0].ToString();
        string nilaistandart = dt.Rows[0][1].ToString();

        if (Int16.Parse(nilairata) >= Int16.Parse(nilaistandart))
        {
            lib.CallProcedure("stp_editStatusProposal", new string[] { txtID.Text, Context.User.Identity.Name, "3" });
        }
        else if (Int16.Parse(nilairata) < Int16.Parse(nilaistandart))
        {
            lib.CallProcedure("stp_editStatusProposal", new string[] { txtID.Text, Context.User.Identity.Name, "4" });
        }

        Response.Redirect("Page_Trans_AuditProposal");
    }

    protected void gridData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //TableCell abstraks = e.Row.Cells[2];
            TableCell status = e.Row.Cells[4];
            //abstraks.Text = abstraks.Text.Length > 30 ? abstraks.Text.Substring(0, 30) + "..." : abstraks.Text;
            if (status.Text == "Belum Tereview")
            {
                status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Menunggu Konfirmasi</span>");
            }
            //else if (status.Text == "-1") status.Text = HttpUtility.HtmlDecode("<span class='label label-danger'>Ditolak</span>");
            //else
            //{
            //    status.Text = HttpUtility.HtmlDecode("<span class='label label-warning' title='Menunggu konfirmasi anggota'>Konfirmasi</span>");
            //}

        }
    }

    public SortDirection GridViewSortDirection
    {
        get
        {
            if (ViewState["sortDirection"] == null)
                ViewState["sortDirection"] = SortDirection.Ascending;
            return (SortDirection)ViewState["sortDirection"];
        }
        set
        {
            ViewState["sortDirection"] = value;
        }
    }

    private DataSet display()
    {
        SqlCommand cmd = new SqlCommand("Select DISTINCT ROW_NUMBER() over (order by A.prp_updated_date desc) as rownum, A.prp_id as prp_id, A.prp_judul as judul, A.atc_id , B.atc_name ,A.prp_created_by as buatby, CONVERT(varchar(12), A.prp_updated_date, 113) as creadate, CONVERT(varchar(12), A.prp_submit, 113) as submitdate, CONVERT(varchar(12), A.prp_approved, 113) as approveddate, (case A.prp_status when 2 then 'Belum direview' when 4 then 'Ditolak' when 3 then 'Diterima' end) as status, A.prp_nomor as prp_nomor FROM TRPROPOSAL AS A JOIN TRATTACHMENT AS B ON A.atc_id = B.atc_id where A.prp_status = 2", connection);
        DataSet ds = new DataSet();
        SqlDataAdapter adp = new SqlDataAdapter(cmd);
        adp.Fill(ds);

        gridData.DataSource = ds;
        gridData.DataBind();
        return ds;
    }


    private void SortGridView(string sortexpression, string direction)
    {
        DataTable dt = display().Tables[0];

        DataView dv = new DataView(dt);
        dv.Sort = sortexpression + direction;

        gridData.DataSource = dv;
        gridData.DataBind();
    }

    protected void gridData_Sorting(object sender, GridViewSortEventArgs e)
    {
        string sortExpression = e.SortExpression;

        if (GridViewSortDirection == SortDirection.Ascending)
        {
            GridViewSortDirection = SortDirection.Descending;
            SortGridView(sortExpression, DESCENDING);
        }
        else
        {
            GridViewSortDirection = SortDirection.Ascending;
            SortGridView(sortExpression, ASCENDING);
        }
    }
}
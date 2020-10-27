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
using SelectPdf;
using System.Web.Configuration;
using System.Configuration;
using ClosedXML.Excel;


public partial class Page_daftarProgressPengabdianReviewer : System.Web.UI.Page
{
    System.Data.SqlClient.SqlConnection connection =
new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);

    PolmanAstraLibrary.PolmanAstraLibrary lib2 = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    String idProp;
    String aa, bb;
    String query = "Select ROW_NUMBER() over(order by A.prp_nomor) as No, A.prp_nomor as Nomor ,A.prp_judul as [Judul Proposal], A.prp_created_by as [Ketua Pengusul],ap.anggota1 AS [Anggota 1], ap.anggota2 AS [Anggota 2], ap.anggota3 AS [Anggota 3],ap.anggota4 AS [Anggota 4], A.prp_abstrak as [Abstrak], A.prp_keyword as Keyword, A.prp_total_rab as [Total Dana], CONVERT(varchar(12), prg.prg_submit, 113) as [Tanggal Diajukan], CONVERT(varchar(12), prg.prg_approve, 113) as [Tanggal Dinilai], prg.prg_totalscore as [Nilai Akhir] ,prg.prg_komentar , (case prg.prg_status when 2 then 'Belum Direview' when 4 then 'Ditolak' when 3 then 'Diterima' end) as Status FROM TRPROGRESS AS PRG JOIN TRATTACHMENT AS B ON PRG.atc_id = B.atc_id JOIN TRPROPOSAL A ON PRG.prp_id = A.prp_id JOIN MSBIDANGFOKUS bidfok on bidfok.bidfok_id = A.bidfok_id JOIN MSANGGOTAPROPOSAL AP on AP.prp_id = A.prp_id where A.jns_id = 2 AND PRG.prg_status>2";
    private const string ASCENDING = " ASC ";
    private const string DESCENDING = " DESC ";
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddStatus.Items.Add(new ListItem("--- Semua ---", "ALL"));
            ddStatus.Items.Add(new ListItem("Ditolak", "4"));
            ddStatus.Items.Add(new ListItem("Diterima", "3"));
        }
        loadData();
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        gridData.Width = Unit.Percentage(100);
    }

    protected void loadData()
    {
        panelDetail.Visible = false;

        if (ddStatus.SelectedValue != "ALL")
        {
            tglMulai.Text = null;
            tglSelesai.Text = null;
            txtCari.Text = null;
            query = "Select ROW_NUMBER() over(order by A.prp_nomor) as No, A.prp_nomor as Nomor ,A.prp_judul as [Judul Proposal], A.prp_created_by as [Ketua Pengusul],ap.anggota1 AS [Anggota 1], ap.anggota2 AS [Anggota 2], ap.anggota3 AS [Anggota 3],ap.anggota4 AS [Anggota 4], A.prp_abstrak as [Abstrak], A.prp_keyword as Keyword, A.prp_total_rab as [Total Dana], CONVERT(varchar(12), prg.prg_submit, 113) as [Tanggal Diajukan], CONVERT(varchar(12), prg.prg_approve, 113) as [Tanggal Dinilai], prg.prg_totalscore as [Nilai Akhir] ,prg.prg_komentar , (case prg.prg_status when 2 then 'Belum Direview' when 4 then 'Ditolak' when 3 then 'Diterima' end) as Status FROM TRPROGRESS AS PRG JOIN TRATTACHMENT AS B ON PRG.atc_id = B.atc_id JOIN TRPROPOSAL A ON PRG.prp_id = A.prp_id JOIN MSBIDANGFOKUS bidfok on bidfok.bidfok_id = A.bidfok_id JOIN MSANGGOTAPROPOSAL AP on AP.prp_id = A.prp_id where A.jns_id = 2 AND PRG.prg_status = " + ddStatus.SelectedValue;
        }

        if (txtCari.Text != "")
        {
            tglMulai.Text = null;
            tglSelesai.Text = null;
            ddStatus.ClearSelection();
            query = "Select ROW_NUMBER() over(order by A.prp_nomor) as No, A.prp_nomor as Nomor ,A.prp_judul as [Judul Proposal], A.prp_created_by as [Ketua Pengusul],ap.anggota1 AS [Anggota 1], ap.anggota2 AS [Anggota 2], ap.anggota3 AS [Anggota 3],ap.anggota4 AS [Anggota 4], A.prp_abstrak as [Abstrak], A.prp_keyword as Keyword, A.prp_total_rab as [Total Dana], CONVERT(varchar(12), prg.prg_submit, 113) as [Tanggal Diajukan], CONVERT(varchar(12), prg.prg_approve, 113) as [Tanggal Dinilai], prg.prg_totalscore as [Nilai Akhir] ,prg.prg_komentar , (case prg.prg_status when 2 then 'Belum Direview' when 4 then 'Ditolak' when 3 then 'Diterima' end) as Status FROM TRPROGRESS AS PRG JOIN TRATTACHMENT AS B ON PRG.atc_id = B.atc_id JOIN TRPROPOSAL A ON PRG.prp_id = A.prp_id JOIN MSBIDANGFOKUS bidfok on bidfok.bidfok_id = A.bidfok_id JOIN MSANGGOTAPROPOSAL AP on AP.prp_id = A.prp_id where A.jns_id = 2 AND PRG.prg_status>2 and (A.prp_judul like '%" + txtCari.Text + "%' OR A.prp_created_by like '%" + txtCari.Text + "%') AND A.prp_status > 2";

        }

        if (tglMulai.Text == String.Empty || tglSelesai.Text == String.Empty)
        {
            gridData.DataSource = lib2.CallProcedure("stp_GetProgressPengabdianScored1", new string[] { Server.HtmlEncode(txtCari.Text.Trim()), ddStatus.SelectedValue });
            gridData.DataBind();

            // txtCari.Text = null;
            //  ddStatus.ClearSelection();

        }
        else
        {
            gridData.DataSource = lib2.CallProcedure("stp_searchnilaiprogresspengabdianbydate", new string[] { Server.HtmlEncode(tglMulai.Text), Server.HtmlEncode(tglSelesai.Text) });
            gridData.DataBind();
            query = "Select ROW_NUMBER() over(order by A.prp_nomor) as No, A.prp_nomor as Nomor ,A.prp_judul as [Judul Proposal], A.prp_created_by as [Ketua Pengusul],ap.anggota1 AS [Anggota 1], ap.anggota2 AS [Anggota 2], ap.anggota3 AS [Anggota 3],ap.anggota4 AS [Anggota 4], A.prp_abstrak as [Abstrak], A.prp_keyword as Keyword, A.prp_total_rab as [Total Dana], CONVERT(varchar(12), prg.prg_submit, 113) as [Tanggal Diajukan], CONVERT(varchar(12), prg.prg_approve, 113) as [Tanggal Dinilai], prg.prg_totalscore as [Nilai Akhir] ,prg.prg_komentar , (case prg.prg_status when 2 then 'Belum Direview' when 4 then 'Ditolak' when 3 then 'Diterima' end) as Status FROM TRPROGRESS AS PRG JOIN TRATTACHMENT AS B ON PRG.atc_id = B.atc_id JOIN TRPROPOSAL A ON PRG.prp_id = A.prp_id JOIN MSBIDANGFOKUS bidfok on bidfok.bidfok_id = A.bidfok_id JOIN MSANGGOTAPROPOSAL AP on AP.prp_id = A.prp_id where A.jns_id = 2 AND PRG.prg_status>2 AND ((CONVERT(date,PRG.PRG_SUBMIT) >= CONVERT(date,'" + tglMulai.Text + "') AND CONVERT(date,PRG.PRG_SUBMIT) <=  CONVERT(date,'" + tglSelesai.Text + "')) OR   (CONVERT(date, PRG.PRG_approve) >= CONVERT(date, '" + tglMulai.Text + "') AND CONVERT(date, PRG.PRG_approve) <= CONVERT(date, '" + tglSelesai.Text + "'))) AND A.prp_status > 2";

            //   ddStatus.ClearSelection();

        }
    }
    protected void Unnamed_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelDetail.Visible = false;
        loadData();
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
        tglSelesai.Text = "";
        tglMulai.Text = "";
        txtCari.Text = "";
        ddStatus.ClearSelection();
    }


    protected void OnRowEditing(object sender, GridViewEditEventArgs e)
    {
        gridData.EditIndex = e.NewEditIndex;
        this.loadData();
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

    public void fillDataTextBox(int id)
    {
        connection.Open();
        SqlCommand sqlCmd = new SqlCommand("stp_GetDataPenilaianMonev", connection);
        sqlCmd.Parameters.AddWithValue("@p1", id);
        sqlCmd.CommandType = CommandType.StoredProcedure;
        // SqlDataReader reader = sqlCmd.ExecuteReader();
        SqlDataReader dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
        panelDetail.Visible = true;
        panelData.Visible = false;

        while (dr.Read())
        {
            txtKomentar.Text = dr["komen"].ToString();
            txtNilaiAkhir.Text = dr["totalnilai"].ToString();
            txtStatus.Text = dr["status"].ToString();
            txtStandarNilai.Text = dr["standar"].ToString();
            tglDinilai.Text = dr["approveddate"].ToString();

        }

        connection.Close();

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

    public void fillDgNilai(int id)
    {
        SqlCommand insertCmd = new SqlCommand("stp_GetDataPenilaianMonev", connection);
        insertCmd.CommandType = CommandType.StoredProcedure;

        insertCmd.Parameters.AddWithValue("@p1", id);

        connection.Open();
        insertCmd.ExecuteNonQuery();
        connection.Close();

        SqlDataAdapter dataAdapter = new SqlDataAdapter(insertCmd);
        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);
        grdNilai.DataSource = dt;
        grdNilai.DataBind();
    }


    protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String idprg = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();

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
                string body = "<style >table, th, td {border: 0px solid black;}</style><table " + "style=width:100%" + "><tr><th>No</th><th>Nama Anggota</th></tr>" + aa + "</table>";
                ClientScript.RegisterStartupScript(this.GetType(), "Popup", "ShowPopup('" + title + "', '" + body + "');", true);


            }
            else
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
                fillDataTextBox(Convert.ToInt32(idprg));
                fillDgAnggota(idProp);
                fillDgNilai(Convert.ToInt32(idprg));
            }

        }
    }


    protected void btnCari_Click(object sender, EventArgs e)
    {
        loadData();
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

    protected void gridData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            //TableCell abstraks = e.Row.Cells[2];
            TableCell status = e.Row.Cells[4];
            //abstraks.Text = abstraks.Text.Length > 30 ? abstraks.Text.Substring(0, 30) + "..." : abstraks.Text;
            //if (status.Text == "Menunggu Konfirmasi")
            //{
            //    status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Menunggu Konfirmasi</span>");
            //}

            if (status.Text == "Menunggu Konfirmasi")
            {
                status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Menunggu Konfirmasi</span>");
            }

            if (status.Text == "Ditolak")
            {
                status.Text = HttpUtility.HtmlDecode("<span class='label label-danger'>Ditolak</span>");

            }
            else if (status.Text == "Diterima")
            {
                status.Text = HttpUtility.HtmlDecode("<span class='label label-success'>Diterima</span>");

            }
            else if (status.Text == "Diajukan")
            {
                status.Text = HttpUtility.HtmlDecode("<span class='label label-warning'>Belum Direview</span>");
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
        SqlCommand cmd = new SqlCommand("Select prg.prg_id , A.prp_id, B.atc_name, ROW_NUMBER() over(order by A.prp_nomor) as No, A.prp_nomor as Nomor ,A.prp_judul as [Judul Proposal], A.prp_created_by as [Ketua Pengusul],ap.anggota1 AS [Anggota 1], ap.anggota2 AS [Anggota 2], ap.anggota3 AS [Anggota 3],ap.anggota4 AS [Anggota 4], A.prp_abstrak as [Abstrak], A.prp_keyword as Keyword, A.prp_total_rab as [Total Dana], CONVERT(varchar(12), prg.prg_submit, 113) as [Tanggal Diajukan], CONVERT(varchar(12), prg.prg_approve, 113) as [Tanggal Dinilai], prg.prg_totalscore as [Nilai Akhir] ,prg.prg_komentar , (case prg.prg_status when 2 then 'Belum Direview' when 4 then 'Ditolak' when 3 then 'Diterima' end) as Status FROM TRPROGRESS AS PRG JOIN TRATTACHMENT AS B ON PRG.atc_id = B.atc_id JOIN TRPROPOSAL A ON PRG.prp_id = A.prp_id JOIN MSBIDANGFOKUS bidfok on bidfok.bidfok_id = A.bidfok_id JOIN MSANGGOTAPROPOSAL AP on AP.prp_id = A.prp_id where A.jns_id != 2 AND PRG.prg_status>2", connection);
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
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

public partial class Page_daftarPublikasiReviewer : System.Web.UI.Page
{
    System.Data.SqlClient.SqlConnection connection =
new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);
    String query = "SELECT ROW_NUMBER() over (order by A.pub_updated_date desc) as [No.],A.pub_title as [Judul Publikasi] , A.pub_creator as [Dosen] ,B.jpb_title as [Jenis Publikasi], CONVERT(varchar(12), A.pub_updated_date, 113) as [Tanggal Dinilai], A.pub_komentar as [Komentar],(case A.pub_status when 1 then 'Ditolak' when 2 then 'Diterima' end) as [Status] FROM TRPUBLIKASI AS A JOIN MSJENISPUBLIKASI AS B ON A.jpb_id = B.jpb_id JOIN TRATTACHMENT AS C ON A.atc_id = C.atc_id where A.pub_status = 1 or A.pub_status= 2";

    PolmanAstraLibrary.PolmanAstraLibrary lib2 = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            ddStatus.Items.Add(new ListItem("--- Semua ---", "ALL"));
            ddStatus.Items.Add(new ListItem("Ditolak", "1"));
            ddStatus.Items.Add(new ListItem("Diterima", "2"));
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
            query = "SELECT ROW_NUMBER() over (order by A.pub_updated_date desc) as [No.],A.pub_title as [Judul Publikasi] , A.pub_creator as [Dosen] ,B.jpb_title as [Jenis Publikasi], CONVERT(varchar(12), A.pub_updated_date, 113) as [Tanggal Dinilai], A.pub_komentar as [Komentar],(case A.pub_status when 1 then 'Ditolak' when 2 then 'Diterima' end) as [Status] FROM TRPUBLIKASI AS A JOIN MSJENISPUBLIKASI AS B ON A.jpb_id = B.jpb_id JOIN TRATTACHMENT AS C ON A.atc_id = C.atc_id where A.pub_status = " + ddStatus.SelectedValue;
        }

        if (txtCari.Text != "")
        {
            tglMulai.Text = null;
            tglSelesai.Text = null;
            ddStatus.ClearSelection();
            query = "SELECT ROW_NUMBER() over (order by A.pub_updated_date desc) as [No.],A.pub_title as [Judul Publikasi] , A.pub_creator as [Dosen] ,B.jpb_title as [Jenis Publikasi], CONVERT(varchar(12), A.pub_updated_date, 113) as [Tanggal Dinilai], A.pub_komentar as [Komentar],(case A.pub_status when 1 then 'Ditolak' when 2 then 'Diterima' end) as [Status] FROM TRPUBLIKASI AS A JOIN MSJENISPUBLIKASI AS B ON A.jpb_id = B.jpb_id JOIN TRATTACHMENT AS C ON A.atc_id = C.atc_id  WHERE (A.pub_title like '%" + txtCari.Text + "%' OR B.jpb_title like '%" + txtCari.Text + "%' OR A.pub_creator like '%" + txtCari.Text + "%' ) AND (A.pub_status = 1 or A.pub_status= 2 )";

        }

        if (tglMulai.Text == String.Empty || tglSelesai.Text == String.Empty)
        {
            gridData.DataSource = lib2.CallProcedure("stp_GetDaftarPublikasiReviewer1", new string[] { Server.HtmlEncode(txtCari.Text.Trim()), ddStatus.SelectedValue});
            gridData.DataBind();

            // txtCari.Text = null;
            //  ddStatus.ClearSelection();

        }
        else
        {
            gridData.DataSource = lib2.CallProcedure("stp_searchpublikasibydate", new string[] { Server.HtmlEncode(tglMulai.Text), Server.HtmlEncode(tglSelesai.Text) });
            gridData.DataBind();
            query = "SELECT ROW_NUMBER() over (order by A.pub_updated_date desc) as [No.],A.pub_title as [Judul Publikasi] , A.pub_creator as [Dosen] ,B.jpb_title as [Jenis Publikasi], CONVERT(varchar(12), A.pub_updated_date, 113) as [Tanggal Dinilai], A.pub_komentar as [Komentar],(case A.pub_status when 1 then 'Ditolak' when 2 then 'Diterima' end) as [Status] FROM TRPUBLIKASI AS A JOIN MSJENISPUBLIKASI AS B ON A.jpb_id = B.jpb_id JOIN TRATTACHMENT AS C ON A.atc_id = C.atc_id  where ((CONVERT(date,A.pub_updated_date) >= CONVERT(date,'" + tglMulai.Text + "') AND CONVERT(date,A.pub_updated_date) <=  CONVERT(date,'" + tglSelesai.Text + "'))) AND (A.pub_status = 1 or A.pub_status= 2 )";

            //   ddStatus.ClearSelection();

        }


        panelDetail.Visible = false;
    }

    protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
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
                //dt = lib2.CallProcedure("stp_TerimaPublikasi", new String[] { id });
                //showAlertSuccess("Publikasi telah diterima");
            }
            else if (e.CommandName == "Detail")
            {
                panelDetail.Visible = true;
                panelData.Visible = false;
                Title = "Detail Penelitian";
                int i = Convert.ToInt32(e.CommandArgument);


                dt = lib2.CallProcedure("stp_getdataPublikasibyid", new String[] { id });
                txtJenisPublikasi.Text = dt.Rows[0][3].ToString();
                txtJudulPublikasi.Text = dt.Rows[0][1].ToString();
                txtKomentar.Text = dt.Rows[0][0].ToString();
                txtStatus.Text = dt.Rows[0][7].ToString();
                txtTahunPublikasi.Text = dt.Rows[0][8].ToString();
                tgldiajukan.Text = dt.Rows[0][6].ToString();

            }
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
        tglSelesai.Text = "";
        tglMulai.Text = "";
        txtCari.Text = "";
        ddStatus.ClearSelection();
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

    protected void Unnamed_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelDetail.Visible = false;
        loadData();
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
}
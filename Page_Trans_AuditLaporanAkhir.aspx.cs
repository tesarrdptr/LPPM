using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ProposalPolmanAstra.Classes;
using System.Web.Configuration;
using ClosedXML.Excel;


public partial class Page_Trans_AuditLaporanAkhir : System.Web.UI.Page
{
    System.Data.SqlClient.SqlConnection connection =
new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);
    String query = "SELECT ROW_NUMBER() over (order by A.laporan_created_date desc) as No , B.prp_judul as [Judul], A.laporan_created_by AS [Dosen Pembuat] ,CONVERT(varchar(12), A.laporan_created_date, 113) as [Tanggal Diajukan] FROM trlaporanakhir AS A JOIN TRPROPOSAL AS B ON B.prp_id= A.prp_id JOIN TRATTACHMENT AS C ON A.atc_id = C.atc_id where A.laporan_komentar IS NULL";
    SiteMaster master;
    PolmanAstraLibrary.PolmanAstraLibrary lib2 = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    MailFunction mail = new MailFunction();
    //LDAPAuthentication adAuth = new LDAPAuthentication();

    protected void Page_Load(object sender, EventArgs e)
    {
        loadData();
        hideAlert();
        Page.Form.Attributes.Add("enctype", "multipart/form-data");
        gridData.Width = Unit.Percentage(100);
    }

    protected void loadData()
    {
        if (txtCari.Text != "")
        {
            tglMulai.Text = null;
            tglSelesai.Text = null;
            query = "SELECT ROW_NUMBER() over (order by A.laporan_created_date desc) as No , B.prp_judul as [Judul], A.laporan_created_by AS [Dosen Pembuat] ,CONVERT(varchar(12), A.laporan_created_date, 113) as [Tanggal Diajukan] FROM trlaporanakhir AS A JOIN TRPROPOSAL AS B ON B.prp_id= A.prp_id JOIN TRATTACHMENT AS C ON A.atc_id = C.atc_id where A.laporan_komentar IS NULL AND (A.laporan_created_by like '%" + txtCari.Text + "%' OR B.prp_judul like '%" + txtCari.Text + "%' OR A.pub_creator like '%" + txtCari.Text + "%' )";

        }

        if (tglMulai.Text == String.Empty || tglSelesai.Text == String.Empty)
        {
            gridData.DataSource = lib2.CallProcedure("stp_GetPengajuanLaporanAkhir", new string[] { Server.HtmlEncode(txtCari.Text.Trim())});
            gridData.DataBind();

            // txtCari.Text = null;
            //  ddStatus.ClearSelection();

        }
        else
        {
            gridData.DataSource = lib2.CallProcedure("stp_searchlaporanakhirbydate", new string[] { Server.HtmlEncode(tglMulai.Text), Server.HtmlEncode(tglSelesai.Text) });
            gridData.DataBind();
            query = "SELECT ROW_NUMBER() over (order by A.laporan_created_date desc) as No , B.prp_judul as [Judul], A.laporan_created_by AS [Dosen Pembuat] ,CONVERT(varchar(12), A.laporan_created_date, 113) as [Tanggal Diajukan] FROM trlaporanakhir AS A JOIN TRPROPOSAL AS B ON B.prp_id= A.prp_id JOIN TRATTACHMENT AS C ON A.atc_id = C.atc_id where A.laporan_komentar IS NULL AND ((CONVERT(date,A.laporan_created_date) >= CONVERT(date,'" + tglMulai.Text + "') AND CONVERT(date,A.laporan_created_date) <=  CONVERT(date,'" + tglSelesai.Text + "'))) ";

            //   ddStatus.ClearSelection();

        }
    }

    protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridData.PageIndex = e.NewPageIndex;
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
    }


    protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            if (e.CommandName == "lnkDownload")
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
        }
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib2.CallProcedure("stp_TolakPublikasi", new string[] { txtConfirmDelete.Text, Context.User.Identity.Name, TxtTolak.Text });
        dt = master.lib.CallProcedure("stp_detailPublikasi", new String[] { txtConfirmDelete.Text });

        String htmlBody = "";

        ////GENERATE MAIL BODY HERE
        //htmlBody += "Dear Bapak/Ibu " + adAuth.GetDisplayName(dt.Rows[0][1].ToString()) + ",<br><br>";
        //htmlBody += "Pengajuan publikasi Anda dengan judul " + dt.Rows[0][0].ToString() + " Ditolak.<br><br>";
        //htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_Trans_PublikasiSaya.aspx'>[LIHAT DETAIL]</a></b><br><br>";
        //htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
        //htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
        ////END GENERATE MAIL BODY HERE

        //mail.SendMail(
        //    "[SISFO LPPM] - Pengajuan Publikasi Anda dengan judul " + dt.Rows[0][0].ToString() + " Ditolak",
        //    adAuth.GetMail(dt.Rows[0][1].ToString()),
        //    htmlBody
        //);

        loadData();
        showAlertSuccess("Publikasi telah ditolak");
    }

    protected void linkConfirmTerima_Click(object sender, EventArgs e)
    {
        lib2.CallProcedure("stp_TerimaLaporanAKhir", new string[] { txtConfirmTerima.Text, Context.User.Identity.Name, TxtTerima.Text });
     //   dt = lib2.CallProcedure("stp_detailPublikasi", new String[] { txtConfirmTerima.Text });
        //try
        //{
        //    String htmlBody = "";

        //    //GENERATE MAIL BODY HERE
        //    htmlBody += "Dear Bapak/Ibu " + adAuth.GetDisplayName(dt.Rows[0][1].ToString()) + ",<br><br>";
        //    htmlBody += "Pengajuan publikasi Anda dengan judul " + dt.Rows[0][0].ToString() + " Diterima.<br><br>";
        //    htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_Trans_PublikasiSaya.aspx'>[LIHAT DETAIL]</a></b><br><br>";
        //    htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
        //    htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
        //    //END GENERATE MAIL BODY HERE

        //    mail.SendMail(
        //        "[SISFO LPPM] - Pengajuan Publikasi Anda dengan judul " + dt.Rows[0][0].ToString() + " Diterima",
        //        adAuth.GetMail(dt.Rows[0][1].ToString()),
        //        htmlBody
        //    );
        //}
        //catch { }
        loadData();
        showAlertSuccess("Publikasi telah diterima");
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

    protected void linkTambah_Click(object sender, EventArgs e)
    {

        panelData.Visible = false;

    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
    }


    protected void btnCari_Click(object sender, EventArgs e)
    {
        loadData();
    }
}
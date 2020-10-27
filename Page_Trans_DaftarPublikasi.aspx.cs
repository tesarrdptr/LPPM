using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using SelectPdf;



    public partial class Page_Trans_DaftarPublikasi : System.Web.UI.Page
    {
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
            gridData.DataSource = lib2.CallProcedure("stp_GetDaftarPublikasi", new string[] { Server.HtmlEncode(txtCari.Text.Trim()), ddStatus.SelectedValue });
            gridData.DataBind();
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
                    //dt = lib2.CallProcedure("stp_TerimaPublikasi", new String[] { id });
                    //showAlertSuccess("Publikasi telah diterima");
                }
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

        protected void linkCetakLaporan_Click(object sender, EventArgs e)
        {
            panelData.Visible = false;
            panelLaporan.Visible = true;
        }

        protected void btnCetak_Click(object sender, EventArgs e)
        {
            dt = lib2.CallProcedure("stp_GetReportDaftarPublikasi", new string[] { tglMulai.Text, tglSelesai.Text });
            WritePdf(dt);
        }

        public void WritePdf(DataTable dt, String file = "")
        {
            String html = "<table class='table'><thead><tr>";


            for (int j = 0; j < dt.Columns.Count; j++)
            {
                String columnName = dt.Columns[j].ToString();
                html += String.Format(@"<th>{0}</th>", columnName);
            }
            html += @"</tr></thead><tbody>";

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                html += "<tr>";
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    String columnName = dt.Columns[j].ToString();
                    String data = dt.Rows[i][columnName].ToString();
                    html += String.Format(@"<td>{0}</td>", data);
                }
                html += @"</tr>";
            }
            html += @"</tbody></table>";
            String baseHtml = @"<!DOCTYPE html><html><head><link href='http://10.5.0.123/Proposal/Content/bootstrap.min.css' rel='stylesheet' />
                        <script src='http://10.5.0.123/Proposal/Scripts/jquery.min.js' 
                        type='text/javascript'></script><script src='http://10.5.0.123/Proposal/Scripts/bootstrap.min.js' 
                        type='text/javascript'></script></head><body><div  style='padding-left:50px;padding-right:50px;padding-top:100px'>
                        <h1><center><b>{0}</b></center></h1><br/><hr/>{1}</div></body></html>";
            using (var exportData = new MemoryStream())
            {
                HtmlToPdf converter = new HtmlToPdf();
                PdfDocument doc = converter.ConvertHtmlString(String.Format(baseHtml, "Laporan Daftar Publikasi", html), "");
                if (file == "")
                    doc.Save(Response, false, "Laporan_Publikasi.pdf");
                else
                    doc.Save(HttpContext.Current.Request.PhysicalApplicationPath + @"\\PdfJs\\" + file);
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            panelData.Visible = true;
            panelLaporan.Visible = false;
        }

        protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridData.PageIndex = e.NewPageIndex;
            loadData();
        }

        protected void gridData_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell status = e.Row.Cells[4];
                //abstraks.Text = abstraks.Text.Length > 30 ? abstraks.Text.Substring(0, 30) + "..." : abstraks.Text;
                //if (status.Text == "1")
                //{
                //    status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Diajukan</span>");
                //}

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
    }
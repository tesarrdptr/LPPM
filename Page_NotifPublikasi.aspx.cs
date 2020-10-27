using System;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.IO;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using ProposalPolmanAstra.Classes;
using System.Web.Configuration;


    public partial class Page_NotifPublikasi : System.Web.UI.Page
    {

        PolmanAstraLibrary.PolmanAstraLibrary lib2 = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
        SiteMaster master;
        DataTable dt = new DataTable();
        MailFunction mail = new MailFunction();
        LDAPAuthentication adAuth = new LDAPAuthentication();

        protected void Page_Load(object sender, EventArgs e)
        {
            loadData();
            hideAlert();
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            gridData.Width = Unit.Percentage(100);
        }

        protected void loadData()
        {

            gridData.DataSource = lib2.CallProcedure("stp_GetPengajuanPublikasi", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
            gridData.DataBind();
        }

        protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridData.PageIndex = e.NewPageIndex;
            loadData();
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
            lib2.CallProcedure("stp_TolakPublikasi", new string[] { txtConfirmDelete.Text, Context.User.Identity.Name });
            dt = master.lib.CallProcedure("stp_detailPublikasi", new String[] { txtConfirmDelete.Text });

            String htmlBody = "";

            //GENERATE MAIL BODY HERE
            htmlBody += "Dear Bapak/Ibu " + adAuth.GetDisplayName(dt.Rows[0][1].ToString()) + ",<br><br>";
            htmlBody += "Pengajuan publikasi Anda dengan judul " + dt.Rows[0][0].ToString() + " Ditolak.<br><br>";
            htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_Trans_PublikasiSaya.aspx'>[LIHAT DETAIL]</a></b><br><br>";
            htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
            htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
            //END GENERATE MAIL BODY HERE

            mail.SendMail(
                "[SISFO LPPM] - Pengajuan Publikasi Anda dengan judul " + dt.Rows[0][0].ToString() + " Ditolak",
                adAuth.GetMail(dt.Rows[0][1].ToString()),
                htmlBody
            );

            loadData();
            showAlertSuccess("Publikasi telah ditolak");
        }

        protected void linkConfirmTerima_Click(object sender, EventArgs e)
        {
            lib2.CallProcedure("stp_TerimaPublikasi", new string[] { txtConfirmTerima.Text, Context.User.Identity.Name });
            dt = lib2.CallProcedure("stp_detailPublikasi", new String[] { txtConfirmTerima.Text });

            try
            {
                String htmlBody = "";

                //GENERATE MAIL BODY HERE
                htmlBody += "Dear Bapak/Ibu " + adAuth.GetDisplayName(dt.Rows[0][1].ToString()) + ",<br><br>";
                htmlBody += "Pengajuan publikasi Anda dengan judul " + dt.Rows[0][0].ToString() + " Diterima.<br><br>";
                htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_Trans_PublikasiSaya.aspx'>[LIHAT DETAIL]</a></b><br><br>";
                htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
                htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
                //END GENERATE MAIL BODY HERE

                mail.SendMail(
                    "[SISFO LPPM] - Pengajuan Publikasi Anda dengan judul " + dt.Rows[0][0].ToString() + " Diterima",
                    adAuth.GetMail(dt.Rows[0][1].ToString()),
                    htmlBody
                );
            }
            catch { }
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
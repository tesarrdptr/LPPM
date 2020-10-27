using System;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Web.Security;
using System.IO;
using ProposalPolmanAstra.Classes;
using System.Web.Configuration;


    public partial class Page_NotifProgress : System.Web.UI.Page
    {
        PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
        DataTable dt = new DataTable();
        MailFunction mail = new MailFunction();
        LDAPAuthentication adAuth = new LDAPAuthentication();

        private string conn = ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString();

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
                if (!IsPostBack)
                {
                    ddStatus.Items.Add(new ListItem("--- Semua ---", "ALL"));
                    ddStatus.Items.Add(new ListItem("Penelitian", "1"));
                    ddStatus.Items.Add(new ListItem("Pengabdian", "2"));
                }
            }
            loadData();
            hideAlert();
            Page.Form.Attributes.Add("enctype", "multipart/form-data");
            gridData.Width = Unit.Percentage(100);
        }

        protected void loadData()
        {
            gridData.DataSource = lib.CallProcedure("stp_GetProgressNotif", new string[] { Server.HtmlEncode(txtCari.Text.Trim()), ddStatus.SelectedValue });
            gridData.DataBind();
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
                if (e.CommandName == "Nilai")
                {
                    String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
                    dt = lib.CallProcedure("stp_getDataBobotMonev", new string[] { id });
                    beriNilai.InnerHtml = "";
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        String input = "<div class=\"form-group\" style=\"margin-bottom: -10px; \">" +
                            " <label class=\"control-label\">" + dt.Rows[i][0] + "</label> " +
                            " <label Style=\"color: red; \"> *</label> " +
                            " <input type=\"text\" name=\"bobot\" class=\"form-control\"  /> " +
                            " <input type='hidden' name='bobotID' value='" + dt.Rows[i][1] + "'/>" +
                            " </div><br/><br/>";
                        beriNilai.InnerHtml += input;
                    }
                    panelNilai.Visible = true;
                    panelData.Visible = false;
                    editKode.Text = id;
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
            string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand com = new SqlCommand("select * from  TRATTACHMENT where atc_name=@id", con);
            com.Parameters.AddWithValue("@id", gridData.SelectedRow.Cells[2].Text);
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
                }
            }
        }

        protected void btnCancelMonev_Click(object sender, EventArgs e)
        {
            panelNilai.Visible = false;
            panelData.Visible = true;
        }

        protected void linkConfirmInsert_Click(object sender, EventArgs e)
        {
            String[] bobots = Request.Form["bobot"].ToString().Split(',');
            String[] idbobot = Request.Form["bobotID"].ToString().Split(',');

            if (bobots != null && idbobot != null)
            {
                for (int i = 0; i < idbobot.Count(); i++)
                {
                    lib.CallProcedure("stp_create_monev", new string[] { editKode.Text, idbobot[i], bobots[i], Context.User.Identity.Name });
                }

                dt = lib.CallProcedure("stp_detailProgress", new String[] { editKode.Text });
                if (dt.Rows[0][2].ToString().Equals("1"))
                {
                    try
                    {
                        String htmlBody = "";

                        //GENERATE MAIL BODY HERE
                        htmlBody += "Dear Bapak/Ibu " + adAuth.GetDisplayName(dt.Rows[0][1].ToString()) + ",<br><br>";
                        htmlBody += "Pengajuan progress penelitian Anda dengan judul " + dt.Rows[0][0].ToString() + " telah direview.<br><br>";
                        htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_Trans_ProgressPenelitian.aspx'>[LIHAT DETAIL]</a></b><br><br>";
                        htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
                        htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
                        //END GENERATE MAIL BODY HERE

                        mail.SendMail(
                            "[SISFO LPPM] - Pengajuan Progress Penelitian dengan Judul " + dt.Rows[0][0].ToString() + " Telah Direview",
                            adAuth.GetMail(dt.Rows[0][1].ToString()),
                            htmlBody
                        );
                    }
                    catch { }
                }
                else if (dt.Rows[0][2].ToString().Equals("2"))
                {
                    try
                    {
                        String htmlBody = "";

                        //GENERATE MAIL BODY HERE
                        htmlBody += "Dear Bapak/Ibu " + adAuth.GetDisplayName(dt.Rows[0][1].ToString()) + ",<br><br>";
                        htmlBody += "Pengajuan progress pengabdian Anda dengan judul " + dt.Rows[0][0].ToString() + " telah direview.<br><br>";
                        htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_Trans_ProgressPengabdian.aspx'>[LIHAT DETAIL]</a></b><br><br>";
                        htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
                        htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
                        //END GENERATE MAIL BODY HERE

                        mail.SendMail(
                            "[SISFO LPPM] - Pengajuan Progress Pengabdian dengan Judul " + dt.Rows[0][0].ToString() + " Telah Direview",
                            adAuth.GetMail(dt.Rows[0][1].ToString()),
                            htmlBody
                        );
                    }
                    catch { }
                }

                btnCancelMonev_Click(sender, e);
                loadData();
                showAlertSuccess("Tambah Nilai Berhasil");
            }
            else
            {
                showAlertDanger("Textbox tidak boleh kosong dan harus angka");
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
                TableCell status = e.Row.Cells[5];

                if (status.Text == "Ditolak")
                {
                    status.Text = HttpUtility.HtmlDecode("<span class='label label-danger'>Ditolak</span>");

                }
                else if (status.Text == "Diterima")
                {
                    status.Text = HttpUtility.HtmlDecode("<span class='label label-success'>Diterima</span>");

                }
                else if (status.Text == "Menunggu Konfirmasi")
                {
                    status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Menunggu Konfirmasi</span>");
                }
            }
        }

        protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gridData.PageIndex = e.NewPageIndex;
            loadData();
        }
    }
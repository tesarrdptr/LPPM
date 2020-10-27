using System;
using System.Web;
using System.Web.Security;
using System.Web.UI.WebControls;
using ProposalPolmanAstra.Classes;
using System.Data;
using System.Web.Configuration;


public partial class Page_MemberProposal : System.Web.UI.Page
{
    SiteMaster master;
    MailFunction mail = new MailFunction();
    DataTable dt, dtBidfok, DtJenis, dtAtc;
    // LDAPAuthentication adAuth = new LDAPAuthentication();

    protected void Page_Load(object sender, EventArgs e)
    {
        
        FormsIdentity id = (FormsIdentity)Context.User.Identity;
        FormsAuthenticationTicket ticket = id.Ticket;
        if (String.IsNullOrEmpty(Request.QueryString["id"])) Response.Redirect("Page_KelolaProposal");
        master = this.Master as SiteMaster;
        var data = master.lib.CallProcedure("stp_getProposalOwner", new String[] { Request.QueryString["id"] });
        if (data == null) Response.Redirect("http://localhost/sso");
        else
        {
            gridData.DataSource = data;
            gridData.DataBind();
            gridData.Columns[2].Visible = false;
        }
        string isconfirmed = master.lib.CallProcedure("stp_isConfirmed", new String[] { Request.QueryString["id"], Context.User.Identity.Name }).Rows[0][0].ToString();
        btnConfirm.Visible = isconfirmed == "0";
        load_Proposal();
        master.setTitle("Detail Proposal");
        
    }

    protected void gridData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            TableCell nama = e.Row.Cells[0];
            TableCell status = e.Row.Cells[1];
            TableCell conf = e.Row.Cells[2];
            if (conf.Text == "0") nama.Text = nama.Text + " (Belum konfirmasi)";
            if (status.Text == "0")
            {
                if (conf.Text == "0") status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Anggota</span>");
                else if (conf.Text == "1") status.Text = HttpUtility.HtmlDecode("<span class='label label-success'>Anggota</span>");
            }
            else status.Text = HttpUtility.HtmlDecode("<span class='label label-warning'>Leader</span>");
        }
    }

    protected void btnConfirm_Click(object sender, EventArgs e)
    {
        master.lib.CallProcedure("stp_confirmProposal", new String[] { Request.QueryString["id"], Context.User.Identity.Name });
        var confirmed = master.lib.CallProcedure("stp_isAllConfirmed", new String[] { Request.QueryString["id"] }).Rows[0][0].ToString();
        DataTable dt = new DataTable();
        dt = master.lib.CallProcedure("stp_detailProposal", new String[] { Request.QueryString["id"] });

        if (confirmed == "1")
        {
            master.lib.CallProcedure("stp_editStatusProposal", new String[] {
                    Request.QueryString["id"],
                    "SYSTEM",
                    "2"
                });
            try
            {
                String htmlBody = "";

                //GENERATE MAIL BODY HERE
                htmlBody += "Dear Bapak/Ibu Reviewer,<br><br>";
                htmlBody += "Terdapat pengajuan proposal baru dengan judul " + dt.Rows[0][0].ToString() + ".<br><br>";
                htmlBody += "Silahkan lakukan review pada proposal tersebut.<br><br>";
                htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_LProposalDiajukan.aspx'>[LIHAT DETAIL]</a></b><br><br>";
                htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
                htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
                //END GENERATE MAIL BODY HERE

                mail.SendMail(
                "[SISFO LPPM] - Pengajuan Progress Pengabdian Baru: " + dt.Rows[0][0].ToString(),
                "rachmadrizky9@gmail.com",
                htmlBody
            );
                //);
            }
            catch { }
        }
        master.showAlertSuccess("Terima Kasih Telah Melakukan Konfirmasi");
        load_Proposal();
        Response.Redirect("Page_MemberProposal?id=" + Request.QueryString["id"]);
        master.showAlertSuccess("Terima Kasih Telah Melakukan Konfirmasi");
    }

    protected void load_Proposal()
    {
        
        dt = master.lib.CallProcedure("stp_getProposalByID", new String[] { Request.QueryString["id"] });
        dtBidfok = master.lib.CallProcedure("stp_getBidangFokus", new String[] { Context.User.Identity.Name });
        DtJenis = master.lib.CallProcedure("stp_getJenisProposal", new String[] { });
        dtAtc = master.lib.CallProcedure("stp_getAtcByID", new String[] { dt.Rows[0][12].ToString() });
        for (int i = 0; i < dtBidfok.Rows.Count; i++)
        {
            if(dtBidfok.Rows[i][0].ToString()== dt.Rows[0][10].ToString())
            {
                BidFok.Text = dtBidfok.Rows[0][1].ToString();
                break;
            }
        }
        for (int i = 0; i < DtJenis.Rows.Count; i++)
        {
            if (DtJenis.Rows[i][0].ToString() == dt.Rows[0][8].ToString())
            {
                JenisID.Text = DtJenis.Rows[0][1].ToString();
                break;
            }
        }

        File.Text = dtAtc.Rows[0][1].ToString();
        Judul.Text = dt.Rows[0][1].ToString();
        Abstrak.Text = dt.Rows[0][2].ToString();
        Keyword.Text = dt.Rows[0][3].ToString();
        Dana.Text = "Rp. "+dt.Rows[0][11].ToString();
        if (dt.Rows[0][13].ToString()!="1")
        {
            btnConfirm.Visible = false;
        }
    }

    protected void linkDownload_Click(object sender, EventArgs e)
    {
        string fileName = dtAtc.Rows[0][1].ToString();
        string fileLength = dtAtc.Rows[0][2].ToString();
        string filePath = dtAtc.Rows[0][4].ToString();
        string fileExtension = dtAtc.Rows[0][3].ToString();

            Response.Clear();
            Response.BufferOutput = false;
            Response.ContentType = dtAtc.Rows[0][3].ToString();
            Response.AddHeader("Content-Length", fileLength);
            Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
            Response.TransmitFile(filePath);
            Response.Flush();
        
    }
}
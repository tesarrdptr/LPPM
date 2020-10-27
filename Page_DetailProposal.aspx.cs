using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Web;
using System.Web.Security;
using ProposalPolmanAstra.Classes;
using System.Web.Configuration;


    public partial class Page_DetailProposal : System.Web.UI.Page
    {
        DataTable dt2 = new DataTable();
        DataTable dt1 = new DataTable();
        SiteMaster master;
        MailFunction mail = new MailFunction();
       // LDAPAuthentication adAuth = new LDAPAuthentication();

        protected void Page_Load(object sender, EventArgs e)
        {
            FormsIdentity id = (FormsIdentity)Context.User.Identity;
            FormsAuthenticationTicket ticket = id.Ticket;
            if (!id.IsAuthenticated || ticket.UserData.Split('@')[0].Equals("ROL04")) btnGrade.Visible = false;

            if (String.IsNullOrEmpty(Request.QueryString["id"])) Response.Redirect("/Proposal/");
            master = this.Master as SiteMaster;
            var data = master.lib.CallProcedure("stp_getProposalByID", new String[] { Request.QueryString["id"] });
            if (data == null) Response.Redirect("http://application.polman.astra.ac.id/sso");
            else
            {
                txtTitle.InnerText = data.Rows[0][1].ToString();
                txtAuthor.InnerText = data.Rows[0][4].ToString();
                txtAbstraks.InnerText = data.Rows[0][2].ToString();
                txtKeywords.InnerText = data.Rows[0][3].ToString();
                txtSize.InnerText = readableSize(data.Rows[0][7].ToString());
            }
        }

        public static string readableSize(string size)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = Convert.ToDouble(size);
            int order = 0;
            while (len >= 1024 && ++order < sizes.Length) len = len / 1024;
            return String.Format("{0:0.##} {1}", len, sizes[order]);
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
            var data = master.lib.CallProcedure("stp_getBobots", new String[] { });
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

        protected void btnDownload_Click(object sender, EventArgs e)
        {

        }

        protected void btnDownload_Click1(object sender, EventArgs e)
        {
            string fileId = "";
            string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand com = new SqlCommand("select atc_id from  TRPROPOSAL where prp_id=@id", con);
            com.Parameters.AddWithValue("id", Request.QueryString["id"]);
            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                fileId = dr["atc_id"].ToString();

            }
            con.Close();
            SqlConnection con1 = new SqlConnection(constr);
            con1.Open();
            SqlCommand com1 = new SqlCommand("select * from TRATTACHMENT where atc_id=@id", con1);
            com1.Parameters.AddWithValue("id", fileId);
            SqlDataReader dr1 = com1.ExecuteReader();
            if (dr1.Read())
            {
                string fileName = dr1["atc_name"].ToString();
                string fileLength = dr1["atc_size"].ToString();
                string filePath = dr1["atc_path"].ToString();
                string fileExtension = dr1["atc_contentType"].ToString();
                if (File.Exists(filePath))
                {
                    Response.Clear();
                    Response.BufferOutput = false;
                    Response.ContentType = dr1["atc_contentType"].ToString();
                    Response.AddHeader("Content-Length", fileLength);
                    Response.AddHeader("content-disposition", "attachment; filename=" + fileName);
                    Response.TransmitFile(filePath);
                    Response.Flush();
                }
                else
                {
                    master.showAlertDanger("Gagal: File tidak ditemukan!");
                }
            }
        }

        protected void confirmGrade_Click(object sender, EventArgs e)
        {
            string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
            SqlConnection con = new SqlConnection(constr);
            con.Open();
            SqlCommand com = new SqlCommand("select std_score from  MSSTANDARSCORE", con);
            SqlDataReader dr = com.ExecuteReader();

            if (dr.Read())
            {
                String score = dr["std_score"].ToString();
                int scr = int.Parse(score);
            }

            List<String> bobot_key = new List<String>();

            String[] param = new String[3];
            param[0] = Request.QueryString["id"];
            param[1] = Context.User.Identity.Name;
            param[2] = komentar.Text;
            var dt = master.lib.CallProcedure("stp_newGrade", param);

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

                master.lib.CallProcedure("stp_newScore", new String[] {
                        grd_id,
                        bob_id,
                        HttpContext.Current.Request.Form["bobot_" + bob_id]
                    });
            }
            dt2 = master.lib.CallProcedure("stp_getDataStandarNilai", new string[] { });
            dt1 = master.lib.CallProcedure("stp_getDataRataRataNilai", new string[] { grd_id });

            string nilairata = dt1.Rows[0][0].ToString();
            string nilaistandart = dt2.Rows[0][1].ToString();
            string status = "";
            if (Int16.Parse(nilairata) >= Int16.Parse(nilaistandart))
            {
                master.lib.CallProcedure("stp_editStatusProposal", new string[] { Request.QueryString["id"], Context.User.Identity.Name, "3" });
                status = "Diterima";
            }
            else if (Int16.Parse(nilairata) < Int16.Parse(nilaistandart))
            {
                master.lib.CallProcedure("stp_editStatusProposal", new string[] { Request.QueryString["id"], Context.User.Identity.Name, "2" });
                status = "Ditolak";
            }
            //try
            //{
            //    dt = master.lib.CallProcedure("stp_detailProposal", new string[] { Request.QueryString["id"] });
            //    String htmlBody = "";

            //    //GENERATE MAIL BODY HERE
            //    htmlBody += "Dear Bapak/Ibu " + adAuth.GetDisplayName(dt.Rows[0][1].ToString()) + ",<br><br>";
            //    htmlBody += "Pengajuan Proposal Anda dengan judul " + dt.Rows[0][0].ToString() + " " + status + ".<br><br>";
            //    htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_KelolaProposal.aspx'>[LIHAT DETAIL]</a></b><br><br>";
            //    htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
            //    htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
            //    //END GENERATE MAIL BODY HERE

            //    mail.SendMail(
            //        "[SISFO LPPM] - Pengajuan Proposal Anda dengan judul " + dt.Rows[0][0].ToString() + " " + status,
            //        adAuth.GetMail(dt.Rows[0][1].ToString()),
            //        htmlBody
            //    );
            //}
            //catch { }

            Response.Redirect("Page_LProposalDiajukan");
        }
    }
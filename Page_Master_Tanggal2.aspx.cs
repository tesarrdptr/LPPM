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


public partial class Page_Trans_daftarProgressProposal : System.Web.UI.Page
{
    //PolmanAstraLibrary.PolmanAstraLibrary lib1 = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["DefaultConnection1"].ToString());
    PolmanAstraLibrary.PolmanAstraLibrary lib2 = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    public string conn = ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString();
    System.Data.SqlClient.SqlConnection connection =
    new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);

    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    String query = "select ROW_NUMBER() over (order by tgl_created_date desc) as no, tgl_id,tgl_jenis,convert (varchar(12),tgl_mulai,113) as tgl_mulai, convert(varchar(12),tgl_selesai,113) as tgl_selesai, tgl_created_by, convert(varchar(12),tgl_created_date,113) as tgl_created_date, tgl_updated_by, convert(varchar(12),tgl_updated_date,113) as tgl_updated_date, YEAR(tgl_anggaran) as tgl_anggaran from MSTANGGAL";

    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            Tanggal();
            TanggalEdit();
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
                    if (!Context.User.Identity.IsAuthenticated || lib2.CallProcedure("stp_getMenuByRole", new string[] { ticket.UserData.Split('|')[index].Split('@')[0], ticket.UserData.Split('|')[index].Split('@')[1], Request.Path.Split('/')[Request.Path.Split('/').Length - 1].Replace(".aspx", "") }).Rows.Count == 0)
                    {
                        Response.Redirect("Default");
                    }
                }
            }
            catch
            {
                Response.Redirect("Default");
            }
            int year = DateTime.Now.Year;
            for (int i = year - 10; i <= year + 10; i++)
            {
                ListItem li = new ListItem(i.ToString());
                tglAnggaran.Items.Add(li);
                ddlAnggaranEdit.Items.Add(li);
            }
            tglAnggaran.Items.FindByText(year.ToString()).Selected = true;
            //ddlAnggaranEdit.Items.FindByText(year.ToString()).Selected = true;
            TanggalEdit();
        }
        loadData();
        hideAlert();
        gridData.Width = Unit.Percentage(100);
    }

    protected void loadData()
    {
        if (txtCari.Text != "")
        {
            tglMulaiCari.Text = null;
            tglSelesaiCari.Text = null;
            query = "select ROW_NUMBER() over (order by tgl_created_date desc) as no, tgl_id,tgl_jenis,convert (varchar(12),tgl_mulai,113) as tgl_mulai, convert(varchar(12),tgl_selesai,113) as tgl_selesai, tgl_created_by, convert(varchar(12),tgl_created_date,113) as tgl_created_date, tgl_updated_by, convert(varchar(12),tgl_updated_date,113) as tgl_updated_date, YEAR(tgl_anggaran) as tgl_anggaran from MSTANGGAL";

        } 
        if (tglMulaiCari.Text == String.Empty || tglSelesaiCari.Text == String.Empty)
        {
            gridData.DataSource = lib2.CallProcedure("stp_getTanggal", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
            gridData.DataBind();
        }
        else
        {
            gridData.DataSource = lib2.CallProcedure("stp_searchtanggalbydate", new string[] { Server.HtmlEncode(tglMulaiCari.Text), Server.HtmlEncode(tglSelesaiCari.Text) });
            gridData.DataBind();
            query = "select ROW_NUMBER() over (order by tgl_created_date desc) as no, tgl_id,tgl_jenis,convert (varchar(12),tgl_mulai,113) as tgl_mulai, convert(varchar(12),tgl_selesai,113) as tgl_selesai, tgl_created_by, convert(varchar(12),tgl_created_date,113) as tgl_created_date, tgl_updated_by, convert(varchar(12),tgl_updated_date,113) as tgl_updated_date, YEAR(tgl_anggaran) as tgl_anggaran from MSTANGGAL";

        }

        hideAlert();
    }

    protected void Tanggal()
    {
        ddlTanggal.Items.Add(new ListItem("--- Pilih Jenis Tanggal ---", ""));
        ddlTanggal.Items.Add(new ListItem("Pengajuan Proposal", "Pengajuan Proposal"));
        ddlTanggal.Items.Add(new ListItem("Penilaian Proposal", "Penilaian Proposal"));
        ddlTanggal.Items.Add(new ListItem("Pengajuan Progress", "Pengajuan Progress"));
        ddlTanggal.Items.Add(new ListItem("Penilaian Progress", "Penilaian Progress"));
        ddlTanggal.Items.Add(new ListItem("Pengajuan Laporan Akhir", "Pengajuan Laporan Akhir"));
    }

    protected void TanggalEdit()
    {
        ddlEditTanggal.Items.Add(new ListItem("--- Pilih Jenis Tanggal ---", ""));
        ddlEditTanggal.Items.Add(new ListItem("Pengusulan Proposal", "Pengusulan Proposal"));
        ddlEditTanggal.Items.Add(new ListItem("Penilaian Proposal", "Penilaian Proposal"));
        ddlEditTanggal.Items.Add(new ListItem("Pengajuan Progress", "Pengajuan Progress"));
        ddlEditTanggal.Items.Add(new ListItem("Penilaian Progress", "Penilaian Progress"));
        ddlEditTanggal.Items.Add(new ListItem("Pengajuan Laporan Akhir", "Pengajuan Laporan Akhir"));
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
            String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
            if (e.CommandName == "Ubah")
            {
                panelData.Visible = false;
                panelEdit.Visible = true;
                int ind = Convert.ToInt32(e.CommandArgument);
                connection.Open();
                SqlCommand sqlCmd = new SqlCommand("stp_detailTanggal", connection);
                sqlCmd.Parameters.AddWithValue("@p1", id);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                // SqlDataReader reader = sqlCmd.ExecuteReader();
                SqlDataReader dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);
                //showAlertSuccess("COBA  " + gridData.Rows[ind].Cells[1].Text);
                while (dr.Read())
                {
                    //ddlAnggaranEdit.SelectedValue = Convert.ToDateTime(dr["anggaran"].ToString()).ToString("yyyy");
                    editTanggalMulai.Text = Convert.ToDateTime(dr["mulai"].ToString()).ToString("yyyy-MM-dd");
                    editTanggalSelesai.Text = Convert.ToDateTime(dr["selesai"].ToString()).ToString("yyyy-MM-dd");
                    ddlEditTanggal.SelectedValue = dr["jenis"].ToString();
                    editKode.Text = id;

                }
                connection.Close();

                //dt = lib2.CallProcedure("stp_detailTanggal", new string[] { id });
                //editKode.Text = id;
                ////dtime = Convert.ToDateTime(dt.Rows[0][2].ToString());
                ////dtime2 = Convert.ToDateTime(dt.Rows[0][3].ToString());

                //ddlEditTanggal.SelectedValue = dt.Rows[0][1].ToString();
                //editTanggalMulai.Text = Convert.ToDateTime(id["tgl_mulai"].to
                ////editTanggalMulai.Text = dtime.ToString("yyyy-MM-dd");
                ////editTanggalSelesai.Text = dtime2.ToString("yyyy-MM-dd");
                ////ddlRoleEdit.SelectedValue = dt.Rows[0][2].ToString();
                //panelAdd.Visible = false;
                //panelEdit.Visible = true;
                //panelData.Visible = false;
                ////panelDetil.Visible = false;
            }
            else
            {

            }
        }
    }


    protected void btnCari_Click(object sender, EventArgs e)
    {
        loadData();
    }

    protected void gridData_PageIndexChanged(object sender, GridViewPageEventArgs e)
    {
        gridData.PageIndex = e.NewPageIndex;
        loadData();
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
        panelAdd.Visible = true;
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
        tglMulai.Text = "";
        ddlTanggal.SelectedValue = "";
        tglSelesai.Text = "";
    }

    protected void btnCetak_Click(object sender, EventArgs e)
    {
        //dt = lib2.CallProcedure("stp_GetReportProposal", new string[] { tglMulai.Text, tglSelesai.Text });
        //WritePdf(dt);

        try
        {
            lib2.CallProcedure("stp_createTanggal", new string[] { Server.HtmlEncode(ddlTanggal.SelectedValue),
                tglMulai.Text, tglSelesai.Text, Context.User.Identity.Name,tglAnggaran.Text });
            //btnCancelAdd_Click(sender, e);
            loadData();
            showAlertSuccess("Tambah tanggal berhasil");
            btnCancelEdit_Click(sender, e);
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
        }
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
            PdfDocument doc = converter.ConvertHtmlString(String.Format(baseHtml, "Laporan Daftar Progress Penelitian", html), "");
            if (file == "")
                doc.Save(Response, false, System.DateTime.Now + "Laporan_Proposal.pdf");
            else
                doc.Save(HttpContext.Current.Request.PhysicalApplicationPath + @"\\PdfJs\\" + file);
        }
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
                status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Belum Tereview</span>");
            }
            else if (status.Text == "Belum Di Konfirmasi")
            {
                status.Text = HttpUtility.HtmlDecode("<span class='label label-warning'>Belum Di Konfirmasi</span>");

            }
            //else if (status.Text == "-1") status.Text = HttpUtility.HtmlDecode("<span class='label label-danger'>Ditolak</span>");
            //else
            //{
            //    status.Text = HttpUtility.HtmlDecode("<span class='label label-warning' title='Menunggu konfirmasi anggota'>Konfirmasi</span>");
            //}
        }
    }

    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
        
    }

    protected void btnSubmitEdit_Click(object sender, EventArgs e)
    {
        try
        {

            lib2.CallProcedure("stp_editTanggal", new string[] { editKode.Text,
                ddlEditTanggal.SelectedValue, editTanggalMulai.Text,
                editTanggalSelesai.Text, Context.User.Identity.Name, ddlAnggaranEdit.Text });
            btnCancelEdit_Click(sender, e);
            loadData();
            showAlertSuccess("Ubah tanggal berhasil");
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
        }
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib2.CallProcedure("stp_deleteTanggal", new string[] { txtConfirmDelete.Text });
        loadData();
        showAlertSuccess("Hapus tanggal berhasil");
    }

    protected void tglAnggaran_SelectedIndexChanged(object sender, EventArgs e)
    {
        //Label1.Text += tglAnggaran.SelectedItem.Text;
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
        return lib2.CallProcedure("stp_getTanggal", new String[] { Context.User.Identity.Name, Server.HtmlEncode(txtCari.Text.Trim()) });
    }
}
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
//using ClosedXML.Excel;


public partial class Page_Master_BidangFokus : System.Web.UI.Page
{
    PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    System.Data.SqlClient.SqlConnection connection =
   new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);


    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";

    String query = " select bidfok_id, bidfok_name, bidfok_created_by, convert (varchar(12),bidfok_created_date,113) as bidfok_created_date,bidfok_updated_by, convert (varchar(12),bidfok_updated_date,113) as bidfok_updated_date from MSBIDANGFOKUS where bidfok_delete_status = 0 order by bidfok_id asc";
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!IsPostBack)
        {
            //panelEdit.Visible = true;
            ddlAdd();
            ddlEdit();
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
                    if (!Context.User.Identity.IsAuthenticated || lib.CallProcedure("stp_getJenisPP", new string[] { ticket.UserData.Split('|')[index].Split('@')[0], ticket.UserData.Split('|')[index].Split('@')[1], Request.Path.Split('/')[Request.Path.Split('/').Length - 1].Replace(".aspx", "") }).Rows.Count == 0)
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
        gridData.Width = Unit.Percentage(100);
    }

    protected void loadData()
    {
        if (txtCari.Text != "")
        {
            tglMulaiCari.Text = null;
            tglSelesaiCari.Text = null;
            //query = "SELECT ROW_NUMBER() over (order by bidfok_created_date desc) as no, bidfok_name, CONVERT(varchar(12), bidfok_created_date, 113) as bidfok_created_date, CONVERT(varchar(12), bidfok_updated_date, 113) as bidfok_updated_date, bidfok_created_by, bidfok_updated_by from  MSBIDANGFOKUS";

        }
        if (tglMulaiCari.Text == String.Empty || tglSelesaiCari.Text == String.Empty)
        {
            gridData.DataSource = lib.CallProcedure("stp_getJenisPP", new string[] { Server.HtmlEncode(txtCari.Text.Trim()) });
            gridData.DataBind();
        }
        else
        {
            gridData.DataSource = lib.CallProcedure("stp_searchjenisPPbydate", new string[] { Server.HtmlEncode(tglMulaiCari.Text), Server.HtmlEncode(tglSelesaiCari.Text) });
            gridData.DataBind();

        }

        hideAlert();
    }

    protected void hideAlert()
    {
        divAlert.Visible = false;
        divSuccess.Visible = false;
    }


    protected void linkTambah_Click(object sender, EventArgs e)
    {
        panelData.Visible = false;
        panelAdd.Visible = true;
        panelEdit.Visible = false;
    }

    protected void btnCari_Click(object sender, EventArgs e)
    {
        loadData();
    }

    protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridData.PageIndex = e.NewPageIndex;
        loadData();
    }

    protected void ddlAdd()
    {
        ddlAddJenis.DataSource = lib.CallProcedure("stp_getJenisProposal", new String[] { Context.User.Identity.Name });
        ddlAddJenis.DataValueField = "jns_id";
        ddlAddJenis.DataTextField = "jns_title";
        ddlAddJenis.DataBind();
        ddlAddJenis.Items.Insert(0, new ListItem("--- Pilih Jenis ---", ""));
    }

    protected void ddlEdit()
    {
        ddlEditJenis.DataSource = lib.CallProcedure("stp_getJenisProposal", new String[] { } );
        ddlEditJenis.DataValueField = "jns_id";
        ddlEditJenis.DataTextField = "jns_title";
        ddlEditJenis.DataBind();
        ddlEditJenis.Items.Insert(0, new ListItem("--- Pilih Jenis ---", ""));
    }

    protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        if (e.CommandName != "Page")
        {
            String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
            if (e.CommandName == "Ubah")
            {
                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;
                int ind = Convert.ToInt32(e.CommandArgument);
                connection.Open();
                SqlCommand sqlCmd = new SqlCommand("stp_detailJenisPP", connection);
                sqlCmd.Parameters.AddWithValue("@p1", id);
                sqlCmd.CommandType = CommandType.StoredProcedure;
                SqlDataReader dr = sqlCmd.ExecuteReader(CommandBehavior.CloseConnection);

                while (dr.Read())
                {
                    EditNama.Text = dr["nama"].ToString();
                    EditDana.Text = dr["dana"].ToString();
                    ddlEditJenis.SelectedValue = dr["idjenis"].ToString();
                    editKode.Text = id;

                }
                connection.Close();
            }
            else if (e.CommandName == "Detil")
            {
                //int index = Convert.ToInt32(e.CommandArgument);
                //string tempid = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();

                //dt = lib.CallProcedure("stp_detailBidangFokus", new string[] { id });
                //editKode.Text = id;

                //TextBox1.Text = dt.Rows[0][1].ToString();
                //TextBox4.Text = dt.Rows[0][2].ToString();
                //TextBox5.Text = dt.Rows[0][3].ToString();
                //TextBox6.Text = dt.Rows[0][4].ToString();
                //TextBox7.Text = dt.Rows[0][5].ToString();
                //panelAdd.Visible = false;
                //panelData.Visible = false;
                //panelDetil.Visible = true;
            }
        }
    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
        panelDetil.Visible = false;
        ddlAddJenis.SelectedValue = "";
        AddNama.Text = "";
        addDana.Text = "";
    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
        String[] param = new String[4];
        param[0] = Server.HtmlEncode(ddlAddJenis.SelectedValue.Trim());
        param[1] = Server.HtmlEncode(AddNama.Text.Trim());
        param[2] = Server.HtmlEncode((addDana.Text.Trim()));
        param[3] = Context.User.Identity.Name;

        try
        {
            lib.CallProcedure("stp_createJenisPP", param);
            btnCancelAdd_Click(sender, e);
            loadData();
            showAlertSuccess(param[0] + " " + param[1] + " " + param[2] + " " + param[3] + " ");
            //showAlertSuccess("Tambah Jenis Penelitian Pengabdian berhasil");
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
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
            lib.CallProcedure("stp_editJenisPP", new string[] { editKode.Text, EditNama.Text ,ddlEditJenis.SelectedValue ,EditDana.Text, Context.User.Identity.Name });
            btnCancelEdit_Click(sender, e);
            loadData();
            showAlertSuccess("Ubah jenis penelitian pengabdian berhasil");
        }
        catch (Exception ex)
        {
            showAlertDanger(ex.Message + " " + ex.StackTrace);
        }
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib.CallProcedure("stp_deleteJenisPP", new string[] { txtConfirmDelete.Text });
        loadData();
        showAlertSuccess("Hapus jenis penelitian pengabdian berhasil");
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
        return lib.CallProcedure("stp_getDataBidangFokus", new String[] { Context.User.Identity.Name, Server.HtmlEncode(txtCari.Text.Trim()) });
    }

    //protected void ExportExcel(object sender, EventArgs e)
    //{
    //    string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
    //    using (SqlConnection con = new SqlConnection(constr))
    //    {
    //        using (SqlCommand cmd = new SqlCommand(query))
    //        {
    //            using (SqlDataAdapter sda = new SqlDataAdapter())
    //            {
    //                cmd.Connection = con;
    //                sda.SelectCommand = cmd;
    //                using (DataTable dt = new DataTable())
    //                {
    //                    sda.Fill(dt);
    //                    using (XLWorkbook wb = new XLWorkbook())
    //                    {
    //                        wb.Worksheets.Add(dt, "Customers");

    //                        Response.Clear();
    //                        Response.Buffer = true;
    //                        Response.Charset = "";
    //                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
    //                        Response.AddHeader("content-disposition", "attachment;filename=SqlExport.xlsx");
    //                        using (MemoryStream MyMemoryStream = new MemoryStream())
    //                        {
    //                            wb.SaveAs(MyMemoryStream);
    //                            MyMemoryStream.WriteTo(Response.OutputStream);
    //                            Response.Flush();
    //                            Response.End();
    //                        }
    //                    }
    //                }
    //            }
    //        }
    //    }
    //    tglSelesaiCari.Text = "";
    //    tglMulaiCari.Text = "";
    //    txtCari.Text = "";
    //}
}
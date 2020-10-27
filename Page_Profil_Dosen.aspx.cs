using ProposalPolmanAstra.Classes;
using System;
using System.Configuration;
using System.Data;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;

    public partial class _Default : System.Web.UI.Page
    {
    SiteMaster master;
    DataTable dt;
    public PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());

    protected void Page_Load(object sender, EventArgs e)
        {
        master = this.Master as SiteMaster;

        master.setTitle("Profil Saya");
        dt = master.sso.CallProcedure("stp_getProfilDosen", new string[] { Context.User.Identity.Name});

        nama.Text = dt.Rows[0][1].ToString();
        prodi.Text = dt.Rows[0][2].ToString();
        Email.Text = dt.Rows[0][10].ToString();
        Posisi.Text = dt.Rows[0][4].ToString();
        Alamat.Text = dt.Rows[0][5].ToString();
        ttl.Text = dt.Rows[0][6].ToString()+", "+ dt.Rows[0][17].ToString();
        Pendidikan.Text = dt.Rows[0][3].ToString();

        //ppDosen.ImageUrl = Request.PhysicalApplicationPath + @"\images\profildosen.png";
        //if (Context.User.Identity.IsAuthenticated)
        //{
        //    Response.Redirect("Page_Login");
        //}
        //panelWelcome.Visible = true;

    }
}

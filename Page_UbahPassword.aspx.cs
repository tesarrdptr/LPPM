using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


    public partial class Page_UbahPassword : System.Web.UI.Page
    {
        PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["SSOConnection"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnCancelChange_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancelAdd_Click(object sender, EventArgs e)
        {

        }

        protected void showAlertSuccess(string message)
        {
            divSuccess.Visible = true;
            divSuccess.InnerHtml = message;
        }

        protected void showAlertDanger(string message)
        {
            divAlert.Visible = true;
            divAlert.InnerHtml = message;
        }

        protected void btnSubmitAdd_Click(object sender, EventArgs e)
        {
            if (passwordBaru.Text == konfPasswordBaru.Text)
            {
                if (passwordLama.Text == konfPasswordBaru.Text)
                {
                    showAlertDanger("Password baru tidak boleh sama dengan password lama");
                }
                else
                {
                    try
                    {
                        lib.CallProcedure("stp_editPassword", new string[] { Context.User.Identity.Name, konfPasswordBaru.Text });
                        btnCancelAdd_Click(sender, e);
                        showAlertSuccess("Ubah password berhasil");
                    }
                    catch (Exception ex)
                    {
                        showAlertDanger(ex.Message + " " + ex.StackTrace);
                    }
                }
            }

        }
    }
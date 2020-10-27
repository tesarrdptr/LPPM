﻿using ProposalPolmanAstra.Classes;
using System;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Configuration;
using System.Web.Security;
using System.Web.UI.WebControls;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using NPOI.XSSF.UserModel;
using System.IO;
using System.Web.UI;
using System.Drawing;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;
using ClosedXML.Excel;
using System.Net.Mail;
using System.Xml;

public partial class Page_KelolaProposal : System.Web.UI.Page
{
    SiteMaster master;
    System.Data.SqlClient.SqlConnection connection =
    new SqlConnection(WebConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString);
    public PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["LP2MConnection"].ToString());
    DataTable dt = new DataTable();
    MailFunction mail = new MailFunction();
    private const string ASCENDING = " ASC";
    private const string DESCENDING = " DESC";
    //   LDAPAuthentication adAuth = new LDAPAuthentication();
    private string[] idAnggota = new string[5];

    protected void Page_Load(object sender, EventArgs e)
    {
        master = this.Master as SiteMaster;

        if (!IsPostBack)
        {
            ddStatus.Items.Add(new ListItem("--- Semua ---", "ALL"));
            ddStatus.Items.Add(new ListItem("Draft", "0"));
            ddStatus.Items.Add(new ListItem("Menunggu Konfirmasi", "1"));
            ddStatus.Items.Add(new ListItem("Diajukan", "2"));

            userPenelitian();
            editUserPenelitian();
            bidangFokus();
            try
            {
                FormsIdentity id = (FormsIdentity)Context.User.Identity;
                FormsAuthenticationTicket ticket = id.Ticket;
                if (!Context.User.Identity.IsAuthenticated || !ticket.UserData.Split('@')[1].Equals("APP07") || master.sso.CallProcedure("stp_getMenuByRole", new string[] { ticket.UserData.Split('@')[0], ticket.UserData.Split('@')[1], Request.Path.Split('/')[Request.Path.Split('/').Length - 1].Replace(".aspx", "") }).Rows.Count == 0)
                {
                    Response.Redirect("http://10.5.0.123/sso");
                }
            }
            catch
            {
                Response.Redirect("http://10.5.0.123/sso");
            }
        }
        if (!String.IsNullOrEmpty(Request.QueryString["action"]))
        {
            string act = Request.QueryString["action"];
            if (act == "new") linkTambah_Click(sender, e);
        }

        this.Page.Form.Enctype = "multipart/form-data";

        loadData();
        //gridData.Width = Unit.Percentage(100);
    }

    protected void bidangFokus()
    {
        addBidangFokus.DataSource = lib.CallProcedure("stp_getBidangFokus", new String[] { Context.User.Identity.Name });
        addBidangFokus.DataValueField = "bidfok_id";
        addBidangFokus.DataTextField = "bidfok_name";
        addBidangFokus.DataBind();
        addBidangFokus.Items.Insert(0, new ListItem("--- Pilih Bidang Fokus ---", ""));
    }
    protected void userPenelitian()
    {
        addMemberDL1.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        addMemberDL1.DataValueField = "iduser";
        addMemberDL1.DataTextField = "iduser";
        addMemberDL1.DataBind();
        addMemberDL1.Items.Insert(0, new ListItem("--- Pilih Anggota ---", ""));
        addMemberDL2.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        addMemberDL2.DataValueField = "iduser";
        addMemberDL2.DataTextField = "iduser";
        addMemberDL2.DataBind();
        addMemberDL2.Items.Insert(0, new ListItem("--- Pilih Anggota ---", ""));
        addMemberDL3.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        addMemberDL3.DataValueField = "iduser";
        addMemberDL3.DataTextField = "iduser";
        addMemberDL3.DataBind();
        addMemberDL3.Items.Insert(0, new ListItem("--- Pilih Anggota ---", ""));
        addMemberDL4.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        addMemberDL4.DataValueField = "iduser";
        addMemberDL4.DataTextField = "iduser";
        addMemberDL4.DataBind();
        addMemberDL4.Items.Insert(0, new ListItem("--- Pilih Anggota ---", ""));
        addJenisID.DataSource = lib.CallProcedure("stp_getJenisProposal", new String[] { });
        addJenisID.DataValueField = "jns_id";
        addJenisID.DataTextField = "jns_title";
        addJenisID.DataBind();
        addJenisID.Items.Insert(0, new ListItem("--- Pilih Jenis Proposal ---", ""));


    }
    protected void editUserPenelitian()
    {
        editMemberDL1.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        editMemberDL1.DataValueField = "iduser";
        editMemberDL1.DataTextField = "iduser";
        editMemberDL1.DataBind();
        editMemberDL1.Items.Insert(0, new ListItem("--- Pilih Anggota ---", ""));
        editMemberDL2.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        editMemberDL2.DataValueField = "iduser";
        editMemberDL2.DataTextField = "iduser";
        editMemberDL2.DataBind();
        editMemberDL2.Items.Insert(0, new ListItem("--- Pilih Anggota ---", ""));
        editMemberDL3.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        editMemberDL3.DataValueField = "iduser";
        editMemberDL3.DataTextField = "iduser";
        editMemberDL3.DataBind();
        editMemberDL3.Items.Insert(0, new ListItem("--- Pilih Anggota ---", ""));
        editMemberDL4.DataSource = master.sso.CallProcedure("stp_getUserPeneliti", new String[] { Context.User.Identity.Name });
        editMemberDL4.DataValueField = "iduser";
        editMemberDL4.DataTextField = "iduser";
        editMemberDL4.DataBind();
        editMemberDL4.Items.Insert(0, new ListItem("--- Pilih Anggota ---", ""));
        editJenisID.DataSource = lib.CallProcedure("stp_getJenisProposal", new String[] { });
        editJenisID.DataValueField = "jns_id";
        editJenisID.DataTextField = "jns_title";
        editJenisID.DataBind();

    }

    protected void loadData()
    {
        if (tglMulai.Text == String.Empty || tglSelesai.Text == String.Empty)
        {
            gridData.DataSource = lib.CallProcedure("stp_getProposalsByOwner", new String[] { Context.User.Identity.Name, ddStatus.SelectedValue, Server.HtmlEncode(txtCari.Text.Trim()) });
            gridData.DataBind();
        }
        else
        {
            gridData.DataSource = lib.CallProcedure("stp_getProposalsByDate", new String[] { Context.User.Identity.Name, ddStatus.SelectedValue, Server.HtmlEncode(tglMulai.Text.Trim()), Server.HtmlEncode(tglSelesai.Text.Trim()) });
            gridData.DataBind();
        }

        master.hideAlert();
        GridView1.DataSource = lib.CallProcedure("stp_getProposalsByOwner", new String[] { Context.User.Identity.Name });
        GridView1.DataBind();
    }

    public void fillDgAnggota(String id)
    {

        SqlCommand insertCmd = new SqlCommand("stp_getProposalownerAnggota", connection);
        insertCmd.CommandType = CommandType.StoredProcedure;

        insertCmd.Parameters.AddWithValue("@p1", id);

        connection.Open();
        insertCmd.ExecuteNonQuery();
        connection.Close();

        SqlDataAdapter dataAdapter = new SqlDataAdapter(insertCmd);
        DataTable dt = new DataTable();
        dataAdapter.Fill(dt);
        grdAnggotaProposal.DataSource = dt;
        grdAnggotaProposal.DataBind();

    }

    protected void linkTambah_Click(object sender, EventArgs e)
    {
        string act = Request.QueryString["action"];
        if (act != "new") Response.Redirect("Page_KelolaProposal?action=new");
        else
        {
            master.setTitle("Tambah Proposal");
            master.hideAlert();
            panelData.Visible = false;
            panelAdd.Visible = true;
            panelEdit.Visible = false;
        }
    }

    protected void btnCancelAdd_Click(object sender, EventArgs e)
    {
        string act = Request.QueryString["action"];
        if (act == "new") Response.Redirect("Page_KelolaProposal");
        //else if(act=="sukses")
        //{
        //    master.setTitle("Proposal Saya");
        //    //master.hideAlert();
        //    panelData.Visible = true;
        //    panelAdd.Visible = false;
        //    panelEdit.Visible = false;
        //    AddJudul.Text = "";
        //    addAbstrak.Text = "";
        //    addKeyword.Text = "";
        //}
        else
        {
            master.setTitle("Proposal Saya");
            //master.hideAlert();
            panelData.Visible = true;
            panelAdd.Visible = false;
            panelEdit.Visible = false;
            AddJudul.Text = "";
            addAbstrak.Text = "";
            addKeyword.Text = "";
        }
    }

    protected void btnSubmitAdd_Click(object sender, EventArgs e)
    {
        string tempLinkBerkas = "";
        string saveDir = @"\Uploaded\";
        string appPath = Request.PhysicalApplicationPath;
        string tempExtensi = "";
        string errormessage = "Error: ";
        string fileSavePath = "";
        string extension = "";


        try
        {
            if (addUpload.HasFile)
            {
                if (addUpload.PostedFile.ContentLength > 10485760)
                    master.showAlertDanger("Berkas terlalu besar. Maksimum adalah 10 MB");
                else if (!Path.GetExtension(addUpload.FileName).ToLower().Equals(".pdf"))
                    errormessage += "<br>- Berkas harus berformat .pdf";
                else
                {
                    if (addMemberDL2.SelectedValue != "" || addMemberDL3.SelectedValue != "" || addMemberDL4.SelectedValue != "")
                    {
                        if ((addMemberDL1.SelectedValue == addMemberDL2.SelectedValue) ||
                    (addMemberDL1.SelectedValue == addMemberDL3.SelectedValue) ||
                    (addMemberDL1.SelectedValue == addMemberDL3.SelectedValue) ||
                    (addMemberDL1.SelectedValue == addMemberDL4.SelectedValue) ||
                    (addMemberDL2.SelectedValue == addMemberDL3.SelectedValue) ||
                    (addMemberDL2.SelectedValue == addMemberDL4.SelectedValue) 
                    )
                        {
                            errormessage += "Anggota Tidak Boleh Sama";
                        }
                        else if(addMemberDL3.SelectedValue != "" || addMemberDL4.SelectedValue != "")
                        {
                            if(addMemberDL3.SelectedValue == addMemberDL4.SelectedValue)
                            errormessage += "Anggota Tidak Boleh Sama";

                        }
                    }


                    {
                        extension = Path.GetExtension(addUpload.PostedFile.FileName).ToString();

                        tempLinkBerkas = addJenisID.SelectedItem + "_" + lib.Encrypt(addUpload.FileName + "#" + DateTime.Now.ToString(), "PolmanAstra_LPPM") + "." + addUpload.FileName.Split('.')[addUpload.FileName.Split('.').Length - 1];
                        addUpload.SaveAs(appPath + saveDir + tempLinkBerkas);

                        tempExtensi = MimeDetector.getMimeType(appPath + saveDir, tempLinkBerkas);
                        fileSavePath = Server.MapPath("Uploaded");
                        fileSavePath = fileSavePath + "//" + tempLinkBerkas;
                    }
                }
            }
        }
        catch (Exception ex)
        {
            if (errormessage.Equals("Error: "))
                errormessage += "<br>- Terjadi kesalahan sistem. Mohon hubungi bagian MIS " + ex.ToString()
                    ;
        }
        if (errormessage.Equals("Error: "))
        {
            String[] param_atc = new String[6];
            param_atc[0] = tempLinkBerkas;
            param_atc[1] = addUpload.FileBytes.Length.ToString();
            param_atc[2] = fileSavePath;
            param_atc[3] = extension;
            var atc = lib.CallProcedure("stp_newAttachment", param_atc);
            int uang = int.Parse(addDana.Text);
            String[] param = new String[15];
            param[0] = Server.HtmlEncode(addJenisID.SelectedValue.Trim());
            param[1] = Context.User.Identity.Name;
            param[2] = Server.HtmlEncode(AddJudul.Text.Trim());
            param[3] = Server.HtmlEncode(addAbstrak.Text.Trim());
            param[4] = Server.HtmlEncode(addKeyword.Text.Trim());
            param[5] = Server.HtmlEncode(atc.Rows[0][0].ToString());
            param[6] = Server.HtmlEncode(addDana.Text);
            param[7] = Server.HtmlEncode(addBidangFokus.SelectedValue.Trim());
            param[8] = Server.HtmlEncode(AddMhs.Text.Trim());
            param[9] = Server.HtmlEncode(addSubJenis.SelectedValue.Trim());


            var prp = lib.CallProcedure("stp_newProposal", param);

            lib.CallProcedure("stp_addProposalLeader", new String[] {
                        prp.Rows[0][0].ToString(),
                        Context.User.Identity.Name
                    });
            lib.CallProcedure("stp_create_proposalAnggota", new String[] {
                            prp.Rows[0][0].ToString(),
                            Context.User.Identity.Name
                        });

            if (addMemberDL1.SelectedValue != "")
            {
                lib.CallProcedure("stp_addProposalMember", new String[] {
                            prp.Rows[0][0].ToString(),
                            addMemberDL1.SelectedValue
                        });
                lib.CallProcedure("stp_edit_proposalAnggota1", new String[] {
                            prp.Rows[0][0].ToString(),
                            addMemberDL1.SelectedValue
                        });
                //try
                //{
                //    String htmlBody = "";

                //    //GENERATE MAIL BODY HERE
                //    htmlBody += "Dear Bapak/Ibu " + adAuth.GetDisplayName(addMemberDL1.SelectedValue) + ",<br><br>";
                //    htmlBody += "Anda ditujuk sebagai anggota dari proposal " + AddJudul.Text + ".<br><br>";
                //    htmlBody += "Silahkan lakukan approval pada proposal tersebut.<br><br>";
                //    htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_KelolaProposal.aspx'>[LIHAT DETAIL]</a></b><br><br>";
                //    htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
                //    htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
                //    //END GENERATE MAIL BODY HERE

                //    mail.SendMail(
                //        "[SISFO LPPM] - Pengajuan Proposal Baru: " + AddJudul.Text,
                //        adAuth.GetMail(addMemberDL1.SelectedValue),
                //        htmlBody
                //    );
                //}
                //catch { }
            }

            if (addMemberDL2.SelectedValue != "")
            {
                lib.CallProcedure("stp_addProposalMember", new String[] {
                            prp.Rows[0][0].ToString(),
                            addMemberDL2.SelectedValue
                        });
                lib.CallProcedure("stp_edit_proposalAnggota2", new String[] {
                            prp.Rows[0][0].ToString(),
                            addMemberDL2.SelectedValue
                        });
                //try
                //{
                //    String htmlBody = "";

                //    //GENERATE MAIL BODY HERE
                //    htmlBody += "Dear Bapak/Ibu " + adAuth.GetDisplayName(addMemberDL2.SelectedValue) + ",<br><br>";
                //    htmlBody += "Anda ditujuk sebagai anggota dari proposal " + AddJudul.Text + ".<br><br>";
                //    htmlBody += "Silahkan lakukan approval pada proposal tersebut.<br><br>";
                //    htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_KelolaProposal.aspx'>[LIHAT DETAIL]</a></b><br><br>";
                //    htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
                //    htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
                //    //END GENERATE MAIL BODY HERE

                //    mail.SendMail(
                //        "[SISFO LPPM] - Pengajuan Proposal Baru: " + AddJudul.Text,
                //        adAuth.GetMail(addMemberDL2.SelectedValue),
                //        htmlBody
                //    );
                //}
                //catch { }
            }
            if (addMemberDL3.SelectedValue != "")
            {
                lib.CallProcedure("stp_addProposalMember", new String[] {
                            prp.Rows[0][0].ToString(),
                            addMemberDL3.SelectedValue
                        });
                lib.CallProcedure("stp_edit_proposalAnggota3", new String[] {
                            prp.Rows[0][0].ToString(),
                            addMemberDL3.SelectedValue
                        });
                //try
                //{
                //    String htmlBody = "";

                //    //GENERATE MAIL BODY HERE
                //    htmlBody += "Dear Bapak/Ibu " + adAuth.GetDisplayName(addMemberDL2.SelectedValue) + ",<br><br>";
                //    htmlBody += "Anda ditujuk sebagai anggota dari proposal " + AddJudul.Text + ".<br><br>";
                //    htmlBody += "Silahkan lakukan approval pada proposal tersebut.<br><br>";
                //    htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_KelolaProposal.aspx'>[LIHAT DETAIL]</a></b><br><br>";
                //    htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
                //    htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
                //    //END GENERATE MAIL BODY HERE

                //    mail.SendMail(
                //        "[SISFO LPPM] - Pengajuan Proposal Baru: " + AddJudul.Text,
                //        adAuth.GetMail(addMemberDL2.SelectedValue),
                //        htmlBody
                //    );
                //}
                //catch { }
            }
            if (addMemberDL4.SelectedValue != "")
            {
                lib.CallProcedure("stp_addProposalMember", new String[] {
                            prp.Rows[0][0].ToString(),
                            addMemberDL4.SelectedValue
                        });
                lib.CallProcedure("stp_edit_proposalAnggota4", new String[] {
                            prp.Rows[0][0].ToString(),
                            addMemberDL4.SelectedValue
                        });
                //try
                //{
                //    String htmlBody = "";

                //    //GENERATE MAIL BODY HERE
                //    htmlBody += "Dear Bapak/Ibu " + adAuth.GetDisplayName(addMemberDL2.SelectedValue) + ",<br><br>";
                //    htmlBody += "Anda ditujuk sebagai anggota dari proposal " + AddJudul.Text + ".<br><br>";
                //    htmlBody += "Silahkan lakukan approval pada proposal tersebut.<br><br>";
                //    htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_KelolaProposal.aspx'>[LIHAT DETAIL]</a></b><br><br>";
                //    htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
                //    htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
                //    //END GENERATE MAIL BODY HERE

                //    mail.SendMail(
                //        "[SISFO LPPM] - Pengajuan Proposal Baru: " + AddJudul.Text,
                //        adAuth.GetMail(addMemberDL2.SelectedValue),
                //        htmlBody
                //    );
                //}
                //catch { }
            }

            //if (addMemberDL1.SelectedValue == "" && addMemberDL2.SelectedValue == "")
            //{
            //    lib.CallProcedure("stp_editStatusProposal", new String[] {
            //                prp.Rows[0][0].ToString(),
            //                Context.User.Identity.Name,
            //                "1"
            //            });
            //}

            btnCancelAdd_Click(sender, e);
            loadData();
            master.showAlertSuccess("Proposal berhasil diajukan.");
        }
        else master.showAlertDanger(errormessage);
    }
    //protected void btnSubmitAdd_Click(object sender, EventArgs e)
    //{
    //    int uang = int.Parse(addDana.Text);
    //    uang.ToString("#,##0");
    //    master.showAlertDanger(uang.ToString("#,##0"));
    //}
    protected void setAnggotaProposal(String[] param)
    {

    }

    protected void btnCancelEdit_Click(object sender, EventArgs e)
    {
        //master.hideAlert();
        panelData.Visible = true;
        panelAdd.Visible = false;
        panelEdit.Visible = false;
        editJudul.Text = "";
        editJenisID.SelectedIndex = 0;
        editAbstrak.Text = "";
        editKeyword.Text = "";
    }

    protected void btnSubmitEdit_Click(object sender, EventArgs e)
    {
        string[] anggota = idAnggota;
        if (editMemberDL1.SelectedValue == "")
        {
            master.showAlertDanger("Anggota 1 tidak boleh kosong ");
        }
        else
        {

            try
            {
                int uang = int.Parse(editDana.Text);
                DataTable data = lib.CallProcedure("stp_getProposalAttachment", new string[] { editPrpID.Text });
                string[] param = new string[15];
                param[0] = Server.HtmlEncode(editPrpID.Text);
                param[1] = Context.User.Identity.Name;
                param[2] = Server.HtmlEncode(editJudul.Text);
                param[3] = Server.HtmlEncode(editJenisID.SelectedValue);
                param[4] = Server.HtmlEncode(editAbstrak.Text);
                param[5] = Server.HtmlEncode(editKeyword.Text);
                param[6] = Server.HtmlEncode(editDana.Text);
                param[7] = Server.HtmlEncode(editBidFok.SelectedValue);
                param[8] = Server.HtmlEncode(editMhs.Text);
                param[9] = Server.HtmlEncode(editSubJenis.SelectedValue);


                //JANGAN LUPA DIGANTI KALO ATCNYA UDAH JADI
                // param[7] = Server.HtmlEncode(data.Rows[0][0].ToString());
                lib.CallProcedure("stp_editProposal", param);
                for (int i = 0; i < 4; i++)
                {
                    switch (i)
                    {
                        case 0:
                            if (editMemberDL1.SelectedValue != idAnggota1.Text)
                            {
                                if (editMemberDL1.SelectedValue != "")
                                {
                                    if(idAnggota1.Text=="Anggota")
                                    {
                                        lib.CallProcedure("stp_addProposalMember", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL1.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota1", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL1.SelectedValue });

                                    }
                                    else
                                    {
                                        lib.CallProcedure("stp_editProposalownerAnggota", new String[] { Server.HtmlEncode(editPrpID.Text),idnya1.Text, editMemberDL1.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota1", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL1.SelectedValue });
                                    }
                                    
                                }
                                else
                                    lib.CallProcedure("stp_edit_proposalAnggota1", new String[] { Server.HtmlEncode(editPrpID.Text), null });
                                lib.CallProcedure("stp_DeleteownerAnggotaProposal", new string[] { idAnggota1.Text, Server.HtmlEncode(editPrpID.Text) });

                            }
                            break;
                        case 1:
                            if (editMemberDL2.SelectedValue != idAnggota2.Text)
                            {
                                if (editMemberDL2.SelectedValue != "")
                                {
                                    if (idAnggota2.Text == "Anggota")
                                    {
                                        lib.CallProcedure("stp_addProposalMember", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL2.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota2", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL2.SelectedValue });

                                    }
                                    else
                                    {
                                        lib.CallProcedure("stp_editProposalownerAnggota", new String[] { Server.HtmlEncode(editPrpID.Text), idnya2.Text, editMemberDL2.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota2", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL2.SelectedValue });
                                    }
                                }
                            else
                                {
                                    lib.CallProcedure("stp_edit_proposalAnggota2", new String[] { Server.HtmlEncode(editPrpID.Text), null });
                                    lib.CallProcedure("stp_DeleteownerAnggotaProposal", new string[] { idAnggota2.Text, Server.HtmlEncode(editPrpID.Text) });
                                }
                                

                            }
                            break;
                        case 2:
                            if (editMemberDL3.SelectedValue != idAnggota3.Text)
                            {
                                if (editMemberDL3.SelectedValue != "") {
                                    if (idAnggota3.Text == "Anggota")
                                    {
                                        lib.CallProcedure("stp_addProposalMember", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL3.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota3", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL3.SelectedValue });

                                    }
                                    else
                                    {
                                        lib.CallProcedure("stp_editProposalownerAnggota", new String[] { Server.HtmlEncode(editPrpID.Text), idnya3.Text, editMemberDL3.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota3", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL3.SelectedValue });
                                    }
                                }
                                else
                                {
                                    lib.CallProcedure("stp_edit_proposalAnggota3", new String[] { Server.HtmlEncode(editPrpID.Text), null });
                                    lib.CallProcedure("stp_DeleteownerAnggotaProposal", new string[] { idAnggota3.Text, Server.HtmlEncode(editPrpID.Text) });
                                }
                                    


                            }
                            break;
                        case 3:
                            if (editMemberDL4.SelectedValue != idAnggota4.Text)
                            {
                                if (editMemberDL4.SelectedValue != "")
                                {
                                    if (idAnggota4.Text == "Anggota")
                                    {
                                        lib.CallProcedure("stp_addProposalMember", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL4.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota4", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL4.SelectedValue });
                                    }
                                    else
                                    {
                                        lib.CallProcedure("stp_editProposalownerAnggota", new String[] { Server.HtmlEncode(editPrpID.Text), idnya4.Text, editMemberDL4.SelectedValue });
                                        lib.CallProcedure("stp_edit_proposalAnggota4", new String[] { Server.HtmlEncode(editPrpID.Text), editMemberDL4.SelectedValue });
                                    }
                                }
                                else
                                {
                                    lib.CallProcedure("stp_edit_proposalAnggota4", new String[] { Server.HtmlEncode(editPrpID.Text), null });
                                lib.CallProcedure("stp_DeleteownerAnggotaProposal", new string[] { idAnggota4.Text, Server.HtmlEncode(editPrpID.Text) });
                                }

                            }
                            break;
                    }
                    btnCancelEdit_Click(sender, e);
                    loadData();
                    master.showAlertSuccess("Edit Proposal Berhasil");
                }
            }
            catch (Exception ex)
            {
                master.showAlertDanger(ex.Message + " " + ex.StackTrace);
            }
        }

    }

    protected void gridData_PageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        gridData.PageIndex = e.NewPageIndex;
        loadData();
    }

    protected void gridData_RowCommand(object sender, GridViewCommandEventArgs e)
    {
        DataTable dtAng;

        if (e.CommandName != "Page")
        {

            if (e.CommandName == "Ubah")
            {
                editUserPenelitian();
                String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();

                dtAng = lib.CallProcedure("stp_getProposalownerAnggota", new String[] { id });
                dt = lib.CallProcedure("stp_getProposalByID", new String[] { id });
                editBidFok.DataSource = lib.CallProcedure("stp_getBidangFokus", new String[] { Context.User.Identity.Name });
                editBidFok.DataValueField = "bidfok_id";
                editBidFok.DataTextField = "bidfok_name";
                editBidFok.DataBind();
                editBidFok.Items.Insert(0, new ListItem("--- Pilih Bidang Fokus ---", ""));
                editBidFok.SelectedValue = dt.Rows[0][10].ToString();
                editJenisID.DataSource = lib.CallProcedure("stp_getJenisProposal", new String[] { });
                editJenisID.DataValueField = "jns_id";
                editJenisID.DataTextField = "jns_title";
                editJenisID.DataBind();
                editJenisID.SelectedValue = dt.Rows[0][8].ToString();
                editPrpID.Text = dt.Rows[0][0].ToString();
                editJudul.Text = dt.Rows[0][1].ToString();
                editAbstrak.Text = dt.Rows[0][2].ToString();
                editKeyword.Text = dt.Rows[0][3].ToString();
                editDana.Text = dt.Rows[0][11].ToString();
                editSubJenis.DataSource = lib.CallProcedure("stp_getSubJenis", new String[] { dt.Rows[0][8].ToString() });
                editSubJenis.DataValueField = "jpp_id";
                editSubJenis.DataTextField = "jpp_nama";
                editSubJenis.DataBind();
                editSubJenis.Items.Insert(0, new ListItem("--- Pilih Sub Jenis ---", ""));
                editSubJenis.SelectedValue = dt.Rows[0][14].ToString();
                editMhs.Text = dt.Rows[0][15].ToString();
                for (int i = 0; i < dtAng.Rows.Count; i++)
                {
                    switch (i)
                    {
                        case 0:
                            editMemberDL1.SelectedValue = dtAng.Rows[i][1].ToString();
                            idAnggota1.Text = dtAng.Rows[i][1].ToString();
                            idnya1.Text = dtAng.Rows[i][0].ToString();
                            break;
                        case 1:
                            editMemberDL2.SelectedValue = dtAng.Rows[i][1].ToString();
                            idAnggota2.Text = dtAng.Rows[i][1].ToString();
                            idnya2.Text = dtAng.Rows[i][0].ToString();
                            break;
                        case 2:
                            editMemberDL3.SelectedValue = dtAng.Rows[i][1].ToString();
                            idnya3.Text = dtAng.Rows[i][0].ToString();
                            idAnggota3.Text = dtAng.Rows[i][1].ToString();
                            break;
                        case 3:
                            editMemberDL4.SelectedValue = dtAng.Rows[i][1].ToString();
                            idnya4.Text = dtAng.Rows[i][0].ToString();
                            idAnggota4.Text = dtAng.Rows[i][1].ToString();
                            break;

                    }
                }
                //for(int i=0; i<idAnggota.Length;i++)
                //{
                //    editKeyword.Text += idAnggota[i];
                //}

                panelAdd.Visible = false;
                panelEdit.Visible = true;
                panelData.Visible = false;

                fillDgAnggota(id);
            }
            else if (e.CommandName == "Kirim")
            {
                String id = gridData.DataKeys[Convert.ToInt32(e.CommandArgument.ToString())].Value.ToString();
                String htmlBody = "";
                DataTable dtprop = new DataTable();
                dtprop=lib.CallProcedure("stp_getProposalByID", new String[] { id });
                try
                {

                    //GENERATE MAIL BODY HERE
                    htmlBody += "Dear Bapak/Ibu Rachmad Rizky Widodo,<br><br>";
                    htmlBody += "Anda ditujuk sebagai anggota dari proposal " + dtprop.Rows[0][1].ToString() + ".<br><br>";
                    htmlBody += "Silahkan lakukan approval pada proposal tersebut.<br><br>";
                    htmlBody += "Rincian lengkap dapat Bapak/Ibu lihat melalui link berikut: <b><a href='" + WebConfigurationManager.AppSettings["linkLPPM"] + "/Page_MemberProposal?id="+ id +"'>[LIHAT DETAIL]</a></b><br><br>";
                    htmlBody += "<b>SISFO LPPM<br>Politeknik Manufaktur Astra</b><br><br>";
                    htmlBody += "Catatan:<br>Email ini dibuat otomatis oleh sistem, jangan membalas email ini.";
                    MailMessage mailMessage = new MailMessage();
                    mailMessage.To.Add("rachmadrizky9@gmail.com");
                    mailMessage.From = new MailAddress(WebConfigurationManager.AppSettings["linkUserMail"]);
                    mailMessage.Subject = "[SISFO LPPM] - Pengajuan Proposal Baru Baru: " + dtprop.Rows[0][1].ToString();
                    mailMessage.Body = htmlBody;
                    mailMessage.IsBodyHtml = true;
                    SmtpClient smtp = new SmtpClient();
                    smtp.Host = "smtp.gmail.com";

                    smtp.Port = 587;
                    smtp.Credentials = new System.Net.NetworkCredential("rchmdrw@gmail.com", "seishin9");

                    smtp.EnableSsl = true;
                    smtp.Send(mailMessage);
                    master.showAlertSuccess("Kirim Proposal Berhasil");
                }
                catch (Exception ex)
                {
                    master.showAlertDanger("Email Gagal, "+ex.ToString());

                }


                //END GENERATE MAIL BODY HERE

                //mail.SendMail(
                //    " ",
                //    "rachmadrizky9@gmail.com",
                //    htmlBody
                //);
                lib.CallProcedure("stp_sendProposalsOwner", new String[] { id });

                btnCancelEdit_Click(sender, e);
                loadData();
                master.showAlertSuccess("Kirim Proposal Berhasil");


            }
            else if (e.CommandName == "download")
            {
                string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
                SqlConnection con = new SqlConnection(constr);
                con.Open();
                SqlCommand com = new SqlCommand("select * from  TRATTACHMENT where atc_id=@id", con);
                com.Parameters.AddWithValue("id", e.CommandArgument.ToString());
                SqlDataReader dr = com.ExecuteReader();

                if (dr.Read())
                {
                    string fileName = dr["atc_name"].ToString();
                    string fileLength = dr["atc_size"].ToString();
                    string filePath = dr["atc_path"].ToString();
                    string fileExtension = dr["atc_contentType"].ToString();
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
                        master.showAlertDanger("Gagal: File tidak ditemukan!");
                    }
                }
            }
        }
    }

    protected void linkConfirmDelete_Click(object sender, EventArgs e)
    {
        lib.CallProcedure("stp_deleteProposal", new string[] { txtConfirmDelete.Text, Context.User.Identity.Name });
        loadData();
        master.showAlertSuccess("Hapus Proposal berhasil");
    }

    //protected void gridData_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    if (e.Row.RowType == DataControlRowType.DataRow)
    //    {
    //        TableCell abstraks = e.Row.Cells[2]; // sebelum ditambahkan nomor proposal index = 2
    //        TableCell status = e.Row.Cells[4]; // sebelum ditambahkan nomor proposal index = 4
    //        abstraks.Text = abstraks.Text.Length > 30 ? abstraks.Text.Substring(0, 30) + "..." : abstraks.Text;
    //        if (status.Text == "1")
    //        {
    //            status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Diajukan</span>");
    //        }

    //        else if (status.Text == "2")
    //        {
    //            status.Text = HttpUtility.HtmlDecode("<span class='label label-danger'>Ditolak</span>");

    //        }
    //        else if (status.Text == "3")
    //        {
    //            status.Text = HttpUtility.HtmlDecode("<span class='label label-success'>Diterima</span>");

    //        }
    //        else
    //        {
    //            status.Text = HttpUtility.HtmlDecode("<span class='label label-warning' title='Menunggu konfirmasi anggota'>Konfirmasi</span>");
    //        }

    //    }
    //}

    protected void btnExcel_Click(object sender, EventArgs e)
    {
        Response.Clear();
        Response.Buffer = true;
        Response.AddHeader("content-disposition", "attachment;filename=GridViewExport.xls");
        Response.Charset = "";
        Response.ContentType = "application/vnd.ms-excel";
        using (StringWriter sw = new StringWriter())
        {
            HtmlTextWriter hw = new HtmlTextWriter(sw);

            //To Export all pages
            GridView1.AllowPaging = false;
            loadData();

            GridView1.HeaderRow.BackColor = Color.White;
            foreach (TableCell cell in GridView1.HeaderRow.Cells)
            {
                cell.BackColor = GridView1.HeaderStyle.BackColor;
            }
            foreach (GridViewRow row in GridView1.Rows)
            {
                row.BackColor = Color.White;
                foreach (TableCell cell in row.Cells)
                {
                    if (row.RowIndex % 2 == 0)
                    {
                        cell.BackColor = GridView1.AlternatingRowStyle.BackColor;
                    }
                    else
                    {
                        cell.BackColor = GridView1.RowStyle.BackColor;
                    }
                    cell.CssClass = "textmode";
                }
            }

            GridView1.RenderControl(hw);

            //style to format numbers to string
            string style = @"<style> .textmode { } </style>";
            Response.Write(style);
            Response.Output.Write(sw.ToString());
            Response.Flush();
            Response.End();
        }
    }
    private string Encrypt(string clearText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] clearBytes = Encoding.Unicode.GetBytes(clearText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(clearBytes, 0, clearBytes.Length);
                    cs.Close();
                }
                clearText = Convert.ToBase64String(ms.ToArray());
            }
        }
        return clearText;
    }

    private string Decrypt(string cipherText)
    {
        string EncryptionKey = "MAKV2SPBNI99212";
        byte[] cipherBytes = Convert.FromBase64String(cipherText);
        using (Aes encryptor = Aes.Create())
        {
            Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });
            encryptor.Key = pdb.GetBytes(32);
            encryptor.IV = pdb.GetBytes(16);
            using (MemoryStream ms = new MemoryStream())
            {
                using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                {
                    cs.Write(cipherBytes, 0, cipherBytes.Length);
                    cs.Close();
                }
                cipherText = Encoding.Unicode.GetString(ms.ToArray());
            }
        }
        return cipherText;
    }
    public override void VerifyRenderingInServerForm(Control control)
    {
        /* Confirms that an HtmlForm control is rendered for the specified ASP.NET
           server control at run time. */
    }

    public void WriteExcel(DataTable dt, String extension)
    {
        IWorkbook workbook;

        if (extension == "xlsx")
        {
            workbook = new XSSFWorkbook();
        }
        else if (extension == "xls")
        {
            workbook = new HSSFWorkbook();
        }
        else
        {
            throw new Exception("This format is not supported");
        }

        ISheet sheet1 = workbook.CreateSheet("Sheet 1");

        //make a header row
        IRow row1 = sheet1.CreateRow(0);

        for (int j = 0; j < dt.Columns.Count; j++)
        {

            ICell cell = row1.CreateCell(j);
            String columnName = dt.Columns[j].ToString();
            cell.SetCellValue(columnName);
        }

        //loops through data
        for (int i = 0; i < dt.Rows.Count; i++)
        {
            IRow row = sheet1.CreateRow(i + 1);
            for (int j = 0; j < dt.Columns.Count; j++)
            {

                ICell cell = row.CreateCell(j);
                String columnName = dt.Columns[j].ToString();
                cell.SetCellValue(dt.Rows[i][columnName].ToString());
            }
        }

        using (var exportData = new MemoryStream())
        {
            workbook.Write(exportData);
            if (extension == "xlsx") //xlsx file format
            {
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ContactNPOI.xlsx"));
                Response.BinaryWrite(exportData.ToArray());
            }
            else if (extension == "xls")  //xls file format
            {
                Response.ContentType = "application/vnd.ms-excel";
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename={0}", "ContactNPOI.xls"));
                Response.BinaryWrite(exportData.GetBuffer());
            }
            Response.End();
        }
    }

    private int WordCount(string s)
    {
        int c = 0;
        string[] split = { " " };


        string[] list = s.Split(split, StringSplitOptions.RemoveEmptyEntries);

        char[] ch = {'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z',
                'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z'};


        string val = string.Empty;

        for (int i = 0; i < list.Length; i++)
        {
            if (list[i].IndexOfAny(ch) >= 0) c++;
        }

        return c;
    }

    public string ProcessNomorProposal(object myValue)
    {
        if (Convert.ToInt32(myValue) < 10)
            return "000";
        else if (Convert.ToInt32(myValue) < 100)
            return "00";
        else if (Convert.ToInt32(myValue) < 1000)
            return "0";
        else
            return "";

    }
    public string ProcessNomorProposalString(string myValue)
    {
        if (Convert.ToInt32(myValue) < 10)
            return "000";
        else if (Convert.ToInt32(myValue) < 100)
            return "00";
        else if (Convert.ToInt32(myValue) < 1000)
            return "0";
        else
            return "";

    }

    protected void grdAnggotaProposal_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.RowType == DataControlRowType.DataRow)
        {
            grdAnggotaProposal.Columns[2].Visible = false;
            TableCell nama = e.Row.Cells[0];
            TableCell status = e.Row.Cells[1];
            //   TableCell conf = e.Row.Cells[2];

            if (status.Text == "0") status.Text = HttpUtility.HtmlDecode("<span class='label label-default'>Belum konfirmasi</span>");
            else if (status.Text == "1") status.Text = HttpUtility.HtmlDecode("<span class='label label-success'>Sudah Konfirmasi</span>");

            // else status.Text = HttpUtility.HtmlDecode("<span class='label label-warning'>Leader</span>");
        }
    }

    protected void addMemberDL1_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (addMemberDL1.SelectedValue == "")
        {
            addMemberDL2.Enabled = false;
            addMemberDL3.Enabled = false;
            addMemberDL4.Enabled = false;
            addMemberDL2.SelectedIndex = 0;
            addMemberDL3.SelectedIndex = 0;
            addMemberDL4.SelectedIndex = 0;
        }
        else
        {
            addMemberDL2.Enabled = true;
        }

    }

    protected void addMemberDL4_SelectedIndexChanged(object sender, EventArgs e)
    {


    }

    protected void addMemberDL3_SelectedIndexChanged(object sender, EventArgs e)
    {
        addMemberDL4.Enabled = true;

    }

    protected void addMemberDL2_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (addMemberDL2.SelectedValue == "")
        {

            addMemberDL3.Enabled = false;
            addMemberDL4.Enabled = false;

            addMemberDL3.SelectedIndex = 0;
            addMemberDL4.SelectedIndex = 0;
        }
        else
        {
            addMemberDL3.Enabled = true;
        }
    }

    protected void editMemberDL1_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void editMemberDL2_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void editMemberDL3_SelectedIndexChanged(object sender, EventArgs e)
    {

    }

    protected void editMemberDL4_SelectedIndexChanged(object sender, EventArgs e)
    {

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
        return lib.CallProcedure("stp_getProposalsByOwner", new String[] { Context.User.Identity.Name, ddStatus.SelectedValue, Server.HtmlEncode(txtCari.Text.Trim()) });
    }
    protected void ExportExcel(object sender, EventArgs e)
    {
        string constr = ConfigurationManager.ConnectionStrings["LP2MConnection"].ConnectionString;
        using (SqlConnection con = new SqlConnection(constr))
        {
            using (SqlCommand cmd = new SqlCommand("Select DISTINCT ROW_NUMBER() over(order by A.prp_updated_date desc) as rownum,A.prp_id as prp_id, A.prp_judul as judul, C.ketua, C.anggota1, C.anggota2, C.anggota3, C.anggota4, A.atc_id,B.atc_name, A.prp_created_by as buatby,CONVERT(varchar(12), A.prp_updated_date, 113) as creadate,CONVERT(varchar(12), A.prp_submit, 113) as submitdate, CONVERT(varchar(12), A.prp_approve, 113) as approveddate,(case A.prp_status when 2 then 'Diajukan' when 4 then 'Ditolak' when 3 then 'Diterima' end) as status, A.prp_nomor as prp_nomor FROM TRPROPOSAL AS A JOIN TRATTACHMENT AS B ON A.atc_id = B.atc_id JOIN MSANGGOTAPROPOSAL C ON A.prp_id = C.prp_id"))
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
                            wb.Worksheets.Add(dt, this.Title);

                            Response.Clear();
                            Response.Buffer = true;
                            Response.Charset = "";
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.AddHeader("content-disposition", "attachment;filename="+this.Title+".xlsx");
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
    }

    protected void addJenisID_SelectedIndexChanged(object sender, EventArgs e)
    {
        addSubJenis.Enabled = true;
        addSubJenis.DataSource = lib.CallProcedure("stp_getSubJenis", new String[] { addJenisID.SelectedValue });
        addSubJenis.DataValueField = "jpp_id";
        addSubJenis.DataTextField = "jpp_nama";
        addSubJenis.DataBind();
        addSubJenis.Items.Insert(0, new ListItem("--- Pilih Sub Jenis ---", ""));
    }

    protected void editJenisID_SelectedIndexChanged(object sender, EventArgs e)
    {
        //editSubJenis.Enabled = true;
        editSubJenis.DataSource = lib.CallProcedure("stp_getSubJenis", new String[] { addJenisID.SelectedValue });
        editSubJenis.DataValueField = "jpp_id";
        editSubJenis.DataTextField = "jpp_nama";
        editSubJenis.DataBind();
        editSubJenis.Items.Insert(0, new ListItem("--- Pilih Sub Jenis ---", ""));
    }
}

<%@ Page Title="Daftar Publikasi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_daftarPublikasiReviewer.aspx.cs" Inherits="Page_daftarPublikasiReviewer" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />

    <asp:UpdateProgress ID="updateProgress1" runat="server" AssociatedUpdatePanelID="updatePanel1">
        <ProgressTemplate>
            <div id="ac-wrapper">
                <div id="popup" class="text-center center">
                    <img src="Images/IMG_Loading.gif" /><h3>Mohon Tunggu...</h3>
                </div>
            </div>
        </ProgressTemplate>
    </asp:UpdateProgress>

    <asp:Panel runat="server" ID="panelData" DefaultButton="btnCari">
        <br />
        <br />

        <div class="form-group form-inline">
            <b>Status :&nbsp;</b>
            <asp:DropDownList runat="server" ID="ddStatus" CssClass="form-control dropdown"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
            <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
            <asp:Button Text="Cetak" CssClass="btn btn-default" OnClick="ExportExcel" runat="server" />

            <%--<asp:Button Text="Coba" CssClass="btn btn-default" runat="server" data-target="#modalAnggota" data-toggle="modal" />--%>
            <br />
            <div class="form-group">
                <label>Tanggal Awal :</label>

                <div class="row">
                    <div class='col-sm-6'>
                        <asp:TextBox runat="server" ID="tglMulai" CssClass="form-control" TextMode="Date"></asp:TextBox>


                    </div>
                </div>

            </div>
            <div class="form-group">
                <label>Tanggal  Akhir :</label>

                <div class="row">
                    <div class='col-sm-6'>
                        <asp:TextBox runat="server" ID="tglSelesai" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>

        <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
            AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="pub_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
            ShowHeaderWhenEmpty="true" OnRowCommand="gridData_RowCommand" OnRowDataBound="gridData_RowDataBound" OnPageIndexChanging="gridData_PageIndexChanging">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
            <Columns>
                <asp:BoundField DataField="rownum" HeaderText="No. " NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="pub_title" HeaderText="Judul Publikasi" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="pub_creator" HeaderText="Dosen Pembuat" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="jpb_title" HeaderText="Jenis Publikasi" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                <%--<asp:BoundField DataField="atc_name" HeaderText="File" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                <%--<asp:BoundField DataField="pub_creator" HeaderText="Penulis" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                <asp:BoundField DataField="creadate" HeaderText="Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                <%--  <asp:BoundField DataField="pub_komentar" HeaderText="Alasan" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                --%>
                <asp:BoundField DataField="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="detai" runat="server" ToolTip="Lihat Detail" CommandName="Detail" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></asp:LinkButton>

                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="download" CommandArgument='<%# Eval("atc_name") %>' ToolTip="Download"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></asp:LinkButton>

                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelDetail">
        <asp:Button Text="Kembali" CssClass="btn btn-default" OnClick="Unnamed_Click" runat="server" />

        <div class="row">
            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="review-content-section">
                    <h3>Detail Publikasi</h3>
                    <hr>
                    <br />
                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Judul Publikasi</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtJudulPublikasi"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Jenis Publikasi</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtJenisPublikasi"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Komentar </asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtKomentar"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <%--                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Abstrak</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtAbstrak"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Keyword</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtKeyword"></asp:Label></b>
                            </div>
                        </div>
                    </div>--%>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Status Publikasi</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtStatus"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Tahun Publikasi</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtTahunPublikasi"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Tanggal Diajukan</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="tgldiajukan"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <%--            <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Tanggal Dinilai</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="tgldinilai"></asp:Label></b>
                            </div>
                        </div>
                    </div>--%>
                </div>
            </div>

        </div>
        <div class="row">
        </div>

    </asp:Panel>

    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <div class="alert alert-danger" runat="server" id="divAlert" visible="false"></div>
            <div class="alert alert-success" runat="server" id="divSuccess" visible="false"></div>


        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

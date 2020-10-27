<%@ Page Title="Daftar Progress Penelitian" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_daftarProgressPenelitianReviewer.aspx.cs" Inherits="Page_auditProgressPenelitianReviewer" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
            AllowSorting="true" OnSorting="gridData_Sorting" AutoGenerateColumns="false" DataKeyNames="prg_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
            ShowHeaderWhenEmpty="true" OnRowCommand="gridData_RowCommand" OnPageIndexChanging="gridData_PageIndexChanging" OnPageIndexChanged="gridData_PageIndexChanged" OnRowDataBound="gridData_RowDataBound">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
            <Columns>
                <asp:BoundField DataField="prp_id" HeaderText="ID Proposal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" Visible="false" />
                <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                    <ItemStyle HorizontalAlign="Center"></ItemStyle>
                </asp:TemplateField>
                <asp:BoundField DataField="Nomor" HeaderText="No. Proposal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="Nomor" />
                <asp:BoundField DataField="Judul Proposal" HeaderText="Judul Proposal" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" SortExpression="Judul Proposal" />
                <asp:BoundField DataField="Ketua Pengusul" HeaderText="Ketua Pengusul" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" SortExpression="Ketua Pengusul" />
                <asp:TemplateField HeaderText="Anggota" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="linkChange" Text="Lihat Anggota" CommandName="lihat" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'></asp:LinkButton></li>
                              
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField DataField="atc_name" HeaderText="File" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                <%--<asp:BoundField DataField="prp_created_by" HeaderText="Pembuat" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                <asp:BoundField DataField="Total Dana" DataFormatString="Rp {0:00,00}" HeaderText="Total Dana" NullDisplayText="-" ItemStyle-HorizontalAlign="Right" SortExpression="Total Dana" />
                <%--<asp:BoundField DataField="creadate" HeaderText="Tanggal Dibuat" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="creadate" />--%>
                <asp:BoundField DataField="Tanggal Diajukan" HeaderText="Tanggal Diajukan" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="Tanggal Diajukan" />
                <asp:BoundField DataField="Tanggal Dinilai" HeaderText="Tanggal Dinilai" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="Tanggal Dinilai" />
                <%--<asp:BoundField DataField="komen" HeaderText="Komentar" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="komen" />--%>
                <asp:BoundField DataField="Nilai Akhir" HeaderText="Nilai Akhir" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="Nilai Akhir" />
                <asp:BoundField DataField="Status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />

                <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="detail" runat="server" ToolTip="Lihat Detail" CommandName="Detail" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></asp:LinkButton>

                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="download" CommandArgument='<%# Eval("atc_name") %>' ToolTip="Unduh"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></asp:LinkButton>

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
                    <h3>Detail Proposal</h3>
                    <hr>
                    <br />
                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Nomor Proposal</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtNoProposal"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Judul Proposal</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtJudul"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Bidang Fokus</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtBidangFokus"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <div class="form-group-inner">
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
                    </div>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Total Dana</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtDana"></asp:Label></b>
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
                </div>
            </div>



            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="review-content-section">
                    <h3>Ketua dan Anggota Proposal</h3>
                    <hr>
                    <br />

                    <div class="form-group-inner">
                        <div class="row">

                            <asp:GridView runat="server" ID="grdAnggotaProposal" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                                AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="own_id" EmptyDataText="Tidak ada data"
                                ShowHeaderWhenEmpty="true">
                                <Columns>
                                    <asp:BoundField DataField="own_name" HeaderText="Nama Anggota" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                                    <asp:BoundField DataField="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="center" />
                                </Columns>
                            </asp:GridView>
                        </div>


                    </div>
                </div>
            </div>
        </div>
        <div class="row">

            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="review-content-section">
                    <h3>Penilaian Proposal</h3>
                    <hr />
                    <br />


                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Nilai Akhir</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtNilaiAkhir"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Standar Nilai</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtStandarNilai"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Komentar</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="txtKomentar"></asp:Label></b>
                            </div>
                        </div>
                    </div>

                    <div class="form-group-inner">
                        <div class="row">
                            <div class="col-lg-3 col-md-3 col-sm-3 col-xs-12">
                                <asp:Label class="login2 pull-left" runat="server">Status Proposal</asp:Label>

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
                                <asp:Label class="login2 pull-left" runat="server">Tanggal DiNilai</asp:Label>

                            </div>
                            <div class="col-lg-9 col-md-9 col-sm-9 col-xs-12">
                                <b>
                                    <asp:Label runat="server" ID="tglDinilai"></asp:Label></b>
                            </div>
                        </div>
                    </div>



                </div>
                <br />
                <br />
            </div>



            <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">
                <div class="review-content-section">
                    <h3>Detail Nilai</h3>
                    <hr>
                    <br />

                    <div class="form-group-inner">
                        <div class="row">

                            <b>
                                <asp:GridView runat="server" ID="grdNilai" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="id" EmptyDataText="Tidak ada data"
                                    ShowHeaderWhenEmpty="true">
                                    <Columns>
                                        <asp:BoundField DataField="kriteria" HeaderText="Bobot Penilaian" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="persen" HeaderText="Presentase (%)" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                                        <asp:BoundField DataField="nilai" HeaderText="Nilai Skor" NullDisplayText="-" ItemStyle-HorizontalAlign="center" />

                                        <asp:BoundField DataField="total" HeaderText="Total Skor" NullDisplayText="-" ItemStyle-HorizontalAlign="center" />
                                    </Columns>
                                </asp:GridView>
                            </b>
                        </div>


                    </div>
                </div>
            </div>
        </div>

    </asp:Panel>



    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <div class="alert alert-danger" runat="server" id="divAlert" visible="false"></div>
            <div class="alert alert-success" runat="server" id="divSuccess" visible="false"></div>


        </ContentTemplate>

    </asp:UpdatePanel>


    <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="deleteModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="deleteModalLabel">Hapus Data</h4>
                </div>
                <div class="modal-body">
                    Apakah Anda yakin?
                </div>
                <div class="modal-footer">
                    <asp:TextBox runat="server" ID="txtConfirmDelete" CssClass="hidden" ClientIDMode="Static"></asp:TextBox>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>
                    <%--<asp:LinkButton runat="server" ID="linkConfirmDelete" CssClass="btn btn-danger" Text="Hapus" OnClick="linkConfirmDelete_Click"></asp:LinkButton>--%>
                </div>
            </div>
        </div>
    </div>

    <div id="MyPopup" class="modal fade" role="dialog">
        <div class="modal-dialog">
            <!-- Modal content-->
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal">
                        &times;</button>
                    <h4 class="modal-title"></h4>
                </div>
                <div class="modal-body">
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-danger" data-dismiss="modal">
                        Close</button>
                </div>
            </div>
        </div>
    </div>
    <!-- Modal Popup -->
    <script type="text/javascript">
        function ShowPopup(title, body) {
            $("#MyPopup .modal-title").html(title);
            $("#MyPopup .modal-body").html(body);
            $("#MyPopup").modal("show");
        }
    </script>

    <script type="text/javascript">
        $('#deleteModal').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget)
            var data = button.data('delete')
            var modal = $(this)
            modal.find('.modal-footer input').val(data)
        })
    </script>

    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css"
        rel="stylesheet" type="text/css" />

    <asp:SqlDataSource
        ID="srcNamaProposal"
        SelectCommand="SELECT prp_id, prp_judul FROM TRPROPOSAL WHERE jns_id = 1"
        ConnectionString="<%$ ConnectionStrings: LP2MConnection %>"
        runat="server" />
</asp:Content>

<%@ Page Title="Penilaian Progress Penelitian" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Trans_AuditProgressPenelitian.aspx.cs" Inherits="Page_Trans_AuditProgressPenelitian" %>

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


    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <div class="alert alert-danger" runat="server" id="divAlert" visible="false"></div>
            <div class="alert alert-success" runat="server" id="divSuccess" visible="false"></div>


        </ContentTemplate>
    </asp:UpdatePanel>
    <asp:Panel runat="server" ID="panelData" DefaultButton="btnCari">

        <div class="form-group form-inline">
            <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian Judul dan Ketua Pengusul"></asp:TextBox>
            <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
            <asp:Button Text="Cetak" CssClass="btn btn-default" OnClick="ExportExcel" runat="server" />

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
            AllowSorting="true" AutoGenerateColumns="false" DataKeyNames="prg_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
            ShowHeaderWhenEmpty="true" OnRowCommand="gridData_RowCommand" OnPageIndexChanged="gridData_PageIndexChanged" OnRowDataBound="gridData_RowDataBound"
            OnSorting="gridData_Sorting">
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
                <asp:BoundField DataField="Total Dana" DataFormatString="Rp {0:00,00}" HeaderText="Total Dana" NullDisplayText="-" ItemStyle-HorizontalAlign="Right" SortExpression="Total Dana" />

                <asp:BoundField DataField="Tanggal Diajukan" HeaderText="Tanggal Diajukan  Progress" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="Tanggal Diajukan" />
                <asp:BoundField DataField="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:LinkButton ID="detai" runat="server" ToolTip="Lihat Detail" CommandName="Detail" CommandArgument='<%# DataBinder.Eval(Container,"RowIndex") %>'><span class="glyphicon glyphicon-eye-open" aria-hidden="true"></asp:LinkButton>

                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="download" CommandArgument='<%# Eval("atc_name") %>' ToolTip="Unduh"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="linkNilai" CommandName="Nilai" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Beri Nilai"><span class="glyphicon glyphicon-edit" aria-hidden="false"></span></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelNilai" Visible="false">
        <h3 class="modal-title" id="lblPenilaian">Skor penilaian (1-100)</h3>
        <br />
        <br />
        <div id="beriNilai" runat="server"></div>
        <asp:Label runat="server" ID="editKode" Visible="false"></asp:Label>
        <div class="form-group" style="margin-bottom: -10px">
            <b>
                <asp:Label runat="server" CssClass="control-label" Text="Komentar"></asp:Label></b>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:RequiredFieldValidator runat="server" ID="reqKomentar" ControlToValidate="komentar" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:TextBox TextMode="MultiLine" Height="200px" runat="server" ID="komentar" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
        </div>
        <div class="form-group">
            <asp:Label runat="server" ID="lblCreated"></asp:Label>
            <asp:LinkButton runat="server" ID="btnCancelMonev" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelMonev_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"></span> Kembali</asp:LinkButton>
            <asp:LinkButton runat="server" ID="btnSubmitMonev" CssClass="btn btn-primary" data-toggle="modal" data-target="#insertModal" Text="Simpan"><span aria-hidden="true" class="glyphicon glyphicon-save-file"></span> Simpan</asp:LinkButton>
        </div>
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
        </div>

    </asp:Panel>



    <div class="modal fade" id="insertModal" tabindex="-1" role="dialog" aria-labelledby="deleteModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="deleteModalLabel">Simpan Data</h4>
                </div>
                <div class="modal-body">
                    Apakah Anda yakin?
                </div>
                <div class="modal-footer">
                    <%--<asp:TextBox runat="server" ID="txtConfirmDelete" CssClass="hidden" ClientIDMode="Static"></--asp:TextBox>--%>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>
                    <asp:LinkButton runat="server" ID="linkConfirmInsert" CssClass="btn btn-primary" Text="Simpan" OnClick="linkConfirmInsert_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $('#insertModal').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget)
            var data = button.data('delete')
            var modal = $(this)
            modal.find('.modal-footer input').val(data)
        })
    </script>
    <!-- Modal Popup -->
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
    <link href="http://ajax.aspnetcdn.com/ajax/jquery.ui/1.8.9/themes/start/jquery-ui.css"
        rel="stylesheet" type="text/css" />
</asp:Content>

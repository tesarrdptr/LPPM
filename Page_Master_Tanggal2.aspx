<%@ Page Title="Master Tanggal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Master_Tanggal2.aspx.cs" Inherits="Page_Trans_daftarProgressProposal" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">

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
        <asp:LinkButton runat="server" ID="linkCetakLaporan" CssClass="btn btn-primary" OnClick="linkCetakLaporan_Click"><span aria-hidden="true" class="glyphicon glyphicon-plus"> Tambah Baru</span></asp:LinkButton>
        <br />
        <br />

        <div class="form-group form-inline">
            <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian Deskripsi, Dibuat Oleh, Diubah Oleh"></asp:TextBox>
            <asp:Label runat="server" ID="lblTglMulaiCari" Text="Tanggal Mulai : "></asp:Label>
            <asp:TextBox runat="server" ID="tglMulaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>
            <asp:Label runat="server" ID="lblTglSelesaiCari" Text="Tanggal Selesai : "></asp:Label>
            <asp:TextBox runat="server" ID="tglSelesaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>
            <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default" OnClick="btnCari_Click"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
        </div>

        <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
            AllowSorting="true" OnSorting="gridData_Sorting" AutoGenerateColumns="false" DataKeyNames="tgl_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowCommand="gridData_RowCommand">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
            <Columns>
                <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField DataField="tgl_id" HeaderText="ID Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" Visible="true" SortExpression="tgl_id" />
                --%><asp:BoundField DataField="tgl_jenis" HeaderText="Deskripsi Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="tgl_jenis" />
                <asp:BoundField DataField="tgl_mulai" HeaderText="Tanggal Mulai" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="tgl_mulai"/>
                <asp:BoundField DataField="tgl_selesai" HeaderText="Tanggal Selesai" NullDisplayText="-" ItemStyle-HorizontalAlign="Center"  SortExpression="tgl_selesai"/>
                <%--<asp:BoundField DataField="tgl_anggaran" HeaderText="Tanggal Anggaran" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />--%>
                <asp:BoundField DataField="tgl_created_by" HeaderText="Dibuat Oleh" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="tgl_created_by"/>
                <asp:BoundField DataField="tgl_created_date" HeaderText="Dibuat Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="tgl_created_date"/>
                <asp:BoundField DataField="tgl_updated_by" HeaderText="Diubah Oleh" NullDisplayText="-" ItemStyle-HorizontalAlign="Center"  SortExpression="tgl_updated_by"/>
                <asp:BoundField DataField="tgl_updated_date" HeaderText="Diubah Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="tgl_updated_date"/>
                <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                    <%--<ItemTemplate>
                        <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="download" CommandArgument='<%# Eval("atc_name") %>' ToolTip="Unduh"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></asp:LinkButton>
                    </ItemTemplate>--%>

                    <ItemTemplate>
                        <asp:LinkButton runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Ubah Data Tanggal"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>
                        <%--<asp:LinkButton runat="server" ID="LinkButton1" CommandName="Detil" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Lihat Detil Tanggal"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span></asp:LinkButton>--%>
                        <asp:LinkButton runat="server" ID="linkDelete" ToolTip="Hapus Data Tanggal" data-delete='<%# Eval("tgl_id") %>' data-toggle="modal" data-target="#deleteModal"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>
                    </ItemTemplate>

                </asp:TemplateField>

            </Columns>
        </asp:GridView>
    </asp:Panel>
    <asp:Panel runat="server" ID="panelAdd" Visible="false" DefaultButton="linkCetakLaporan">
        <div>
            <asp:Label runat="server" CssClass="control-label" Text="Pilih Tanggal"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:RequiredFieldValidator runat="server" ID="ReqJenisPub" ControlToValidate="ddlTanggal" Text="harus dipilih" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:DropDownList
                ID="ddlTanggal"
                CssClass="form-control dropdown"
                runat="server"
                AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label runat="server" CssClass="control-label" Text="Tanggal Mulai"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <div style="padding-left: 0px; width: 450px">
                <div class="row">
                    <div class='col-sm-6'>
                        <asp:TextBox runat="server" AutoCompleteType="Disabled" ID="tglMulai" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="tglMulai" ErrorMessage="Data wajib diisi" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" CssClass="control-label" Text="Tanggal Selesai"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <div style="padding-left: 0px; width: 450px">
                <div class="row">
                    <div class='col-sm-6'>
                        <asp:TextBox runat="server" ID="tglSelesai" AutoCompleteType="Disabled" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="tglSelesai" ErrorMessage="Data wajib diisi" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </div>
                </div>
            </div>
        </div>
        <div class="form-group">
            <asp:Label runat="server" Visible="false" CssClass="control-label" ID="Label1" Text="Tanggal Anggaran"></asp:Label>
            <asp:Label runat="server" Visible="false" Text=" *" Style="color: red;"></asp:Label>
            <div style="padding-left: 0px; width: 450px">
                <div class="row">
                    <div class='col-sm-6'>
                        <asp:DropDownList ID="tglAnggaran" AutoCompleteType="Disabled" Visible="false" CssClass="form-control dropdown" runat="server" OnSelectedIndexChanged="tglAnggaran_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <%--<asp:TextBox runat="server" ID="tglAnggaran" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>--%>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="tglSelesai" ErrorMessage="Data wajib diisi" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <asp:LinkButton runat="server" ID="btnCancel" CssClass="btn btn-default" Text="Kembali" OnClick="btnCancel_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"> Kembali</span></asp:LinkButton>
        <asp:LinkButton runat="server" ID="btnCetak" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valEdit" OnClick="btnCetak_Click"><span aria-hidden="true" class="glyphicon glyphicon-print"> Simpan</span></asp:LinkButton>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelEdit" Visible="false" DefaultButton="btnSubmitEdit">
        <asp:Label runat="server" ID="editKode" Visible="false"></asp:Label>
        <%--<div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis Tanggal "></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:TextBox runat="server" ID="editTanggal" CssClass="form-control" MaxLength="50" ValidationGroup="valEdit"></asp:TextBox>
                </div>--%>
        <div>
            <asp:Label runat="server" CssClass="control-label" Text="Pilih Tanggal"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlTanggal" Text="harus dipilih" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:DropDownList
                ID="ddlEditTanggal"
                CssClass="form-control dropdown"
                runat="server"
                AutoPostBack="true">
            </asp:DropDownList>
        </div>
        <div class="form-group">
            <asp:Label runat="server" CssClass="control-label" Text="Tanggal Mulai"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:TextBox runat="server" TextMode="Date" AutoCompleteType="Disabled" ClientIDMode="Static" ID="editTanggalMulai" CssClass="form-control" MaxLength="50" ValidationGroup="valEdit"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label runat="server" CssClass="control-label" Text="Tanggal Selesai"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:TextBox runat="server" TextMode="Date" AutoCompleteType="Disabled" ClientIDMode="Static" ID="editTanggalSelesai" CssClass="form-control" MaxLength="50" ValidationGroup="valEdit"></asp:TextBox>
        </div>
        <div class="form-group">
            <asp:Label runat="server" Visible="false" CssClass="control-label" ID="Label2" Text="Tanggal Anggaran"></asp:Label>
            <asp:Label runat="server" Visible="false" Text=" *" Style="color: red;"></asp:Label>
            <div style="padding-left: 0px; width: 450px">
                <div class="row">
                    <div class='col-sm-6'>
                        <asp:DropDownList Visible="false" ID="ddlAnggaranEdit" CssClass="form-control dropdown" runat="server" OnSelectedIndexChanged="tglAnggaran_SelectedIndexChanged" AutoPostBack="true">
                        </asp:DropDownList>
                        <%--<asp:TextBox runat="server" ID="tglAnggaran" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>--%>
                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="tglSelesai" ErrorMessage="Data wajib diisi" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                    </div>
                </div>
            </div>
        </div>
        <br />
        <div class="form-group">
            <asp:LinkButton runat="server" ID="btnCancelEdit" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelEdit_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"></span> Kembali </asp:LinkButton>
            <asp:LinkButton runat="server" ID="btnSubmitEdit" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valEdit" OnClick="btnSubmitEdit_Click"><span aria-hidden="true" class="glyphicon glyphicon-save-file"></span> Simpan</asp:LinkButton>
        </div>
    </asp:Panel>


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
                    <asp:LinkButton runat="server" ID="linkConfirmDelete" CssClass="btn btn-danger" Text="Hapus" OnClick="linkConfirmDelete_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $('#deleteModal').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget)
            var data = button.data('delete')
            var modal = $(this)
            modal.find('.modal-footer input').val(data)
        })
    </script>

    <script type="text/javascript">
        $(document).ready(function () {
            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();

            $('.tomboldelete').on('click', function (event) {
                var data = $(this).data('delete');
                var modal = $("#deleteModal");
                modal.find('.modal-footer #txtConfirmDelete').val(data);
            });

            $('#tglMulai').datetimepicker({
                format: 'DD MMMM YYYY'
            });
            $('#tglSelesai').datetimepicker({
                format: 'DD MMMM YYYY',
                useCurrent: false //Important! See issue #1075
            });
            $("#tglMulai").on("dp.change", function (e) {
                $('#tglSelesai').data("DateTimePicker").minDate(e.date);
            });
            $("#tglSelesai").on("dp.change", function (e) {
                $('#tglMulai').data("DateTimePicker").maxDate(e.date);
            });

        });
    </script>

    <%-- <script type="text/javascript">
        $(document).ready(function () {
            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();

            $('.tomboldelete').on('click', function (event) {
                var data = $(this).data('delete');
                var modal = $("#deleteModal");
                modal.find('.modal-footer #txtConfirmDelete').val(data);
            });

            $('#editTanggalMulai').datetimepicker({
                format: 'DD MMMM YYYY'
            });
            $('#editTanggalSelesai').datetimepicker({
                format: 'DD MMMM YYYY',
                useCurrent: false //Important! See issue #1075
            });
            $("#editTanggalMulai").on("dp.change", function (e) {
                $('#editTanggalSelesai').data("DateTimePicker").minDate(e.date);
            });
            $("#editTanggalSelesai").on("dp.change", function (e) {
                $('#editTanggalMulai').data("DateTimePicker").maxDate(e.date);
            });

        });
    </script>--%>
</asp:Content>

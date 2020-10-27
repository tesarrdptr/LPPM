<%@ Page Title="Master Dokumen Monev" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Master_Dokumen_Monev.aspx.cs" Inherits="Page_Master_Dokumen_Monev" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">

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
        <asp:LinkButton runat="server" ID="linkTambah" CssClass="btn btn-primary" OnClick="linkTambah_Click"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Tambah Baru</asp:LinkButton>
        <br />
        <br />

        <div class="form-group form-inline">
            <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
            <asp:Label runat="server" ID="lblTglMulaiCari" Text="Tanggal Mulai : "></asp:Label>
            <asp:TextBox runat="server" ID="tglMulaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>
            <asp:Label runat="server" ID="lblTglSelesaiCari" Text="Tanggal Selesai : "></asp:Label>
            <asp:TextBox runat="server" ID="tglSelesaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>
            <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default" OnClick="btnCari_Click"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
        </div>

        <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
            AllowSorting="true" OnSorting="gridData_Sorting" AutoGenerateColumns="false" DataKeyNames="monevdoc_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowCommand="gridData_RowCommand">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
            <Columns>
                <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="monevdoc_jenis" HeaderText="Jenis Dokumen" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="monevdoc_jenis" />
                <asp:BoundField DataField="jns_title" HeaderText="Jenis" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="jns_title" />
                <asp:BoundField DataField="monevdoc_createdby" HeaderText="Dibuat Oleh" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="monevdoc_createdby" />
                <asp:BoundField DataField="monevdoc_created_date" HeaderText="Dibuat Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="monevdoc_created_date" />
                <asp:BoundField DataField="monevdoc_updatedby" HeaderText="Diubah Oleh " NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="monevdoc_updatedby" />
                <asp:BoundField DataField="monevdoc_update_date" HeaderText="Diubah Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="monevdoc_update_date" />
                <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:LinkButton ClientIDMode="Static" runat="server" ID="linkDownload" CommandArgument='<%# Eval("monevdoc_id") %>' Text="Download Dokumen" CommandName="download" ToolTip="Unduh Dokumen"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Ubah Dokumen Monev"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="linkDelete" ToolTip="Hapus Data Tanggal" data-delete='<%# Eval("monevdoc_id") %>' data-toggle="modal" data-target="#deleteModal"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelAdd" Visible="false" DefaultButton="btnSubmitAdd">
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Nama Dokumen"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddJudul" ControlToValidate="addNama" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="addNama" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="ddlAddJenis" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddlAddJenis" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Upload File ( Max :10 MB|| File : PDF )"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddUpload" ControlToValidate="addUpload" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:FileUpload runat="server" ID="addUpload" CssClass="form-control" ValidationGroup="valAdd" onchange="cekImage(this, 'proposal');"></asp:FileUpload>
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnCancelAdd" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelAdd_Click" />
                    <asp:Button runat="server" ID="btnSubmitAdd" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valAdd" OnClick="btnSubmitAdd_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelEdit" Visible="false">
        <div class="row">
            <div class="col-lg-4">
                <asp:Label runat="server" ID="editKode" Visible="false"></asp:Label>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text=" Nama Dokumen"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="editNama" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="editNama" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlEditJenis" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddlEditJenis" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Upload File ( Max :10 MB|| File : PDF )"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="editUpload" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:FileUpload runat="server" ID="editUpload" CssClass="form-control" ValidationGroup="valAdd" onchange="cekImage(this, 'proposal');"></asp:FileUpload>
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnCancelEdit" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelEdit_Click" />
                    <asp:Button runat="server" ID="btnSubmitEdit" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valAdd" OnClick="btnSubmitEdit_Click" />
                </div>
            </div>
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

</asp:Content>


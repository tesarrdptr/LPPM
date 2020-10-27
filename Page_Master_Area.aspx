<%@ Page Title="Master Area" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Master_Area.aspx.cs" Inherits="Page_Master_Area" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Master Area</h3>
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

            <asp:Panel runat="server" ID="panelData" DefaultButton="btnCari">
                <asp:LinkButton runat="server" ID="linkTambah" CssClass="btn btn-primary" OnClick="linkTambah_Click"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Tambah Baru</asp:LinkButton>
                <br />
                <br />

                <div class="form-group form-inline">
                    <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
                    <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default" OnClick="btnCari_Click"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
                </div>

                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="idarea" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowCommand="gridData_RowCommand">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:BoundField DataField="deskripsiarea" HeaderText="Nama Area" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="deskripsidept" HeaderText="Nama Departemen" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Ubah Data Area"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkDelete" ToolTip="Hapus Data Area" data-delete='<%# Eval("idarea") %>' data-toggle="modal" data-target="#deleteModal"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <asp:Panel runat="server" ID="panelAdd" Visible="false" DefaultButton="btnSubmitAdd">
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Nama Area"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddNamaArea" ControlToValidate="addNamaArea" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="addNamaArea" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Departemen"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddDepartemen" ControlToValidate="addDepartemen" Text="harus dipilih" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="addDepartemen" CssClass="form-control dropdown" ValidationGroup="valAdd"></asp:DropDownList>&nbsp;
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnCancelAdd" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelAdd_Click" />
                    <asp:Button runat="server" ID="btnSubmitAdd" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valAdd" OnClick="btnSubmitAdd_Click" />
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="panelEdit" Visible="false" DefaultButton="btnSubmitEdit">
                <asp:Label runat="server" ID="editKode" Visible="false"></asp:Label>
                <div class="form-group">
                    <asp:Label runat="server" ID="editNamaArea" CssClass="control-label" Font-Bold="true"></asp:Label>
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Departemen"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqEditDepartemen" ControlToValidate="editDepartemen" Text="harus dipilih" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="editDepartemen" CssClass="form-control dropdown" ValidationGroup="valEdit"></asp:DropDownList>&nbsp;
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnCancelEdit" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelEdit_Click" />
                    <asp:Button runat="server" ID="btnSubmitEdit" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valEdit" OnClick="btnSubmitEdit_Click" />
                </div>
            </asp:Panel>
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
                    Semua bisnis proses yang memakai nama area/bagian ini kemungkinan tidak dapat diakses kembali.<br /><br />
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


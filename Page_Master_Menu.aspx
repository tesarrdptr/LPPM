<%@ Page Title="Master Menu" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Master_Menu.aspx.cs" Inherits="Page_Master_Menu" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Master Menu</h3>
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
                    <asp:Label runat="server" CssClass="control-label" Text="Pilih Aplikasi"></asp:Label>&nbsp;
                    <asp:DropDownList runat="server" ID="cariAplikasi" CssClass="form-control dropdown" ValidationGroup="valCari"></asp:DropDownList>&nbsp;
                    <asp:RequiredFieldValidator runat="server" ID="reqCariAplikasi" ControlToValidate="cariAplikasi" Text="*" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:Label runat="server" CssClass="control-label" Text="Pilih Aplikasi"></asp:Label>&nbsp;
                    <asp:DropDownList runat="server" ID="cariJabatan" CssClass="form-control dropdown" ValidationGroup="valCari"></asp:DropDownList>&nbsp;
                    <asp:RequiredFieldValidator runat="server" ID="reqCariJabatan" ControlToValidate="cariJabatan" Text="*" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>&nbsp;
                    <asp:Button runat="server" ID="btnCari" CssClass="btn btn-default" Text="Cari" ValidationGroup="valCari" OnClick="btnCari_Click" />
                </div>

                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="false"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="idmenu" EmptyDataText="Tidak ada data"
                    ShowHeaderWhenEmpty="true" OnRowCommand="gridData_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="deskripsi" HeaderText="Nama Menu" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="tautan" HeaderText="Tautan" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="urutan" HeaderText="Urutan" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="jumlahsub" HeaderText="Jumlah Submenu" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="deskripsirole" HeaderText="Jabatan" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="linkUp" CommandName="Atas" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Pindahkan Urutan ke Atas"><span class="glyphicon glyphicon-triangle-top" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkDown" CommandName="Bawah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Pindahkan Urutan ke Bawah"><span class="glyphicon glyphicon-triangle-bottom" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkDetail" CommandName="Detail" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Detail Data Menu"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Ubah Data Menu"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkDelete" ToolTip="Hapus Data Menu" data-delete='<%# Eval("idmenu") %>' data-toggle="modal" data-target="#deleteModal"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <asp:Panel runat="server" ID="panelAdd" Visible="false" DefaultButton="btnSubmitAdd">
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Nama Menu"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddNamaMenu" ControlToValidate="addNamaMenu" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="addNamaMenu" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Tautan"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddTautan" ControlToValidate="addTautan" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="addTautan" CssClass="form-control" MaxLength="50" placeholder="Ketik # jika mempunyai submenu" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Aplikasi"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddAplikasi" ControlToValidate="addAplikasi" Text="harus dipilih" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="addAplikasi" CssClass="form-control dropdown" ValidationGroup="valAdd"></asp:DropDownList>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Jabatan"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddJabatan" ControlToValidate="addJabatan" Text="harus dipilih" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="addJabatan" CssClass="form-control dropdown" ValidationGroup="valAdd"></asp:DropDownList>&nbsp;
                </div>
                <div class="form-group">
                    <asp:CheckBox runat="server" ID="addStatus" Text="&nbsp;Apakah menu ini aktif?" />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnCancelAdd" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelAdd_Click" />
                    <asp:Button runat="server" ID="btnSubmitAdd" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valAdd" OnClick="btnSubmitAdd_Click" />
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="panelEdit" Visible="false" DefaultButton="btnSubmitEdit">
                <asp:Label runat="server" ID="editKode" Visible="false"></asp:Label>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Nama Menu"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqEditNamaMenu" ControlToValidate="editNamaMenu" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="editNamaMenu" CssClass="form-control" MaxLength="50" ValidationGroup="valEdit"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Tautan"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqEditTautan" ControlToValidate="editTautan" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="editTautan" CssClass="form-control" MaxLength="50" placeholder="Ketik # jika mempunyai submenu" ValidationGroup="valEdit"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Aplikasi"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqEditAplikasi" ControlToValidate="editAplikasi" Text="harus dipilih" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="editAplikasi" CssClass="form-control dropdown" ValidationGroup="valEdit"></asp:DropDownList>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Jabatan"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqEditJabatan" ControlToValidate="editJabatan" Text="harus dipilih" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="editJabatan" CssClass="form-control dropdown" ValidationGroup="valEdit"></asp:DropDownList>&nbsp;
                </div>
                <div class="form-group">
                    <asp:CheckBox runat="server" ID="editStatus" Text="&nbsp;Apakah menu ini aktif?" />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnCancelEdit" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelEdit_Click" />
                    <asp:Button runat="server" ID="btnSubmitEdit" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valEdit" OnClick="btnSubmitEdit_Click" />
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="panelDetail" Visible="false">
                <asp:Label runat="server" ID="detailKode" Visible="false"></asp:Label>
                Nama Menu :&nbsp;<asp:Label runat="server" ID="detailMenu" CssClass="control-label"></asp:Label><br />
                Aplikasi :&nbsp<asp:Label runat="server" ID="detailAplikasi" CssClass="control-label"></asp:Label><br />
                Jabatan :&nbsp<asp:Label runat="server" ID="detailJabatan" CssClass="control-label"></asp:Label><br />
                <br />
                <asp:LinkButton runat="server" ID="linkTambahDetail" CssClass="btn btn-primary" data-toggle="modal" data-target="#addDetailModal"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Tambah Submenu</asp:LinkButton>
                <br />
                <br />
                <asp:GridView runat="server" ID="gridDetail" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="false"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="idsubmenu" EmptyDataText="Tidak ada data"
                    ShowHeaderWhenEmpty="true" OnRowCommand="gridDetail_RowCommand">
                    <Columns>
                        <asp:BoundField DataField="deskripsi" HeaderText="Nama Sub Menu" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="tautan" HeaderText="Tautan" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="urutan" HeaderText="Urutan" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="linkUp" CommandName="Atas" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Pindahkan Urutan ke Atas"><span class="glyphicon glyphicon-triangle-top" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkDown" CommandName="Bawah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Pindahkan Urutan ke Bawah"><span class="glyphicon glyphicon-triangle-bottom" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkDelete" ToolTip="Hapus Data Submenu" data-deletedetail='<%# Eval("idsubmenu") %>' data-toggle="modal" data-target="#deleteDetailModal"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnCancelDetail" CssClass="btn btn-default" Text="Kembali" OnClick="btnCancelDetail_Click" />
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
                    Semua submenu pada menu ini akan ikut terhapus.<br />
                    <br />
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

    <div class="modal fade" id="deleteDetailModal" tabindex="-1" role="dialog" aria-labelledby="deleteDetailModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="deleteDetailModalLabel">Hapus Data Submenu</h4>
                </div>
                <div class="modal-body">
                    Apakah Anda yakin?
                </div>
                <div class="modal-footer">
                    <asp:TextBox runat="server" ID="txtConfirmDeleteDetail" CssClass="hidden" ClientIDMode="Static"></asp:TextBox>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>
                    <asp:LinkButton runat="server" ID="linkConfirmDeleteDetail" CssClass="btn btn-danger" Text="Hapus" OnClick="linkConfirmDeleteDetail_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <div class="modal fade" id="addDetailModal" tabindex="-1" role="dialog" aria-labelledby="addDetailModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="addDetailModalLabel">Tambah Submenu</h4>
                </div>
                <div class="modal-body">
                    <asp:Panel runat="server" ID="panelAddDetailDetail">
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Nama Submenu"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="reqAddDetailNamaSubMenu" ControlToValidate="addDetailNamaSubMenu" Text="harus diisi" ForeColor="Red" ValidationGroup="valAddDetail" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="addDetailNamaSubMenu" CssClass="form-control" MaxLength="50" ValidationGroup="valAddDetail"></asp:TextBox>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Tautan"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="reqAddDetailTautan" ControlToValidate="addDetailTautan" Text="harus diisi" ForeColor="Red" ValidationGroup="valAddDetail" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="addDetailTautan" CssClass="form-control" MaxLength="50" ValidationGroup="valAddDetail"></asp:TextBox>&nbsp;
                        </div>
                        <div class="form-group">
                            <asp:CheckBox runat="server" ID="addDetailStatus" Text="&nbsp;Apakah sub menu ini aktif?" />
                        </div>
                    </asp:Panel>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>
                    <asp:LinkButton runat="server" ID="linkConfirmAddDetail" CssClass="btn btn-primary" Text="Simpan" OnClick="linkConfirmAddDetail_Click" ValidationGroup="valAddDetail"></asp:LinkButton>
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

        $('#deleteDetailModal').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget)
            var data = button.data('deletedetail')
            var modal = $(this)
            modal.find('.modal-footer input').val(data)
        })

        $('#addDetailModal').on('show.bs.modal', function (event) { })
    </script>
</asp:Content>

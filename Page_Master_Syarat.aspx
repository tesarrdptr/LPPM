<%@ Page Title="Master Syarat Pengajuan Proposal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Master_Syarat.aspx.cs" Inherits="Page_Master_Syarat" %>

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

            <asp:Panel runat="server" ID="panelData" DefaultButton="btnCari">
                <asp:LinkButton runat="server" ID="linkTambah" CssClass="btn btn-primary" OnClick="linkTambah_Click"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Tambah Baru</asp:LinkButton>
                <br />
                <br />

                <div class="form-group form-inline">
                    <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian "></asp:TextBox>
                    <asp:Label runat="server" ID="lblTglMulaiCari" Text="Tanggal Mulai : "></asp:Label>
                    <asp:TextBox runat="server" ID="tglMulaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    <asp:Label runat="server" ID="lblTglSelesaiCari" Text="Tanggal Selesai : "></asp:Label>
                    <asp:TextBox runat="server" ID="tglSelesaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default" OnClick="btnCari_Click"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
                </div>

                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="true" OnSorting="gridData_Sorting" AutoGenerateColumns="false" DataKeyNames="syr_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowCommand="gridData_RowCommand">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="syr_nidn" HeaderText="NIDN/NIDK" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="syr_nidn" />
                        <asp:BoundField DataField="syr_job" HeaderText="Jabatan" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="syr_job" />
                        <asp:BoundField DataField="syr_edu" HeaderText="Pendidikan Terakhir" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="syr_edu" />
                        <asp:BoundField DataField="jpp_nama" HeaderText="Nama Penelitian Pengabdian" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="jpp_nama" />
                        <asp:BoundField DataField="creaby" HeaderText="Dibuat Oleh" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="creaby" />
                        <asp:BoundField DataField="creadate" HeaderText="Dibuat Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="creadate" />
                        <asp:BoundField DataField="updateby" HeaderText="Diubah Oleh" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="updateby" />
                        <asp:BoundField DataField="updatedate" HeaderText="Diubah Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="updatedate" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Ubah Data Karyawan"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>
                                <%--<asp:LinkButton runat="server" ID="LinkButton1" CommandName="Detil" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Lihat Detil Karyawan"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span></asp:LinkButton>--%>
                                <asp:LinkButton runat="server" ID="linkDelete" ToolTip="Hapus Data Karyawan" data-delete='<%# Eval("syr_id") %>' data-toggle="modal" data-target="#deleteModal"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <asp:Panel runat="server" ID="panelAdd" Visible="false" DefaultButton="btnSubmitAdd">

                <div class="row">
                    <div class="col-lg-4">
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Status NIDN/NIDK"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" ControlToValidate="ddlAddNIDN" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlAddNIDN" runat="server" CssClass="form-control dropdown" AutoPostBack="false" />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Jabatan Fungsi"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="reqAddJenis" ControlToValidate="ddlAddJabatan" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlAddJabatan" runat="server" CssClass="form-control dropdown" AutoPostBack="false" />
                        </div>

                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Pendidikan Terakhir"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlAddPendidikan" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlAddPendidikan" runat="server" CssClass="form-control dropdown" AutoPostBack="false" />
                        </div>

                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Jenis Penelitian Pengabdian"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlAddPP" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlAddPP" runat="server" CssClass="form-control dropdown" AutoPostBack="false" />
                        </div>
                    </div>
                </div>

                <br />
                <div class="form-group">
                    <asp:LinkButton runat="server" ID="btnCancelAdd" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelAdd_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"></span> Kembali </asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnSubmitAdd" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valAdd" OnClick="btnSubmitAdd_Click"><span aria-hidden="true" class="glyphicon glyphicon-save-file"></span> Simpan</asp:LinkButton>
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="panelEdit" Visible="false" DefaultButton="btnSubmitEdit">
                <div class="row">
                    <div class="col-lg-4">
                        <asp:Label runat="server" ID="editKode" Visible="false"></asp:Label>
                        <div class="form-group">
                            <asp:Label runat="server" ID="editPlotId" CssClass="control-label" Font-Bold="true" Visible="false"></asp:Label>
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Status NIDN/NIDK"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlEditNIDN" CssClass="form-control" ValidationGroup="valEdit"></asp:DropDownList>&nbsp;
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Jabatan Fungsi"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="ddlEditJabatan" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlEditJabatan" runat="server" CssClass="form-control dropdown" AutoPostBack="false" />
                        </div>

                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Pendidikan Terakhir"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="ddlEditPendidikan" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlEditPendidikan" runat="server" CssClass="form-control dropdown" AutoPostBack="false" />
                        </div>

                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Jenis Penelitian Pengabdian"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="ddlEditPP" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="ddlEditPP" runat="server" CssClass="form-control dropdown" AutoPostBack="false" />
                        </div>
                        <br />
                        <div class="form-group">
                            <asp:LinkButton runat="server" ID="btnCancelEdit" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelEdit_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"></span> Kembali </asp:LinkButton>
                            <asp:LinkButton runat="server" ID="btnSubmitEdit" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valEdit" OnClick="btnSubmitEdit_Click"><span aria-hidden="true" class="glyphicon glyphicon-save-file"></span> Simpan</asp:LinkButton>
                        </div>
                    </div>
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
                    Semua bisnis proses yang memakai syarat ini kemungkinan tidak dapat diakses kembali.<br />
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


    <script type="text/javascript">
        $('#deleteModal').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget)
            var data = button.data('delete')
            var modal = $(this)
            modal.find('.modal-footer input').val(data)
        })
    </script>

</asp:Content>


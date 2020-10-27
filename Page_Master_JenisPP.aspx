<%@ Page Title="Master Jenis Penelitian Pengabdian" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Master_JenisPP.aspx.cs" Inherits="Page_Master_BidangFokus" %>

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
                    <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian Dekskripsi, Dibuat Oleh, Diubah Oleh"></asp:TextBox>
                    <asp:Label runat="server" ID="lblTglMulaiCari" Text="Tanggal Mulai : "></asp:Label>
                    <asp:TextBox runat="server" ID="tglMulaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    <asp:Label runat="server" ID="lblTglSelesaiCari" Text="Tanggal Selesai : "></asp:Label>
                    <asp:TextBox runat="server" ID="tglSelesaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>
                    <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default" OnClick="btnCari_Click"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
                </div>

                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="true" OnSorting="gridData_Sorting" AutoGenerateColumns="false" DataKeyNames="jpp_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowCommand="gridData_RowCommand">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="jns_title" HeaderText="Jenis" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="jns_title" />
                        <asp:BoundField DataField="jpp_nama" HeaderText="Nama" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="jpp_nama" />
                        <asp:BoundField DataField="jpp_dana" HeaderText="Dana" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="jpp_dana" />
                        <asp:BoundField DataField="creaby" HeaderText="Dibuat Oleh" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="creaby" />
                        <asp:BoundField DataField="creadate" HeaderText="Dibuat Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="creadate" />
                        <asp:BoundField DataField="updateby" HeaderText="Diubah Oleh" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="updateby" />
                        <asp:BoundField DataField="updatedate" HeaderText="Diubah Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="updatedate" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Ubah Data Bidang Fokus"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="LinkButton1" Visible="false" CommandName="Detil" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Lihat Detil Bobot"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span></asp:LinkButton>

                                <asp:LinkButton runat="server" ID="linkDelete" ToolTip="Hapus Data Bidang Fokus" data-delete='<%# Eval("jpp_id") %>' data-toggle="modal" data-target="#deleteModal"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <asp:Panel runat="server" ID="panelAdd" Visible="false" DefaultButton="btnSubmitAdd">
                 <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddJenisID" ControlToValidate="ddlAddJenis" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddlAddJenis" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Nama"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddJenisPub" ControlToValidate="AddNama" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="AddNama" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Total Dana"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="AddDana" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="addDana" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                            <ajaxToolkit:MaskedEditExtender runat="server"
                                TargetControlID="addDana"
                                Mask="999,999,999"
                                MessageValidatorTip="true"
                                OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError"
                                MaskType="Number"
                                InputDirection="RightToLeft"
                                AcceptNegative="Left"
                                DisplayMoney="Left"
                                ErrorTooltipEnabled="True" />
                </div>
                </div>
                <br />
                <div class="form-group">
                    <asp:LinkButton runat="server" ID="btnCancelAdd" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelAdd_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"></span> Kembali </asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnSubmitAdd" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valAdd" OnClick="btnSubmitAdd_Click"><span aria-hidden="true" class="glyphicon glyphicon-save-file"></span> Simpan</asp:LinkButton>
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="panelEdit" Visible="false" DefaultButton="btnSubmitEdit">
                <asp:Label runat="server" ID="editKode" Visible="false"></asp:Label>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqddlEditJenis" ControlToValidate="ddlEditJenis" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddlEditJenis" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Nama"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="ReqEditNama" ControlToValidate="EditNama" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="EditNama" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Total Dana"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="ReqEditDana" ControlToValidate="EditDana" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="EditDana" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                   
                </div>
                </div>
                <br />

                <br />
                <div class="form-group">
                    <asp:LinkButton runat="server" ID="btnCancelEdit" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelEdit_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"></span> Kembali </asp:LinkButton>
                    <asp:LinkButton runat="server" ID="btnSubmitEdit" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valEdit" OnClick="btnSubmitEdit_Click"><span aria-hidden="true" class="glyphicon glyphicon-save-file"></span> Simpan</asp:LinkButton>
                </div>
            </asp:Panel>

            <asp:Panel runat="server" ID="panelDetil" Visible="false" DefaultButton="btnSubmitEdit">
                <div class="row">
                    <div class="col-lg-4">
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Deskripsi Bidang Fokus"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="reqDetilJudul" ControlToValidate="TextBox1" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" Enabled="false" ID="TextBox1" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                        </div>
                        <div class="form-group">
                            <asp:Button runat="server" ID="Button1" CssClass="btn btn-default" Text="Kembali" OnClick="btnCancelAdd_Click" />
                        </div>
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Dibuat Oleh"></asp:Label>
                            <asp:TextBox Enabled="false" runat="server" ID="TextBox4" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Dibuat Tanggal"></asp:Label>
                            <asp:TextBox Enabled="false" runat="server" ID="TextBox5" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Diubah Oleh"></asp:Label>
                            <asp:TextBox Enabled="false" runat="server" ID="TextBox6" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Dibuat Tanggal"></asp:Label>
                            <asp:TextBox Enabled="false" runat="server" ID="TextBox7" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
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
                    Semua bisnis proses yang memakai nama bidang fokus ini kemungkinan tidak dapat diakses kembali.<br />
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


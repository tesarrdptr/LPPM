<%@ Page Title="Master Reviewer" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Master_Reviewer_2.aspx.cs" Inherits="Page_Master_Reviewer" %>

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
                    <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
                    <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default" OnClick="btnCari_Click"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
                </div>

                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="rev_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowCommand="gridData_RowCommand">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="iduser" HeaderText="Nama Reviewer" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="rev_start_date" HeaderText="Tanggal Mulai" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="rev_end_date" HeaderText="Tanggal Selesai" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="jns_title" HeaderText="Jenis" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="bidfok_name" HeaderText="Bidang Fokus" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="rev_created_by" HeaderText="Dibuat Oleh" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="rev_created_date" HeaderText="Dibuat Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="rev_updated_by" HeaderText="Diubah Oleh" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="rev_updated_date" HeaderText="Diubah Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Ubah Data Reviwer"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>
                                <%--<asp:LinkButton runat="server" ID="LinkButton1" CommandName="Detil" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Lihat Detil Reviewer"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span></asp:LinkButton>--%>
                                <asp:LinkButton runat="server" ID="linkDelete" ToolTip="Hapus Data Tanggal" data-delete='<%# Eval("rev_id") %>' data-toggle="modal" data-target="#deleteModal"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

            <asp:Panel runat="server" ID="panelAdd" Visible="false">
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Nama Reviewer"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddReviewer" ControlToValidate="ddlReviewer" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="ddlReviewer" CssClass="form-control" ValidationGroup="valAdd"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddJenis" ControlToValidate="ddlJenis" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddlJenis" runat="server" CssClass="form-control dropdown" ValidationGroup="valAdd" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlBidfok" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddlBidfok" runat="server" CssClass="form-control dropdown" ValidationGroup="valAdd" />
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Tanggal Mulai"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddTanggalMulai" ControlToValidate="AddTanggalMulai" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" TextMode="Date" ID="AddTanggalMulai" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Tanggal Selesai"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddTanggalSelesai" ControlToValidate="AddTanggalSelesai" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" TextMode="Date" ID="AddTanggalSelesai" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
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
                    <asp:Label runat="server" ID="editRevId" CssClass="control-label" Font-Bold="true" Visible="false"></asp:Label>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Nama Reviewer"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="ReqEditReviewer" ControlToValidate="ddlReviewerEdit" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddlReviewerEdit" Enabled="false" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="ReqEditJenis" ControlToValidate="ddlJenisEdit" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddlJenisEdit" runat="server" CssClass="form-control" />
                </div>
                 <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="ddlBidfokEdit" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddlBidfokEdit" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Tanggal Mulai"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:TextBox runat="server" TextMode="Date" ClientIDMode="Static" ID="editTanggalMulai" CssClass="form-control" MaxLength="50" ValidationGroup="valEdit"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Tanggal Selesai"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:TextBox runat="server" TextMode="Date" ClientIDMode="Static" ID="editTanggalSelesai" CssClass="form-control" MaxLength="50" ValidationGroup="valEdit"></asp:TextBox>
                </div>
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
                            <asp:Label runat="server" CssClass="control-label" Text="Nama Reviewer"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="reqAddJudul" ControlToValidate="TextBox1" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" Enabled="false" ID="TextBox1" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Tanggal Mulai"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="reqAddAbstrak" ControlToValidate="TextBox2" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" Enabled="false" ID="TextBox2" ClientIDMode="Static" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Tanggal Selesai"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="reqKeyword" ControlToValidate="TextBox3" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:TextBox Enabled="false" placeholder="Pisahkan dengan koma" runat="server" ID="TextBox3" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Jenis Reviewer"></asp:Label>
                            <asp:TextBox Enabled="false" runat="server" ID="TextBox4" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                        </div>
                        <div class="form-group">
                            <asp:Button runat="server" ID="Button1" CssClass="btn btn-default" Text="Kembali" OnClick="btnCancelAdd_Click" />
                            <%--                             <asp:Button runat="server" ID="Button2" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valAdd" OnClick="btnSubmitAdd_Click" />--%>
                        </div>
                    </div>
                    <div class="col-lg-6">
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
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Dibuat Tanggal"></asp:Label>
                            <asp:TextBox Enabled="false" runat="server" ID="TextBox8" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
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
                    Semua bisnis proses yang memakai tanggal ini kemungkinan tidak dapat diakses kembali.<br />
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


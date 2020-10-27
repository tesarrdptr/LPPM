﻿<%@ Page Title="Pengajuan Publikasi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_NotifPublikasi.aspx.cs" Inherits="Page_NotifPublikasi" %>

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
           
    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
             <div class="alert alert-danger" runat="server" id="divAlert" visible="false"></div>
            <div class="alert alert-success" runat="server" id="divSuccess" visible="false"></div>

         </ContentTemplate>
    </asp:UpdatePanel>

     <asp:Panel runat="server" ID="panelData" DefaultButton="btnCari">
                <%--<asp:LinkButton runat="server" ID="linkTambah" CssClass="btn btn-primary" OnClick="linkTambah_Click"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Tambah Baru</asp:LinkButton>--%>
                <br />
                <div class="form-group form-inline">
                    <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
                    <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default" OnClick="btnCari_Click"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
                </div>
                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="pub_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowCommand="gridData_RowCommand">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:BoundField DataField="rownum" HeaderText="No. " NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="pub_title" HeaderText="Judul" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="jpb_title" HeaderText="Jenis Publikasi" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <%--<asp:BoundField DataField="atc_name" HeaderText="File" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                        <%--<asp:BoundField DataField="pub_creator" HeaderText="Nama Pengaju" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                        <asp:BoundField DataField="creadate" HeaderText="Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="lnkDownload" CommandArgument='<%# Eval("atc_name") %>' ToolTip="Download"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkTerima" ToolTip="Terima Publikasi" CommandName="Terima" data-delete='<%# Eval("pub_id") %>'  data-toggle="modal" data-target="#deleteModal2"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkTolak" ToolTip="Tolak Publikasi" CommandName="Tolak" data-delete='<%# Eval("pub_id") %>' data-toggle="modal" data-target="#deleteModal"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>

    <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="deleteModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="deleteModalLabel">Tolak Publikasi</h4>
                </div>
                <div class="modal-body">
                    Apakah Anda yakin ingin menolak ?
                         <br />                   <br />
                 <asp:Label runat="server" CssClass="control-label" Text="Alasan"></asp:Label>
                 <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                 <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="TxtTolak" Text="harus diisi" ForeColor="Red" ValidationGroup="valBeforeAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                 <asp:TextBox id="TxtTolak" TextMode="multiline" Columns="30" Rows="3" MaxLength="160" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:TextBox runat="server" ID="txtConfirmDelete" CssClass="hidden" ClientIDMode="Static"></asp:TextBox>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>
                    <asp:LinkButton runat="server" ID="linkConfirmDelete" CssClass="btn btn-danger" Text="Tolak" OnClick="linkConfirmDelete_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

     <div class="modal fade" id="deleteModal2" tabindex="-1" role="dialog" aria-labelledby="deleteModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="deleteModalLabel2">Terima Publikasi</h4>
                </div>
                <div class="modal-body">
                    Apakah Anda yakin ingin menerima ?
                     <br /> <br />
                 <asp:Label runat="server" CssClass="control-label" Text="Alasan"></asp:Label>
                 <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                 <asp:RequiredFieldValidator runat="server" ID="ReqAlasan" ControlToValidate="TxtTerima" Text="harus diisi" ForeColor="Red" ValidationGroup="valBeforeAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                 <asp:TextBox id="TxtTerima" TextMode="multiline" Columns="30" Rows="3" MaxLength="160" runat="server" />
                </div>
                <div class="modal-footer">
                    <asp:TextBox runat="server" ID="txtConfirmTerima" CssClass="hidden" ClientIDMode="Static"></asp:TextBox>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>
                    <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-primary" Text="Terima" OnClick="linkConfirmTerima_Click"></asp:LinkButton>
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
        $('#deleteModal2').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget)
            var data = button.data('delete')
            var modal = $(this)
            modal.find('.modal-footer input').val(data)
        })
    </script>
</asp:Content>

<%@ Page Title="Progress Masuk" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_NotifProgress.aspx.cs" Inherits="Page_NotifProgress" %>

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
                     <b>Status :&nbsp;</b>
                    <asp:DropDownList runat="server" ID="ddStatus" CssClass="form-control dropdown"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
                    <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
                </div>

                 <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="prg_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnRowCommand="gridData_RowCommand" OnPageIndexChanged="gridData_PageIndexChanged" OnRowDataBound="gridData_RowDataBound" OnPageIndexChanging="gridData_PageIndexChanging">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:BoundField DataField="rownum" HeaderText="No. " NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="prg_id" HeaderText="ID Progress" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" Visible="false" />
                        <asp:BoundField DataField="jenis" HeaderText="Jenis Proposal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="prp_judul" HeaderText="Judul Proposal" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <%--<asp:BoundField DataField="atc_name" HeaderText="File" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                        <%--<asp:BoundField DataField="prg_created_by" HeaderText="Pembuat" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                        <asp:BoundField DataField="creadate" HeaderText="Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                         <asp:BoundField DataField="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                 <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="download" CommandArgument='<%# Eval("atc_name") %>' ToolTip="Unduh"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkNilai" CommandName="Nilai" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Beri Nilai"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>     
            </asp:Panel>

    <asp:Panel runat="server" ID="panelNilai" Visible="false" >
        <div id="beriNilai" runat="server"></div>
            <asp:Label runat="server" ID="editKode" Visible="false"></asp:Label>
            <div class="form-group">
                <asp:LinkButton runat="server" ID="btnCancelMonev" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelMonev_Click" ><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"></span> Kembali</asp:LinkButton>
                 <asp:LinkButton runat="server" ID="btnSubmitMonev" CssClass="btn btn-primary" data-toggle="modal" data-target="#insertModal" Text="Simpan" ><span aria-hidden="true" class="glyphicon glyphicon-save-file"></span> Simpan</asp:LinkButton>
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
                    <asp:LinkButton runat="server" ID="linkConfirmInsert" CssClass="btn btn-primary" Text="Simpan" OnClick="linkConfirmInsert_Click" ></asp:LinkButton>
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
</asp:Content>

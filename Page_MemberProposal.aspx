<%@ Page Title="Member Proposal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_MemberProposal.aspx.cs" Inherits="Page_MemberProposal" %>

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
    <asp:Panel runat="server" ID="panelEdit" Visible="true">
        <div class="row">
            <div class="col-lg-12">
                <div class="form-group">
                    <asp:Label runat="server" ID="editPrpID" CssClass="control-label" Font-Bold="true" Visible="false"></asp:Label>
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Bidang Fokus Penelitian :"></asp:Label>
                    <asp:Label runat="server" ID="BidFok" CssClass="form-control" ValidationGroup="valAdd"></asp:Label>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Judul"></asp:Label>
                    <asp:Label runat="server" ID="Judul" CssClass="form-control" MaxLength="100" ValidationGroup="valEdit"></asp:Label>&nbsp;
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis"></asp:Label>
                    <asp:Label ID="JenisID" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Abstrak"></asp:Label>
                    <asp:Label TextMode="MultiLine" runat="server" ID="Abstrak" ClientIDMode="Static" CssClass="form-control" MaxLength="100" ValidationGroup="valEdit"></asp:Label>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Keyword"></asp:Label>
                    <asp:Label runat="server" ID="Keyword" CssClass="form-control" MaxLength="100" ValidationGroup="valEdit"></asp:Label>&nbsp;
                </div>
                    <div class="form-group-lg-8">

                        <asp:Label runat="server" CssClass="control-label" Text="File Proposal"></asp:Label>
                        <asp:LinkButton ClientIDMode="Static" runat="server" ID="linkDownload" OnClick="linkDownload_Click" Text="Download" CommandName="download" ToolTip="Unduh"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span></asp:LinkButton>
                        <asp:Label runat="server" ID="File" CssClass="form-control"></asp:Label>
                    </div>

                
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Total Dana"></asp:Label>
                    <asp:Label runat="server" ID="Dana" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:Label>&nbsp;
                            
                </div>
                <div class="form-group">
                    <%--<asp:Button runat="server" ID="btnCancelEdit" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelEdit_Click" />
                    <asp:Button runat="server" ID="btnSubmitEdit" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valEdit" OnClick="btnSubmitEdit_Click" />--%>
                </div>
            </div>
        </div>
    </asp:Panel>
    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="panelData">
                <br />
                <br />
                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="own_id" EmptyDataText="Tidak ada data"
                    ShowHeaderWhenEmpty="true" OnRowDataBound="gridData_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="own_name" HeaderText="Nama" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="own_leader" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="own_confirmed" />
                    </Columns>
                </asp:GridView>
                <asp:LinkButton CssClass="btn btn-danger btn-sm" runat="server" ID="btnBack" href='javascript:history.go(-1)'><i class="glyphicon glyphicon-chevron-left"></i> Kembali</asp:LinkButton>
                <asp:LinkButton CssClass="btn btn-success" OnClick="btnConfirm_Click" runat="server" ID="btnConfirm">Konfirmasi Proposal <i class="glyphicon glyphicon-check" aria-hidden="true"></i></asp:LinkButton>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


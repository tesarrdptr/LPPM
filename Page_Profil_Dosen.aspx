<%@ Page Title="Beranda" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Profil_Dosen.aspx.cs" Inherits="_Default" %>

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
    <asp:Label runat="server" ID="coba" />
    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>

            <asp:Panel runat="server" ID="panelProfil">
                <div class="row">
                    <div class="col-lg-4">
                        <asp:Image ID="ppDosen" runat="server" ImageUrl="~/Images/profildosen.png" Width="250px" Height="400px" />
                    </div>
                    <div class="col-lg-6">
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Nama :"></asp:Label>
                            <asp:Label runat="server" ID="nama" CssClass="form-control" ValidationGroup="valAdd"></asp:Label>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Prodi :"></asp:Label>
                            <asp:Label runat="server" ID="prodi" CssClass="form-control" ValidationGroup="valAdd"></asp:Label>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Email :"></asp:Label>
                            <asp:Label runat="server" ID="Email" CssClass="form-control" ValidationGroup="valAdd"></asp:Label>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Posisi :"></asp:Label>
                            <asp:Label runat="server" ID="Posisi" CssClass="form-control" ValidationGroup="valAdd"></asp:Label>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Alamat :"></asp:Label>
                            <asp:Label runat="server" ID="Alamat" CssClass="form-control" ValidationGroup="valAdd"></asp:Label>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Tempat, Tanggal Lahir :"></asp:Label>
                            <asp:Label runat="server" ID="ttl" CssClass="form-control" ValidationGroup="valAdd"></asp:Label>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Pendidikan Terakhir :"></asp:Label>
                            <asp:Label runat="server" ID="Pendidikan" CssClass="form-control" ValidationGroup="valAdd"></asp:Label>&nbsp;
                        </div>
                    </div>
                </div>
                <%--Selamat datang di Sistem Informasi Pengelolaan Proposal LPPM Politeknik Manufatur Astra.
                <br />
                Silahkan klik pada menu di atas untuk memulai menggunakan sistem ini.
                <br />
                <br />
                Panduan penggunaan Sistem Informasi LPPM dapat diunduh <a href="USERGUIDE_LPPM.pdf" target="_blank"><b>Klik Disini</b></a><br />--%>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

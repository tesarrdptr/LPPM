<%@ Page Title="Beranda" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

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
          
            <asp:Panel runat="server" ID="panelWelcome" Visible="false">
                Selamat datang di Sistem Informasi Pengelolaan Proposal LPPM Politeknik Manufatur Astra.
                <br />
                Silahkan klik pada menu di atas untuk memulai menggunakan sistem ini.
                <br /><br />
                Panduan penggunaan Sistem Informasi LPPM dapat diunduh <a href="USERGUIDE_LPPM.pdf"  target="_blank"><b>Klik Disini</b></a><br />
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>
 
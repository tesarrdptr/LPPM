<%@ Page Title="Beranda" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Default.aspx.cs" Inherits="_Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="alert alert-danger" runat="server" id="divAlert" visible="false"></div>
    <div class="alert alert-success" runat="server" id="divSuccess" visible="false"></div>
    <h3>Beranda</h3>
    <br />
    <asp:Literal runat="server" ID="literal1"></asp:Literal>
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
            <asp:Literal runat="server" ID="literalContent"></asp:Literal>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

<%@ Page Title="Masuk" Language="C#" MasterPageFile="~/SiteSSO.Master" AutoEventWireup="true" CodeFile="Page_Login.aspx.cs" Inherits="Page_Login" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <br />

    <div class="alert alert-danger" runat="server" id="divAlert" visible="false"></div>
    <div class="alert alert-success" runat="server" id="divSuccess" visible="false"></div>
    <br />

    <div class="img-circle" style="text-align: center;margin-bottom: -20px;margin-right: 340px;">
        <img src="Images/IMG_Logo.png" style="width:340px; position:absolute;" />
    </div>
    <div class="panel panel-default center" style="width: 400px;">
        <div class="panel-body" style="padding-bottom: 20px; padding-top: 70px;">
            <br />
            <br />
            <br />
            <div class="form-group text-center">
                <asp:RequiredFieldValidator runat="server" ID="reqTxtUsername" ControlToValidate="txtUsername" Text="Nama akun harus diisi" ForeColor="Red" ValidationGroup="valLogin" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:TextBox runat="server" ID="txtUsername" CssClass="form-control text-center center" placeholder="Nama Akun"></asp:TextBox>
            </div>
           <div class="form-group text-center">
                <asp:RequiredFieldValidator runat="server" ID="reqTxtPassword" ControlToValidate="txtPassword" Text="Kata Sandi harus diisi" ForeColor="Red" ValidationGroup="valLogin" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:TextBox runat="server" ID="txtPassword" CssClass="form-control text-center center" placeholder="Kata Sandi"></asp:TextBox>
            </div>
            <div class="form-group">
                <asp:Button runat="server" ID="btnLogin" CssClass="btn btn-block btn-primary text-center center" Text="Masuk" OnClick="btnLogin_Click" ValidationGroup="valLogin" />
            </div>
        </div>
    </div>

    <br />
</asp:Content>

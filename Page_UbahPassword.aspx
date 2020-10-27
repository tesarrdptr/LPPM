<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_UbahPassword.aspx.cs" Inherits="Page_UbahPassword" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Ubah Password</h3>
    <asp:Panel runat="server" ID="panelNilai" Visible="false" >
        <div id="beriNilai" runat="server"></div>
            <asp:Label runat="server" ID="editKode" Visible="false"></asp:Label>
            <div class="form-group">
                <asp:LinkButton runat="server" ID="btnCancelChange" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelChange_Click" ><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"></span> Kembali</asp:LinkButton>
                 <asp:LinkButton runat="server" ID="btnSubmitChange" CssClass="btn btn-primary" data-toggle="modal" data-target="#insertModal" Text="Simpan" ><span aria-hidden="true" class="glyphicon glyphicon-save-file"></span> Simpan</asp:LinkButton>
            </div>
     </asp:Panel>
    <div class="alert alert-success" runat="server" id="divSuccess" visible="false"></div>
    <div class="alert alert-danger" runat="server" id="divAlert" visible="false"></div>
    <asp:Panel runat="server" ID="panelEdit" Visible="true" DefaultButton="btnSubmitAdd">
            <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Password Lama"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:TextBox runat="server" TextMode="Password" ID="passwordLama" CssClass="form-control" MaxLength="50"></asp:TextBox>&nbsp;
            </div>
        <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Password Baru"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:TextBox runat="server" TextMode="Password" ID="passwordBaru" CssClass="form-control" MaxLength="50"></asp:TextBox>&nbsp;
            </div>
        <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Konfirmasi Password Baru"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:TextBox runat="server" TextMode="Password" ID="konfPasswordBaru" CssClass="form-control" MaxLength="50"></asp:TextBox>&nbsp;
            </div>
        <div class="form-group">
            <asp:Button runat="server" ID="btnCancelAdd" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelAdd_Click" />
            <asp:Button runat="server" ID="btnSubmitAdd" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valAdd" OnClick="btnSubmitAdd_Click"/>
        </div>
    </asp:Panel>
</asp:Content>

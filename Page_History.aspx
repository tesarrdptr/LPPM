<%@ Page Title="Kelola Proposal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_History.aspx.cs" Inherits="Page_History" %>

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

    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <asp:Panel runat="server" ID="panelData" DefaultButton="btnCari">
               
                <div class="form-group form-inline">
                    <asp:Label runat="server" CssClass="control-label" Text="Pilih Riwayat "></asp:Label>&nbsp;
                    <asp:DropDownList runat="server" ID="cariHistory" CssClass="form-control dropdown" ValidationGroup="valCari"></asp:DropDownList>&nbsp;
                    <asp:RequiredFieldValidator runat="server" ID="reqCariHistory" ControlToValidate="cariHistory" Text="*" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:Label runat="server" CssClass="control-label" Text="Pilih Pengguna "></asp:Label>&nbsp;
                    <asp:TextBox runat="server" ID="cariUser" CssClass="form-control" MaxLength="100" ValidationGroup="valCari"></asp:TextBox>&nbsp;
                    <asp:RequiredFieldValidator runat="server" ID="ReqCariUser" ControlToValidate="cariUser" Text="*" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:Button runat="server" ID="btnCari" CssClass="btn btn-default" Text="Cari" ValidationGroup="valCari" OnClick="btnCari_Click" />
                </div> 
                <br />

                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="false"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="his_id" EmptyDataText="Tidak ada data"
                    ShowHeaderWhenEmpty="true" ShowHeader="true" OnRowDataBound="gridData_RowDataBound" OnPreRender="gridData_PreRender">
                    <Columns>
                        <asp:BoundField DataField="his_icon" HeaderText="Icon" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="his_created_by" HeaderText="Created By" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="his_text" HeaderText="Text" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="his_created_date" HeaderText="Created Date" NullDisplayText="-" ItemStyle-HorizontalAlign="Right" />                     
                    </Columns>
                </asp:GridView>
            </asp:Panel>

           
        </ContentTemplate>
    </asp:UpdatePanel>

</asp:Content>


<%@ Page Title="Notifikasi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Notifikasi.aspx.cs" Inherits="Page_Notifikasi" %>

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
            <div class="alert alert-danger" runat="server" id="divAlert" visible="false"></div>
            <div class="alert alert-success" runat="server" id="divSuccess" visible="false"></div>

            <asp:Panel runat="server" ID="panelData" DefaultButton="btnCari">
                <div class="form-group form-inline">
                    <b>Status :&nbsp;</b>
                    <asp:DropDownList runat="server" ID="ddStatus" CssClass="form-control dropdown"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default" OnClick="btnCari_Click"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
                </div>

                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="idnotifikasi" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowCommand="gridData_RowCommand" OnRowDataBound="gridData_RowDataBound">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:BoundField DataField="rownum" HeaderText="No" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" />
                        <asp:BoundField DataField="dari" HeaderText="Dari" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" />
                        <asp:BoundField DataField="pesan" HeaderText="Pesan" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" HtmlEncode="false" />
                        <asp:BoundField DataField="creadate" HeaderText="Waktu" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false" />
                        <asp:BoundField DataField="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" ItemStyle-Wrap="false"/>
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton visible='<%#Eval("status").ToString()== "Belum Dibaca" %>' runat="server" ID="linkRead" CommandName="Baca" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Set Sudah Dibaca"><span class="glyphicon glyphicon-ok" aria-hidden="true"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </asp:Panel>
        </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>


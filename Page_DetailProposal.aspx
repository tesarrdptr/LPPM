<%@ Page Title="Detail Proposal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_DetailProposal.aspx.cs" Inherits="Page_DetailProposal" %>

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
            
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnDownload" />
            <asp:PostBackTrigger ControlID="confirmGrade" />
            <%--<asp:PostBackTrigger ControlID="btnSubmitAdd" />--%>
            <%--<asp:PostBackTrigger ControlID="linkDownload" />--%>
            <%--<asp:PostBackTrigger ControlID="linkDownload" />--%>
        </Triggers>
    </asp:UpdatePanel>

    <asp:Panel runat="server" ID="panelData">
                <h1 runat="server" id="txtTitle" style="text-align:center;color:#4a4a4a">Judul Proposal</h1>
                <p runat="server" id="txtAuthor" style="text-align:center;color:#4a4a4a;margin-top:-10px">Pengarang</p>
                <b>Abstraks</b>
                <p runat="server" id="txtAbstraks" style="text-align:justify">Abstraksnya sedkit mantap sih</p>
                <hr />
                <i>Keywords : <span runat="server" id="txtKeywords">a, b, c, d</span></i>
                <br /><br />
                
                <asp:LinkButton CssClass="btn btn-danger btn-sm" runat="server" id="btnBack" href='javascript:history.go(-1)'><i class="glyphicon glyphicon-chevron-left"></i> Back</asp:LinkButton>
                <asp:LinkButton CssClass="btn btn-primary btn-sm" runat="server" id="btnDownload" OnClick="btnDownload_Click1" ToolTip="Download Proposal" ClientIDMode="Static">Download Proposal (<span runat="server" id="txtSize"></span>) <i class="glyphicon glyphicon-download-alt" aria-hidden="true"></i></asp:LinkButton>
                <asp:LinkButton CssClass="btn btn-primary btn-sm" runat="server" id="btnGrade" data-toggle="modal" data-target="#reviewModal" ToolTip="Review Proposal">Review Proposal <i class="glyphicon glyphicon-pencil" aria-hidden="true"></i></asp:LinkButton>
                    
            </asp:Panel>
    <div class="modal fade" id="reviewModal" tabindex="-1" role="dialog" aria-labelledby="reviewModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="reviewModalLabel">Form Penilaian</h4>
                </div>
                <div class="modal-body">
                    <div class="row">
                    <% List<bobot> bobots = getBobots(); %>
                    <% foreach (var data in bobots)
                        {%>
                        <span class="col-sm-8"><%=data.name %> (<%=data.percent %>)</span>
                        <div class="col-sm-4">
                            <input type="number" id="bobot_<%=data.id %>" value="0" name="bobot_<%=data.id %>" class="form-control" max="100" min="0" />
                        </div>
                    <% } %>
                    </div>
                    
                    <div class="form-group" style="margin-bottom: -10px">
                        <asp:Label runat="server" CssClass="control-label" Text="Komentar"></asp:Label>
                        <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                        <asp:RequiredFieldValidator runat="server" ID="reqKomentar" ControlToValidate="komentar" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                        <asp:TextBox TextMode="MultiLine" Height="200px" runat="server" ID="komentar" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                    </div>
                </div>
                <div class="modal-footer">
                    <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>
                    <asp:LinkButton ClientIDMode="Static" runat="server" ID="confirmGrade" CssClass="btn btn-primary" Text="Simpan" OnClick="confirmGrade_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $('#reviewModal').on('show.bs.modal', function (event) {
           
        })
    </script>
</asp:Content>


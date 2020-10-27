<%@ Page Title="Proposal" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_KelolaProposal.aspx.cs" Inherits="Page_KelolaProposal" EnableEventValidation="false" %>

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
            <asp:PostBackTrigger ControlID="btnSubmitAdd" />
            <%--<asp:PostBackTrigger ControlID="linkDownload" />--%>
        </Triggers>
    </asp:UpdatePanel>
    <asp:Panel runat="server" ID="panelData" DefaultButton="linkTambah">
        <asp:LinkButton Visible="true" runat="server" ID="linkTambah" CssClass="btn btn-primary" OnClick="linkTambah_Click"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Tambah Baru</asp:LinkButton>
        <br />
        <br />
        <div class="form-group form-inline">
            <asp:DropDownList runat="server" ID="ddStatus" CssClass="form-control dropdown"></asp:DropDownList>&nbsp;&nbsp;
            <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Nomor proposal, judul, bidang fokus, jenis, ketua"></asp:TextBox>
            <br />
            <div class="form-group">
                <label>Tanggal Awal :</label>

                <div class="row">
                    <div class='col-sm-6'>
                        <asp:TextBox runat="server" ID="tglMulai" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                </div>

            </div>
            <div class="form-group">
                <label>Tanggal  Akhir :</label>

                <div class="row">
                    <div class='col-sm-6'>
                        <asp:TextBox runat="server" ID="tglSelesai" CssClass="form-control" TextMode="Date"></asp:TextBox>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label style="color: white;">a </label>

                <div class="row">
                    <div class='col-sm-6'>
                        <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
                    </div>
                </div>
            </div>
            <div class="form-group">
                <label style="color: white;">a </label>

                <div class="row">
                    <div class='col-sm-6'>
                        <asp:Button Text="Cetak" CssClass="btn btn-default" OnClick="ExportExcel" runat="server" />
                    </div>
                </div>
            </div>

        </div>
        <!-- export dat excel -->
        <script data-require="angular.js@*" data-semver="2.0.0" src="Scripts/angular.js
"></script>
        <script data-require="jquery@*" data-semver="2.1.4" src="Scripts/jquery-2.1.4.js"></script>
        <script>
            // Code goes here
            var myApp = angular.module("myApp", []);
            myApp.factory('Excel', function ($window) {
                var uri = 'data:application/vnd.ms-excel;base64,',
                    template = '<html xmlns:o="urn:schemas-microsoft-com:office:office" xmlns:x="urn:schemas-microsoft-com:office:excel" xmlns="http://www.w3.org/TR/REC-html40"><head><!--[if gte mso 9]><xml><x:ExcelWorkbook><x:ExcelWorksheets><x:ExcelWorksheet><x:Name>{worksheet}</x:Name><x:WorksheetOptions><x:DisplayGridlines/></x:WorksheetOptions></x:ExcelWorksheet></x:ExcelWorksheets></x:ExcelWorkbook></xml><![endif]--></head><body><table>{table}</table></body></html>',
                    base64 = function (s) { return $window.btoa(unescape(encodeURIComponent(s))); },
                    format = function (s, c) { return s.replace(/{(\w+)}/g, function (m, p) { return c[p]; }) };
                return {
                    tableToExcel: function (tableId, worksheetName) {
                        var table = $(tableId),
                            ctx = { worksheet: worksheetName, table: table.html() },
                            href = uri + base64(format(template, ctx));
                        return href;
                    }
                };
            })
                .controller('MyCtrl', function (Excel, $timeout, $scope) {
                    $scope.exportToExcel = function (tableId) { // ex: '#my-table'
                        var exportHref = Excel.tableToExcel(tableId, 'WireWorkbenchDataExport');
                        $timeout(function () { location.href = exportHref; }, 100); // trigger download
                    }
                });
        </script>

        <div ng-controller="MyCtrl" ng-app="myApp">
            <div id="tableToExport">
                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="true" AutoGenerateColumns="false" DataKeyNames="prp_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowCommand="gridData_RowCommand"
                    OnSorting="gridData_Sorting">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="prp_nomor" SortExpression="prp_nomor" HeaderText="Nomor Proposal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="prp_judul" SortExpression="prp_judul" HeaderText="Judul Proposal" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="bidfok_name" SortExpression="bidfok_name" HeaderText="Bidang Fokus" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="jns_title" SortExpression="jns_title" HeaderText="Jenis" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="jpp_nama" SortExpression="jpp_nama" HeaderText="Sub Jenis" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="ketua" SortExpression="ketua" HeaderText="Ketua Pengusul" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <%--<asp:BoundField DataField="prp_keyword" SortExpression="prp_keyword" HeaderText="Keyword" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                        <asp:BoundField DataField="prp_total_rab" DataFormatString="Rp. {0:###,###,###.00}" SortExpression="prp_total_rab" HeaderText="Total Dana" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="mhs" SortExpression="mhs" HeaderText="Total Mahasiswa" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="prp_submit" SortExpression="prp_submit" HeaderText="Tanggal Kirim" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="status" SortExpression="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <%--<asp:BoundField DataField="own_leader" SortExpression="status" HeaderText="Leader" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />--%>

                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:LinkButton Visible='<%#Eval("prp_status").ToString()== "0" && Eval("own_leader").ToString() == "1" %>' ClientIDMode="Static" runat="server" ID="LinkKirim" CommandName="Kirim" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Kirim Proposal"><span class="glyphicon glyphicon-send" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkMember" href='<%# Page.ResolveUrl("Page_MemberProposal?id="+Eval("prp_id").ToString()) %>' ToolTip="Anggota Proposal"><span class="glyphicon glyphicon-user" aria-hidden="true"></span></asp:LinkButton>
                                <%--<asp:LinkButton runat="server" ID="linkGrade" href='<%# Page.ResolveUrl("Page_DetailProposal?id="+Eval("prp_id").ToString()) %>' ToolTip="Detail Proposal"><span class="glyphicon glyphicon-send" aria-hidden="true"></span></asp:LinkButton>--%>
                                <asp:LinkButton ClientIDMode="Static" runat="server" ID="linkDownload" CommandArgument='<%# Eval("atc_id") %>' Text="Download" CommandName="download" ToolTip="Unduh"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton Visible='<%#Eval("prp_status").ToString() == "0" && Eval("own_leader").ToString() == "1" %>' Enabled='<%#Eval("own_leader").ToString() == "1" %>' ClientIDMode="Static" runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Ubah Proposal"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton Visible='<%#Eval("prp_status").ToString() == "0" && Eval("own_leader").ToString() == "1" %>' ClientIDMode="Static" runat="server" ID="linkDelete" ToolTip="Hapus Proposal" data-delete='<%# Eval("prp_id") %>' data-toggle="modal" data-target="#deleteModal" OnClientClick="deleteyuk(this)"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton Visible='<%#Eval("prp_status").ToString() == "3" %>' runat="server" ID="linkLihat" CommandName="Lihat_Nilai" ToolTip="Lihat Nilai"><span class="glyphicon glyphicon-list" aria-hidden="true"></span></asp:LinkButton>
                                <%--<asp:LinkButton Visible='<%#Eval("prp_status").ToString()== "0" %>' ClientIDMode="Static" runat="server" ID="LinkButton2" ToolTip="Hapus Proposal" data-delete='<%# Eval("prp_id") %>' data-toggle="modal" data-target="#deleteModal" OnClientClick="deleteyuk(this)"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <%--<asp:TemplateField HeaderText="Nomor" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="150px">
                            <ItemTemplate>
                                LPPM/<%# DateTime.Now.Year %>/<%# ProcessNomorProposal(Container.DataItemIndex + 1) %><%# Container.DataItemIndex + 1 %>
                            </ItemTemplate>
                        </asp:TemplateField>--%>
                    </Columns>
                </asp:GridView>
                <%--<asp:Button runat="server" ID="btnExcel" ClientIDMode="Static" CssClass="btn btn-success" Text="Simpan ke Excel"  ng-click="exportToExcel('#tableToExport')" />--%>
            </div>
        </div>

        <asp:GridView runat="server" ID="GridView1" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
            AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="prp_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" Visible="false" OnRowCommand="gridData_RowCommand">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
            <Columns>

                <asp:BoundField DataField="prp_judul" HeaderText="Judul" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="jns_title" HeaderText="Jenis" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="prp_abstrak" HeaderText="Abstrak" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="prp_keyword" HeaderText="Keyword" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="prp_status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
            </Columns>
        </asp:GridView>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelAdd" Visible="false" DefaultButton="btnSubmitAdd">
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Bidang Fokus Penelitian :"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqBFP" ControlToValidate="addBidangFokus" Text="harus dipilih" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="addBidangFokus" CssClass="form-control" ValidationGroup="valAdd"></asp:DropDownList>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Judul"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddJudul" ControlToValidate="addJudul" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="AddJudul" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <asp:UpdatePanel ID="UpdatePanel4" runat="server">
                    <ContentTemplate>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Jenis"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="reqAddJenisID" ControlToValidate="addJenisID" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="addJenisID" runat="server" CssClass="form-control" AutoPostBack="true" OnSelectedIndexChanged="addJenisID_SelectedIndexChanged" />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Sub Jenis"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="addSubJenis" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="addSubJenis" Enabled="false" runat="server" CssClass="form-control" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>


                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Abstrak"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <span id="countword" class="label label-default" style="position: relative; top: -3px; left: 200px">0</span>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddAbstrak" ControlToValidate="addAbstrak" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox TextMode="MultiLine" runat="server" ID="addAbstrak" ClientIDMode="Static" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Keyword"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqKeyword" ControlToValidate="addKeyword" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox placeholder="Pisahkan dengan koma" runat="server" ID="addKeyword" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Upload File ( Max :10 MB|| File : PDF )"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddUpload" ControlToValidate="addUpload" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:FileUpload runat="server" ID="addUpload" CssClass="form-control" ValidationGroup="valAdd" onchange="cekImage(this, 'proposal');"></asp:FileUpload>
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Total Dana"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqDana" ControlToValidate="addDana" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="addDana" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd" onkeypress="return isNumber(event)"></asp:TextBox>&nbsp;
                            
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnCancelAdd" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelAdd_Click" />
                    <asp:Button runat="server" ID="btnSubmitAdd" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valAdd" OnClick="btnSubmitAdd_Click" />
                </div>
            </div>
            <div class="col-lg-6">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Anggota Proposal 1"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="reqvalAnggota" ControlToValidate="addMemberDL1" Text="harus dipilih minimal 1 anggota" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList runat="server" ID="addMemberDL1" CssClass="form-control" ValidationGroup="valAdd" OnSelectedIndexChanged="addMemberDL1_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>&nbsp;
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Anggota Proposal 2"></asp:Label>
                            <asp:DropDownList Enabled="false" runat="server" ID="addMemberDL2" CssClass="form-control" ValidationGroup="valAdd" OnSelectedIndexChanged="addMemberDL2_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Anggota Proposal 3"></asp:Label>
                            <asp:DropDownList Enabled="false" runat="server" ID="addMemberDL3" CssClass="form-control" ValidationGroup="valAdd" OnSelectedIndexChanged="addMemberDL3_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Anggota Proposal 4"></asp:Label>
                            <asp:DropDownList Enabled="false" runat="server" ID="addMemberDL4" CssClass="form-control" ValidationGroup="valAdd" OnSelectedIndexChanged="addMemberDL4_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>&nbsp;
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Total Mahasiswa"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="AddMhs" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="AddMhs" CssClass="form-control" MaxLength="100" ValidationGroup="valEdit" onkeypress="return isNumber(event)"></asp:TextBox>&nbsp;
                            
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelEdit" Visible="false" DefaultButton="btnSubmitEdit">
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group">
                    <asp:Label runat="server" ID="editPrpID" CssClass="control-label" Font-Bold="true" Visible="false"></asp:Label>
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Bidang Fokus Penelitian :"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="editBidFok" Text="harus dipilih" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="editBidFok" CssClass="form-control" ValidationGroup="valAdd"></asp:DropDownList>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Judul"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="ReqEditJudul" ControlToValidate="editJudul" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="editJudul" CssClass="form-control" MaxLength="100" ValidationGroup="valEdit"></asp:TextBox>&nbsp;
                </div>
                <asp:UpdatePanel ID="UpdatePanel5" runat="server">
                    <ContentTemplate>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Jenis"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="ReqEditJenisID" ControlToValidate="editJenisID" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="editJenisID" runat="server" CssClass="form-control" OnSelectedIndexChanged="editJenisID_SelectedIndexChanged" />
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Sub Jenis"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="addSubJenis" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList ID="editSubJenis" runat="server" CssClass="form-control" />
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Abstrak"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <span id="countwordEdit" class="label label-default" style="position: relative; top: -3px; left: 200px">0</span>
                    <asp:RequiredFieldValidator runat="server" ID="ReqEditAbstark" ControlToValidate="editAbstrak" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox TextMode="MultiLine" runat="server" ID="editAbstrak" ClientIDMode="Static" CssClass="form-control" MaxLength="100" ValidationGroup="valEdit"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Keyword"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="ReqEditKeyword" ControlToValidate="editKeyword" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="editKeyword" CssClass="form-control" MaxLength="100" ValidationGroup="valEdit"></asp:TextBox>&nbsp;
                </div>
                <%--           <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Upload File"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="addUpload" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:FileUpload runat="server" ID="FileUpload1" CssClass="form-control" ValidationGroup="valAdd" onchange="cekImage(this, 'proposal');" ></asp:FileUpload>
                        </div>--%>
                <%--<div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Upload File ( Max :10 MB|| File : PDF )"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqEditUpload" ControlToValidate="editUpload" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:FileUpload runat="server" ID="editUpload" CssClass="form-control" ValidationGroup="valAdd" onchange="cekImage(this, 'proposal');"></asp:FileUpload>
                </div>--%>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Total Dana (Rp.)"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqEditDana" ControlToValidate="editDana" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="editDana" CssClass="form-control" MaxLength="100" ValidationGroup="valEdit" onkeypress="return isNumber(event)"></asp:TextBox>&nbsp;
                            
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnCancelEdit" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelEdit_Click" />
                    <asp:Button runat="server" ID="btnSubmitEdit" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valEdit" OnClick="btnSubmitEdit_Click" />
                </div>
            </div>
            <div class="col-lg-6">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Anggota Proposal 1"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="editMemberDL1" Text="harus dipilih minimal 1 anggota" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:DropDownList runat="server" ID="editMemberDL1" CssClass="form-control" ValidationGroup="valEdit" OnSelectedIndexChanged="editMemberDL1_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>&nbsp;
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Anggota Proposal 2"></asp:Label>
                            <asp:DropDownList runat="server" ID="editMemberDL2" CssClass="form-control" ValidationGroup="valEdit" OnSelectedIndexChanged="editMemberDL2_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Anggota Proposal 3"></asp:Label>
                            <asp:DropDownList runat="server" ID="editMemberDL3" CssClass="form-control" ValidationGroup="valEdit" OnSelectedIndexChanged="editMemberDL3_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Anggota Proposal 4"></asp:Label>
                            <asp:DropDownList runat="server" ID="editMemberDL4" CssClass="form-control" ValidationGroup="valEdit" OnSelectedIndexChanged="editMemberDL4_SelectedIndexChanged" AutoPostBack="true"></asp:DropDownList>&nbsp;
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Total Mahasiswa"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="editMhs" Text="harus diisi" ForeColor="Red" ValidationGroup="valEdit" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="editMhs" CssClass="form-control" MaxLength="100" ValidationGroup="valEdit" onkeypress="return isNumber(event)"></asp:TextBox>&nbsp;
                            
                        </div>
                        <asp:Label Visible="false" runat="server" CssClass="control-label" ID="idAnggota1" Text="Anggota"></asp:Label>
                        <asp:Label Visible="false" runat="server" CssClass="control-label" ID="idAnggota2" Text="Anggota"></asp:Label>
                        <asp:Label Visible="false" runat="server" CssClass="control-label" ID="idAnggota3" Text="Anggota"></asp:Label>
                        <asp:Label Visible="false" runat="server" CssClass="control-label" ID="idAnggota4" Text="Anggota"></asp:Label>
                        <asp:Label Visible="false" runat="server" CssClass="control-label" ID="idnya1" Text="Anggota"></asp:Label>
                        <asp:Label Visible="false" runat="server" CssClass="control-label" ID="idnya2" Text="Anggota"></asp:Label>
                        <asp:Label Visible="false" runat="server" CssClass="control-label" ID="idnya3" Text="Anggota"></asp:Label>
                        <asp:Label Visible="false" runat="server" CssClass="control-label" ID="idnya4" Text="Anggota"></asp:Label>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
            <div class="col-lg-6">

                <%-- <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Anggota Proposal 1"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlEditAnggota1" CssClass="form-control" ValidationGroup="valEdit"></asp:DropDownList>&nbsp;
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Anggota Proposal 2"></asp:Label>
                            <asp:DropDownList runat="server" ID="ddlEditAnggota2" CssClass="form-control" ValidationGroup="valEdit"></asp:DropDownList>&nbsp;
                        </div>--%>
                <asp:GridView Visible="false" runat="server" ID="grdAnggotaProposal" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="own_id" EmptyDataText="Tidak ada data"
                    ShowHeaderWhenEmpty="true" OnRowDataBound="grdAnggotaProposal_RowDataBound">
                    <Columns>
                        <asp:BoundField DataField="own_name" HeaderText="Nama Anggota" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="own_confirmed" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="center" />
                        <asp:BoundField DataField="own_confirmed" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>


    <div class="modal fade" id="deleteModal" tabindex="-1" role="dialog" aria-labelledby="deleteModalLabel">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="deleteModalLabel">Hapus Data</h4>
                </div>
                <div class="modal-body">
                    Apakah Anda yakin?
                </div>
                <div class="modal-footer">
                    <asp:TextBox runat="server" ID="txtConfirmDelete" ClientIDMode="Static" CssClass="hidden"></asp:TextBox>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>
                    <asp:LinkButton runat="server" ID="linkConfirmDelete" CssClass="btn btn-danger" Text="Hapus" OnClick="linkConfirmDelete_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">



        function deleteyuk(coba) {
            $("#deleteModal").find('.modal-footer input').val($(coba).data("delete"));
        }

        function WordCount(str) {
            var a = str.split(" ");
            var b = 0;
            for (var i = 0; i < a.length; i++) {
                if (/[a-z]/i.test(a[i])) b++;
            }
            return b;
        }

        $("#addAbstrak").keyup(function (e) {
            var total = WordCount($(this).val());
            $("#countword").html(total);
            if (total > 500) $("#countword").removeClass("label-warning").removeClass("label-default").addClass("label-danger");
            else if (total > 400) $("#countword").removeClass("label-default").removeClass("label-danger").addClass("label-warning");
            else $("#countword").removeClass("label-warning").removeClass("label-danger").addClass("label-default");
        });
        $("#editAbstrak").keyup(function (e) {
            var total = WordCount($(this).val());
            $("#countwordEdit").html(total);
            if (total > 500) $("#countwordEdit").removeClass("label-warning").removeClass("label-default").addClass("label-danger");
            else if (total > 400) $("#countwordEdit").removeClass("label-default").removeClass("label-danger").addClass("label-warning");
            else $("#countwordEdit").removeClass("label-warning").removeClass("label-danger").addClass("label-default");
        });

        $("form").submit(function (e) {
            if (WordCount($("#addAbstrak").val()) > 500) {
                e.preventDefault();
                swal({
                    title: "Opps,",
                    text: "Maksimal abstraks adalah 500 kata!",
                    type: "error"
                });
            }
        });

        function cekImage(param, deskripsi) {
            var input, file, valid = true;
            input = param;
            file = input.files[0];
            if (file.size / 1024 > 10240) {
                alert("Opps! Berkas " + deskripsi + " terlalu besar! Ukuran maksimal berkas yang bisa dikirim adalah 10 MB");
                valid = false;
            }
            var a = input.value.split(".").pop();
            if (a.toLowerCase() != "pdf") {
                alert("Opps! Format berkas " + deskripsi + " yang dibolehkan adalah .pdf");
                valid = false;
            }
            if (!valid) {
                param.value = "";
            }
        }
        function isNumber(evt) {
            evt = (evt) ? evt : window.event;
            var charCode = (evt.which) ? evt.which : evt.keyCode;
            if (charCode > 31 && (charCode < 48 || charCode > 57)) {
                return false;
            }
            return true;
        }
    </script>

</asp:Content>


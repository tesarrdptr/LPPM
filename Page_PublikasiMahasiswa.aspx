<%@ Page Title="Publikasi Saya" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_PublikasiMahasiswa.aspx.cs" Inherits="Page_PublikasiMahasiswa" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
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
        </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="btnUpload" />
            <asp:PostBackTrigger ControlID="LinkButton3" />
            <%--<asp:PostBackTrigger ControlID="lnkDownload"/>--%>
        </Triggers>
    </asp:UpdatePanel>

    <asp:Panel runat="server" ID="panelData" DefaultButton="btnCari">
        <asp:LinkButton runat="server" ID="linkTambah" CssClass="btn btn-primary" OnClick="linkTambah_KLIK"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Tambah Baru</asp:LinkButton>
        <br />
        <br />

        <!-- export dat excel -->
        <script data-require="angular.js@*" data-semver="2.0.0" src="Scripts/angular.js"></script>
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

        <div class="form-group form-inline">
            <b>Status :&nbsp;</b>
            <asp:DropDownList runat="server" ID="ddStatus" CssClass="form-control dropdown"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
            <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default" OnClick="btnCari_Click"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
        </div>
        <div ng-controller="MyCtrl" ng-app="myApp">
            <div id="tableToExport">
                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="jpb_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowDataBound="gridData_RowDataBound" OnRowCommand="gridData_RowCommand">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:BoundField DataField="rownum" HeaderText="No. " NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="pub_title" HeaderText="Judul Publikasi" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="jpb_title" HeaderText="Jenis Publikasi" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="creadate" HeaderText="Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="pub_komentar" HeaderText="Komentar" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="download" CommandArgument='<%# Eval("atc_id") %>' ToolTip="Unduh"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton Visible='<%#Eval("status").ToString()== "Belum Dikirim" %>' runat="server" ID="linkGrade" data-delete='<%# Eval("pub_id") %>' data-toggle="modal" data-target="#deleteModal1" OnClientClick="deleteyuk1(this)" ToolTip="Diajukan Ke Reviewer"><span class="glyphicon glyphicon-send" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton Visible='<%#Eval("status").ToString()== "Belum Dikirim" %>' ClientIDMode="Static" runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# Eval("pub_id") %>' ToolTip="Ubah Publikasi"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton Visible='<%#Eval("status").ToString()== "Belum Dikirim" %>' ClientIDMode="Static" runat="server" ID="linkDelete" ToolTip="Hapus Progress Penelitian" data-delete='<%# Eval("pub_id") %>' data-toggle="modal" data-target="#deleteModal" OnClientClick="deleteyuk(this)"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
                <asp:Button runat="server" ID="btnExcel" ClientIDMode="Static" CssClass="btn btn-success" Text="Simpan ke Excel" ng-click="exportToExcel('#tableToExport')" />

            </div>
        </div>
    </asp:Panel>
    <asp:Panel runat="server" ID="panelAdd" Visible="false" DefaultButton="btnUpload">
        <asp:Label runat="server" CssClass="control-label" Text="Pilih Jenis Publikasi"></asp:Label>
        <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
        <asp:RequiredFieldValidator runat="server" ID="ReqJenisPub" ControlToValidate="ddlJnsPublikasi" Text="harus dipilih" ForeColor="Red" ValidationGroup="valBeforeAdd" Display="Dynamic"></asp:RequiredFieldValidator>
        <asp:DropDownList
            ID="ddlJnsPublikasi"
            CssClass="form-control dropdown"
            runat="server"
            AutoPostBack="true"
            OnSelectedIndexChanged="ddlJnsPublikasi_SelectedIndexChanged">
        </asp:DropDownList>
        <br />
        <asp:Label runat="server" CssClass="control-label" Text="Tahun Penulisan"></asp:Label>
        <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
        <asp:TextBox ID="txtYear" runat="server" CssClass="form-control" MaxLength="4" TextMode="Number" Width="500" placeholder="Tahun Penulisan" onkeyup="limit(this);" />
        <br />
        <%--<asp:DropDownList
                      id="ddlTahunPenulisan"
                      CssClass="form-control dropdown"             
                      runat="server"
                      AutoPostBack="true"
                    > 
                </asp:DropDownList>--%>

        <%--<asp:DropDownList
                    id="ddlYear"
                    CssClass="form-control dropdown"
                    runat="server"
                    AutoPostBack="true"></asp:DropDownList>

                <br />--%>
        <asp:Label runat="server" CssClass="control-label" Text="Judul Publikasi"></asp:Label>
        <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
        <asp:TextBox ID="txtJudulPublikasi" runat="server" CssClass="form-control" Width="500" placeholder="Judul Publikasi" />
        <br />

        <div>
            <asp:Label runat="server" CssClass="control-label" Text="Pilih Berkas"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="btn btn-default" onchange="cekImage(this, 'publikasi');" />
        </div>
        <div>
            <br />
            <br />
            <asp:TextBox runat="server" ID="txtConfirmDelete" CssClass="hidden" ClientIDMode="Static"></asp:TextBox>
            <asp:LinkButton ID="btnCancel" runat="server" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelAd_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"></span> Kembali </asp:LinkButton>
            <asp:LinkButton ID="btnUpload" runat="server" Text="Simpan" CssClass="btn btn-primary" OnClick="btnUpload_Click"><span aria-hidden="true" class="glyphicon glyphicon-save-file"></span> Simpan </asp:LinkButton>
        </div>
        <hr />
    </asp:Panel>
    <asp:Panel runat="server" ID="panelEdit" Visible="false" DefaultButton="LinkButton3">
        <asp:Label runat="server" CssClass="control-label" Text="Pilih Jenis Publikasi"></asp:Label>
        <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
        <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlEditJenisPublikasi" Text="harus dipilih" ForeColor="Red" ValidationGroup="valBeforeAdd" Display="Dynamic"></asp:RequiredFieldValidator>
        <asp:DropDownList
            ID="ddlEditJenisPublikasi"
            CssClass="form-control dropdown"
            runat="server">
        </asp:DropDownList>
        <br />
        <asp:Label runat="server" CssClass="control-label" Text="Tahun Penulisan"></asp:Label>
        <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
        <asp:TextBox ID="txtEditYear" MaxLength="4" TextMode="Number" runat="server" CssClass="form-control" Width="500" placeholder="Tahun Penulisan" onkeyup="limit(this);" />
        <br />
        <%--<asp:DropDownList
                      id="ddlTahunPenulisan"
                      CssClass="form-control dropdown"             
                      runat="server"
                      AutoPostBack="true"
                    > 
                </asp:DropDownList>--%>

        <%--<asp:DropDownList
                    id="ddlYear"
                    CssClass="form-control dropdown"
                    runat="server"
                    AutoPostBack="true"></asp:DropDownList>--%>

        <%--                <br />--%>
        <asp:Label runat="server" CssClass="control-label" Text="Judul Publikasi"></asp:Label>
        <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
        <asp:TextBox ID="txtEditJudulPublikasi" runat="server" CssClass="form-control" Width="500" placeholder="Judul Publikasi" />
        <br />

        <div>
            <asp:Label runat="server" CssClass="control-label" Text="Pilih Berkas"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:FileUpload ID="FileUpload2" runat="server" CssClass="btn btn-default" onchange="cekImage(this, 'publikasi');" />
        </div>
        <div>
            <br />
            <br />
            <asp:TextBox runat="server" ID="TextBox4" CssClass="hidden" ClientIDMode="Static"></asp:TextBox>
            <asp:LinkButton ID="LinkButton2" runat="server" CssClass="btn btn-default" Text="Batal" OnClick="LinkButton2_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"></span> Kembali </asp:LinkButton>
            <asp:LinkButton ID="LinkButton3" runat="server" Text="Simpan" CssClass="btn btn-primary" OnClick="LinkButton3_Click"><span aria-hidden="true" class="glyphicon glyphicon-save-file"></span> Simpan </asp:LinkButton>
        </div>
        <hr />
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
                    <asp:TextBox runat="server" ID="TextBox1" ClientIDMode="Static" CssClass="hidden"></asp:TextBox>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>
                    <asp:LinkButton runat="server" ID="linkConfirmDelete" CssClass="btn btn-danger" Text="Hapus" OnClick="linkConfirmDelete_Click"></asp:LinkButton>
                </div>
            </div>
        </div>
    </div>
    <div class="modal fade" id="deleteModal1" tabindex="-1" role="dialog" aria-labelledby="deleteModalLabel1">
        <div class="modal-dialog modal-sm" role="document">
            <div class="modal-content">
                <div class="modal-header">
                    <button type="button" class="close" data-dismiss="modal" aria-label="Close"><span aria-hidden="true">&times;</span></button>
                    <h4 class="modal-title" id="deleteModalLabel1">Kirim Ke Reviewer</h4>
                </div>
                <div class="modal-body">
                    Apakah Anda yakin?
                </div>
                <div class="modal-footer">
                    <asp:TextBox runat="server" ID="txtKirim" ClientIDMode="Static" CssClass="hidden"></asp:TextBox>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>
                    <asp:LinkButton runat="server" ID="LinkButton1" CssClass="btn btn-danger" Text="Kirim" OnClick="LinkButton1_Click"></asp:LinkButton>
                </div>
            </div>
        </div>



        <asp:Label ID="lblMessage" runat="server" />
        <script type="text/javascript">
            function deleteyuk(coba) {
                $("#deleteModal").find('.modal-footer input').val($(coba).data("delete"));
            }
            function deleteyuk1(coba) {
                $("#deleteModal1").find('.modal-footer input').val($(coba).data("delete"));
            }
            function limit(element) {
                var max_chars = 4;
                if (element.value.length > max_chars) {
                    element.value = element.value.substr(0, max_chars);
                }
            }
            function cekImage(param, deskripsi) {
                var input, file, valid = true;
                input = param;
                file = input.files[0];
                if (file.size / 1024 > 5120) {
                    alert("Opps! Berkas " + deskripsi + " terlalu besar! Ukuran maksimal berkas yang bisa dikirim adalah 5 MB");
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
        </script>

        <asp:SqlDataSource
            ID="srcJenisPublikasi"
            SelectCommand="SELECT * FROM MSJENISPUBLIKASI where jpb_delete_status=0"
            ConnectionString="<%$ ConnectionStrings:DefaultConnection %>"
            runat="server" />
</asp:Content>


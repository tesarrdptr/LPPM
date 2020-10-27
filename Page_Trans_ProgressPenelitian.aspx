    <%@ Page Title="Progress Penelitian" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Trans_ProgressPenelitian.aspx.cs" Inherits="Page_Trans_ProgressPenelitian" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
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
        </Triggers>
    </asp:UpdatePanel>
    <asp:Panel runat="server" ID="panelData" Visible="false" DefaultButton="btnCari">
        <asp:LinkButton runat="server" ID="linkTambahpProgress" CssClass="btn btn-primary" OnClick="linkTambah_Click"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Tambah Baru</asp:LinkButton>
        <br />
        <br />
        <div class="form-group form-inline">
            <b>Status :&nbsp;</b>
            <asp:DropDownList runat="server" ID="ddStatus" CssClass="form-control dropdown"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
            <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
        </div>
        <div ng-controller="MyCtrl" ng-app="myApp">
            <div id="tableToExport">

                <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="true" AutoGenerateColumns="false" DataKeyNames="prg_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnRowCommand="gridData_RowCommand"  
                    OnSorting="gridData_Sorting1" 
                    OnRowDataBound="gridData_RowDataBound" 
                    OnPageIndexChanging="gridData_PageIndexChanging">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <%--                    <asp:TemplateField HeaderText="No." HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <%# Container.DataItemIndex + 1 %> 
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                        <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="prg_id" SortExpression="prp_nomor" HeaderText="ID Progress" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" Visible="false" />
                        
                        <asp:BoundField DataField="prp_nomor" SortExpression="prp_nomor" HeaderText="Nomor" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="prp_judul" SortExpression="prp_judul" HeaderText="Judul Proposal" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <%--<asp:BoundField DataField="atc_name" HeaderText="File" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                        <%--<asp:BoundField DataField="prg_created_by" HeaderText="Pembuat" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                        <asp:BoundField DataField="monevdoc_jenis" SortExpression="monevdoc_jenis" HeaderText="Jenis Progress" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />

                        <asp:BoundField DataField="creadate" SortExpression="creadate" HeaderText="Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="status" SortExpression="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:LinkButton Visible='<%#Eval("status").ToString()== "Draft" %>' ClientIDMode="Static" runat="server" ID="LinkKirim" CommandName="Kirim" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Kirim Proposal"><span class="glyphicon glyphicon-send" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="download" CommandArgument='<%# Eval("atc_name") %>' ToolTip="Unduh"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></asp:LinkButton>
                                <asp:LinkButton runat="server" ID="linkLihat" CommandName="Lihat_Nilai" CommandArgument='<%# Eval("prg_id")%>' ToolTip="Lihat Nilai"><span class="glyphicon glyphicon-list" aria-hidden="true"></span></asp:LinkButton>
                                <%--<asp:LinkButton Visible='<%#Eval("status").ToString()== "Belum Dikirim" %>' runat="server" ID="linkGrade" data-delete='<%# Eval("prg_id") %>' data-toggle="modal" data-target="#deleteModal1" OnClientClick="deleteyuk1(this)" ToolTip="Diajukan Ke Reviewer"><span class="glyphicon glyphicon-send" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton Visible='<%#Eval("status").ToString()== "Belum Dikirim" %>' ClientIDMode="Static" runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Ubah Progress Penelitian"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>
                                <asp:LinkButton Visible='<%#Eval("status").ToString()== "Belum Dikirim" %>' ClientIDMode="Static" runat="server" ID="linkDelete" ToolTip="Hapus Progress Penelitian" data-delete='<%# Eval("prg_id") %>' data-toggle="modal" data-target="#deleteModal" OnClientClick="deleteyuk(this)"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>--%>
                            </ItemTemplate>
                        </asp:TemplateField>
                    
                    </Columns>
                </asp:GridView>
                <%--<asp:Button runat="server" ID="btnExcel" ClientIDMode="Static" CssClass="btn btn-success" Text="Simpan ke Excel" ng-click="exportToExcel('#tableToExport')" />--%>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelAdd" Visible="false">
        <div>
            <asp:Label runat="server" CssClass="control-label" Text="Pilih Proposal"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:RequiredFieldValidator runat="server" ID="ReqJenisPub" ControlToValidate="ddlAddProgress"  Text="harus dipilih" ForeColor="Red" ValidationGroup="valBeforeAdd" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:DropDownList
                ID="ddlAddProgress"
                CssClass="form-control dropdown" Enabled="false"
                runat="server"
                >
            </asp:DropDownList>
        </div>
        <br />
        <div>
            <asp:Label runat="server" CssClass="control-label" Text="Pilih Jenis Dokumen"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:RequiredFieldValidator runat="server" ID="reqjenisdokumen" ControlToValidate="ddljenisDokumen" Text="harus dipilih" ForeColor="Red" ValidationGroup="valBeforeAdd" Display="Dynamic"></asp:RequiredFieldValidator>
            <asp:DropDownList
                ID="ddlJenisDokumen"
                CssClass="form-control dropdown"
                runat="server"
                >
            </asp:DropDownList>
        </div>
        <br />
        <div>
            <asp:Label runat="server" CssClass="control-label" Text="Pilih Berkas"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:FileUpload ID="FileUpload1" runat="server" CssClass="btn btn-default" onchange="cekImage(this, 'progress penelitian');" />
        </div>
        <br />
        <br />
        <div>
            <asp:LinkButton runat="server" ID="btnCancelAdd" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelAdd_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"> Kembali</span></asp:LinkButton>
            <asp:LinkButton ID="btnUpload" runat="server" Text="Simpan" OnClick="Upload" CssClass="btn btn-primary" ValidationGroup="valBeforeAdd"><span aria-hidden="true" class="glyphicon glyphicon-save-file"></span> Simpan</asp:LinkButton>
            <hr />
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelLihatNilai" Visible="false">
        <asp:GridView runat="server" ID="GridNilai" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
            AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="prg_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
            ShowHeaderWhenEmpty="true">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
            <Columns>
                <asp:TemplateField HeaderText="No." HeaderStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <%# Container.DataItemIndex + 1 %>
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="bbt_title" HeaderText="Penilaian" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                <asp:BoundField DataField="bbt_percent" HeaderText="Bobot %" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="mon_score" HeaderText="Nilai Awal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                <asp:BoundField DataField="bobot_akhir" HeaderText="Nilai Akhir" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
            </Columns>
        </asp:GridView>
        <div class="form-group" style="margin-bottom: -10px;">
            <asp:Label runat="server" CssClass="control-label" Text="Total Nilai"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:Label runat="server" ID="TotalScore" Enabled="false" Style="color: greenyellow;"></asp:Label>
        </div>
        <br />

        <asp:LinkButton runat="server" ID="btn_back" CssClass="btn btn-default" Text="Kembali" OnClick="btnCancelAdd_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"> Kembali</span></asp:LinkButton>

    </asp:Panel>
    <asp:Panel runat="server" ID="panelEdit" Visible="false" DefaultButton="btnSubmitEdit">
        <div class="form-group">
            <asp:Label runat="server" ID="editPrgID" CssClass="control-label" Font-Bold="true" Visible="false"></asp:Label>
        </div>
        <div class="form-group" style="margin-bottom: -10px;">
            <div>
                <asp:Label runat="server" CssClass="control-label" Text="Pilih Proposal"></asp:Label>
                <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="ddlEditProgress" Text="harus dipilih" ForeColor="Red" ValidationGroup="valBeforeAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                <asp:DropDownList
                    ID="ddlEditProgress"
                    CssClass="form-control dropdown"
                    runat="server">
                </asp:DropDownList>
            </div>
        </div>
        <br />
        <br />
        <div class="form-group">
            <asp:Label runat="server" CssClass="control-label" Text="Pilih Berkas"></asp:Label>
            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
            <asp:FileUpload ID="FileUpload2" runat="server" CssClass="btn btn-default" onchange="cekImage(this, 'progress penelitian');"/>
        </div>
        <div class="form-group">
            <asp:Button runat="server" ID="btnCancelEdit" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelEdit_Click" />
            <asp:Button runat="server" ID="btnSubmitEdit" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valEdit" OnClick="btnSubmitEdit_Click" />
        </div>
    </asp:Panel>
      <asp:Panel runat="server" ID="panelProggres" DefaultButton="btnCari">
        <br />
        <br />
        <div class="form-group form-inline">
            <%--<b>Status :&nbsp;</b>
            <asp:DropDownList runat="server" ID="DropDownList1" CssClass="form-control dropdown"></asp:DropDownList>&nbsp;&nbsp;--%>
                    <asp:TextBox runat="server" ID="TextBox1" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
            <asp:LinkButton runat="server" ID="LinkButton3" CssClass="btn btn-default"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
        </div>
        <div ng-controller="MyCtrl" ng-app="myApp">
            <div id="tableToExport">

                <asp:GridView runat="server" ID="gridproposal" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="true" AutoGenerateColumns="false" DataKeyNames="prp_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridproposal_PageIndexChanging" OnRowCommand="gridproposal_RowCommand"
                    OnSorting="gridproposal_Sorting">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="prp_nomor" SortExpression="prp_nomor" HeaderText="Nomor Proposal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="prp_judul" SortExpression="prp_judul" HeaderText="Judul Proposal" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="bidfok_name" SortExpression="bidfok_name" HeaderText="Bidang Fokus Penelitian" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="jns_title" SortExpression="jns_title" HeaderText="Jenis" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="ketua" SortExpression="ketua" HeaderText="Ketua Pengusul" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="prp_keyword" SortExpression="prp_keyword" HeaderText="Keyword" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="prp_total_rab" SortExpression="prp_total_rab" HeaderText="Total Dana" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="prp_submit" SortExpression="prp_submit" HeaderText="Tanggal Kirim" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="status" SortExpression="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <%--<asp:BoundField DataField="own_leader" SortExpression="status" HeaderText="Leader" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />--%>

                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                            <ItemTemplate>
                                <asp:LinkButton ClientIDMode="Static" href='<%# Page.ResolveUrl("Page_Trans_DataProgress?id="+Eval("prp_id").ToString()) %>' runat="server" ID="linkLihat" ToolTip="Ajukan Progress"><span class="glyphicon glyphicon-list" aria-hidden="true"></span></asp:LinkButton>
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
                <%--<asp:Button runat="server" ID="btnExcel" ClientIDMode="Static" CssClass="btn btn-success" Text="Simpan ke Excel" ng-click="exportToExcel('#tableToExport')" />--%>
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
    </div>


    <script type="text/javascript">
        function deleteyuk(coba) {
            $("#deleteModal").find('.modal-footer input').val($(coba).data("delete"));
        }
        function deleteyuk1(coba) {
            $("#deleteModal1").find('.modal-footer input').val($(coba).data("delete"));
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
        ID="srcNamaProposal"
        SelectCommand="SELECT prp_id, prp_judul FROM TRPROPOSAL WHERE jns_id = 1"
        ConnectionString="<%$ ConnectionStrings: LP2MConnection %>"
        runat="server" />
</asp:Content>

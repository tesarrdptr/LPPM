<%@ Page Title="History Buku" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Master_Buku.aspx.cs" Inherits="Page_Master_Buku" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="Server">
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
    </asp:UpdatePanel>

    <asp:Panel runat="server" ID="panelData" DefaultButton="btnCari">
        <asp:LinkButton runat="server" ID="linkTambah" CssClass="btn btn-primary" OnClick="linkTambah_Click"><span class="glyphicon glyphicon-plus" aria-hidden="true"></span> Tambah Baru</asp:LinkButton>
        <br />
        <br />

        <div class="form-group form-inline">
            <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
           <%-- <asp:Label runat="server" ID="lblTglMulaiCari" Text="Tanggal Mulai : "></asp:Label>
            <asp:TextBox runat="server" ID="tglMulaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>
            <asp:Label runat="server" ID="lblTglSelesaiCari" Text="Tanggal Selesai : "></asp:Label>
            <asp:TextBox runat="server" ID="tglSelesaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>--%>
            <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default" OnClick="btnCari_Click"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
        </div>

        <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
            AllowSorting="true" OnSorting="gridData_Sorting" AutoGenerateColumns="false" DataKeyNames="bk_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowCommand="gridData_RowCommand">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
            <Columns>
                <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <asp:BoundField DataField="bk_judul" HeaderText="Judul Buku" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" Visible="true" SortExpression="bk_judul" />
                <asp:BoundField DataField="bk_penulis" HeaderText="Penulis" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="bk_penulis"/>
                <asp:BoundField DataField="bk_penerbit" HeaderText="Penerbit" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="bk_penerbit"/>
                <asp:BoundField DataField="bk_editor" HeaderText="Editor" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="bk_editor"/>
                <asp:BoundField DataField="bk_tahun" HeaderText="Tahun" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="bk_tahun"/>
                <asp:BoundField DataField="bk_halaman" HeaderText="Halaman" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="bk_halaman"/>
                <asp:BoundField DataField="atc_id" Visible="false" HeaderText="Attachment" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="atc_id"/>
                <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <%--<asp:LinkButton Visible='<%#Eval("prp_status").ToString()== "0" && Eval("own_leader").ToString() == "1" %>' ClientIDMode="Static" runat="server" ID="LinkKirim" CommandName="Kirim" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Kirim Proposal"><span class="glyphicon glyphicon-send" aria-hidden="true"></span></asp:LinkButton>--%>
                        <%--<asp:LinkButton runat="server" ID="linkMember" href='<%# Page.ResolveUrl("Page_MemberProposal?id="+Eval("prp_id").ToString()) %>' ToolTip="Anggota Proposal"><span class="glyphicon glyphicon-user" aria-hidden="true"></span></asp:LinkButton>--%>
                        <asp:LinkButton ClientIDMode="Static" runat="server" ID="linkDownload" CommandArgument='<%# Eval("atc_id") %>' Text="Download" CommandName="download" ToolTip="Unduh"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="LinkButton1" CommandName="Detil" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Lihat Detil Bobot"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span></asp:LinkButton>
                        <%--<asp:LinkButton Visible='<%#Eval("prp_status").ToString() == "0" && Eval("own_leader").ToString() == "1" %>' Enabled='<%#Eval("own_leader").ToString() == "1" %>' ClientIDMode="Static" runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Ubah Proposal"><span class="glyphicon glyphicon-edit" aria-hidden="true"></span></asp:LinkButton>--%>
                        <%--<asp:LinkButton Visible='<%#Eval("prp_status").ToString() == "0" && Eval("own_leader").ToString() == "1" %>' ClientIDMode="Static" runat="server" ID="linkDelete" ToolTip="Hapus Proposal" data-delete='<%# Eval("prp_id") %>' data-toggle="modal" data-target="#deleteModal" OnClientClick="deleteyuk(this)"><span class="glyphicon glyphicon-remove" aria-hidden="true"></span></asp:LinkButton>--%>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelAdd" Visible="false" DefaultButton="btnSubmitAdd">
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Judul Buku"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddJudul" ControlToValidate="addJudul" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" TextMode="MultiLine" ID="AddJudul" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Penerbit"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="AddPenerbit" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="AddPenerbit" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Penulis"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="addPenulis" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList runat="server" ID="addPenulis" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:DropDownList>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Editor"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="addEditor" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="addEditor" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Halaman"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="addHalaman" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="addHalaman" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Tahun Terbit"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:DropDownList ID="ddlTahun" CssClass="form-control dropdown" runat="server" AutoPostBack="true"></asp:DropDownList>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="URL"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="addUrl" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="addUrl" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Upload File ( Max :10 MB|| File : PDF )"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddUpload" ControlToValidate="addUpload" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:FileUpload runat="server" ID="addUpload" CssClass="form-control" ValidationGroup="valAdd" onchange="cekImage(this, 'proposal');"></asp:FileUpload>
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnCancelAdd" CssClass="btn btn-default" Text="Batal" OnClick="btnCancelAdd_Click" />
                    <asp:Button runat="server" ID="btnSubmitAdd" CssClass="btn btn-primary" Text="Simpan" ValidationGroup="valAdd" OnClick="btnSubmitAdd_Click" />
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelDetil" Visible="false" DefaultButton="btnKembali">
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Judul Buku"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="detilJudul" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" TextMode="MultiLine" ID="detilJudul" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Penerbit"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="detilPenerbit" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="detilPenerbit" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Penulis"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="detilPenulis" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="detilPenulis" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Editor"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" ControlToValidate="detilEditor" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="detilEditor" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Halaman"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator10" ControlToValidate="detilHalaman" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="detilHalaman" runat="server" CssClass="form-control" />
                </div>
                <%--<div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Upload File ( Max :10 MB|| File : PDF )"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator12" ControlToValidate="addUpload" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:FileUpload runat="server" ID="FileUpload1" CssClass="form-control" ValidationGroup="valAdd" onchange="cekImage(this, 'proposal');"></asp:FileUpload>
                </div>--%>
                <div class="form-group">
                    <asp:Button runat="server" ID="btnKembali" CssClass="btn btn-default" Text="Kembali" OnClick="btnKembali_Click" />
                </div>
            </div>
            <div class="col-lg-6">
                
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Tahun Terbit"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:TextBox ID="ddlEditTahun" CssClass="form-control dropdown" runat="server" AutoPostBack="true"></asp:TextBox>
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="URL"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator11" ControlToValidate="editUrl" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="editUrl" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Dibuat Oleh"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator13" ControlToValidate="creaby" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" TextMode="MultiLine" ID="creaby" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Dibuat Tanggal"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator14" ControlToValidate="creadate" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="creadate" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" Visible="false" CssClass="control-label" Text="Diubah Oleh"></asp:Label>
                    <asp:Label runat="server" Visible="false"  Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator Visible="false"  runat="server" ID="RequiredFieldValidator15" ControlToValidate="updby" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" Visible="false"  ID="updby" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" Visible="false"  CssClass="control-label" Text="Diubah Tanggal"></asp:Label>
                    <asp:Label runat="server" Visible="false"  Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator Visible="false"  runat="server" ID="RequiredFieldValidator16" ControlToValidate="upddate" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" Visible="false"  ID="upddate" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
            </div>
        </div>
    </asp:Panel>

    <script type="text/javascript">
        $(document).ready(function () {
            var date = new Date();
            var d = date.getDate();
            var m = date.getMonth();
            var y = date.getFullYear();

            $('.tomboldelete').on('click', function (event) {
                var data = $(this).data('delete');
                var modal = $("#deleteModal");
                modal.find('.modal-footer #txtConfirmDelete').val(data);
            });

            $('#tglMulai').datetimepicker({
                format: 'DD MMMM YYYY'
            });
            $('#tglSelesai').datetimepicker({
                format: 'DD MMMM YYYY',
                useCurrent: false //Important! See issue #1075
            });
            $("#tglMulai").on("dp.change", function (e) {
                $('#tglSelesai').data("DateTimePicker").minDate(e.date);
            });
            $("#tglSelesai").on("dp.change", function (e) {
                $('#tglMulai').data("DateTimePicker").maxDate(e.date);
            });


        });

        $("editTanggal")

    </script>


</asp:Content>


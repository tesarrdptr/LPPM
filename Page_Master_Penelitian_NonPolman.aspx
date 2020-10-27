<%@ Page Title="History Penelitian Non-Polman" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Master_Penelitian_NonPolman.aspx.cs" Inherits="Page_Master_Penelitian" %>

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
            <%--<asp:Label runat="server" ID="lblTglMulaiCari" Text="Tanggal Mulai : "></asp:Label>
            <asp:TextBox runat="server" ID="tglMulaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>
            <asp:Label runat="server" ID="lblTglSelesaiCari" Text="Tanggal Selesai : "></asp:Label>
            <asp:TextBox runat="server" ID="tglSelesaiCari" TextMode="Date" CssClass="form-control"></asp:TextBox>--%>
            <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default" OnClick="btnCari_Click"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
        </div>

        <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
            AllowSorting="true" OnSorting="gridData_Sorting" AutoGenerateColumns="false" DataKeyNames="pp_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
            ShowHeaderWhenEmpty="true" OnPageIndexChanging="gridData_PageIndexChanging" OnRowCommand="gridData_RowCommand">
            <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
            <Columns>
                <asp:TemplateField HeaderText="No." ItemStyle-HorizontalAlign="Center">
                    <ItemTemplate>
                        <asp:Label ID="lblRowNumber" Text='<%# Container.DataItemIndex + 1 %>' runat="server" />
                    </ItemTemplate>
                </asp:TemplateField>
                <%--<asp:BoundField DataField="pp_id"  HeaderText="ID Penelitian" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" Visible="true" />--%>
                <asp:BoundField DataField="pp_judul" HeaderText="Judul Penelitian" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="pp_judul" />
                <asp:BoundField DataField="pp_tahun_anggaran" HeaderText="Tahun Anggaran" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="pp_tahun_anggaran" />
                <asp:BoundField DataField="pp_ketua" HeaderText="Ketua" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="pp_ketua" />
                <asp:BoundField DataField="pp_anggota" HeaderText="Anggota" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="pp_anggota" />
                <asp:BoundField DataField="pp_totaldana" HeaderText="Total Dana Rp. " NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression="pp_totaldana" />
                <%--<asp:BoundField DataField="pp_tgl_mulai" HeaderText="Tanggal Mulai" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression />
                <asp:BoundField DataField="pp_tgl_selesai" HeaderText="Tanggal Selesai" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" SortExpression />--%>

                <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center" ItemStyle-Width="100px">
                    <ItemTemplate>
                        <asp:LinkButton ClientIDMode="Static" runat="server" ID="linkDownload" CommandArgument='<%# Eval("atc_id") %>' Text="Download" CommandName="download" ToolTip="Unduh"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></span></asp:LinkButton>
                        <asp:LinkButton runat="server" ID="linkEdit" CommandName="Ubah" CommandArgument='<%# DataBinder.Eval(Container, "RowIndex") %>' ToolTip="Lihat Detil Penelitian"><span class="glyphicon glyphicon-th-list" aria-hidden="true"></span></asp:LinkButton>
                    </ItemTemplate>
                </asp:TemplateField>
            </Columns>
        </asp:GridView>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelAdd" Visible="false" DefaultButton="btnSubmitAdd">
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Judul"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddJudul" ControlToValidate="addJudul" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="AddJudul" TextMode="MultiLine" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Ketua"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator4" ControlToValidate="AddKetua" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="AddKetua" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Anggota"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator5" ControlToValidate="addAnggota" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="addAnggota" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Sumber Dana"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqAddJenisID" ControlToValidate="ddlAddSumDana" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:DropDownList ID="ddlAddSumDana" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Total Dana"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="reqDana" ControlToValidate="addDana" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="addDana" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                            <ajaxToolkit:MaskedEditExtender runat="server"
                                TargetControlID="addDana"
                                Mask="999,999,999"
                                MessageValidatorTip="true"
                                OnFocusCssClass="MaskedEditFocus"
                                OnInvalidCssClass="MaskedEditError"
                                MaskType="Number"
                                InputDirection="RightToLeft"
                                AcceptNegative="Left"
                                DisplayMoney="Left"
                                ErrorTooltipEnabled="True" />
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis Penelitian"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator3" ControlToValidate="addJenis" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="addJenis" runat="server" CssClass="form-control" />
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
            <div class="col-lg-6">
                <asp:UpdatePanel ID="UpdatePanel2" runat="server">
                    <ContentTemplate>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Tanggal Mulai"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:TextBox runat="server" AutoCompleteType="Disabled" ID="tglMulai" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Tanggal Selesai"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:TextBox runat="server" ID="tglSelesai" AutoCompleteType="Disabled" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Tahun Anggaran"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:DropDownList ID="tglAnggaran" CssClass="form-control dropdown" runat="server" AutoPostBack="true"></asp:DropDownList>
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

            </div>
        </div>
    </asp:Panel>

    <asp:Panel runat="server" ID="panelDetil" Visible="false" DefaultButton="btnSubmitAdd">
        <div class="row">
            <div class="col-lg-4">
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Judul"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator1" ControlToValidate="dtJudul" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" TextMode="MultiLine" ID="dtJudul" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Ketua"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator2" ControlToValidate="dtKetua" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="dtKetua" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Anggota"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator6" ControlToValidate="dtAnggota" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="dtAnggota" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Sumber Dana"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator7" ControlToValidate="dtSumDana" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="dtSumDana" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group" style="margin-bottom: -10px;">
                    <asp:Label runat="server" CssClass="control-label" Text="Total Dana"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator8" ControlToValidate="dtDana" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox runat="server" ID="dtDana" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                </div>
                <div class="form-group">
                    <asp:Label runat="server" CssClass="control-label" Text="Jenis Penelitian"></asp:Label>
                    <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                    <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator9" ControlToValidate="dtJenis" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                    <asp:TextBox ID="dtJenis" runat="server" CssClass="form-control" />
                </div>
                <div class="form-group">
                    <asp:Button runat="server" ID="Button1" CssClass="btn btn-default" Text="Kembali" OnClick="btnCancelAdd_Click" />
                </div>
            </div>
            <div class="col-lg-6">
                <asp:UpdatePanel ID="UpdatePanel3" runat="server">
                    <ContentTemplate>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Tanggal Mulai"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:TextBox runat="server" AutoCompleteType="Disabled" ID="dtTglMulai" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Tanggal Selesai"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:TextBox runat="server" ID="dtTglSelesai" AutoCompleteType="Disabled" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                        </div>
                        <div class="form-group">
                            <asp:Label runat="server" CssClass="control-label" Text="Tahun Anggaran"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:TextBox ID="dtThnAnggaran" CssClass="form-control dropdown" runat="server" AutoPostBack="true"></asp:TextBox>
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Dibuat Oleh"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator13" ControlToValidate="creaby" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="creaby" CssClass="form-control" MaxLength="100" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                        </div>
                        <div class="form-group" style="margin-bottom: -10px;">
                            <asp:Label runat="server" CssClass="control-label" Text="Dibuat Tanggal"></asp:Label>
                            <asp:Label runat="server" Text=" *" Style="color: red;"></asp:Label>
                            <asp:RequiredFieldValidator runat="server" ID="RequiredFieldValidator14" ControlToValidate="creadate" Text="harus diisi" ForeColor="Red" ValidationGroup="valAdd" Display="Dynamic"></asp:RequiredFieldValidator>
                            <asp:TextBox runat="server" ID="creadate" CssClass="form-control" MaxLength="50" ValidationGroup="valAdd"></asp:TextBox>&nbsp;
                        </div>
                    </ContentTemplate>
                </asp:UpdatePanel>

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


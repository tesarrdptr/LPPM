<%@ Page Title="Daftar Publikasi" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_Trans_DaftarPublikasi.aspx.cs" Inherits="Page_Trans_DaftarPublikasi" %>
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

           <asp:Panel runat="server" ID="panelData" DefaultButton="btnCari">
                <asp:LinkButton runat="server" ID="linkCetakLaporan" CssClass="btn btn-primary" OnClick="linkCetakLaporan_Click"><span aria-hidden="true" class="glyphicon glyphicon-print"> Cetak Laporan</span></asp:LinkButton>
                <br />  <br />

                <div class="form-group form-inline">
                    <b>Status :&nbsp;</b>
                    <asp:DropDownList runat="server" ID="ddStatus" CssClass="form-control dropdown"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
                    <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
                </div>

                 <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="atc_name" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnRowCommand="gridData_RowCommand" OnPageIndexChanging="gridData_PageIndexChanging" OnRowDataBound="gridData_RowDataBound">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:BoundField DataField="rownum" HeaderText="No. " NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="pub_title" HeaderText="Judul Publikasi" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <asp:BoundField DataField="jpb_title" HeaderText="Jenis Publikasi" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <%--<asp:BoundField DataField="atc_name" HeaderText="File" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                        <%--<asp:BoundField DataField="pub_creator" HeaderText="Penulis" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                        <asp:BoundField DataField="creadate" HeaderText="Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                 <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="download" CommandArgument='<%# Eval("atc_name") %>' ToolTip="Download"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></asp:LinkButton>
                                
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>     
            </asp:Panel>
            
            <asp:Panel runat="server" ID="panelLaporan" Visible="false" DefaultButton="linkCetakLaporan">
                <div class="form-inline" >
                    <div class="form-group">
                            <label for="tglMulai"><span style="color: red">* </span>Tanggal Awal :</label>
                            <div style="padding-left:0px;width:450px">
                                <div class="row">
                                    <div class='col-sm-6'>
                                        <asp:TextBox runat="server" ID="tglMulai" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="tglMulai" ErrorMessage="Data wajib diisi" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                            </div>
                        </div>
                        <div class="form-group">
                            <label for="tglSelesai"><span style="color: red">* </span>Tanggal  Akhir :</label>
                            <div style="padding-left:0px;width:450px">
                                <div class="row">
                                    <div class='col-sm-6'>
                                        <asp:TextBox runat="server" ID="tglSelesai" CssClass="form-control" ClientIDMode="Static"></asp:TextBox>
                                        <%--<asp:RequiredFieldValidator runat="server" ControlToValidate="tglSelesai" ErrorMessage="Data wajib diisi" ForeColor="Red"></asp:RequiredFieldValidator>--%>
                                    </div>
                                </div>
                            </div>
                        </div> 
                </div>
                <br />
                <asp:LinkButton runat="server" ID="btnCancelCetak" CssClass="btn btn-default" OnClick="btnCancel_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"> Kembali</span></asp:LinkButton>
                <asp:LinkButton runat="server" ID="btnCetak" CssClass="btn btn-primary" Text="Cetak Laporan" ValidationGroup="valEdit" OnClick="btnCetak_Click"><span aria-hidden="true" class="glyphicon glyphicon-print"> Cetak Laporan</span></asp:LinkButton>  
            </asp:Panel>

      <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <div class="alert alert-danger" runat="server" id="divAlert" visible="false"></div>
            <div class="alert alert-success" runat="server" id="divSuccess" visible="false"></div>

        
        </ContentTemplate>
    </asp:UpdatePanel>

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
    </script>
</asp:Content>

<%@ Page Title="Daftar Progress Penelitian Saya" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeFile="Page_DaftarPeneltianDosen.aspx.cs" Inherits="Page_DaftarPeneltianDosen" %>

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

           <asp:Panel runat="server" ID="panelData" DefaultButton="btnCari">
                  <asp:LinkButton runat="server" ID="linkCetakLaporan" Visible="false" CssClass="btn btn-primary" OnClick="linkCetakLaporan_Click"><span aria-hidden="true" class="glyphicon glyphicon-print"> Cetak Laporan</span></asp:LinkButton>
                <br />  <br />

                <div class="form-group form-inline">
                      <b>Status :&nbsp;</b>
                    <asp:DropDownList runat="server" ID="ddStatus" CssClass="form-control dropdown"></asp:DropDownList>&nbsp;&nbsp;
                    <asp:TextBox runat="server" ID="txtCari" CssClass="form-control" Width="500" placeholder="Pencarian"></asp:TextBox>
                    <asp:LinkButton runat="server" ID="btnCari" CssClass="btn btn-default"><span class="glyphicon glyphicon-search" aria-hidden="true"></span> Cari</asp:LinkButton>
                </div>

                 <asp:GridView runat="server" ID="gridData" CssClass="table table-hover table-bordered table-condensed table-striped grid" AllowPaging="true"
                    AllowSorting="false" AutoGenerateColumns="false" DataKeyNames="prg_id" EmptyDataText="Tidak ada data" PageSize="10" PagerStyle-CssClass="pagination-ys"
                    ShowHeaderWhenEmpty="true" OnRowCommand="gridData_RowCommand" OnPageIndexChanged="gridData_PageIndexChanged" OnPageIndexChanging="gridData_PageIndexChanging" OnRowDataBound="gridData_RowDataBound">
                    <PagerSettings Mode="NumericFirstLast" FirstPageText="<<" LastPageText=">>" />
                    <Columns>
                        <asp:BoundField DataField="rownum" HeaderText="No. " NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                         <asp:BoundField DataField="prp_nomor" HeaderText="Nomor" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="prg_id" HeaderText="ID Progress" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" visible="false"/>
                        <asp:BoundField DataField="prp_judul" HeaderText="Judul Proposal" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />
                        <%--<asp:BoundField DataField="atc_name" HeaderText="File" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                        <%--<asp:BoundField DataField="prg_created_by" HeaderText="Pembuat" NullDisplayText="-" ItemStyle-HorizontalAlign="Left" />--%>
                        <asp:BoundField DataField="creadate" HeaderText="Tanggal" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:BoundField DataField="status" HeaderText="Status" NullDisplayText="-" ItemStyle-HorizontalAlign="Center" />
                        <asp:TemplateField HeaderText="Aksi" ItemStyle-HorizontalAlign="Center">
                            <ItemTemplate>
                                 <asp:LinkButton ID="lnkDownload" runat="server" Text="Download" CommandName="download" CommandArgument='<%# Eval("atc_name") %>' ToolTip="Unduh"><span class="glyphicon glyphicon-download-alt" aria-hidden="true"></asp:LinkButton>
                                
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
                            <label for="tglSelesai"><span style="color: red">* </span>Tanggal Akhir :</label>
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
                <asp:LinkButton runat="server" ID="btnCancel" CssClass="btn btn-default" Text="Kembali" OnClick="btnCancel_Click"><span aria-hidden="true" class="glyphicon glyphicon-arrow-left"> Kembali</span></asp:LinkButton>
                <asp:LinkButton runat="server" ID="btnCetak" CssClass="btn btn-primary" Text="Cetak Laporan" ValidationGroup="valEdit" OnClick="btnCetak_Click"><span aria-hidden="true" class="glyphicon glyphicon-print"> Cetak Laporan</span></asp:LinkButton>    
            </asp:Panel>

    <asp:UpdatePanel ID="updatePanel1" runat="server">
        <ContentTemplate>
            <div class="alert alert-danger" runat="server" id="divAlert" visible="false"></div>
            <div class="alert alert-success" runat="server" id="divSuccess" visible="false"></div>

        
        </ContentTemplate>

    </asp:UpdatePanel>


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
                    <asp:TextBox runat="server" ID="txtConfirmDelete" CssClass="hidden" ClientIDMode="Static"></asp:TextBox>
                    <button type="button" class="btn btn-default" data-dismiss="modal">Batal</button>
                    <%--<asp:LinkButton runat="server" ID="linkConfirmDelete" CssClass="btn btn-danger" Text="Hapus" OnClick="linkConfirmDelete_Click"></asp:LinkButton>--%>
                </div>
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $('#deleteModal').on('show.bs.modal', function (event) {
            var button = $(event.relatedTarget)
            var data = button.data('delete')
            var modal = $(this)
            modal.find('.modal-footer input').val(data)
        })
    </script>  

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

            $('#tglMulaiEdit').datetimepicker({
                format: 'DD MMMM YYYY'
            });
            $('#tglSelesaiEdit').datetimepicker({
                format: 'DD MMMM YYYY',
                useCurrent: false //Important! See issue #1075
            });
            $("#tglMulaiEdit").on("dp.change", function (e) {
                $('#tglSelesaiEdit').data("DateTimePicker").minDate(e.date);
            });
            $("#tglSelesaiEdit").on("dp.change", function (e) {
                $('#tglMulaiEdit').data("DateTimePicker").maxDate(e.date);
            });
        });
    </script>

</asp:Content>


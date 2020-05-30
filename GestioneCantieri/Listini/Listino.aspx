<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="Listino.aspx.cs" Inherits="GestioneCantieri.Listino" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Listino Mef</title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#body_spinnerImg").hide();
        });

        function ShowHideLoader() {
            $("#body_spinnerImg").show();
        }
    </script>
</asp:Content>
<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">

    <%-- Titolo Pagina --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col">
            <h1>Listino Mef</h1>
        </div>
    </div>

    <%-- Importazione/Eliminazione listino --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:Button ID="btn_ImportaListino" class="btn btn-info btn-lg" OnClick="btn_ImportaListino_Click" OnClientClick="javascript:ShowHideLoader();return confirm('Vuoi importare il nuovo listino?');" Text="Importa Listino da DBF" runat="server" />
            <asp:Button ID="btnEliminaListino" runat="server" OnClick="btnEliminaListino_Click" Text="ELIMINA LISTINO" CssClass="btn btn-danger btn-lg" OnClientClick="return confirm('Vuoi veramente eliminare TUTTO il listino?');" />
            <br />
            <asp:Label ID="lblImportMsg" runat="server"></asp:Label>
        </div>
    </div>

    <%-- Filtri --%>
    <asp:Panel ID="pnlListinoSearch" CssClass="row mt-3 d-flex justify-content-center align-items-center" DefaultButton="btnSearch" runat="server">
        <div class="col text-center">
            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col-3">
                    <asp:Label ID="lblCercaCodArt" runat="server" Text="Cerca per codice articolo"></asp:Label>
                </div>
                <div class="col-3">
                    <asp:Label ID="lblCercaDescriCodArt" runat="server" Text="Cerca per Descrizione Cod. Art."></asp:Label>
                </div>
            </div>

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col-3">
                    <asp:TextBox ID="txtCodArt1" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtCodArt2" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtCodArt3" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-3">
                    <asp:TextBox ID="txtDescriCodArt1" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtDescriCodArt2" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:TextBox ID="txtDescriCodArt3" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>

            <!-- Cerca e Svuota -->
            <div class="row mt-1 d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary btn-lg" Text="Cerca" />
                    <asp:Button ID="btnSvuotaTxt" runat="server" OnClick="btnSvuotaTxt_Click" Text="Svuota" CssClass="btn btn-secondary btn-lg" />
                </div>
            </div>
        </div>
    </asp:Panel>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center table-overflow listino-mef-table">
            <asp:GridView ID="grdListino" runat="server" ItemType="GestioneCantieri.Data.Mamg0" AutoGenerateColumns="False" 
                HeaderStyle-CssClass="border-bottom-bold" CssClass="table table-dark table-striped scrollable-table">
                <Columns>
                    <asp:BoundField DataField="CodArt" HeaderText="Codice Articolo" />
                    <asp:BoundField DataField="Desc" HeaderText="Descrizione" />
                    <asp:BoundField DataField="UnitMis" HeaderText="Unità di Misura" />
                    <asp:BoundField DataField="Pezzo" HeaderText="Pezzo" />
                    <asp:BoundField DataField="PrezzoListino" HeaderText="Prezzo Listino" />
                    <asp:BoundField DataField="Sconto1" HeaderText="Sconto 1" />
                    <asp:BoundField DataField="Sconto2" HeaderText="Sconto2" />
                    <asp:BoundField DataField="Sconto3" HeaderText="Sconto3" />
                    <asp:BoundField DataField="PrezzoNetto" HeaderText="Prezzo Netto" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

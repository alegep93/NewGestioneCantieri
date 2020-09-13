<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="StampaOrdFrutLocale.aspx.cs" Inherits="GestioneCantieri.StampaOrdFrutLocale" EnableEventValidation="false" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Stampa Ord Frut Locale</title>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".groupHeader").css("font-weight", "bold")
            $("#body_grdGruppiInLocale td:not(.groupHeader)").css("padding-left", 40)
        });
    </script>
    <style>
        .table-container > div {
            max-height: 500px;
            overflow: hidden;
            overflow-y: auto;
        }
    </style>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Stampa Ord Frut Loc</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6">
            <asp:Label ID="lblScegliCantiere" runat="server" Text="Scegli Cantiere"></asp:Label>
            <asp:DropDownList ID="ddlScegliCantiere" CssClass="form-control" OnTextChanged="ddlScegliCantiere_TextChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 table-container">
            <asp:GridView ID="grdGruppiInLocale" AllowSorting="true" OnSorting="grdGruppiInLocale_Sorting" AutoGenerateColumns="false" 
                ItemType="GestioneCantieri.Data.StampaCantiere" runat="server" CssClass="table table-dark table-striped scrollable-table">
                <Columns>
                    <asp:BoundField HeaderText="Nome Locale" DataField="NomeLocale" />
                    <asp:BoundField HeaderText="Nome Locale / Gruppi Contenuti" DataField="NomeGruppo" />
                    <asp:BoundField HeaderText="Quantità" DataField="Qta" />
                    <asp:BoundField HeaderText="Gruppo Ordine" DataField="DescrizioneGruppoOrdine" />
                </Columns>
            </asp:GridView>
        </div>

        <div class="col-6 table-container">
            <asp:GridView ID="grdFruttiInLocale" AutoGenerateColumns="false" ItemType="GestioneCantieri.Data.StampaCantiere" 
                runat="server" CssClass="table table-dark table-striped scrollable-table">
                <Columns>
                    <asp:BoundField HeaderText="Descrizione Frutto" DataField="Descr001" />
                    <asp:BoundField HeaderText="Quantità (tot.)" DataField="Qta" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

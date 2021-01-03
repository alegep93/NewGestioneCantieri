<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="StampaOrdFrutCantExcel.aspx.cs" Inherits="GestioneCantieri.StampaExcell" EnableEventValidation="false" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Stampa Ord Frut Cant Excel</title>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script src="Scripts/DynamicStampaOrdFrutStyle.js"></script>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Stampa Ord Frut Cant Excel</h1>
        </div>
    </div>

    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <asp:Label ID="lblScegliCantiere" runat="server" Text="Scegli Cantiere"></asp:Label>
            <asp:DropDownList ID="ddlScegliCantiere" CssClass="form-control" OnTextChanged="ddlScegliCantiere_TextChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
            <asp:Button ID="btnPrint" CssClass="btn btn-primary btn-lg pull-right" OnClick="btnPrint_Click" runat="server" Text="Stampa" />
        </div>
    </div>

    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-6 table-container">
            <asp:GridView ID="grdFruttiInLocale" AutoGenerateColumns="false" ItemType="GestioneCantieri.Data.StampaCantiere" runat="server" CssClass="table table-dark table-striped scrollable-table">
                <Columns>
                    <asp:BoundField HeaderText="Descrizione Frutto" DataField="Descr001" />
                    <asp:BoundField HeaderText="Quantità (tot.)" DataField="Qta" />
                </Columns>
            </asp:GridView>

            <asp:GridView ID="grdGruppi" AutoGenerateColumns="false" ItemType="GestioneCantieri.Data.StampaCantiere" runat="server" CssClass="table table-dark table-striped scrollable-table" Visible="false">
                <Columns>
                    <asp:BoundField HeaderText="Descrizione Frutto" DataField="DescrFrutto" />
                    <asp:BoundField HeaderText="Quantità" DataField="Qta" />
                </Columns>
            </asp:GridView>

            <asp:GridView ID="grdFruttiNonInGruppo" AutoGenerateColumns="false" ItemType="GestioneCantieri.Data.StampaCantiere" runat="server" CssClass="table table-dark table-striped scrollable-table" Visible="false">
                <Columns>
                    <asp:BoundField HeaderText="Descrizione Frutto" DataField="Descr001" />
                    <asp:BoundField HeaderText="Quantità (tot.)" DataField="Qta" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

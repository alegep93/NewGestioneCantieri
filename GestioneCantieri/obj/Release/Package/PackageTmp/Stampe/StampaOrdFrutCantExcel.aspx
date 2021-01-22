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
        <div class="col-6 text-center">
            <asp:Panel ID="pnlLocaliWrapper" CssClass="row d-flex justify-content-center align-items-center" runat="server">
                <asp:Repeater ID="rptLocali" ItemType="GestioneCantieri.Data.MatOrdFrut" runat="server">
                    <ItemTemplate>
                        <div class="col-3 text-center">
                            <asp:CheckBox ID="chkLocale" Text="<%# Item.NomeLocale %>" Checked="true" AutoPostBack="true" OnCheckedChanged="chkLocale_CheckedChanged" runat="server" />
                            <asp:HiddenField ID="hfIdLocale" Value='<%# Item.IdLocale %>' runat="server" />
                        </div>
                    </ItemTemplate>
                </asp:Repeater>
            </asp:Panel>
        </div>
    </div>

    <%--<div class="row d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:Button ID="btnFiltraStampa" CssClass="btn btn-lg btn-dark" Text="Filtra per locali" OnClick="btnFiltraStampa_Click" runat="server" />
        </div>
    </div>--%>

    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-6 table-container">
            <asp:GridView ID="grdFruttiInLocale" AutoGenerateColumns="false" ItemType="GestioneCantieri.Data.StampaCantiere" runat="server" CssClass="table table-dark table-striped scrollable-table">
                <Columns>
                    <asp:BoundField HeaderText="Descrizione Frutto" DataField="Descr001" />
                    <asp:BoundField HeaderText="Articolo Serie" DataField="ArticoloSerie" />
                    <asp:BoundField HeaderText="Descrizione Serie" DataField="DescrizioneSerie" />
                    <asp:BoundField HeaderText="Prezzo Netto" DataField="PrezzoNetto" />
                    <asp:BoundField HeaderText="Quantità (tot.)" DataField="Qta" />
                    <asp:BoundField HeaderText="Valore (tot.)" DataField="Valore" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

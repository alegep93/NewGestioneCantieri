<%@ Page Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="StampaValoriCantieriConOpzioni.aspx.cs" Inherits="GestioneCantieri.StampaValoriCantieriConOpzioni" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Stampa valori cantieri con opzioni</title>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <h1>Stampa valori cantieri con opzioni</h1>
        </div>
    </div>

    <!-- Filtri per la stampa -->
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-6">
            <div class="row d-flex justify-content-center align-items-center">
                <!-- Filtri per scelta cliente -->
                <div class="col">
                    <asp:Label ID="lblFiltraCliente" runat="server" Text="Cliente"></asp:Label>
                    <asp:TextBox ID="txtFiltraCliente" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Button ID="btnFiltraCantieri" CssClass="btn btn-lg btn-dark" OnClick="btnFiltraCantieri_Click" runat="server" Text="Filtra Cantieri" />
                </div>
            </div>
        </div>
    </div>

    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <asp:Label ID="lblScegliCliente" runat="server" Text="Scegli Cliente"></asp:Label>
            <asp:DropDownList ID="ddlScegliCliente" AutoPostBack="true" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
    </div>

    <!-- Altri filtri -->
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col">
            <asp:Label ID="lblAnno" runat="server" Text="Anno"></asp:Label>
            <asp:TextBox ID="txtAnno" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblCodCant" runat="server" Text="Codice Cantiere"></asp:Label>
            <asp:TextBox ID="txtCodCant" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col-1 text-center">
            <asp:Label ID="lblChiuso" runat="server" Text="Chiuso"></asp:Label><br />
            <asp:CheckBox ID="chkChiuso" runat="server" />
        </div>
        <div class="col-1 text-center">
            <asp:Label ID="lblRiscosso" runat="server" Text="Riscosso"></asp:Label><br />
            <asp:CheckBox ID="chkRiscosso" runat="server" />
        </div>
        <div class="col-1 text-center">
            <asp:Label ID="lblFatturato" runat="server" Text="Fatturato"></asp:Label><br />
            <asp:CheckBox ID="chkFatturato" runat="server" />
        </div>
        <div class="col-1 text-center">
            <asp:Label ID="lblNonRiscuotibile" runat="server" Text="Non Riscuotibile"></asp:Label><br />
            <asp:CheckBox ID="chkNonRiscuotibile" runat="server" />
        </div>
    </div>

    <!-- Bottone visualizzazione stampa -->
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:Button ID="btnStampaValoriCantieri" CssClass="btn btn-lg btn-primary" OnClick="btnStampaContoCliente_Click" Text="Stampa Valori Cantieri" runat="server" />
            <asp:Button ID="btnGeneraExcel" CssClass="btn btn-lg btn-dark" OnClick="btnGeneraExcel_Click" Text="Genera Excel" runat="server" />
        </div>
    </div>

    <%-- Label Totale --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:Label ID="lblTotaleGeneraleStampa" CssClass="font-weight-bold lblIntestazione" runat="server"></asp:Label>
        </div>
    </div>

    <!-- Griglia di visualizzazione -->
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col text-center table-container">
            <asp:GridView ID="grdStampaConOpzioni" AutoGenerateColumns="false"
                ItemType="GestioneCantieri.Data.StampaValoriCantieriConOpzioni" CssClass="table table-striped table-dark text-center scrollable-table" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Codice Cantiere" DataField="CodCant" />
                    <asp:BoundField HeaderText="Descrizione Cantiere" DataField="DescriCodCAnt" />
                    <asp:BoundField HeaderText="Cliente" DataField="RagSocCli" />
                    <asp:BoundField HeaderText="Totale Conto" DataField="TotaleConto" DataFormatString="{0:0.00}" />
                    <asp:BoundField HeaderText="Totale Acconti" DataField="TotaleAcconti" DataFormatString="{0:0.00}" />
                    <asp:BoundField HeaderText="Totale Finale" DataField="TotaleFinale" DataFormatString="{0:0.00}" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

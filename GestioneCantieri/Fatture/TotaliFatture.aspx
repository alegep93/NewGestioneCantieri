<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="TotaliFatture.aspx.cs" Inherits="GestioneCantieri.TotaliFatture" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <style>
        span {
            font-size: 24px;
        }

        .ml {
            margin-left: 50px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid">
        <h1>Totali Fatture</h1>

        <%-- Filtro per anno --%>
        <div class="row">
            <div class="col-md-offset-5 col-md-2 text-center">
                <asp:Label ID="lblFiltroAnno" Text="Filtro per Anno" runat="server"></asp:Label>
                <asp:TextBox ID="txtFiltroAnno" AutoPostBack="true" OnTextChanged="txtFiltroAnno_TextChanged" CssClass="form-control text-center" runat="server"></asp:TextBox>
            </div>
        </div>

        <%-- Fatture Emesse --%>
        <div class="row">
            <div class="col-md-12 text-center">
                <asp:Label ID="lblFattureEmesseTotaleImponibile" Text="Totale Imponibile Emesso: " runat="server"></asp:Label>
                <asp:Label ID="lblFattureEmesseTotaleFatturato" CssClass="ml" Text="Totale Fatturato Emesso: " runat="server"></asp:Label>
            </div>
        </div>

        <%-- Fatture acquisto --%>
        <div class="row">
            <div class="col-md-12 text-center">
                <asp:Label ID="lblFattureAcquistoTotaleImponibile" Text="Totale Imponibile Acquisto: " runat="server"></asp:Label>
                <asp:Label ID="lblFattureAcquistoTotaleFatturato" CssClass="ml" Text="Totale Fatturato Acquisto: " runat="server"></asp:Label>
            </div>
        </div>

        <%-- Differenze --%>
        <div class="row">
            <div class="col-md-12 text-center">
                <asp:Label ID="lblDifferenzaTotaleImponibile" Text="Differenze Totale Imponibile: " runat="server"></asp:Label>
                <asp:Label ID="lblDifferenzaTotaleFatturato" CssClass="ml" Text="Differenze Totale Fatturato: " runat="server"></asp:Label>
            </div>
        </div>

        <%-- Bollette --%>
        <div class="row">
            <div class="col-md-12 text-center">
                <asp:Label ID="lblTotaleBollette" Text="Totale Bollette: " runat="server"></asp:Label>
                <asp:Label ID="lblUtile" CssClass="ml" Text="Utile: " runat="server"></asp:Label>
            </div>
        </div>

        <%-- Totali IVA per Quarter --%>
        <div class="row" style="margin-top: 30px;">
            <div class="col-md-offset-2 col-md-8 text-center">
                <asp:GridView ID="grdTotaleIvaPerQuarter" AutoGenerateColumns="false" runat="server" CssClass="table table-striped table-responsive text-center">
                    <Columns>
                        <asp:BoundField HeaderText="Trimestre" DataField="Trimestre" />
                        <asp:BoundField HeaderText="Totale Iva Acquisto" DataField="TotaleIvaAcquisto" DataFormatString="€ {0:###,###.##}" />
                        <asp:BoundField HeaderText="Totale Iva Emesso" DataField="TotaleIvaEmesso" DataFormatString="€ {0:###,###.##}" />
                        <asp:BoundField HeaderText="Saldo" DataField="Saldo" DataFormatString="€ {0:###,###.##}" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <asp:HiddenField ID="hfUtile" runat="server" />

        <%-- Tasse --%>
        <div class="row">
            <div class="col-md-offset-5 col-md-2 text-center">
                <asp:Label ID="lblTassePerc" Text="Tassa %" runat="server"></asp:Label>
                <asp:TextBox ID="txtTassePerc" CssClass="form-control text-center" AutoPostBack="true" OnTextChanged="txtTassePerc_TextChanged" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-12 text-center">
                <asp:Label ID="lblUtileNettoTasse" Text="" runat="server"></asp:Label>
            </div>
        </div>
    </div>
</asp:Content>

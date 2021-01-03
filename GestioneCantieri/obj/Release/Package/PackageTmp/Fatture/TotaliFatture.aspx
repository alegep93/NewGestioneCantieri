<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="TotaliFatture.aspx.cs" Inherits="GestioneCantieri.TotaliFatture" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Totali Fatture</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Totali Fatture</h1>
        </div>
    </div>

    <%-- Filtro per anno --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-2 text-center">
            <asp:Label ID="lblFiltroAnno" Text="Filtro per Anno" runat="server"></asp:Label>
            <asp:TextBox ID="txtFiltroAnno" AutoPostBack="true" OnTextChanged="txtFiltroAnno_TextChanged" CssClass="form-control text-center" runat="server"></asp:TextBox>
        </div>
    </div>

    <%-- Fatture Emesse --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblFattureEmesseTotaleImponibile" Text="Totale Imponibile Emesso: " CssClass="label-totali-fatture" runat="server"></asp:Label>
                </div>
                <div class="col">
                    <asp:Label ID="lblFattureEmesseTotaleFatturato" Text="Totale Fatturato Emesso: " CssClass="label-totali-fatture" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <%-- Fatture acquisto --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblFattureAcquistoTotaleImponibile" Text="Totale Imponibile Acquisto: " CssClass="label-totali-fatture" runat="server"></asp:Label>
                </div>
                <div class="col">
                    <asp:Label ID="lblFattureAcquistoTotaleFatturato" Text="Totale Fatturato Acquisto: " CssClass="label-totali-fatture" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <%-- Differenze --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblDifferenzaTotaleImponibile" Text="Differenze Totale Imponibile: " CssClass="label-totali-fatture" runat="server"></asp:Label>
                </div>
                <div class="col">
                    <asp:Label ID="lblDifferenzaTotaleFatturato" Text="Differenze Totale Fatturato: " CssClass="label-totali-fatture" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <%-- Bollette --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblTotaleBollette" Text="Totale Bollette: " CssClass="label-totali-fatture" runat="server"></asp:Label>
                </div>
                <div class="col">
                    <asp:Label ID="lblUtile" Text="Utile: " CssClass="label-totali-fatture" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>

    <%-- Totali IVA per Quarter --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-8 text-center">
            <asp:GridView ID="grdTotaleIvaPerQuarter" AutoGenerateColumns="false" runat="server" CssClass="table table-striped table-dark text-center scrollable-table">
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
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col-4 text-center">
                    <asp:Label ID="lblTassePerc" Text="Tassa %" runat="server"></asp:Label>
                    <asp:TextBox ID="txtTassePerc" CssClass="form-control text-center" AutoPostBack="true" OnTextChanged="txtTassePerc_TextChanged" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col-4 text-center">
                    <asp:Label ID="lblUtileNettoTasse" Text="" CssClass="label-totali-fatture" runat="server"></asp:Label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

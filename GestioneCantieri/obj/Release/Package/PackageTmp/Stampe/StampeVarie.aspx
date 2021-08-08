<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="StampeVarie.aspx.cs" Inherits="GestioneCantieri.StampeVarie" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Stampe Varie</title>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col">
            <h1>Stampe Varie</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-3 text-center">
            <asp:Label ID="lblScegliStampa" runat="server" Text="Seleziona Stampa"></asp:Label>
            <asp:DropDownList ID="ddlScegliStampa" CssClass="form-control" AutoPostBack="true" OnTextChanged="ddlScegliStampa_TextChanged" runat="server"></asp:DropDownList>
        </div>
    </div>

    <asp:Panel ID="pnlCampiStampaDDT_MatCant" CssClass="row mt-3 d-flex justify-content-center align-items-center" runat="server">
        <div class="col">
            <asp:Label ID="lblDataDa" runat="server" Text="Data Da:"></asp:Label>
            <asp:TextBox ID="txtDataDa" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblDataA" runat="server" Text="Data A:"></asp:Label>
            <asp:TextBox ID="txtDataA" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblScegliFornitore" runat="server" Text="Scegli Fornitore"></asp:Label>
            <asp:DropDownList ID="ddlScegliFornitore" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
        <div class="col">
            <asp:Label ID="lblScegliAcquirente" runat="server" Text="Scegli Acquirente"></asp:Label>
            <asp:DropDownList ID="ddlScegliAcquirente" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
        <div class="col">
            <asp:Label ID="lblNumDDT" runat="server" Text="Numero DDT"></asp:Label>
            <asp:TextBox ID="txtNumDDT" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblNomeFile" runat="server" Text="Nome File"></asp:Label>
            <asp:TextBox ID="txtNomeFile" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Button ID="btnStampaDDT" CssClass="btn btn-lg btn-primary" OnClick="btnStampaDDT_Click" runat="server" Text="Stampa DDT" />
            <asp:Button ID="btnStampaMatCant" CssClass="btn btn-lg btn-primary" OnClick="btnStampaMatCant_Click" runat="server" Text="Stampa Mat Cant" />
            <asp:Button ID="btnAggiungiNumPagine" CssClass="btn btn-lg btn-dark mt-2" OnClick="btnAggiungiNumPagine_Click" runat="server" Text="Aggiungi Num. Pagine" /><br />
            <asp:Label ID="lblIsNomeFileInserito" runat="server" CssClass="pull-right" Text=""></asp:Label>
        </div>
    </asp:Panel>


    <asp:GridView ID="grdStampaDDT" runat="server" ItemType="Database.Models.DDTMef" AutoGenerateColumns="False" CssClass="table table-striped table-responsive text-center scrollable-table" Visible="true">
        <Columns>
            <asp:BoundField DataField="N_ddt" HeaderText="N_DDT" />
            <asp:BoundField DataField="CodArt" HeaderText="Codice Articolo" />
            <asp:BoundField DataField="DescriCodArt" HeaderText="Descrizione Cod. Art." />
            <asp:BoundField DataField="Qta" HeaderText="Quantità" />
            <asp:BoundField DataField="PrezzoUnitario" HeaderText="Prezzo Unit." />
            <asp:BoundField DataField="Valore" HeaderText="Valore" />
        </Columns>
    </asp:GridView>

    <asp:GridView ID="grdStampaMateCant" runat="server" ItemType="Database.Models.MaterialiCantieri" AutoGenerateColumns="False" CssClass="table table-striped table-responsive text-center scrollable-table" Visible="false">
        <Columns>
            <asp:BoundField DataField="NumeroBolla" HeaderText="Num. Bolla" />
            <asp:BoundField DataField="Fornitore" HeaderText="Fornit." />
            <asp:BoundField DataField="CodCant" HeaderText="CodCant" />
            <asp:BoundField DataField="Acquirente" HeaderText="Acquirente" />
            <asp:BoundField DataField="CodArt" HeaderText="CodArt" />
            <asp:BoundField DataField="DescriCodArt" HeaderText="Descr. CodArt" />
            <asp:BoundField DataField="Qta" HeaderText="Qta" />
            <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Pzzo Unit." />
            <asp:BoundField DataField="Valore" HeaderText="Valore" />
        </Columns>
    </asp:GridView>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="DDT-Fornitori.aspx.cs" Inherits="GestioneCantieri.DDT_Fornitori" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>DDT Fornitori</title>
    <style>
        .btn.btn-lg.btn-primary {
            position: relative;
            top: 13px;
            width: 180px;
        }

        .errorLabel {
            position: relative;
            top: 13px;
            left: 10px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <%-- Titolo Pagina --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col">
            <h1>DDT Fornitori</h1>
        </div>
    </div>

    <%-- Inserimento nuovo DDT Fornitore --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col">
            <asp:Label ID="lblScegliFornitore" Text="Scegli Fornitore" runat="server"></asp:Label>
            <asp:DropDownList ID="ddlScegliFornitore" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
        <div class="col">
            <asp:Label ID="lblInsData" Text="Data" runat="server"></asp:Label>
            <asp:TextBox ID="txtInsData" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblInsProtocollo" Text="Protocollo" runat="server"></asp:Label>
            <asp:TextBox ID="txtInsProtocollo" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblInsNumeroDdt" Text="Numero DDT" runat="server"></asp:Label>
            <asp:TextBox ID="txtInsNumeroDdt" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblInsArticolo" Text="Articolo" runat="server"></asp:Label>
            <asp:TextBox ID="txtInsArticolo" MaxLength="50" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblInsDescrForn" Text="Descr. Fornitore" runat="server"></asp:Label>
            <asp:TextBox ID="txtInsDescrForn" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblInsDescrMau" Text="Descr. Mau" runat="server"></asp:Label>
            <asp:TextBox ID="txtInsDescrMau" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblInsQta" Text="Quantità" runat="server"></asp:Label>
            <asp:TextBox ID="txtInsQta" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblInsValore" Text="Valore" runat="server"></asp:Label>
            <asp:TextBox ID="txtInsValore" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col button-col">
            <asp:Button ID="btnInserisciDDT" CssClass="btn btn-lg btn-primary" OnClick="btnInserisciDDT_Click" Text="Inserisci" runat="server"></asp:Button>
            <asp:Button ID="btnModificaDDT" CssClass="btn btn-lg btn-primary" OnClick="btnModificaDDT_Click" Text="Modifica" runat="server"></asp:Button>
            <asp:Label ID="lblError" Text="" CssClass="errorLabel" runat="server"></asp:Label>
        </div>
    </div>

    <%-- Filtri --%>
    <asp:Panel ID="pnlFiltri" DefaultButton="btnFiltra" CssClass="row mt-5 d-flex justify-content-center align-items-center" runat="server">
        <div class="col">
            <asp:Label ID="lblFiltraFornitore" Text="Filtro Fornitore" runat="server"></asp:Label>
            <asp:DropDownList ID="ddlFiltraFornitore" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
        <div class="col">
            <asp:Label ID="lblFiltraProtocollo" Text="Filtro Protocollo" runat="server"></asp:Label>
            <asp:TextBox ID="txtFiltraProtocollo" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblFiltraNumeroDdt" Text="Filtro Numero DDT" runat="server"></asp:Label>
            <asp:TextBox ID="txtFiltraNumeroDdt" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblFiltraArticolo" Text="Filtro Articolo" runat="server"></asp:Label>
            <asp:TextBox ID="txtFiltraArticolo" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblFiltraQta" Text="Filtro Quantità" runat="server"></asp:Label>
            <asp:TextBox ID="txtFiltraQta" TextMode="Number" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblFiltroDescrForn" Text="Filtro Descr. Fornitore" runat="server"></asp:Label>
            <asp:TextBox ID="txtFiltroDescrForn" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblFiltroDescrMau" Text="Filtro Descr. Mau" runat="server"></asp:Label>
            <asp:TextBox ID="txtFiltroDescrMau" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col button-col">
            <asp:Button ID="btnFiltra" CssClass="btn btn-lg btn-primary" OnClick="btnFiltra_Click" Text="Filtra" runat="server"></asp:Button>
        </div>
    </asp:Panel>

    <asp:HiddenField ID="hfIdDDT" runat="server" />

    <%-- Visualizzazione dati --%>
    <div class="row mt-2">
        <div class="col text-center table-overflow ddt-fornitori-table">
            <asp:GridView ID="grdListaDDTFornitori" runat="server" ItemType="GestioneCantieri.Data.DDTFornitori" AutoGenerateColumns="False"
                OnRowCommand="grdListaDDTFornitori_RowCommand" HeaderStyle-CssClass="border-bottom-bold" CssClass="table table-dark table-striped scrollable-table">
                <Columns>
                    <asp:BoundField DataField="RagSocFornitore" HeaderText="Rag. Soc. Fornitore" />
                    <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                    <asp:BoundField DataField="Protocollo" HeaderText="Protocollo" />
                    <asp:BoundField DataField="NumeroDdt" HeaderText="Numero DDT" />
                    <asp:BoundField DataField="Articolo" HeaderText="Articolo" />
                    <asp:BoundField DataField="DescrizioneFornitore" HeaderText="Descrizione Fornitore" />
                    <asp:BoundField DataField="DescrizioneMau" HeaderText="Descrizione Mau" />
                    <asp:BoundField DataField="Qta" HeaderText="Quantità" />
                    <asp:BoundField DataField="Valore" HeaderText="Valore" DataFormatString="{0:0.00}" />
                    <asp:BoundField DataField="PrezzoUnitario" HeaderText="Prezzo Unitario" DataFormatString="{0:0.00}" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnMod" CommandName="ModDDT" CommandArgument="<%# BindItem.Id %>" CssClass="btn btn-lg btn-default" runat="server" Text="Modifica">
                                    <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElim" CommandName="ElimDDT" CommandArgument="<%# BindItem.Id %>"
                                CssClass="btn btn-lg btn-default" runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo DDT Fornitore?');">
                                    <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

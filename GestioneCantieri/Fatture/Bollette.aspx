<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="Bollette.aspx.cs" Inherits="GestioneCantieri.Bollette" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 text-center">
                <h1>Bollette</h1>
            </div>
        </div>
        <div class="row" style="margin-top: 0px;">
            <div class="col-md-4">
                <asp:Label ID="lblDataBolletta" runat="server">Data Bolletta</asp:Label>
                <asp:TextBox ID="txtDataBolletta" AutoPostBack="true" OnTextChanged="txtDataBolletta_TextChanged" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <asp:Label ID="lblDataScadenza" runat="server">Data Scadenza</asp:Label>
                <asp:TextBox ID="txtDataScadenza" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <asp:Label ID="lblDataPagamento" runat="server">Data Pagamento</asp:Label>
                <asp:TextBox ID="txtDataPagamento" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
            </div>

        </div>
        <div class="row" style="margin-top: 0px;">
            <div class="col-md-4">
                <asp:Label ID="lblProgressivo" runat="server">Progressivo</asp:Label>
                <asp:TextBox ID="txtProgressivo" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-4">
                <asp:Label ID="lblScegliFornitore" runat="server">Scegli Fornitore</asp:Label>
                <asp:DropDownList ID="ddlScegliFornitore" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
            <div class="col-md-4">
                <asp:Label ID="lblTotaleBolletta" runat="server">Totale Bolletta</asp:Label>
                <asp:TextBox ID="txtTotaleBolletta" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
        </div>
        <div class="row" style="margin-top: 0px;">
            <div class="col-md-12 text-center">
                <asp:Button ID="btnAggiungiBolletta" CssClass="btn btn-lg btn-primary" OnClick="btnAggiungiBolletta_Click" Text="Aggiungi Bolletta" runat="server" />
                <asp:Button ID="btnModificaBolletta" CssClass="btn btn-lg btn-primary" OnClick="btnModificaBolletta_Click" Text="Modifica Bolletta" runat="server" />
            </div>
            <div class="col-md-12 text-center">
                <asp:Label ID="lblMsg" runat="server"></asp:Label>
                <asp:HiddenField ID="hfIdBolletta" runat="server"></asp:HiddenField>
            </div>
        </div>

        <div class="row" style="margin-top: 30px;">
            <div class="col-md-offset-3 col-md-3">
                <asp:Label ID="lblFiltroAnno" Text="Filtro per Anno" runat="server"></asp:Label>
                <asp:TextBox ID="txtFiltroAnno" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-3">
                <asp:Label ID="lblFiltroFornitore" Text="Filtro per Fornitore" runat="server"></asp:Label>
                <asp:DropDownList ID="ddlFiltroFornitore" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
            <div class="col-md-12 text-center">
                <asp:Button ID="btnFiltraBollette" CssClass="btn btn-lg btn-primary" OnClick="btnFiltraBollette_Click" Text="Filtra" runat="server" />
            </div>
        </div>

        <div class="row text-center" style="margin-top: 10px;">
            <div class="col-md-offset-2 col-md-8 text-center tableContainer">
                <asp:GridView ID="grdBollette" AutoGenerateColumns="false" ItemType="GestioneCantieri.Data.Bolletta" OnRowCommand="grdBollette_RowCommand" CssClass="table table-striped text-center scrollable-table" runat="server">
                    <Columns>
                        <asp:BoundField HeaderText="Fornitore" DataField="RagSocForni" />
                        <asp:BoundField HeaderText="Progressivo" DataField="Progressivo" />
                        <asp:BoundField HeaderText="Data Bolletta" DataField="DataBolletta" DataFormatString="{0:d}" />
                        <asp:BoundField HeaderText="Data Scadenza" DataField="DataScadenza" DataFormatString="{0:d}" />
                        <asp:BoundField HeaderText="Data Pagamento" DataField="DataPagamento" DataFormatString="{0:d}" />
                        <asp:BoundField HeaderText="Totale Bolletta" DataField="TotaleBolletta" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnModifica" CommandName="Modifica" CommandArgument="<%# BindItem.IdBollette %>" runat="server">
                                    <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:LinkButton ID="btnElimina" CommandName="Elimina" CommandArgument="<%# BindItem.IdBollette %>" runat="server" OnClientClick="return confirm('Vuoi veramente eliminare questa bolletta?');">
                                    <i class="fas fa-times" style="color: red;"></i>
                                </asp:LinkButton>
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

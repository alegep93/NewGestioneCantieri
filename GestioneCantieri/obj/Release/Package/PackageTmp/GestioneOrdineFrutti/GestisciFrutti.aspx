<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="GestisciFrutti.aspx.cs" Inherits="GestioneCantieri.GestisciFrutti" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Gestisci Frutti</title>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <%--<div class="row mt-3">
        <div class="col-md-12 text-center btnChoosePanelContainer">
            <asp:Button ID="btnApriInserisci" OnClick="btnApriInserisci_Click" CssClass="btn btn-dark btn-lg" runat="server" Text="Inserisci" />
            <asp:Button ID="btnApriModifica" OnClick="btnApriModifica_Click" CssClass="btn btn-dark btn-lg" runat="server" Text="Modifica" />
            <asp:Button ID="btnApriElimina" OnClick="btnApriElimina_Click" CssClass="btn btn-dark btn-lg" runat="server" Text="Elimina" />
        </div>
    </div>--%>

    <%-- Titolo Pagina --%>
    <div class="row mt-3">
        <div class="col text-center">
            <h1>
                <asp:Label ID="lblTitle" runat="server" Text="Gestisci Frutti"></asp:Label>
            </h1>
        </div>
    </div>

    <asp:Panel ID="pnlInserisci" runat="server">
        <div class="row d-flex justify-content-center align-items-center">
            <div class="col-4 text-center">
                <asp:Label ID="lblNomeFrutto" runat="server" Text="Nome Frutto"></asp:Label>
                <asp:TextBox ID="txtNomeFrutto" CssClass="form-control text-center" runat="server"></asp:TextBox>
                <asp:Button ID="btnInsFrutto" OnClick="btnInsFrutto_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Inserisci Frutto" />
                <asp:Button ID="btnSaveModFrutto" OnClick="btnSaveModFrutto_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Modifica Frutto" Visible="false" /><br />
                <asp:Label ID="lblMsg" runat="server" Text="" CssClass="pull-right"></asp:Label>
            </div>
        </div>
    </asp:Panel>

    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-4">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:TextBox ID="txtFiltroFrutti1" placeholder="Filtro 1" OnTextChanged="txtFiltroListaFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:TextBox ID="txtFiltroFrutti2" placeholder="Filtro 2" OnTextChanged="txtFiltroListaFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:TextBox ID="txtFiltroFrutti3" placeholder="Filtro 3" OnTextChanged="txtFiltroListaFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfIdFrutto" runat="server" />

    <%-- Lista Frutti --%>
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-6 text-center table-container">
            <asp:GridView ID="grdFrutti" ItemType="Database.Models.Frutto" OnRowCommand="grdFrutti_RowCommand" CssClass="table table-dark table-striped scrollable-table" AutoGenerateColumns="false" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Nome Frutto" DataField="Nome" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnModifica" CommandName="Modifica" CommandArgument="<%# BindItem.IdFrutti %>" runat="server">
                                    <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElimina" CommandName="Elimina" CommandArgument="<%# BindItem.IdFrutti %>" runat="server" OnClientClick="return confirm('Vuoi veramente eliminare questo frutto?');">
                                    <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

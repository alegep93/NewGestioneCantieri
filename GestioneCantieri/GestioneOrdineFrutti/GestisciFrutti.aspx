<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="GestisciFrutti.aspx.cs" Inherits="GestioneCantieri.GestisciFrutti" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Gestisci Frutti</title>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3">
        <div class="col-md-12 text-center btnChoosePanelContainer">
            <asp:Button ID="btnApriInserisci" OnClick="btnApriInserisci_Click" CssClass="btn btn-dark btn-lg" runat="server" Text="Inserisci" />
            <asp:Button ID="btnApriModifica" OnClick="btnApriModifica_Click" CssClass="btn btn-dark btn-lg" runat="server" Text="Modifica" />
            <asp:Button ID="btnApriElimina" OnClick="btnApriElimina_Click" CssClass="btn btn-dark btn-lg" runat="server" Text="Elimina" />
        </div>
    </div>

    <%-- Titolo Pagina --%>
    <div class="row mt-3">
        <div class="col text-center">
            <h1>
                <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
            </h1>
        </div>
    </div>

    <asp:Panel ID="pnlInserisci" runat="server">
        <div class="row row d-flex justify-content-center align-items-center">
            <div class="col-4 text-center">
                <div class="panel panel-default">
                    <div class="panel-body">
                        <asp:Label ID="lblInsFrutto" runat="server" Text="Nome Frutto"></asp:Label>
                        <asp:TextBox ID="txtInsNomeFrutto" CssClass="form-control text-center" runat="server"></asp:TextBox>
                        <asp:Button ID="btnInsFrutto" OnClick="btnInsFrutto_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Inserisci Frutto" /><br />
                        <asp:Label ID="lblIsFruttoInserito" runat="server" Text="" CssClass="pull-right"></asp:Label>
                    </div>
                </div>

                <div class="row">
                    <div class="col-4">
                        <asp:TextBox ID="txtFiltroFruttiIns1" placeholder="Filtro 1" OnTextChanged="txtFiltroListaFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-4">
                        <asp:TextBox ID="txtFiltroFruttiIns2" placeholder="Filtro 2" OnTextChanged="txtFiltroListaFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-4">
                        <asp:TextBox ID="txtFiltroFruttiIns3" placeholder="Filtro 3" OnTextChanged="txtFiltroListaFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlModifica" runat="server">
        <div class="row row d-flex justify-content-center align-items-center">
            <div class="col-4 text-center">
                <div class="row">
                    <div class="col-4">
                        <asp:TextBox ID="txtFiltroFruttiMod1" placeholder="Filtro 1" OnTextChanged="txtFiltroFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-4">
                        <asp:TextBox ID="txtFiltroFruttiMod2" placeholder="Filtro 2" OnTextChanged="txtFiltroFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-4">
                        <asp:TextBox ID="txtFiltroFruttiMod3" placeholder="Filtro 3" OnTextChanged="txtFiltroFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <asp:Label ID="lblModScegliFrutto" runat="server" Text="Scegli Frutto"></asp:Label>
                <asp:DropDownList ID="ddlModScegliFrutto" CssClass="form-control" runat="server" OnTextChanged="ddlModScegliFrutto_TextChanged" AutoPostBack="true"></asp:DropDownList>

                <asp:Panel ID="pnlModFrutto" runat="server">
                    <asp:Label ID="lblModNomeFrutto" runat="server" Text="Nome Frutto"></asp:Label>
                    <asp:TextBox ID="txtModNomeFrutto" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSaveModFrutto" OnClick="btnSaveModFrutto_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Modifica Frutto" /><br />
                    <asp:Label ID="lblSaveModFrutto" runat="server" Text="" CssClass="pull-right"></asp:Label>
                </asp:Panel>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlElimina" runat="server">
        <div class="row row d-flex justify-content-center align-items-center">
            <div class="col-4 text-center">
                <div class="row">
                    <div class="col-4">
                        <asp:TextBox ID="txtFiltroFruttiDel1" placeholder="Filtro 1" OnTextChanged="txtFiltroFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-4">
                        <asp:TextBox ID="txtFiltroFruttiDel2" placeholder="Filtro 2" OnTextChanged="txtFiltroFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                    <div class="col-4">
                        <asp:TextBox ID="txtFiltroFruttiDel3" placeholder="Filtro 3" OnTextChanged="txtFiltroFrutti_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                    </div>
                </div>

                <asp:Label ID="lblDelFrutto" runat="server" Text="Nome Frutto"></asp:Label>
                <asp:DropDownList ID="ddlDelFrutto" OnTextChanged="ddlDelFrutto_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:DropDownList>
                <asp:Button ID="btnDelFrutto" OnClick="btnDelFrutto_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Elimina Frutto" OnClientClick="return confirm('Vuoi veramente eliminare questo frutto?');" /><br />
                <asp:Label ID="lblIsDelFrutto" runat="server" Text="" CssClass="pull-right"></asp:Label>
            </div>
        </div>
    </asp:Panel>

    <%-- Lista Frutti --%>
    <div class="row mt-2 d-flex justify-content-center align-items-center">
        <div class="col-4 text-center">
            <div class="panel panel-default">
                <div class="panel-body">
                    <ul class="list-group">
                        <% foreach (var item in fruttiList)
                            {%>
                        <li class="list-group-item list-group-item-dark"><%= item.Descr001 %></li>
                        <% } %>
                    </ul>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="GestisciGruppiFrutti.aspx.cs" Inherits="GestioneCantieri.DistintaBase" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Gestisci Gruppi Frutti</title>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Gestisci Gruppi Frutti</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <div class="row">
                <div class="col-4">
                    <asp:TextBox ID="txtFiltroGruppo1" placeholder="Filtro 1" OnTextChanged="txtFiltroGruppo_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-4">
                    <asp:TextBox ID="txtFiltroGruppo2" placeholder="Filtro 2" OnTextChanged="txtFiltroGruppo_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-4">
                    <asp:TextBox ID="txtFiltroGruppo3" placeholder="Filtro 3" OnTextChanged="txtFiltroGruppo_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row">
                <div class="col">
                    <asp:Label ID="lblScegliGruppo" runat="server" Text="Scegli Gruppo"></asp:Label>
                    <asp:DropDownList ID="ddlScegliGruppo" CssClass="form-control" runat="server" OnTextChanged="ddlScegliGruppo_TextChanged1" AutoPostBack="true"></asp:DropDownList>

                    <asp:Panel ID="pnlAssociaFruttoAGruppo" Visible="false" runat="server">
                        <asp:Label ID="lblScegliFrutto" runat="server" Text="Scegli Frutto"></asp:Label>
                        <asp:DropDownList ID="ddlScegliFrutto" CssClass="form-control" runat="server"></asp:DropDownList>

                        <asp:Label ID="lblQuantitàFrutto" runat="server" Text="Quantità"></asp:Label>
                        <asp:TextBox ID="txtQuantitàFrutto" CssClass="form-control" runat="server"></asp:TextBox>

                        <asp:Button ID="btnInserisciFruttoInGruppo" CssClass="btn btn-primary" OnClick="btnInserisciFruttoInGruppo_Click" Text="Inserisci" runat="server" />
                        <asp:Button ID="btnModificaFruttoInGruppo" CssClass="btn btn-primary" OnClick="btnModificaFruttoInGruppo_Click" Text="Modifica" runat="server" />
                    </asp:Panel>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

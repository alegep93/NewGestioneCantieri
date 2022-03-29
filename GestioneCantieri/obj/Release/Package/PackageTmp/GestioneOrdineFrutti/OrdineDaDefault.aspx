<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="OrdineDaDefault.aspx.cs" Inherits="GestioneCantieri.GestioneOrdineFrutti.OrdineDaDefault" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Ordine Da Default</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row d-flex justify-content-center align-items-center mt-3">
        <div class="col text-center">
            <h1>Ordine Da Default</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <!-- Scegli Cantiere -->
        <div class="col-6 text-center">
            <asp:Label ID="lblScegliCantiere" runat="server" Text="Scegli Cantiere"></asp:Label>
            <asp:DropDownList ID="ddlScegliCantiere" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
    </div>

    <div class="row d-flex justify-content-center align-items-center mt-3">
        <div class="col-4 mt-3 text-center" runat="server">
            <asp:Label ID="lblScegliLocaleDefault" runat="server" Text="Scegli Locale Sorgente (Default)"></asp:Label>
            <asp:DropDownList ID="ddlScegliLocaleDefault" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
        <div class="col-4 mt-3 text-center" runat="server">
            <asp:Label ID="lblScegliLocale" runat="server" Text="Scegli Locale di destinazione"></asp:Label>
            <asp:DropDownList ID="ddlScegliLocale" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
    </div>

    <div class="row d-flex justify-content-center align-items-center mt-3">
        <asp:Button ID="btnInserisciDaDefault" CssClass="btn btn-lg btn-dark" OnClick="btnInserisciDaDefault_Click" Text="Inserisci da Default" runat="server"></asp:Button>
    </div>
</asp:Content>

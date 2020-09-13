<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="StampaFruttiGruppi.aspx.cs" Inherits="GestioneCantieri.StampaFruttiGruppi" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Stampa Frutti Gruppi</title>
    <script src="Scripts/jquery-1.9.1.min.js"></script>
    <script type="text/javascript">
        $(function () {
            $(".groupHeader").css("font-weight", "bold")
            $("table.table-striped td:not(.groupHeader)").css("padding-left", 40)
        });
    </script>
    <style>
    </style>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3">
        <div class="col text-center">
            <h1>Stampa Frutti Gruppi</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-md-6">
            <asp:Label ID="lblScegliGruppo" runat="server" Text="Scegli Gruppo"></asp:Label>
            <asp:DropDownList ID="ddlScegliGruppo" CssClass="form-control" OnTextChanged="ddlScegliGruppo_TextChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-md-6 table-container">
            <asp:GridView ID="grdFruttiInGruppo" AllowSorting="true" OnSorting="grdFruttiInGruppo_Sorting"
                AutoGenerateColumns="false" ItemType="GestioneCantieri.Data.StampaFruttiPerGruppi"
                runat="server" CssClass="table table-dark table-striped scrollable-table">
                <Columns>
                    <asp:BoundField HeaderText="Nome Gruppo" DataField="NomeGruppo" />
                    <asp:BoundField HeaderText="Nome Gruppo / Nome Frutto" DataField="NomeFrutto" />
                    <asp:BoundField HeaderText="Quantità" DataField="Qta" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

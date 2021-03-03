<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="CodiciMef.aspx.cs" Inherits="GestioneCantieri.CodiciMef" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Codici Mef</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Codici Mef</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-2 text-center">
            <asp:Label ID="lblAnnoDa" runat="server" Text="Da"></asp:Label>
            <asp:TextBox ID="txtAnnoDa" CssClass="form-control text-center" AutoPostBack="true" OnTextChanged="txtAnno_TextChanged" TextMode="Number" runat="server"></asp:TextBox>
        </div>
        <div class="col-2 text-center">
            <asp:Label ID="lblAnnoA" runat="server" Text="A"></asp:Label>
            <asp:TextBox ID="txtAnnoA" CssClass="form-control text-center" AutoPostBack="true" OnTextChanged="txtAnno_TextChanged" TextMode="Number" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-4 text-center table-overflow tableContainer">
            <asp:GridView ID="grdCodiciMef" runat="server" AutoGenerateColumns="false" ItemType="System.String" CssClass="table table-dark table-striped scrollable-table">
                <Columns>
                    <asp:TemplateField>
                        <HeaderTemplate>
                            <asp:Label ID="lblCodiceMefHeader" Text="Codice Mef" runat="server"></asp:Label>
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:Label ID="lblCodiceMef" Text="<%# Item %>" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

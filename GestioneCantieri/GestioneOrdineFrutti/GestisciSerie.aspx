<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="GestisciSerie.aspx.cs" Inherits="GestioneCantieri.GestioneOrdineFrutti.GestisciSerie" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Gestisci Serie</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3">
        <div class="col text-center">
            <h1>
                <asp:Label ID="lblTitle" runat="server" Text="Gestisci Serie"></asp:Label>
            </h1>
        </div>
    </div>

    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-4 text-center">
            <asp:Label ID="lblNomeSerie" runat="server" Text="Nome Serie"></asp:Label>
            <asp:TextBox ID="txtNomeSerie" CssClass="form-control text-center" runat="server"></asp:TextBox>
            <asp:Button ID="btnInserisciSerie" OnClick="btnInsSerie_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Inserisci Serie" />
            <asp:Button ID="btnModificaSerie" OnClick="btnModificaSerie_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Modifica Serie" Visible="false" />
        </div>
    </div>

    <asp:HiddenField ID="hfIdSerie" runat="server" />

    <%-- Lista Serie --%>
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-6 text-center table-container">
            <asp:GridView ID="grdSerie" ItemType="GestioneCantieri.Data.Serie" OnRowCommand="grdSerie_RowCommand" CssClass="table table-dark table-striped scrollable-table" AutoGenerateColumns="false" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Nome Serie" DataField="NomeSerie" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnModifica" CommandName="Modifica" CommandArgument="<%# BindItem.IdSerie %>" runat="server">
                                    <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElimina" CommandName="Elimina" CommandArgument="<%# BindItem.IdSerie %>" runat="server" OnClientClick="return confirm('Vuoi veramente eliminare questa serie?');">
                                    <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="GestioneLocali.aspx.cs" Inherits="GestioneCantieri.GestioneLocali" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Gestione Locali</title>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Gestione Locali</h1>
        </div>
    </div>

    <asp:HiddenField ID="hfIdLocale" runat="server" />

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6">
            <asp:Label ID="lblNomeLocale" Text="Nome Locale" runat="server"></asp:Label>
            <asp:TextBox ID="txtNomeLocale" CssClass="form-control" runat="server"></asp:TextBox>
            <asp:Button ID="btnInserisciLocale" CssClass="btn btn-lg btn-primary pull-right" OnClick="btnInserisciLocale_Click" Text="Inserisci Locale" runat="server" />
            <asp:Button ID="btnModificaLocale" CssClass="btn btn-lg btn-primary pull-right" OnClick="btnModificaLocale_Click" Text="Modifica Locale" runat="server" />
            <asp:Label ID="lblError" Text="" runat="server"></asp:Label>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 table-container text-center">
            <asp:GridView ID="grdLocali" AutoGenerateColumns="false" CssClass="table table-striped table-dark scrollable-table" ItemType="GestioneCantieri.Data.Locali" OnRowCommand="grdLocali_RowCommand" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Nome Locale" DataField="NomeLocale" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnModificaLocale" CommandName="ModificaLocale" CommandArgument="<%# BindItem.IdLocali %>" CssClass="btn btn-lg btn-default" runat="server" Text="Modifica">
                                <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElimLocale" CommandName="EliminaLocale" CommandArgument="<%# BindItem.IdLocali %>"
                                CssClass="btn btn-lg btn-default" runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo locale?');">
                                <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

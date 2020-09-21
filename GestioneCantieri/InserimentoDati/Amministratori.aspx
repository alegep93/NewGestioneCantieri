<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="Amministratori.aspx.cs" Inherits="GestioneCantieri.Amministratori" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Amministratori</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-4 text-center">
            <asp:Label ID="lblNomeAmministratore" runat="server">Nome Amministratore</asp:Label>
            <asp:TextBox ID="txtNomeAmministratore" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:Button ID="btnAggiungiAmministratore" CssClass="btn btn-lg btn-primary" OnClick="btnAggiungiAmministratore_Click" Text="Aggiungi amministratore" runat="server"></asp:Button>
            <asp:Button ID="btnModificaAmministratore" CssClass="btn btn-lg btn-primary" OnClick="btnModificaAmministratore_Click" Visible="false" Text="Modifica amministratore" runat="server"></asp:Button>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:HiddenField ID="hfIdAmministratore" runat="server" />
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <asp:GridView ID="grdAmministratori" AutoGenerateColumns="false" ItemType="GestioneCantieri.Data.Amministratore"
                OnRowCommand="grdAmministratori_RowCommand" CssClass="table table-striped table-dark text-center scrollable-table" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Nome amministratore" DataField="Nome" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnModifica" CommandName="Modifica" CommandArgument="<%# BindItem.IdAmministratori %>" runat="server">
                                <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElimina" CommandName="Elimina" CommandArgument="<%# BindItem.IdAmministratori %>" runat="server" OnClientClick="return confirm('Vuoi veramente eliminare questo amministratore?');">
                                <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

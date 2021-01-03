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
        <div class="col-4">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <asp:TextBox ID="txtFiltroGruppo1" placeholder="Filtro 1" OnTextChanged="txtFiltroGruppo_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col text-center">
                    <asp:TextBox ID="txtFiltroGruppo2" placeholder="Filtro 2" OnTextChanged="txtFiltroGruppo_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col text-center">
                    <asp:TextBox ID="txtFiltroGruppo3" placeholder="Filtro 3" OnTextChanged="txtFiltroGruppo_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-4 text-center">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <asp:Label ID="lblScegliGruppo" runat="server" Text="Scegli Gruppo"></asp:Label>
                    <asp:DropDownList ID="ddlScegliGruppo" CssClass="form-control" runat="server" OnTextChanged="ddlScegliGruppo_TextChanged" AutoPostBack="true"></asp:DropDownList>
                </div>
            </div>
        </div>
    </div>

    <asp:Panel ID="pnlAssociaFruttoAGruppo" CssClass="row d-flex justify-content-center align-items-center" Visible="false" runat="server">
        <div class="col-6 text-center">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblScegliFrutto" runat="server" Text="Scegli Frutto"></asp:Label>
                    <asp:DropDownList ID="ddlScegliFrutto" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
                <div class="col">
                    <asp:Label ID="lblQuantitàFrutto" runat="server" Text="Quantità"></asp:Label>
                    <asp:TextBox ID="txtQuantitàFrutto" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Button ID="btnInserisciFruttoInGruppo" CssClass="btn btn-primary" OnClick="btnInserisciFruttoInGruppo_Click" Text="Inserisci Frutto" runat="server" />
                    <asp:Button ID="btnModificaFruttoInGruppo" CssClass="btn btn-primary" OnClick="btnModificaFruttoInGruppo_Click" Text="Modifica Frutto" Visible="false" runat="server" />
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:HiddenField ID="hfIdCompGruppoFrutto" runat="server" />

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center table-container">
            <asp:GridView ID="grdFruttiInGruppo" ItemType="GestioneCantieri.Data.CompGruppoFrut" OnRowCommand="grdGruppi_RowCommand" CssClass="table table-dark table-striped" AutoGenerateColumns="false" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Nome Frutto" DataField="NomeFrutto" />
                    <asp:BoundField HeaderText="Quantità" DataField="Qta" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnVisualizza" CommandName="Visualizza" CommandArgument="<%# BindItem.Id %>" runat="server">
                                    <i class="fas fa-eye" style="color: darkblue;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnModifica" CommandName="Modifica" CommandArgument="<%# BindItem.Id %>" runat="server">
                                    <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElimina" CommandName="Elimina" CommandArgument="<%# BindItem.Id %>" runat="server" OnClientClick="return confirm('Vuoi veramente eliminare questo componente?');">
                                    <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

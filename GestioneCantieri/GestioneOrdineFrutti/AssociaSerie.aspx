<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="AssociaSerie.aspx.cs" Inherits="GestioneCantieri.GestioneOrdineFrutti.AssociaSerie" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Associa Serie</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <%-- Associazione serie a Frutto --%>
    <div class="row mt-5">
        <div class="col text-center">
            <h1>
                <asp:Label ID="lblAssociaSerieTitle" runat="server" Text="Associa Serie"></asp:Label>
            </h1>
        </div>
    </div>

    <div class="row d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col-3 text-center">
                    <asp:Label ID="lblScegliSerie" runat="server" Text="Scegli Serie"></asp:Label>
                </div>
                <div class="col-3 text-center">
                    <asp:Label ID="lblScegliFrutto" runat="server" Text="Scegli Frutto"></asp:Label>
                </div>
                <div class="col-5 ml-2 text-center">
                    <asp:Label ID="lblScegliListino" runat="server" Text="Scegli Listino"></asp:Label>
                </div>
            </div>

            <div class="row mt-3 d-flex justify-content-center align-items-end">
                <div class="col-3 text-center">

                    <asp:DropDownList ID="ddlScegliSerie" CssClass="form-control text-center" runat="server"></asp:DropDownList>
                </div>
                <div class="col-3 text-center">
                    <div class="row d-flex justify-content-center align-items-end">
                        <div class="col text-center">
                            <div class="row d-flex justify-content-center align-items-center">
                                <div class="col text-center">
                                    <asp:TextBox ID="txtFiltroFrutti1" CssClass="form-control text-center" placeholder="Nome Frutto" AutoPostBack="true" OnTextChanged="txtFiltroFrutti_TextChanged" runat="server"></asp:TextBox>
                                </div>
                                <div class="col text-center">
                                    <asp:TextBox ID="txtFiltroFrutti2" CssClass="form-control text-center" placeholder="Nome Frutto" AutoPostBack="true" OnTextChanged="txtFiltroFrutti_TextChanged" runat="server"></asp:TextBox>
                                </div>
                                <div class="col text-center">
                                    <asp:TextBox ID="txtFiltroFrutti3" CssClass="form-control text-center" placeholder="Nome Frutto" AutoPostBack="true" OnTextChanged="txtFiltroFrutti_TextChanged" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>

                    <div class="row d-flex justify-content-center align-items-center">
                        <asp:DropDownList ID="ddlScegliFrutto" CssClass="form-control text-center" runat="server"></asp:DropDownList>
                    </div>
                </div>
                <div class="col-5 ml-2 text-center">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col text-center">
                            <asp:TextBox ID="txtFiltroCodice1" CssClass="form-control text-center" placeholder="Codice Listino" AutoPostBack="true" OnTextChanged="txtFiltroListino_TextChanged" runat="server"></asp:TextBox>
                        </div>
                        <div class="col text-center">
                            <asp:TextBox ID="txtFiltroCodice2" CssClass="form-control text-center" placeholder="Codice Listino" AutoPostBack="true" OnTextChanged="txtFiltroListino_TextChanged" runat="server"></asp:TextBox>
                        </div>
                        <div class="col text-center">
                            <asp:TextBox ID="txtFiltroCodice3" CssClass="form-control text-center" placeholder="Codice Listino" AutoPostBack="true" OnTextChanged="txtFiltroListino_TextChanged" runat="server"></asp:TextBox>
                        </div>
                    </div>

                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col text-center">
                            <asp:TextBox ID="txtFiltroDescr1" CssClass="form-control text-center" placeholder="Descrizione Listino" AutoPostBack="true" OnTextChanged="txtFiltroListino_TextChanged" runat="server"></asp:TextBox>
                        </div>
                        <div class="col text-center">
                            <asp:TextBox ID="txtFiltroDescr2" CssClass="form-control text-center" placeholder="Descrizione Listino" AutoPostBack="true" OnTextChanged="txtFiltroListino_TextChanged" runat="server"></asp:TextBox>
                        </div>
                        <div class="col text-center">
                            <asp:TextBox ID="txtFiltroDescr3" CssClass="form-control text-center" placeholder="Descrizione Listino" AutoPostBack="true" OnTextChanged="txtFiltroListino_TextChanged" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row d-flex justify-content-center align-items-center">
                        <asp:DropDownList ID="ddlScegliListino" CssClass="form-control text-center" runat="server"></asp:DropDownList>
                    </div>
                </div>
            </div>

            <div class="row d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <asp:Button ID="btnInserisciFruttoSerie" OnClick="btnInserisciFruttoSerie_Click" CssClass="btn btn-primary" runat="server" Text="Inserisci Associazione" />
                    <asp:Button ID="btnModificaFruttoSerie" OnClick="btnModificaFruttoSerie_Click" CssClass="btn btn-primary" runat="server" Text="Modifica Associazione" Visible="false" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfIdFruttoSerie" runat="server" />

    <div class="row mt-4 d-flex justify-content-center align-items-center">
        <div class="col-2 text-center">
            <asp:Label ID="lblFiltroSerie" Text="Filtra Serie" runat="server"></asp:Label>
            <asp:DropDownList ID="ddlFiltroSerie" CssClass="form-control text-center" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroSerie_SelectedIndexChanged" runat="server"></asp:DropDownList>
        </div>
    </div>

    <%-- Lista Associzioni Frutti Serie --%>
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-10 text-center table-container">
            <asp:GridView ID="grdFruttiSerie" ItemType="Database.Models.FruttoSerie" OnRowCommand="grdFruttiSerie_RowCommand" CssClass="table table-dark table-striped scrollable-table" AutoGenerateColumns="false" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Serie" DataField="NomeSerie" />
                    <asp:BoundField HeaderText="Frutto" DataField="NomeFrutto" />
                    <asp:BoundField HeaderText="Codice Listino" DataField="CodiceListino" />
                    <asp:BoundField HeaderText="Descrizione Listino" DataField="DescrizioneListino" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnModifica" CommandName="Modifica" CommandArgument="<%# BindItem.IdFruttoSerie %>" runat="server">
                                <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElimina" CommandName="Elimina" CommandArgument="<%# BindItem.IdFruttoSerie %>" runat="server" OnClientClick="return confirm('Vuoi veramente eliminare questa associazione frutto-serie?');">
                                <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

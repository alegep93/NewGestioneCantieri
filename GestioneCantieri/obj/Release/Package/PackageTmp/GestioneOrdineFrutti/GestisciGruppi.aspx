<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="GestisciGruppi.aspx.cs" Inherits="GestioneCantieri.GestisciGruppi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Gestisci Gruppi</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Gestisci Gruppi</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <asp:Label ID="lblNomeGruppo" Text="Nome Gruppo" runat="server"></asp:Label>
                    <asp:TextBox ID="txtNomeGruppo" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col text-center">
                    <asp:Label ID="lblDescrizioneGruppo" Text="Descrizione Gruppo" runat="server"></asp:Label>
                    <asp:TextBox ID="txtDescrizioneGruppo" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-1 text-center">
                    <asp:Button ID="btnInserisciGruppo" CssClass="btn btn-lg btn-primary" Text="Inserisci" OnClick="btnInserisciGruppo_Click" runat="server" />
                    <asp:Button ID="btnModificaGruppo" CssClass="btn btn-lg btn-primary" Text="Modifica" OnClick="btnModificaGruppo_Click" Visible="false" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <asp:HiddenField ID="hfIdGruppo" runat="server" />

    <div class="row mt-5 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
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
        <div class="col-6 text-center">
            <div class="col text-center">
                <asp:Label ID="lblScegliGruppo" Text="Scegli gruppo" runat="server"></asp:Label>
                <asp:DropDownList ID="ddlScegliGruppo" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
            <div class="col text-center">
                <asp:Button ID="btnClonaGruppo" CssClass="btn btn-dark" OnClick="btnClonaGruppo_Click" Text="Clona Gruppo" runat="server" />
            </div>
        </div>
    </div>

    <div class="row d-flex justify-content-center align-items-center">
        <div class="col text-center table-container">
            <asp:GridView ID="grdGruppi" ItemType="GestioneCantieri.Data.GruppiFrutti" OnRowCommand="grdGruppi_RowCommand" CssClass="table table-dark table-striped scrollable-table" AutoGenerateColumns="false" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="Nome Gruppo" DataField="NomeGruppo" />
                    <asp:BoundField HeaderText="Descrizione Gruppo" DataField="Descrizione" />
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
                            <asp:LinkButton ID="btnElimina" CommandName="Elimina" CommandArgument="<%# BindItem.Id %>" runat="server" OnClientClick="return confirm('Vuoi veramente eliminare questo gruppo?');">
                                    <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

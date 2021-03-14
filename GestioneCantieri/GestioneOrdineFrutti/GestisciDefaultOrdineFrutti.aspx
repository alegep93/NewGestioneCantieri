<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="GestisciDefaultOrdineFrutti.aspx.cs" Inherits="GestioneCantieri.GestioneOrdineFrutti.GestisciDefaultOrdineFrutti" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Default Ordine Frutti</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Default Ordine Frutti</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-4 text-center form-group">
            <asp:Label ID="lblScegliLocale" Text="Scegli Locale" runat="server"></asp:Label>
            <asp:DropDownList ID="ddlScegliLocale" CssClass="form-control" OnSelectedIndexChanged="ddlScegliLocale_SelectedIndexChanged" AutoPostBack="true" runat="server"></asp:DropDownList>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlScegliGruppo" CssClass="col" runat="server">

            <div class="row d-flex justify-content-center align-items-center">
                <div class="col-2 text-center" runat="server">
                    <asp:Label ID="lblScegliSerie" runat="server" Text="Scegli Serie"></asp:Label>
                    <asp:DropDownList ID="ddlScegliSerie" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-6 text-center">
                    <div class="row d-flex justify-content-center align-items-center">
                        <!-- Filtri sui nomi dei gruppi presenti su DB -->
                        <div class="col text-center">
                            <asp:Label ID="lblFiltro1" runat="server" Text="Filtro 1"></asp:Label>
                            <asp:TextBox ID="txtFiltroGruppo1" placeholder="Filtro 1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col text-center">
                            <asp:Label ID="lblFiltro2" runat="server" Text="Filtro 2"></asp:Label>
                            <asp:TextBox ID="txtFiltroGruppo2" placeholder="Filtro 2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col text-center">
                            <asp:Label ID="lblFiltro3" runat="server" Text="Filtro 3"></asp:Label>
                            <asp:TextBox ID="txtFiltroGruppo3" placeholder="Filtro 3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col text-center">
                            <asp:Button ID="btnFiltroGruppi" CssClass="btn btn-primary btn-lg pull-right" OnClick="btnFiltroGruppi_Click" runat="server" Text="Filtra Gruppi" />
                        </div>
                    </div>
                </div>

                <div class="col-6">
                    <div class="row d-flex justify-content-center align-items-center">
                        <!-- Filtri sui nomi dei gruppi presenti su DB -->
                        <div class="col text-center">
                            <asp:Label ID="lblFiltroFrutto1" runat="server" Text="Filtro Frutto 1"></asp:Label>
                            <asp:TextBox ID="txtFiltroFrutto1" placeholder="Filtro Frutto 1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col text-center">
                            <asp:Label ID="lblFiltroFrutto2" runat="server" Text="Filtro Frutto 2"></asp:Label>
                            <asp:TextBox ID="txtFiltroFrutto2" placeholder="Filtro Frutto 2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col text-center">
                            <asp:Label ID="lblFiltroFrutto3" runat="server" Text="Filtro Frutto 3"></asp:Label>
                            <asp:TextBox ID="txtFiltroFrutto3" placeholder="Filtro Frutto 3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col text-center">
                            <asp:Button ID="btnFiltraFrutti" CssClass="btn btn-primary btn-lg pull-right" OnClick="btnFiltraFrutti_Click" runat="server" Text="Filtra Frutti" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row d-flex justify-content-center">
                <!-- Lista dei gruppi (filtrati o non) -->
                <div class="col-6 text-center">
                    <asp:Label ID="lblScegliGruppo" runat="server" Text="Scegli Gruppo"></asp:Label>
                    <asp:DropDownList ID="ddlScegliGruppo" CssClass="form-control" AutoPostBack="true" OnTextChanged="ddlScegliGruppo_TextChanged" runat="server"></asp:DropDownList>
                    <asp:Button ID="btnInserisciGruppo" CssClass="btn btn-primary btn-lg pull-right" OnClick="btnInserisciGruppo_Click" runat="server" Text="Inserisci Gruppo" /><br />
                    <asp:Label ID="lblIsGruppoInserito" CssClass="pull-right" runat="server" Text=""></asp:Label>
                </div>

                <!-- Lista e Qta dei frutti da inserire -->
                <div class="col-6 text-center">
                    <asp:Label ID="lblScegliFrutto" runat="server" Text="Scegli Frutto"></asp:Label>
                    <asp:DropDownList ID="ddlScegliFrutto" CssClass="form-control" AutoPostBack="true" OnTextChanged="ddlScegliFrutto_TextChanged" runat="server"></asp:DropDownList>
                    <asp:Label ID="lblQtaFrutto" runat="server" Text="Quantità Frutto"></asp:Label>
                    <asp:TextBox ID="txtQtaFrutto" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:Button ID="btnInserisciFrutto" CssClass="btn btn-primary btn-lg pull-right" OnClick="btnInserisciFrutto_Click" runat="server" Text="Inserisci Frutto" /><br />
                    <asp:Label ID="lblIsFruttoInserito" CssClass="pull-right" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </asp:Panel>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-10 text-center table-responsive">
            <asp:GridView ID="grdDefMatOrdFrut" runat="server" ItemType="GestioneCantieri.Data.DefaultMatOrdFrut" OnRowCommand="grdDefMatOrdFrut_RowCommand" 
                OnRowDataBound="grdDefMatOrdFrut_RowDataBound" CssClass="table table-dark table-striped text-center scrollable-table" AutoGenerateColumns="False">
                <Columns>
                    <%--<asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="lblProgressivo" runat="server"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>--%>
                    <asp:BoundField DataField="NomeGruppo" HeaderText="Nome Gruppo" />
                    <asp:BoundField DataField="Descrizione" HeaderText="Descrizione Gruppo" />
                    <asp:BoundField DataField="NomeFrutto" HeaderText="Nome Frutto" />
                    <asp:BoundField DataField="QtaFrutti" HeaderText="Quantità Frutti" />
                    <asp:BoundField DataField="NomeSerie" HeaderText="Nome Serie" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElimimina" CommandName="Elimina" CommandArgument="<%# Item.IdTblDefaultMatOrdFrut %>"
                                CssClass="btn btn-lg" runat="server" OnClientClick="return confirm('Vuoi veramente eliminare questo elemento?');">
                                <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="GestisciGruppiFrutti.aspx.cs" Inherits="GestioneCantieri.DistintaBase" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Gestisci Gruppi Frutti</title>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3">
        <div class="col-md-12 text-center btnChoosePanelContainer">
            <asp:Button ID="btnApriInserisci" OnClick="btnApriInserisci_Click" CssClass="btn btn-dark btn-lg" runat="server" Text="Inserisci" />
            <asp:Button ID="btnApriModifica" OnClick="btnApriModifica_Click" CssClass="btn btn-dark btn-lg" runat="server" Text="Modifica" />
            <asp:Button ID="btnApriElimina" OnClick="btnApriElimina_Click" CssClass="btn btn-dark btn-lg" runat="server" Text="Elimina" />
        </div>
    </div>

    <div class="row mt-3">
        <h1>
            <asp:Label ID="lblTitle" runat="server" Text=""></asp:Label>
        </h1>
    </div>

    <asp:Panel ID="pnlInserisci" runat="server">
        <div class="row mt-3 d-flex justify-content-center">
            <div class="col-3">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Crea Gruppo</h3>
                    </div>
                    <div class="panel-body">
                        <asp:Label ID="lblNomeGruppo" runat="server" Text="Nome Gruppo"></asp:Label>
                        <asp:TextBox ID="txtNomeGruppo" CssClass="form-control" runat="server"></asp:TextBox>

                        <asp:Label ID="lblInsDescrGruppo" runat="server" Text="Descrizione Gruppo"></asp:Label>
                        <asp:TextBox ID="txaDescr" TextMode="MultiLine" CssClass="form-control" runat="server" Rows="8"></asp:TextBox>

                        <asp:Button ID="btnCreaGruppo" OnClick="btnCreaGruppo_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Crea Gruppo" /><br />
                        <asp:Label ID="lblInserimento" runat="server" Text="" CssClass="pull-right"></asp:Label>
                    </div>
                </div>
            </div>

            <!-- Mostra Gruppi Inseriti -->
            <div class="col-3">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Mostra gruppi inseriti</h3>
                    </div>
                    <div class="panel-body">
                        <ul class="list-group">
                            <% foreach (var item in gruppiList)
                                {%>
                            <li class="list-group-item list-group-item-dark"><%= item.NomeGruppo %></li>
                            <%} %>
                        </ul>
                    </div>
                </div>
            </div>

            <!-- Inserimento frutti in Gruppo -->
            <div class="col-3">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Aggiungi Frutto a Gruppo</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <%-- Filtri --%>
                            <div class="col-4">
                                <asp:TextBox ID="txtFiltroGruppi1" placeholder="Filtro 1" OnTextChanged="txtFiltroGruppi1_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-4">
                                <asp:TextBox ID="txtFiltroGruppi2" placeholder="Filtro 2" OnTextChanged="txtFiltroGruppi2_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-4">
                                <asp:TextBox ID="txtFiltroGruppi3" placeholder="Filtro 3" OnTextChanged="txtFiltroGruppi3_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>

                        <div class="row">
                            <div class="col">
                                <asp:Label ID="lblElencoGruppi" runat="server" Text="Gruppi"></asp:Label>
                                <asp:DropDownList ID="ddlGruppi" CssClass="form-control" runat="server" OnTextChanged="ddlGruppi_TextChanged" AutoPostBack="true"></asp:DropDownList>

                                <asp:Label ID="lblDescrGruppo" runat="server" Text="Descrizione Gruppo"></asp:Label>
                                <asp:TextBox ID="txaShowDescrGruppo" TextMode="MultiLine" ReadOnly="true" CssClass="form-control" runat="server" Rows="5"></asp:TextBox>

                                <asp:Panel ID="nuovoFruttoPanel" runat="server">
                                    <asp:Label ID="lblElencoFrutti" runat="server" Text="Frutti"></asp:Label>
                                    <asp:DropDownList ID="ddlFrutti" AutoPostBack="true" OnTextChanged="ddlFrutti_TextChanged" CssClass="form-control" runat="server"></asp:DropDownList>

                                    <asp:Label ID="lblQuantita" runat="server" Text="Quantità"></asp:Label>
                                    <asp:TextBox ID="txtQta" CssClass="form-control" runat="server"></asp:TextBox>

                                    <asp:Button ID="btnInsCompgruppo" OnClick="btnInsCompgruppo_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Aggiungi Frutto" />
                                    <asp:Button ID="btnCompletaGruppo" OnClick="btnCompletaGruppo_Click" CssClass="btn btn-primary pull-right" runat="server" Text="Completa Gruppo" /><br />
                                    <asp:Label ID="lblFruttoAggiungo" runat="server" Text="" CssClass="pull-right"></asp:Label>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Mostra contenuto Gruppo -->
            <div class="col-md-3">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Mostra contenuto gruppo selezionato</h3>
                    </div>
                    <div class="panel-body">
                        <asp:Label ID="lblQtaDescr" runat="server" Text="Qta - Descrizione"></asp:Label>
                        <ul class="list-group">
                            <% foreach (var item in compList)
                                {%>
                            <li class="list-group-item list-group-item-dark"><%= item.Qta + " - " + item.NomeFrutto %></li>
                            <%} %>
                        </ul>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlModifica" runat="server">
        <div class="row mt-3 d-flex justify-content-center">
            <div class="col-3">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Modifica Gruppo</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <%-- Filtri --%>
                            <div class="col-4">
                                <asp:TextBox ID="txtFiltroMod1" placeholder="Filtro 1" OnTextChanged="txtFiltroGruppiMod1_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-4">
                                <asp:TextBox ID="txtFiltroMod2" placeholder="Filtro 2" OnTextChanged="txtFiltroGruppiMod2_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-4">
                                <asp:TextBox ID="txtFiltroMod3" placeholder="Filtro 3" OnTextChanged="txtFiltroGruppiMod3_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <asp:Label ID="lblScegliGruppo" runat="server" Text="Nome Gruppo"></asp:Label>
                                <asp:DropDownList ID="ddlModScegliGruppo" CssClass="form-control" runat="server" OnTextChanged="ddlModMostraGruppi_TextChanged" AutoPostBack="true"></asp:DropDownList>

                                <asp:Panel ID="pnlModGruppo" runat="server">
                                    <asp:Label ID="lblModNomeGruppo" runat="server" Text="Nome Gruppo"></asp:Label>
                                    <asp:TextBox ID="txtModNomeGruppo" CssClass="form-control" runat="server"></asp:TextBox>

                                    <asp:Label ID="lblModDescrGruppo" runat="server" Text="Descrizione Gruppo"></asp:Label>
                                    <asp:TextBox ID="txtModDescrGruppo" TextMode="MultiLine" Rows="8" CssClass="form-control" runat="server"></asp:TextBox>

                                    <asp:Button ID="btnSaveModGruppo" OnClick="btnSaveModGruppo_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Modifica Gruppo" />
                                    <asp:Button ID="btnRiapriGruppo" OnClick="btnRiapriGruppo_Click" CssClass="btn btn-primary pull-right" runat="server" Text="Riapri Gruppo" /><br />
                                    <asp:Label ID="lblSaveModGruppo" runat="server" Text="" CssClass="pull-right"></asp:Label>
                                </asp:Panel>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Mostra Gruppi Inseriti -->
            <div class="col-3">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Mostra gruppi inseriti</h3>
                    </div>
                    <div class="panel-body">
                        <ul class="list-group">
                            <% foreach (var item in gruppiList)
                                {%>
                            <li class="list-group-item list-group-item-dark"><%= item.NomeGruppo %></li>
                            <%} %>
                        </ul>
                    </div>
                </div>
            </div>

            <!-- Mostra contenuto Gruppo -->
            <div class="col-3">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Mostra contenuto gruppo selezionato</h3>
                    </div>
                    <div class="panel-body">
                        <asp:Label ID="Label2" runat="server" Text="Qta - Descrizione"></asp:Label>
                        <ul class="list-group">
                            <% foreach (var item in compList)
                                {%>
                            <li class="list-group-item list-group-item-dark"><%= item.Qta + " - " + item.NomeFrutto %></li>
                            <%} %>
                        </ul>
                        <asp:Button ID="btnClonaGruppo" OnClick="btnClonaGruppo_Click" CssClass="btn-lg btn-primary mt-2" runat="server" Text="Clona Gruppo Selezionato" /><br />
                        <asp:Label ID="lblClonaGruppo" runat="server"></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <asp:Panel ID="pnlElimina" runat="server">
        <div class="row mt-3 d-flex justify-content-center">
            <div class="col-3">
                <!-- Elimina Gruppo -->
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Elimina Gruppo</h3>
                    </div>
                    <div class="panel-body">
                        <div class="row">
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFiltroDel1" placeholder="Filtro 1" OnTextChanged="txtFiltroGruppi1_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFiltroDel2" placeholder="Filtro 2" OnTextChanged="txtFiltroGruppi2_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtFiltroDel3" placeholder="Filtro 3" OnTextChanged="txtFiltroGruppi3_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="row">
                            <div class="col">
                                <asp:Label ID="lblDelGruppo" runat="server" Text="Nome Gruppo"></asp:Label>
                                <asp:DropDownList ID="ddlDelGruppo" OnTextChanged="ddlDelGruppo_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:DropDownList>

                                <asp:Button ID="btnDelGruppo" OnClick="btnDelGruppo_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Elimina Gruppo" OnClientClick="return confirm('Vuoi veramente eliminare questo gruppo?');" />
                                <asp:Label ID="lblIsDelGruppo" runat="server" Text="" CssClass="pull-right"></asp:Label>
                            </div>
                        </div>
                    </div>
                </div>
            </div>

            <!-- Mostra Gruppi Inseriti -->
            <div class="col-3">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Mostra gruppi inseriti</h3>
                    </div>
                    <div class="panel-body">
                        <ul class="list-group">
                            <% foreach (var item in gruppiList)
                                {%>
                            <li class="list-group-item list-group-item-dark"><%= item.NomeGruppo %></li>
                            <%} %>
                        </ul>
                    </div>
                </div>
            </div>

            <!-- Elimina componenti Gruppo -->
            <div class="col-3">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Elimina compotente Gruppo</h3>
                    </div>
                    <div class="panel-body">
                        <asp:Label ID="lblDelNomeGruppo" runat="server" Text="Gruppi"></asp:Label>
                        <asp:DropDownList ID="ddlDelNomeGruppo" CssClass="form-control" runat="server" OnTextChanged="ddlDelNomeGruppo_TextChanged" AutoPostBack="true"></asp:DropDownList>

                        <asp:Label ID="lblDelDescrGruppo" runat="server" Text="Descrizione Gruppo"></asp:Label>
                        <asp:TextBox ID="txtDelDescrGruppo" TextMode="MultiLine" ReadOnly="true" CssClass="form-control" runat="server" Rows="5"></asp:TextBox>

                        <asp:Panel ID="pnlDelCompGrup" runat="server">
                            <asp:Label ID="lblDelCompGrup" runat="server" Text="Componenti Gruppo"></asp:Label>
                            <asp:DropDownList ID="ddlDelCompGrup" OnTextChanged="ddlDelCompGrup_TextChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:DropDownList>

                            <asp:Button ID="btnDelCompGruppo" OnClick="btnDelCompGruppo_Click" CssClass="btn btn-primary pull-left" runat="server" Text="Elimina Componente Gruppo" OnClientClick="return confirm('Vuoi veramente eliminare questo componente?');" /><br />
                            <asp:Label ID="lblIsDelCompGruppo" runat="server" Text="" CssClass="pull-right"></asp:Label>
                        </asp:Panel>
                    </div>
                </div>
            </div>

            <!-- Mostra contenuto Gruppo -->
            <div class="col-3">
                <div class="panel panel-default">
                    <div class="panel-heading">
                        <h3 class="panel-title">Mostra contenuto gruppo selezionato</h3>
                    </div>
                    <div class="panel-body">
                        <asp:Label ID="lblDelMostraCompGruppo" runat="server" Text="Qta - Descrizione"></asp:Label>
                        <ul class="list-group">
                            <% foreach (var item in compList)
                                { %>
                            <li class="list-group-item list-group-item-dark"><%= item.Qta + " - " + item.NomeFrutto %></li>
                            <% } %>
                        </ul>
                    </div>
                </div>
            </div>
            <!-- Fine Mostra contenuto Gruppo -->
        </div>
    </asp:Panel>
</asp:Content>

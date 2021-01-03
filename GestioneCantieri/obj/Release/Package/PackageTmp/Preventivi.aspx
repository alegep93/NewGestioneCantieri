<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="Preventivi.aspx.cs" Inherits="GestioneCantieri.Preventivi" %>

<asp:Content ID="Head" ContentPlaceHolderID="title" runat="server">
    <title>Gestione Preventivi</title>
</asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Gestione Preventivi</h1>
        </div>
    </div>

    <div class="row d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlInsPreventivi" DefaultButton="btnInsPreventivo" CssClass="panel-container col" runat="server">
            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col form-group">
                    <asp:Label ID="lblNumeroPreventivo" runat="server" Text="Numero Preventivo" />
                    <asp:TextBox ID="txtNumeroPreventivo" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                </div>
                <div class="col form-group">
                    <asp:Label ID="lblScegliOperaio" runat="server" Text="Scegli Operaio" />
                    <asp:DropDownList ID="ddlScegliOperaio" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
                <div class="col form-group">
                    <asp:Label ID="lblData" runat="server" Text="Data" />
                    <asp:TextBox ID="txtData" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col form-group">
                    <asp:Label ID="lblDescrizione" runat="server" Text="Descrizione" />
                    <asp:TextBox ID="txtDescrizione" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col form-group">
                    <asp:Label ID="lblConcatenazione" runat="server" Text="Concatenazione" />
                    <asp:TextBox ID="txtConcatenazione" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                </div>
                <div class="col form-group">
                    <asp:Label ID="lblAnno" runat="server" Text="Anno" />
                    <asp:TextBox ID="txtAnno" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtAnno_TextChanged" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col text-center form-group">
                    <asp:Button ID="btnInsPreventivo" OnClick="btnInsPreventivo_Click" CssClass="btn btn-lg btn-primary pull-right" runat="server" Text="Inserisci Preventivo" />
                    <asp:Button ID="btnModPreventivo" OnClick="btnModPreventivo_Click" CssClass="btn btn-lg btn-primary pull-right" runat="server" Text="Modifica Preventivo" />
                    <asp:Label ID="lblMessaggio" CssClass="pull-right labelConferma" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </asp:Panel>
    </div>

    <asp:HiddenField ID="hidIdPrev" runat="server" />

    <div class="row d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlFiltriCant" CssClass="col-6 text-center" runat="server">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblFiltroAnno" runat="server" Text="Anno"></asp:Label>
                    <asp:TextBox ID="txtFiltroAnno" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblFiltroNumero" runat="server" Text="Numero"></asp:Label>
                    <asp:TextBox ID="txtFiltroNumero" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblFiltroDescr" runat="server" Text="Descrizione"></asp:Label>
                    <asp:TextBox ID="txtFiltroDescr" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Button ID="btnFiltraPreventivi" OnClick="btnFiltraPreventivi_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Filtra" />
                    <asp:Button ID="btnSvuotaFiltri" OnClick="btnSvuotaFiltri_Click" CssClass="btn btn-lg btn-dark" runat="server" Text="Svuota" />
                </div>
            </div>
        </asp:Panel>
    </div>

    <!-- Griglia di visualizzazione record -->
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col-8 text-center table-container">
            <asp:GridView ID="grdPreventivi" OnRowCommand="grdPreventivi_RowCommand" AutoGenerateColumns="false"
                ItemType="GestioneCantieri.Data.Preventivo" runat="server" CssClass="table table-striped table-dark text-center scrollable-table">
                <Columns>
                    <asp:BoundField HeaderText="Anno" DataField="Anno" />
                    <asp:BoundField HeaderText="Numero" DataField="Numero" />
                    <asp:BoundField HeaderText="Descrizione" DataField="Descrizione" />
                    <asp:BoundField HeaderText="Operaio" DataField="NomeOp" />
                    <asp:BoundField HeaderText="Data" DataField="Data" DataFormatString="{0:d}" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnVisualPrev" CommandName="VisualPrev" CommandArgument="<%# BindItem.Id %>" CssClass="btn btn-lg btn-default" runat="server" Text="Visualizza">
                                <i class="fas fa-eye" style="color: darkblue;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnModPrev" CommandName="ModPrev" CommandArgument="<%# BindItem.Id %>" CssClass="btn btn-lg btn-default" runat="server" Text="Modifica">
                                <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElimPrev" CommandName="ElimPrev" CommandArgument="<%# BindItem.Id %>"
                                CssClass="btn btn-lg btn-default" runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo preventivo?');">
                                <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="Preventivi.aspx.cs" Inherits="GestioneCantieri.Preventivi" %>

<asp:Content ID="Head" ContentPlaceHolderID="title" runat="server">
    <title>Inserimento Dati</title>
    <style>
        h1 {
            margin-bottom: 20px;
        }

        .btn.btn-lg {
            min-width: 100px;
        }

        .panel-container {
            margin-top: 20px;
        }

        .labelConferma {
            position: relative;
            top: 14px;
            right: 10px;
        }

        span.form-control {
            border: none;
            background-color: transparent;
            box-shadow: none;
            -webkit-box-shadow: none;
        }

        input[type="checkbox"] {
            width: 20px;
            height: 20px;
            position: relative;
            left: -10px;
        }

        .table-container {
            max-height: 450px;
            overflow: scroll;
            overflow-y: auto;
        }
    </style>
</asp:Content>
<asp:Content ID="Body" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <h1>Gestione Preventivi</h1>
        </div>
        <div class="row">
            <asp:Panel ID="pnlInsPreventivi" DefaultButton="btnInsPreventivo" CssClass="panel-container col-md-12" runat="server" Style="margin-top: 20px;">
                <div class="col-md-2 form-group">
                    <asp:Label ID="lblNumeroPreventivo" runat="server" Text="Numero Preventivo" />
                    <asp:TextBox ID="txtNumeroPreventivo" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2 form-group">
                    <asp:Label ID="lblScegliOperaio" runat="server" Text="Scegli Operaio" />
                    <asp:DropDownList ID="ddlScegliOperaio" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
                <div class="col-md-2 form-group">
                    <asp:Label ID="lblData" runat="server" Text="Data" />
                    <asp:TextBox ID="txtData" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-3 form-group">
                    <asp:Label ID="lblDescrizione" runat="server" Text="Descrizione" />
                    <asp:TextBox ID="txtDescrizione" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-3 form-group">
                    <asp:Label ID="lblConcatenazione" runat="server" Text="Concatenazione" />
                    <asp:TextBox ID="txtConcatenazione" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                </div>

                <div class="col-md-12 form-group">
                    <asp:Button ID="btnInsPreventivo" OnClick="btnInsPreventivo_Click" CssClass="btn btn-lg btn-primary pull-right" runat="server" Text="Inserisci Preventivo" />
                    <asp:Button ID="btnModPreventivo" OnClick="btnModPreventivo_Click" CssClass="btn btn-lg btn-primary pull-right" runat="server" Text="Modifica Preventivo" />
                    <asp:Label ID="lblMessaggio" CssClass="pull-right labelConferma" runat="server" Text=""></asp:Label>
                </div>
            </asp:Panel>
        </div>

        <asp:HiddenField ID="hidIdPrev" runat="server" />

        <asp:Panel ID="pnlFiltriCant" CssClass="col-md-12" runat="server" Style="margin-top: 20px;">
            <div class="col-md-2">
                <asp:Label ID="lblFiltroAnno" runat="server" Text="Anno"></asp:Label>
                <asp:TextBox ID="txtFiltroAnno" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <asp:Label ID="lblFiltroNumero" runat="server" Text="Numero"></asp:Label>
                <asp:TextBox ID="txtFiltroNumero" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col-md-2">
                <asp:Label ID="lblFiltroDescr" runat="server" Text="Descrizione"></asp:Label>
                <asp:TextBox ID="txtFiltroDescr" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
<%--            <div class="col-md-2">
                <asp:Label ID="lblFiltroData" runat="server" Text="Data"></asp:Label>
                <asp:TextBox ID="txtFiltroData" CssClass="form-control" runat="server"></asp:TextBox>
            </div>--%>
            <div class="col-md-2">
                <asp:Button ID="btnFiltraPreventivi" OnClick="btnFiltraPreventivi_Click" CssClass="btn btn-lg btn-primary pull-right" runat="server" Text="Filtra" />
                <asp:Button ID="btnSvuotaFiltri" OnClick="btnSvuotaFiltri_Click" CssClass="btn btn-default pull-right" runat="server" Text="Svuota" Style="margin-right: 5px;" />
            </div>
        </asp:Panel>

        <!-- Griglia di visualizzazione record -->
        <div class="col-md-12 table-container">
            <asp:GridView ID="grdPreventivi" OnRowCommand="grdPreventivi_RowCommand" AutoGenerateColumns="false"
                ItemType="GestioneCantieri.Data.Preventivo" runat="server" CssClass="table table-striped table-responsive text-center">
                <Columns>
                    <asp:BoundField HeaderText="Anno" DataField="Anno" />
                    <asp:BoundField HeaderText="Numero" DataField="Numero" />
                    <asp:BoundField HeaderText="Descrizione" DataField="Descrizione" />
                    <asp:BoundField HeaderText="Operaio" DataField="NomeOp" />
                    <asp:BoundField HeaderText="Data" DataField="Data" DataFormatString="{0:d}" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnVisualPrev" CommandName="VisualPrev" CommandArgument="<%# BindItem.Id %>" CssClass="btn btn-lg btn-default" runat="server" Text="Visualizza" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnModPrev" CommandName="ModPrev" CommandArgument="<%# BindItem.Id %>" CssClass="btn btn-lg btn-default" runat="server" Text="Modifica" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnElimPrev" CommandName="ElimPrev" CommandArgument="<%# BindItem.Id %>"
                                CssClass="btn btn-lg btn-default" runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo preventivo?');" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

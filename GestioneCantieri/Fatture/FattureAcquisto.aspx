<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="FattureAcquisto.aspx.cs" Inherits="GestioneCantieri.FattureAcquisto" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Fatture Acquisto</title>
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

        .grd-checkbox input[type="checkbox"] {
            position: static !important;
        }

        .table-container {
            max-height: 550px;
            overflow: scroll;
            overflow-y: auto;
        }
    </style>
    <link href="../Css/Fatture.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 text-center">
                <h1>Fatture Acquisto</h1>
            </div>
        </div>

        <div class="row" style="margin-bottom: 30px;">
            <div class="col-md-offset-3 col-md-6 text-center">
                <div class="row">
                    <div class="col-md-6 text-right">
                        <asp:Button ID="btnInserisciFatture" CssClass="btn btn-lg btn-default" Text="Inserisci" OnClick="btnInserisciFatture_Click" runat="server" />
                    </div>
                    <div class="col-md-6 text-left">
                        <asp:Button ID="btnRicercaFatture" CssClass="btn btn-lg btn-default" Text="Ricerca" OnClick="btnRicercaFatture_Click" runat="server" />
                    </div>
                </div>
            </div>
        </div>

        <asp:Panel ID="pnlInsFatture" DefaultButton="btnInsFattura" CssClass="panel-container" runat="server" Style="margin-top: 20px;">
            <div class="row">
                <div class="col-md-offset-4 col-md-2 form-group">
                    <asp:Label ID="lblNumeroFattura" runat="server" Text="Numero Fattura" />
                    <asp:TextBox ID="txtNumeroFattura" CssClass="form-control" ReadOnly="true" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2 form-group">
                    <asp:Label ID="lblData" runat="server" Text="Data" />
                    <asp:TextBox ID="txtData" TextMode="Date" AutoPostBack="true" OnTextChanged="txtData_TextChanged" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-1 form-group">
                    <asp:Label ID="lblNotaCredito" runat="server" Text="Nota di credito" />
                    <asp:CheckBox ID="chkNotaCredito" CssClass="form-control" runat="server"></asp:CheckBox>
                </div>
            </div>

            <div class="row">
                <div class="col-md-offset-5 col-md-4">
                    <div class="row">
                        <div class="col-md-4">
                            <asp:TextBox ID="txtFiltroFornitore" CssClass="form-control" placeholder="Filtro Ragione Sociale Fornitore" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-md-4">
                            <asp:Button ID="btnFiltraFornitore" CssClass="btn btn-primary" OnClick="btnFiltraFornitore_Click" Text="Filtra Fornitori" runat="server"></asp:Button>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row">
                <div class="col-md-offset-4 col-md-4 form-group">
                    <asp:Label ID="lblScegliFornitore" runat="server" Text="Scegli Fornitore" />
                    <asp:DropDownList ID="ddlScegliFornitore" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
            </div>

            <div class="row">
                <div class="col-md-offset-3 col-md-1 form-group">
                    <asp:Label ID="lblImponibile" runat="server" Text="Imponibile" />
                    <asp:TextBox ID="txtImponibile" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-1 form-group">
                    <asp:Label ID="lblRitenutaAcconto" runat="server" Text="Ritenuta d'acconto" />
                    <asp:TextBox ID="txtRitenutaAcconto" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-1 form-group">
                    <asp:Label ID="lblIva" runat="server" Text="Iva" />
                    <asp:TextBox ID="txtIva" CssClass="form-control" runat="server">22</asp:TextBox>
                </div>
                <div class="col-md-1 form-group">
                    <asp:Label ID="lblReverseCharge" runat="server" Text="Reverse charge" />
                    <asp:CheckBox ID="chkReverseCharge" CssClass="form-control" runat="server"></asp:CheckBox>
                </div>
                <div class="col-md-2 form-group">
                    <asp:Label ID="lblConcatenazione" runat="server" Text="Concatenazione" />
                    <asp:TextBox ID="txtConcatenazione" ReadOnly="true" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 text-center form-group">
                    <asp:FileUpload ID="fuUploadFattura" style="opacity: 0;" runat="server" />
                    <label for="body_fuUploadFattura" style="cursor: pointer;">
                        <i class="fas fa-upload" style="font-size: xx-large; color: #333;"></i>
                    </label>
                </div>
            </div>

            <div class="row">
                <div class="col-md-12 text-center form-group">
                    <asp:Button ID="btnInsFattura" OnClick="btnInsFattura_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Inserisci Fattura Acquisto" />
                    <asp:Button ID="btnModFattura" OnClick="btnModFattura_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Modifica Fattura Acquisto" /><br />
                    <asp:Label ID="lblMessaggio" runat="server" Text=""></asp:Label>
                </div>
            </div>
        </asp:Panel>

        <asp:HiddenField ID="hfIdFattura" runat="server" />

        <asp:Panel ID="pnlRicercaFatture" runat="server" Visible="false" Style="margin-top: 20px;">
            <div class="row">
                <div class="col-md-2">
                    <asp:Label ID="lblFiltroGrdAnno" runat="server" Text="Anno"></asp:Label>
                    <asp:TextBox ID="txtFiltroGrdAnno" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblFiltroGrdFornitore" runat="server" Text="Fornitore"></asp:Label>
                    <asp:TextBox ID="txtFiltroGrdFornitore" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblFiltroGrdDataDa" runat="server" Text="Data Da"></asp:Label>
                    <asp:TextBox ID="txtFiltroGrdDataDa" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblFiltroGrdDataA" runat="server" Text="Data A"></asp:Label>
                    <asp:TextBox ID="txtFiltroGrdDataA" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Label ID="lblFiltroGrdNumeroFattura" runat="server" Text="Numero Fattura"></asp:Label>
                    <asp:TextBox ID="txtFiltroGrdNumeroFattura" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-md-2">
                    <asp:Button ID="btnFiltraGrdFatture" OnClick="btnFiltraFatture_Click" CssClass="btn btn-lg btn-primary pull-right" runat="server" Text="Filtra" />
                    <asp:Button ID="btnSvuotaGrdFiltri" OnClick="btnSvuotaFiltri_Click" CssClass="btn btn-default pull-right" runat="server" Text="Svuota" Style="margin-right: 5px;" />
                </div>
            </div>

            <div class="row">
                <!-- Griglia di visualizzazione record -->
                <div class="col-md-12 table-container">
                    <asp:GridView ID="grdFatture" OnRowCommand="grdFatture_RowCommand" AutoGenerateColumns="false"
                        ItemType="GestioneCantieri.Data.FatturaAcquisto" OnRowDataBound="grdFatture_RowDataBound" runat="server" CssClass="table table-striped table-responsive text-center">
                        <Columns>
                            <asp:BoundField HeaderText="Numero" DataField="Numero" />
                            <asp:BoundField HeaderText="Data" DataField="Data" DataFormatString="{0:d}" />
                            <asp:BoundField HeaderText="Cliente" DataField="RagioneSocialeFornitore" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblImponibile" Text='<%# Item.Imponibile + " €" %>' runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:BoundField HeaderText="Iva %" DataField="Iva" />
                            <asp:BoundField HeaderText="Ritenuta d'acconto %" DataField="RitenutaAcconto" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblValoreIva" runat="server" />
                                    €
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblValoreRitenutaAcconto" runat="server" />
                                    €
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Label ID="lblImportoFattura" runat="server" />
                                    €
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HiddenField ID="hfRowIdFattura" Value="<%# Item.IdFattureAcquisto %>" runat="server" />
                                    <asp:CheckBox ID="chkReverseCharge" CssClass="grd-checkbox" Checked="<%# Item.ReverseCharge %>" Enabled="false" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:CheckBox ID="chkNotaCredito" CssClass="grd-checkbox" Checked="<%# Item.IsNotaDiCredito %>" Enabled="false" runat="server" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:HyperLink ID="hlOpenPDF" NavigateUrl='<%# "~/" + Item.FilePath %>' runat="server">
                                        <i class="fas fa-file-pdf" style="color: darkblue;"></i>
                                    </asp:HyperLink>
                                    <%--<asp:LinkButton ID="btnVisualizzaPDF" CommandName="VisualizzaPDF" CommandArgument="<%# BindItem.IdFattureAcquisto %>" runat="server">
                                        <i class="fas fa-file-pdf" style="color: darkblue;"></i>
                                    </asp:LinkButton>--%>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnVisualizza" CommandName="Visualizza" CommandArgument="<%# BindItem.IdFattureAcquisto %>" runat="server">
                                        <i class="fas fa-eye" style="color: darkblue;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnModifica" CommandName="Modifica" CommandArgument="<%# BindItem.IdFattureAcquisto %>" runat="server">
                                        <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnElimina" CommandName="Elimina" CommandArgument="<%# BindItem.IdFattureAcquisto %>" runat="server" OnClientClick="return confirm('Vuoi veramente eliminare questo fattura?');">
                                        <i class="fas fa-times" style="color: red;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>

            <%-- Griglie Totali Per Quarter --%>
            <asp:GridView ID="grdTotaleIvaPerQuarter" AutoGenerateColumns="false" runat="server" CssClass="table table-striped table-responsive quarter-total-table iva-quarter-total-table text-center">
                <Columns>
                    <asp:BoundField HeaderText="Trimestre" DataField="Trimestre" />
                    <asp:BoundField HeaderText="Totale Iva" DataField="TotaleIva" DataFormatString="€ {0:###,###.##}" />
                </Columns>
            </asp:GridView>

            <asp:GridView ID="grdTotaleImponibilePerQuarter" AutoGenerateColumns="false" runat="server" CssClass="table table-striped table-responsive quarter-total-table imponibile-quarter-total-table text-center">
                <Columns>
                    <asp:BoundField HeaderText="Trimestre" DataField="Trimestre" />
                    <asp:BoundField HeaderText="Totale Imponibile" DataField="TotaleIva" DataFormatString="€ {0:###,###.##}" />
                </Columns>
            </asp:GridView>

            <asp:GridView ID="grdTotaleImportoPerQuarter" AutoGenerateColumns="false" runat="server" CssClass="table table-striped table-responsive quarter-total-table importo-quarter-total-table text-center">
                <Columns>
                    <asp:BoundField HeaderText="Trimestre" DataField="Trimestre" />
                    <asp:BoundField HeaderText="Totale Importo" DataField="TotaleIva" DataFormatString="€ {0:###,###.##}" />
                </Columns>
            </asp:GridView>

            <%-- Griglia Totali Filtrati --%>
            <asp:GridView ID="grdTotali" AutoGenerateColumns="false" runat="server" CssClass="table table-striped table-responsive total-table text-center">
                <Columns>
                    <asp:BoundField HeaderText="Titolo" DataField="Titolo" />
                    <asp:BoundField HeaderText="Valore" DataField="Valore" DataFormatString="€ {0:###,###.##}" />
                </Columns>
            </asp:GridView>
        </asp:Panel>
    </div>
</asp:Content>

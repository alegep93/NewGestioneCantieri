<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="ListinoHTS.aspx.cs" Inherits="GestioneCantieri.ListinoHTS" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Listino HTS</title>
    <script type="text/javascript">
        $(document).ready(function () {
            $("#body_spinnerImg").hide();
        });

        function ShowHideLoader() {
            $("#body_spinnerImg").show();
        }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="container-fluid">
        <div class="row">
            <div class="col-md-12 text-center">
                <h1>Listino HTS</h1>
            </div>
        </div>
        <div class="row">
            <div class="col-md-12 text-center">
                <asp:Button ID="btnImportaListinoHts" CssClass="btn btn-info btn-lg" OnClick="btnImportaListinoHts_Click" Text="Importa Listino HTS" runat="server" /><br />
                <asp:Label ID="lblImportMsg" runat="server"></asp:Label>
            </div>
        </div>
        <div class="row">
            <asp:Panel ID="pnlListinoSearch" DefaultButton="btnSearch" runat="server">
                <div id="filterContainer">
                    <!-- Ricerca Per Codice, CodiceProdotto e Descrizione -->
                    <div class="searchFilterContainer col-md-4 col-md-offset-3">
                        <asp:Label ID="Label1" runat="server" Text="Codice, Codice Articolo & Descrizione"></asp:Label>
                        <div class="col-md-12">
                            <asp:Label ID="lblCercaCodice" runat="server" Text="Cerca per codice"></asp:Label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCodice1" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCodice2" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCodice3" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <asp:Label ID="lblCercaCodProd" runat="server" Text="Cerca per codice prodotto"></asp:Label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCodProd1" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCodProd2" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtCodProd3" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-12">
                            <asp:Label ID="lblCercaDescriCodProd" runat="server" Text="Cerca per Descrizione"></asp:Label>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDescriCodProd1" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDescriCodProd2" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                            <div class="col-md-4">
                                <asp:TextBox ID="txtDescriCodProd3" CssClass="form-control" runat="server"></asp:TextBox>
                            </div>
                        </div>
                    </div>

                    <!-- Cerca e Svuota -->
                    <div class="searchFilterContainer col-md-2 text-center">
                        <div class="col-md-12 text-center">
                            <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary btn-lg" Text="Cerca" />
                        </div>
                        <div class="col-md-12 text-center">
                            <asp:Button ID="btnSvuotaTxt" runat="server" OnClick="btnSvuotaTxt_Click" Text="Svuota Caselle di Testo" CssClass="btn btn-default btn-lg" />
                        </div>
                        <div class="col-md-12 text-center">
                            <asp:Button ID="btnEliminaListino" runat="server" OnClick="btnEliminaListino_Click" Text="ELIMINA LISTINO" CssClass="btn btn-danger btn-lg" OnClientClick="return confirm('Vuoi veramente eliminare TUTTO il listino?');" />
                        </div>
                        <div class="col-md-12 text-center" style="margin-top: 20px;">
                            <asp:Label ID="lblPercentualeSconto" runat="server">Percentuale Sconto</asp:Label>
                            <asp:TextBox ID="txtPercentualeSconto" AutoPostBack="true" OnTextChanged="txtPercentualeSconto_TextChanged" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </asp:Panel>
        </div>
        <div class="row">
            <div class="col-md-12 text-center">
                <asp:GridView ID="grdListinoHts" runat="server" ItemType="GestioneCantieri.Data.Mamg0" OnRowDataBound="grdListinoHts_RowDataBound" AutoGenerateColumns="False" CssClass="table table-striped table-responsive">
                    <Columns>
                        <asp:BoundField DataField="Id" HeaderText="Id" />
                        <asp:BoundField DataField="Codice" HeaderText="Codice" />
                        <asp:BoundField DataField="CodiceProdotto" HeaderText="Codice Prodotto" />
                        <asp:BoundField DataField="Descrizione" HeaderText="Descrizione" />
                        <asp:BoundField DataField="Prezzo" HeaderText="Prezzo" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:Label ID="lblPrezzoScontato" runat="server"></asp:Label>
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:BoundField DataField="Cr" HeaderText="Cr" />
                        <asp:BoundField DataField="G" HeaderText="G" />
                        <asp:BoundField DataField="NoteDisponibilita" HeaderText="Note Disponibilità" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </div>
</asp:Content>

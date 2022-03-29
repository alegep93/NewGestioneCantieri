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
    <%-- Titolo Pagina --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col">
            <h1>Listino HTS</h1>
        </div>
    </div>

    <%-- Importazione/Eliminazione Listino HTS --%>
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:Button ID="btnImportaListinoHts" CssClass="btn btn-info btn-lg" OnClick="btnImportaListinoHts_Click" Text="Importa Listino HTS" runat="server" />
            <asp:Button ID="btnEliminaListino" runat="server" OnClick="btnEliminaListino_Click" Text="ELIMINA LISTINO" CssClass="btn btn-danger btn-lg" OnClientClick="return confirm('Vuoi veramente eliminare TUTTO il listino?');" />
            <br />
            <asp:Label ID="lblImportMsg" runat="server"></asp:Label>
        </div>
    </div>

    <asp:Panel ID="pnlListinoSearch" DefaultButton="btnSearch" CssClass="row mt-4 d-flex justify-content-center align-items-center" runat="server">
        <div class="col">
            <div class="row d-flex justify-content-center align-items-center">
                <%-- Codice --%>
                <div class="col-3">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col text-center">
                            <asp:Label ID="lblCercaCodice" runat="server" Text="Cerca per codice"></asp:Label>
                        </div>
                    </div>
                    <div class="row mt-3 d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:TextBox ID="txtCodice1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCodice2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCodice3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <%-- Codice Prodotto --%>
                <div class="col-3 ml-5">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col text-center">
                            <asp:Label ID="lblCercaCodProd" runat="server" Text="Cerca per codice prodotto"></asp:Label>
                        </div>
                    </div>
                    <div class="row mt-3 d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:TextBox ID="txtCodProd1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCodProd2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtCodProd3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <%-- Descrizione --%>
                <div class="col-3 ml-5">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col text-center">
                            <asp:Label ID="lblCercaDescriCodProd" runat="server" Text="Cerca per Descrizione"></asp:Label>
                        </div>
                    </div>
                    <div class="row mt-3 d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:TextBox ID="txtDescriCodProd1" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtDescriCodProd2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:TextBox ID="txtDescriCodProd3" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-2 d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary btn-lg" Text="Cerca" />
                    <asp:Button ID="btnSvuotaTxt" runat="server" OnClick="btnSvuotaTxt_Click" Text="Svuota" CssClass="btn btn-secondary btn-lg" />
                </div>
            </div>
        </div>
    </asp:Panel>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-2 text-center">
            <asp:Label ID="lblPercentualeSconto" CssClass="bold-title" runat="server">Percentuale Sconto</asp:Label><br />
            <asp:TextBox ID="txtPercentualeSconto" AutoPostBack="true" CssClass="form-control text-center" OnTextChanged="txtPercentualeSconto_TextChanged" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row mt-1 d-flex justify-content-center align-items-center">
        <div class="col text-center table-overflow listino-hts-table">
            <asp:GridView ID="grdListinoHts" runat="server" ItemType="Database.Models.Mamg0" OnRowDataBound="grdListinoHts_RowDataBound" AutoGenerateColumns="False" 
                HeaderStyle-CssClass="border-bottom-bold" CssClass="table table-dark table-striped scrollable-table">
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

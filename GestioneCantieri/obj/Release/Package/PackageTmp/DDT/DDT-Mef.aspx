<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="DDT-Mef.aspx.cs" Inherits="GestioneCantieri.Default" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Gestione DDT Mef</title>
    <link href="../Css/DdtMef.css" rel="stylesheet" />
</asp:Content>
<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <%-- Titolo --%>
    <div class="row mt-3">
        <div class="col text-center">
            <h1>DDT Mef</h1>
        </div>
    </div>

    <%-- Importazione DBF --%>
    <div class="row">
        <div class="offset-3 col-6 text-center">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col-4">
                    <asp:Label ID="lblAcquirente" Text="Acquirente" runat="server"></asp:Label>
                    <asp:TextBox ID="txtAcquirente" CssClass="form-control" runat="server" Text="Mau"></asp:TextBox>
                </div>
                <div class="col-4">
                    <asp:Label ID="lblFornitore" runat="server" Text="Fornitore"></asp:Label>
                    <asp:DropDownList ID="ddlFornitore" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
                <div class="col-4 text-left">
                    <asp:Button ID="btn_GeneraDdtDaDbf" class="btn btn-lg btn-primary" OnClick="btn_GeneraDdtDaDbf_Click" OnClientClick="javascript:ShowHideLoader()" Text="Importa DBF" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <%-- Recap valori --%>
    <div class="row mt-3">
        <div class="col">
            <div class="row d-flex justify-content-center align-items-center text-center">
                <!-- Media prezzo unitario -->
                <div class="col-3">
                    <asp:Label ID="lblMedia" runat="server" CssClass="font-weight-bold" Text="Media Prezzo Unitario"></asp:Label>
                    <asp:TextBox ID="txtMedia" CssClass="form-control text-center font-weight-bold" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <!-- Imponibile,Iva,Totale DDT -->
                <div class="col-3">
                    <asp:Label ID="lblImponibileDDT" CssClass="font-weight-bold" runat="server" Text="Imponibile DDT"></asp:Label>
                    <asp:TextBox ID="txtImponibileDDT" CssClass="form-control text-center font-weight-bold" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <div class="col-3">
                    <asp:Label ID="lblIvaDDT" CssClass="font-weight-bold" runat="server" Text="Iva DDT"></asp:Label>
                    <asp:TextBox ID="txtIvaDDT" CssClass="form-control text-center font-weight-bold" Enabled="false" runat="server"></asp:TextBox>
                </div>
                <div class="col-3">
                    <asp:Label ID="lblTotDDT" CssClass="font-weight-bold" runat="server" Text="Totale DDT"></asp:Label>
                    <asp:TextBox ID="txtTotDDT" CssClass="form-control text-center font-weight-bold" Enabled="false" runat="server"></asp:TextBox>
                </div>
            </div>
        </div>
    </div>

    <%-- Pannello di ricerca --%>
    <asp:Panel ID="pnlFiltriDDT" CssClass="row mt-3" DefaultButton="btnSearch" runat="server">
        <div class="col">
            <div class="row d-flex justify-content-center align-items-center">
                <!-- Ricerca Per Anno -->
                <div class="col">
                    <div class="row">
                        <div class="col">
                            <asp:Label ID="lblAnnoInizio" runat="server" Text="Anno Inizio"></asp:Label>
                            <asp:TextBox ID="txtAnnoInizio" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <asp:Label ID="lblAnnoFine" runat="server" Text="Anno Fine"></asp:Label>
                            <asp:TextBox ID="txtAnnoFine" runat="server" CssClass="form-control"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <!-- Ricerca Per Data -->
                <div class="col">
                    <div class="row">
                        <div class="col">
                            <asp:Label ID="lblDataInizio" runat="server" Text="Data Inizio"></asp:Label>
                            <asp:TextBox ID="txtDataInizio" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <asp:Label ID="lblDataFine" runat="server" Text="Data Fine"></asp:Label>
                            <asp:TextBox ID="txtDataFine" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <!-- Ricerca Per Quantità -->
                <div class="col">
                    <div class="row">
                        <div class="col">
                            <asp:Label ID="lblQta" runat="server" Text="Quantità"></asp:Label>
                            <asp:TextBox ID="txtQta" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <asp:Label ID="lblN_DDT" runat="server" Text="N_DDT"></asp:Label>
                            <asp:TextBox ID="txtN_DDT" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>

                <!-- Ricerca Per CodArt e DescriCodArt -->
                <div class="col-4">
                    <div class="row">
                        <div class="col">
                            <div class="row">
                                <div class="col">
                                    <asp:Label ID="lblCercaCodArt" runat="server" Text="Codice articolo"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <asp:TextBox ID="txtCodArt1" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtCodArt2" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtCodArt3" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                    <div class="row">
                        <div class="col">
                            <div class="row">
                                <div class="col">
                                    <asp:Label ID="lblCercaDescriCodArt" runat="server" Text="Descrizione Codice Articolo"></asp:Label>
                                </div>
                            </div>
                            <div class="row">
                                <div class="col">
                                    <asp:TextBox ID="txtDescriCodArt1" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtDescriCodArt2" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <asp:TextBox ID="txtDescriCodArt3" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>
                    </div>
                </div>

                <!-- Cerca e Svuota -->
                <div class="col text-center">
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" CssClass="btn btn-primary btn-lg" Text="Cerca" />
                    <asp:Button ID="btnSvuotaTxt" runat="server" OnClick="btnSvuotaTxt_Click" Text="Svuota Filtri" CssClass="btn btn-lg btn-dark ml-3" />
                </div>
            </div>
        </div>
    </asp:Panel>

    <div class="row mt-3">
        <div class="col text-center table-overflow ddt-mef-table">
            <asp:GridView ID="grdListaDDTMef" HeaderStyle-CssClass="border-bottom-bold" runat="server" ItemType="Database.Models.DDTMef" 
                AutoGenerateColumns="False" CssClass="table table-dark table-striped scrollable-table">
                <Columns>
                    <asp:BoundField DataField="Anno" HeaderText="Anno" />
                    <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                    <asp:BoundField DataField="N_ddt" HeaderText="N_DDT" />
                    <asp:BoundField DataField="CodArt" HeaderText="Codice Articolo" />
                    <asp:BoundField DataField="DescriCodArt" HeaderText="Descrizione Codice Articolo" />
                    <asp:BoundField DataField="Qta" HeaderText="Quantità" />
                    <asp:BoundField DataField="Importo" HeaderText="Importo" DataFormatString="{0:0.00} €" />
                    <asp:BoundField DataField="Acquirente" HeaderText="Acquirente" />
                    <asp:BoundField DataField="PrezzoUnitario" HeaderText="Prezzo Unitario" DataFormatString="{0:0.00} €" />
                    <asp:BoundField DataField="AnnoN_ddt" HeaderText="Anno N_DDT" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

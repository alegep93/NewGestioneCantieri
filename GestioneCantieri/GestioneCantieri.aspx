<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="GestioneCantieri.aspx.cs" Inherits="GestioneCantieri.GestioneCantieri" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Gestione Cantieri</title>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Matieriali Di Cantiere</h1>
        </div>
    </div>

    <!-- Intestazione -->
    <div class="row d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlIntestazione" CssClass="col text-center" runat="server">
            <div class="row d-flex justify-content-center align-items-center">
                <asp:Panel ID="pnlFiltriSceltaCant" CssClass="col-8 text-center" runat="server">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Label ID="lblFiltroCantAnno" Text="Anno" runat="server" />
                            <asp:TextBox ID="txtFiltroCantAnno" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroCantCodCant" Text="Cod Cant" runat="server" />
                            <asp:TextBox ID="txtFiltroCantCodCant" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroCantDescrCodCant" Text="Descri Cod Cant" runat="server" />
                            <asp:TextBox ID="txtFiltroCantDescrCodCant" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            <asp:Label ID="lblFiltroCantChiuso" Text="Chiuso" runat="server" />
                            <asp:CheckBox ID="chkFiltroCantChiuso" CssClass="form-control" Checked="false" runat="server" />
                        </div>
                        <div class="col-1">
                            <asp:Label ID="lblFiltroCantRiscosso" Text="Riscosso" runat="server" />
                            <asp:CheckBox ID="chkFiltroCantRiscosso" CssClass="form-control" Checked="false" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Button ID="btnFiltroCant" CssClass="btn btn-lg btn-primary" OnClick="btnFiltroCant_Click" runat="server" Text="Filtra" />
                            <asp:Button ID="btnSvuotaIntestazione" CssClass="btn btn-lg btn-dark" OnClick="btnSvuotaIntestazione_Click" runat="server" Text="Svuota" />
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="row d-flex justify-content-center align-items-center">
                <div class="col-4 text-center">
                    <asp:Label ID="lblScegliCant" Text="Scegli Cantiere" runat="server" />
                    <asp:DropDownList ID="ddlScegliCant" CssClass="form-control" AutoPostBack="true" OnTextChanged="ddlScegliCant_TextChanged" runat="server" /><br />
                    <asp:Label ID="lblErroreGeneraNumBolla" Text="" runat="server" />
                </div>
            </div>

            <asp:Panel ID="pnlSubIntestazione" CssClass="row d-flex justify-content-center align-items-center" runat="server">
                <div class="col text-center">
                    <div class="row d-flex align-items-center">
                        <div class="col-2">
                            <div class="row d-flex align-items-center">
                                <div class="col">
                                    <asp:Label ID="lblFiltroAnnoDDT" Text="Filtro Anno DDT" runat="server" />
                                    <asp:TextBox ID="txtFiltroAnnoDDT" AutoPostBack="true" OnTextChanged="txtFiltroAnnoDDT_TextChanged" placeholder="Filtro Anno DDT" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                                <div class="col">
                                    <asp:Label ID="lblFiltroN_DDT" Text="Filtro N_DDT" runat="server" />
                                    <asp:TextBox ID="txtFiltroN_DDT" AutoPostBack="true" OnTextChanged="txtFiltroN_DDT_TextChanged" placeholder="Filtro N_DDT" CssClass="form-control" runat="server"></asp:TextBox>
                                </div>
                            </div>
                        </div>

                        <div class="offset-2 col-4">
                            <asp:Button ID="btnNuovoProtocollo" CssClass="btn btn-dark ml-5" OnClick="btnNuovoProtocollo_Click" Text="Nuovo Protocollo" runat="server" />
                            <asp:Button ID="btnStampaProtocollo" CssClass="btn btn-dark ml-2" OnClick="btnStampaProtocollo_Click" Text="Stampa Protocollo" runat="server" /><br />
                            <asp:Label ID="lblMsg" runat="server"></asp:Label>
                        </div>
                    </div>

                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col-2 text-center">
                            <asp:Label ID="lblScegliDDTMef" Text="Scegli DDT" runat="server" />
                            <asp:DropDownList ID="ddlScegliDDTMef" AutoPostBack="true" OnTextChanged="ddlScegliDDTMef_TextChanged" CssClass="form-control" runat="server" />
                        </div>
                        <div class="col text-center">
                            <asp:Label ID="lblDataDDT" Text="Data DDT" runat="server" />
                            <asp:TextBox ID="txtDataDDT" AutoPostBack="true" OnTextChanged="txtDataDDT_TextChanged" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col text-center">
                            <asp:Label ID="lblNumBolla" Text="Numero Bolla" runat="server" />
                            <asp:TextBox ID="txtNumBolla" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col text-center d-none">
                            <asp:Label ID="lblFascia" Text="Fascia" runat="server" />
                            <asp:TextBox ID="txtFascia" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblProtocollo" Text="Protocollo" runat="server" />
                            <asp:TextBox ID="txtProtocollo" AutoPostBack="true" OnTextChanged="txtProtocollo_TextChanged" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblAcquirente" Text="Acquirente" runat="server" />
                            <asp:DropDownList ID="ddlScegliAcquirente" CssClass="form-control" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Label ID="lblScegliFornit" Text="Fornitore" runat="server" />
                            <asp:DropDownList ID="ddlScegliFornit" CssClass="form-control" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Label ID="lblTipDatCant" Text="Tipologia" runat="server" />
                            <asp:TextBox ID="txtTipDatCant" CssClass="form-control" Text="MATERIALE" Enabled="false" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </asp:Panel>

            <!-- Pulsantiera per scelta maschera -->
            <asp:Panel ID="pnlScegliMaschera" CssClass="row mt-3 d-flex justify-content-center align-items-center" runat="server">
                <div class="col text-center">
                    <asp:Button ID="btnMatCantFromDDT" runat="server" OnClick="btnMatCantFromDDT_Click" CssClass="btn btn-lg btn-dark" Text="Materiali da DDT" />
                    <asp:Button ID="btnMatCant" runat="server" OnClick="btnMatCant_Click" CssClass="btn btn-lg btn-dark" Text="Materiali Cantieri" />
                    <asp:Button ID="btnMatCantAltriFornitori" runat="server" OnClick="btnMatCantAltriFornitori_Click" CssClass="btn btn-lg btn-dark" Text="Materiali Cantieri Altri Fornitori" />
                    <asp:Button ID="btnRientro" runat="server" OnClick="btnRientro_Click" CssClass="btn btn-lg btn-dark" Text="Rientro Materiali" />
                    <asp:Button ID="btnAccrediti" runat="server" OnClick="btnAccrediti_Click" CssClass="btn btn-lg btn-dark" Text="Accrediti" />
                    <asp:Button ID="btnManodop" runat="server" OnClick="btnManodop_Click" CssClass="btn btn-lg btn-dark" Text="Manodopera" />
                    <asp:Button ID="btnGestOper" runat="server" OnClick="btnGestOper_Click" CssClass="btn btn-lg btn-dark" Text="Gest. Operaio" />
                    <asp:Button ID="btnGestArrot" runat="server" OnClick="btnGestArrot_Click" CssClass="btn btn-lg btn-dark" Text="Gest. Arrotond." />
                    <asp:Button ID="btnGestChiam" runat="server" OnClick="btnGestChiam_Click" CssClass="btn btn-lg btn-dark" Text="Gest. A Chiamata." />
                    <asp:Button ID="btnGestSpese" runat="server" OnClick="btnGestSpese_Click" CssClass="btn btn-lg btn-dark" Text="Gest. Spese" />
                </div>
            </asp:Panel>
        </asp:Panel>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h3>
                <asp:Label ID="lblTitoloMaschera" runat="server" Text=""></asp:Label>
            </h3>
        </div>
    </div>

    <!-- Maschera gestione materiali da DDT -->
    <div class="row d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlMascheraMaterialiDaDDT" CssClass="col text-center" runat="server">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col-8">
                    <asp:Button ID="btnSelezionaTuttoTOP" CssClass="btn btn-lg btn-dark" OnClick="btnSelezionaTutto_Click" Text="Seleziona Tutto" runat="server" />
                    <asp:Button ID="btnDeselezionaTuttoTOP" CssClass="btn btn-lg btn-dark" OnClick="btnDeselezionaTutto_Click" Text="Deseleziona Tutto" runat="server" />
                </div>
            </div>
            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <asp:GridView ID="grdMostraDDTDaInserire" ItemType="GestioneCantieri.Data.DDTMef" AutoGenerateColumns="false"
                    CssClass="table table-dark table-striped text-center scrollable-table" runat="server">
                    <Columns>
                        <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                        <asp:BoundField DataField="N_ddt" HeaderText="N_DDT" />
                        <asp:BoundField DataField="CodArt" HeaderText="Codice Articolo" />
                        <asp:BoundField DataField="DescriCodArt" HeaderText="Descrizione Codice Articolo" />
                        <asp:BoundField DataField="Qta" HeaderText="Quantità" />
                        <asp:BoundField DataField="PrezzoUnitario" HeaderText="Prezzo Unitario" DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="Acquirente" HeaderText="Acquirente" />
                        <asp:TemplateField>
                            <ItemTemplate>
                                <asp:CheckBox ID="chkDDTSelezionato" Checked="<%# BindItem.DaInserire %>" runat="server" />
                            </ItemTemplate>
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </div>
            <div class="row d-flex justify-content-center align-items-center">
                <asp:Button ID="btnSelezionaTuttoBOTTOM" CssClass="btn btn-lg btn-dark mr-2" OnClick="btnSelezionaTutto_Click" Text="Seleziona Tutto" runat="server" />
                <asp:Button ID="btnDeselezionaTuttoBOTTOM" CssClass="btn btn-lg btn-dark" OnClick="btnDeselezionaTutto_Click" Text="Deseleziona Tutto" runat="server" />
                <asp:Button ID="btnInsMatDaDDT" OnClick="btnInsMatDaDDT_Click" CssClass="btn btn-lg btn-primary ml-2" Text="Inserisci Materiali" runat="server" /><br />
                <asp:Label ID="lblInsMatDaDDT" CssClass="pull-right" runat="server"></asp:Label>
            </div>
        </asp:Panel>
    </div>

    <!-- Maschera gestione materiali cantieri e Rientro -->
    <div class="row d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlMascheraGestCant" CssClass="col text-center" runat="server">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col-10">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col-2">
                            <asp:Label ID="lblFiltroCod_FSS" Text="Filtro Codice Articolo" runat="server" />
                            <asp:TextBox ID="txtFiltroCodFSS" placeholder="Filtro Codice Articolo" AutoPostBack="true" OnTextChanged="txtFiltroCodFSS_TextChanged" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-2">
                            <asp:Label ID="lblFiltroAA_Des" Text="Filtro Descrizione Articolo" runat="server" />
                            <asp:TextBox ID="txtFiltroAA_Des" placeholder="Filtro Descrizione Articolo" AutoPostBack="true" OnTextChanged="txtFiltroAA_Des_TextChanged" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col-6 matCantDdl">
                            <asp:Label ID="lblScegliListino" Text="Scegli Listino" runat="server" />
                            <asp:DropDownList ID="ddlScegliListino" AutoPostBack="true" OnTextChanged="ddlScegliListino_TextChanged" CssClass="form-control" runat="server" />
                        </div>
                    </div>
                </div>

                <div class="col-10">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col-3 rientroDdl">
                            <asp:Label ID="lblFiltroMatCantCodArt" Text="Filtro Cod. Art." runat="server" />
                            <asp:TextBox ID="txtFiltroMatCantCodArt" placeholder="Filtro Cod. Art." AutoPostBack="true" OnTextChanged="txtFiltroMatCantCodArt_TextChanged" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-3 rientroDdl">
                            <asp:Label ID="lblFiltroMatCantDescriCodArt" Text="Filtro Descri. Cod. Art." runat="server" />
                            <asp:TextBox ID="txtFiltroMatCantDescriCodArt" placeholder="Filtro Descri. Cod. Art." AutoPostBack="true" OnTextChanged="txtFiltroMatCantDescriCodArt_TextChanged" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-6 rientroDdl">
                            <asp:Label ID="lblScegliMatCant" Text="Scegli Materiale Cantiere" runat="server" />
                            <asp:DropDownList ID="ddlScegliMatCant" AutoPostBack="true" OnTextChanged="ddlScegliMatCant_TextChanged" CssClass="form-control" runat="server" />
                        </div>
                    </div>
                </div>
            </div>
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblCodArt" Text="Codice Articolo" runat="server" />
                    <asp:TextBox ID="txtCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblDescriCodArt" Text="Descrizione Codice Articolo" runat="server" />
                    <asp:TextBox ID="txtDescriCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblDescrMat" Text="Descrizione Materiale" runat="server" />
                    <asp:TextBox ID="txtDescrMat" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblNote" Text="Note" runat="server" />
                    <asp:TextBox ID="txtNote" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblNote_2" Text="Note 2" runat="server" />
                    <asp:TextBox ID="txtNote_2" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblQta" Text="Quantità" runat="server" />
                    <asp:TextBox ID="txtQta" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblPzzoNettoMef" Text="Prezzo Netto Mef" runat="server" />
                    <asp:TextBox ID="txtPzzoNettoMef" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblPzzoUnit" Text="Prezzo Unitario" runat="server" />
                    <asp:TextBox ID="txtPzzoUnit" Text="0.00" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblPzzoFinCli" Text="Prezzo Finale Cliente" runat="server" />
                    <asp:TextBox ID="txtPzzoFinCli" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="col-1">
                    <asp:Label ID="lblVisibile" Text="Visibile" runat="server" />
                    <asp:CheckBox ID="chkVisibile" CssClass="form-control" Checked="true" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblRicalcolo" Text="Ricalcolo" runat="server" />
                    <asp:CheckBox ID="chkRicalcolo" CssClass="form-control" Checked="true" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblRicarico" Text="Ricarico Si/No" runat="server" />
                    <asp:CheckBox ID="chkRicarico" CssClass="form-control" Checked="true" runat="server" />
                </div>
            </div>

            <div class="row mt-2 d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Button ID="btnCalcolaPrezzoUnit" OnClick="btnCalcolaPrezzoUnit_Click" CssClass="btn btn-lg btn-primary mr-3" runat="server" Text="Calcola Prezzo Unitario" />
                    <asp:Button ID="btnInserisciMatCant" OnClick="btnInserisciMatCant_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Inserisci Mat Cant" />
                    <asp:Button ID="btnModMatCant" OnClick="btnModMatCant_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Modifica Mat Cant" />
                    <asp:Button ID="btnInserisciRientro" OnClick="btnInserisciRientro_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Inserisci Rientro" />
                    <asp:Button ID="btnModRientro" OnClick="btnModRientro_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Modifica Rientro" /><br />
                    <asp:Label ID="lblIsRecordInserito" Text="" CssClass="ml-3" runat="server" />
                </div>
            </div>

            <asp:HiddenField ID="hidIdMatCant" runat="server" />

            <div class="row d-flex justify-content-center align-items-center">
                <asp:Panel ID="pnlFiltriMatCant" CssClass="col-8 text-center" runat="server">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Label ID="lblFiltroCodArtGrdMatCant" runat="server" Text="Filtro Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroCodArtGrdMatCant" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroDescriCodArtGrdMatCant" runat="server" Text="Filtro Descri Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroDescriCodArtGrdMatCant" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroProtocolloGrdMatCant" runat="server" Text="Filtro Protocollo"></asp:Label>
                            <asp:TextBox ID="txtFiltroProtocolloGrdMatCant" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroFornitoreGrdMatCant" runat="server" Text="Filtro Fornitore"></asp:Label>
                            <asp:TextBox ID="txtFiltroFornitoreGrdMatCant" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            <asp:Button ID="btnFiltraGrdMatCant" OnClick="btnFiltraGrdMatCant_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Filtra" />
                        </div>
                        <div class="col">
                            <asp:Label ID="lblTotaleValoreMatCant_Rientro" CssClass="label-totale-valore ml-3" runat="server"></asp:Label>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="row d-flex justify-content-center align-items-center">
                <div class="col text-center table-responsive tableContainer">
                    <asp:GridView ID="grdMatCant" ItemType="GestioneCantieri.Data.MaterialiCantieri" AutoGenerateColumns="false"
                        OnRowCommand="grdMatCant_RowCommand" CssClass="table table-dark table-striped text-center scrollable-table" runat="server">
                        <Columns>
                            <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                            <asp:BoundField DataField="ProtocolloInterno" HeaderText="Protocollo" />
                            <asp:BoundField DataField="Fornitore" HeaderText="Fornitore" />
                            <asp:BoundField DataField="CodArt" HeaderText="Cod. Art" />
                            <asp:BoundField DataField="DescriCodArt" HeaderText="Descr. Cod. Art." />
                            <asp:BoundField DataField="Qta" HeaderText="Quantità" />
                            <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Prezzo Unitario" DataFormatString="{0:0.00}" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnVisualMatCant" CommandName="VisualMatCant" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Visualizza">
                                        <i class="fas fa-eye" style="color: darkblue;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnModMatCant" CommandName="ModMatCant" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Modifica">
                                        <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnElimMatCant" CommandName="ElimMatCant" CommandArgument="<%# BindItem.IdMaterialiCantiere %>"
                                        runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo record?');">
                                        <i class="fas fa-times" style="color: red;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>

                    <asp:GridView ID="grdRientro" ItemType="GestioneCantieri.Data.MaterialiCantieri" AutoGenerateColumns="false" OnRowCommand="grdRientro_RowCommand" CssClass="table table-striped text-center scrollable-table" runat="server">
                        <Columns>
                            <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                            <asp:BoundField DataField="ProtocolloInterno" HeaderText="Protocollo" />
                            <asp:BoundField DataField="Fornitore" HeaderText="Fornitore" />
                            <asp:BoundField DataField="CodArt" HeaderText="Cod. Art" />
                            <asp:BoundField DataField="DescriCodArt" HeaderText="Descr. Cod. Art." />
                            <asp:BoundField DataField="Qta" HeaderText="Quantità" />
                            <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Prezzo Unitario" DataFormatString="{0:0.00}" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnVisualRientro" CommandName="VisualRientro" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" CssClass="btn btn-lg btn-dark" runat="server" Text="Visualizza" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnModRientro" CommandName="ModRientro" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" CssClass="btn btn-lg btn-dark" runat="server" Text="Modifica" />
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:Button ID="btnElimRientro" CommandName="ElimRientro" CommandArgument="<%# BindItem.IdMaterialiCantiere %>"
                                        CssClass="btn btn-lg btn-dark" runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo record?');" />
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>

    <!-- Maschera Accrediti -->
    <div class="row d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlMascheraAccrediti" CssClass="col text-center" runat="server">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblCodArtAccrediti" Text="Codice Articolo" runat="server" />
                    <asp:TextBox ID="txtCodArtAccrediti" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtCodArtAccrediti_TextChanged" runat="server"></asp:TextBox>
                </div>
                <div class="col-2">
                    <asp:Label ID="lblDescrCodArtAccrediti" Text="Descrizione Codice Articolo" runat="server" />
                    <asp:TextBox ID="txtDescrCodArtAccrediti" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtDescrCodArtAccrediti_TextChanged" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblDescrMatAccrediti" Text="Descrizione Materiale" runat="server" />
                    <asp:TextBox ID="txtDescrMatAccrediti" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblQtaAccrediti" Text="Quantità" runat="server" />
                    <asp:TextBox ID="txtQtaAccrediti" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblPrezzoNettoAccrediti" Text="Prezzo Netto Mef" runat="server" />
                    <asp:TextBox ID="txtPrezzoNettoAccrediti" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblPrezzoUniAccrediti" Text="Prezzo Unitario" runat="server" />
                    <asp:TextBox ID="txtPrezzoUniAccrediti" Text="0.00" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblPrezzoFinAccrediti" Text="Prezzo Finale Cliente" runat="server" />
                    <asp:TextBox ID="txtPrezzoFinAccrediti" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="col-1">
                    <asp:Label ID="lblVisibileAccrediti" Text="Visibile" runat="server" />
                    <asp:CheckBox ID="chkVisibileAccrediti" CssClass="form-control" Checked="true" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblRicalcoloAccrediti" Text="Ricalcolo" runat="server" />
                    <asp:CheckBox ID="chkRicalcoloAccrediti" CssClass="form-control" Checked="true" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblRicaricoAccrediti" Text="Ricarico Si/No" runat="server" />
                    <asp:CheckBox ID="chkRicaricoAccrediti" CssClass="form-control" Checked="true" runat="server" />
                </div>
            </div>

            <div class="row d-flex justify-content-center align-items-center">
                <div class="col-6 text-center">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Label ID="lblNoteAccrediti" Text="Note" runat="server" />
                            <asp:TextBox ID="txtNoteAccrediti" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblNote2Accrediti" Text="Note 2" runat="server" />
                            <asp:TextBox ID="txtNote2Accrediti" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <asp:Button ID="btnCalcolaPrezzoAccrediti" OnClick="btnCalcolaPrezzoAccrediti_Click" CssClass="btn btn-lg btn-primary mr-3" runat="server" Text="Calcola Prezzo Unitario" />
                    <asp:Button ID="btnInserisciAccrediti" OnClick="btnInserisciAccrediti_Click" CssClass="btn btn-lg btn-primary mr-3" runat="server" Text="Inserisci Accredito" />
                    <asp:Button ID="btnModificaAccrediti" OnClick="btnModificaAccrediti_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Modifica Accredito" Visible="false" />
                    <br />
                    <asp:Label ID="lblMsgAccrediti" Text="" CssClass="pull-right" runat="server" />
                </div>
            </div>

            <asp:HiddenField ID="hfIdAccrediti" runat="server" />

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <asp:Panel ID="pnlFiltriAccrediti" CssClass="col-6 text-center" runat="server">
                    <div class="row mt-3 d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Label ID="lblFiltroCodArtAccrediti" runat="server" Text="Filtro Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroCodArtAccrediti" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroDescrAcrrediti" runat="server" Text="Filtro Descri Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroDescrAcrrediti" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroProtAccrediti" runat="server" Text="Filtro Protocollo"></asp:Label>
                            <asp:TextBox ID="txtFiltroProtAccrediti" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroFornAccrediti" runat="server" Text="Filtro Fornitore"></asp:Label>
                            <asp:TextBox ID="txtFiltroFornAccrediti" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Button ID="btnFiltraAccrediti" OnClick="btnFiltraAccrediti_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Filtra" />
                            <asp:Label ID="lblTotValAccrediti" CssClass="pull-right" runat="server"></asp:Label>
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <asp:GridView ID="grdAccrediti" ItemType="GestioneCantieri.Data.MaterialiCantieri" AutoGenerateColumns="false"
                        OnRowCommand="grdAccrediti_RowCommand" CssClass="table table-dark table-striped text-center scrollable-table" runat="server">
                        <Columns>
                            <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                            <asp:BoundField DataField="ProtocolloInterno" HeaderText="Protocollo" />
                            <asp:BoundField DataField="Fornitore" HeaderText="Fornitore" />
                            <asp:BoundField DataField="CodArt" HeaderText="Cod. Art" />
                            <asp:BoundField DataField="DescriCodArt" HeaderText="Descr. Cod. Art." />
                            <asp:BoundField DataField="Qta" HeaderText="Quantità" />
                            <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Prezzo Unitario" DataFormatString="{0:0.00}" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnVisualRientro" CommandName="VisualAccrediti" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Visualizza">
                                    <i class="fas fa-eye" style="color: darkblue;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnModRientro" CommandName="ModAccrediti" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Modifica">
                                    <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnElimRientro" CommandName="ElimAccrediti" CommandArgument="<%# BindItem.IdMaterialiCantiere %>"
                                        runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo record?');">
                                    <i class="fas fa-times" style="color: red;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>

    <!-- Maschera manodopera -->
    <div class="row d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlManodopera" CssClass="col text-center" runat="server">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblManodopQta" Text="Quantità" runat="server" />
                    <asp:TextBox ID="txtManodopQta" CssClass="form-control" AutoPostBack="false" OnTextChanged="txtManodopQta_TextChanged" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblPzzoManodop" Text="Prezzo Manodopera" runat="server" />
                    <asp:TextBox ID="txtPzzoManodop" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblDescrManodop" Text="Descrizione Manodopera" runat="server" />
                    <asp:TextBox ID="txtDescrManodop" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-1">
                    <asp:Label ID="lblManodopVisibile" Text="Visibile" runat="server" />
                    <asp:CheckBox ID="chkManodopVisibile" CssClass="form-control" Checked="true" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblManodopRicalcolo" Text="Ricalcolo" runat="server" />
                    <asp:CheckBox ID="chkManodopRicalcolo" CssClass="form-control" Enabled="false" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblManodopRicaricoSiNo" Text="Ricarico Si/No" runat="server" />
                    <asp:CheckBox ID="chkManodopRicaricoSiNo" CssClass="form-control" runat="server" />
                </div>
                <div class="col">
                    <asp:Label ID="lblNote1" Text="Note 1" runat="server" />
                    <asp:TextBox ID="txtNote1" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblNote2" Text="Note 2" runat="server" />
                    <asp:TextBox ID="txtNote2" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>

            <div class="row d-flex justify-content-center align-items-center">
                <div class="col-2">
                    <asp:TextBox ID="txtAggiornaValManodop" placeholder="Nuovo Valore Manodopera" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:Button ID="btnAggiornaValManodop" OnClick="btnAggiornaValManodop_Click" CssClass="btn btn-dark" runat="server" Text="Aggiorna Val. Manodop." />
                </div>
                <div class="col text-right">
                    <asp:Button ID="btnInsManodop" OnClick="btnInsManodop_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Inserisci Manodopera" />
                    <asp:Button ID="btnModManodop" OnClick="btnModManodop_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Modifica Manodopera" /><br />
                    <asp:Label ID="lblIsManodopInserita" Text="" runat="server" />
                </div>
            </div>

            <asp:HiddenField ID="hidManodop" runat="server" />

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <asp:Panel ID="pnlFiltriManodop" CssClass="col-8 text-center" runat="server">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Label ID="lblFiltroManodopCodArt" runat="server" Text="Filtro Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroManodopCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroManodopDescriCodArt" runat="server" Text="Filtro Descri Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroManodopDescriCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroManodopProtocollo" runat="server" Text="Filtro Protocollo"></asp:Label>
                            <asp:TextBox ID="txtFiltroManodopProtocollo" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            <asp:Button ID="btnFiltraGrdManodop" OnClick="btnFiltraGrdManodop_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Filtra" />
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="row d-flex justify-content-center align-items-center">
                <div class="col text-center table-responsive tableContainer">
                    <asp:GridView ID="grdManodop" ItemType="GestioneCantieri.Data.MaterialiCantieri" AutoGenerateColumns="false"
                        OnRowCommand="grdManodop_RowCommand" CssClass="table table-dark table-striped text-center scrollable-table" runat="server">
                        <Columns>
                            <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                            <asp:BoundField DataField="ProtocolloInterno" HeaderText="Protocollo" />
                            <asp:BoundField DataField="CodArt" HeaderText="Cod. Art" />
                            <asp:BoundField DataField="DescriCodArt" HeaderText="Descr. Cod. Art." />
                            <asp:BoundField DataField="Qta" HeaderText="Quantità" />
                            <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Prezzo Unitario" DataFormatString="{0:0.00}" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnVisualManodop" CommandName="VisualManodop" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Visualizza">
                                        <i class="fas fa-eye" style="color: darkblue;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnModManodop" CommandName="ModManodop" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Modifica">
                                        <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnElimManodop" CommandName="ElimManodop" CommandArgument="<%# BindItem.IdMaterialiCantiere %>"
                                        runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo record?');">
                                        <i class="fas fa-times" style="color: red;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>

    <!-- Maschera Gestione Operaio -->
    <div class="row d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlGestioneOperaio" CssClass="col text-center" runat="server">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblScegliOperaio" Text="Scegli Operaio" runat="server" />
                    <asp:DropDownList ID="ddlScegliOperaio" CssClass="form-control" AutoPostBack="true" OnTextChanged="ddlScegliOperaio_TextChanged" runat="server"></asp:DropDownList>
                </div>
                <div class="col">
                    <asp:Label ID="lblOperQta" Text="Quantità" runat="server" />
                    <asp:TextBox ID="txtOperQta" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtOperQta_TextChanged" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblPzzoOper" Text="Prezzo Operaio" runat="server" />
                    <asp:TextBox ID="txtPzzoOper" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblDescrOper" Text="Descrizione Operaio" runat="server" />
                    <asp:TextBox ID="txtDescrOper" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblOperNote1" Text="Note 1" runat="server" />
                    <asp:TextBox ID="txtOperNote1" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblOperNote2" Text="Note 2" runat="server" />
                    <asp:TextBox ID="txtOperNote2" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-1">
                    <asp:Label ID="lblOperVisibile" Text="Visibile" runat="server" />
                    <asp:CheckBox ID="chkOperVisibile" CssClass="form-control" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblOperRicalcolo" Text="Ricalcolo" runat="server" />
                    <asp:CheckBox ID="chkOperRicalcolo" CssClass="form-control" Enabled="false" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblOperRicaricoSiNo" Text="Ricarico Si/No" runat="server" />
                    <asp:CheckBox ID="chkOperRicaricoSiNo" CssClass="form-control" runat="server" />
                </div>
            </div>

            <div class="row d-flex justify-content-center align-items-center">
                <div class="col-2">
                    <asp:TextBox ID="txtNuovoCostoOperaio" placeholder="Nuovo Costo Operaio" CssClass="form-control" runat="server"></asp:TextBox>
                    <asp:Button ID="btnNuovoCostoOperaio" OnClick="btnNuovoCostoOperaio_Click" CssClass="btn btn-dark" runat="server" Text="Aggiorna Costo Operaio" />
                </div>
                <div class="col text-right">
                    <asp:Button ID="btnInsOper" OnClick="btnInsOper_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Inserisci Operaio" />
                    <asp:Button ID="btnModOper" OnClick="btnModOper_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Modifica Operaio" /><br />
                    <asp:Label ID="lblIsOperInserita" Text="" CssClass="pull-right" runat="server" />
                </div>
            </div>

            <asp:HiddenField ID="hidOper" runat="server" />

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <asp:Panel ID="pnlFiltriOper" CssClass="col-6 text-center" runat="server">
                    <div class="row mt-3 d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Label ID="lblFiltroOperCodArt" runat="server" Text="Filtro Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroOperCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroOperDescriCodArt" runat="server" Text="Filtro Descri Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroOperDescriCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroOperProtocollo" runat="server" Text="Filtro Protocollo"></asp:Label>
                            <asp:TextBox ID="txtFiltroOperProtocollo" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            <asp:Button ID="btnOperFiltraGrd" OnClick="btnOperFiltraGrd_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Filtra" />
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="row d-flex justify-content-center align-items-center">
                <div class="col text-center table-responsive tableContainer">
                    <asp:GridView ID="grdOperai" ItemType="GestioneCantieri.Data.MaterialiCantieri" AutoGenerateColumns="false"
                        OnRowCommand="grdOperai_RowCommand" CssClass="table table-dark table-striped text-center scrollable-table" runat="server">
                        <Columns>
                            <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                            <asp:BoundField DataField="ProtocolloInterno" HeaderText="Protocollo" />
                            <asp:BoundField DataField="CodArt" HeaderText="Cod. Art" />
                            <asp:BoundField DataField="DescriCodArt" HeaderText="Descr. Cod. Art." />
                            <asp:BoundField DataField="Qta" HeaderText="Quantità" />
                            <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Prezzo Unitario" DataFormatString="{0:0.00}" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnVisualOper" CommandName="VisualOper" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Visualizza">
                                        <i class="fas fa-eye" style="color: darkblue;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnModOper" CommandName="ModOper" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Modifica">
                                        <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnElimOper" CommandName="ElimOper" CommandArgument="<%# BindItem.IdMaterialiCantiere %>"
                                        runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo record?');">
                                        <i class="fas fa-times" style="color: red;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>

    <!-- Maschera Gestione Arrotondamenti -->
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlGestArrotond" CssClass="col text-center" runat="server">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Label ID="lblArrotCodArt" Text="Codice Articolo" runat="server" />
                            <asp:TextBox ID="txtArrotCodArt" CssClass="form-control" runat="server" Text="Arrotondamento"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblArrotDescriCodArt" Text="Descrizione Codice Articolo" runat="server" />
                            <asp:TextBox ID="txtArrotDescriCodArt" CssClass="form-control" runat="server" Text="Arrotondamento"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblArrotQta" Text="Quantità" runat="server" />
                            <asp:TextBox ID="txtArrotQta" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblArrotPzzoUnit" Text="Prezzo Unitario" runat="server" />
                            <asp:TextBox ID="txtArrotPzzoUnit" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col-1">
                            <asp:Label ID="lblArrotVisibile" Text="Visibile" runat="server" />
                            <asp:CheckBox ID="chkArrotVisibile" CssClass="form-control" runat="server" />
                        </div>
                        <div class="col-1">
                            <asp:Label ID="lblArrotRicalcolo" Text="Ricalcolo" runat="server" />
                            <asp:CheckBox ID="chkArrotRicalcolo" CssClass="form-control" Enabled="false" runat="server" />
                        </div>
                        <div class="col-1">
                            <asp:Label ID="lblArrotRicaricoSiNo" Text="Ricarico Si/No" runat="server" />
                            <asp:CheckBox ID="chkArrotRicaricoSiNo" CssClass="form-control" runat="server" />
                        </div>
                        <div class="col">
                            <asp:Button ID="btnInsArrot" OnClick="btnInsArrot_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Inserisci Arrotondamento" />
                            <asp:Button ID="btnModArrot" OnClick="btnModArrot_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Modifica Arrotondamento" /><br />
                            <asp:Label ID="lblIsArrotondInserito" Text="" CssClass="pull-right" runat="server" />
                        </div>
                    </div>
                </div>
            </div>

            <asp:HiddenField ID="hidArrot" runat="server" />

            <div class="row mt-4 d-flex justify-content-center align-items-center">
                <asp:Panel ID="pnlFiltriArrot" CssClass="col-6 text-center" runat="server">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Label ID="lblFiltroArrotCodArt" runat="server" Text="Filtro Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroArrotCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroArrotDescriCodArt" runat="server" Text="Filtro Descri Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroArrotDescriCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroArrotProtocollo" runat="server" Text="Filtro Protocollo"></asp:Label>
                            <asp:TextBox ID="txtFiltroArrotProtocollo" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Button ID="btnArrotFiltraGrd" OnClick="btnArrotFiltraGrd_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Filtra" />
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col text-center table-responsive tableContainer">
                    <asp:GridView ID="grdArrot" ItemType="GestioneCantieri.Data.MaterialiCantieri" AutoGenerateColumns="false"
                        OnRowCommand="grdArrot_RowCommand" CssClass="table table-dark table-striped text-center scrollable-table" runat="server">
                        <Columns>
                            <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                            <asp:BoundField DataField="ProtocolloInterno" HeaderText="Protocollo" />
                            <asp:BoundField DataField="CodArt" HeaderText="Cod. Art" />
                            <asp:BoundField DataField="DescriCodArt" HeaderText="Descr. Cod. Art." />
                            <asp:BoundField DataField="Qta" HeaderText="Quantità" />
                            <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Prezzo Unitario" DataFormatString="{0:0.00}" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnVisualArrot" CommandName="VisualArrot" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Visualizza">
                                        <i class="fas fa-eye" style="color: darkblue;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnModArrot" CommandName="ModArrot" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Modifica">
                                        <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnElimArrot" CommandName="ElimArrot" CommandArgument="<%# BindItem.IdMaterialiCantiere %>"
                                        runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo record?');">
                                        <i class="fas fa-times" style="color: red;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>

    <!-- Maschera Gestione A Chiamata -->
    <div class="row d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlGestChiamata" CssClass="col text-center" runat="server">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblChiamCodArt" Text="Codice Articolo" runat="server" />
                    <asp:TextBox ID="txtChiamCodArt" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtChiamCodArt_TextChanged" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblChiamDescriCodArt" Text="Descrizione Codice Articolo" runat="server" />
                    <asp:TextBox ID="txtChiamDescriCodArt" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtChiamDescriCodArt_TextChanged" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblChiamDescrMate" Text="Descrizione Materiale" runat="server" />
                    <asp:TextBox ID="txtChiamDescrMate" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblChiamNote" Text="Note" runat="server" />
                    <asp:TextBox ID="txtChiamNote" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblChiamNote2" Text="Note 2" runat="server" />
                    <asp:TextBox ID="txtChiamNote2" TextMode="MultiLine" Rows="2" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
            </div>
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblChiamQta" Text="Quantità" runat="server" />
                    <asp:TextBox ID="txtChiamQta" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblChiamPzzoNetto" Text="Prezzo Netto Mef" runat="server" />
                    <asp:TextBox ID="txtChiamPzzoNetto" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblChiamPzzoUnit" Text="Prezzo Unitario" runat="server" />
                    <asp:TextBox ID="txtChiamPzzoUnit" Text="0.00" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblChiamPzzoFinCli" Text="Prezzo Finale Cliente" runat="server" />
                    <asp:TextBox ID="txtChiamPzzoFinCli" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <div class="col-1">
                    <asp:Label ID="lblChiamVisibile" Text="Visibile" runat="server" />
                    <asp:CheckBox ID="chkChiamVisibile" CssClass="form-control" Checked="true" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblChiamRicalcolo" Text="Ricalcolo" runat="server" />
                    <asp:CheckBox ID="chkChiamRicalcolo" CssClass="form-control" Checked="false" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblChiamRicaricoSiNo" Text="Ricarico Si/No" runat="server" />
                    <asp:CheckBox ID="chkChiamRicaricoSiNo" CssClass="form-control" Checked="false" runat="server" />
                </div>
            </div>
            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col-6">
                    <asp:Button ID="btnCalcolaPzzoUnitAChiam" OnClick="btnCalcolaPzzoUnitAChiam_Click" CssClass="btn btn-lg btn-dark mr-3" runat="server" Text="Calcola Prezzo Unitario" />
                    <asp:Button ID="btnInsAChiam" OnClick="btnInsAChiam_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Inserisci A Chiamata" />
                    <asp:Button ID="btnModAChiam" OnClick="btnModAChiam_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Modifica A Chiamata" /><br />
                    <asp:Label ID="lblIsAChiamInserita" Text="" CssClass="pull-right" runat="server" />
                </div>
            </div>

            <asp:HiddenField ID="hidAChiamata" runat="server" />

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <asp:Panel ID="pnlFiltriGrdAChiam" CssClass="col-6 text-center" runat="server">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Label ID="lblFiltroAChiamCodArt" runat="server" Text="Filtro Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroAChiamCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroAChiamDescriCodArt" runat="server" Text="Filtro Descri Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroAChiamDescriCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroAChiamProtocollo" runat="server" Text="Filtro Protocollo"></asp:Label>
                            <asp:TextBox ID="txtFiltroAChiamProtocollo" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Button ID="btnFiltraGrdAChiam" OnClick="btnFiltraGrdAChiam_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Filtra" />
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col text-center table-responsive tableContainer">
                    <asp:GridView ID="grdAChiam" ItemType="GestioneCantieri.Data.MaterialiCantieri" AutoGenerateColumns="false"
                        OnRowCommand="grdAChiam_RowCommand" CssClass="table table-dark table-striped text-center scrollable-table" runat="server">
                        <Columns>
                            <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                            <asp:BoundField DataField="ProtocolloInterno" HeaderText="Protocollo" />
                            <asp:BoundField DataField="Fornitore" HeaderText="Fornitore" />
                            <asp:BoundField DataField="CodArt" HeaderText="Cod. Art" />
                            <asp:BoundField DataField="DescriCodArt" HeaderText="Descr. Cod. Art." />
                            <asp:BoundField DataField="Qta" HeaderText="Quantità" />
                            <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Prezzo Unitario" DataFormatString="{0:0.00}" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnVisualChiam" CommandName="VisualChiam" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Visualizza">
                                        <i class="fas fa-eye" style="color: darkblue;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnModChiam" CommandName="ModChiam" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Modifica">
                                        <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnElimChiam" CommandName="ElimChiam" CommandArgument="<%# BindItem.IdMaterialiCantiere %>"
                                        runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo record?');">
                                        <i class="fas fa-times" style="color: red;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>

    <!-- Maschera di Gestione Spese -->
    <div class="row d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlGestSpese" CssClass="col text-center" runat="server">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <asp:Label ID="lblScegliSpesa" Text="Scegli Spesa" runat="server" />
                    <asp:DropDownList ID="ddlScegliSpesa" CssClass="form-control" AutoPostBack="true" OnTextChanged="ddlScegliSpesa_TextChanged" runat="server"></asp:DropDownList>
                </div>
                <div class="col">
                    <asp:Label ID="lblSpeseCodArt" Text="Codice Articolo" runat="server" />
                    <asp:TextBox ID="txtSpeseCodArt" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSpeseCodArt_TextChanged" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblSpeseDescriCodArt" Text="Descrizione Codice Articolo" runat="server" />
                    <asp:TextBox ID="txtSpeseDescriCodArt" CssClass="form-control" AutoPostBack="true" OnTextChanged="txtSpeseDescriCodArt_TextChanged" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblSpeseQta" Text="Quantità" runat="server" />
                    <asp:TextBox ID="txtSpeseQta" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblSpesaPrezzo" Text="Prezzo" runat="server" />
                    <asp:TextBox ID="txtSpesaPrezzo" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblSpesaPrezzoCalcolato" Text="Prezzo Calcolato" runat="server" />
                    <asp:TextBox ID="txtSpesaPrezzoCalcolato" Text="0.00" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-1">
                    <asp:Label ID="lblSpesaVisibile" Text="Visibile" runat="server" />
                    <asp:CheckBox ID="chkSpesaVisibile" CssClass="form-control" Checked="false" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblSpesaRicalcolo" Text="Ricalcolo" runat="server" />
                    <asp:CheckBox ID="chkSpesaRicalcolo" CssClass="form-control" Checked="false" Enabled="false" runat="server" />
                </div>
                <div class="col-1">
                    <asp:Label ID="lblSpesaRicarico" Text="Ricarico Si/No" runat="server" />
                    <asp:CheckBox ID="chkSpesaRicarico" CssClass="form-control" Checked="false" runat="server" />
                </div>
            </div>

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <asp:Button ID="btnCalcolaPzzoUnitSpese" OnClick="btnCalcolaPzzoUnitSpese_Click" CssClass="btn btn-lg btn-dark mr-3" runat="server" Text="Calcola Prezzo Spesa" />
                    <asp:Button ID="btnInsSpesa" OnClick="btnInsSpesa_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Inserisci Spesa" />
                    <asp:Button ID="btnModSpesa" OnClick="btnModSpesa_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Modifica Spesa" /><br />
                    <asp:Label ID="lblIsSpesaInserita" Text="" CssClass="pull-right" runat="server" />
                </div>
            </div>

            <asp:HiddenField ID="hidIdSpesa" runat="server" />

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <asp:Panel ID="pnlFiltriGrdSpese" CssClass="col-6 text-center" runat="server">
                    <div class="row mt-3 d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Label ID="lblFiltroSpeseCodArt" runat="server" Text="Filtro Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroSpeseCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroSpeseDescriCodArt" runat="server" Text="Filtro Descri Cod Art"></asp:Label>
                            <asp:TextBox ID="txtFiltroSpeseDescriCodArt" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Label ID="lblFiltroSpeseProtocollo" runat="server" Text="Filtro Protocollo"></asp:Label>
                            <asp:TextBox ID="txtFiltroSpeseProtocollo" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                        <div class="col">
                            <asp:Button ID="btnFiltraGrdSpese" OnClick="btnFiltraGrdSpese_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Filtra" />
                        </div>
                    </div>
                </asp:Panel>
            </div>

            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col text-center table-responsive tableContainer">
                    <asp:GridView ID="grdSpese" ItemType="GestioneCantieri.Data.MaterialiCantieri" AutoGenerateColumns="false"
                        OnRowCommand="grdSpese_RowCommand" CssClass="table table-dark table-striped text-center scrollable-table" runat="server">
                        <Columns>
                            <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                            <asp:BoundField DataField="ProtocolloInterno" HeaderText="Protocollo" />
                            <asp:BoundField DataField="Fornitore" HeaderText="Fornitore" />
                            <asp:BoundField DataField="CodArt" HeaderText="Cod. Art" />
                            <asp:BoundField DataField="DescriCodArt" HeaderText="Descr. Cod. Art." />
                            <asp:BoundField DataField="Qta" HeaderText="Quantità" />
                            <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Prezzo Unitario" DataFormatString="{0:0.00}" />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnVisualSpesa" CommandName="VisualSpesa" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Visualizza">
                                        <i class="fas fa-eye" style="color: darkblue;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnModSpesa" CommandName="ModSpesa" CommandArgument="<%# BindItem.IdMaterialiCantiere %>" runat="server" Text="Modifica">
                                        <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnElimSpesa" CommandName="ElimSpesa" CommandArgument="<%# BindItem.IdMaterialiCantiere %>"
                                        runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo record?');">
                                        <i class="fas fa-times" style="color: red;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </asp:Panel>
    </div>
</asp:Content>

<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="GestionePagamenti.aspx.cs" Inherits="GestioneCantieri.GestionePagamenti" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Acconti Clienti</title>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Acconti Clienti</h1>
        </div>
    </div>

    <asp:Panel ID="pnlFiltriSceltaCant" CssClass="row mt-3 d-flex justify-content-center align-items-center" runat="server">
        <div class="col-6">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <asp:Label ID="lblFiltroCantAnno" Text="Anno" runat="server" />
                    <asp:TextBox ID="txtFiltroCantAnno" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col text-center">
                    <asp:Label ID="lblFiltroCantCodCant" Text="Cod Cant" runat="server" />
                    <asp:TextBox ID="txtFiltroCantCodCant" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col text-center">
                    <asp:Label ID="lblFiltroCantDescrCodCant" Text="Descri Cod Cant" runat="server" />
                    <asp:TextBox ID="txtFiltroCantDescrCodCant" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col text-center">
                    <asp:Label ID="lblFiltroCantChiuso" Text="Chiuso" runat="server" />
                    <asp:CheckBox ID="chkFiltroCantChiuso" CssClass="form-control" Checked="false" runat="server" />
                </div>
                <div class="col text-center">
                    <asp:Label ID="lblFiltroCantRiscosso" Text="Riscosso" runat="server" />
                    <asp:CheckBox ID="chkFiltroCantRiscosso" CssClass="form-control" Checked="false" runat="server" />
                </div>
                <div class="col text-center">
                    <asp:Button ID="btnFiltroCant" CssClass="btn btn-lg btn-primary pull-left" OnClick="btnFiltroCant_Click" runat="server" Text="Filtra" />
                </div>
            </div>

            <div class="row d-flex justify-content-center align-items-center">
                <div class="col text-center">
                    <asp:Label ID="lblScegliCant" Text="Scegli Cantiere" runat="server" />
                    <asp:DropDownList ID="ddlScegliCant" CssClass="form-control" AutoPostBack="true" OnTextChanged="ddlScegliCant_TextChanged" runat="server" />
                </div>
            </div>
        </div>
    </asp:Panel>

    <!-- Maschera Gestione Pagamenti-->
    <asp:Panel ID="pnlGestPagam" CssClass="row mt-3 d-flex justify-content-center align-items-center" runat="server">
        <div class="col-8 text-center">
            <div class="row d-flex justify-content-center align-items-center">
                <!-- Data -->
                <div class="col text-center">
                    <asp:Label ID="lblDataDDT" Text="Data DDT" runat="server" />
                    <asp:TextBox ID="txtDataDDT" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                </div>

                <!-- Campi per l'inserimento dei valori -->
                <div class="col text-center">
                    <asp:Label ID="lblImportoPagam" Text="Importo" runat="server" />
                    <asp:TextBox ID="txtImportoPagam" CssClass="form-control" runat="server" Text=""></asp:TextBox>
                </div>
                <div class="col text-center">
                    <asp:Label ID="lblDescrPagam" Text="Descrizione" runat="server" />
                    <asp:TextBox ID="txtDescrPagam" CssClass="form-control" runat="server" Text=""></asp:TextBox>
                </div>

                <%-- Checkbox --%>
                <div class="col-1 text-center">
                    <asp:Label ID="lblAcconto" Text="Acconto" runat="server" />
                    <asp:CheckBox ID="chkAcconto" CssClass="form-control" runat="server" />
                </div>
                <div class="col-1 text-center">
                    <asp:Label ID="lblSaldo" Text="Saldo" runat="server" />
                    <asp:CheckBox ID="chkSaldo" CssClass="form-control" runat="server" />
                </div>
                <div class="col-2 text-center">
                    <asp:Button ID="btnInsPagam" OnClick="btnInsPagam_Click" CssClass="btn btn-lg btn-primary pull-right" runat="server" Text="Inserisci Pagamento" />
                    <asp:Button ID="btnModPagam" OnClick="btnModPagam_Click" CssClass="btn btn-lg btn-primary pull-right" runat="server" Text="Modifica Pagamento" />
                    <asp:Label ID="lblIsPagamInserito" Text="" CssClass="pull-right" runat="server" />
                </div>
            </div>

            <asp:HiddenField ID="hidPagamenti" runat="server" />

            <div class="row mt-5 d-flex justify-content-center align-items-center">
                <div class="col-6 text-center">
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Label ID="lblFiltroPagamDescri" runat="server" Text="Filtro Descrizione"></asp:Label>
                            <asp:TextBox ID="txtFiltroPagamDescri" CssClass="form-control" runat="server"></asp:TextBox>
                        </div>
                    </div>
                    <div class="row d-flex justify-content-center align-items-center">
                        <div class="col">
                            <asp:Button ID="btnFiltraPagam" OnClick="btnFiltraPagam_Click" CssClass="btn btn-lg btn-primary" runat="server" Text="Filtra Pagamenti" />
                        </div>
                    </div>
                </div>
            </div>

            <div class="row mt-4 d-flex justify-content-center align-items-center">
                <div class="col text-center table-responsive tableContainer">
                    <asp:GridView ID="grdPagamenti" ItemType="GestioneCantieri.Data.Pagamenti" AutoGenerateColumns="false"
                        OnRowCommand="grdPagamenti_RowCommand" CssClass="table table-striped table-dark text-center scrollable-table" runat="server">
                        <Columns>
                            <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                            <asp:BoundField DataField="Imporo" HeaderText="Importo" DataFormatString="{0:0.00}" />
                            <asp:BoundField DataField="DescriPagamenti" HeaderText="Descriz. Pagam." />
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnVisualPagam" CommandName="VisualPagam" CommandArgument="<%# BindItem.IdPagamenti %>" CssClass="btn btn-lg btn-default" runat="server" Text="Visualizza">
                                        <i class="fas fa-eye" style="color: darkblue;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnModPagam" CommandName="ModPagam" CommandArgument="<%# BindItem.IdPagamenti %>" CssClass="btn btn-lg btn-default" runat="server" Text="Modifica">
                                        <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <asp:LinkButton ID="btnElimPagam" CommandName="ElimPagam" CommandArgument="<%# BindItem.IdPagamenti %>"
                                        CssClass="btn btn-lg btn-default" runat="server" Text="Elimina" OnClientClick="return confirm('Vuoi veramente eliminare questo record?');">
                                        <i class="fas fa-times" style="color: red;"></i>
                                    </asp:LinkButton>
                                </ItemTemplate>
                            </asp:TemplateField>
                        </Columns>
                    </asp:GridView>
                </div>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

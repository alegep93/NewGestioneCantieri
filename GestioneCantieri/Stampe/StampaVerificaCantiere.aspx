<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="StampaVerificaCantiere.aspx.cs" Inherits="GestioneCantieri.StampaVerificaCantiere" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Stampa Verifica Cantiere</title>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Verifica Cantiere</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6">
            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblAnno" runat="server" Text="Anno"></asp:Label>
                    <asp:TextBox ID="txtAnno" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblCodCant" runat="server" Text="Codice Cantiere"></asp:Label>
                    <asp:TextBox ID="txtCodCant" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col-1 text-center">
                    <asp:Label ID="lblChiuso" runat="server" Text="Chiuso"></asp:Label>
                    <asp:CheckBox ID="chkChiuso" runat="server" />
                </div>
                <div class="col-1 text-center">
                    <asp:Label ID="lblRiscosso" runat="server" Text="Riscosso"></asp:Label>
                    <asp:CheckBox ID="chkRiscosso" runat="server" />
                </div>
                <div class="col text-right">
                    <asp:Button ID="btnFiltraCantieri" CssClass="btn btn-lg btn-primary" OnClick="btnFiltraCantieri_Click" runat="server" Text="Filtra Cantieri" />
                </div>
            </div>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <asp:Label ID="lblScegliCantiere" runat="server" Text="Scegli Cantiere"></asp:Label>
            <asp:DropDownList ID="ddlScegliCant" CssClass="form-control" AutoPostBack="true" OnTextChanged="ddlScegliCant_TextChanged" runat="server"></asp:DropDownList>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:Button ID="btnStampaVerificaCant" CssClass="btn btn-lg btn-primary pull-right" OnClick="btnStampaVerificaCant_Click" runat="server" Text="Stampa Verifica Cantiere" />
        </div>
    </div>

    <asp:Panel ID="pnlViewGridAndLabels" runat="server">
        <div class="row mt-3 d-flex justify-content-center align-items-center">
            <div class="col text-center">
                <asp:Label ID="lblIntestStampa" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
            </div>
        </div>

        <div class="row mt-3 d-flex justify-content-center align-items-center">
            <div class="table-container col text-center">
                <asp:GridView ID="grdStampaVerificaCant" runat="server"
                    ItemType="GestioneCantieri.Data.MaterialiCantieri" AutoGenerateColumns="False" CssClass="table table-dark table-striped text-center scrollable-table"
                    AllowSorting="true" OnSorting="grdStampaVerificaCant_Sorting">
                    <Columns>
                        <asp:BoundField DataField="Tipologia" HeaderText="Tipologia" />
                        <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                        <asp:BoundField DataField="CodArt" HeaderText="Codice Articolo" />
                        <asp:BoundField DataField="NumeroBolla" HeaderText="Numero Bolla" />
                        <asp:BoundField DataField="Qta" HeaderText="Qta" />
                        <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Pzzo Unit." DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="Valore" HeaderText="Valore" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>

        <div class="row mt-3 d-flex justify-content-center align-items-center">
            <div class="col text-center">
                <div class="row mt-3 d-flex justify-content-center align-items-center">
                    <div class="col">
                        <asp:Label ID="lblTotContoCliente" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="lblTotMate" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="lblTotRientro" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="lblTotOper" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="row d-flex justify-content-center align-items-center">
            <div class="col text-center">
                <div class="row mt-3 d-flex justify-content-center align-items-center">
                    <div class="col">
                        <asp:Label ID="lblTotArrot" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="lblTotAChiamata" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="lblTotGuadagno" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="lblTotSpese" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>

        <div class="row mb-3 d-flex justify-content-center align-items-center">
            <div class="col text-center">
                <div class="row mt-3 d-flex justify-content-center align-items-center">
                    <div class="col">
                        <asp:Label ID="lblTotManodop" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="lblTotGuadagnoConManodop" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
                    </div>
                    <div class="col">
                        <asp:Label ID="lblTotGuadagnoOrarioManodop" runat="server" CssClass="lblIntestazione" Text=""></asp:Label>
                    </div>
                </div>
            </div>
        </div>
    </asp:Panel>

    <!-- GridView di appoggio -->
    <asp:GridView ID="grdStampaMateCant" runat="server" ItemType="GestioneCantieri.Data.MaterialiCantieri"
        AutoGenerateColumns="False" Visible="false">
        <Columns>
            <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
            <asp:BoundField DataField="DescriCodArt" HeaderText="Descr. CodArt" />
            <asp:BoundField DataField="Qta" HeaderText="Qta" />
            <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Pzzo Unit." DataFormatString="{0:0.00}" />
            <asp:BoundField DataField="ValoreRicarico" HeaderText="Valore Ricarico" />
            <asp:BoundField DataField="ValoreRicalcolo" HeaderText="Valore Ricalcolo" />
            <asp:BoundField DataField="PzzoFinCli" HeaderText="Pzzo Unit Fin Cli" />
            <asp:BoundField DataField="Valore" HeaderText="Valore" />
            <asp:BoundField DataField="Visibile" HeaderText="Visibile" />
            <asp:BoundField DataField="Ricalcolo" HeaderText="Ricalcolo" />
            <asp:BoundField DataField="RicaricoSiNo" HeaderText="RicaricoSiNo" />
        </Columns>
    </asp:GridView>

    <asp:GridView ID="grdStampaMateCantPDF" runat="server" ItemType="GestioneCantieri.Data.MaterialiCantieri"
        AutoGenerateColumns="False" Visible="false">
        <Columns>
            <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
            <asp:BoundField DataField="DescriCodArt" HeaderText="Descr. CodArt" />
            <asp:BoundField DataField="Qta" HeaderText="Qta" />
            <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Pzzo Unit." DataFormatString="{0:0.00}" />
            <asp:BoundField DataField="Valore" HeaderText="Valore" />
        </Columns>
    </asp:GridView>
</asp:Content>

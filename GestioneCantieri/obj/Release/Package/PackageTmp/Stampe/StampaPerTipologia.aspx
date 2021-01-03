<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="StampaPerTipologia.aspx.cs" Inherits="GestioneCantieri.StampaPerTipologia" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Stampa Manodopera</title>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row d-flex justify-content-center align-items-center">
        <div class="col">
            <h1>Stampa Manodopera</h1>
        </div>
    </div>

    <div class="row d-flex justify-content-center align-items-center">
        <!-- Stampa Per Cantiere -->
        <div class="col-6">
            <div class="row d-flex justify-content-center align-items-center">
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

    <div class="row d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:Label ID="lblScegliCantiere" runat="server" Text="Scegli Cantiere"></asp:Label>
            <asp:DropDownList ID="ddlScegliCant" AutoPostBack="true" OnSelectedIndexChanged="ddlScegliCant_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
        <div class="col">
            <asp:Label ID="lblDataDa" runat="server" Text="Data Da:"></asp:Label>
            <asp:TextBox ID="txtDataDa" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <div class="col">
            <asp:Label ID="lblDataA" runat="server" Text="Data A:"></asp:Label>
            <asp:TextBox ID="txtDataA" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
        <!-- Scelta Operaio -->
        <%--<div class="col text-center">
            <asp:Label ID="lblScegliOperaio" runat="server" Text="Scegli Operaio"></asp:Label>
            <asp:DropDownList ID="ddlScegliOperaio" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>--%>
    </div>

    <!-- Tipologie -->
    <%--<div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:Label ID="lblManodop" runat="server" Text="Manodopera"></asp:Label>
            <asp:RadioButton ID="rdbManodop" GroupName="rdbTipol" Checked="true" runat="server" />
            <asp:Label ID="lblOperaio" CssClass="ml-5" runat="server" Text="Operaio"></asp:Label>
            <asp:RadioButton ID="rdbOper" GroupName="rdbTipol" runat="server" />
        </div>
    </div>--%>

    <!-- Bottone di stampa -->
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:Button ID="btnStampaPerTipologia" CssClass="btn btn-lg btn-primary" OnClick="btnStampaVerificaCant_Click" runat="server" Text="Stampa" />
        </div>
    </div>

    <asp:Panel ID="pnlShowGridAndLabel" Visible="false" runat="server">
        <div class="row mt-3 d-flex justify-content-center align-items-center">
            <div class="col text-center">
                <asp:Label ID="lblTotOre" CssClass="lblIntestazione" runat="server" Text=""></asp:Label>
            </div>
            <div class="col text-center">
                <asp:Label ID="lblTotale" CssClass="lblIntestazione" runat="server" Text=""></asp:Label>
            </div>
        </div>

        <div class="row mt-3 d-flex justify-content-center align-items-center">
            <div class="table-container col text-center">
                <asp:GridView ID="grdStampaPerTipologia" runat="server" ItemType="GestioneCantieri.Data.MaterialiCantieri" AutoGenerateColumns="False" CssClass="table table-dark table-striped text-center scrollable-table">
                    <Columns>
                        <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                        <asp:BoundField DataField="CodCant" HeaderText="Codice Cantiere" />
                        <asp:BoundField DataField="DescriCodCant" HeaderText="Descrizione CodCant" />
                        <asp:BoundField DataField="RagSocCli" HeaderText="Ragione Soc. Cliente" />
                        <asp:BoundField DataField="Qta" HeaderText="Qta" />
                        <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Pzzo Unit. Cantiere" DataFormatString="{0:0.00}" />
                        <asp:BoundField DataField="Acquirente" HeaderText="Operaio" />
                    </Columns>
                </asp:GridView>
            </div>
        </div>
    </asp:Panel>
</asp:Content>

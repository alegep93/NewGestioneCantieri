﻿<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/layout.Master" CodeBehind="ResocontoOperaio.aspx.cs" Inherits="GestioneCantieri.ResocontoOperaio" EnableEventValidation="false" %>

<asp:Content ID="title" ContentPlaceHolderID="title" runat="server">
    <title>Resoconto Operaio</title>
</asp:Content>

<asp:Content ID="body" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Resoconto Operaio</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <%-- Icona stampa Excel --%>
            <asp:LinkButton ID="btnStampaExcel" CssClass="excel-icon" OnClick="btnStampaExcel_Click" runat="server">
        <i class="fas fa-file-excel"></i>
            </asp:LinkButton>
        </div>
    </div>

    <asp:Panel ID="pnlResocontoOperaio" DefaultButton="btnStampaResoconto" runat="server">
        <div class="row mt-3 d-flex justify-content-center align-items-center">
            <div class="col">
                <asp:Label ID="lblDataDa" runat="server" Text="Data Da:"></asp:Label>
                <asp:TextBox ID="txtDataDa" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <asp:Label ID="lblDataA" runat="server" Text="Data A:"></asp:Label>
                <asp:TextBox ID="txtDataA" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
            </div>
            <div class="col">
                <asp:Label ID="lblScegliOperaio" runat="server" Text="Scegli Operaio"></asp:Label>
                <asp:DropDownList ID="ddlScegliOperaio" OnSelectedIndexChanged="ddlScegliOperaio_SelectedIndexChanged" AutoPostBack="true" CssClass="form-control" runat="server"></asp:DropDownList>
            </div>
        </div>
        <div class="row d-flex justify-content-center align-items-center">
            <div class="col text-center">
                <asp:Button ID="btnStampaResoconto" CssClass="btn btn-lg btn-primary" OnClick="btnStampaResoconto_Click" runat="server" Text="Stampa Resoconto" />
                <asp:Button ID="btnResocontoRaggruppato" CssClass="btn btn-lg btn-primary ml-3" OnClick="btnResocontoRaggruppato_Click" runat="server" Text="Resoconto Raggruppato" />
                <asp:Button ID="btnPagaOperaio" CssClass="btn btn-lg btn-dark ml-3" OnClick="btnPagaOperaio_Click" OnClientClick="return confirm('Vuoi veramente pagare questo operaio?')" runat="server" Text="Paga Operaio" /><br />
                <asp:Label ID="lblIsOperaioPagato" runat="server" Text=""></asp:Label>
            </div>
        </div>
    </asp:Panel>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <asp:Panel ID="pnlFiltri" DefaultButton="btnFiltra" CssClass="col-6 text-center" runat="server">
            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblFiltroCantiere" Text="Filtra Cantiere" runat="server"></asp:Label>
                    <asp:TextBox ID="txtFiltroCantiere" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col text-center">
                    <asp:RadioButtonList ID="rblChooseView" RepeatDirection="Horizontal" Width="100%" runat="server">
                        <asp:ListItem Value="0" Selected="True">Non Pagato</asp:ListItem>
                        <asp:ListItem Value="1">Pagato</asp:ListItem>
                        <asp:ListItem Value="2">Tutti</asp:ListItem>
                    </asp:RadioButtonList>
                </div>
                <div class="col text-left">
                    <asp:Button ID="btnFiltra" Text="Filtra" CssClass="btn btn-lg btn-primary" OnClick="btnFiltra_Click" runat="server"></asp:Button>
                </div>
            </div>
        </asp:Panel>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <div class="row mt-3 d-flex justify-content-center align-items-center">
                <div class="col">
                    <strong>
                        <asp:Label ID="lblTotaleOre" runat="server" Text="" CssClass="label-totali-resoconto-operaio"></asp:Label></strong>
                </div>
                <div class="col">
                    <strong>
                        <asp:Label ID="lblTotaleValore" runat="server" Text="" CssClass="label-totali-resoconto-operaio ml-3"></asp:Label></strong>
                </div>
            </div>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center table-container">
            <asp:GridView ID="grdResocontoOperaio" runat="server" ItemType="GestioneCantieri.Data.MaterialiCantieri"
                AutoGenerateColumns="False" CssClass="table table-dark table-striped text-center scrollable-table">
                <Columns>
                    <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                    <asp:BoundField DataField="Acquirente" HeaderText="Operaio" />
                    <asp:BoundField DataField="CodCant" HeaderText="CodCant" />
                    <asp:BoundField DataField="DescriCodCant" HeaderText="DescriCodCant" />
                    <asp:BoundField DataField="Qta" HeaderText="Qta" />
                    <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Pzzo Unit. Cant" DataFormatString="{0:0.00}" />
                    <asp:BoundField DataField="Valore" HeaderText="Valore" />
                    <asp:BoundField DataField="OperaioPagato" HeaderText="Operaio Pagato" />
                </Columns>
            </asp:GridView>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center table-container">
            <asp:GridView ID="grdResocontoRaggruppato" runat="server" ItemType="GestioneCantieri.Data.MaterialiCantieri"
                AutoGenerateColumns="False" CssClass="table table-dark table-striped text-center scrollable-table">
                <Columns>
                    <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                    <asp:BoundField DataField="Acquirente" HeaderText="Operaio" />
                    <asp:BoundField DataField="Qta" HeaderText="Qta" />
                    <asp:BoundField DataField="PzzoUniCantiere" HeaderText="Pzzo Unit. Cant" DataFormatString="{0:0.00}" />
                    <asp:BoundField DataField="Valore" HeaderText="Valore" />
                    <asp:BoundField DataField="OperaioPagato" HeaderText="Operaio Pagato" />
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

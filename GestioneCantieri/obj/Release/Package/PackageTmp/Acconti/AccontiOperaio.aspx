<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="AccontiOperaio.aspx.cs" Inherits="GestioneCantieri.Acconti.AccontiOperaio" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Acconti Operaio</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Acconti Operaio</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <div class="row d-flex justify-content-center align-items-center">
                <div class="col">
                    <asp:Label ID="lblData" runat="server" Text="Data"></asp:Label>
                    <asp:TextBox ID="txtData" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblScegliOperaio" runat="server" Text="Scegli Operaio"></asp:Label>
                    <asp:DropDownList ID="ddlScegliOperaio" CssClass="form-control" runat="server"></asp:DropDownList>
                </div>
                <div class="col">
                    <asp:Label ID="lblImportoAcconto" runat="server" Text="Importo Acconto"></asp:Label>
                    <asp:TextBox ID="txtImportoAcconto" CssClass="form-control" runat="server"></asp:TextBox>
                </div>
                <div class="col">
                    <asp:Label ID="lblDescrizioneAcconto" runat="server" Text="Descrizione Acconto"></asp:Label>
                    <asp:TextBox ID="txtDescrizioneAcconto" CssClass="form-control" TextMode="MultiLine" Rows="3" runat="server"></asp:TextBox>
                </div>
                <div class="col text-center">
                    <asp:Button ID="btnSalvaAcconto" CssClass="btn btn-lg btn-primary" OnClick="btnSalvaAcconto_Click" Text="Inserisci" runat="server" />
                    <asp:Button ID="btnModificaAcconto" CssClass="btn btn-lg btn-primary" OnClick="btnModificaAcconto_Click" Text="Modifica" runat="server" />
                </div>
            </div>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <asp:Label ID="lblMsg" runat="server"></asp:Label>
        </div>
    </div>

    <asp:HiddenField ID="hfIdAccontoOperaio" runat="server" />

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-3 text-center">
            <asp:Label ID="lblFiltroScegliOperaio" runat="server" Text="Filtro per operaio"></asp:Label>
            <asp:DropDownList ID="ddlFiltroScegliOperaio" AutoPostBack="true" OnSelectedIndexChanged="ddlFiltroScegliOperaio_SelectedIndexChanged" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center table-container">
            <asp:GridView ID="grdAccontiOperai" ItemType="GestioneCantieri.Data.AccontoOperaio" AutoGenerateColumns="False" runat="server"
                CssClass="table table-dark table-striped text-center scrollable-table" OnRowCommand="grdAccontiOperai_RowCommand" OnRowDataBound="grdAccontiOperai_RowDataBound">
                <Columns>
                    <asp:BoundField DataField="Data" HeaderText="Data" DataFormatString="{0:d}" ApplyFormatInEditMode="True" />
                    <asp:BoundField DataField="NomeOp" HeaderText="Nome Operaio" />
                    <asp:BoundField DataField="Importo" HeaderText="Importo" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkAccontoPagato" Checked="<%# Item.Pagato %>" Enabled="false" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnVisualizza" CommandName="Visualizza" CommandArgument="<%# BindItem.IdAccontoOperaio %>" runat="server">
                                    <i class="fas fa-eye" style="color: darkblue;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnModifica" CommandName="Modifica" CommandArgument="<%# BindItem.IdAccontoOperaio %>" runat="server">
                                    <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElimina" CommandName="Elimina" CommandArgument="<%# BindItem.IdAccontoOperaio %>" runat="server" OnClientClick="return confirm('Vuoi veramente eliminare questo acconto?');">
                                    <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

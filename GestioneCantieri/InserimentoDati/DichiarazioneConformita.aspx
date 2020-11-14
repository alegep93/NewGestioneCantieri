<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="DichiarazioneConformita.aspx.cs" Inherits="GestioneCantieri.DichiarazioneConformita" %>
<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Dichiarazione di conformità</title>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col text-center">
            <h1>Dichiarazione di conformità</h1>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <asp:Label ID="lblScegliCantiere" Text="Scegli cantiere" runat="server"></asp:Label>
            <asp:DropDownList ID="ddlScegliCantiere" CssClass="form-control" runat="server"></asp:DropDownList>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-2 text-center">
            <asp:Label ID="lblData" Text="Data" runat="server"></asp:Label>
            <asp:TextBox ID="txtData" TextMode="Date" CssClass="form-control" runat="server"></asp:TextBox>
        </div>
    </div>

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center">
            <asp:Button ID="btnSalva" CssClass="btn btn-lg btn-primary" OnClick="btnSalva_Click" Text="Salva" runat="server" />
        </div>
    </div>

    <asp:HiddenField ID="hfDataDichiarazioneCantiereOld" runat="server" />

    <div class="row mt-3 d-flex justify-content-center align-items-center">
        <div class="col-6 text-center table-container">
            <asp:GridView ID="grdDiCo" AutoGenerateColumns="false" ItemType="GestioneCantieri.Data.Cantieri"
                OnRowCommand="grdDiCo_RowCommand" CssClass="table table-striped table-dark text-center scrollable-table" runat="server">
                <Columns>
                    <asp:BoundField HeaderText="N° DiCo" DataField="NumDiCo" />
                    <asp:BoundField HeaderText="Codice Cantiere" DataField="CodCant" />
                    <asp:BoundField HeaderText="Descrizione Cantiere" DataField="DescriCodCant" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnModifica" CommandName="Modifica" CommandArgument="<%# BindItem.IdCantieri %>" runat="server">
                                <i class="fas fa-pencil-alt" style="color: darkorange;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:LinkButton ID="btnElimina" CommandName="Elimina" CommandArgument="<%# BindItem.IdCantieri %>" runat="server" OnClientClick="return confirm('Vuoi veramente eliminare questa dichiarazione di conformità?');">
                                <i class="fas fa-times" style="color: red;"></i>
                            </asp:LinkButton>
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
    </div>
</asp:Content>

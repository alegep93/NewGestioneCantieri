<%@ Page Title="" Language="C#" MasterPageFile="~/layout.Master" AutoEventWireup="true" CodeBehind="ControlloGruppi.aspx.cs" Inherits="GestioneCantieri.ControlloGruppi" %>

<asp:Content ID="Content1" ContentPlaceHolderID="title" runat="server">
    <title>Controllo Gruppi</title>
    <style>
        .tableContainer {
            max-height: 800px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="body" runat="server">
    <div class="row">
        <div class="col-md-10 tableContainer">
            <asp:gridview id="grdFruttiNonControllati" itemtype="GestioneCantieri.Data.GruppiFrutti" autogeneratecolumns="false" onrowdatabound="grdFruttiNonControllati_RowDataBound" onrowcommand="grdFruttiNonControllati_RowCommand" cssclass="table table-striped text-center scrollable-table" runat="server">
                <Columns>
                    <asp:BoundField DataField="NomeGruppo" HeaderText="Nome Gruppo" />
                    <asp:BoundField DataField="Descr" HeaderText="Descrizione" />
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkGruppoCompletato" Enabled="false" Checked="<%# BindItem.Completato %>" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkGruppoControllato" Enabled="false" Checked="<%# BindItem.Controllato %>" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnMostraCompGruppo" CommandName="MostraCompGruppo" CommandArgument='<%# BindItem.Id %>' CssClass="btn btn-lg btn-default" runat="server" Text="Visualizza Componenti" />
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Button ID="btnControllaGruppo" CommandName="ControllaGruppo" CommandArgument="<%# BindItem.Id %>" CssClass="btn btn-lg btn-default" runat="server" Text="Controllato" />
                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:gridview>
        </div>

        <div class="col-md-2 compGruppoFixed">
            <div class="panel panel-default">
                <div class="panel-heading">
                    <h3 class="panel-title">
                        <asp:label id="lblPanelTitleGroupName" runat="server"></asp:label>
                    </h3>
                </div>
                <div class="panel-body">
                    <asp:label id="lblGruppiNonControllati" runat="server" text="Componenti Gruppo"></asp:label>
                    <ul class="list-group">
                        <% foreach (var item in componentiGruppo)
                            {%>
                        <li class="list-group-item"><%= item.Qta + " - " + item.NomeFrutto %></li>
                        <%} %>
                    </ul>
                </div>
                <div class="panel-footer">
                    <asp:label id="lblNumGruppiNonControllati" runat="server"></asp:label>
                </div>
            </div>
        </div>
    </div>
</asp:Content>

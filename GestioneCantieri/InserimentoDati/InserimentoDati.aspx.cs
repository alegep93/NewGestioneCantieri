using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class InserimentoDati : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                MostraPannello(true, false, false, false, false);
                lblTitoloInserimento.Text = "Inserimento Clienti";
                BindGridClienti();
                FillDdlScegliAmministratore();
                btnModCliente.Visible = btnModFornit.Visible = false;
                btnModOper.Visible = btnModCantiere.Visible = false;
                btnModSpesa.Visible = false;

                string annoCantiere = txtAnnoCant.Text != "" ? txtAnnoCant.Text : DateTime.Now.Year.ToString();
                PopolaCodCantAnnoNumero(annoCantiere);
            }

            Page.MaintainScrollPositionOnPostBack = true;
        }

        private void FillDdlScegliAmministratore()
        {
            ddlScegliAmministratore.Items.Clear();
            ddlScegliAmministratore.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlAmministratore(AmministratoriDAO.GetAll(), ref ddlScegliAmministratore);
        }

        #region Clienti
        protected void btnInsCliente_Click(object sender, EventArgs e)
        {
            Clienti cliente = FillObjCliente();

            if (txtRagSocCli.Text != "")
            {
                if (txtDataInserimento.Text != "")
                {
                    bool isInserito = ClientiDAO.InserisciCliente(cliente);

                    if (isInserito)
                    {
                        lblIsClienteInserito.Text = "Cliente '" + txtRagSocCli.Text + "' inserito correttamente";
                        lblIsClienteInserito.ForeColor = Color.Blue;
                    }
                    else
                    {
                        lblIsClienteInserito.Text = "Errore durante l'inserimento del cliente '" + txtRagSocCli.Text + "'";
                        lblIsClienteInserito.ForeColor = Color.Red;
                    }

                    ResettaCampi(pnlInsClienti);
                    BindGridClienti();
                }
                else
                {
                    lblIsClienteInserito.Text = "Il campo 'Data' deve essere compilato";
                    lblIsClienteInserito.ForeColor = Color.Red;
                }
            }
            else
            {
                lblIsClienteInserito.Text = "Il campo 'Ragione Sociale' deve essere compilato";
                lblIsClienteInserito.ForeColor = Color.Red;
            }
        }
        private Clienti FillObjCliente()
        {
            return new Clienti
            {
                RagSocCli = txtRagSocCli.Text,
                IdAmministratore = Convert.ToInt64(ddlScegliAmministratore.SelectedValue) == -1 ? (long?)null : Convert.ToInt64(ddlScegliAmministratore.SelectedValue),
                Indirizzo = txtIndirizzo.Text,
                Cap = txtCap.Text,
                Città = txtCitta.Text,
                Provincia = txtProvincia.Text,
                Tel1 = txtTelefono.Text,
                Cell1 = txtCellulare.Text,
                PartitaIva = txtPartitaIva.Text,
                CodFiscale = txtCodiceFiscale.Text,
                Data = Convert.ToDateTime(txtDataInserimento.Text != "" ? txtDataInserimento.Text : DateTime.Now.ToString("dd-MM-yyyy")),
                Note = txtNote.Text
            };
        }
        protected void btnModCliente_Click(object sender, EventArgs e)
        {
            Clienti cliente = FillObjCliente();
            cliente.IdCliente = Convert.ToInt32(hidIdClienti.Value);
            bool isUpdated = ClientiDAO.UpdateCliente(cliente);

            if (isUpdated)
            {
                lblIsClienteInserito.Text = "Cliente '" + txtRagSocCli.Text + "' modificato con successo";
                lblIsClienteInserito.ForeColor = Color.Blue;
            }
            else
            {
                lblIsClienteInserito.Text = "Errore durante la modifica del cliente '" + txtRagSocCli.Text + "'";
                lblIsClienteInserito.ForeColor = Color.Red;
            }

            ResettaCampi(pnlInsClienti);
            BindGridClienti();
            btnInsCliente.Visible = true;
            btnModCliente.Visible = !btnInsCliente.Visible;
        }
        protected void btnFiltraClienti_Click(object sender, EventArgs e)
        {
            BindGridClienti();
        }
        protected void btnSvuotaFiltriClienti_Click(object sender, EventArgs e)
        {
            ResettaCampi(pnlFiltriCliente);
            BindGridClienti();
        }
        protected void BindGridClienti()
        {
            grdClienti.DataSource = ClientiDAO.GetClienti(txtFiltroRagSocCli.Text);
            grdClienti.DataBind();
        }
        protected void PopolaCampiCliente(int idCli, bool isControlEnabled)
        {
            EnableDisableFields(pnlInsClienti, isControlEnabled);

            //Popolo i textbox
            Clienti cli = ClientiDAO.GetSingle(idCli);
            txtRagSocCli.Text = cli.RagSocCli;
            txtIndirizzo.Text = cli.Indirizzo;
            txtCap.Text = cli.Cap;
            txtCitta.Text = cli.Città;
            txtTelefono.Text = cli.Tel1.ToString();
            txtCellulare.Text = cli.Cell1.ToString();
            txtPartitaIva.Text = cli.PartitaIva;
            txtCodiceFiscale.Text = cli.CodFiscale;
            txtDataInserimento.Text = cli.Data.ToString();
            txtProvincia.Text = cli.Provincia;
            txtDataInserimento.Text = cli.Data.ToString("yyyy-MM-dd");
            txtNote.Text = cli.Note;
            ddlScegliAmministratore.SelectedValue = cli.IdAmministratore > 0 ? cli.IdAmministratore.ToString() : "-1";
        }
        protected void grdClienti_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idCli = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "VisualCli")
            {
                lblTitoloInserimento.Text = "Visualizza Cliente";
                lblIsClienteInserito.Text = "";
                btnInsCliente.Visible = btnModCliente.Visible = false;
                PopolaCampiCliente(idCli, false);
            }
            else if (e.CommandName == "ModCli")
            {
                lblTitoloInserimento.Text = "Modifica Cliente";
                lblIsClienteInserito.Text = "";
                btnModCliente.Visible = true;
                btnInsCliente.Visible = false;
                hidIdClienti.Value = idCli.ToString();
                PopolaCampiCliente(idCli, true);
            }
            else if (e.CommandName == "ElimCli")
            {
                bool isEliminato = ClientiDAO.EliminaCliente(idCli);
                if (isEliminato)
                {
                    lblIsClienteInserito.Text = "Cliente eliminato con successo";
                    lblIsClienteInserito.ForeColor = Color.Blue;
                }
                else
                {
                    lblIsClienteInserito.Text = "Errore durante l'eliminazione del cliente, potrebbe avere delle referenze in altre tabelle";
                    lblIsClienteInserito.ForeColor = Color.Red;
                }

                BindGridClienti();
                ResettaCampi(pnlInsClienti);
                btnInsCliente.Visible = true;
                btnModCliente.Visible = !btnInsCliente.Visible;
                lblTitoloInserimento.Text = "Inserimento Clienti";
            }
        }
        #endregion

        #region Fornitori
        protected void btnInsFornit_Click(object sender, EventArgs e)
        {
            if (txtRagSocFornit.Text != "")
            {
                Fornitori fornitore = FillObjFornitore();
                bool isInserito = FornitoriDAO.InserisciFornitore(fornitore);
                if (isInserito)
                {
                    lblIsFornitoreInserito.Text = "Fornitore '" + txtRagSocFornit.Text + "' inserito correttamente";
                    lblIsFornitoreInserito.ForeColor = Color.Blue;
                }
                else
                {
                    lblIsFornitoreInserito.Text = "Errore durante l'inserimento del cliente '" + txtRagSocFornit.Text + "'";
                    lblIsFornitoreInserito.ForeColor = Color.Red;
                }
                ResettaCampi(pnlInsFornitori);
                BindGridFornitori();
            }
            else
            {
                lblIsFornitoreInserito.Text = "Il campo 'Ragione Sociale Fornitore' deve essere compilato";
                lblIsFornitoreInserito.ForeColor = Color.Red;
            }
        }
        private Fornitori FillObjFornitore()
        {
            return new Fornitori
            {
                RagSocForni = txtRagSocFornit.Text,
                Città = txtCittaFornit.Text,
                Indirizzo = txtIndirFornit.Text,
                Cap = txtCapFornit.Text,
                Tel1 = txtTelFornit.Text == "" ? 0 : Convert.ToInt32(txtTelFornit.Text),
                Cell1 = txtCelFornit.Text == "" ? 0 : Convert.ToInt32(txtCelFornit.Text),
                CodFiscale = txtCodFiscFornit.Text,
                PartitaIva = txtPartIvaFornit.Text == "" ? 0 : Convert.ToDouble(txtPartIvaFornit.Text),
                Abbreviato = txtAbbrevFornit.Text
            };
        }
        protected void btnModFornit_Click(object sender, EventArgs e)
        {
            Fornitori fornitore = FillObjFornitore();
            fornitore.IdFornitori = Convert.ToInt32(hidIdFornit.Value);
            bool isUpdated = FornitoriDAO.UpdateFornitore(fornitore);

            if (isUpdated)
            {
                lblIsFornitoreInserito.Text = "Fornitore '" + txtRagSocFornit.Text + "' modificato con successo";
                lblIsFornitoreInserito.ForeColor = Color.Blue;
            }
            else
            {
                lblIsFornitoreInserito.Text = "Errore durante la modifica del fornitore '" + txtRagSocFornit.Text + "'";
                lblIsFornitoreInserito.ForeColor = Color.Red;
            }

            ResettaCampi(pnlInsFornitori);
            BindGridFornitori();
            btnModFornit.Visible = false;
            btnInsFornit.Visible = true;
        }
        protected void btnFiltraGrdFornitori_Click(object sender, EventArgs e)
        {
            BindGridFornitori();
        }
        protected void BindGridFornitori()
        {
            grdFornitori.DataSource = FornitoriDAO.GetFornitori(txtFiltroRagSocForni.Text);
            grdFornitori.DataBind();
        }
        protected void PopolaCampiFornitore(int idFornitore, bool isControlEnabled)
        {
            EnableDisableFields(pnlInsFornitori, isControlEnabled);

            //Popolo i textbox
            Fornitori fornitore = FornitoriDAO.GetSingle(idFornitore);
            txtRagSocFornit.Text = fornitore.RagSocForni;
            txtIndirFornit.Text = fornitore.Indirizzo;
            txtCapFornit.Text = fornitore.Cap;
            txtCittaFornit.Text = fornitore.Città;
            txtTelFornit.Text = fornitore.Tel1.ToString();
            txtCelFornit.Text = fornitore.Cell1.ToString();
            txtPartIvaFornit.Text = fornitore.PartitaIva.ToString();
            txtCodFiscFornit.Text = fornitore.CodFiscale;
            txtAbbrevFornit.Text = fornitore.Abbreviato;
        }
        protected void grdFornitori_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idFornitore = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "VisualFornit")
            {
                lblTitoloInserimento.Text = "Visualizza Fornitore";
                lblIsFornitoreInserito.Text = "";
                PopolaCampiFornitore(idFornitore, false);
                btnInsFornit.Visible = btnModFornit.Visible = false;
            }
            else if (e.CommandName == "ModFornit")
            {
                lblTitoloInserimento.Text = "Modifica Fornitore";
                lblIsFornitoreInserito.Text = "";
                btnModFornit.Visible = true;
                btnInsFornit.Visible = false;
                PopolaCampiFornitore(idFornitore, true);
                hidIdFornit.Value = idFornitore.ToString();
            }
            else if (e.CommandName == "ElimFornit")
            {
                bool isEliminato = FornitoriDAO.EliminaFornitore(idFornitore);
                if (isEliminato)
                {
                    lblIsFornitoreInserito.Text = "Fornitore eliminato con successo";
                    lblIsFornitoreInserito.ForeColor = Color.Blue;
                }
                else
                {
                    lblIsFornitoreInserito.Text = "Errore durante l'eliminazione del fornitore";
                    lblIsFornitoreInserito.ForeColor = Color.Red;
                }

                BindGridFornitori();

                ResettaCampi(pnlInsFornitori);
                btnModFornit.Visible = false;
                btnInsFornit.Visible = true;
                lblTitoloInserimento.Text = "Inserimento Fornitori";
            }
        }
        #endregion

        #region Operai
        protected void btnInsOper_Click(object sender, EventArgs e)
        {
            if (txtNomeOper.Text != "")
            {
                bool isInserito = OperaiDAO.InserisciOperaio(new Operai
                {
                    NomeOp = txtNomeOper.Text,
                    DescrOp = txtDescrOper.Text,
                    Suffisso = txtSuffOper.Text,
                    Operaio = txtOperaio.Text,
                    CostoOperaio = Convert.ToDecimal(txtCostoOperaio.Text)
                });

                if (isInserito)
                {
                    lblIsOperaioInserito.Text = "Operaio '" + txtNomeOper.Text + "' inserito con successo";
                    lblIsOperaioInserito.ForeColor = Color.Blue;
                }
                else
                {
                    lblIsOperaioInserito.Text = "Errore durante l'inserimento dell'operaio '" + txtNomeOper.Text + "'";
                    lblIsOperaioInserito.ForeColor = Color.Red;
                }

                BindGridOperai();
                ResettaCampi(pnlInsOperai);
            }
            else
            {
                lblIsOperaioInserito.Text = "Il campo 'Nome Operaio' deve essere compilato";
                lblIsOperaioInserito.ForeColor = Color.Red;
            }
        }
        protected void btnModOper_Click(object sender, EventArgs e)
        {
            bool isUpdated = OperaiDAO.UpdateOperaio(new Operai
            {
                IdOperaio = Convert.ToInt32(hidIdOper.Value),
                NomeOp = txtNomeOper.Text,
                DescrOp = txtDescrOper.Text,
                Suffisso = txtSuffOper.Text,
                Operaio = txtOperaio.Text,
                CostoOperaio = Convert.ToDecimal(txtCostoOperaio.Text)
            });

            if (isUpdated)
            {
                lblIsOperaioInserito.Text = "Operaio '" + txtDescrOper.Text + "' modificato con successo";
                lblIsOperaioInserito.ForeColor = Color.Blue;
            }
            else
            {
                lblIsOperaioInserito.Text = "Errore durante la modifica dell'operaio '" + txtDescrOper.Text + "'";
                lblIsOperaioInserito.ForeColor = Color.Red;
            }

            ResettaCampi(pnlInsOperai);
            BindGridOperai();
            btnModOper.Visible = false;
            btnInsOper.Visible = true;
        }
        protected void BindGridOperai()
        {
            grdOperai.DataSource = OperaiDAO.GetAll();
            grdOperai.DataBind();
        }
        protected void PopolaCampiOperaio(int idOperaio, bool isControlEnabled)
        {
            EnableDisableFields(pnlInsOperai, isControlEnabled);

            //Popolo i textbox
            Operai operaio = OperaiDAO.GetSingle(idOperaio);
            txtNomeOper.Text = operaio.NomeOp;
            txtDescrOper.Text = operaio.DescrOp;
            txtSuffOper.Text = operaio.Suffisso;
            txtOperaio.Text = operaio.Operaio;
            txtCostoOperaio.Text = operaio.CostoOperaio.ToString();
        }
        protected void grdOperai_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idOperaio = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "VisualOper")
            {
                lblTitoloInserimento.Text = "Visualizza Operaio";
                lblIsOperaioInserito.Text = "";
                btnInsOper.Visible = btnModOper.Visible = false;
                PopolaCampiOperaio(idOperaio, false);
            }
            else if (e.CommandName == "ModOper")
            {
                lblTitoloInserimento.Text = "Modifica Operaio";
                lblIsOperaioInserito.Text = "";
                btnModOper.Visible = true;
                btnInsOper.Visible = false;
                hidIdOper.Value = idOperaio.ToString();
                PopolaCampiOperaio(idOperaio, true);
            }
            else if (e.CommandName == "ElimOper")
            {
                bool isEliminato = OperaiDAO.EliminaOperaio(idOperaio);
                if (isEliminato)
                {
                    lblIsOperaioInserito.Text = "Operaio eliminato con successo";
                    lblIsOperaioInserito.ForeColor = Color.Blue;
                }
                else
                {
                    lblIsOperaioInserito.Text = "Errore durante l'eliminazione dell'operaio, verificare che non sia presente tra i materiali cantieri";
                    lblIsOperaioInserito.ForeColor = Color.Red;
                }

                BindGridOperai();
                ResettaCampi(pnlInsOperai);
                btnInsOper.Visible = true;
                btnModOper.Visible = !btnInsOper.Visible;
                lblTitoloInserimento.Text = "Inserimento Operai";
            }
        }
        #endregion

        #region Cantieri
        protected void btnInsCantiere_Click(object sender, EventArgs e)
        {
            if (ddlScegliClientePerCantiere.SelectedIndex != 0)
            {
                bool isInserito = CantieriDAO.InserisciCantiere(new Cantieri
                {
                    IdtblClienti = Convert.ToInt32(ddlScegliClientePerCantiere.SelectedValue),
                    Data = Convert.ToDateTime(txtDataInserCant.Text),
                    CodCant = txtCodCant.Text,
                    DescriCodCant = txtDescrCodCant.Text,
                    //Indirizzo = txtIndirizzoCant.Text,
                    //Città = txtCittaCant.Text,
                    Ricarico = Convert.ToInt32(txtRicaricoCant.Text),
                    PzzoManodopera = Convert.ToDecimal(txtPzzoManodopCant.Text),
                    Chiuso = chkCantChiuso.Checked,
                    Riscosso = chkCantRiscosso.Checked,
                    Numero = Convert.ToInt32(txtNumeroCant.Text),
                    ValorePreventivo = Convert.ToDecimal(txtValPrevCant.Text == "" ? "0" : txtValPrevCant.Text),
                    Iva = Convert.ToInt32(txtIvaCant.Text),
                    Anno = Convert.ToInt32(txtAnnoCant.Text),
                    Preventivo = chkPreventivo.Checked,
                    DaDividere = chkDaDividere.Checked,
                    Diviso = chkDiviso.Checked,
                    Fatturato = chkFatturato.Checked,
                    NonRiscuotibile = chkNonRiscuotibile.Checked,
                    FasciaTblCantieri = Convert.ToInt32(txtFasciaCant.Text),
                    //CodRiferCant = CostruisciCodRiferCant()
                    CodRiferCant = txtCodiceRiferimentoCant.Text
                });

                if (isInserito)
                {
                    lblIsCantInserito.Text = "Cantiere '" + txtDescrCodCant.Text + "' inserito con successo";
                    lblIsCantInserito.ForeColor = Color.Blue;
                }
                else
                {
                    lblIsCantInserito.Text = "Errore durante l'inserimento del cantiere '" + txtDescrCodCant.Text + "'";
                    lblIsCantInserito.ForeColor = Color.Red;
                }

                BindGridCantieri();
                ResettaCampi(pnlTxtBoxCantContainer);
                txtAnnoCant.Text = txtAnnoCant.Text != "" ? txtAnnoCant.Text : DateTime.Now.Year.ToString();
                PopolaCodCantAnnoNumero(txtAnnoCant.Text);
            }
            else
            {
                lblIsCantInserito.Text = "Devi scegliere un cliente da associare al nuovo cantiere";
                lblIsCantInserito.ForeColor = Color.Red;
            }
        }
        protected void btnModCantiere_Click(object sender, EventArgs e)
        {
            bool isUpdated = CantieriDAO.UpdateCantiere(new Cantieri
            {
                IdCantieri = Convert.ToInt32(hidIdCant.Value),
                IdtblClienti = Convert.ToInt32(ddlScegliClientePerCantiere.SelectedValue),
                Data = Convert.ToDateTime(txtDataInserCant.Text),
                CodCant = txtCodCant.Text,
                DescriCodCant = txtDescrCodCant.Text,
                //Indirizzo = txtIndirizzoCant.Text,
                //Città = txtCittaCant.Text,
                Ricarico = Convert.ToInt32(txtRicaricoCant.Text),
                PzzoManodopera = Convert.ToDecimal(txtPzzoManodopCant.Text),
                Chiuso = chkCantChiuso.Checked,
                Riscosso = chkCantRiscosso.Checked,
                Numero = Convert.ToInt32(txtNumeroCant.Text),
                ValorePreventivo = Convert.ToDecimal(txtValPrevCant.Text == "" ? "0" : txtValPrevCant.Text),
                Iva = Convert.ToInt32(txtIvaCant.Text),
                Anno = Convert.ToInt32(txtAnnoCant.Text),
                Preventivo = chkPreventivo.Checked,
                DaDividere = chkDaDividere.Checked,
                Diviso = chkDiviso.Checked,
                Fatturato = chkFatturato.Checked,
                NonRiscuotibile = chkNonRiscuotibile.Checked,
                FasciaTblCantieri = Convert.ToInt32(txtFasciaCant.Text),
                CodRiferCant = txtCodiceRiferimentoCant.Text
                //CodRiferCant = CostruisciCodRiferCant()
            });

            if (isUpdated)
            {
                lblIsCantInserito.Text = "Cantiere '" + txtDescrCodCant.Text + "' modificato con successo";
                lblIsCantInserito.ForeColor = Color.Blue;
            }
            else
            {
                lblIsCantInserito.Text = "Errore durante la modifica del cantiere '" + txtDescrCodCant.Text + "'";
                lblIsCantInserito.ForeColor = Color.Red;
            }

            BindGridCantieri();
            ResettaCampi(pnlTxtBoxCantContainer);
            PopolaCodCantAnnoNumero(txtAnnoCant.Text != "" ? txtAnnoCant.Text : DateTime.Now.Year.ToString());
            btnModCantiere.Visible = false;
            btnInsCantiere.Visible = true;
        }
        protected void btnFiltraCant_Click(object sender, EventArgs e)
        {
            BindGridCantieri();
        }
        protected void btnSvuotaFiltri_Click(object sender, EventArgs e)
        {
            chkFiltroChiuso.Checked = chkFiltroRiscosso.Checked = chkFiltroFatturato.Checked = chkFiltroNonRiscuotibile.Checked = false;
            ResettaCampi(pnlFiltriCant);
            BindGridCantieri();
        }
        protected void BindGridCantieri()
        {
            grdCantieri.DataSource = CantieriDAO.GetCantieri(txtFiltroAnno.Text, txtFiltroCodCant.Text, txtFiltroDescr.Text, txtFiltroCliente.Text, chkFiltroChiuso.Checked, chkFiltroRiscosso.Checked, chkFiltroFatturato.Checked, chkFiltroNonRiscuotibile.Checked);
            grdCantieri.DataBind();
        }
        protected void FillDdlClientiPerCantieri(string ragSocCli = "")
        {
            ddlScegliClientePerCantiere.Items.Clear();
            ddlScegliClientePerCantiere.Items.Add(new ListItem("", "-1"));
            ClientiDAO.GetClienti(ragSocCli).ForEach(f =>
            {
                ddlScegliClientePerCantiere.Items.Add(new ListItem(f.RagSocCli, f.IdCliente.ToString()));
            });
        }
        protected void PopolaCampiCantiere(int idCant, bool isControlEnabled)
        {
            EnableDisableFields(pnlTxtBoxCantContainer, isControlEnabled);

            //Deseleziono tutti gli elementi della dropdownlist
            foreach (ListItem item in ddlScegliClientePerCantiere.Items)
            {
                item.Selected = false;
            }

            Cantieri cant = CantieriDAO.GetSingle(idCant);

            // Seleziono il cliente con la Ragione Sociale associata al cantiere di riferimento
            ddlScegliClientePerCantiere.SelectedValue = ddlScegliClientePerCantiere.Items.FindByText(cant.RagSocCli).Value;

            //Popolo i textbox
            txtDataInserCant.Text = cant.Data.ToString("yyyy-MM-dd");
            txtDataInserCant.TextMode = TextBoxMode.Date;
            txtCodCant.Text = cant.CodCant;
            txtDescrCodCant.Text = cant.DescriCodCant;
            txtCodiceRiferimentoCant.Text = cant.CodRiferCant;
            //txtIndirizzoCant.Text = cant.Indirizzo;
            //txtCittaCant.Text = cant.Città;
            txtRicaricoCant.Text = cant.Ricarico.ToString();
            txtPzzoManodopCant.Text = cant.PzzoManodopera.ToString("N2");
            txtNumeroCant.Text = cant.Numero.ToString();
            txtValPrevCant.Text = cant.ValorePreventivo.ToString("N2");
            txtIvaCant.Text = cant.Iva.ToString();
            txtAnnoCant.Text = cant.Anno.ToString();
            txtFasciaCant.Text = cant.FasciaTblCantieri.ToString();
            txtConcatenazioneCant.Text = $"{cant.CodCant}-{cant.DescriCodCant}";

            //Spunto i checkbox se necessario
            chkCantChiuso.Checked = cant.Chiuso;
            chkCantRiscosso.Checked = cant.Riscosso;
            chkPreventivo.Checked = cant.Preventivo;
            chkDaDividere.Checked = cant.DaDividere;
            chkDiviso.Checked = cant.Diviso;
            chkFatturato.Checked = cant.Fatturato;
            chkNonRiscuotibile.Checked = cant.NonRiscuotibile;
        }
        protected void PopolaCodCantAnnoNumero(string anno, string num = "")
        {
            string numCant = "";
            if (num == "")
            {
                List<Cantieri> items = CantieriDAO.GetAll().Where(w => w.Anno == Convert.ToInt32(anno)).ToList();
                if (items.Count > 0)
                {
                    txtNumeroCant.Text = (items.Select(s => s.Numero).Max() + 1).ToString() ?? "";
                }
                else
                {
                    txtNumeroCant.Text = "001";
                }
                numCant = txtNumeroCant.Text;
            }
            else
            {
                numCant = num;
            }

            if (numCant.Length == 1)
            {
                numCant = "00" + numCant;
            }
            else if (numCant.Length == 2)
            {
                numCant = "0" + numCant;
            }
            txtCodCant.Text = anno.Substring(2, 2) + numCant + "Ma";
        }
        protected string CostruisciCodRiferCant()
        {
            DateTime date = DateTime.Now;
            int numCant = CantieriDAO.GetAll().Where(w => w.Anno == Convert.ToInt32(txtAnnoCant.Text)).Count();
            int descrLength = txtDescrCodCant.Text.Trim().Length;
            string firstTwoDescrCodCant = txtDescrCodCant.Text.Substring(0, 2);
            string lastYearDigits = date.Year.ToString().Substring(2, 2);
            string firstTwoRagSocCli = ddlScegliClientePerCantiere.SelectedItem.Text.Substring(0, 2);
            string codRiferCant = Convert.ToString(numCant + descrLength) + firstTwoDescrCodCant + lastYearDigits + firstTwoRagSocCli;
            return codRiferCant.Replace(" ", "-").ToUpper();
        }
        protected void grdCantieri_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idCant = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "VisualCant")
            {
                lblTitoloInserimento.Text = "Visualizza Cantiere";
                lblIsCantInserito.Text = "";
                btnInsCantiere.Visible = btnModCantiere.Visible = false;
                PopolaCampiCantiere(idCant, false);
            }
            else if (e.CommandName == "ModCant")
            {
                lblTitoloInserimento.Text = "Modifica Cantiere";
                lblIsCantInserito.Text = "";
                btnInsCantiere.Visible = false;
                btnModCantiere.Visible = !btnInsCantiere.Visible;
                hidIdCant.Value = idCant.ToString();
                PopolaCampiCantiere(idCant, true);
            }
            else if (e.CommandName == "ElimCant")
            {
                if (PagamentiDAO.GetAll().Where(w => w.IdTblCantieri == Convert.ToInt32(idCant)).Count() == 0)
                {
                    bool isDeleted = CantieriDAO.EliminaCantiere(idCant);

                    if (isDeleted)
                    {
                        lblIsCantInserito.Text = "Cantiere eliminato con successo";
                        lblIsCantInserito.ForeColor = Color.Blue;
                    }
                    else
                    {
                        lblIsCantInserito.Text = "Errore durante l'eliminazione del cantiere";
                        lblIsCantInserito.ForeColor = Color.Red;
                    }
                    BindGridCantieri();
                    ResettaCampi(pnlTxtBoxCantContainer);
                    txtCodCant.Enabled = false;
                    btnInsCantiere.Visible = true;
                    btnModCantiere.Visible = !btnInsCantiere.Visible;
                    lblTitoloInserimento.Text = "Inserimento Cantieri";
                }
                else
                {
                    lblIsCantInserito.Text = "Impossibile eliminare il cantiere selezionato perchè ha dei pagamenti associati";
                    lblIsCantInserito.ForeColor = Color.Red;
                }
            }
        }
        #endregion

        #region Spese
        protected void btnInsSpesa_Click(object sender, EventArgs e)
        {
            if (txtSpeseDescr.Text != "")
            {
                if (txtSpesePrezzo.Text != "")
                {
                    bool isInserito = SpeseDAO.InsertSpesa(new Spese
                    {
                        Descrizione = txtSpeseDescr.Text,
                        Prezzo = Convert.ToDecimal(txtSpesePrezzo.Text)
                    });

                    if (isInserito)
                    {
                        lblIsSpesaInserita.Text = "Spesa '" + txtSpeseDescr.Text + "' inserita con successo";
                        lblIsSpesaInserita.ForeColor = Color.Blue;
                    }
                    else
                    {
                        lblIsSpesaInserita.Text = "Errore durante l'inserimento della spesa '" + txtSpeseDescr.Text + "'";
                        lblIsSpesaInserita.ForeColor = Color.Red;
                    }

                    BindGridSpese();
                    ResettaCampi(pnlInsSpese);
                }
                else
                {
                    lblIsSpesaInserita.Text = "È necessario inserire un valore nel campo \"Prezzo\"";
                    lblIsSpesaInserita.ForeColor = Color.Red;
                }
            }
            else
            {
                lblIsSpesaInserita.Text = "Il campo 'Descrizione' deve essere compilato";
                lblIsSpesaInserita.ForeColor = Color.Red;
            }
        }
        protected void btnModSpesa_Click(object sender, EventArgs e)
        {
            bool isUpdated = SpeseDAO.UpdateSpesa(new Spese
            {
                IdSpesa = Convert.ToInt32(hidSpese.Value),
                Descrizione = txtSpeseDescr.Text,
                Prezzo = Convert.ToDecimal(txtSpesePrezzo.Text)
            });

            if (isUpdated)
            {
                lblIsSpesaInserita.Text = "Spesa '" + txtSpeseDescr.Text + "' modificata con successo";
                lblIsSpesaInserita.ForeColor = Color.Blue;
            }
            else
            {
                lblIsSpesaInserita.Text = "Errore durante la modifica della spesa '" + txtSpeseDescr.Text + "'";
                lblIsSpesaInserita.ForeColor = Color.Red;
            }

            ResettaCampi(pnlInsSpese);
            BindGridSpese();
            btnModSpesa.Visible = false;
            btnInsSpesa.Visible = true;
        }
        protected void btnFiltraGrdSpese_Click(object sender, EventArgs e)
        {
            BindGridSpese();
        }
        private void BindGridSpese()
        {
            grdSpese.DataSource = SpeseDAO.GetByDescription(txtFiltroSpesaDescr.Text);
            grdSpese.DataBind();
        }
        protected void PopolaCampiSpesa(int idSpesa, bool isControlEnabled)
        {
            EnableDisableFields(pnlInsSpese, isControlEnabled);

            //Popolo i textbox
            Spese sp = SpeseDAO.GetDettagliSpesa(idSpesa.ToString());
            txtSpeseDescr.Text = sp.Descrizione;
            txtSpesePrezzo.Text = sp.Prezzo.ToString("N2");
        }
        protected void grdSpese_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idSpesa = Convert.ToInt32(e.CommandArgument.ToString());
            if (e.CommandName == "VisualSpesa")
            {
                lblTitoloInserimento.Text = "Visualizza Spesa";
                lblIsSpesaInserita.Text = "";
                btnInsSpesa.Visible = btnModSpesa.Visible = false;
                PopolaCampiSpesa(idSpesa, false);
            }
            else if (e.CommandName == "ModSpesa")
            {
                lblTitoloInserimento.Text = "Modifica Spesa";
                lblIsSpesaInserita.Text = "";
                btnModSpesa.Visible = true;
                btnInsSpesa.Visible = false;
                hidSpese.Value = idSpesa.ToString();
                PopolaCampiSpesa(idSpesa, true);
            }
            else if (e.CommandName == "ElimSpesa")
            {
                bool isEliminato = SpeseDAO.DeleteSpesa(idSpesa);
                if (isEliminato)
                {
                    lblIsSpesaInserita.Text = "Spesa eliminata con successo";
                    lblIsSpesaInserita.ForeColor = Color.Blue;
                }
                else
                {
                    lblIsSpesaInserita.Text = "Errore durante l'eliminazione della spesa";
                    lblIsSpesaInserita.ForeColor = Color.Red;
                }

                BindGridSpese();

                ResettaCampi(pnlInsSpese);
                btnModSpesa.Visible = false;
                btnInsSpesa.Visible = true;
                lblTitoloInserimento.Text = "Inserimento Spese";
            }
        }
        #endregion

        #region Mostra/Nascondi Pannelli
        protected void btnShowInsClienti_Click(object sender, EventArgs e)
        {
            lblTitoloInserimento.Text = "Inserimento Clienti";
            lblIsClienteInserito.Text = "";
            MostraPannello(true, false, false, false, false);
            BindGridClienti();
            ResettaCampi(pnlInsClienti);
            btnInsCliente.Visible = true;
            btnModCliente.Visible = false;
        }
        protected void btnShowInsFornitori_Click(object sender, EventArgs e)
        {
            BindGridFornitori();
            lblTitoloInserimento.Text = "Inserimento Fornitori";
            lblIsFornitoreInserito.Text = "";
            MostraPannello(false, true, false, false, false);
            ResettaCampi(pnlInsFornitori);
            btnInsFornit.Visible = true;
            btnModFornit.Visible = false;
        }
        protected void btnShowInsOperai_Click(object sender, EventArgs e)
        {
            BindGridOperai();
            lblTitoloInserimento.Text = "Inserimento Operai";
            lblIsOperaioInserito.Text = "";
            MostraPannello(false, false, true, false, false);
            ResettaCampi(pnlInsOperai);
            btnInsOper.Visible = true;
            btnModOper.Visible = false;
        }
        protected void btnShowInsCantieri_Click(object sender, EventArgs e)
        {
            MostraPannello(false, false, false, true, false);
            lblTitoloInserimento.Text = "Inserimento Cantieri";
            lblIsCantInserito.Text = "";
            BindGridCantieri();
            FillDdlClientiPerCantieri();
            ResettaCampi(pnlTxtBoxCantContainer);
            txtAnnoCant.Text = DateTime.Now.Year.ToString();
            PopolaCodCantAnnoNumero(txtAnnoCant.Text);
            txtCodCant.Enabled = false;
            btnModCantiere.Visible = false;
            btnInsCantiere.Visible = true;
        }
        protected void btnShowInsSpese_Click(object sender, EventArgs e)
        {
            MostraPannello(false, false, false, false, true);
            lblTitoloInserimento.Text = "Inserimento Spese";
            lblIsSpesaInserita.Text = "";
            BindGridSpese();
            ResettaCampi(pnlInsSpese);
            btnModSpesa.Visible = false;
            btnInsSpesa.Visible = true;
        }
        #endregion

        #region Helpers
        protected void MostraPannello(bool pnlClienti, bool pnlFornitori, bool pnlOperai, bool pnlCantieri, bool pnlSpese)
        {
            pnlInsClienti.Visible = pnlClienti;
            pnlInsFornitori.Visible = pnlFornitori;
            pnlInsOperai.Visible = pnlOperai;
            pnlInsCantieri.Visible = pnlCantieri;
            pnlInsSpese.Visible = pnlSpese;
        }
        protected void ResettaCampi(Panel container)
        {
            foreach (Control control in container.Controls)
            {
                if (control is TextBox textBox)
                {
                    textBox.Text = string.Empty;
                    textBox.Enabled = true;
                }
                if (control is CheckBox checkBox)
                {
                    checkBox.Checked = false;
                    checkBox.Enabled = true;
                }
                if (control is DropDownList dropDown)
                {
                    dropDown.Enabled = true;
                    dropDown.SelectedIndex = 0;
                }
            }
        }
        protected void EnableDisableFields(Panel panel, bool enableFields)
        {
            foreach (Control c in panel.Controls)
            {
                if (c is TextBox textBox)
                {
                    textBox.Enabled = enableFields;
                }
                else if (c is DropDownList ddl)
                {
                    ddl.Enabled = enableFields;
                }
                else if (c is CheckBox chkBox)
                {
                    chkBox.Enabled = enableFields;
                }
            }
        }
        #endregion

        #region EVENTI TEXT-CHANGED 
        protected void txtAnnoCant_TextChanged(object sender, EventArgs e)
        {
            PopolaCodCantAnnoNumero(txtAnnoCant.Text);
        }
        protected void txtFiltroClientePerInserimentoCantieri_TextChanged(object sender, EventArgs e)
        {
            FillDdlClientiPerCantieri(txtFiltroClientePerInserimentoCantieri.Text);
        }
        #endregion
    }
}
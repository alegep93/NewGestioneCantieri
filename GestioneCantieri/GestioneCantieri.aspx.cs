using Database.DAO;
using Database.Models;
using GestioneCantieri.Utils;
using iText.IO.Font.Constants;
using iText.Kernel.Font;
using iText.Kernel.Geom;
using iText.Kernel.Pdf;
using iText.Layout;
using iText.Layout.Element;
using iText.Layout.Properties;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class GestioneCantieri : Page
    {
        readonly string pdfProtocolloBasePath = ConfigurationManager.AppSettings["PdfProtocollo"];

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FillAllDdl();
                ShowPanels(false, false, false, false, false, false, false, false);
                pnlSubIntestazione.Visible = pnlMascheraGestCant.Visible = false;
                grdMatCant.Visible = grdRientro.Visible = false;
                btnModMatCant.Visible = btnModRientro.Visible = false;
            }
            Page.MaintainScrollPositionOnPostBack = true;
        }

        #region Helpers 
        protected void SvuotaIntestazione()
        {
            string tipol = txtTipDatCant.Text;

            //Svuoto tutti i TextBox
            foreach (Control c in pnlFiltriSceltaCant.Controls)
            {
                if (c is TextBox box)
                {
                    box.Text = "";
                }

                if (c is CheckBox chk)
                {
                    chk.Checked = false;
                }
            }
            foreach (Control c in pnlSubIntestazione.Controls)
            {
                if (c is TextBox box)
                {
                    box.Text = "";
                }

                if (ddlScegliDDTMef.SelectedIndex != -1)
                {
                    ddlScegliDDTMef.SelectedIndex = 0;
                }

                ddlScegliFornit.SelectedIndex = 0;
            }

            //Textbox Tipologia sempre Disabilitato
            txtTipDatCant.Enabled = false;
            txtTipDatCant.Text = tipol;
        }
        protected void SvuotaCampi(Panel pnl)
        {
            //Svuoto tutti i TextBox
            foreach (Control c in pnl.Controls)
            {
                if (c is TextBox box)
                {
                    box.Text = "";
                }
            }

            //Svuoto il DDL del listino solamente se è stato popolato
            if (ddlScegliListino.SelectedIndex != -1)
            {
                ddlScegliListino.SelectedIndex = 0;
            }

            //Visibile TRUE
            chkVisibile.Checked = chkManodopVisibile.Checked = chkOperVisibile.Checked = chkChiamVisibile.Checked = true;
            //Ricalcolo TRUE
            chkRicalcolo.Checked = true;
            //RicaricoSiNo TRUE
            chkRicarico.Checked = chkOperRicaricoSiNo.Checked = true;

            //Visibile FALSE
            chkArrotVisibile.Checked = false;
            //Ricalcolo FALSE
            chkManodopRicalcolo.Checked = chkOperRicalcolo.Checked = chkArrotRicalcolo.Checked = false;
            //RicaricoSiNo FALSE
            chkManodopRicaricoSiNo.Checked = chkArrotRicaricoSiNo.Checked = false;

            //Reimposto i textbox ai valori di default
            txtQta.Text = txtManodopQta.Text = txtOperQta.Text = txtArrotQta.Text = txtChiamQta.Text = txtSpesaPrezzo.Text = "";
            txtPzzoUnit.Text = txtChiamPzzoUnit.Text = txtSpesaPrezzoCalcolato.Text = "0.00";

            //Textbox Tipologia sempre Disabilitato
            txtTipDatCant.Enabled = false;

            //Reimposto il pzzoFinCli
            txtPzzoFinCli.Text = txtChiamPzzoFinCli.Text = "0.00";

            //Reimposto il campo Prezzo manodopera
            txtPzzoManodop.Text = CantieriDAO.GetSingle(Convert.ToInt32(ddlScegliCant.SelectedItem.Value))?.PzzoManodopera.ToString("N2") ?? "0";

            //Reimposto il DDLScegliOperaio del pannello GestioneOperaio
            ddlScegliOperaio.SelectedIndex = 0;

            //Reimposto il ddlScegliDDTMef dell'intestazione
            if (ddlScegliDDTMef.SelectedIndex != -1)
            {
                ddlScegliDDTMef.SelectedIndex = 0;
            }

            grdMostraDDTDaInserire.DataSource = null;
            grdMostraDDTDaInserire.DataBind();
        }
        protected void EnableDisableControls(bool enableControls, Panel panelName)
        {
            foreach (Control c in pnlIntestazione.Controls)
            {
                if (c is TextBox box)
                {
                    box.Enabled = enableControls;
                }
                else if (c is DropDownList ddl)
                {
                    ddl.Enabled = enableControls;
                }
            }
            foreach (Control c in pnlSubIntestazione.Controls)
            {
                if (c is TextBox box)
                {
                    box.Enabled = enableControls;
                }
                else if (c is DropDownList ddl)
                {
                    ddl.Enabled = enableControls;
                }
            }
            foreach (Control c in panelName.Controls)
            {
                if (c is TextBox box)
                {
                    box.Enabled = enableControls;
                }
                else if (c is DropDownList ddl)
                {
                    ddl.Enabled = enableControls;
                }
            }

            //Textbox Tipologia sempre Disabilitato
            txtTipDatCant.Enabled = false;
        }
        protected void ChooseFornitore(string nomeFornitore)
        {
            int i = 0;
            if (nomeFornitore == "")
            {
                ddlScegliFornit.SelectedIndex = 0;
            }
            else
            {
                foreach (System.Web.UI.WebControls.ListItem li in ddlScegliFornit.Items)
                {
                    if (li.Text == nomeFornitore)
                    {
                        ddlScegliFornit.SelectedIndex = i;
                        return;
                    }
                    i++;
                }
            }
        }
        protected void BindAllGrid()
        {
            BindGridMatCant();
            BindGridManodop();
            BindGridOper();
            BindGridArrot();
            BindGridChiamata();
            BindGridSpese();
        }
        protected void HideMessageLabels()
        {
            lblIsRecordInserito.Text = lblIsManodopInserita.Text = lblIsOperInserita.Text =
               lblIsArrotondInserito.Text = lblIsSpesaInserita.Text = lblIsAChiamInserita.Text = lblInsMatDaDDT.Text = "";
        }
        protected void FillDdlScegliDdtMef()
        {
            ddlScegliDDTMef.Items.Clear();
            ddlScegliDDTMef.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlDdtMef(DDTMefDAO.GetByAnnoNumeroDdt(txtFiltroAnnoDDT.Text, txtFiltroN_DDT.Text), ref ddlScegliDDTMef);
        }
        protected void FillDddlScegliListino()
        {
            ddlScegliListino.Items.Clear();
            ddlScegliListino.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlMamg0(Mamg0DAO.GetListino(txtFiltroCodFSS.Text, "", "", txtFiltroAA_Des.Text, "", ""), ref ddlScegliListino);
        }
        protected void FillAllDdl(bool refreshCantieri = true)
        {
            if (refreshCantieri)
            {
                // Cantieri
                ddlScegliCant.Items.Clear();
                ddlScegliCant.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
                DropDownListManager.FillDdlCantieri(CantieriDAO.GetCantieri(txtFiltroCantAnno.Text, txtFiltroCantCodCant.Text, txtFiltroCantDescrCodCant.Text, chkFiltroCantChiuso.Checked, chkFiltroCantRiscosso.Checked), ref ddlScegliCant);
            }

            // Operaio / Acquirente
            List<Operai> items = OperaiDAO.GetAll();
            ddlScegliAcquirente.Items.Clear();
            ddlScegliAcquirente.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            ddlScegliOperaio.Items.Clear();
            ddlScegliOperaio.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlOperaio(items, ref ddlScegliAcquirente);
            DropDownListManager.FillDdlOperaio(items, ref ddlScegliOperaio);

            // Fornitori
            ddlScegliFornit.Items.Clear();
            ddlScegliFornit.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlFornitore(FornitoriDAO.GetFornitori(), ref ddlScegliFornit);

            // Rientro
            ddlScegliMatCant.Items.Clear();
            ddlScegliMatCant.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlMaterialiCantieri(MaterialiCantieriDAO.GetMaterialeCantiere(ddlScegliCant.SelectedItem.Value, txtFiltroMatCantCodArt.Text, txtFiltroMatCantDescriCodArt.Text), ref ddlScegliMatCant);

            // Spese
            ddlScegliSpesa.Items.Clear();
            ddlScegliSpesa.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlSpese(SpeseDAO.GetAll(), ref ddlScegliSpesa);
        }
        protected void ShowPanels(bool pnlMatDaDDT, bool pnlMatCant, bool pnlManodop, bool pnlOper, bool pnlArrotond, bool pnlSpese, bool pnlChiam, bool pnlAccrediti, bool pnlAltriFornitoriDaDdt = false)
        {
            pnlMascheraMaterialiDaDDT.Visible = pnlMatDaDDT;
            pnlMascheraGestCant.Visible = pnlMatCant;
            pnlManodopera.Visible = pnlManodop;
            pnlGestioneOperaio.Visible = pnlOper;
            pnlGestArrotond.Visible = pnlArrotond;
            pnlGestSpese.Visible = pnlSpese;
            pnlGestChiamata.Visible = pnlChiam;
            pnlMascheraAccrediti.Visible = pnlAccrediti;
            pnlMateAltriFornitoriDaDdt.Visible = pnlAltriFornitoriDaDdt;
        }
        protected bool IsIntestazioneCompilata()
        {
            return ddlScegliCant.SelectedIndex != 0 && ddlScegliAcquirente.SelectedIndex != 0 && ddlScegliFornit.SelectedIndex != 0 &&
                (ddlScegliDDTMef.SelectedIndex != 0 || txtNumBolla.Text != "") && txtDataDDT.Text != "" && txtFascia.Text != "" && txtProtocollo.Text != "";
        }
        protected bool IsDateNotSet()
        {
            bool ret = false;
            if (txtDataDDT.Text == "")
            {
                (Master as layout).SetModal($"Inserire un valore per la data");
                ret = true;
            }
            return ret;
        }
        private void CalcolaTotaleValore(List<MaterialiCantieri> mcList)
        {
            lblTotaleValoreMatCant_Rientro.Text = $"Totale Valore: {mcList.Sum(s => (decimal)s.Qta * s.PzzoUniCantiere):N2} €";
        }
        private void GeneraNumeroBolla()
        {
            if (txtDataDDT.Text != "")
            {
                DateTime date = Convert.ToDateTime(txtDataDDT.Text);
                txtNumBolla.Text = date.Year.ToString() + date.Month.ToString() + date.Day.ToString() + txtProtocollo.Text;
            }
        }
        private void GeneraNuovoProtocollo(Cantieri cant)
        {
            List<MaterialiCantieri> items = MaterialiCantieriDAO.GetByIdCantiere(cant.IdCantieri);
            txtProtocollo.Text = (items.Count > 0 ? items.Select(s => s.ProtocolloInterno).Max() + 1 : 1).ToString();
            txtDataDDT.Text = txtNumBolla.Text = "";
        }
        #endregion

        #region Eventi click
        protected void btnFiltroCant_Click(object sender, EventArgs e)
        {
            FillAllDdl();
            pnlSubIntestazione.Visible = false;
            txtDataDDT.Text = txtNumBolla.Text = txtProtocollo.Text = "";
            ShowPanels(false, false, false, false, false, false, false, false);
            lblTitoloMaschera.Text = "";
        }
        protected void btnSvuotaIntestazione_Click(object sender, EventArgs e)
        {
            SvuotaIntestazione();
        }
        protected void btnCalcolaPrezzoUnit_Click(object sender, EventArgs e)
        {
            if (txtPzzoNettoMef.Text != "")
            {
                txtPzzoUnit.Text = Math.Round(Convert.ToDecimal(txtPzzoNettoMef.Text.Replace(".", ",")), 2).ToString();
            }
            else
            {
                (Master as layout).SetModal($"Inserire un valore nella casella \"Prezzo Netto Mef\" per calcolare il \"Prezzo Unitario\"");
            }

            if (txtTipDatCant.Text == "MATERIALE")
            {
                btnInserisciMatCant.Focus();
            }
            else if (txtTipDatCant.Text == "RIENTRO")
            {
                btnInserisciRientro.Focus();
            }
        }
        protected void btnNuovoProtocollo_Click(object sender, EventArgs e)
        {
            Cantieri cant = CantieriDAO.GetSingle(Convert.ToInt32(ddlScegliCant.SelectedItem.Value));
            GeneraNuovoProtocollo(cant);
        }
        protected void btnStampaProtocollo_Click(object sender, EventArgs e)
        {
            try
            {
                Cantieri cant = CantieriDAO.GetSingle(Convert.ToInt32(ddlScegliCant.SelectedItem.Value));
                string numeroProtocolloCompleto = GeneraNumeroProtocollo(txtProtocollo.Text);
                string fileName = $"{cant.CodCant}-{cant.DescriCodCant}_{numeroProtocolloCompleto}.pdf";
                string filePath = System.IO.Path.Combine(pdfProtocolloBasePath, fileName);
                FileInfo file = new FileInfo(filePath);
                file.Directory.Create();
                CreatePdf(filePath, fileName, cant);

                lblMsg.Text = $"Stampa del protocollo avvenuta con successo";
                lblMsg.ForeColor = Color.DarkBlue;
            }
            catch (Exception ex)
            {
                (Master as layout).SetModal($"Errore durante la stampa del protocollo {ex.Message}");
            }
        }
        #endregion

        #region Eventi text-changed
        protected void ddlScegliCant_TextChanged(object sender, EventArgs e)
        {
            Cantieri cant = CantieriDAO.GetSingle(Convert.ToInt32(ddlScegliCant.SelectedItem.Value));

            if (ddlScegliCant.SelectedIndex != 0)
            {
                pnlSubIntestazione.Visible = true;
                txtPzzoManodop.Text = cant.PzzoManodopera.ToString("N2");
                txtFascia.Text = cant.FasciaTblCantieri.ToString();

                Preventivo preventivo = PreventiviDAO.GetSingle(cant.IdPreventivo);
                txtPreventivoCantiere.Text = preventivo != null ? $"{preventivo.Numero} - {preventivo.Descrizione}" : "";
            }
            else
            {
                pnlSubIntestazione.Visible = false;
                pnlMascheraGestCant.Visible = false;
            }
            GeneraNuovoProtocollo(cant);

            FillAllDdl(false);
            BindAllGrid();
        }
        protected void txtFiltroAnnoDDT_TextChanged(object sender, EventArgs e)
        {
            FillDdlScegliDdtMef();
        }
        protected void txtFiltroN_DDT_TextChanged(object sender, EventArgs e)
        {
            FillDdlScegliDdtMef();
        }
        protected void ddlScegliDDTMef_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliDDTMef.SelectedIndex != 0)
            {
                txtNumBolla.Enabled = false;
                BindGridMatDaDDT();
            }
            else
            {
                txtNumBolla.Enabled = true;
            }
        }
        protected void txtDataDDT_TextChanged(object sender, EventArgs e)
        {
            GeneraNumeroBolla();
        }
        protected void txtProtocollo_TextChanged(object sender, EventArgs e)
        {
            GeneraNumeroBolla();
        }
        #endregion

        #region Materiali da DDT
        /* HELPERS */
        private void BindGridMatDaDDT()
        {
            if (ddlScegliDDTMef.SelectedItem != null && ddlScegliDDTMef.SelectedItem.Text != "" && ddlScegliDDTMef.SelectedIndex != 0)
            {
                int nDDT = Convert.ToInt32(ddlScegliDDTMef.SelectedItem.Text.Split('-')[1].Trim());
                List<DDTMef> ddtList = DDTMefDAO.GetAll().Where(w => w.N_DDT == nDDT).ToList();
                grdMostraDDTDaInserire.DataSource = ddtList;
                grdMostraDDTDaInserire.DataBind();

                if (ddtList.Count > 0)
                {
                    btnInsMatDaDDT.Enabled = true;
                }
            }
        }

        /* EVENTI CLICK*/
        protected void btnMatCantFromDDT_Click(object sender, EventArgs e)
        {
            lblTitoloMaschera.Text = "Materiali da DDT";
            txtTipDatCant.Text = "MATERIALE";
            grdMostraDDTDaInserire.Visible = true;
            ShowPanels(true, false, false, false, false, false, false, false);
            grdMatCant.Visible = true;
            grdRientro.Visible = false;
            BindGridMatCant();
            BindGridMatDaDDT();
            EnableDisableControls(true, pnlMascheraMaterialiDaDDT);
            SvuotaCampi(pnlMascheraMaterialiDaDDT);
            ChooseFornitore("Mef");
            HideMessageLabels();
            txtFiltroAnnoDDT.Text = txtFiltroN_DDT.Text = "";

            if (grdMostraDDTDaInserire.Rows.Count == 0)
            {
                btnInsMatDaDDT.Enabled = false;
            }
        }
        protected void btnInsMatDaDDT_Click(object sender, EventArgs e)
        {
            if (txtProtocollo.Text != "")
            {
                for (int i = 0; i < grdMostraDDTDaInserire.Rows.Count; i++)
                {
                    if (((CheckBox)grdMostraDDTDaInserire.Rows[i].FindControl("chkDDTSelezionato")).Checked)
                    {
                        MaterialiCantieriDAO.InserisciMaterialeCantiere(new MaterialiCantieri
                        {
                            IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                            DescriMateriali = grdMostraDDTDaInserire.Rows[i].Cells[3].Text,
                            Qta = Convert.ToInt32(grdMostraDDTDaInserire.Rows[i].Cells[4].Text),
                            Data = Convert.ToDateTime(grdMostraDDTDaInserire.Rows[i].Cells[0].Text),
                            PzzoUniCantiere = Convert.ToDecimal(grdMostraDDTDaInserire.Rows[i].Cells[5].Text),
                            CodArt = grdMostraDDTDaInserire.Rows[i].Cells[2].Text,
                            DescriCodArt = grdMostraDDTDaInserire.Rows[i].Cells[3].Text,
                            Tipologia = txtTipDatCant.Text,
                            Fascia = Convert.ToInt32(txtFascia.Text),
                            Acquirente = OperaiDAO.GetAll().Where(w => w.NomeOp == grdMostraDDTDaInserire.Rows[i].Cells[6].Text).FirstOrDefault().IdOperaio.ToString(),
                            Fornitore = ddlScegliFornit.SelectedItem.Value,
                            NumeroBolla = grdMostraDDTDaInserire.Rows[i].Cells[1].Text,
                            ProtocolloInterno = Convert.ToInt32(txtProtocollo.Text),
                            Visibile = true,
                            Ricalcolo = true,
                            RicaricoSiNo = true
                        });
                    }
                }
                grdMostraDDTDaInserire.DataSource = null;
                grdMostraDDTDaInserire.DataBind();
                lblInsMatDaDDT.Text = "";
            }
            else
            {
                (Master as layout).SetModal($"È necessario specificare il protocollo prima di inserire i materiali");
            }
        }
        protected void btnSelezionaTutto_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grdMostraDDTDaInserire.Rows.Count; i++)
            {
                ((CheckBox)grdMostraDDTDaInserire.Rows[i].FindControl("chkDDTSelezionato")).Checked = true;
            }
        }
        protected void btnDeselezionaTutto_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < grdMostraDDTDaInserire.Rows.Count; i++)
            {
                ((CheckBox)grdMostraDDTDaInserire.Rows[i].FindControl("chkDDTSelezionato")).Checked = false;
            }
        }
        #endregion

        #region Materiali Cantieri
        decimal maxQtaRientro = -1;

        /* HELPERS */
        protected void FillMatCant(MaterialiCantieri mc)
        {
            mc.IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value);
            mc.DescriMateriali = txtDescrMat.Text;
            mc.Visibile = chkVisibile.Checked;
            mc.Ricalcolo = chkRicalcolo.Checked;
            mc.RicaricoSiNo = chkRicarico.Checked;
            mc.Data = Convert.ToDateTime(txtDataDDT.Text);
            mc.PzzoUniCantiere = Convert.ToDecimal(txtPzzoUnit.Text);
            mc.CodArt = txtCodArt.Text;
            mc.DescriCodArt = txtDescriCodArt.Text;
            mc.Tipologia = txtTipDatCant.Text;
            mc.Acquirente = ddlScegliAcquirente.SelectedItem.Value;
            mc.Fornitore = ddlScegliFornit.SelectedItem.Value;
            mc.Note = txtNote.Text;
            mc.Note2 = txtNote_2.Text;
            mc.Qta = txtQta.Text != "" ? Convert.ToDouble(txtQta.Text) : 0;
            mc.Fascia = txtFascia.Text != "" ? Convert.ToInt32(txtFascia.Text) : 0;
            mc.ProtocolloInterno = txtProtocollo.Text != "" ? Convert.ToInt32(txtProtocollo.Text) : 0;
            mc.NumeroBolla = txtNumBolla.Enabled && txtNumBolla.Text != "" ? txtNumBolla.Text : (ddlScegliDDTMef.SelectedIndex != -1 ? ddlScegliDDTMef.SelectedItem.Text.Split('-')[3] : "");
            mc.PzzoFinCli = txtPzzoFinCli.Text != "" ? Convert.ToDecimal(txtPzzoFinCli.Text) : 0.0m;

            if (mc.Tipologia == "RIENTRO")
            {
                mc.Qta *= -1;
            }
        }
        protected void ShowForMatCant()
        {
            lblScegliListino.Visible = true;
            ddlScegliListino.Visible = true;
            lblFiltroCod_FSS.Visible = true;
            txtFiltroCodFSS.Visible = true;
            lblFiltroAA_Des.Visible = true;
            txtFiltroAA_Des.Visible = true;
            lblScegliMatCant.Visible = false;
            ddlScegliMatCant.Visible = false;
            btnInserisciMatCant.Visible = true;
            btnInserisciRientro.Visible = false;
            lblFiltroMatCantCodArt.Visible = txtFiltroMatCantCodArt.Visible = lblFiltroMatCantDescriCodArt.Visible = txtFiltroMatCantDescriCodArt.Visible = false;
        }
        protected void BindGridMatCant(string tipologia = "MATERIALE")
        {
            List<MaterialiCantieri> mcList = MaterialiCantieriDAO.GetMaterialeCantiereForGridView(ddlScegliCant.SelectedItem.Value, txtFiltroCodArtGrdMatCant.Text,
                txtFiltroDescriCodArtGrdMatCant.Text, txtFiltroProtocolloGrdMatCant.Text, txtFiltroFornitoreGrdMatCant.Text, tipologia);
            grdMatCant.DataSource = mcList;
            grdMatCant.DataBind();
            CalcolaTotaleValore(mcList);
        }

        /* EVENTI CLICK */
        protected void btnInserisciMatCant_Click(object sender, EventArgs e)
        {
            if (IsDateNotSet())
                return;

            MaterialiCantieri mc = new MaterialiCantieri();
            FillMatCant(mc);

            if (txtQta.Text != "" && Convert.ToDecimal(txtQta.Text) > 0 && Convert.ToDecimal(txtPzzoUnit.Text) > 0)
            {
                if (ddlScegliDDTMef.SelectedItem == null || ddlScegliDDTMef.SelectedItem.Text == "")
                {
                    if (txtNumBolla.Text == "")
                    {
                        (Master as layout).SetModal($"Scegliere un DDT dal menù a discesa o compilare il campo \"Numero Bolla\"");
                        return;
                    }
                }

                if (IsIntestazioneCompilata())
                {
                    if (txtCodArt.Text != "" && txtDescriCodArt.Text != "")
                    {
                        if (!MaterialiCantieriDAO.InserisciMaterialeCantiere(mc))
                        {
                            (Master as layout).SetModal($"Errore durante l'inserimento del record. L'intestazione deve essere interamente compilata");
                        }
                    }
                    else
                    {
                        (Master as layout).SetModal($"Codice Articolo e Descrizione Codice Articolo obbligatori");
                        return;
                    }
                }
            }
            else
            {
                (Master as layout).SetModal($"Quantità e/o Prezzo Unitario devono essere maggiori di 0");
            }

            BindGridMatCant();
            SvuotaCampi(pnlMascheraGestCant);
            txtFiltroCodFSS.Focus();
        }
        protected void btnFiltraGrdMatCant_Click(object sender, EventArgs e)
        {
            BindGridMatCant(txtTipDatCant.Text);
        }
        //Visibilità pannelli
        protected void btnMatCant_Click(object sender, EventArgs e)
        {
            SetMatCantFieldsAndPanels("Inserisci Materiali Cantieri", "Mef");
        }
        protected void btnMatCantAltriFornitori_Click(object sender, EventArgs e)
        {
            SetMatCantFieldsAndPanels("Inserisci Materiali Cantieri Altri Fornitori", "Magazzino");
        }
        protected void btnMatCantAltriFornitoriDdt_Click(object sender, EventArgs e)
        {
            SetMatCantFieldsAndPanels("Inserisci Materiali Cantieri Altri Fornitori da DDT", "Magazzino", true);
        }
        public void SetMatCantFieldsAndPanels(string title, string fornitore, bool showDdtPanel = false)
        {
            lblTitoloMaschera.Text = title;
            txtTipDatCant.Text = "MATERIALE";
            ShowForMatCant();
            ShowPanels(false, true, false, false, false, false, false, false);
            grdMatCant.Visible = true;
            grdRientro.Visible = false;
            btnModMatCant.Visible = false;
            txtFiltroAnnoDDT.Text = txtFiltroN_DDT.Text = "";

            if (ddlScegliDDTMef.SelectedValue != "")
            {
                ddlScegliDDTMef.SelectedIndex = 0;
            }

            pnlMateCantFilters.Visible = !showDdtPanel;
            pnlMateAltriFornitoriDaDdt.Visible = showDdtPanel;

            BindGridMatCant();
            EnableDisableControls(true, pnlMascheraGestCant);
            SvuotaCampi(pnlMascheraGestCant);
            ChooseFornitore(fornitore);
            HideMessageLabels();
        }

        /* EVENTI TEXT-CHANGED */
        protected void txtFiltroCodFSS_TextChanged(object sender, EventArgs e)
        {
            FillDddlScegliListino();
        }
        protected void txtFiltroAA_Des_TextChanged(object sender, EventArgs e)
        {
            FillDddlScegliListino();
        }
        protected void ddlScegliListino_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliListino.SelectedIndex != 0)
            {
                string[] partiListino = ddlScegliListino.SelectedItem.Text.Split('|');
                txtCodArt.Text = partiListino[0].Trim();
                txtDescriCodArt.Text = partiListino[1].Trim();
                txtPzzoNettoMef.Text = partiListino[2].Trim();
                txtPzzoUnit.Text = "0.00";
            }
            else
            {
                txtCodArt.Text = txtDescriCodArt.Text = txtPzzoNettoMef.Text = "";
                txtPzzoUnit.Text = "0.00";
            }

            HideMessageLabels();
            txtQta.Focus();
        }
        protected void ddlScegliMatCant_TextChanged(object sender, EventArgs e)
        {
            if (ddlScegliMatCant.SelectedIndex > 0)
            {
                string[] partiMatCant = ddlScegliMatCant.SelectedItem.Text.Split('|');
                txtCodArt.Text = partiMatCant[0].Trim();
                txtDescriCodArt.Text = partiMatCant[1].Trim();
                txtQta.Text = partiMatCant[2].Trim();
                txtPzzoNettoMef.Text = partiMatCant[3].Trim();
                txtPzzoUnit.Text = "0.00";
                txtPzzoFinCli.Text = partiMatCant[4].Trim();
            }
            else
            {
                txtCodArt.Text = txtDescriCodArt.Text = txtPzzoNettoMef.Text = txtPzzoFinCli.Text = "";
                txtPzzoUnit.Text = "0.00";
            }

            HideMessageLabels();
            txtQta.Focus();
        }
        protected void txtFiltroMatCantCodArt_TextChanged(object sender, EventArgs e)
        {
            FillAllDdl();
        }
        protected void txtFiltroMatCantDescriCodArt_TextChanged(object sender, EventArgs e)
        {
            FillAllDdl();
        }

        /* EVENTI PER LA GESTIONE DEI ROWCOMMAND */
        //MatCant
        protected void grdMatCant_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idMatCant = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "VisualMatCant")
            {
                lblTitoloMaschera.Text = "Visualizza Materiali Cantieri";
                btnInserisciMatCant.Visible = btnModMatCant.Visible = false;
                PopolaCampiMatCant(idMatCant, false);
                HideMessageLabels();
            }
            else if (e.CommandName == "ModMatCant")
            {
                lblTitoloMaschera.Text = "Modifica Materiali Cantieri";
                btnInserisciMatCant.Visible = false;
                btnModMatCant.Visible = !btnInserisciMatCant.Visible;
                btnModRientro.Visible = !btnModMatCant.Visible;
                hidIdMatCant.Value = idMatCant.ToString();
                PopolaCampiMatCant(idMatCant, true);
                BindGridMatCant();
                HideMessageLabels();
            }
            else if (e.CommandName == "ElimMatCant")
            {
                bool isDeleted = MaterialiCantieriDAO.DeleteMatCant(idMatCant);
                if (isDeleted)
                {
                    lblIsRecordInserito.Text = "Record eliminato con successo";
                    lblIsRecordInserito.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante l'eliminazione del record");
                }
                BindGridMatCant();
            }
        }
        private void PopolaCampiMatCant(int id, bool enableControls)
        {
            //Rendo i textbox abilitati/disabilitati
            EnableDisableControls(enableControls, pnlMascheraGestCant);

            MaterialiCantieri mc = MaterialiCantieriDAO.GetSingle(id);
            ddlScegliAcquirente.SelectedValue = mc.Acquirente;
            ddlScegliFornit.SelectedValue = mc.Fornitore;
            txtTipDatCant.Text = mc.Tipologia;
            txtNumBolla.Text = mc.NumeroBolla.ToString();
            txtDataDDT.Text = mc.Data.ToString("yyyy-MM-dd");
            txtDataDDT.TextMode = TextBoxMode.Date;
            txtFascia.Text = mc.Fascia.ToString();
            txtProtocollo.Text = mc.ProtocolloInterno.ToString();
            txtCodArt.Text = mc.CodArt;
            txtDescriCodArt.Text = mc.DescriCodArt;
            txtDescrMat.Text = mc.DescriMateriali;
            txtNote.Text = mc.Note;
            txtNote_2.Text = mc.Note2;
            txtPzzoUnit.Text = mc.PzzoUniCantiere.ToString();
            txtPzzoFinCli.Text = mc.PzzoFinCli.ToString();
            chkVisibile.Checked = mc.Visibile;
            chkRicalcolo.Checked = mc.Ricalcolo;
            chkRicarico.Checked = mc.RicaricoSiNo;
            txtPzzoFinCli.Text = mc.PzzoFinCli.ToString();

            if (txtTipDatCant.Text == "MATERIALE")
                txtQta.Text = mc.Qta.ToString();
            else if (txtTipDatCant.Text == "RIENTRO")
                txtQta.Text = (mc.Qta * (-1)).ToString();
        }
        private void PopolaObjMatCant(MaterialiCantieri mc)
        {
            mc.IdMaterialiCantiere = Convert.ToInt32(hidIdMatCant.Value);
            mc.IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value);
            mc.Acquirente = ddlScegliAcquirente.SelectedItem.Value;
            mc.Fornitore = ddlScegliFornit.SelectedItem.Value;
            mc.Tipologia = txtTipDatCant.Text;
            mc.NumeroBolla = txtNumBolla.Text;
            mc.Data = Convert.ToDateTime(txtDataDDT.Text);
            mc.Fascia = Convert.ToInt32(txtFascia.Text);
            mc.ProtocolloInterno = Convert.ToInt32(txtProtocollo.Text);
            mc.CodArt = txtCodArt.Text;
            mc.DescriCodArt = txtDescriCodArt.Text;
            mc.DescriMateriali = txtDescrMat.Text;
            mc.Note = txtNote.Text;
            mc.Note2 = txtNote_2.Text;
            mc.PzzoUniCantiere = Convert.ToDecimal(txtPzzoUnit.Text);
            mc.PzzoFinCli = Convert.ToDecimal(txtPzzoFinCli.Text);
            mc.Visibile = chkVisibile.Checked;
            mc.Ricalcolo = chkRicalcolo.Checked;
            mc.RicaricoSiNo = chkRicarico.Checked;
            mc.PzzoFinCli = Convert.ToDecimal(txtPzzoFinCli.Text);

            if (mc.Tipologia == "MATERIALE")
                mc.Qta = Convert.ToDouble(txtQta.Text);
            else if (mc.Tipologia == "RIENTRO")
                mc.Qta = (Convert.ToDouble(txtQta.Text)) * (-1);
        }
        protected void btnModMatCant_Click(object sender, EventArgs e)
        {
            if ((Convert.ToDecimal(txtQta.Text) > 0 && txtQta.Text != "") && Convert.ToDecimal(txtPzzoUnit.Text) > 0)
            {
                MaterialiCantieri mc = new MaterialiCantieri();
                PopolaObjMatCant(mc);
                bool isUpdated = MaterialiCantieriDAO.UpdateMatCant(mc);

                if (isUpdated)
                {
                    lblIsRecordInserito.Text = "Record modificato con successo";
                    lblIsRecordInserito.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante la modifica del record");
                }

                BindGridMatCant();
                SvuotaCampi(pnlMascheraGestCant);

                btnInserisciMatCant.Visible = true;
                btnModMatCant.Visible = false;
                lblTitoloMaschera.Text = "Inserisci Materiali Cantieri";
            }
            else
            {
                (Master as layout).SetModal($"Quantità e/o Prezzo Unitario devono essere maggiori di 0");
            }
        }
        #endregion

        #region Mate altri fornitori da DDT
        protected void MateDaDdtFiltro_TextChanged(object sender, EventArgs e)
        {
            DDTMefObject ddt = new DDTMefObject();
            ddt.AnnoInizio = txtFiltroMateDaDdtAnnoInizio.Text;
            ddt.AnnoFine = txtFiltroMateDaDdtAnnoFine.Text;
            ddt.CodArt1 = txtFiltroMateDaDdtCodArt.Text;
            ddt.DescriCodArt1 = txtFiltroMateDaDdtDescr.Text;
            List<DDTMef> items = DDTMefDAO.GetDdt(ddt);
            FillDdlAltriFornitoriDaDdt(items);
        }

        public void FillDdlAltriFornitoriDaDdt(List<DDTMef> items)
        {
            ddlScegliMateDaDdt.Items.Clear();
            ddlScegliMateDaDdt.Items.Add(new System.Web.UI.WebControls.ListItem("", "-1"));
            DropDownListManager.FillDdlDdtForGestCant(items, ref ddlScegliMateDaDdt);
            lblMediaPrezziUnitari.Text = "Media Prezzi Unitari: " + (items.Sum(s => s.PrezzoUnitario) / (items.Count() == 0 ? 1 : items.Count())).ToString("0.00") + " €";
        }

        protected void ddlScegliMateDaDdt_TextChanged(object sender, EventArgs e)
        {

        }
        #endregion

        #region Rientro
        protected void ShowForRientro()
        {
            lblScegliListino.Visible = false;
            ddlScegliListino.Visible = false;
            lblFiltroCod_FSS.Visible = false;
            txtFiltroCodFSS.Visible = false;
            lblFiltroAA_Des.Visible = false;
            txtFiltroAA_Des.Visible = false;
            lblScegliMatCant.Visible = true;
            ddlScegliMatCant.Visible = true;
            btnInserisciMatCant.Visible = false;
            btnInserisciRientro.Visible = true;
            lblFiltroMatCantCodArt.Visible = txtFiltroMatCantCodArt.Visible = lblFiltroMatCantDescriCodArt.Visible = txtFiltroMatCantDescriCodArt.Visible = true;
        }
        protected void btnRientro_Click(object sender, EventArgs e)
        {
            string tipologia = "RIENTRO";
            lblTitoloMaschera.Text = "Inserisci Rientro Materiali";
            txtTipDatCant.Text = tipologia;
            FillAllDdl(false);
            ShowForRientro();
            ShowPanels(false, true, false, false, false, false, false, false);
            grdMatCant.Visible = false;
            grdRientro.Visible = true;
            btnModMatCant.Visible = btnInserisciMatCant.Visible = btnModRientro.Visible = false;
            BindGridMatCant(tipologia);
            EnableDisableControls(true, pnlMascheraGestCant);
            SvuotaCampi(pnlMascheraGestCant);
            ChooseFornitore("Rientro");
            HideMessageLabels();
            txtFiltroAnnoDDT.Text = txtFiltroN_DDT.Text = "";
        }
        protected void btnModRientro_Click(object sender, EventArgs e)
        {
            if ((txtQta.Text != "" && txtQta.Text != "0") && Convert.ToDecimal(txtPzzoUnit.Text) > 0)
            {
                MaterialiCantieri mc = new MaterialiCantieri();
                PopolaObjMatCant(mc);
                bool isUpdated = MaterialiCantieriDAO.UpdateMatCant(mc);

                if (isUpdated)
                {
                    lblIsRecordInserito.Text = "Record modificato con successo";
                    lblIsRecordInserito.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante la modifica del record");
                }

                BindGridMatCant("RIENTRO");
                SvuotaCampi(pnlMascheraGestCant);

                btnInserisciRientro.Visible = true;
                btnModRientro.Visible = false;
                lblTitoloMaschera.Text = "Inserisci Rientro Materiali";
            }
            else
            {
                (Master as layout).SetModal($"Quantità e/o Prezzo Unitario devono essere maggiori di 0");
            }
        }
        protected void btnInserisciRientro_Click(object sender, EventArgs e)
        {
            bool isInserito = false;

            if (IsDateNotSet())
                return;

            if ((txtQta.Text != "" && txtQta.Text != "0") && Convert.ToDecimal(txtPzzoUnit.Text) > 0)
            {
                if (ddlScegliDDTMef.SelectedItem == null || ddlScegliDDTMef.SelectedItem.Text == "")
                {
                    if (txtNumBolla.Text == "")
                    {
                        (Master as layout).SetModal($"Scegliere un DDT dal menù a discesa o compilare il campo \"Numero Bolla\"");
                        return;
                    }
                }

                if (IsIntestazioneCompilata())
                {
                    string[] partiMatCant = ddlScegliMatCant.SelectedItem.Text.Split('|');
                    maxQtaRientro = Convert.ToDecimal(partiMatCant[2].Trim());

                    if (Convert.ToInt32(txtQta.Text) <= maxQtaRientro)
                    {
                        MaterialiCantieri mc = new MaterialiCantieri();
                        FillMatCant(mc);
                        isInserito = MaterialiCantieriDAO.InserisciMaterialeCantiere(mc);
                    }
                    else
                    {
                        (Master as layout).SetModal($"La quantità non deve superare quella specificata nel record di materiale cantiere");
                        return;
                    }
                }

                if (isInserito)
                {
                    lblIsRecordInserito.Text = "Record inserito con successo";
                    lblIsRecordInserito.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante l'inserimento del record. L'intestazione deve essere interamente compilata");
                }
            }
            else
            {
                (Master as layout).SetModal($"Quantità e/o Prezzo Unitario devono essere maggiori di 0");
            }

            BindGridMatCant("RIENTRO");
            SvuotaCampi(pnlMascheraGestCant);

            txtFiltroCodFSS.Focus();
        }
        protected void grdRientro_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idRientro = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "VisualRientro")
            {
                lblTitoloMaschera.Text = "Visualizza Rientro Materiali";
                btnInserisciRientro.Visible = btnModRientro.Visible = false;
                PopolaCampiMatCant(idRientro, false);
                HideMessageLabels();
            }
            else if (e.CommandName == "ModRientro")
            {
                lblTitoloMaschera.Text = "Modifica Rientro Materiali";
                btnInserisciRientro.Visible = false;
                btnModRientro.Visible = !btnInserisciRientro.Visible;
                btnModMatCant.Visible = !btnModRientro.Visible;
                hidIdMatCant.Value = idRientro.ToString();
                PopolaCampiMatCant(idRientro, true);
                HideMessageLabels();
            }
            else if (e.CommandName == "ElimRientro")
            {
                bool isDeleted = MaterialiCantieriDAO.DeleteMatCant(idRientro);

                if (isDeleted)
                {
                    lblIsRecordInserito.Text = "Record eliminato con successo";
                    lblIsRecordInserito.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante l'eliminazione del record");
                }

                BindGridMatCant("RIENTRO");
            }
        }
        #endregion

        #region Accrediti
        protected void ShowForAccrediti()
        {
            lblScegliListino.Visible = false;
            ddlScegliListino.Visible = false;
            lblFiltroCod_FSS.Visible = false;
            txtFiltroCodFSS.Visible = false;
            lblFiltroAA_Des.Visible = false;
            txtFiltroAA_Des.Visible = false;
            lblScegliMatCant.Visible = true;
            ddlScegliMatCant.Visible = true;
            btnInserisciAccrediti.Visible = true;
            lblFiltroCodArtAccrediti.Visible = txtFiltroCodArtAccrediti.Visible = lblFiltroDescriCodArtGrdMatCant.Visible = txtFiltroDescriCodArtGrdMatCant.Visible = true;
        }
        protected void BindGridAccrediti()
        {
            List<MaterialiCantieri> mcList = MaterialiCantieriDAO.GetMaterialeCantiereForGridView(ddlScegliCant.SelectedItem.Value, txtFiltroCodArtAccrediti.Text,
                txtFiltroDescriCodArtGrdMatCant.Text, txtFiltroProtAccrediti.Text, txtFiltroFornAccrediti.Text, "ACCREDITI");
            grdAccrediti.DataSource = mcList;
            grdAccrediti.DataBind();
            CalcolaTotaleValore(mcList);
        }
        protected void btnAccrediti_Click(object sender, EventArgs e)
        {
            lblTitoloMaschera.Text = "Inserisci Accredito";
            txtTipDatCant.Text = "ACCREDITI";
            ShowForAccrediti();
            ShowPanels(false, false, false, false, false, false, false, true);
            grdMatCant.Visible = false;
            grdAccrediti.Visible = true;
            btnModMatCant.Visible = btnInserisciMatCant.Visible = btnModRientro.Visible = false;
            BindGridAccrediti();
            EnableDisableControls(true, pnlMascheraAccrediti);
            SvuotaCampi(pnlMascheraAccrediti);
            ChooseFornitore("Accrediti");
            HideMessageLabels();
            txtFiltroAnnoDDT.Text = txtFiltroN_DDT.Text = "";
        }
        protected void txtCodArtAccrediti_TextChanged(object sender, EventArgs e)
        {
            HideMessageLabels();
        }
        protected void txtDescrCodArtAccrediti_TextChanged(object sender, EventArgs e)
        {
            HideMessageLabels();
        }
        protected void btnInserisciAccrediti_Click(object sender, EventArgs e)
        {
            bool isInserito = false;

            if (IsDateNotSet())
                return;

            if ((txtQtaAccrediti.Text != "" && txtQtaAccrediti.Text != "0") && Convert.ToDecimal(txtPrezzoUniAccrediti.Text) > 0)
            {
                if (ddlScegliDDTMef.SelectedItem == null || ddlScegliDDTMef.SelectedItem.Text == "")
                {
                    if (txtNumBolla.Text == "")
                    {
                        (Master as layout).SetModal($"Scegliere un DDT dal menù a discesa o compilare il campo \"Numero Bolla\"");
                        return;
                    }
                }

                if (IsIntestazioneCompilata())
                {
                    isInserito = MaterialiCantieriDAO.InserisciMaterialeCantiere(new MaterialiCantieri
                    {
                        IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                        DescriMateriali = txtDescrMatAccrediti.Text,
                        Visibile = chkVisibileAccrediti.Checked,
                        Ricalcolo = chkRicalcoloAccrediti.Checked,
                        RicaricoSiNo = chkRicaricoAccrediti.Checked,
                        Data = Convert.ToDateTime(txtDataDDT.Text),
                        PzzoUniCantiere = Convert.ToDecimal(txtPrezzoUniAccrediti.Text),
                        CodArt = txtCodArtAccrediti.Text,
                        DescriCodArt = txtDescrCodArtAccrediti.Text,
                        Tipologia = txtTipDatCant.Text,
                        Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                        Fornitore = ddlScegliFornit.SelectedItem.Value,
                        Note = txtNoteAccrediti.Text,
                        Note2 = txtNote2Accrediti.Text,
                        Qta = (Convert.ToDouble(txtQtaAccrediti.Text)) * (-1),
                        Fascia = txtFascia.Text != "" ? Convert.ToInt32(txtFascia.Text) : 0,
                        ProtocolloInterno = txtProtocollo.Text != "" ? Convert.ToInt32(txtProtocollo.Text) : 0,
                        NumeroBolla = txtNumBolla.Enabled && txtNumBolla.Text != "" ? txtNumBolla.Text : (ddlScegliDDTMef.SelectedIndex != -1 ? ddlScegliDDTMef.SelectedItem.Text.Split('-')[3] : ""),
                        PzzoFinCli = txtPrezzoFinAccrediti.Text != "" ? Convert.ToDecimal(txtPrezzoFinAccrediti.Text) : 0.0m
                    });
                }

                if (isInserito)
                {
                    lblMsgAccrediti.Text = "Record inserito con successo";
                    lblMsgAccrediti.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante l'inserimento del record. L'intestazione deve essere interamente compilata");
                }
            }
            else
            {
                (Master as layout).SetModal($"Quantità e/o Prezzo Unitario devono essere maggiori di 0");
            }

            BindGridAccrediti();
            SvuotaCampi(pnlMascheraAccrediti);
        }

        protected void btnModificaAccrediti_Click(object sender, EventArgs e)
        {
            if ((txtQtaAccrediti.Text != "" && txtQtaAccrediti.Text != "0") && Convert.ToDecimal(txtPrezzoUniAccrediti.Text) > 0)
            {
                bool isUpdated = MaterialiCantieriDAO.UpdateMatCant(new MaterialiCantieri
                {
                    IdMaterialiCantiere = Convert.ToInt32(hfIdAccrediti.Value),
                    IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                    Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                    Fornitore = ddlScegliFornit.SelectedItem.Value,
                    Tipologia = txtTipDatCant.Text,
                    NumeroBolla = txtNumBolla.Text,
                    Data = Convert.ToDateTime(txtDataDDT.Text),
                    Fascia = Convert.ToInt32(txtFascia.Text),
                    ProtocolloInterno = Convert.ToInt32(txtProtocollo.Text),
                    CodArt = txtCodArtAccrediti.Text,
                    DescriCodArt = txtDescrCodArtAccrediti.Text,
                    DescriMateriali = txtDescrMatAccrediti.Text,
                    Note = txtNoteAccrediti.Text,
                    Note2 = txtNote2Accrediti.Text,
                    PzzoUniCantiere = Convert.ToDecimal(txtPrezzoUniAccrediti.Text),
                    PzzoFinCli = Convert.ToDecimal(txtPrezzoFinAccrediti.Text),
                    Visibile = chkVisibileAccrediti.Checked,
                    Ricalcolo = chkRicalcoloAccrediti.Checked,
                    RicaricoSiNo = chkRicaricoAccrediti.Checked,
                    Qta = (Convert.ToDouble(txtQtaAccrediti.Text)) * (-1)
                });

                if (isUpdated)
                {
                    lblMsgAccrediti.Text = "Record modificato con successo";
                    lblMsgAccrediti.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante la modifica del record");
                }

                BindGridAccrediti();
                SvuotaCampi(pnlMascheraAccrediti);

                btnInserisciAccrediti.Visible = true;
                btnModificaAccrediti.Visible = false;
                lblTitoloMaschera.Text = "Inserisci Accrediti";
            }
            else
            {
                (Master as layout).SetModal($"Quantità e/o Prezzo Unitario devono essere maggiori di 0");
            }
        }
        protected void btnFiltraAccrediti_Click(object sender, EventArgs e)
        {
            BindGridAccrediti();
        }
        private void PopolaCampiAccrediti(int id, bool enableControls)
        {
            MaterialiCantieri mc = MaterialiCantieriDAO.GetSingle(id);

            //Rendo i textbox abilitati/disabilitati
            EnableDisableControls(enableControls, pnlMascheraAccrediti);

            ddlScegliAcquirente.SelectedValue = mc.Acquirente;
            ddlScegliFornit.SelectedValue = mc.Fornitore;
            txtTipDatCant.Text = mc.Tipologia;
            txtNumBolla.Text = mc.NumeroBolla.ToString();
            txtDataDDT.Text = mc.Data.ToString("yyyy-MM-dd");
            txtDataDDT.TextMode = TextBoxMode.Date;
            txtFascia.Text = mc.Fascia.ToString();
            txtProtocollo.Text = mc.ProtocolloInterno.ToString();
            txtCodArtAccrediti.Text = mc.CodArt;
            txtDescrCodArtAccrediti.Text = mc.DescriCodArt;
            txtDescrMatAccrediti.Text = mc.DescriMateriali;
            txtNoteAccrediti.Text = mc.Note;
            txtNote2Accrediti.Text = mc.Note2;
            txtPrezzoUniAccrediti.Text = mc.PzzoUniCantiere.ToString();
            txtPrezzoFinAccrediti.Text = mc.PzzoFinCli.ToString();
            chkVisibileAccrediti.Checked = mc.Visibile;
            chkRicalcoloAccrediti.Checked = mc.Ricalcolo;
            chkRicaricoAccrediti.Checked = mc.RicaricoSiNo;
            txtQtaAccrediti.Text = (mc.Qta * (-1)).ToString();
        }
        private void VisualizzaAccrediti(int id)
        {
            lblTitoloMaschera.Text = "Visualizza Accrediti";
            btnInserisciAccrediti.Visible = btnModificaAccrediti.Visible = false;
            PopolaCampiAccrediti(id, false);
            HideMessageLabels();
        }
        private void ModificaDatiAccrediti(int id)
        {
            lblTitoloMaschera.Text = "Modifica Accrediti";
            btnInserisciAccrediti.Visible = false;
            btnModificaAccrediti.Visible = true;
            PopolaCampiAccrediti(id, true);
            BindGridAccrediti();
            hfIdAccrediti.Value = id.ToString();
            HideMessageLabels();
        }
        private void EliminaAccrediti(int id)
        {
            bool isDeleted = MaterialiCantieriDAO.DeleteMatCant(id);

            if (isDeleted)
            {
                lblMsgAccrediti.Text = "Record eliminato con successo";
                lblMsgAccrediti.ForeColor = Color.Blue;
            }
            else
            {
                (Master as layout).SetModal($"Errore durante l'eliminazione del record");
            }

            BindGridAccrediti();
        }
        protected void grdAccrediti_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idMatCant = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "VisualAccrediti")
                VisualizzaAccrediti(idMatCant);
            else if (e.CommandName == "ModAccrediti")
                ModificaDatiAccrediti(idMatCant);
            else if (e.CommandName == "ElimAccrediti")
                EliminaAccrediti(idMatCant);
        }
        protected void btnCalcolaPrezzoAccrediti_Click(object sender, EventArgs e)
        {
            if (txtPrezzoNettoAccrediti.Text != "")
                txtPrezzoUniAccrediti.Text = Math.Round(Convert.ToDecimal(txtPrezzoNettoAccrediti.Text.Replace(".", ",")), 2).ToString();
            else
            {
                (Master as layout).SetModal($"Inserire un valore nella casella \"Prezzo Netto Mef\" per calcolare il \"Prezzo Unitario\"");
            }
        }
        #endregion

        #region Manodopera
        /* HELPERS */
        protected void BindGridManodop()
        {
            List<MaterialiCantieri> mcList = MaterialiCantieriDAO.GetMaterialeCantiereForGridView(ddlScegliCant.SelectedItem.Value, txtFiltroManodopCodArt.Text,
                txtFiltroManodopDescriCodArt.Text, txtFiltroProtocolloGrdMatCant.Text, txtFiltroFornitoreGrdMatCant.Text, "MANODOPERA");
            grdManodop.DataSource = mcList;
            grdManodop.DataBind();
        }

        /* EVENTI CLICK */
        protected void btnInsManodop_Click(object sender, EventArgs e)
        {
            if (IsDateNotSet())
                return;

            if ((Convert.ToDecimal(txtManodopQta.Text) > 0 && txtManodopQta.Text != ""))
            {
                if (IsIntestazioneCompilata())
                {
                    Operai op = OperaiDAO.GetSingle(Convert.ToInt32(ddlScegliAcquirente.SelectedItem.Value));
                    bool isInserito = MaterialiCantieriDAO.InserisciMaterialeCantiere(new MaterialiCantieri
                    {
                        CodArt = "Manodopera" + op.Operaio,
                        DescriCodArt = "Manodopera" + op.Operaio,
                        IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                        Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                        Fornitore = Enumeratori.Fornitori.Manodopera.GetHashCode().ToString(), // ddlScegliFornit.SelectedItem.Value,
                        Qta = Convert.ToDouble(txtManodopQta.Text.Replace(".", ",")),
                        Tipologia = txtTipDatCant.Text,
                        ProtocolloInterno = Convert.ToInt32(txtProtocollo.Text),
                        DescriMateriali = txtDescrManodop.Text,
                        Data = Convert.ToDateTime(txtDataDDT.Text),
                        Note = txtNote1.Text,
                        Note2 = txtNote2.Text,
                        Visibile = chkManodopVisibile.Checked,
                        Ricalcolo = chkManodopRicalcolo.Checked,
                        RicaricoSiNo = chkManodopRicaricoSiNo.Checked,
                        NumeroBolla = txtNumBolla.Text,
                        Fascia = Convert.ToInt32(txtFascia.Text),
                        PzzoUniCantiere = txtPzzoManodop.Text != "" ? Convert.ToDecimal(txtPzzoManodop.Text.Replace(".", ",")) : 0
                    });

                    if (isInserito)
                    {
                        lblIsManodopInserita.Text = "Record inserito con successo";
                        lblIsManodopInserita.ForeColor = Color.Blue;
                    }
                    else
                    {
                        (Master as layout).SetModal($"Errore durante l'inserimento del record. L'intestazione deve essere interamente compilata.");
                    }
                }
                else
                {
                    (Master as layout).SetModal($"La quantità deve essere maggiore di \"0\"");
                }
            }
            else
            {
                (Master as layout).SetModal($"Inserire un valore per la data");
            }

            BindGridManodop();
            SvuotaCampi(pnlManodopera);
        }

        //Visibilità pannello
        protected void btnManodop_Click(object sender, EventArgs e)
        {
            lblTitoloMaschera.Text = "Manodopera";
            txtTipDatCant.Text = "MANODOPERA";
            ShowPanels(false, false, true, false, false, false, false, false);
            btnInsManodop.Visible = true;
            btnModManodop.Visible = false;
            BindGridManodop();
            EnableDisableControls(true, pnlManodopera);
            SvuotaCampi(pnlManodopera);
            ChooseFornitore("Manodopera");
            HideMessageLabels();
            txtFiltroAnnoDDT.Text = txtFiltroN_DDT.Text = "";

            //Popolo il campo PzzoManodopera a partire dal prezzo scritto nella tabella Cantieri
            txtPzzoManodop.Text = CantieriDAO.GetSingle(Convert.ToInt32(ddlScegliCant.SelectedItem.Value))?.PzzoManodopera.ToString() ?? "0";
        }

        /* EVENTI PER IL ROWCOMMAND */
        protected void btnFiltraGrdManodop_Click(object sender, EventArgs e)
        {
            BindGridManodop();
        }
        protected void grdManodop_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idManodop = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "VisualManodop")
            {
                lblTitoloMaschera.Text = "Visualizza Manodopera";
                btnInsManodop.Visible = btnModManodop.Visible = false;
                PopolaCampiManodop(idManodop, false);
                HideMessageLabels();
            }
            else if (e.CommandName == "ModManodop")
            {
                lblTitoloMaschera.Text = "Modifica Manodopera";
                btnInsManodop.Visible = false;
                btnModManodop.Visible = !btnInsManodop.Visible;
                hidManodop.Value = idManodop.ToString();
                PopolaCampiManodop(idManodop, true);
                HideMessageLabels();
            }
            else if (e.CommandName == "ElimManodop")
            {
                bool isDeleted = MaterialiCantieriDAO.DeleteMatCant(idManodop);

                if (isDeleted)
                {
                    lblIsManodopInserita.Text = "Record eliminato con successo";
                    lblIsManodopInserita.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante l'eliminazione del record");
                }

                BindGridManodop();
            }
        }
        private void PopolaCampiManodop(int idManodop, bool enableControls)
        {
            MaterialiCantieri mc = MaterialiCantieriDAO.GetSingle(idManodop);

            //Rendo i textbox abilitati/disabilitati
            EnableDisableControls(enableControls, pnlManodopera);

            ddlScegliAcquirente.SelectedValue = mc.Acquirente;
            ddlScegliFornit.SelectedValue = mc.Fornitore;
            txtTipDatCant.Text = mc.Tipologia;
            txtNumBolla.Text = mc.NumeroBolla.ToString();
            txtDataDDT.Text = mc.Data.ToString("yyyy-MM-dd");
            txtDataDDT.TextMode = TextBoxMode.Date;
            txtFascia.Text = mc.Fascia.ToString();
            txtProtocollo.Text = mc.ProtocolloInterno.ToString();
            txtManodopQta.Text = mc.Qta.ToString();
            txtPzzoManodop.Text = mc.PzzoUniCantiere.ToString();
            chkManodopVisibile.Checked = mc.Visibile;
            chkManodopRicalcolo.Checked = mc.Ricalcolo;
            chkManodopRicaricoSiNo.Checked = mc.RicaricoSiNo;
            txtNote1.Text = mc.Note;
            txtNote2.Text = mc.Note2;

            if (mc.DescriMateriali.ToString() != "NULL")
                txtDescrManodop.Text = mc.DescriMateriali.ToString();
            else
                txtDescrManodop.Text = "";
        }
        protected void btnModManodop_Click(object sender, EventArgs e)
        {
            if ((Convert.ToDecimal(txtManodopQta.Text) > 0 && txtManodopQta.Text != ""))
            {
                Operai op = OperaiDAO.GetSingle(Convert.ToInt32(ddlScegliAcquirente.SelectedItem.Value));
                bool isUpdated = MaterialiCantieriDAO.UpdateMatCant(new MaterialiCantieri
                {
                    IdMaterialiCantiere = Convert.ToInt32(hidManodop.Value),
                    CodArt = "Manodopera" + op.Operaio,
                    DescriCodArt = "Manodopera" + op.Operaio,
                    IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                    Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                    Fornitore = Enumeratori.Fornitori.Manodopera.GetHashCode().ToString(), //ddlScegliFornit.SelectedItem.Value,
                    Tipologia = txtTipDatCant.Text,
                    NumeroBolla = txtNumBolla.Text,
                    Data = Convert.ToDateTime(txtDataDDT.Text),
                    Fascia = Convert.ToInt32(txtFascia.Text),
                    ProtocolloInterno = Convert.ToInt32(txtProtocollo.Text),
                    DescriMateriali = txtDescrManodop.Text,
                    Note = txtNote1.Text,
                    Note2 = txtNote2.Text,
                    Qta = Convert.ToDouble(txtManodopQta.Text.Replace(".", ",")),
                    PzzoUniCantiere = Convert.ToDecimal(txtPzzoManodop.Text.Replace(".", ",")),
                    Visibile = chkManodopVisibile.Checked,
                    Ricalcolo = chkManodopRicalcolo.Checked,
                    RicaricoSiNo = chkManodopRicaricoSiNo.Checked
                });

                if (isUpdated)
                {
                    lblIsManodopInserita.Text = "Record modificato con successo";
                    lblIsManodopInserita.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante la modifica del record");
                }

                BindGridManodop();
                SvuotaCampi(pnlManodopera);

                btnInsManodop.Visible = true;
                btnModManodop.Visible = false;
                lblTitoloMaschera.Text = "Inserisci Manodopera";
            }
            else
            {
                (Master as layout).SetModal($"La Quantità deve essere maggiore di 0");
            }
        }

        /* EVENTI TEXT-CHANGED */
        protected void txtManodopQta_TextChanged(object sender, EventArgs e)
        {
            HideMessageLabels();
        }

        //Aggiornamento del valore della manodopera per il cantiere corrente
        protected void btnAggiornaValManodop_Click(object sender, EventArgs e)
        {
            if (txtAggiornaValManodop.Text != "" && txtAggiornaValManodop.Text != "0")
            {
                bool isUpdated = MaterialiCantieriDAO.UpdateValoreManodopera(ddlScegliCant.SelectedItem.Value, txtAggiornaValManodop.Text);
                if (isUpdated)
                {
                    lblIsManodopInserita.Text = "Valore della manodopera modificato con successo";
                    lblIsManodopInserita.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante la modifica del valore della manodopera");
                }
            }
            else
            {
                (Master as layout).SetModal($"Il campo \"Nuovo Valore Manodopera\" NON può essere nè vuoto nè 0");
            }

            BindGridManodop();
        }
        #endregion

        #region Operaio
        /* HELPERS */
        protected void BindGridOper()
        {
            List<MaterialiCantieri> mcList = MaterialiCantieriDAO.GetMaterialeCantiereForGridView(ddlScegliCant.SelectedItem.Value, txtFiltroOperCodArt.Text,
                txtFiltroOperDescriCodArt.Text, txtFiltroProtocolloGrdMatCant.Text, txtFiltroFornitoreGrdMatCant.Text, "OPERAIO");
            grdOperai.DataSource = mcList;
            grdOperai.DataBind();
        }

        /* EVENTI CLICK */
        protected void btnInsOper_Click(object sender, EventArgs e)
        {
            bool isInserito = false;

            if (IsDateNotSet())
                return;

            Operai op = OperaiDAO.GetSingle(Convert.ToInt32(ddlScegliOperaio.SelectedItem.Value));
            if (Convert.ToDecimal(txtOperQta.Text) > 0 && txtOperQta.Text != "")
            {
                if (IsIntestazioneCompilata())
                    isInserito = MaterialiCantieriDAO.InserisciMaterialeCantiere(new MaterialiCantieri
                    {
                        CodArt = "Manodopera" + op.Operaio,
                        DescriCodArt = "Manodopera" + op.Operaio,
                        IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                        Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                        Fornitore = Enumeratori.Fornitori.Operaio.GetHashCode().ToString(), //ddlScegliFornit.SelectedItem.Value,
                        DescriMateriali = txtDescrOper.Text,
                        Qta = Convert.ToDouble(txtOperQta.Text.Replace(".", ",")),
                        Visibile = chkOperVisibile.Checked,
                        Ricalcolo = chkOperRicalcolo.Checked,
                        RicaricoSiNo = chkOperRicaricoSiNo.Checked,
                        Tipologia = txtTipDatCant.Text,
                        ProtocolloInterno = Convert.ToInt32(txtProtocollo.Text),
                        PzzoUniCantiere = Convert.ToDecimal(txtPzzoOper.Text.Replace(".", ",")),
                        Data = Convert.ToDateTime(txtDataDDT.Text),
                        Note = txtOperNote1.Text,
                        Note2 = txtOperNote2.Text,
                        NumeroBolla = txtNumBolla.Text,
                        Fascia = Convert.ToInt32(txtFascia.Text),
                        IdTblOperaio = Convert.ToInt32(ddlScegliOperaio.SelectedItem.Value),
                    });

                if (isInserito)
                {
                    lblIsOperInserita.Text = "Record inserito con successo";
                    lblIsOperInserita.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante l'inserimento del record. L'intestazione deve essere interamente compilata.");
                }
            }
            else
            {
                (Master as layout).SetModal($"La Quantità deve essere maggiore di 0");
            }

            BindGridOper();
            SvuotaCampi(pnlGestioneOperaio);
        }
        //Visibilità pannello
        protected void btnGestOper_Click(object sender, EventArgs e)
        {
            txtPzzoOper.Text = OperaiDAO.GetSingle(Convert.ToInt32(ddlScegliOperaio.SelectedItem.Value))?.CostoOperaio.ToString("N2") ?? "";
            lblTitoloMaschera.Text = "Gestione Operaio";
            txtTipDatCant.Text = "OPERAIO";
            ShowPanels(false, false, false, true, false, false, false, false);
            btnModOper.Visible = false;
            BindGridOper();
            EnableDisableControls(true, pnlGestioneOperaio);
            SvuotaCampi(pnlGestioneOperaio);
            ChooseFornitore("Operaio");
            HideMessageLabels();
            txtFiltroAnnoDDT.Text = txtFiltroN_DDT.Text = "";
        }

        /* EVENTI TEXT-CHANGED */
        protected void ddlScegliOperaio_TextChanged(object sender, EventArgs e)
        {
            txtPzzoOper.Text = OperaiDAO.GetSingle(Convert.ToInt32(ddlScegliOperaio.SelectedItem.Value))?.CostoOperaio.ToString() ?? "0";
        }

        /* EVENTI PER IL ROWCOMMAND */
        protected void btnOperFiltraGrd_Click(object sender, EventArgs e)
        {
            BindGridOper();
        }
        protected void grdOperai_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idOper = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "VisualOper")
            {
                lblTitoloMaschera.Text = "Visualizza Operaio";
                btnInsOper.Visible = btnModOper.Visible = false;
                PopolaCampiOper(idOper, false);
                HideMessageLabels();
            }
            else if (e.CommandName == "ModOper")
            {
                lblTitoloMaschera.Text = "Modifica Operaio";
                btnInsOper.Visible = false;
                btnModOper.Visible = !btnInsOper.Visible;
                hidOper.Value = idOper.ToString();
                PopolaCampiOper(idOper, true);
                HideMessageLabels();
            }
            else if (e.CommandName == "ElimOper")
            {
                bool isDeleted = MaterialiCantieriDAO.DeleteMatCant(idOper);

                if (isDeleted)
                {
                    lblIsOperInserita.Text = "Record eliminato con successo";
                    lblIsOperInserita.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante l'eliminazione del record");
                }

                BindGridOper();
            }
        }
        private void PopolaCampiOper(int idOper, bool enableControls)
        {
            //Rendo i textbox abilitati/disabilitati
            EnableDisableControls(enableControls, pnlGestioneOperaio);

            MaterialiCantieri mc = MaterialiCantieriDAO.GetSingle(idOper);
            ddlScegliAcquirente.SelectedValue = mc.Acquirente;
            ddlScegliFornit.SelectedValue = mc.Fornitore;
            ddlScegliOperaio.SelectedValue = mc.IdTblOperaio.ToString();
            txtTipDatCant.Text = mc.Tipologia;
            txtNumBolla.Text = mc.NumeroBolla.ToString();
            txtDataDDT.Text = mc.Data.ToString("yyyy-MM-dd");
            txtDataDDT.TextMode = TextBoxMode.Date;
            txtFascia.Text = mc.Fascia.ToString();
            txtProtocollo.Text = mc.ProtocolloInterno.ToString();
            txtOperQta.Text = mc.Qta.ToString();
            txtPzzoOper.Text = mc.PzzoUniCantiere.ToString();
            chkOperVisibile.Checked = mc.Visibile;
            chkOperRicalcolo.Checked = mc.Ricalcolo;
            chkOperRicaricoSiNo.Checked = mc.RicaricoSiNo;
            txtNote1.Text = mc.Note;
            txtNote2.Text = mc.Note2;
            txtDescrOper.Text = mc.DescriMateriali.ToString() != "NULL" ? mc.DescriMateriali.ToString() : "";
        }
        protected void btnModOper_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtOperQta.Text.Replace(".", ",")) > 0 && txtOperQta.Text != "")
            {
                Operai op = OperaiDAO.GetSingle(Convert.ToInt32(ddlScegliOperaio.SelectedItem.Value));
                bool isUpdated = MaterialiCantieriDAO.UpdateMatCant(new MaterialiCantieri
                {
                    IdMaterialiCantiere = Convert.ToInt32(hidOper.Value),
                    CodArt = "Manodopera" + op.Operaio,
                    DescriCodArt = "Manodopera" + op.Operaio,
                    IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                    IdTblOperaio = Convert.ToInt32(ddlScegliOperaio.SelectedItem.Value),
                    Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                    Fornitore = Enumeratori.Fornitori.Operaio.GetHashCode().ToString(), //ddlScegliFornit.SelectedItem.Value,
                    Tipologia = txtTipDatCant.Text,
                    NumeroBolla = txtNumBolla.Text,
                    Data = Convert.ToDateTime(txtDataDDT.Text),
                    Fascia = Convert.ToInt32(txtFascia.Text),
                    ProtocolloInterno = Convert.ToInt32(txtProtocollo.Text),
                    DescriMateriali = txtDescrOper.Text,
                    Note = txtNote1.Text,
                    Note2 = txtNote2.Text,
                    Qta = Convert.ToDouble(txtOperQta.Text.Replace(".", ",")),
                    PzzoUniCantiere = Convert.ToDecimal(txtPzzoOper.Text.Replace(".", ",")),
                    Visibile = chkOperVisibile.Checked,
                    Ricalcolo = chkOperRicalcolo.Checked,
                    RicaricoSiNo = chkOperRicaricoSiNo.Checked
                });

                if (isUpdated)
                {
                    lblIsOperInserita.Text = "Record modificato con successo";
                    lblIsOperInserita.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante la modifica del record");
                }

                BindGridOper();
                SvuotaCampi(pnlGestioneOperaio);

                btnInsOper.Visible = true;
                btnModOper.Visible = !btnInsOper.Visible;
                lblTitoloMaschera.Text = "Inserisci Operaio";
            }
            else
            {
                (Master as layout).SetModal($"La Quantità deve essere maggiore di 0");
            }
        }

        //Aggiornamento Costo Operaio
        protected void btnNuovoCostoOperaio_Click(object sender, EventArgs e)
        {
            if (ddlScegliOperaio.SelectedIndex != 0)
            {
                if (txtNuovoCostoOperaio.Text != "" && txtNuovoCostoOperaio.Text != "0")
                {
                    bool isUpdated = MaterialiCantieriDAO.UpdateCostoOperaio(ddlScegliCant.SelectedItem.Value, txtNuovoCostoOperaio.Text.Replace(".", ","), ddlScegliOperaio.SelectedItem.Value);
                    if (isUpdated)
                    {
                        lblIsOperInserita.Text = "Costo operaio modificato con successo";
                        lblIsOperInserita.ForeColor = Color.Blue;
                    }
                    else
                    {
                        (Master as layout).SetModal($"Errore durante la modifica del costo operaio");
                    }
                }
                else
                {
                    (Master as layout).SetModal($"Il campo \"Nuovo Costo Operaio\" NON può essere nè vuoto nè 0");
                }
            }
            else
            {
                (Master as layout).SetModal($"È necessario scegliere un Operaio prima di modificarne il costo");
            }

            BindGridOper();
        }

        /* TEXT CHANGED */
        protected void txtOperQta_TextChanged(object sender, EventArgs e)
        {
            HideMessageLabels();
        }

        #endregion

        #region Arrotondamento
        /* HELPERS */
        protected void BindGridArrot()
        {
            List<MaterialiCantieri> mcList = MaterialiCantieriDAO.GetMaterialeCantiereForGridView(ddlScegliCant.SelectedItem.Value, txtFiltroArrotCodArt.Text,
                txtFiltroArrotDescriCodArt.Text, txtFiltroProtocolloGrdMatCant.Text, txtFiltroFornitoreGrdMatCant.Text, "ARROTONDAMENTO");
            grdArrot.DataSource = mcList;
            grdArrot.DataBind();
        }
        private void PopolaCampiArrot(int idArrot, bool enableControls)
        {
            MaterialiCantieri mc = MaterialiCantieriDAO.GetSingle(idArrot);

            //Rendo i textbox abilitati/disabilitati
            EnableDisableControls(enableControls, pnlGestArrotond);

            ddlScegliAcquirente.SelectedValue = mc.Acquirente;
            ddlScegliFornit.SelectedValue = mc.Fornitore;
            txtTipDatCant.Text = mc.Tipologia;
            txtNumBolla.Text = mc.NumeroBolla.ToString();
            txtDataDDT.Text = mc.Data.ToString("yyyy-MM-dd");
            txtDataDDT.TextMode = TextBoxMode.Date;
            txtFascia.Text = mc.Fascia.ToString();
            txtArrotCodArt.Text = mc.CodArt;
            txtArrotDescriCodArt.Text = mc.DescriCodArt;
            txtProtocollo.Text = mc.ProtocolloInterno.ToString();
            txtArrotQta.Text = mc.Qta.ToString();
            txtArrotPzzoUnit.Text = mc.PzzoUniCantiere.ToString();
            chkArrotVisibile.Checked = mc.Visibile;
            chkArrotRicalcolo.Checked = mc.Ricalcolo;
            chkArrotRicaricoSiNo.Checked = mc.RicaricoSiNo;
        }

        /* EVENTI CLICK */
        protected void btnInsArrot_Click(object sender, EventArgs e)
        {
            if (IsDateNotSet())
                return;

            if (Convert.ToDecimal(txtArrotQta.Text) > 0 && txtArrotQta.Text != "")
            {
                if (IsIntestazioneCompilata())
                {
                    bool isInserito = MaterialiCantieriDAO.InserisciMaterialeCantiere(new MaterialiCantieri
                    {
                        IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                        Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                        Fornitore = Enumeratori.Fornitori.Arrotondamento.GetHashCode().ToString(), //ddlScegliFornit.SelectedItem.Value,
                        Tipologia = txtTipDatCant.Text,
                        NumeroBolla = txtNumBolla.Text,
                        Data = Convert.ToDateTime(txtDataDDT.Text),
                        Fascia = Convert.ToInt32(txtFascia.Text),
                        ProtocolloInterno = Convert.ToInt32(txtProtocollo.Text),
                        Qta = Convert.ToDouble(txtArrotQta.Text),
                        PzzoUniCantiere = Convert.ToDecimal(txtArrotPzzoUnit.Text.Replace('.', ',')),
                        CodArt = txtArrotCodArt.Text,
                        DescriCodArt = txtArrotDescriCodArt.Text,
                        Visibile = chkArrotVisibile.Checked,
                        Ricalcolo = chkArrotRicalcolo.Checked,
                        RicaricoSiNo = chkArrotRicaricoSiNo.Checked
                    });
                    if (isInserito)
                    {
                        lblIsArrotondInserito.Text = "Record inserito con successo";
                        lblIsArrotondInserito.ForeColor = Color.Blue;
                    }
                    else
                    {
                        (Master as layout).SetModal($"Errore durante l'inserimento del record. L'intestazione deve essere interamente compilata.");
                    }
                }
            }
            else
            {
                (Master as layout).SetModal($"La Quantità deve essere maggiore di 0");
            }

            BindGridArrot();
            SvuotaCampi(pnlGestArrotond);
        }
        protected void btnModArrot_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtArrotQta.Text) > 0 && txtArrotQta.Text != "")
            {
                bool isUpdated = MaterialiCantieriDAO.UpdateMatCant(new MaterialiCantieri
                {
                    IdMaterialiCantiere = Convert.ToInt32(hidArrot.Value),
                    IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                    Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                    Fornitore = Enumeratori.Fornitori.Arrotondamento.GetHashCode().ToString(), //ddlScegliFornit.SelectedItem.Value,
                    Tipologia = txtTipDatCant.Text,
                    NumeroBolla = txtNumBolla.Text,
                    Data = Convert.ToDateTime(txtDataDDT.Text),
                    Fascia = Convert.ToInt32(txtFascia.Text),
                    ProtocolloInterno = Convert.ToInt32(txtProtocollo.Text),
                    Qta = Convert.ToDouble(txtArrotQta.Text),
                    PzzoUniCantiere = Convert.ToDecimal(txtArrotPzzoUnit.Text.Replace('.', ',')),
                    CodArt = txtArrotCodArt.Text,
                    DescriCodArt = txtArrotDescriCodArt.Text,
                    Visibile = chkArrotVisibile.Checked,
                    Ricalcolo = chkArrotRicalcolo.Checked,
                    RicaricoSiNo = chkArrotRicaricoSiNo.Checked
                });

                if (isUpdated)
                {
                    lblIsArrotondInserito.Text = "Record modificato con successo";
                    lblIsArrotondInserito.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante la modifica del record");
                }

                BindGridArrot();
                SvuotaCampi(pnlGestArrotond);

                btnInsArrot.Visible = true;
                btnModArrot.Visible = !btnInsArrot.Visible;
                lblTitoloMaschera.Text = "Inserisci Arrotondamento";
            }
            else
            {
                (Master as layout).SetModal($"La Quantità deve essere maggiore di 0");
            }
        }
        protected void btnGestArrot_Click(object sender, EventArgs e)
        {
            lblTitoloMaschera.Text = "Gestione Arrotondamenti";
            txtTipDatCant.Text = "ARROTONDAMENTO";
            ShowPanels(false, false, false, false, true, false, false, false);
            btnModArrot.Visible = false;
            BindGridArrot();
            EnableDisableControls(true, pnlGestArrotond);
            SvuotaCampi(pnlGestArrotond);
            ChooseFornitore("Arrotondamento");
            HideMessageLabels();
            txtFiltroAnnoDDT.Text = txtFiltroN_DDT.Text = "";
        }
        protected void btnArrotFiltraGrd_Click(object sender, EventArgs e)
        {
            BindGridArrot();
        }

        /* EVENTI PER IL ROWCOMMAND */
        protected void grdArrot_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idArrot = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "VisualArrot")
            {
                lblTitoloMaschera.Text = "Visualizza Arrotondamento";
                btnInsArrot.Visible = btnModArrot.Visible = false;
                PopolaCampiArrot(idArrot, false);
                btnInsArrot.Visible = btnModArrot.Visible = false;
                HideMessageLabels();
            }
            else if (e.CommandName == "ModArrot")
            {
                lblTitoloMaschera.Text = "Modifica Arrotondamento";
                btnInsArrot.Visible = false;
                btnModArrot.Visible = true;
                PopolaCampiArrot(idArrot, true);
                hidArrot.Value = idArrot.ToString();
                HideMessageLabels();
            }
            else if (e.CommandName == "ElimArrot")
            {
                bool isDeleted = MaterialiCantieriDAO.DeleteMatCant(idArrot);

                if (isDeleted)
                {
                    lblIsArrotondInserito.Text = "Record eliminato con successo";
                    lblIsArrotondInserito.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante l'eliminazione del record");
                }

                BindGridArrot();
            }
        }
        #endregion

        #region A Chiamata
        /* HELPERS */
        protected void BindGridChiamata()
        {
            List<MaterialiCantieri> mcList = MaterialiCantieriDAO.GetMaterialeCantiereForGridView(ddlScegliCant.SelectedItem.Value, txtFiltroAChiamCodArt.Text,
                txtFiltroAChiamDescriCodArt.Text, txtFiltroProtocolloGrdMatCant.Text, txtFiltroFornitoreGrdMatCant.Text, "A CHIAMATA");
            grdAChiam.DataSource = mcList;
            grdAChiam.DataBind();
        }
        private void PopolaCampiChiamata(int id, bool enableControls)
        {
            MaterialiCantieri mc = MaterialiCantieriDAO.GetSingle(id);

            //Rendo i textbox abilitati/disabilitati
            EnableDisableControls(enableControls, pnlGestChiamata);

            ddlScegliAcquirente.SelectedValue = mc.Acquirente;
            ddlScegliFornit.SelectedValue = mc.Fornitore;
            txtTipDatCant.Text = mc.Tipologia;
            txtNumBolla.Text = mc.NumeroBolla.ToString();
            txtDataDDT.Text = mc.Data.ToString("yyyy-MM-dd");
            txtDataDDT.TextMode = TextBoxMode.Date;
            txtFascia.Text = mc.Fascia.ToString();
            txtProtocollo.Text = mc.ProtocolloInterno.ToString();
            txtChiamCodArt.Text = mc.CodArt;
            txtChiamDescriCodArt.Text = mc.DescriCodArt;
            txtChiamDescrMate.Text = mc.DescriMateriali;
            txtChiamNote.Text = mc.Note;
            txtChiamNote.Text = mc.Note2;
            txtChiamQta.Text = mc.Qta.ToString();
            txtChiamPzzoUnit.Text = mc.PzzoUniCantiere.ToString();
            txtChiamPzzoFinCli.Text = mc.PzzoFinCli.ToString();
            chkChiamVisibile.Checked = mc.Visibile;
            chkChiamRicalcolo.Checked = mc.Ricalcolo;
            chkChiamRicaricoSiNo.Checked = mc.RicaricoSiNo;
            txtChiamNote.Text = mc.Note;
            txtChiamNote2.Text = mc.Note2;
        }

        /* EVENTI CLICK */
        protected void btnCalcolaPzzoUnitAChiam_Click(object sender, EventArgs e)
        {
            if (txtChiamPzzoNetto.Text != "")
                txtChiamPzzoUnit.Text = Math.Round(Convert.ToDecimal(txtChiamPzzoNetto.Text.Replace(".", ",")), 2).ToString();
            else
            {
                (Master as layout).SetModal($"Inserire un valore nella casella \"Prezzo Netto Mef\" per calcolare il \"Prezzo Unitario\"");
            }
        }
        protected void btnInsAChiam_Click(object sender, EventArgs e)
        {
            bool isInserito = false;

            if (IsDateNotSet())
                return;

            if ((Convert.ToDecimal(txtChiamQta.Text) > 0 && txtChiamQta.Text != "") && Convert.ToDecimal(txtChiamPzzoUnit.Text) > 0)
            {
                if (ddlScegliDDTMef.SelectedItem == null || ddlScegliDDTMef.SelectedItem.Text == "")
                {
                    if (txtNumBolla.Text == "")
                    {
                        (Master as layout).SetModal($"Scegliere un DDT dal menù a discesa o compilare il campo \"Numero Bolla\"");
                        return;
                    }
                }

                if (IsIntestazioneCompilata())
                {
                    if (txtChiamCodArt.Text != "" && txtChiamDescriCodArt.Text != "")
                    {
                        isInserito = MaterialiCantieriDAO.InserisciMaterialeCantiere(new MaterialiCantieri
                        {
                            IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                            DescriMateriali = txtChiamDescrMate.Text,
                            Visibile = chkChiamVisibile.Checked,
                            Ricalcolo = chkChiamRicalcolo.Checked,
                            RicaricoSiNo = chkChiamRicaricoSiNo.Checked,
                            Data = Convert.ToDateTime(txtDataDDT.Text),
                            PzzoUniCantiere = Convert.ToDecimal(txtChiamPzzoUnit.Text),
                            CodArt = txtChiamCodArt.Text,
                            DescriCodArt = txtChiamDescriCodArt.Text,
                            Tipologia = txtTipDatCant.Text,
                            Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                            Fornitore = Enumeratori.Fornitori.AChiamata.GetHashCode().ToString(), //ddlScegliFornit.SelectedItem.Value,
                            Note = txtChiamNote.Text,
                            Note2 = txtChiamNote2.Text,
                            Qta = Convert.ToDouble(txtChiamQta.Text),
                            Fascia = txtFascia.Text != "" ? Convert.ToInt32(txtFascia.Text) : 0,
                            ProtocolloInterno = txtProtocollo.Text != "" ? Convert.ToInt32(txtProtocollo.Text) : 0,
                            NumeroBolla = txtNumBolla.Enabled && txtNumBolla.Text != ""
                             ? txtNumBolla.Text
                             : ddlScegliDDTMef.SelectedIndex != -1 ? (ddlScegliDDTMef.SelectedItem.Text).Split('-')[3] : "",
                            PzzoFinCli = txtChiamPzzoFinCli.Text != "" ? Convert.ToDecimal(txtChiamPzzoFinCli.Text) : 0.0m
                        });
                    }
                    else
                    {
                        (Master as layout).SetModal($"Codice Articolo e Descrizione Codice Articolo obbligatori");
                        return;
                    }
                }

                if (isInserito)
                {
                    lblIsAChiamInserita.Text = "Record inserito con successo";
                    lblIsAChiamInserita.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante l'inserimento del record. L'intestazione deve essere interamente compilata.");
                }
            }
            else
            {
                (Master as layout).SetModal($"Quantità e/o Prezzo Unitario devono essere maggiori di 0");
            }

            BindGridChiamata();
            SvuotaCampi(pnlGestChiamata);
        }
        protected void btnGestChiam_Click(object sender, EventArgs e)
        {
            lblTitoloMaschera.Text = "Inserisci A Chiamata";
            txtTipDatCant.Text = "A CHIAMATA";
            ShowPanels(false, false, false, false, false, false, true, false);
            btnInsAChiam.Visible = true;
            btnModAChiam.Visible = !btnInsAChiam.Visible;
            BindGridChiamata();
            EnableDisableControls(true, pnlGestChiamata);
            SvuotaCampi(pnlGestChiamata);
            ChooseFornitore("A Chiamata");
            HideMessageLabels();
            txtFiltroAnnoDDT.Text = txtFiltroN_DDT.Text = "";
        }

        /* Eventi Per il RowCommand */
        protected void grdAChiam_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idChiamata = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "VisualChiam")
            {
                lblTitoloMaschera.Text = "Visualizza A Chiamata";
                btnInsAChiam.Visible = btnModAChiam.Visible = false;
                PopolaCampiChiamata(idChiamata, false);
                HideMessageLabels();
            }
            else if (e.CommandName == "ModChiam")
            {
                lblTitoloMaschera.Text = "Modifica A Chiamata";
                btnInsAChiam.Visible = false;
                btnModAChiam.Visible = true;
                PopolaCampiChiamata(idChiamata, true);
                hidAChiamata.Value = idChiamata.ToString();
                HideMessageLabels();
            }
            else if (e.CommandName == "ElimChiam")
            {
                bool isDeleted = MaterialiCantieriDAO.DeleteMatCant(idChiamata);

                if (isDeleted)
                {
                    lblIsAChiamInserita.Text = "Record eliminato con successo";
                    lblIsAChiamInserita.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante l'eliminazione del record");
                }

                BindGridChiamata();
            }
        }
        protected void btnFiltraGrdAChiam_Click(object sender, EventArgs e)
        {
            BindGridChiamata();
        }
        protected void btnModAChiam_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtChiamQta.Text) > 0 && txtChiamQta.Text != "" && Convert.ToDecimal(txtChiamPzzoUnit.Text) > 0)
            {
                bool isUpdated = MaterialiCantieriDAO.UpdateMatCant(new MaterialiCantieri
                {
                    IdMaterialiCantiere = Convert.ToInt32(hidAChiamata.Value),
                    IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                    Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                    Fornitore = Enumeratori.Fornitori.AChiamata.GetHashCode().ToString(), //ddlScegliFornit.SelectedItem.Value,
                    Tipologia = txtTipDatCant.Text,
                    NumeroBolla = txtNumBolla.Text,
                    Data = Convert.ToDateTime(txtDataDDT.Text),
                    Fascia = Convert.ToInt32(txtFascia.Text),
                    ProtocolloInterno = Convert.ToInt32(txtProtocollo.Text),
                    CodArt = txtChiamCodArt.Text,
                    DescriCodArt = txtChiamDescriCodArt.Text,
                    DescriMateriali = txtChiamDescrMate.Text,
                    Note = txtChiamNote.Text,
                    Note2 = txtChiamNote2.Text,
                    Qta = Convert.ToDouble(txtChiamQta.Text),
                    PzzoUniCantiere = Convert.ToDecimal(txtChiamPzzoUnit.Text),
                    PzzoFinCli = Convert.ToDecimal(txtChiamPzzoFinCli.Text),
                    Visibile = chkChiamVisibile.Checked,
                    Ricalcolo = chkChiamRicalcolo.Checked,
                    RicaricoSiNo = chkChiamRicaricoSiNo.Checked
                });

                if (isUpdated)
                {
                    lblIsAChiamInserita.Text = "Record modificato con successo";
                    lblIsAChiamInserita.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante la modifica del record");
                }

                BindGridChiamata();
                SvuotaCampi(pnlGestChiamata);

                btnInsAChiam.Visible = true;
                btnModAChiam.Visible = false;
                lblTitoloMaschera.Text = "Inserisci A Chiamata";

            }
            else
            {
                (Master as layout).SetModal($"Quantità e/o Prezzo Unitario devono essere maggiori di 0");
            }
        }

        /* TEXT-CHANGED */
        protected void txtChiamCodArt_TextChanged(object sender, EventArgs e)
        {
            HideMessageLabels();
        }
        protected void txtChiamDescriCodArt_TextChanged(object sender, EventArgs e)
        {
            HideMessageLabels();
        }
        #endregion

        #region Spese
        /* HELPERS */
        protected void BindGridSpese()
        {
            List<MaterialiCantieri> mcList = MaterialiCantieriDAO.GetMaterialeCantiereForGridView(ddlScegliCant.SelectedItem.Value, txtFiltroAChiamCodArt.Text,
                txtFiltroAChiamDescriCodArt.Text, txtFiltroProtocolloGrdMatCant.Text, txtFiltroFornitoreGrdMatCant.Text, "SPESE");
            grdSpese.DataSource = mcList;
            grdSpese.DataBind();
        }
        private void PopolaCampiSpese(int id, bool enableControls)
        {
            MaterialiCantieri mc = MaterialiCantieriDAO.GetSingle(id);

            //Rendo i textbox abilitati/disabilitati
            EnableDisableControls(enableControls, pnlGestSpese);

            ddlScegliAcquirente.SelectedValue = mc.Acquirente;
            ddlScegliFornit.SelectedValue = mc.Fornitore;
            txtTipDatCant.Text = mc.Tipologia;
            txtNumBolla.Text = mc.NumeroBolla.ToString();
            txtDataDDT.Text = mc.Data.ToString("yyyy-MM-dd");
            txtDataDDT.TextMode = TextBoxMode.Date;
            txtFascia.Text = mc.Fascia.ToString();
            txtProtocollo.Text = mc.ProtocolloInterno.ToString();
            txtSpeseCodArt.Text = mc.CodArt;
            txtSpeseDescriCodArt.Text = mc.DescriCodArt;
            txtSpeseQta.Text = mc.Qta.ToString();
            txtSpesaPrezzoCalcolato.Text = mc.PzzoUniCantiere.ToString();
            chkSpesaVisibile.Checked = mc.Visibile;
            chkSpesaRicalcolo.Checked = mc.Ricalcolo;
            chkSpesaRicarico.Checked = mc.RicaricoSiNo;
        }

        /* EVENTI CLICK */
        protected void btnGestSpese_Click(object sender, EventArgs e)
        {
            lblTitoloMaschera.Text = "Inserisci Spese";
            txtTipDatCant.Text = "SPESE";
            ShowPanels(false, false, false, false, false, true, false, false);
            btnInsSpesa.Visible = true;
            btnModSpesa.Visible = false;
            BindGridSpese();
            EnableDisableControls(true, pnlGestSpese);
            SvuotaCampi(pnlGestSpese);
            ChooseFornitore("Spese");
            HideMessageLabels();
            txtFiltroAnnoDDT.Text = txtFiltroN_DDT.Text = "";
        }
        protected void btnFiltraGrdSpese_Click(object sender, EventArgs e)
        {
            BindGridSpese();
        }
        protected void btnCalcolaPzzoUnitSpese_Click(object sender, EventArgs e)
        {
            if (txtSpesaPrezzoCalcolato.Text != "" && txtSpesaPrezzo.Text != "")
                txtSpesaPrezzoCalcolato.Text = Math.Round(Convert.ToDecimal(txtSpesaPrezzo.Text.Replace(".", ",")), 2).ToString();
            else
            {
                (Master as layout).SetModal($"Inserire un valore nella casella \"Prezzo\" per calcolare il \"Prezzo Calcolato\"");
            }

            btnInsAChiam.Focus();
        }
        protected void btnInsSpesa_Click(object sender, EventArgs e)
        {
            if (IsDateNotSet())
                return;

            if (Convert.ToDecimal(txtSpeseQta.Text) > 0 && txtSpeseQta.Text != "" && Convert.ToDecimal(txtSpesaPrezzoCalcolato.Text) > 0)
            {
                if (IsIntestazioneCompilata())
                {
                    bool isInserito = MaterialiCantieriDAO.InserisciMaterialeCantiere(new MaterialiCantieri
                    {
                        IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                        DescriMateriali = txtSpeseDescriCodArt.Text,
                        Visibile = chkSpesaVisibile.Checked,
                        Ricalcolo = chkSpesaRicalcolo.Checked,
                        RicaricoSiNo = chkSpesaRicarico.Checked,
                        Data = Convert.ToDateTime(txtDataDDT.Text),
                        PzzoUniCantiere = Convert.ToDecimal(txtSpesaPrezzoCalcolato.Text),
                        CodArt = txtSpeseCodArt.Text,
                        DescriCodArt = txtSpeseDescriCodArt.Text,
                        Tipologia = txtTipDatCant.Text,
                        Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                        Fornitore = Enumeratori.Fornitori.Spese.GetHashCode().ToString(), //ddlScegliFornit.SelectedItem.Value,
                        Qta = txtSpeseQta.Text != "" ? Convert.ToDouble(txtSpeseQta.Text) : 0,
                        Fascia = txtFascia.Text != "" ? Convert.ToInt32(txtFascia.Text) : 0,
                        ProtocolloInterno = txtProtocollo.Text != "" ? Convert.ToInt32(txtProtocollo.Text) : 0,
                        NumeroBolla = txtNumBolla.Enabled && txtNumBolla.Text != ""
                            ? txtNumBolla.Text
                            : ddlScegliDDTMef.SelectedIndex != -1 ? (ddlScegliDDTMef.SelectedItem.Text).Split('-')[3] : ""
                    });

                    if (isInserito)
                    {
                        lblIsSpesaInserita.Text = "Record inserito con successo";
                        lblIsSpesaInserita.ForeColor = Color.Blue;
                    }
                    else
                    {
                        (Master as layout).SetModal($"Errore durante l'inserimento del record. L'intestazione deve essere interamente compilata.");
                        return;
                    }
                }
            }
            else
            {
                (Master as layout).SetModal($"Il valore della quantità e/o del prezzo devono essere maggiori di \"0\"");
                return;
            }

            BindGridSpese();
            SvuotaCampi(pnlGestSpese);
            ddlScegliSpesa.SelectedIndex = 0;
        }

        /* EVENTI TEXT-CHANGED */
        protected void ddlScegliSpesa_TextChanged(object sender, EventArgs e)
        {
            Spese spesa = SpeseDAO.GetDettagliSpesa(ddlScegliSpesa.SelectedItem.Value);
            txtSpeseCodArt.Text = txtSpeseDescriCodArt.Text = spesa.Descrizione;
            txtSpesaPrezzo.Text = spesa.Prezzo.ToString("N2");
            HideMessageLabels();
        }
        protected void txtSpeseCodArt_TextChanged(object sender, EventArgs e)
        {
            HideMessageLabels();
        }
        protected void txtSpeseDescriCodArt_TextChanged(object sender, EventArgs e)
        {
            HideMessageLabels();
        }

        /* EVENTI ROW-COMMAND */
        protected void grdSpese_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idSpesa = Convert.ToInt32(e.CommandArgument.ToString());

            if (e.CommandName == "VisualSpesa")
            {
                lblTitoloMaschera.Text = "Visualizza Spese";
                btnInsSpesa.Visible = btnModSpesa.Visible = false;
                PopolaCampiSpese(idSpesa, false);
                HideMessageLabels();
            }
            else if (e.CommandName == "ModSpesa")
            {
                lblTitoloMaschera.Text = "Modifica Spese";
                btnInsSpesa.Visible = false;
                btnModSpesa.Visible = !btnInsSpesa.Visible;
                hidIdSpesa.Value = idSpesa.ToString();
                PopolaCampiSpese(idSpesa, true);
                HideMessageLabels();
            }
            else if (e.CommandName == "ElimSpesa")
            {
                bool isDeleted = MaterialiCantieriDAO.DeleteMatCant(idSpesa);

                if (isDeleted)
                {
                    lblIsSpesaInserita.Text = "Record eliminato con successo";
                    lblIsSpesaInserita.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante l'eliminazione del record");
                }

                BindGridSpese();
            }
        }
        protected void btnModSpesa_Click(object sender, EventArgs e)
        {
            if (Convert.ToDecimal(txtSpeseQta.Text) > 0 && txtSpeseQta.Text != "")
            {
                bool isUpdated = MaterialiCantieriDAO.UpdateMatCant(new MaterialiCantieri
                {
                    IdMaterialiCantiere = Convert.ToInt32(hidIdSpesa.Value),
                    IdTblCantieri = Convert.ToInt32(ddlScegliCant.SelectedItem.Value),
                    Acquirente = ddlScegliAcquirente.SelectedItem.Value,
                    Fornitore = Enumeratori.Fornitori.Spese.GetHashCode().ToString(), //ddlScegliFornit.SelectedItem.Value,
                    Tipologia = txtTipDatCant.Text,
                    NumeroBolla = txtNumBolla.Text,
                    Data = Convert.ToDateTime(txtDataDDT.Text),
                    Fascia = Convert.ToInt32(txtFascia.Text),
                    ProtocolloInterno = Convert.ToInt32(txtProtocollo.Text),
                    CodArt = txtSpeseCodArt.Text,
                    DescriCodArt = txtSpeseDescriCodArt.Text,
                    Qta = Convert.ToDouble(txtSpeseQta.Text),
                    PzzoUniCantiere = Convert.ToDecimal(txtSpesaPrezzoCalcolato.Text),
                    Visibile = chkSpesaVisibile.Checked,
                    Ricalcolo = chkSpesaRicalcolo.Checked,
                    RicaricoSiNo = chkSpesaRicarico.Checked
                });

                if (isUpdated)
                {
                    lblIsSpesaInserita.Text = "Record modificato con successo";
                    lblIsSpesaInserita.ForeColor = Color.Blue;
                }
                else
                {
                    (Master as layout).SetModal($"Errore durante la modifica del record");
                }

                BindGridSpese();
                SvuotaCampi(pnlGestSpese);

                btnInsSpesa.Visible = true;
                btnModSpesa.Visible = !btnInsSpesa.Visible;
                lblTitoloMaschera.Text = "Inserisci Spese";
            }
            else
            {
                (Master as layout).SetModal($"La Quantità deve essere maggiore di 0");
            }
        }
        #endregion

        #region Generate PDF
        public void CreatePdf(string filePath, string fileName, Cantieri cant)
        {
            int tableDimension = 7;
            PdfWriter writer = new PdfWriter(filePath);
            PdfDocument pdf = new PdfDocument(writer);
            Document document = new Document(pdf, PageSize.A4.Rotate());
            document.SetMargins(20, 20, 20, 20);
            PdfFont font = PdfFontFactory.CreateFont(StandardFonts.HELVETICA);
            PdfFont bold = PdfFontFactory.CreateFont(StandardFonts.HELVETICA_BOLD);
            iText.Layout.Element.Table table = new iText.Layout.Element.Table(tableDimension).UseAllAvailableWidth();
            table.SetHorizontalAlignment(HorizontalAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);

            List<Operai> operai = OperaiDAO.GetAll();
            List<MaterialiCantieri> items = MaterialiCantieriDAO.GetByIdCantiere(cant.IdCantieri).Where(w => w.ProtocolloInterno == Convert.ToInt32(txtProtocollo.Text)).ToList();
            Cell emptyCellManodopera = new Cell(1, tableDimension).Add(new Paragraph("\n\n"));
            Cell emptyCellOperaio = new Cell(1, tableDimension).Add(new Paragraph("\n\n"));
            Cell emptyCellMateriale = new Cell(1, tableDimension).Add(new Paragraph("\n\n"));
            Cell emptyCellSpese = new Cell(1, tableDimension).Add(new Paragraph("\n\n"));
            Cell emptyCellAChiamata = new Cell(1, tableDimension).Add(new Paragraph("\n\n"));
            Cell emptyCellArrotondamento = new Cell(1, tableDimension).Add(new Paragraph("\n\n"));
            Cell emptyCellAccrediti = new Cell(1, tableDimension).Add(new Paragraph("\n\n"));
            //Cell emptyCellRientro = new Cell(1, tableDimension).Add(new Paragraph("\n\n"));

            if (items.Count > 0)
            {
                // Data e nome e numero protocollo
                table.AddHeaderCell(new Cell(1, tableDimension).Add(new Paragraph(items.FirstOrDefault().Data.ToString("dd/MM/yyyy")).SetFont(font)));
                table.AddHeaderCell(new Cell(1, tableDimension).Add(new Paragraph(fileName.Replace(".pdf", "")).SetFont(font)));

                // Intestazioni
                table.AddHeaderCell(new Cell().Add(new Paragraph("Articolo").SetFont(bold)).SetTextAlignment(TextAlignment.CENTER)).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddHeaderCell(new Cell().Add(new Paragraph("Descrizione").SetFont(bold)).SetTextAlignment(TextAlignment.CENTER)).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddHeaderCell(new Cell().Add(new Paragraph("Quantità").SetFont(bold)).SetTextAlignment(TextAlignment.CENTER)).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddHeaderCell(new Cell().Add(new Paragraph("Prezzo Unitario").SetFont(bold)).SetTextAlignment(TextAlignment.CENTER)).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddHeaderCell(new Cell().Add(new Paragraph("Note 1").SetFont(bold)).SetTextAlignment(TextAlignment.CENTER)).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddHeaderCell(new Cell().Add(new Paragraph("Note 2").SetFont(bold)).SetTextAlignment(TextAlignment.CENTER)).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddHeaderCell(new Cell().Add(new Paragraph("Totale").SetFont(bold)).SetTextAlignment(TextAlignment.CENTER)).SetVerticalAlignment(VerticalAlignment.MIDDLE);

                string tipologia = "";

                // Dati in tabella
                tipologia = Enumeratori.Tipologie.MANODOPERA.ToString().Replace("_", " ");
                List<MaterialiCantieri> manodoperaList = items.Where(w => w.Tipologia == tipologia).ToList();
                if (manodoperaList.Count > 0)
                {
                    table.AddCell(new Cell(1, tableDimension - 1).Add(new Paragraph(tipologia).SetFont(bold)));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"€ {manodoperaList.Sum(s => (decimal)s.Qta * s.PzzoUniCantiere):N2}").SetFont(bold)));
                    InsertDataIntoPdfTable(ref table, manodoperaList, operai, font);
                    table.AddCell(emptyCellManodopera);
                }

                tipologia = Enumeratori.Tipologie.OPERAIO.ToString().Replace("_", " ");
                List<MaterialiCantieri> operaioList = items.Where(w => w.Tipologia == tipologia).ToList();
                if (operaioList.Count > 0)
                {
                    table.AddCell(new Cell(1, tableDimension - 1).Add(new Paragraph(tipologia)).SetFont(bold));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"€ {operaioList.Sum(s => (decimal)s.Qta * s.PzzoUniCantiere):N2}").SetFont(bold)));
                    InsertDataIntoPdfTable(ref table, operaioList, operai, font);
                    table.AddCell(emptyCellOperaio);
                }

                tipologia = Enumeratori.Tipologie.MATERIALE.ToString().Replace("_", " ");
                List<MaterialiCantieri> materialeList = items.Where(w => w.Tipologia == tipologia).ToList();
                if (materialeList.Count > 0)
                {
                    table.AddCell(new Cell(1, tableDimension - 1).Add(new Paragraph(tipologia)).SetFont(bold));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"€ {materialeList.Sum(s => (decimal)s.Qta * s.PzzoUniCantiere):N2}").SetFont(bold)));
                    InsertDataIntoPdfTable(ref table, materialeList, operai, font);
                    table.AddCell(emptyCellMateriale);
                }

                tipologia = Enumeratori.Tipologie.SPESE.ToString().Replace("_", " ");
                List<MaterialiCantieri> speseList = items.Where(w => w.Tipologia == tipologia).ToList();
                if (speseList.Count > 0)
                {
                    table.AddCell(new Cell(1, tableDimension - 1).Add(new Paragraph(tipologia)).SetFont(bold));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"€ {speseList.Sum(s => (decimal)s.Qta * s.PzzoUniCantiere):N2}").SetFont(bold)));
                    InsertDataIntoPdfTable(ref table, speseList, operai, font);
                    table.AddCell(emptyCellSpese);
                }

                tipologia = Enumeratori.Tipologie.A_CHIAMATA.ToString().Replace("_", " ");
                List<MaterialiCantieri> aChiamataList = items.Where(w => w.Tipologia == tipologia).ToList();
                if (aChiamataList.Count > 0)
                {
                    table.AddCell(new Cell(1, tableDimension - 1).Add(new Paragraph(tipologia)).SetFont(bold));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"€ {aChiamataList.Sum(s => (decimal)s.Qta * s.PzzoUniCantiere):N2}").SetFont(bold)));
                    InsertDataIntoPdfTable(ref table, aChiamataList, operai, font);
                    table.AddCell(emptyCellAChiamata);
                }

                tipologia = Enumeratori.Tipologie.ARROTONDAMENTO.ToString().Replace("_", " ");
                List<MaterialiCantieri> arrotondamentoList = items.Where(w => w.Tipologia == tipologia).ToList();
                if (arrotondamentoList.Count > 0)
                {
                    table.AddCell(new Cell(1, tableDimension - 1).Add(new Paragraph(tipologia)).SetFont(bold));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"€ {arrotondamentoList.Sum(s => (decimal)s.Qta * s.PzzoUniCantiere):N2}").SetFont(bold)));
                    InsertDataIntoPdfTable(ref table, arrotondamentoList, operai, font);
                    table.AddCell(emptyCellArrotondamento);
                }

                tipologia = Enumeratori.Tipologie.ACCREDITI.ToString().Replace("_", " ");
                List<MaterialiCantieri> accreditiList = items.Where(w => w.Tipologia == tipologia).ToList();
                if (accreditiList.Count > 0)
                {
                    table.AddCell(new Cell(1, tableDimension - 1).Add(new Paragraph(tipologia)).SetFont(bold));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"€ {accreditiList.Sum(s => (decimal)s.Qta * s.PzzoUniCantiere):N2}").SetFont(bold)));
                    InsertDataIntoPdfTable(ref table, accreditiList, operai, font);
                    table.AddCell(emptyCellAccrediti);
                }

                tipologia = Enumeratori.Tipologie.RIENTRO.ToString().Replace("_", " ");
                List<MaterialiCantieri> rientroList = items.Where(w => w.Tipologia == tipologia).ToList();
                if (rientroList.Count > 0)
                {
                    table.AddCell(new Cell(1, tableDimension - 1).Add(new Paragraph(tipologia)).SetFont(bold));
                    table.AddCell(new Cell(1, 1).Add(new Paragraph($"€ {rientroList.Sum(s => (decimal)s.Qta * s.PzzoUniCantiere):N2}").SetFont(bold)));
                    InsertDataIntoPdfTable(ref table, rientroList, operai, font);
                    //table.AddCell(emptyCellRientro);
                }
            }
            document.Add(table);
            document.Close();
        }

        public void InsertDataIntoPdfTable(ref iText.Layout.Element.Table table, List<MaterialiCantieri> items, List<Operai> operai, PdfFont font)
        {
            foreach (MaterialiCantieri item in items)
            {
                //table.StartNewRow();
                table.AddCell(item.CodArt).SetFont(font).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddCell(item.Tipologia == "OPERAIO" ? operai.Where(w => w.IdOperaio == item.IdTblOperaio).FirstOrDefault().NomeOp : item.DescriCodArt).SetFont(font).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddCell(item.Qta.ToString()).SetFont(font).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddCell($"€ {item.PzzoUniCantiere:N2}").SetFont(font).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddCell(item.Note).SetFont(font).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddCell(item.Note2).SetFont(font).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
                table.AddCell($"€ {(decimal)item.Qta * item.PzzoUniCantiere:N2}").SetFont(font).SetTextAlignment(TextAlignment.CENTER).SetVerticalAlignment(VerticalAlignment.MIDDLE);
            }
        }

        protected string GeneraNumeroProtocollo(string protocollo)
        {
            string ret = protocollo;
            if (ret.Length == 1)
            {
                ret = "000" + ret;
            }
            else if (ret.Length == 2)
            {
                ret = "00" + ret;
            }
            if (ret.Length == 3)
            {
                ret = "0" + ret;
            }
            return ret;
        }
        #endregion
    }
}
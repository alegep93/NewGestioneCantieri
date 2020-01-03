using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class Fatture : Page
    {
        public double totaleImporto = 0;
        public double totaleImportoAmministratore = 0;
        public double totaleDaRiscuotere = 0;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ResetToInitial();
                BindGrid();
            }
        }

        /* HELPERS */
        protected void BindGrid()
        {
            List<Fattura> fatture = FattureDAO.GetFatture(txtFiltroGrdAnno.Text, txtFiltroGrdDataDa.Text, txtFiltroGrdDataA.Text, txtFiltroGrdCliente.Text, txtFiltroGrdCantiere.Text, txtFiltroGrdAmministratore.Text);
            grdFatture.DataSource = fatture;
            grdFatture.DataBind();

            int anno = txtFiltroGrdAnno.Text == "" ? DateTime.Now.Year : Convert.ToInt32(txtFiltroGrdAnno.Text);

            grdTotaleIvaPerQuarter.DataSource = FattureDAO.GetTotaliIvaPerQuarter(anno).Select(s => new
            {
                Trimestre = s.quarter,
                TotaleIva = s.totaleIva.ToString("N2")
            });
            grdTotaleIvaPerQuarter.DataBind();

            grdTotaleImponibilePerQuarter.DataSource = FattureDAO.GetTotaliImponibilePerQuarter(anno).Select(s => new
            {
                Trimestre = s.quarter,
                TotaleIva = s.totaleIva.ToString("N2")
            });
            grdTotaleImponibilePerQuarter.DataBind();

            grdTotaleImportoPerQuarter.DataSource = FattureDAO.GetTotaliImportoPerQuarter(anno).Select(s => new
            {
                Trimestre = s.quarter,
                TotaleIva = s.totaleIva.ToString("N2")
            });
            grdTotaleImportoPerQuarter.DataBind();

            grdTotali.DataSource = FattureDAO.GetTotaliFatture(txtFiltroGrdCliente.Text, txtFiltroGrdAmministratore.Text, txtFiltroGrdAnno.Text).Select(s => new
            {
                Titolo = s.titolo,
                Valore = s.valore.ToString("N2")
            });
            grdTotali.DataBind();
        }

        protected void FillDdlScegliCliente(string ragSocCliente = "")
        {
            List<Clienti> clienti = ClientiDAO.GetByRagSoc(ragSocCliente);

            ddlScegliCliente.Items.Clear();
            ddlScegliCliente.Items.Add(new ListItem("", "-1"));

            foreach (Clienti cli in clienti)
            {
                ddlScegliCliente.Items.Add(new ListItem(cli.RagSocCli, cli.IdCliente.ToString()));
            }
        }

        protected void FillDdlScegliCantiere(string codiceCantiere = "", string descrizioneCantiere = "")
        {
            List<Cantieri> cantieri = CantieriDAO.GetForFatture(codiceCantiere, descrizioneCantiere);

            ddlScegliCantiere.Items.Clear();
            ddlScegliCantiere.Items.Add(new ListItem("", "-1"));

            foreach (Cantieri cant in cantieri)
            {
                ddlScegliCantiere.Items.Add(new ListItem($"{cant.CodCant} - {cant.DescriCodCAnt}", cant.IdCantieri.ToString()));
            }
        }

        //private void FillDdlScegliAmministratore()
        //{
        //    List<Amministratore> amministratori = AmministratoriDAO.GetAll();

        //    ddlScegliAmministratore.Items.Clear();
        //    ddlScegliAmministratore.Items.Add(new ListItem("", "-1"));

        //    foreach (Amministratore amministratore in amministratori)
        //    {
        //        ddlScegliAmministratore.Items.Add(new ListItem(amministratore.Nome, amministratore.IdAmministratori.ToString()));
        //    }
        //}

        private void ResetToInitial()
        {
            txtFiltroGrdAnno.Text = txtFiltroGrdCliente.Text = txtFiltroGrdCantiere.Text = txtFiltroGrdAmministratore.Text = "";
            txtNumeroFattura.Text = txtData.Text;
            txtIva.Text = "22";
            ddlScegliCliente.SelectedIndex = 0;
            ddlScegliCantiere.SelectedIndex = 0;
            btnInsFattura.Visible = true;
            btnModFattura.Visible = false;
            txtData.ReadOnly = false;
            lblShowAccontiAggiunti.Text = lblShowCantieriAggiunti.Text = "";
            SetNumeroFattura();
            FillDdlScegliCliente();
            FillDdlScegliCantiere();
            //FillDdlScegliAmministratore();
        }

        private void SetNumeroFattura()
        {
            long numeroFattura = FattureDAO.GetLastNumber(DateTime.Now.Year);
            if (numeroFattura == 0)
            {
                txtNumeroFattura.Text = "001";
            }
            else
            {
                txtNumeroFattura.Text = numeroFattura.ToString();
            }
        }

        private Fattura PopolaFatturaObj()
        {
            int idCliente = Convert.ToInt32(ddlScegliCliente.SelectedValue);
            Fattura fatt = new Fattura
            {
                IdFatture = hfIdFattura.Value != "" ? Convert.ToInt32(hfIdFattura.Value) : 0,
                IdClienti = idCliente,
                IdAmministratori = Convert.ToInt64(ClientiDAO.GetSingleCliente(idCliente).IdAmministratore),
                Numero = txtNumeroFattura.Text != "" ? Convert.ToInt32(txtNumeroFattura.Text) : 0,
                Data = Convert.ToDateTime(txtData.Text),
                Riscosso = chkRiscosso.Checked,
                Imponibile = txtImponibile.Text != "" ? Convert.ToDouble(txtImponibile.Text) : 0,
                Iva = txtIva.Text != "" ? Convert.ToInt32(txtIva.Text) : 0,
                RitenutaAcconto = txtRitenutaAcconto.Text != "" ? Convert.ToInt32(txtRitenutaAcconto.Text) : 0,
                ReverseCharge = chkReverseCharge.Checked,
                IsNotaDiCredito = chkNotaCredito.Checked
            };
            return fatt;
        }

        private void PopolaCampi(int idFattura, bool isModifica)
        {
            Fattura fatt = FattureDAO.GetSingle(idFattura);
            List<FatturaCantiere> fatCantieri = FattureCantieriDAO.GetByIdFattura(fatt.IdFatture);
            List<FatturaAcconto> fatAcconti = FattureAccontiDAO.GetByIdFattura(fatt.IdFatture);

            txtNumeroFattura.Text = fatt.Numero.ToString();
            ddlScegliCliente.SelectedValue = fatt.IdClienti.ToString();
            txtShowAmministratore.Text = fatt.IdAmministratori.ToString();
            fatCantieri.ForEach(f => lblShowCantieriAggiunti.Text += (lblShowCantieriAggiunti.Text == "" ? "" : ",") + CantieriDAO.GetByIdCantiere(f.IdCantieri).CodCant);
            txtData.Text = fatt.Data.ToString("yyyy-MM-dd");
            txtData.TextMode = TextBoxMode.Date;
            fatAcconti.ForEach(f => lblShowAccontiAggiunti.Text += (lblShowAccontiAggiunti.Text == "" ? "" : "-") + f.ValoreAcconto.ToString());
            txtImponibile.Text = fatt.Imponibile.ToString();
            txtRitenutaAcconto.Text = fatt.RitenutaAcconto.ToString();
            txtIva.Text = fatt.Iva.ToString();
            chkNotaCredito.Checked = fatt.IsNotaDiCredito;
            chkReverseCharge.Checked = fatt.ReverseCharge;
            chkRiscosso.Checked = fatt.Riscosso;
            txtConcatenazione.Text = $"Fat. {fatt.Numero.ToString()} del {fatt.Data.ToString("dd/MM/yyyy")}";

            // Accessibilità campi
            txtNumeroFattura.ReadOnly = txtData.ReadOnly = txtValoreAcconto.ReadOnly = !isModifica;
            txtImponibile.ReadOnly = txtRitenutaAcconto.ReadOnly = txtIva.ReadOnly = !isModifica;
            txtFiltroCliente.ReadOnly = txtFiltroCodCantiere.ReadOnly = txtFiltroDescrizioneCantiere.ReadOnly = !isModifica;
            chkNotaCredito.Enabled = chkReverseCharge.Enabled = chkRiscosso.Enabled = isModifica;
            ddlScegliCantiere.Enabled = ddlScegliCliente.Enabled = isModifica;

            // Visibilità pannelli
            pnlInsFatture.Visible = true;
            pnlRicercaFatture.Visible = !pnlInsFatture.Visible;
        }

        private void VisualizzaDati(int idFattura)
        {
            ResetToInitial();
            PopolaCampi(idFattura, false);
            btnInsFattura.Visible = btnModFattura.Visible = false;
        }

        private void ModificaDati(int idFattura)
        {
            ResetToInitial();
            PopolaCampi(idFattura, true);
            btnInsFattura.Visible = false;
            btnModFattura.Visible = true;
            hfIdFattura.Value = idFattura.ToString();
        }

        private void Elimina(int idFattura)
        {
            bool isDeleted = false;
            try
            {
                FattureAccontiDAO.Delete(idFattura);
                FattureCantieriDAO.Delete(idFattura);
                FattureDAO.Delete(idFattura);
                isDeleted = true;
            }
            catch (Exception)
            {
                lblMessaggio.Text = "Errore durante l'eliminazione di una fattura";
                lblMessaggio.ForeColor = Color.Red;
            }

            if (isDeleted)
            {
                lblMessaggio.ForeColor = Color.Blue;
                lblMessaggio.Text = "Fattura eliminato con successo";
            }
            else
            {
                lblMessaggio.ForeColor = Color.Red;
                lblMessaggio.Text = "Errore durante l'eliminazione del Fattura";
            }

            ResetToInitial();
            BindGrid();
        }

        protected void btnInsFattura_Click(object sender, EventArgs e)
        {
            Fattura p = new Fattura();
            try
            {
                if (ddlScegliCliente.SelectedIndex > 0 && txtImponibile.Text != "" && txtData.Text != "")
                {
                    p = PopolaFatturaObj();

                    // Inserisco la fattura
                    p.IdFatture = FattureDAO.Insert(p);

                    // Inserisco i cantieri e gli acconti
                    if (hfIdCantieriDaAggiungere.Value.Contains(";"))
                    {
                        hfIdCantieriDaAggiungere.Value.Split(';').ToList().ForEach(c => FattureCantieriDAO.Insert(p.IdFatture, Convert.ToInt32(c)));
                    }

                    if (hfValoriAccontiDaAggiungere.Value.Contains(";"))
                    {
                        hfValoriAccontiDaAggiungere.Value.Split(';').ToList().ForEach(a => FattureAccontiDAO.Insert(p.IdFatture, Convert.ToDouble(a)));
                    }

                    lblMessaggio.ForeColor = Color.Blue;
                    lblMessaggio.Text = "Fattura " + p.Numero + " inserito con successo";

                    ResetToInitial();
                    BindGrid();
                }
                else
                {
                    lblMessaggio.ForeColor = Color.Red;
                    lblMessaggio.Text = "I campi Cliente e Imponibile devono essere compilati";
                }
            }
            catch (Exception ex)
            {
                lblMessaggio.ForeColor = Color.Red;
                lblMessaggio.Text = "Errore durante l'inserimento della Fattura " + p.Numero + " ===> " + ex.Message;
            }
        }

        protected void btnModFattura_Click(object sender, EventArgs e)
        {
            try
            {
                Fattura p = PopolaFatturaObj();

                if (FattureDAO.Update(p))
                {
                    lblMessaggio.ForeColor = Color.Blue;
                    lblMessaggio.Text = "Fattura " + p.Numero + " aggiornata con successo";
                }
                else
                {
                    lblMessaggio.ForeColor = Color.Red;
                    lblMessaggio.Text = "Errore durante l'aggiornamento della Fattura " + p.Numero;
                }
            }
            catch (Exception ex)
            {
                lblMessaggio.ForeColor = Color.Red;
                lblMessaggio.Text = "Errore durante l'aggiornamento del Fattura ===> " + ex.Message;
            }

            ResetToInitial();
            BindGrid();
        }

        protected void grdFatture_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            int idFattura = Convert.ToInt32(e.CommandArgument.ToString());
            hfIdFattura.Value = idFattura.ToString();

            if (e.CommandName == "Visualizza")
                VisualizzaDati(idFattura);
            else if (e.CommandName == "Modifica")
                ModificaDati(idFattura);
            else if (e.CommandName == "Elimina")
                Elimina(idFattura);
        }

        protected void btnFiltraFatture_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnSvuotaFiltri_Click(object sender, EventArgs e)
        {
            ResetToInitial();
        }

        protected void btnInserisciFatture_Click(object sender, EventArgs e)
        {
            pnlInsFatture.Visible = true;
            pnlRicercaFatture.Visible = !pnlInsFatture.Visible;
        }

        protected void btnRicercaFatture_Click(object sender, EventArgs e)
        {
            pnlInsFatture.Visible = false;
            pnlRicercaFatture.Visible = !pnlInsFatture.Visible;
        }

        protected void grdFatture_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.Header)
            {
                e.Row.Cells[5].Text = "Imponibile";
                e.Row.Cells[8].Text = "Val. Iva";
                e.Row.Cells[9].Text = "Val. Rit. Acc.";
                e.Row.Cells[10].Text = "Imp. Fattura";
                e.Row.Cells[11].Text = "Imp. Amm.re";
                e.Row.Cells[12].Text = "Rev. Char.";
                e.Row.Cells[13].Text = "Riscosso";
                e.Row.Cells[14].Text = "Nota Credito";
            }
            else if (e.Row.RowType == DataControlRowType.DataRow)
            {
                int rowIndex = e.Row.RowIndex;
                long rowIdFattura = Convert.ToInt64((e.Row.FindControl("hfRowIdFattura") as HiddenField).Value);

                Fattura fattura = FattureDAO.GetSingle(rowIdFattura);

                double imponibile = fattura.Imponibile;
                double valoreIva = imponibile * fattura.Iva / 100;
                double valoreRitenutaAcconto = imponibile * fattura.RitenutaAcconto / 100;

                (e.Row.FindControl("lblValoreIva") as Label).Text = valoreIva.ToString("N2");
                (e.Row.FindControl("lblValoreRitenutaAcconto") as Label).Text = valoreRitenutaAcconto.ToString("N2");

                double importoFattura = imponibile + valoreIva - valoreRitenutaAcconto;
                (e.Row.FindControl("lblImportoFattura") as Label).Text = importoFattura.ToString("N2");

                // Se è stato valorizzato il campo amministratore
                if (fattura.IdAmministratori != 0)
                {
                    // Eseguo il calcolo per quell'amministratore e lo sommo del totale generale
                    double importoAmministratore = imponibile - valoreRitenutaAcconto;
                    (e.Row.FindControl("lblImportoAmministratore") as Label).Text = importoAmministratore.ToString("N2");
                    totaleImportoAmministratore += importoAmministratore;
                }

                // Scrivo i totali generali
                totaleImporto += importoFattura;
                totaleDaRiscuotere += (e.Row.FindControl("chkRiscosso") as CheckBox).Checked ? 0 : importoFattura - FattureAccontiDAO.GetTotaleAccontiFattura(rowIdFattura);
            }
        }

        protected void btnAggiungiCantiereAllaLista_Click(object sender, EventArgs e)
        {
            int idCantiere = Convert.ToInt32(ddlScegliCantiere.SelectedValue);

            if (idCantiere != -1)
            {
                lblShowCantieriAggiunti.Text += (lblShowCantieriAggiunti.Text == "" ? "" : ",") + CantieriDAO.GetSingleCantiere(idCantiere).CodCant;
                hfIdCantieriDaAggiungere.Value += (hfIdCantieriDaAggiungere.Value == "" ? "" : ";") + idCantiere;
            }
        }

        protected void btnAggiungiAccontiAllaLista_Click(object sender, EventArgs e)
        {
            string valoreAcconto = txtValoreAcconto.Text.Replace(".", ",");

            if (valoreAcconto != "")
            {
                lblShowAccontiAggiunti.Text += (lblShowAccontiAggiunti.Text == "" ? "" : "-") + valoreAcconto;
                hfValoriAccontiDaAggiungere.Value += (hfValoriAccontiDaAggiungere.Value == "" ? "" : ";") + valoreAcconto;
            }
        }

        protected void btnFiltraCantiere_Click(object sender, EventArgs e)
        {
            FillDdlScegliCantiere(txtFiltroCodCantiere.Text, txtFiltroDescrizioneCantiere.Text);
        }

        protected void btnFiltraCliente_Click(object sender, EventArgs e)
        {
            FillDdlScegliCliente(txtFiltroCliente.Text);
        }
    }
}
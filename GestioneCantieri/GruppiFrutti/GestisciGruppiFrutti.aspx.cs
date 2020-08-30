﻿using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class DistintaBase : System.Web.UI.Page
    {
        /* Liste pubbliche per la visualizzazione dinamica di record */
        public List<CompGruppoFrut> compList = new List<CompGruppoFrut>();
        public List<GruppiFrutti> gruppiList = new List<GruppiFrutti>();

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtNomeGruppo.Text = "";
                lblTitle.Text = "Inserisci Gruppo";
                nuovoFruttoPanel.Visible = false;
                pnlModifica.Visible = pnlElimina.Visible = false;
                pnlModGruppo.Visible = pnlDelCompGrup.Visible = false;
                FillDdlFrutti();
                FillDdlGruppi();
                FillDdlGruppiNonCompletati();
            }
            FiltraGruppi();
            lblClonaGruppo.Text = "";
        }

        /* EVENTI CLICK */
        protected void btnCreaGruppo_Click(object sender, EventArgs e)
        {
            if (txtNomeGruppo.Text != "")
            {
                bool insert = GruppiFruttiDAO.CreaGruppo(txtNomeGruppo.Text, txaDescr.Text);

                if (insert)
                {
                    lblInserimento.Text = "Gruppo '" + txtNomeGruppo.Text + "' creato con successo";
                    lblInserimento.ForeColor = Color.Blue;
                }
                else
                {
                    lblInserimento.Text = "Esiste già un gruppo con nome '" + txtNomeGruppo.Text + "'";
                    lblInserimento.ForeColor = Color.Red;
                }
                FillDdlGruppiNonCompletati();
                FiltraGruppi();
                txtNomeGruppo.Text = txaDescr.Text = "";
            }
            else
            {
                lblInserimento.Text = "Il campo 'Nome Gruppo' deve essere compilato";
                lblInserimento.ForeColor = Color.Red;
            }
        }
        protected void btnInsCompgruppo_Click(object sender, EventArgs e)
        {
            int idGruppo = Convert.ToInt32(ddlGruppi.SelectedItem.Value);
            int idFrutto = Convert.ToInt32(ddlFrutti.SelectedItem.Value);
            bool isAggiunto = CompGruppoFrutDAO.InserisciCompGruppo(idGruppo, idFrutto, txtQta.Text);

            if (isAggiunto)
            {
                lblFruttoAggiungo.Text = "Componente '" + ddlFrutti.SelectedItem.Text + "' aggiunto correttamente!";
                lblFruttoAggiungo.ForeColor = Color.Blue;
                btnInsCompgruppo.Visible = false;
            }
            else
            {
                lblFruttoAggiungo.Text = "Errore durante l'inserimento del componente";
                lblFruttoAggiungo.ForeColor = Color.Red;
            }

            compList = CompGruppoFrutDAO.GetCompGruppo(idGruppo);
            ddlFrutti.SelectedIndex = 0;
            txtQta.Text = "";
            ddlFrutti.Focus();
        }
        protected void btnApriInserisci_Click(object sender, EventArgs e)
        {
            pnlInserisci.Visible = true;
            pnlModifica.Visible = false;
            pnlElimina.Visible = false;
            lblTitle.Text = "Inserisci Gruppo";
            lblInserimento.Text = "";
        }
        protected void btnApriModifica_Click(object sender, EventArgs e)
        {
            pnlInserisci.Visible = pnlElimina.Visible = pnlModGruppo.Visible = false;
            pnlModifica.Visible = true;
            FillDdlGruppi();
            lblTitle.Text = "Modifica Gruppo";
            txtModNomeGruppo.Text = txtModDescrGruppo.Text = "";
            btnSaveModGruppo.Visible = false;
            lblSaveModGruppo.Text = "";
        }
        protected void btnApriElimina_Click(object sender, EventArgs e)
        {
            pnlInserisci.Visible = false;
            pnlModifica.Visible = false;
            pnlElimina.Visible = true;
            FillDdlGruppi();
            lblTitle.Text = "Elimina Gruppo";
            btnDelGruppo.Visible = false;
            txtDelDescrGruppo.Text = "";
            ddlDelCompGrup.SelectedIndex = -1;
            btnDelCompGruppo.Visible = false;
            lblIsDelGruppo.Text = lblIsDelCompGruppo.Text = "";
        }
        protected void btnSaveModGruppo_Click(object sender, EventArgs e)
        {
            int idGruppo = Convert.ToInt32(ddlModScegliGruppo.SelectedItem.Value);
            bool isSaved = GruppiFruttiDAO.UpdateGruppo(new GruppiFrutti
            {
                Id = idGruppo,
                NomeGruppo = txtModNomeGruppo.Text,
                Descrizione = txtModDescrGruppo.Text
            });
                
            if (isSaved)
            {
                lblSaveModGruppo.Text = "Nome gruppo modificato con successo in '" + txtModNomeGruppo.Text + "'";
                lblSaveModGruppo.ForeColor = Color.Blue;
            }
            else
            {
                lblSaveModGruppo.Text = "Errore durante la modifica del nome gruppo";
                lblSaveModGruppo.ForeColor = Color.Red;
            }
            FillDdlGruppi();
            FiltraGruppi();
            txtModNomeGruppo.Text = txtModDescrGruppo.Text = "";
            btnSaveModGruppo.Visible = false;
            btnRiapriGruppo.Visible = false;
        }
        protected void btnDelGruppo_Click(object sender, EventArgs e)
        {
            int idGruppo = Convert.ToInt32(ddlDelGruppo.SelectedItem.Value);
            bool isDeleted = false;
            isDeleted = OrdineFruttiDAO.DeleteGruppo(idGruppo);

            if (isDeleted)
            {
                isDeleted = CompGruppoFrutDAO.DeleteGruppo(idGruppo);
                if (isDeleted)
                {
                    isDeleted = GruppiFruttiDAO.DeleteGruppo(idGruppo);
                    if (isDeleted)
                    {
                        lblIsDelGruppo.Text = "Gruppo '" + ddlDelGruppo.SelectedItem.Text + "' eliminato con successo";
                        lblIsDelGruppo.ForeColor = Color.Blue;
                    }
                    else
                    {
                        lblIsDelGruppo.Text = "Impossibile eliminare il gruppo '" + ddlDelGruppo.SelectedItem.Text + "' dalla tabella GruppiFrutti";
                        lblIsDelGruppo.ForeColor = Color.Red;
                    }
                }
                else
                {
                    lblIsDelGruppo.Text = "Impossibile eliminare il gruppo '" + ddlDelGruppo.SelectedItem.Text + "' dalla tabella ComGruppoFrut ";
                    lblIsDelGruppo.ForeColor = Color.Red;
                }
            }
            else
            {
                lblIsDelGruppo.Text = "Impossibile eliminare il gruppo '" + ddlDelGruppo.SelectedItem.Text + "' dall'ordine in cui è referenziato";
                lblIsDelGruppo.ForeColor = Color.Red;
            }
            FillDdlGruppi();
            FiltraGruppi();
            txtDelDescrGruppo.Text = "";
        }
        protected void btnDelCompGruppo_Click(object sender, EventArgs e)
        {
            int idCompGruppo = Convert.ToInt32(ddlDelCompGrup.SelectedItem.Value);
            bool isDeleted = CompGruppoFrutDAO.Delete(idCompGruppo);
            if (isDeleted)
            {
                lblIsDelCompGruppo.Text = "Componente '" + ddlDelCompGrup.SelectedItem.Text + "' eliminato con successo dal gruppo '" + ddlDelNomeGruppo.SelectedItem.Text + "'";
                lblIsDelCompGruppo.ForeColor = Color.Blue;
            }
            else
            {
                lblIsDelCompGruppo.Text = "Impossibile eliminare il componente del gruppo '" + ddlDelCompGrup.SelectedItem.Text + "'";
                lblIsDelCompGruppo.ForeColor = Color.Red;
            }

            fillDdlCompGruppo();
            compList = CompGruppoFrutDAO.GetCompGruppo(Convert.ToInt32(ddlDelNomeGruppo.SelectedItem.Value));
            ddlDelCompGrup.SelectedIndex = 0;
            btnDelCompGruppo.Visible = false;
        }
        protected void btnCompletaGruppo_Click(object sender, EventArgs e)
        {
            bool isClosed = GruppiFruttiDAO.CompletaRiapriGruppo(ddlGruppi.SelectedItem.Value, true);
            if (isClosed)
            {
                lblFruttoAggiungo.Text = "Gruppo '" + ddlGruppi.SelectedItem.Text + "' chiuso con successo";
                lblFruttoAggiungo.ForeColor = Color.Blue;
            }
            else
            {
                lblFruttoAggiungo.Text = "Non è stato possibile chiudere il gruppo '" + ddlGruppi.SelectedItem.Text + "'";
                lblFruttoAggiungo.ForeColor = Color.Red;
            }
            FillDdlGruppiNonCompletati();
            txaShowDescrGruppo.Text = "";
        }
        protected void btnRiapriGruppo_Click(object sender, EventArgs e)
        {
            bool isOpen = GruppiFruttiDAO.CompletaRiapriGruppo(ddlModScegliGruppo.SelectedItem.Value, false);
            if (isOpen)
            {
                lblSaveModGruppo.Text = "Gruppo '" + ddlModScegliGruppo.SelectedItem.Text + "' aperto con successo";
                lblSaveModGruppo.ForeColor = Color.Blue;
                btnRiapriGruppo.Visible = false;
            }
            else
            {
                lblSaveModGruppo.Text = "Non è stato possibile aprire il gruppo '" + ddlModScegliGruppo.SelectedItem.Text + "'";
                lblSaveModGruppo.ForeColor = Color.Red;
            }
            FillDdlGruppiNonCompletati();
            txtModNomeGruppo.Text = txtModDescrGruppo.Text = "";
        }
        protected void btnClonaGruppo_Click(object sender, EventArgs e)
        {
            try
            {
                int idGruppo = Convert.ToInt32(ddlModScegliGruppo.SelectedItem.Value);
                GruppiFrutti gf = GruppiFruttiDAO.GetSingle(idGruppo);
                int idGruppoCopia = GruppiFruttiDAO.InserisciGruppo("Copia" + gf.NomeGruppo, gf.Descrizione);
                compList = CompGruppoFrutDAO.GetCompGruppo(idGruppo);

                foreach (CompGruppoFrut cgf in compList)
                {
                    CompGruppoFrutDAO.InserisciCompGruppo(idGruppoCopia, cgf.IdTblFrutto, cgf.Qta.ToString());
                }

                lblClonaGruppo.ForeColor = Color.Blue;
                lblClonaGruppo.Text = "Gruppo clonato con successo";

                gruppiList = GruppiFruttiDAO.GetGruppi("", "", "");
            }
            catch (Exception ex)
            {
                lblClonaGruppo.ForeColor = Color.Red;
                lblClonaGruppo.Text = "Errore durante la clonazione del gruppo selezionato";
            }
        }

        /* EVENTI TEXT-CHANGED */
        protected void txtFiltroGruppi1_TextChanged(object sender, EventArgs e)
        {
            FillDdlGruppiNonCompletati();
        }
        protected void txtFiltroGruppi2_TextChanged(object sender, EventArgs e)
        {
            FillDdlGruppiNonCompletati();
        }
        protected void txtFiltroGruppi3_TextChanged(object sender, EventArgs e)
        {
            FillDdlGruppiNonCompletati();
        }

        protected void txtFiltroGruppiMod1_TextChanged(object sender, EventArgs e)
        {
            FillDdlGruppi();
        }
        protected void txtFiltroGruppiMod2_TextChanged(object sender, EventArgs e)
        {
            FillDdlGruppi();
        }
        protected void txtFiltroGruppiMod3_TextChanged(object sender, EventArgs e)
        {
            FillDdlGruppi();
        }

        protected void ddlGruppi_TextChanged(object sender, EventArgs e)
        {
            if (ddlGruppi.SelectedItem.Text == "")
                nuovoFruttoPanel.Visible = false;
            else
            {
                int idGruppo = Convert.ToInt32(ddlGruppi.SelectedItem.Value);
                txaShowDescrGruppo.Text = GruppiFruttiDAO.GetSingle(idGruppo).Descrizione;
                nuovoFruttoPanel.Visible = true;
                compList = CompGruppoFrutDAO.GetCompGruppo(idGruppo);
            }
        }
        protected void ddlModMostraGruppi_TextChanged(object sender, EventArgs e)
        {
            int idGruppo = Convert.ToInt32(ddlModScegliGruppo.SelectedItem.Value);
            if (ddlModScegliGruppo.SelectedItem.Value != "")
            {
                btnSaveModGruppo.Visible = true;
                pnlModGruppo.Visible = true;
                txtModNomeGruppo.Text = ddlModScegliGruppo.SelectedItem.Text;
                txtModDescrGruppo.Text = GruppiFruttiDAO.GetSingle(idGruppo).Descrizione;
                compList = CompGruppoFrutDAO.GetCompGruppo(idGruppo);

                if (GruppiFruttiDAO.GetSingle(idGruppo).Completato)
                    btnRiapriGruppo.Visible = false;
                else
                    btnRiapriGruppo.Visible = true;
            }
        }
        protected void ddlDelNomeGruppo_TextChanged(object sender, EventArgs e)
        {
            int idGruppo = Convert.ToInt32(ddlDelNomeGruppo.SelectedItem.Value);
            if (ddlDelNomeGruppo.SelectedItem.Value != "")
            {
                pnlDelCompGrup.Visible = true;
                txtDelDescrGruppo.Text = GruppiFruttiDAO.GetSingle(idGruppo).Descrizione;
                fillDdlCompGruppo();
                compList = CompGruppoFrutDAO.GetCompGruppo(idGruppo);
                btnDelCompGruppo.Visible = false;
            }
            else
            {
                btnDelCompGruppo.Visible = true;
            }
        }
        protected void ddlDelGruppo_TextChanged(object sender, EventArgs e)
        {
            if (ddlDelGruppo.SelectedItem.Text != "")
                btnDelGruppo.Visible = true;
            else
                btnDelGruppo.Visible = false;

            compList = CompGruppoFrutDAO.GetCompGruppo(Convert.ToInt32(ddlDelNomeGruppo.SelectedItem.Value));
        }
        protected void ddlDelCompGrup_TextChanged(object sender, EventArgs e)
        {
            if (ddlDelCompGrup.SelectedItem.Text != "")
                btnDelCompGruppo.Visible = true;
            else
                btnDelCompGruppo.Visible = false;

            compList = CompGruppoFrutDAO.GetCompGruppo(Convert.ToInt32(ddlDelNomeGruppo.SelectedItem.Value));
        }

        /* HELPERS */
        protected void FillDdlFrutti()
        {
            List<Frutti> listFrutti = FruttiDAO.GetFrutti("","","");

            ddlFrutti.Items.Clear();

            //Il primo parametro ("") corrisponde al valore e il secondo alla chiave (il valore è quello che viene visualizzato nella form)
            ddlFrutti.Items.Add(new ListItem("", "-1"));

            foreach (Frutti f in listFrutti)
            {
                string descrFrutto = f.Descr001;
                ddlFrutti.Items.Add(new ListItem(descrFrutto, f.Id1.ToString())); //new ListItem(valore, chiave);
            }
        }
        protected void FillDdlGruppi()
        {
            List<GruppiFrutti> listGruppiFrutti = GruppiFruttiDAO.GetGruppi(txtFiltroMod1.Text, txtFiltroMod2.Text, txtFiltroMod3.Text);

            ddlModScegliGruppo.Items.Clear();

            //Il primo parametro ("") corrisponde al valore e il secondo alla chiave (il valore è quello che viene visualizzato nella form)
            ddlModScegliGruppo.Items.Add(new ListItem("", "-1"));

            foreach (GruppiFrutti gf in listGruppiFrutti)
            {
                string nomeGruppo = gf.NomeGruppo;
                ddlModScegliGruppo.Items.Add(new ListItem(nomeGruppo, gf.Id.ToString())); //new ListItem(valore, chiave);
            }
        }
        protected void FillDdlGruppiNonCompletati()
        {
            string filtro1 = (pnlInserisci.Visible == true ? txtFiltroGruppi1.Text : txtFiltroDel1.Text);
            string filtro2 = (pnlInserisci.Visible == true ? txtFiltroGruppi2.Text : txtFiltroDel2.Text);
            string filtro3 = (pnlInserisci.Visible == true ? txtFiltroGruppi3.Text : txtFiltroDel3.Text);
            List<GruppiFrutti> listGruppiFrutti = GruppiFruttiDAO.GetGruppi(filtro1, filtro2, filtro3, true);

            ddlGruppi.Items.Clear();
            ddlDelGruppo.Items.Clear();
            ddlDelNomeGruppo.Items.Clear();

            //Il primo parametro ("") corrisponde al valore e il secondo alla chiave (il valore è quello che viene visualizzato nella form)
            ddlGruppi.Items.Add(new ListItem("", "-1"));
            ddlDelGruppo.Items.Add(new ListItem("", "-1"));
            ddlDelNomeGruppo.Items.Add(new ListItem("", "-1"));

            foreach (GruppiFrutti gf in listGruppiFrutti)
            {
                string nomeGruppo = gf.NomeGruppo;
                ddlGruppi.Items.Add(new ListItem(nomeGruppo, gf.Id.ToString())); //new ListItem(valore, chiave);
                ddlDelGruppo.Items.Add(new ListItem(nomeGruppo, gf.Id.ToString())); //new ListItem(valore, chiave);
                ddlDelNomeGruppo.Items.Add(new ListItem(nomeGruppo, gf.Id.ToString())); //new ListItem(valore, chiave);
            }
        }
        protected void fillDdlCompGruppo()
        {
            List<CompGruppoFrut> listFrutti = CompGruppoFrutDAO.GetCompGruppo(Convert.ToInt32(ddlDelNomeGruppo.SelectedItem.Value));

            ddlDelCompGrup.Items.Clear();

            //Il primo parametro ("") corrisponde al valore e il secondo alla chiave (il valore è quello che viene visualizzato nella form)
            ddlDelCompGrup.Items.Add(new ListItem("", "-1"));

            foreach (CompGruppoFrut f in listFrutti)
            {
                string nomeFrutto = f.NomeFrutto;
                ddlDelCompGrup.Items.Add(new ListItem(nomeFrutto, f.Id.ToString())); //new ListItem(valore, chiave);
            }
        }
        protected void FiltraGruppi()
        {
            gruppiList = GruppiFruttiDAO.GetGruppi(txtFiltroGruppi1.Text, txtFiltroGruppi2.Text, txtFiltroGruppi3.Text);
        }

        protected void ddlFrutti_TextChanged(object sender, EventArgs e)
        {
            int idGruppo = Convert.ToInt32(ddlGruppi.SelectedItem.Value);
            btnInsCompgruppo.Visible = true;
            compList = CompGruppoFrutDAO.GetCompGruppo(idGruppo);
        }
    }
}
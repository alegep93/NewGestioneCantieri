using GestioneCantieri.DAO;
using GestioneCantieri.Data;
using GestioneCantieri.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri
{
    public partial class DichiarazioneConformita : Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Bind();
            }
        }


        private void Bind()
        {
            List<Cantieri> cantieri = CantieriDAO.GetAll();

            ddlScegliCantiere.Items.Clear();
            ddlScegliCantiere.Items.Add(new ListItem("", "-1"));
            DropDownListManager.FillDdlCantieri(cantieri, ref ddlScegliCantiere);

            grdDiCo.DataSource = cantieri.Where(w => w.NumDiCo != null).OrderBy(o => o.NumDiCo).ToList();
            grdDiCo.DataBind();

            txtData.Text = "";
            txtData.TextMode = TextBoxMode.Date;
        }

        protected void btnSalva_Click(object sender, EventArgs e)
        {
            try
            {
                long numDiCo = Convert.ToInt64(txtData.Text != "" ? Convert.ToDateTime(txtData.Text).ToString("yyyyMMdd") : hfDataDichiarazioneCantiereOld.Value);
                CantieriDAO.SetDiCo(Convert.ToInt32(ddlScegliCantiere.SelectedValue), numDiCo);
                btnSalva.Text = "Salva";
                Bind();
            }
            catch (Exception ex)
            {
                (Master as layout).SetModal($"Errore durante il btnSalva_Click in DichiarazioneConformità.aspx.cs ===> {ex.Message}");
            }
        }

        protected void grdDiCo_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int idCantiere = Convert.ToInt32(e.CommandArgument);
                switch (e.CommandName)
                {
                    case "Modifica":
                        ddlScegliCantiere.SelectedValue = idCantiere.ToString();
                        txtData.Enabled = false;
                        btnSalva.Text = "Modifica";
                        hfDataDichiarazioneCantiereOld.Value = CantieriDAO.GetSingle(idCantiere).NumDiCo.Value.ToString();
                        CantieriDAO.DeleteDiCo(idCantiere);
                        break;
                    case "Elimina":
                        // In realtà è una Delete LOGICA, che va a impostare il campo NumDiCo a NULL
                        CantieriDAO.DeleteDiCo(idCantiere);
                        Bind();
                        break;
                }
            }
            catch (Exception ex)
            {
                (Master as layout).SetModal($"Errore durante il grdDiCo_RowCommand in DichiarazioneConformità.aspx.cs ===> {ex.Message}");
            }
        }
    }
}
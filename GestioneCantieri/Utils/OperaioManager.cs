using GestioneCantieri.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace GestioneCantieri.Utils
{
    public class OperaioManager
    {
        public static void FillDdlOperaio(List<Operai> items, ref DropDownList ddl)
        {
            foreach (Operai item in items)
            {
                ddl.Items.Add(new ListItem($"{item.NomeOp} - {item.DescrOp}", item.IdOperaio.ToString()));

                if (item.NomeOp.ToUpper() == "MAURIZIO" || item.NomeOp.ToUpper() == "MAU")
                {
                    ddl.SelectedValue = item.IdOperaio.ToString();
                }
            };
        }
    }
}
using GestioneCantieri.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace GestioneCantieri.Utils
{
    public class SpesaManager
    {
        public static void FillDdlSpese(List<Spese> items, ref DropDownList ddl)
        {
            foreach(Spese item in items)
            {
                ddl.Items.Add(new ListItem(item.Descrizione, item.IdSpesa.ToString()));
            };
        }
    }
}
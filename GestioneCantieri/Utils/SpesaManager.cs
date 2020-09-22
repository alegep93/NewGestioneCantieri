using GestioneCantieri.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace GestioneCantieri.Utils
{
    public class SpesaManager
    {
        public static DropDownList FillDdlSpese(List<Spese> items)
        {
            DropDownList ret = new DropDownList();
            items.ForEach(f =>
            {
                ret.Items.Add(new ListItem(f.Descrizione, f.IdSpesa.ToString()));
            });
            return ret;
        }
    }
}
using GestioneCantieri.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace GestioneCantieri.Utils
{
    public class CantiereManager
    {
        public static DropDownList FillDdlCantieri(List<Cantieri> items)
        {
            DropDownList ret = new DropDownList();
            items.ForEach(f =>
            {
                ret.Items.Add(new ListItem($"{f.CodCant} - {f.DescriCodCant}", f.IdCantieri.ToString()));
            });
            return ret;
        }
    }
}
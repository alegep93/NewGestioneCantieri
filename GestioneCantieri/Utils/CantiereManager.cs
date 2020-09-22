using GestioneCantieri.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace GestioneCantieri.Utils
{
    public class CantiereManager
    {
        public static void FillDdlCantieri(List<Cantieri> items, ref DropDownList ddl)
        {
            foreach(Cantieri item in items)
            {
                ddl.Items.Add(new ListItem($"{item.CodCant} - {item.DescriCodCant}", item.IdCantieri.ToString()));
            };
        }
    }
}
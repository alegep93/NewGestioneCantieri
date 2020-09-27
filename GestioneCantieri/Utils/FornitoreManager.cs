using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace GestioneCantieri.Utils
{
    public class FornitoreManager
    {
        public static void FillDdlFornitore(List<Fornitori> items, ref DropDownList ddl)
        {
            foreach (Fornitori item in items)
            {
                ddl.Items.Add(new ListItem(item.RagSocForni, item.IdFornitori.ToString()));
            };
        }
    }
}
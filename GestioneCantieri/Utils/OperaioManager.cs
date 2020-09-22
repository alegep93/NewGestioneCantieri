using GestioneCantieri.Data;
using System.Collections.Generic;
using System.Web.UI.WebControls;

namespace GestioneCantieri.Utils
{
    public class OperaioManager
    {
        public static DropDownList FillDdlOperaio(List<Operai> items)
        {
            DropDownList ret = new DropDownList();
            items.ForEach(f =>
            {
                ret.Items.Add(new ListItem($"{f.NomeOp} - {f.DescrOp}", f.IdOperaio.ToString()));

                if (f.NomeOp.ToUpper() == "MAURIZIO" || f.NomeOp.ToUpper() == "MAU")
                {
                    ret.SelectedValue = f.IdOperaio.ToString();
                }
            });
            return ret;
        }
    }
}
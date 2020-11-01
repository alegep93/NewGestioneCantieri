using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace GestioneCantieri.Utils
{
    public class ExcelManager
    {
        public static void ExportGridToExcel(HttpResponse response, GridView gridView, string fileName)
        {
            StringWriter strwritter = new StringWriter();
            HtmlTextWriter htmltextwrtter = new HtmlTextWriter(strwritter);
            response.Clear();
            response.Buffer = true;
            response.ClearContent();
            response.ClearHeaders();
            response.Charset = "";
            fileName += ".xls";
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.ContentType = "application/vnd.ms-excel";
            response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            gridView.GridLines = GridLines.Both;
            gridView.HeaderStyle.Font.Bold = true;
            gridView.RenderControl(htmltextwrtter);
            response.Write(strwritter.ToString());
            response.End();
        }
    }
}
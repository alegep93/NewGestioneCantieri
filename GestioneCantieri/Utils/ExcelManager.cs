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
            string style = @"<style> .textmode {mso-number-format:General} </style>";
            response.Clear();
            response.Buffer = true;
            response.ClearContent();
            response.ClearHeaders();
            response.Charset = "";
            response.ContentEncoding = System.Text.Encoding.Unicode; /*c# uses UTF-16 internally*/
            response.BinaryWrite(System.Text.Encoding.Unicode.GetPreamble());
            fileName += ".xls";
            response.Cache.SetCacheability(HttpCacheability.NoCache);
            response.AddHeader("Content-Disposition", "attachment;filename=" + fileName);
            response.ContentType = "application/ms-excel";
            gridView.GridLines = GridLines.Both;
            gridView.HeaderStyle.Font.Bold = true;
            response.Write(style);
            gridView.RenderControl(htmltextwrtter);
            response.Write(strwritter.ToString());
            response.End();
        }
    }
}
﻿using GestioneCantieri.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI.WebControls;

namespace GestioneCantieri.Utils
{
    public class DropDownListManager
    {
        public static void FillDdlCantieri(List<Cantieri> items, ref DropDownList ddl)
        {
            foreach (Cantieri item in items)
            {
                ddl.Items.Add(new ListItem($"{item.CodCant} - {item.DescriCodCant}", item.IdCantieri.ToString()));
            };
        }

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

        public static void FillDdlFornitore(List<Fornitori> items, ref DropDownList ddl)
        {
            foreach (Fornitori item in items)
            {
                ddl.Items.Add(new ListItem(item.RagSocForni, item.IdFornitori.ToString()));
            };
        }

        public static void FillDdlMaterialiCantieri(List<MaterialiCantieri> items, ref DropDownList ddl)
        {
            foreach (MaterialiCantieri item in items)
            {
                string show = $"{item.CodArt} | {item.DescriCodArt} | {item.Qta} | {item.PzzoUniCantiere} | {item.PzzoFinCli}";
                ddl.Items.Add(new ListItem(show, item.IdMaterialiCantiere.ToString()));
            }
        }

        public static void FillDdlSpese(List<Spese> items, ref DropDownList ddl)
        {
            foreach (Spese item in items)
            {
                ddl.Items.Add(new ListItem(item.Descrizione, item.IdSpesa.ToString()));
            };
        }

        public static void FillDdlDdtMef(List<DDTMef> items, ref DropDownList ddl)
        {
            foreach (DDTMef item in items)
            {
                string show = $"{item.Data.ToString().Split(' ')[0]} - {item.N_DDT}";
                ddl.Items.Add(new ListItem(show, item.IdDDTMef.ToString()));
            }
        }

        public static void FillDdlMamg0(List<Mamg0> items, ref DropDownList ddl)
        {
            foreach (Mamg0 item in items)
            {
                string show = String.Format("{0,-18} | {1,-30} | {2,-8} | {3,-8} | {4,-3} | {5,-3} | {6,-3}", 
                    item.CodArt, item.Desc, item.PrezzoNetto, item.PrezzoListino, item.Sconto1, item.Sconto2, item.Sconto3);
                ddl.Items.Add(new ListItem(show, item.CodArt.ToString()));
            }
        }
    }
}
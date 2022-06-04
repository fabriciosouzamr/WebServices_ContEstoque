using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace APP
{
    public class Funcao
    {
        public static string FNC_QuotedStr(string sTexto)
        {
            return "'" + sTexto.Trim() + "'";
        }
    }
}
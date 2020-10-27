using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Web.Script.Services;
using System.Web.Services;

namespace ProposalPolmanAstra
{
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.Web.Script.Services.ScriptService]

    public class List : System.Web.Services.WebService
    {
        [ScriptMethod()]
        [WebMethod]
        public List<string> GetListBarang(string prefixText)
        {
            PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["ProposalConnection"].ToString());
            DataTable dt = new DataTable();
            dt = lib.CallProcedure("stp_getListBarang", new String[] { prefixText });
            List<string> result = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(dt.Rows[i][0].ToString());
            }
            return result;
        }

        [ScriptMethod()]
        [WebMethod]
        public List<string> GetListNomorPP(string prefixText)
        {
            PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["ProposalConnection"].ToString());
            DataTable dt = new DataTable();
            dt = lib.CallProcedure("stp_getListNomorPP", new String[] { prefixText });
            List<string> result = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(dt.Rows[i][0].ToString());
            }
            return result;
        }

        [ScriptMethod()]
        [WebMethod]
        public List<string> GetListNomorPO(string prefixText)
        {
            PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["ProposalConnection"].ToString());
            DataTable dt = new DataTable();
            dt = lib.CallProcedure("stp_getListNomorPO", new String[] { prefixText });
            List<string> result = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(dt.Rows[i][0].ToString());
            }
            return result;
        }

        [ScriptMethod()]
        [WebMethod]
        public List<string> GetListVendor(string prefixText)
        {
            PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["ProposalConnection"].ToString());
            DataTable dt = new DataTable();
            dt = lib.CallProcedure("stp_getListVendor2", new String[] { prefixText });
            List<string> result = new List<string>();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                result.Add(dt.Rows[i][0].ToString());
            }
            return result;
        }
    }
}

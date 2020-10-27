using System;
using System.DirectoryServices;
using System.Configuration;

namespace SSOPolmanAstra.Classes
{
    public class LDAPAuthenticationStaff
    {
        PolmanAstraLibrary.PolmanAstraLibrary lib = new PolmanAstraLibrary.PolmanAstraLibrary(ConfigurationManager.ConnectionStrings["DefaultConnection"].ToString());
        private string _path;
        string adPath = "yourLDAPhere";

        public LDAPAuthenticationStaff()
        {
            _path = adPath;
        }

        public bool IsAuthenticated(string username, string pwd)
        {
            DirectoryEntry entry = new DirectoryEntry(_path, "polman\\" + username, pwd);
            try
            {
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("cn");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return false;
                }
            }
            catch
            {
                return false;
            }
            return true;
        }

        public String GetMail(string username)
        {
            DirectoryEntry entry = new DirectoryEntry(_path, "polman\\yourusername", "yourpassword");
            try
            {
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("mail");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return "";
                }
                try
                {
                    return (String)result.Properties["mail"][0];
                }
                catch
                {
                    return "";
                }
            }
            catch
            {
                return "";
            }
        }

        public String GetDisplayName(string username)
        {
            DirectoryEntry entry = new DirectoryEntry(_path, "polman\\yourusername", "yourpassword");
            try
            {
                Object obj = entry.NativeObject;
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                search.PropertiesToLoad.Add("displayName");
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return "";
                }
                try
                {
                    return (String)result.Properties["displayName"][0];
                }
                catch
                {
                    return username;
                }
            }
            catch
            {
                return username;
            }
        }

        public string SetPassword(string username, string newpassword)
        {
            DirectoryEntry entry = new DirectoryEntry(_path, "polman\\yourusername", "yourpassword");
            try
            {
                DirectorySearcher search = new DirectorySearcher(entry);
                search.Filter = "(SAMAccountName=" + username + ")";
                SearchResult result = search.FindOne();
                if (null == result)
                {
                    return "FAILED";
                }
                try
                {
                    entry = result.GetDirectoryEntry();
                    entry.Invoke("SetPassword", new object[] { newpassword });
                    entry.Properties["LockOutTime"].Value = 0;
                    entry.Close();
                    return "SUCCESS";
                }
                catch (Exception ex)
                {
                    return ex.Message + " " + ex.StackTrace;
                }
            }
            catch (Exception ex)
            {
                return ex.Message + " " + ex.StackTrace;
            }
        }
    }
}
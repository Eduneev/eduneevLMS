using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.DirectoryServices;

namespace UtilityClass
{
    public class LDAPAuth
    {
        //srvr = ldap server, e.g. LDAP://domain.com LDAP://actis-dc3.act.is
        //usr = user name   Port:389
        //pwd = user password
        
        public static bool IsAuthenticated(string usr, string pwd)
        {
        bool authenticated = false;
            string srvr = "LDAP://actis-dc3.act.is";
            try
            {
                DirectoryEntry entry = new DirectoryEntry(srvr, usr, pwd);
                object nativeObject = entry.NativeObject;
                authenticated = true;
            }
            catch (DirectoryServicesCOMException cex)
            {
                //not authenticated; reason why is in cex
            }
            catch (Exception ex)
            {
                //not authenticated due to some other exception [this is optional]
            }

        return authenticated;
        }
    }
}

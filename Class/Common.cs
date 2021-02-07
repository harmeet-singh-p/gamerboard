using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GameProj
{
    class Common
    {
        /* Writes an error line to the log file
         * arguments - page name (where error occured),log file name, Error text,User logged in when error created */
        public void WritErrorLog(string PageName, string strFileName, string strErrorDescription, string strUserName)
        {
            try
            {
                StreamWriter objStreamWriter;
                objStreamWriter = File.AppendText(strFileName);
                objStreamWriter.WriteLine(PageName + " || " + DateTime.Now.ToString() + " || " + strErrorDescription + " || " + strUserName);
                objStreamWriter.WriteLine("--------------------------------------------------------------------------------------------------------------");
                objStreamWriter.Close();
            }
            catch (Exception ex)
            {
                string ls_tmpstr = "";
                ls_tmpstr = ex.Message + " ";
            }
        }
    }
}

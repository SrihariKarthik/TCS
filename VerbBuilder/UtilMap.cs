using System;
using System.Collections.Generic;
using System.Text;

namespace VerbBuilder
{
    public static class UtilMap
    {
        public static void AddItemToList(string sItem, ref List<string> lstItems)
        {
            bool isItemFnd = false;
            foreach (string str in lstItems)
            {
                if (str == sItem)
                {
                    isItemFnd = true;
                    break;
                }
            }
            if (!isItemFnd)
                lstItems.Add(sItem);
        }

        public static bool IsContains(string sFull, string sSrch)
        {
            bool isContains = false;
            sFull = Lower(sFull);
            sSrch = Lower(sSrch);
            if (sFull.Contains(sSrch))
                isContains = true;
            return isContains;
        }

        public static string GetFirstWord(string sFull)
        {
            string sFword = "";
            string[] sValues = sFull.Trim().Split(' ');
            if (sValues.Length > 0)
                sFword = sValues[0];
            return sFword;
        }

        public static int GetWordCount(string sFull)
        {
            string[] sValues = sFull.Split(' ');
            return sValues.Length;
        }

        public static string GetVerb(string sFull, List<Single> lstVerbs)
        {
            string sVerb = "";
            foreach (string sToken in GetAllTokens(sFull))
            {
                foreach (Single sngle in lstVerbs)
                {
                    if (sngle.subject == sToken)
                    {
                        sVerb = sToken;
                    }
                }
            }
            return sVerb;
        }


        public static string[] GetAllTokens(string sFull)
        {
            string[] lstTokens = sFull.Trim().ToLower().Split(' ');
            return lstTokens;
        }

        public static bool GetParts(string sFull, ref string sSubj, ref string sObj)
        {
            string sTemp = "";
            return GetParts(sFull, ref sSubj, ref sObj, ref sTemp);
        }

        public static bool GetParts(string sFull, ref string sSubj, ref string sVerb, ref string sOther)
        {
            int i = 0;
            string sOth = "";
            bool isSplit = true;
            string[] sValues = null;
            try
            {
                sValues = sFull.Trim().Split(' ');
                foreach (string str in sValues)
                {
                    if (i == 0)
                        sSubj = str;
                    else if (i == 1)
                        sVerb = str;
                    else
                    {
                        if (sOth != "")
                            sOth += " ";
                        sOth += str;
                    }
                    i++;
                }
                sOther = sOth;
            }
            catch (Exception ex)
            {
                isSplit = false;
            }
            return isSplit;
        }

        public static string Lower(string str)
        {
            str = str.Trim().ToLower();
            return str;
        }

        public static string LowerTrim(string str)
        {
            str = str.Trim().ToLower().Replace(" ", "");
            return str;
        }

        public static string RemoveText(string sFull, string sRemove)
        {
            string subj = "";
            sFull = Lower(sFull);
            sRemove = Lower(sRemove);
            if (sFull.Contains(sRemove))
                subj = sFull.Replace(sRemove, "");
            return subj;
        }

        public static bool SuppressText(string sActual, string sSuppress, ref string sSubj, ref string sObj)
        {
            bool isSplit = true;
            string sSuppr = sSuppress.Replace(" ", "");
            try
            {
                
                sActual = LowerTrim(sActual).Replace(sSuppr, "|");
                string[] sValues = sActual.Split("|");
                if (sValues.Length == 2)
                {
                    sSubj = sValues[0].Trim().ToLower();
                    sObj = sValues[1].Trim().ToLower();
                }
                else
                {
                    isSplit = false;
                }
            }
            catch (Exception ex)
            {
                isSplit = false;
            }
            return isSplit;
        }
    }
}

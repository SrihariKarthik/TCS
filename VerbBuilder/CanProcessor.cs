using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace VerbBuilder
{
    public class CanProcessor
    {
        public List<Single> lstVerbs;
        public List<Map> lstSimilar;
        public List<Map> lstOpposite;
        public List<Map> lstCans;
        public List<string> lstProcessVerbs;
        public CanProcessor()
        {
            lstVerbs = new List<Single>();
            lstSimilar = new List<Map>();
            lstOpposite = new List<Map>();
            lstCans = new List<Map>();

            // Add all processing verbs
            lstProcessVerbs = new List<string>();
            lstProcessVerbs.Add(" behave similar as "); // boy behave similar as cow
            lstProcessVerbs.Add(" can ");               // boy can jump
            lstProcessVerbs.Add(" is a ");              // This is only for verb '' Jump is a verb
            lstProcessVerbs.Add(" is similar to ");     // jump is similar to walk
            lstProcessVerbs.Add(" oppose with ");       // can oppose with cannot
        }

        private void AddAllCanVerbs(List<string> lstSubjs, ref List<string> lstVerbs)
        {
            string sVerb = "";
            foreach (string sSub in lstSubjs)
            {
                // Get all From Cans
                foreach (Map map in lstCans)
                {
                    if (sSub == map.Subject)
                    {
                        sVerb = map.Subjvalue;
                        lstVerbs.Add(sVerb);
                        sVerb = "";
                    }
                }
            }
        }

        private void AddAllSimilarVerbs(ref List<string> lstVerbs)
        {
            int len = lstVerbs.Count;
            int i = 0;
            string sSub = "";
            while (i < len)
            {
                sSub = lstVerbs[i];
                // Get all From Cans
                foreach (Map map in lstSimilar)
                {
                    if (map.Type == "is")
                    {
                        if (map.Subject == sSub)
                            lstVerbs.Add(map.Subjvalue);
                        else if (map.Subjvalue == sSub)
                            lstVerbs.Add(map.Subject);
                    }
                }
                i++;
            }
        }

        public string GetProcessStatementType(string sFull)
        {
            string pType = "";
            foreach (string str in lstProcessVerbs)
            {
                if (UtilMap.IsContains(sFull, str))
                {
                    pType = UtilMap.GetFirstWord(str);
                    break;
                }
            }
            return pType;
        }
        private List<string> GetSimilarSubjects(string sSubj)
        {
            List<string> lstSubjs = new List<string>();
            lstSubjs.Add(sSubj);
            int i = 0;
            string sTempSubj;
            int len = lstSubjs.Count;
            while (i < len)
            {
                sTempSubj = lstSubjs[i];
                foreach (Map map in lstSimilar)
                {
                    if (map.Type != "is")
                    {
                        if (map.Subject == sTempSubj)
                        {
                            UtilMap.AddItemToList(map.Subjvalue, ref lstSubjs);
                        }
                        else if (map.Subjvalue == sTempSubj)
                        {
                            UtilMap.AddItemToList(map.Subject, ref lstSubjs);
                        }
                    }
                }
                i++;
            }
            return lstSubjs;
        }

        private List<string> GetSimilarSubjectsold(string sSubj)
        {
            List<string> lstSubjs = new List<string>();
            lstSubjs.Add(sSubj);
            int i = 0;
            string sTempSubj;
            while (i < lstSubjs.Count)
            {
                sTempSubj = lstSubjs[i];
                foreach (Map map in lstSimilar)
                {
                    if (map.Type != "is")
                    {
                        if (map.Subject == sTempSubj)
                        {
                            UtilMap.AddItemToList(map.Subjvalue, ref lstSubjs);
                        }
                        else if (map.Subjvalue == sTempSubj)
                        {
                            UtilMap.AddItemToList(map.Subject, ref lstSubjs);
                        }
                    }
                }
                i++;
            }
            return lstSubjs;
        }

        private List<string> GetSimilarVerbs(string sSubj)
        {
            List<string> lstVerbs = new List<string>();
            List<string> lstSubjs = GetSimilarSubjects(sSubj);

            //ADD FROM CAN List
            AddAllCanVerbs(lstSubjs, ref lstVerbs);

            // Now get similar verbs from SimilarList
            AddAllSimilarVerbs(ref lstVerbs);

            return lstVerbs;
        }

        private bool IsIemFound(string sSubj, string sVerb, List<Map> lstItems)
        {
            bool isFnd = false;
            foreach (Map map in lstItems)
            {
                if (map.Subject == sSubj && sVerb == map.Subjvalue)
                {
                    isFnd = true;
                    break;
                }
            }
            return isFnd;
        }
        
        private bool IsOpposeFound(string sSubj, string sVerb)
        {
            return IsIemFound(sSubj, sVerb, lstOpposite);
        }

        private bool IsSimilarFound(string sSubj, string sVerb)
        {
            return IsIemFound(sSubj, sVerb, lstSimilar);
        }

        public int IsValid(string sFull)
        {
            int isValid = -1;
            try
            {
                if (GetProcessStatementType(sFull) == "can")
                {
                    if (ProcessCan(sFull))
                        isValid = 1;
                }
                else
                {
                    isValid = 0;
                }
            }
            catch (Exception ex)
            {
                isValid = -1;
            }
            return isValid;
        }

        private bool ProcessCan(string sFull)
        {
            bool isCan = false;
            string sVerb = "";
            string sSubj = "";
            UtilMap.SuppressText(sFull, " can ", ref sSubj, ref sVerb);
            List<string> verbs = GetSimilarVerbs(sSubj);
            foreach (string vrb in verbs)
            {
                if (vrb == sVerb)
                {
                    isCan = true;
                }
            }
            return isCan; 
        }

        public void ReadData(string sCSVPath)
        {
            List<CSV> lstCSV = File.ReadAllLines(sCSVPath)
                                           .Skip(1)
                                           .Select(v => CSV.FromCsv(v))
                                           .ToList();

            string sSubj = "";
            string sObj = "";
            string col1val;
            string col2val;
            foreach (CSV csv in lstCSV)
            {
                sSubj = "";
                sObj = "";
                col1val = csv.Col1.Trim();
                col2val = csv.Col2.Trim();
                    
                if (UtilMap.IsContains(col2val, "is a verb"))
                {
                    lstVerbs.Add(new Single(UtilMap.RemoveText(col2val, "is a verb")));
                }
                else if (UtilMap.IsContains(col2val, "behave similar as"))
                {
                    if (UtilMap.SuppressText(col2val, "behave similar as", ref sSubj, ref sObj))
                    {
                        lstSimilar.Add(new Map(sSubj, sObj, "behave"));
                    }
                }
                else if (UtilMap.IsContains(col2val, "is similar to"))
                {
                    if (UtilMap.SuppressText(col2val, "is similar to", ref sSubj, ref sObj))
                    {
                        lstSimilar.Add(new Map(sSubj, sObj, "is"));
                    }
                }
                else if (UtilMap.IsContains(col2val, "oppose with"))
                {
                    if (UtilMap.SuppressText(col2val, "oppose with", ref sSubj, ref sObj))
                    {
                        lstOpposite.Add(new Map(sSubj, sObj, "oppose"));
                    }
                }
                else if (UtilMap.IsContains(col2val, " can "))
                {
                    if (UtilMap.SuppressText(col2val, " can ", ref sSubj, ref sObj))
                    {
                        lstCans.Add(new Map(sSubj, sObj, "can"));
                    }
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Oracle.DataAccess.Client;
using Sovos.Scripting;

namespace Module3
{
  class TCountryCode
  {    
      public string CODEVALUE;
      public string DESCRIPTION; // This expands the class definition
      public string GetCODEVALUE()
      {
        return CODEVALUE;
      }

    public string GetDESCRIPTION()
    {
      return DESCRIPTION;
    }

    static public TCountryCode TCountryCode_Construct(int a)
    {
      return new TCountryCode();
    }   
  }

  class TLookupTableCountryCode
  {
    List<object> TList = new List<object>();
    Dictionary<string, TCountryCode> FHash = new Dictionary<string, TCountryCode>();

    public void SetUp()
    {
    }

    public void TearDown()
    {
    }

    public TCountryCode Add(string AKey)
    {
      TCountryCode CountryCode = new TCountryCode();
      FHash.Add(AKey, CountryCode);
      return CountryCode;
    }

    public bool Find(string AKey, ref TCountryCode AObj)
    {
      if (FHash.ContainsKey(AKey))
      {
        AObj = FHash[AKey];
        return true;
      }
      else
      {
        return false;
      }
    }

    public int Count()
    {
      return FHash.Count;
    }

    public void Clear()
    {
      
    }

    static public TLookupTableCountryCode TLookupTableCountryCode_Construct(int a)
    {
      return new TLookupTableCountryCode();
    }
  }

  class TSQLLookupTableCountryCode : TLookupTableCountryCode
  {
    bool FAppendSuffixMode;

    public bool GetAppendSuffixMode()
    {
      return false;     
    }

    public void SetAppendSuffixMode(bool AAppendSuffix)
    {
      FAppendSuffixMode = AAppendSuffix;
    }

    public void LoadFromSQL(string ASQL)
    {
      TCountryCode Obj = null;
      string CurKey, LastKey;
      int SeqKey;

      LastKey = "";

      string ConnectionString = "User Id=ceenv119;Password=DMpBkB6BEaLOphUe5dMZ;Data Source=p11d1;";      

      using (var conn = new OracleConnection { ConnectionString = ConnectionString })
      {
        conn.Open();
        using (var cmd = conn.CreateCommand())
        {
          cmd.CommandText = ASQL;
          var dr = cmd.ExecuteReader();
          while (dr.Read())
          {
            CurKey = dr.GetString(0).ToString();
            if (! Find(CurKey, ref Obj))
            {
              Obj = Add(CurKey);
              Obj.CODEVALUE = dr.GetString(0).ToString();
              Obj.DESCRIPTION = dr.GetString(1).ToString();
            }
          }
        }
      }      
    }

    public TCountryCode Get(string Key)
    {
      TCountryCode CountryCode = null;
      if (!Find(Key, ref CountryCode))
        return null;
      else
        return CountryCode;
    }

    static public TSQLLookupTableCountryCode TSQLLookupTableCountryCode_Construct(int a)
    {
      return new TSQLLookupTableCountryCode();
    }

  }
  class TSQLLookupTableBrokerageCountryCode : TSQLLookupTableCountryCode
  {
    public void SetUp()
    {
      LoadCountryName();
    }

    public string GetCountryName(string aCountryCode)
    {
      const string VARIOUS_TEXT = "VARIOUS";
      TCountryCode lCountryCode;
      string lFixedValue;
      lFixedValue = aCountryCode.ToUpper();
      if (lFixedValue == "-" || lFixedValue == "")
      {
        return lFixedValue;
      }
      else
      {
        lCountryCode = Get(lFixedValue);
        if (lCountryCode != null)
        {
          if (lFixedValue == "US")
          {
            return VARIOUS_TEXT;
          }
          else
          {
            return lCountryCode.DESCRIPTION;
          }

        }
        else
          return VARIOUS_TEXT;
      }
    }

    public string GetCountryNameTitleCase(string aCountryCode)
    {
      TCountryCode lCountryCode;
      string lFixedValue;
      string Result = GetCountryName(aCountryCode);
      if (Result == "VARIOUS")
      {
        Result = Result.Substring(0, 1) + (Result.ToLower()).Substring(1);
      }
      return Result;
    }

    public void LoadCountryName(string APrefix = "PAX15")
    {
      //string QueryString = "Select 'TH' CODEVALUE, 'THAILAND' DESCRIPTION from Dual";
      string QueryString = "SELECT CODEVALUE, DESCRIPTION FROM "+APrefix+"CODES WHERE CODETYPE = 'CC'";
      LoadFromSQL(QueryString);
    }

    static public TSQLLookupTableBrokerageCountryCode TSQLLookupTableBrokerageCountryCode_Construct(int a)
    {
      return new TSQLLookupTableBrokerageCountryCode();
    }
  }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Module3;
using NUnit.Framework;
using Oracle.DataAccess.Client;
using Sovos.Scripting;

namespace csharp_code_evaluator_ut
{
    class CSharpCodeModule3
    {
        [Test]
        public void BasicModule3_Success()
        {
          TSQLLookupTableBrokerageCountryCode SQLLookupTableBrokerageCountryCode = new TSQLLookupTableBrokerageCountryCode();
          SQLLookupTableBrokerageCountryCode.SetUp();
          Assert.AreEqual("THAILAND",SQLLookupTableBrokerageCountryCode.GetCountryName("TH"),"The country required is THAILAND");
          Assert.AreEqual("VARIOUS", SQLLookupTableBrokerageCountryCode.GetCountryName("US"), "The country required is VARIOUS");
          Assert.AreEqual("Various", SQLLookupTableBrokerageCountryCode.GetCountryNameTitleCase("US"), "The country required is VARIOUS");
          Assert.AreEqual("-", SQLLookupTableBrokerageCountryCode.GetCountryName("-"), "The country required is -");
          Assert.AreEqual("", SQLLookupTableBrokerageCountryCode.GetCountryName(""), "The country required is BLANK");
        }
    }
}

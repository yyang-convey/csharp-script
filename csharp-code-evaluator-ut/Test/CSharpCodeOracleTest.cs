using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using Oracle.DataAccess.Client;
using Sovos.Scripting;

namespace csharp_code_evaluator_ut
{
    class CSharpCodeOracleTest
    {
        [Test]
        public void BasicExpression_Success()
        {
            string ConnectionString = "User Id=rxbridge;Password=ce1099;Data Source=p11d1;";
            string QueryString = "Select '1' Cnt from Dual";
            List<string> SnList = new List<string>();
            using (var expression = new CSharpScript("1 + 1"))
                Assert.AreEqual(2, expression.Execute());

            SnList.Clear();
            using (var conn = new OracleConnection { ConnectionString = ConnectionString })
            {
                conn.Open();
                using (var cmd = conn.CreateCommand())
                {
                    cmd.CommandText = QueryString;
                    var dr = cmd.ExecuteReader();
                    while (dr.Read())
                    {
                        SnList.Add(dr.GetString(0).ToString());
                    }
                }
            }

            Assert.AreEqual(1, SnList.Count);
        }

        [Test]
        public void BasicOracle_Success()
        {
          string CodeSnippet = @"string ConnectionString = ""User Id=rxbridge;Password=ce1099;Data Source=p11d1;"";
                                 var conn = new OracleConnection { ConnectionString = ConnectionString };
                                 return 2";
          using (var expression = new CSharpScript())
          {
            expression.AddCodeSnippet(CodeSnippet);
            Assert.AreEqual(2, expression.Execute());
          }
        }

        [Test]
        public void BasicOracleOpen_Success()
        {
          string CodeSnippet = @"string ConnectionString = ""User Id=rxbridge;Password=ce1099;Data Source=p11d1;"";
                                string QueryString = ""Select '1' Cnt from Dual"";
                                List<string> SnList = new List<string>();
                                SnList.Clear();
                                using (var conn = new OracleConnection { ConnectionString = ConnectionString })
                                {
                                    conn.Open();
                                    using (var cmd = conn.CreateCommand())
                                    {
                                        cmd.CommandText = QueryString;
                                        var dr = cmd.ExecuteReader();
                                        while (dr.Read())
                                        {
                                            SnList.Add(dr.GetString(0).ToString());
                                        }
                                    }
                                }
                                return SnList.Count";
          using (var expression = new CSharpScript())
          {
            expression.AddCodeSnippet(CodeSnippet);
            Assert.AreEqual(1, expression.Execute());
          }
        }

        [Test]
        public void BasicOracleAddObject_Success()
        {
          string ConnectionString = "User Id=rxbridge;Password=ce1099;Data Source=p11d1;";
          string CodeSnippet = @"string QueryString = ""Select '1' Cnt from Dual"";
                                List<string> SnList = new List<string>();
                                SnList.Clear();                                
                                using (var cmd = conn.CreateCommand())
                                {
                                    cmd.CommandText = QueryString;
                                    var dr = cmd.ExecuteReader();
                                    while (dr.Read())
                                    {
                                        SnList.Add(dr.GetString(0).ToString());
                                    }
                                }

                                return SnList.Count";
          using (var expression = new CSharpScript())
          {
            using (var conn = new OracleConnection {ConnectionString = ConnectionString})
            {
              conn.Open();
              expression.AddObjectInScope("conn", conn);
              expression.AddCodeSnippet(CodeSnippet);
              Assert.AreEqual(1, expression.Execute());
          }
        }
        }

        [Test]
        public void CodeSnippet_Success()
        {
          using (var expression = new CSharpScript())
          {
            Assert.AreEqual(0, expression.AddCodeSnippet("var i = 1; return 1 + i"));
            Assert.AreEqual(2, expression.Execute());
          }
        }
    }
}

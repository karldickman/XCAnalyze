using System;
using System.Data;
using System.IO;

using Mono.Data.Sqlite;
using NUnit.Framework;

namespace XCAnalyze.IO.Sql
{
    public partial class SqliteWriter
    {
        #if DEBUG
        [TestFixture]
        public new class Test : AbstractWriter.Test
        {
            protected override AbstractReader CreateExampleReader ()
            {
                return new SqliteReader (SupportFiles.GetPath (ExampleDatabase + ".db"));
            }
            
            protected override AbstractReader CreateReader ()
            {
                return new SqliteReader(TestDatabase);
            }

            protected override AbstractWriter CreateWriter()
            {
                return new SqliteWriter (TestDatabase);
            }

            protected override void SetUpPartial()
            {
                File.Delete (TestDatabase);
                Writer = new SqliteWriter (SqliteWriter.CreateConnection(TestDatabase), TestDatabase, false);
            }

            [TearDown]
            public override void TearDown ()
            {
                base.TearDown ();
                File.Delete (TestDatabase);
            }
        }
        #endif
    }
}

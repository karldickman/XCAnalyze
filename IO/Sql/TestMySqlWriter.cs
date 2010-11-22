using System;
using System.Data;

using NUnit.Framework;

namespace XCAnalyze.IO.Sql
{
    public partial class MySqlWriter
    {
        #if DEBUG
        [TestFixture]
        public new class Test : Writer.Test
        {
            protected override AbstractReader CreateExampleReader()
            {
                return new MySqlReader(ExampleDatabase, TestAccount, TestAccount);
            }

            protected override AbstractReader CreateReader()
            {
                return new MySqlReader(TestDatabase, TestAccount, TestAccount);
            }

            protected override AbstractWriter CreateWriter()
            {
                return new MySqlWriter(TestDatabase, TestAccount, TestAccount);
            }

            protected override void SetUpPartial()
            {
                Writer.Close();
                WriterConnection.Open();
                MySqlWriter writer = new MySqlWriter(WriterConnection, WriterDatabase);
                IDbCommand command = WriterConnection.CreateCommand();
                writer.Command = command;
                Writer = writer;
                command.CommandText = "DROP DATABASE " + TestDatabase;
                command.ExecuteNonQuery();
                command.CommandText = "CREATE DATABASE " + TestDatabase;
                command.ExecuteNonQuery();
                command.CommandText = "USE " + TestDatabase;
                command.ExecuteNonQuery();
            }

            [TearDown]
            public override void TearDown()
            {
                for(int i = Sql.Writer.Tables.Count - 1; i >= 0; i--) {
                    WriterCommand.CommandText = "DELETE FROM " + Sql.Writer.Tables[i];
                    WriterCommand.ExecuteNonQuery();
                }
                base.TearDown();
            }
        }
        #endif
    }
}

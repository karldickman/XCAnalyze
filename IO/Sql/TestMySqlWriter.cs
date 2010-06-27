using System;
using System.Data;

using NUnit.Framework;

namespace XCAnalyze.IO.Sql
{  
    [TestFixture]
    public class TestMySqlWriter : TestWriter
    {
        [SetUp]
        override public void SetUp ()
        {
            base.SetUp();
            Writer = CreateWriter();
            Reader = new MySqlReader (TEST_DATABASE, TEST_ACCOUNT);
        }  
        
        override protected internal AbstractReader CreateExampleReader()
        {
            return new MySqlReader(EXAMPLE_DATABASE, TEST_ACCOUNT);
        }
        
        override protected internal AbstractWriter CreateWriter()
        {
            return new MySqlWriter(TEST_DATABASE, TEST_ACCOUNT);
        }
        
        override protected internal void SetUpPartial ()
        {
            IDbCommand command;
            Writer.Dispose();
            Writer.Connection.Open();
            command = Writer.Connection.CreateCommand();
            Writer = new MySqlWriter (Writer.Connection, Writer.Database, command);
            command.CommandText = "DROP DATABASE " + TEST_DATABASE;
            command.ExecuteNonQuery();
            command.CommandText = "CREATE DATABASE " + TEST_DATABASE;
            command.ExecuteNonQuery();
            command.CommandText = "USE " + TEST_DATABASE;
            command.ExecuteNonQuery();
        }

        [TearDown]
        override public void TearDown()
        {
            for(int i = Sql.Writer.TABLES.Length - 1; i >= 0; i--)
            {
                Writer.Command.CommandText = "DELETE FROM " + Sql.Writer.TABLES[i];
                Writer.Command.ExecuteNonQuery();
            }
            base.TearDown();
        }
    }
}

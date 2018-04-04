using System.Data;
using FirebirdSql.Data.FirebirdClient;
using FluentMigrator.Expressions;
using FluentMigrator.Runner.Announcers;
using FluentMigrator.Runner.Generators.Firebird;
using FluentMigrator.Runner.Processors;
using FluentMigrator.Runner.Processors.Firebird;
using NUnit.Framework;
using NUnit.Should;

namespace FluentMigrator.Tests.Integration.Processors.Firebird
{
	[TestFixture]
	[Category("Integration")]
    [Category("Firebird")]
    public class TestDisposing
	{
	    private readonly FirebirdLibraryProber _prober = new FirebirdLibraryProber();
	    private TemporaryDatabase _temporaryDatabase;

        private FbConnection _connection;
		private FirebirdProcessor _processor;

		[SetUp]
		public void SetUp()
		{
		    _temporaryDatabase = new TemporaryDatabase(
		        IntegrationTestOptions.Firebird.ConnectionString,
		        _prober);
			_connection = new FbConnection(_temporaryDatabase.ConnectionString);
			_processor = MakeProcessor();
			_connection.Open();
			_processor.BeginTransaction();
		}

		[TearDown]
		public void TearDown()
		{
			if (!_processor.WasCommitted)
				_processor.CommitTransaction();
			_connection.Close();

            FbDatabase.DropDatabase(_temporaryDatabase.ConnectionString);
		}

		[Test]
		public void Dispose_WasCommited_ShouldNotRollback()
		{
			var createTable = new CreateTableExpression { TableName = "silly" };
			createTable.Columns.Add(new Model.ColumnDefinition { Name = "one", Type = DbType.Int32 });
			_processor.Process(createTable);

			// this will close the connection
			_processor.CommitTransaction();
			// and this will reopen it again causing Dispose->RollbackTransaction not to throw
			var tableExists = _processor.TableExists("", createTable.TableName);
			tableExists.ShouldBeTrue();

			// Now dispose (->RollbackTransaction)
			_processor.Dispose();

			_processor = MakeProcessor();

			// Check that the table still exists after dispose
			_processor.TableExists("", createTable.TableName).ShouldBeTrue();
		}

		private FirebirdProcessor MakeProcessor()
		{
			var options = FirebirdOptions.AutoCommitBehaviour();
			return new FirebirdProcessor(_connection, new FirebirdGenerator(options), new TextWriterAnnouncer(TestContext.Out), new ProcessorOptions(), new FirebirdDbFactory(), options);
		}

	}
}

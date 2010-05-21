LIBRARIES = DatabaseReader.dll IoInterfaces.dll Model.dll TableModel.dll
TESTS = TestDatabaseReader.dll

all: $(LIBRARIES) $(TESTS)
	chmod a-x *.dll

clean:
	rm -rf $(LIBRARIES) $(TESTS)

test: $(TESTS)
	nunit-console TestDatabaseReader.dll

DatabaseReader.dll: DatabaseReader.cs IoInterfaces.dll Model.dll TableModel.dll
	gmcs -t:library -pkg:dotnet -r:IoInterfaces.dll -r:Model.dll -r:TableModel.dll DatabaseReader.cs

IoInterfaces.dll: IoInterfaces.cs
	gmcs -t:library IoInterfaces.cs

Model.dll: Model.cs
	gmcs -t:library Model.cs

TableModel.dll: TableModel.cs Model.dll
	gmcs -t:library -r:Model.dll TableModel.cs

TestDatabaseReader.dll: TestDatabaseReader.cs DatabaseReader.dll
	gmcs -t:library -pkg:dotnet -pkg:nunit -pkg:mysql-connector-net -r:DatabaseReader.dll TestDatabaseReader.cs

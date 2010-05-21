LIBRARIES = DatabaseReader.dll IoInterfaces.dll Model.dll TableModel.dll

all: $(LIBRARIES)
	chmod a-x *.dll

clean:
	rm -rf $(LIBRARIES)

DatabaseReader.dll: DatabaseReader.cs IoInterfaces.dll Model.dll TableModel.dll
	gmcs -t:library -pkg:dotnet -r:IoInterfaces.dll -r:Model.dll -r:TableModel.dll DatabaseReader.cs

IoInterfaces.dll: IoInterfaces.cs
	gmcs -t:library IoInterfaces.cs

Model.dll: Model.cs
	gmcs -t:library Model.cs

TableModel.dll: TableModel.cs Model.dll
	gmcs -t:library -r:Model.dll TableModel.cs

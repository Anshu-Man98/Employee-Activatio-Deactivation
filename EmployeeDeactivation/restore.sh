/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P '1234@Abcd' -Q 'RESTORE FILELISTONLY FROM DISK = "/data/database.bacpac"' | tr -s ' ' | cut -d ' ' -f 1-2
/opt/mssql-tools/bin/sqlcmd -S localhost -U SA -P '1234@Abcd' -Q 'RESTORE DATABASE WideWorldImporters FROM DISK = "/data/database.bacpac" WITH MOVE "WWI_Primary" TO "/var/opt/mssql/data/WideWorldImporters.mdf", MOVE "WWI_UserData" TO "/var/opt/mssql/data/WideWorldImporters_userdata.ndf", MOVE "WWI_Log" TO "/var/opt/mssql/data/WideWorldImporters.ldf", MOVE "WWI_InMemory_Data_1" TO "/var/opt/mssql/data/WideWorldImporters_InMemory_Data_1"'
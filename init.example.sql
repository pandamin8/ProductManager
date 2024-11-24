USE [master];
GO

IF NOT EXISTS (SELECT * FROM sys.sql_logins WHERE name = 'admin')
BEGIN
    CREATE LOGIN [newuser] WITH PASSWORD = 'admin password', CHECK_POLICY = OFF;
    ALTER SERVER ROLE [sysadmin] ADD MEMBER [newuser];
END
GO

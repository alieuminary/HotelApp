﻿/*
Deployment script for HotelAppDB

This code was generated by a tool.
Changes to this file may cause incorrect behavior and will be lost if
the code is regenerated.
*/

GO
SET ANSI_NULLS, ANSI_PADDING, ANSI_WARNINGS, ARITHABORT, CONCAT_NULL_YIELDS_NULL, QUOTED_IDENTIFIER ON;

SET NUMERIC_ROUNDABORT OFF;


GO
:setvar DatabaseName "HotelAppDB"
:setvar DefaultFilePrefix "HotelAppDB"
:setvar DefaultDataPath "C:\Users\new\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\mssqllocaldb\"
:setvar DefaultLogPath "C:\Users\new\AppData\Local\Microsoft\Microsoft SQL Server Local DB\Instances\mssqllocaldb\"

GO
:on error exit
GO
/*
Detect SQLCMD mode and disable script execution if SQLCMD mode is not supported.
To re-enable the script after enabling SQLCMD mode, execute the following:
SET NOEXEC OFF; 
*/
:setvar __IsSqlCmdEnabled "True"
GO
IF N'$(__IsSqlCmdEnabled)' NOT LIKE N'True'
    BEGIN
        PRINT N'SQLCMD mode must be enabled to successfully execute this script.';
        SET NOEXEC ON;
    END


GO
USE [$(DatabaseName)];


GO
PRINT N'Altering Procedure [dbo].[spBookings_GetFilterBookings]...';


GO
ALTER PROCEDURE [dbo].[spBookings_GetFilterBookings]
	@startDate Date,
	@lastName nvarchar(50)
AS
begin
	set nocount on;

	select b.Id, b.StartDate, b.EndDate, g.FirstName, g.LastName, r.RoomNumber, b.CheckedIn, g.Id, r.Id
	from dbo.Guests g
	join dbo.Bookings b on g.Id = b.GuestId
	join dbo.Rooms r on b.RoomId = r.Id
	where StartDate = @startDate 
		AND LastName = @lastName;
end
GO
/*
Post-Deployment Script Template							
--------------------------------------------------------------------------------------
 This file contains SQL statements that will be appended to the build script.		
 Use SQLCMD syntax to include a file in the post-deployment script.			
 Example:      :r .\myfile.sql								
 Use SQLCMD syntax to reference a variable in the post-deployment script.		
 Example:      :setvar TableName MyTable							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/

if not exists (select 1 from dbo.RoomTypes)
begin
    insert into dbo.RoomTypes (Title, Description, Price)
    values ('King Size Bed', 'A room with a king-size bed and a window.', 100),
    ('Two Queen Size Beds', 'A room with two queen-size beds and a window.',115),
    ('Executive Suite', 'Two rooms, each with a king-size bed and a window.', 205);
end

if not exists (select 1 from dbo.Rooms)
begin
-- in the situation where the data from RoomType may get deleted,
-- we will store the values in variables below so it can be accessed anytime:

    declare @roomTypeId1 int; -- declaring a variable container to be referenced
    declare @roomTypeId2 int;
    declare @roomTypeId3 int;

    select @roomTypeId1 = Id from dbo.RoomTypes where Title = 'King Size Bed'; -- gets value
    select @roomTypeId2 = Id from dbo.RoomTypes where Title = 'Two Queen Size Beds';
    select @roomTypeId3 = Id from dbo.RoomTypes where Title = 'Executive Suite';


    insert into dbo.Rooms (RoomNumber, RoomTypeId)
    values ('101', @roomTypeId1),
    ('102', @roomTypeId1),
    ('103', @roomTypeId1),
    ('201', @roomTypeId2),
    ('202', @roomTypeId2),
    ('301', @roomTypeId3);
end
GO

GO
PRINT N'Update complete.';


GO

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
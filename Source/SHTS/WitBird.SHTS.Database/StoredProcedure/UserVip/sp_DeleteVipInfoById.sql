
CREATE PROCEDURE [dbo].[sp_DeleteVipInfoById]
@Id int

AS
BEGIN
update dbo.UserVip set [State] = 2 Where Id = @Id
END
GO

﻿CREATE PROCEDURE [dbo].[sp_UserLogin]
@UserName nvarchar(50),
@EncryptedPassword varchar(MAX)
AS
BEGIN
	SELECT * 
	from [User] where (UserName=@UserName OR Email=@UserName OR Cellphone=@UserName)
	 and EncryptedPassword=@EncryptedPassword AND [State]!=1;
END
GO
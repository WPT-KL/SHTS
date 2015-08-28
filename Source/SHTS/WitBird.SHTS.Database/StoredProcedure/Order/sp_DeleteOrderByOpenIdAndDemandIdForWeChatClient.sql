﻿CREATE PROCEDURE [dbo].[sp_DeleteOrderByOpenIdAndDemandIdForWeChatClient]
	@UserName nvarchar(100),
	@ResourceId int
AS
	DELETE FROM TradeOrder WHERE UserName = @UserName and ResourceId = @ResourceId AND OrderType = 2 AND [State] <> 1

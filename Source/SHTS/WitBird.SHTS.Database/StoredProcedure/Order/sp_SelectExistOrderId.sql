﻿CREATE PROCEDURE [dbo].[sp_SelectExistOrderId]
	@OrderId NVARCHAR(50)
AS
BEGIN
SELECT TOP(1) OrderId FROM dbo.TradeOrder WHERE OrderId = @OrderId
END
GO
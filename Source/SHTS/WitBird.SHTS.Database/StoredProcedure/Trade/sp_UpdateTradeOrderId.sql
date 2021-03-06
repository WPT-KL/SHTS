﻿CREATE PROCEDURE [dbo].[sp_UpdateTradeOrderId]
	@TradeId int,
	@IsBuyerPaid bit,
	@OrderId nvarchar(50),
	@LastUpdatedTime datetime
AS
BEGIN
	UPDATE dbo.Trade SET IsBuyerPaid=@IsBuyerPaid, TradeOrderId = @OrderId, LastUpdatedTime=@LastUpdatedTime WHERE TradeId = @TradeId
END
GO
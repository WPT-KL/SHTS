
CREATE PROCEDURE [dbo].[sp_SelectVipUserReviewList]
@startRowIndex int,
@PageSize int

AS
BEGIN
select UT.* from (
		  select u.UserId,ROW_NUMBER() over(Order by U.LastUpdatedTime) rownum
			 from UserVip u
			 where U.[State]=0 AND U.IdentifyImg <> ''
		)t,UserVip UT
		where t.rownum>=(@startRowIndex-1)*@PageSize+1 
		and t.rownum<=@startRowIndex*@PageSize and UT.UserId=t.UserId;

		SELECT Count(UserId) as 'totalCount' from UserVip
			 where [State]=0 AND IdentifyImg <> ''
END
GO
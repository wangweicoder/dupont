using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DuPont.Repository
{
    public class SqlStrings
    {
        #region "产业商或农机手供求列表"
        public const string GetBusinessOrOperatorRequirementList = @"--计算符合条件的总记录条数
set @totalcount=(select count(1) from(select AreaId,Province,City,Region,Township,Village,Distance=dbo.GetDistance(@source_lat,@source_lng,A.Lat,A.Lng) 
	from (
			select case 
				when Province IS NULL then null 
				when Province ='' then null
				when Province='0' then null
				else
					case
						when City is null then Province
						when City='' then Province
						when City='0' then Province
						else
						case
							when Region is null then City
							when Region='' then City
							when Region='0' then City
							else
							case
								when Township is null then Region
								when Township='' then Region
								when Township='0' then Region
								else
								case
									when Village is null then Township
									when Village ='' then Township
									when Village='0' then Township
									else
									Village
								end
							end
						end
					end
			end as AreaId,
            Province,
            City,
            Region,
            Township,
            Village
			from [dbo].[T_FARMER_PUBLISHED_DEMAND] as D
			where IsDeleted=0 and IsOpen=1 and PublishStateId in(100501,100502,100503,100507,100508) and DemandTypeId=@demand_typeid) as filtered_valid_data
			inner join [dbo].T_AREA as A on filtered_valid_data.AreaId=A.AID
		)as result
		where {AreaFilter} and result.Distance<=@distance_limit)

declare @pageRecordEnd int,@pageRecordStart int
set @pageRecordEnd=@pageindex*@pagesize
set @pageRecordStart=@pageRecordEnd-@pagesize+1

--获取分页数据
select * from(select RowNum=ROW_NUMBER() over(order by {OrderField}),DemandId,Distance,Lat,Lng,Province,City,Region,Township,Village,CreateUserId,AcresId,CreateTime,NumberSort from (
	select DemandId,AcresId,filtered_valid_data.CreateTime,AreaId,Province,City,Region,Township,Village,CreateUserId,Distance=dbo.GetDistance(@source_lat,@source_lng,A.Lat,A.Lng),A.Lat,A.Lng,NumberSort=cast(replace(replace(replace(replace(replace(replace(replace(replace(replace(dic.DisplayName,'500-',''),'200-',''),'100-',''),'50-',''),'30-',''),'10-',''),'0-',''),'亩以上','0'),'亩','') as int)
	from (
			select DemandId=Id,
				   case 
				when Province IS NULL then null 
				when Province ='' then null
				when Province='0' then null
				else
					case
						when City is null then Province
						when City='' then Province
						when City='0' then Province
						else
						case
							when Region is null then City
							when Region='' then City
							when Region='0' then City
							else
							case
								when Township is null then Region
								when Township='' then Region
								when Township='0' then Region
								else
								case
									when Village is null then Township
									when Village ='' then Township
									when Village='0' then Township
									else
									Village
								end
							end
						end
					end
			end as AreaId,
			AcresId,
			D.CreateTime,
            Province,
            City,
            Region,
            Township,
            Village,
            CreateUserId
			from [dbo].[T_FARMER_PUBLISHED_DEMAND] as D
			where IsDeleted=0 and IsOpen=1 and PublishStateId in(100501,100502,100503,100507,100508) and DemandTypeId=@demand_typeid) as filtered_valid_data
			inner join [dbo].T_AREA as A on filtered_valid_data.AreaId=A.AID
            inner join T_SYS_DICTIONARY as dic on filtered_valid_data.AcresId=dic.Code
		) as result
	where {AreaFilter} and Distance<=@distance_limit)as final_result
    where RowNum between @pageRecordStart and (@pageindex*@pagesize)
	order by {OrderField}"; 
        #endregion

        public const string GetUserRoleLevel = @"  select UserId=U.Id,U.UserName,[Level]=ISNULL(R.Star,0) from T_USER as U
  inner join [dbo].T_USER_ROLE_RELATION as R on U.Id=R.UserID and R.RoleID={RoleId} and  R.MemberType=1 and U.Id in({UserIdList})";

        #region "获取分配给农机手的供求列表"
        /// <summary>
        /// 获取产业商
        /// </summary>
        public const string GetAssignToOperatorRequirementList = @"--计算符合条件的总记录条数
set @totalcount=(select count(1) from(select AreaId,Province,City,Region,Township,Village,Distance=dbo.GetDistance(@source_lat,@source_lng,A.Lat,A.Lng) 
	from (
			select case 
				when Province IS NULL then null 
				when Province ='' then null
				when Province='0' then null
				else
					case
						when City is null then Province
						when City='' then Province
						when City='0' then Province
						else
						case
							when Region is null then City
							when Region='' then City
							when Region='0' then City
							else
							case
								when Township is null then Region
								when Township='' then Region
								when Township='0' then Region
								else
								case
									when Village is null then Township
									when Village ='' then Township
									when Village='0' then Township
									else
									Village
								end
							end
						end
					end
			end as AreaId,
            Province,
            City,
            Region,
            Township,
            Village
			from [dbo].[T_FARMER_PUBLISHED_DEMAND] as D
			where IsDeleted=0 and IsOpen=0 and PublishStateId in(100501,100502,100503) and DemandTypeId=@demand_typeid and
			   Id in(select FarmerDemandId from [dbo].T_USER_FARMERDEMANDS where UserId=@response_userid)
			) as filtered_valid_data
			inner join [dbo].T_AREA as A on filtered_valid_data.AreaId=A.AID
		)as result
		where {AreaFilter} and result.Distance<=@distance_limit)

declare @pageRecordEnd int,@pageRecordStart int
set @pageRecordEnd=@pageindex*@pagesize
set @pageRecordStart=@pageRecordEnd-@pagesize+1

--获取分页数据
select * from (select RowNum=ROW_NUMBER() over(order by {OrderField}),DemandId,Distance,Lat,Lng,Province,City,Region,Township,Village,CreateUserId,AcresId,CreateTime,NumberSort from (
	select DemandId,AcresId,filtered_valid_data.CreateTime,AreaId,Province,City,Region,Township,Village,CreateUserId,Distance=dbo.GetDistance(@source_lat,@source_lng,A.Lat,A.Lng),A.Lat,A.Lng,NumberSort=cast(replace(replace(replace(replace(replace(replace(replace(replace(replace(dic.DisplayName,'500-',''),'200-',''),'100-',''),'50-',''),'30-',''),'10-',''),'0-',''),'亩以上','0'),'亩','') as int)
	from (
			select DemandId=Id,
				   case 
				when Province IS NULL then null 
				when Province ='' then null
				when Province='0' then null
				else
					case
						when City is null then Province
						when City='' then Province
						when City='0' then Province
						else
						case
							when Region is null then City
							when Region='' then City
							when Region='0' then City
							else
							case
								when Township is null then Region
								when Township='' then Region
								when Township='0' then Region
								else
								case
									when Village is null then Township
									when Village ='' then Township
									when Village='0' then Township
									else
									Village
								end
							end
						end
					end
			end as AreaId,
			AcresId,
			D.CreateTime,
            Province,
            City,
            Region,
            Township,
            Village,
            CreateUserId
			from [dbo].[T_FARMER_PUBLISHED_DEMAND] as D
			where IsDeleted=0 and IsOpen=0 and PublishStateId in(100501,100502,100503) and DemandTypeId=@demand_typeid  and
			   Id in(select FarmerDemandId from [dbo].T_USER_FARMERDEMANDS where UserId=@response_userid)) as filtered_valid_data
			inner join [dbo].T_AREA as A on filtered_valid_data.AreaId=A.AID
            inner join T_SYS_DICTIONARY as dic on filtered_valid_data.AcresId=dic.Code
		) as result
	where {AreaFilter} and Distance<=@distance_limit) as final_result
    where RowNum between @pageRecordStart and (@pageindex*@pagesize) 
	order by {OrderField}"; 
        #endregion

        #region 产业商发布给大农户的需求列表(已登录)
        /// <summary>
        /// 获取大农户的供求列表查询语句
        /// </summary>
        public const string Get_Farmer_RequirementList = @"--计算符合条件的总记录条数
set @totalcount=(select count(1) from(select AreaId,Province,City,Region,Township,Village,Distance=dbo.GetDistance(@source_lat,@source_lng,A.Lat,A.Lng) 
	from (
			select case 
				when Province IS NULL then null 
				when Province ='' then null
				when Province='0' then null
				else
					case
						when City is null then Province
						when City='' then Province
						when City='0' then Province
						else
						case
							when Region is null then City
							when Region='' then City
							when Region='0' then City
							else
							case
								when Township is null then Region
								when Township='' then Region
								when Township='0' then Region
								else
								case
									when Village is null then Township
									when Village ='' then Township
									when Village='0' then Township
									else
									Village
								end
							end
						end
					end
			end as AreaId,
            Province,
            City,
            Region,
            Township,
            Village
			from [dbo].[T_BUSINESS_PUBLISHED_DEMAND] as D
			where IsDeleted=0 and PublishStateId in({ValidPublishStateIds}) and DemandTypeId=@demand_typeid) as filtered_valid_data
			inner join [dbo].T_AREA as A on filtered_valid_data.AreaId=A.AID
		)as result
		where {AreaFilter} and result.Distance<=@distance_limit)

declare @pageRecordEnd int,@pageRecordStart int
set @pageRecordEnd=@pageindex*@pagesize
set @pageRecordStart=@pageRecordEnd-@pagesize+1

--获取分页数据
select * from (select RowNum=ROW_NUMBER() over(order by {OrderField}),DemandId,Distance,Lat,Lng,Province,City,Region,Township,Village,CreateUserId,FirstWeight,CreateTime,NumberSort from (
	select DemandId,FirstWeight,filtered_valid_data.CreateTime,AreaId,Province,City,Region,Township,Village,CreateUserId,Distance=dbo.GetDistance(@source_lat,@source_lng,A.Lat,A.Lng),A.Lat,A.Lng,NumberSort=cast(iif(DemandTypeId='100201',replace(replace(replace(dic.DisplayName,'吨以上',''),'吨',''),'0-',''),
				replace(replace(replace(replace(replace(replace(replace(replace(replace(dic.DisplayName,'500-',''),'200-',''),'100-',''),'50-',''),'30-',''),'10-',''),'0-',''),'亩以上','0'),'亩','')) as int)
	from (
			select DemandId=Id,
				   case 
				when Province IS NULL then null 
				when Province ='' then null
				when Province='0' then null
				else
					case
						when City is null then Province
						when City='' then Province
						when City='0' then Province
						else
						case
							when Region is null then City
							when Region='' then City
							when Region='0' then City
							else
							case
								when Township is null then Region
								when Township='' then Region
								when Township='0' then Region
								else
								case
									when Village is null then Township
									when Village ='' then Township
									when Village='0' then Township
									else
									Village
								end
							end
						end
					end
			end as AreaId,
			FirstWeight,
			DemandTypeId,
            AcquisitionWeightRangeTypeId,
			D.CreateTime,
            Province,
            City,
            Region,
            Township,
            Village,
            CreateUserId
			from [dbo].[T_BUSINESS_PUBLISHED_DEMAND] as D
			where IsDeleted=0 and PublishStateId in({ValidPublishStateIds}) and DemandTypeId=@demand_typeid) as filtered_valid_data
			inner join [dbo].T_AREA as A on filtered_valid_data.AreaId=A.AID
            inner join T_SYS_DICTIONARY as dic on filtered_valid_data.FirstWeight=dic.Code
		) as result
	where {AreaFilter} and Distance<=@distance_limit) as final_result
    where RowNum between @pageRecordStart and (@pageindex*@pagesize)
	order by {OrderField}";
#endregion

        #region 产业商发布给大农户的需求列表(未登录)
        /// <summary>
        /// 获取产业商发给大农户的收粮、收青贮列表查询语句
        /// </summary>
        public const string Get_Farmer_RequirementListByTime = @"--计算符合条件的总记录条数
set @totalcount=(select count(1) from(select AreaId,Province,City,Region,Township,Village
	from (
			select case 
				when Province IS NULL then null 
				when Province ='' then null
				when Province='0' then null
				else
					case
						when City is null then Province
						when City='' then Province
						when City='0' then Province
						else
						case
							when Region is null then City
							when Region='' then City
							when Region='0' then City
							else
							case
								when Township is null then Region
								when Township='' then Region
								when Township='0' then Region
								else
								case
									when Village is null then Township
									when Village ='' then Township
									when Village='0' then Township
									else
									Village
								end
							end
						end
					end
			end as AreaId,
            Province,
            City,
            Region,
            Township,
            Village
			from [dbo].[T_BUSINESS_PUBLISHED_DEMAND] as D
			where IsDeleted=0 and PublishStateId in({ValidPublishStateIds}) and DemandTypeId=@demand_typeid) as filtered_valid_data
			inner join [dbo].T_AREA as A on filtered_valid_data.AreaId=A.AID
		)as result
		where {AreaFilter})

declare @pageRecordEnd int,@pageRecordStart int
set @pageRecordEnd=@pageindex*@pagesize
set @pageRecordStart=@pageRecordEnd-@pagesize+1

--获取分页数据
select * from (select RowNum=ROW_NUMBER() over(order by {OrderField}),DemandId,Lat,Lng,Province,City,Region,Township,Village,CreateUserId,FirstWeight,CreateTime,NumberSort from (
	select DemandId,FirstWeight,filtered_valid_data.CreateTime,AreaId,Province,City,Region,Township,Village,CreateUserId,A.Lat,A.Lng,NumberSort=cast(iif(DemandTypeId='100201',replace(replace(replace(dic.DisplayName,'吨以上',''),'吨',''),'0-',''),
				replace(replace(replace(replace(replace(replace(replace(replace(replace(dic.DisplayName,'500-',''),'200-',''),'100-',''),'50-',''),'30-',''),'10-',''),'0-',''),'亩以上','0'),'亩','')) as int)
	from (
			select DemandId=Id,
				   case 
				when Province IS NULL then null 
				when Province ='' then null
				when Province='0' then null
				else
					case
						when City is null then Province
						when City='' then Province
						when City='0' then Province
						else
						case
							when Region is null then City
							when Region='' then City
							when Region='0' then City
							else
							case
								when Township is null then Region
								when Township='' then Region
								when Township='0' then Region
								else
								case
									when Village is null then Township
									when Village ='' then Township
									when Village='0' then Township
									else
									Village
								end
							end
						end
					end
			end as AreaId,
			FirstWeight,
			DemandTypeId,
            AcquisitionWeightRangeTypeId,
			D.CreateTime,
            Province,
            City,
            Region,
            Township,
            Village,
            CreateUserId
			from [dbo].[T_BUSINESS_PUBLISHED_DEMAND] as D
			where IsDeleted=0 and PublishStateId in({ValidPublishStateIds}) and DemandTypeId=@demand_typeid) as filtered_valid_data
			inner join [dbo].T_AREA as A on filtered_valid_data.AreaId=A.AID
            inner join T_SYS_DICTIONARY as dic on filtered_valid_data.FirstWeight=dic.Code
		) as result
	where {AreaFilter}) as final_result
    where RowNum between @pageRecordStart and (@pageindex*@pagesize)
	order by {OrderField}";
        #endregion
       
        #region "产业商或农机手供求列表"（未登录）
        public const string GetRequirementListForOperatorAndBusiness = @"--计算符合条件的总记录条数
set @totalcount=(select count(1) from(select AreaId,Province,City,Region,Township,Village 
	from (
			select case 
				when Province IS NULL then null 
				when Province ='' then null
				when Province='0' then null
				else
					case
						when City is null then Province
						when City='' then Province
						when City='0' then Province
						else
						case
							when Region is null then City
							when Region='' then City
							when Region='0' then City
							else
							case
								when Township is null then Region
								when Township='' then Region
								when Township='0' then Region
								else
								case
									when Village is null then Township
									when Village ='' then Township
									when Village='0' then Township
									else
									Village
								end
							end
						end
					end
			end as AreaId,
            Province,
            City,
            Region,
            Township,
            Village
			from [dbo].[T_FARMER_PUBLISHED_DEMAND] as D
			where IsDeleted=0 and IsOpen=1 and PublishStateId in(100501,100502,100503,100507,100508) and DemandTypeId=@demand_typeid) as filtered_valid_data			
		)as result
		where {AreaFilter})

declare @pageRecordEnd int,@pageRecordStart int
set @pageRecordEnd=@pageindex*@pagesize
set @pageRecordStart=@pageRecordEnd-@pagesize+1

--获取分页数据
select * from(select RowNum=ROW_NUMBER() over(order by {OrderField}),DemandId,Lat,Lng,Province,City,Region,Township,Village,CreateUserId,AcresId,CreateTime,NumberSort from (
	select DemandId,AcresId,filtered_valid_data.CreateTime,AreaId,Province,City,Region,Township,Village,CreateUserId,A.Lat,A.Lng,NumberSort=cast(replace(replace(replace(replace(replace(replace(replace(replace(replace(dic.DisplayName,'500-',''),'200-',''),'100-',''),'50-',''),'30-',''),'10-',''),'0-',''),'亩以上','0'),'亩','') as int)
	from (
			select DemandId=Id,
				   case 
				when Province IS NULL then null 
				when Province ='' then null
				when Province='0' then null
				else
					case
						when City is null then Province
						when City='' then Province
						when City='0' then Province
						else
						case
							when Region is null then City
							when Region='' then City
							when Region='0' then City
							else
							case
								when Township is null then Region
								when Township='' then Region
								when Township='0' then Region
								else
								case
									when Village is null then Township
									when Village ='' then Township
									when Village='0' then Township
									else
									Village
								end
							end
						end
					end
			end as AreaId,
			AcresId,
			D.CreateTime,
            Province,
            City,
            Region,
            Township,
            Village,
            CreateUserId
			from [dbo].[T_FARMER_PUBLISHED_DEMAND] as D
			where IsDeleted=0 and IsOpen=1 and PublishStateId in(100501,100502,100503,100507,100508) and DemandTypeId=@demand_typeid) as filtered_valid_data
			inner join [dbo].T_AREA as A on filtered_valid_data.AreaId=A.AID
            inner join T_SYS_DICTIONARY as dic on filtered_valid_data.AcresId=dic.Code
		) as result
	where {AreaFilter} )as final_result
    where RowNum between @pageRecordStart and (@pageindex*@pagesize)
	order by {OrderField}";
        #endregion
    }
}

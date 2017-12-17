// ***********************************************************************
// Assembly         : DuPont
// Author           : 毛文君
// Created          : 08-14-2015
//
// Last Modified By : 毛文君
// Last Modified On : 08-14-2015
// ***********************************************************************
// <copyright file="DemandController.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DuPont.Extensions;
using DuPont.Interface;
using DuPont.Models;
using Microsoft.Ajax.Utilities;
using DuPont.Models.Dtos.Background.Demand;
using DuPont.Models.Models;
using DuPont.Global.Filters.ActionFilters;
using DuPont.Entity.Enum;
using NPOI.HSSF.UserModel;
using System.IO;
using NPOI.SS.UserModel;
using NPOI.SS.Util;


namespace DuPont.Controllers
{
    public class DemandController : BaseController
    {
        private IAdminUser _adminUserRepository;
        private IBusiness _business;
        private IArea _areaService;
        private IUser _userService;
        private ISys_Dictionary _sysDictionary;
        private IFarmerRequirement _farmerRequirement;
        public DemandController(IPermissionProvider permissionManage, IAdminUser adminUserRepository, IBusiness business, IArea area, IUser user, ISys_Dictionary sysDictionary, IFarmerRequirement farmerRequirement)
            : base(permissionManage, adminUserRepository)
        {
            this._adminUserRepository = adminUserRepository;
            _business = business;
            _areaService = area;
            _userService = user;
            _sysDictionary = sysDictionary;
            _farmerRequirement = farmerRequirement;
        }

        #region "产业商需求列表"
        /// <summary>
        /// 产业商需求列表
        /// </summary>
        /// <param name="seachModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BusinessList(BusinessSeachModel seachModel)
        {
            //CheckPermission();
            using (var result = new ResponseResult<List<BusinessListModel>>())
            {
                int totalCount = 0;
                result.Entity = _business.GetAll(seachModel, out totalCount).Select(model => new BusinessListModel
                {
                    Id = model.Id,
                    City = string.IsNullOrEmpty(model.City) ? "" : _areaService.GetAreaNamesBy(model.City),
                    CreateTime = model.CreateTime,
                    CreateUser = _userService.GetByKey(model.CreateUserId) == null ? "" : _userService.GetByKey(model.CreateUserId).UserName,
                    DemandType = _sysDictionary.GetByKey(model.DemandTypeId) == null ? "" : _sysDictionary.GetByKey(model.DemandTypeId).DisplayName,
                    Province = string.IsNullOrEmpty(model.Province) ? "" : _areaService.GetAreaNamesBy(model.Province),
                    PublishState = _sysDictionary.GetByKey(model.PublishStateId) == null ? "" : _sysDictionary.GetByKey(model.PublishStateId).DisplayName,
                    Region = string.IsNullOrEmpty(model.Region) ? "" : _areaService.GetAreaNamesBy(model.Region),
                    IsDeleted = model.IsDeleted
                }).ToList();
                result.PageIndex = seachModel.pageIndex;
                result.PageSize = seachModel.pageSize;
                result.TotalNums = totalCount;
                result.IsSuccess = true;
                return Json(result);
            }
        }
        #endregion

        #region "产业商需求详情"
        /// <summary>
        /// 产业商详细
        /// </summary>
        /// <param name="DemandId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BusinessDetail(long DemandId)
        {
            var model = _business.GetByKey(DemandId);
            using (var result = new ResponseResult<BusinessDetailModel>())
            {
                var entity = new BusinessDetailModel
                            {
                                Id = model.Id,
                                Name = _userService.GetByKey(model.CreateUserId) == null ? "" : _userService.GetByKey(model.CreateUserId).UserName,
                                Province = string.IsNullOrEmpty(model.Province) ? "" : _areaService.GetAreaNamesBy(model.Province),
                                City = string.IsNullOrEmpty(model.City) ? "" : _areaService.GetAreaNamesBy(model.City),
                                Region = string.IsNullOrEmpty(model.Region) ? "" : _areaService.GetAreaNamesBy(model.Region),
                                DemandType = _sysDictionary.GetByKey(model.DemandTypeId) == null ? "" : _sysDictionary.GetByKey(model.DemandTypeId).DisplayName,
                                PublishState = _sysDictionary.GetByKey(model.PublishStateId) == null ? "" : _sysDictionary.GetByKey(model.PublishStateId).DisplayName,
                                ExpectedDate = string.IsNullOrEmpty(model.ExpectedDate) ? "" : model.ExpectedDate,
                                ExpectedEndPrice = model.ExpectedEndPrice,
                                ExpectedStartPrice = model.ExpectedStartPrice,
                                TimeSlot = string.IsNullOrEmpty(model.TimeSlot) ? "" : model.TimeSlot,
                                AcquisitionWeightRangeType = _sysDictionary.GetByKey(model.AcquisitionWeightRangeTypeId) == null ? "" : _sysDictionary.GetByKey(model.AcquisitionWeightRangeTypeId).DisplayName,
                                FirstWeight = _sysDictionary.GetByKey(model.FirstWeight) == null ? "" : _sysDictionary.GetByKey(model.FirstWeight).DisplayName,
                                Brief = string.IsNullOrEmpty(model.Brief) ? "" : model.Brief,
                                CreateTime = model.CreateTime
                            };

                entity.ReplyList = _business.GetDemandReplyList(DemandId);

                result.IsSuccess = true;
                result.Entity = entity;
                return Json(result);
            }
        }
        #endregion

        #region "关闭产业商指定需求"
        /// <summary>
        /// 关闭需求
        /// </summary>
        /// <param name="DemandId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult BusinessClose(string ids)
        {
            var idList = ids.Split(',');
            foreach (var id in idList)
            {
                var entityId = Convert.ToInt64(id);
                var entity = _business.GetByKey(entityId);
                if (entity.PublishStateId == 100505)
                {
                    break;
                }
                entity.PublishStateId = 100505;
                _business.Update(entity);
            }
            var result = new ResponseResult<object>();
            result.IsSuccess = true;
            return Json(result);
        }
        #endregion

        #region "关闭大农户指定需求"
        /// <summary>
        /// 关闭大农户需求
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FarmerClose(string ids)
        {
            var idList = ids.Split(',');
            foreach (var id in idList)
            {
                var entityId = Convert.ToInt64(id);
                var entity = _farmerRequirement.GetByKey(entityId);
                if (entity.PublishStateId == 100505)//系统关闭
                {
                    break;
                }
                entity.PublishStateId = 100505;
                _farmerRequirement.Update(entity);
            }
            var result = new ResponseResult<object>();
            result.IsSuccess = true;
            return Json(result);
        }
        #endregion

        #region "大农户需求列表"
        /// <summary>
        /// 大农户需求列表
        /// </summary>
        /// <param name="seachModel"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FarmerList(FarmerSeachModel seachModel)
        {
            using (var result = new ResponseResult<List<FarmerDemandModel>>())
            {
                int totalCount = 0;
                var entitys = _farmerRequirement.GetAll(seachModel, out totalCount).Select(model => new FarmerDemandModel
                    {
                        Id = model.Id,
                        City = string.IsNullOrEmpty(model.City) ? "" : _areaService.GetAreaNamesBy(model.City),
                        CreateTime = model.CreateTime,
                        CreateUser = _userService.GetByKey(model.CreateUserId) == null ? "" : _userService.GetByKey(model.CreateUserId).UserName,
                        DemandType = _sysDictionary.GetByKey(model.DemandTypeId) == null ? "" : _sysDictionary.GetByKey(model.DemandTypeId).DisplayName,
                        Province = string.IsNullOrEmpty(model.Province) ? "" : _areaService.GetAreaNamesBy(model.Province),
                        PublishState = _sysDictionary.GetByKey(model.PublishStateId) == null ? "" : _sysDictionary.GetByKey(model.PublishStateId).DisplayName,
                        Region = string.IsNullOrEmpty(model.Region) ? "" : _areaService.GetAreaNamesBy(model.Region),
                        IsDeleted = model.IsDeleted
                    }).ToList();
                result.PageIndex = seachModel.pageIndex;
                result.PageSize = seachModel.pageSize;
                result.TotalNums = totalCount;
                result.Entity = entitys;
                result.IsSuccess = true;
                return Json(result);
            }
        }
        #endregion

        #region "大农户需求详情"
        /// <summary>
        /// 大农户详细
        /// </summary>
        /// <param name="DemandId"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult FarmerDetail(int DemandId)
        {
            var model = _farmerRequirement.GetByKey(DemandId);
            using (var result = new ResponseResult<FarmerDetailModel>())
            {
                var entity = new FarmerDetailModel
                            {
                                Id = model.Id,
                                Name = _userService.GetByKey(model.CreateUserId) == null ? "" : _userService.GetByKey(model.CreateUserId).UserName,
                                Province = string.IsNullOrEmpty(model.Province) ? "" : _areaService.GetAreaNamesBy(model.Province),
                                City = string.IsNullOrEmpty(model.City) ? "" : _areaService.GetAreaNamesBy(model.City),
                                Region = string.IsNullOrEmpty(model.Region) ? "" : _areaService.GetAreaNamesBy(model.Region),
                                DemandType = _sysDictionary.GetByKey(model.DemandTypeId) == null ? "" : _sysDictionary.GetByKey(model.DemandTypeId).DisplayName,
                                PublishState = _sysDictionary.GetByKey(model.PublishStateId) == null ? "" : _sysDictionary.GetByKey(model.PublishStateId).DisplayName,
                                ExpectedDate = string.IsNullOrEmpty(model.ExpectedDate) ? "" : model.ExpectedDate,
                                ExpectedEndPrice = model.ExpectedEndPrice,
                                ExpectedStartPrice = model.ExpectedStartPrice,
                                TimeSlot = string.IsNullOrEmpty(model.TimeSlot) ? "" : model.TimeSlot,
                                DetailedAddress = string.IsNullOrEmpty(model.DetailedAddress) ? "" : model.DetailedAddress,
                                Crop = _sysDictionary.GetByKey(model.CropId) == null ? "" : _sysDictionary.GetByKey(model.CropId).DisplayName,
                                Variety = _sysDictionary.GetByKey(model.VarietyId) == null ? "" : _sysDictionary.GetByKey(model.VarietyId).DisplayName,
                                Acres = _sysDictionary.GetByKey(model.AcresId) == null ? "" : _sysDictionary.GetByKey(model.AcresId).DisplayName,
                                Brief = string.IsNullOrEmpty(model.Brief) ? "" : model.Brief,
                                CreateTime = model.CreateTime
                            };

                entity.ReplyList = _farmerRequirement.GetDemandReplyList(DemandId);
                result.IsSuccess = true;
                result.Entity = entity;
                return Json(result);
            }
        }
        #endregion

        #region "导出产业商需求列表到Excel"
        [HttpGet]
        [Validate]
        public ActionResult ExportExcelWithBusinessList(BusinessSeachModel seachModel)
        {
            CheckPermission();
            if (seachModel.pageSize > 10000)
            {
                seachModel.pageSize = 10000;
            }
            int totalCount = 0;
            var modelList = _business.GetAll(seachModel, out totalCount).Select(model => new BusinessDemandModel
           {
               Id = model.Id,
               Province = model.Province,
               City = model.City,
               Region = model.Region,
               CreateTime = model.CreateTime,
               CreateUser = model.CreateUserId.ToString(),
               DemandType = ((BusinessDemandType)model.DemandTypeId).GetDescription(),
               PublishState = ((PublishState)model.PublishStateId).GetDescription(),
               PhoneNumber = model.PhoneNumber,
               CropId = model.CropId ?? 0,
               Township = model.Township,
               Village = model.Village,
               FirstWeight = model.FirstWeight,
               AcquisitionWeightRangeTypeId = model.AcquisitionWeightRangeTypeId,
               Brief = model.Brief,
               ExpectedDate = model.ExpectedDate,
               ExpectedStartPrice = model.ExpectedStartPrice,
               ExpectedEndPrice = model.ExpectedEndPrice
           }).ToList();

            var userIdListDictionary = new Dictionary<long, string>();
            if (modelList.Count > 0)
            {
                var areaCodeDictionary = new Dictionary<string, string>();
                modelList.Select(m =>
                {
                    if (ValidatorAreaCode(m.Province) && !areaCodeDictionary.ContainsKey(m.Province))
                        areaCodeDictionary.Add(m.Province, string.Empty);

                    if (ValidatorAreaCode(m.City) && !areaCodeDictionary.ContainsKey(m.City))
                        areaCodeDictionary.Add(m.City, string.Empty);

                    if (ValidatorAreaCode(m.Region) && !areaCodeDictionary.ContainsKey(m.Region))
                        areaCodeDictionary.Add(m.Region, string.Empty);
                    var userId = int.Parse(m.CreateUser);
                    if (!userIdListDictionary.ContainsKey(userId))
                        userIdListDictionary.Add(userId, string.Empty);

                    return m;
                }).Count();
                var areaCodeList = areaCodeDictionary.Keys.ToList();
                var areaInfoList = _areaService.GetAll(p => areaCodeList.Contains(p.AID));
                var userIdList = userIdListDictionary.Keys.ToList();
                var userList = _userService.GetAll(p => userIdList.Contains(p.Id));

                var cropIdList = modelList.Select(p => p.CropId).Distinct().ToList();
                var firstWeigtIdList = modelList.Select(p => p.FirstWeight).Distinct().ToList();
                var AcquisitionWeightRangeTypeIdList = modelList.Select(p => p.AcquisitionWeightRangeTypeId).Distinct().ToList();
                var dicCodeList = new List<int>();
                dicCodeList.AddRange(cropIdList);
                dicCodeList.AddRange(firstWeigtIdList);
                dicCodeList.AddRange(AcquisitionWeightRangeTypeIdList);
                //字典信息集合
                var dicInfoList = _sysDictionary.GetAll(p => dicCodeList.Contains(p.Code));

                HSSFWorkbook workbook = new HSSFWorkbook();
                MemoryStream ms = new MemoryStream();
                // 创建一张工作薄。
                var workSheet = workbook.CreateSheet("产业商需求列表");
                var headerRow = workSheet.CreateRow(0);
                var tableHeaderTexts = new string[] { 
                    "需求单编号",
                    "发起人",
                    "发起人用户编号", 
                    "发起人手机号",
                    "省",
                    "市",
                    "县",
                    "乡",
                    "村",
                    "农作物类型",
                    "需求类型",
                    "起购",
                    "收购区间",
                    "摘要",
                    "发起时间",
                    "期望日期",
                    "期望价格下限",
                    "期望价格上限",
                    "需求状态",
                    "应答人用户编号",
                    "应答人手机号",
                    "应答时间",
                    "提供的重量",
                    "收到的评价"
                };
                ICellStyle style = workbook.CreateCellStyle();
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style.FillPattern = FillPattern.SolidForeground;

                //左对齐样式
                var cellAlignLeftStyle = workbook.CreateCellStyle();
                cellAlignLeftStyle.Alignment = HorizontalAlignment.Left;
                cellAlignLeftStyle.VerticalAlignment = VerticalAlignment.Center;

                //生成列头
                for (int i = 0; i < tableHeaderTexts.Length; i++)
                {
                    var currentCell = headerRow.CreateCell(i);
                    currentCell.SetCellValue(tableHeaderTexts[i]);
                    currentCell.CellStyle = style;
                }

                workSheet.CreateFreezePane(0, 1, 0, 1);

                var currentRowIndex = 0;
                var dateFormatString = "yyyy.MM.dd HH:mm:ss";
                //给用地区信息赋上说明
                foreach (var demand in modelList)
                {
                    currentRowIndex++;
                    //获取应答列表
                    var responseRecordList = _business.GetDemandReplyList(demand.Id);
                    var responseRecordCount = responseRecordList.Count;

                    //创建内容行
                    var sheetRow = workSheet.CreateRow(currentRowIndex);//创建一行
                    var tableHeadersLength = tableHeaderTexts.Length;
                    //创建内容列
                    for (int cellIndex = 0; cellIndex < tableHeadersLength; cellIndex++)
                    {
                        var cell = sheetRow.CreateCell(cellIndex);
                        cell.SetCellType(CellType.String);
                        cell.CellStyle = cellAlignLeftStyle;

                        if (responseRecordCount > 1)
                        {
                            if (cellIndex < (tableHeadersLength - 5))
                            {
                                workSheet.AddMergedRegion(new CellRangeAddress(currentRowIndex, currentRowIndex + responseRecordCount - 1, cellIndex, cellIndex));
                            }
                        }
                    }

                    //需求单编号
                    sheetRow.Cells[0].SetCellValue(demand.Id);
                    //发起人
                    var demandCreateUserId = int.Parse(demand.CreateUser);
                    var userInfo = userList.Where(p => p.Id == demandCreateUserId).FirstOrDefault();
                    sheetRow.Cells[1].SetCellValue(userInfo == null ? string.Empty : userInfo.UserName);
                    //发起人用户编号
                    sheetRow.Cells[2].SetCellValue(demand.CreateUser);
                    //发起人手机号
                    sheetRow.Cells[3].SetCellValue(demand.PhoneNumber);
                    //省份
                    var provinceInfo = areaInfoList.Where(p => p.AID == demand.Province).FirstOrDefault();
                    sheetRow.Cells[4].SetCellValue(provinceInfo == null ? string.Empty : provinceInfo.DisplayName);
                    //城市
                    var cityInfo = areaInfoList.Where(p => p.AID == demand.City).FirstOrDefault();
                    sheetRow.Cells[5].SetCellValue(cityInfo == null ? string.Empty : cityInfo.DisplayName);
                    //区县
                    var regionInfo = areaInfoList.Where(p => p.AID == demand.Region).FirstOrDefault();
                    sheetRow.Cells[6].SetCellValue(regionInfo == null ? string.Empty : regionInfo.DisplayName);
                    //乡镇
                    var townshipInfo = areaInfoList.Where(p => p.AID == demand.Township).FirstOrDefault();
                    sheetRow.Cells[7].SetCellValue(townshipInfo == null ? string.Empty : townshipInfo.DisplayName);
                    //村
                    var villageInfo = areaInfoList.Where(p => p.AID == demand.Village).FirstOrDefault();
                    sheetRow.Cells[8].SetCellValue(villageInfo == null ? string.Empty : villageInfo.DisplayName);
                    //农作物类型
                    var cropInfo = dicInfoList.Where(p => p.Code == demand.CropId).FirstOrDefault();
                    sheetRow.Cells[9].SetCellValue(cropInfo == null ? string.Empty : cropInfo.DisplayName);
                    //需求类型
                    sheetRow.Cells[10].SetCellValue(demand.DemandType);
                    //起购
                    var firstWeightInfo = dicInfoList.Where(p => p.Code == demand.FirstWeight).FirstOrDefault();
                    sheetRow.Cells[11].SetCellValue(firstWeightInfo == null ? string.Empty : firstWeightInfo.DisplayName);
                    //收购区间
                    var acquisitionWeightRangeInfo = dicInfoList.Where(p => p.Code == demand.AcquisitionWeightRangeTypeId).FirstOrDefault();
                    sheetRow.Cells[12].SetCellValue(acquisitionWeightRangeInfo == null ? string.Empty : acquisitionWeightRangeInfo.DisplayName);
                    //摘要
                    sheetRow.Cells[13].SetCellValue(demand.Brief ?? string.Empty);
                    //发起时间
                    sheetRow.Cells[14].SetCellValue(demand.CreateTime.ToString(dateFormatString));
                    //期望日期
                    sheetRow.Cells[15].SetCellValue(demand.ExpectedDate ?? string.Empty);
                    //期望的价格下限
                    double expectedEndPrice = demand.ExpectedEndPrice == null ? double.MinValue : (double)demand.ExpectedEndPrice.Value;
                    sheetRow.Cells[16].SetCellValue(expectedEndPrice);
                    //期望的价格上限
                    double expectedStartPrice = demand.ExpectedStartPrice == null ? double.MinValue : (double)demand.ExpectedStartPrice.Value;
                    sheetRow.Cells[17].SetCellValue(expectedStartPrice);
                    //需求状态
                    sheetRow.Cells[18].SetCellValue(demand.PublishState);
                    if (responseRecordCount > 0)
                    {
                        var responseRecord = responseRecordList[0];
                        sheetRow.Cells[19].SetCellValue(responseRecord.UserId);
                        sheetRow.Cells[20].SetCellValue(responseRecord.PhoneNumber ?? string.Empty);
                        sheetRow.Cells[21].SetCellValue(responseRecord.ReplyTime.ToString(dateFormatString));
                        sheetRow.Cells[22].SetCellValue(responseRecord.WeightRange);
                        string commentResult = string.Empty;
                        var scoreString = "|" + responseRecord.Score + "星";
                        if (responseRecord.Comments.IsNullOrEmpty() == false || responseRecord.Score > 0)
                        {
                            //收到的评价
                            sheetRow.Cells[23].SetCellValue(responseRecord.Comments.IsNullOrEmpty() ? "[未作评论]" + scoreString : responseRecord.Comments + scoreString);
                        }
                    }

                    if (responseRecordCount > 1)
                    {
                        for (int responseRecordIndex = 1; responseRecordIndex < responseRecordCount; responseRecordIndex++)
                        {
                            currentRowIndex++;
                            var replyRecordRow = workSheet.CreateRow(currentRowIndex);
                            //创建内容列
                            for (int cellIndex = 0; cellIndex < tableHeadersLength; cellIndex++)
                            {
                                var cell = replyRecordRow.CreateCell(cellIndex);
                                cell.SetCellType(CellType.String);
                                cell.CellStyle = cellAlignLeftStyle;
                            }

                            var responseRecord = responseRecordList[responseRecordIndex];
                            replyRecordRow.Cells[19].SetCellValue(responseRecord.UserId);
                            replyRecordRow.Cells[20].SetCellValue(responseRecord.PhoneNumber ?? string.Empty);
                            replyRecordRow.Cells[21].SetCellValue(responseRecord.ReplyTime.ToString(dateFormatString));
                            replyRecordRow.Cells[22].SetCellValue(responseRecord.WeightRange);
                            string commentResult = string.Empty;
                            var scoreString = "|" + responseRecord.Score + "星";
                            if (responseRecord.Comments.IsNullOrEmpty() == false || responseRecord.Score > 0)
                            {
                                //收到的评价
                                replyRecordRow.Cells[23].SetCellValue(responseRecord.Comments.IsNullOrEmpty() ? "[未作评论]" + scoreString : responseRecord.Comments + scoreString);
                            }
                        }
                    }

                }

                workbook.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=BusinessDemandList" + (DateTime.Now.ToString("yyyyMMddHHmmss")) + ".xls"));
                Response.BinaryWrite(ms.ToArray()); workbook = null;
                return File(ms, "application/ms-excel");
            }
            else
            {
                return Content("no data");
            }

        }
        #endregion

        #region "导出大农户需求列表到Excel"
        public ActionResult ExportExcelWithFarmList(FarmerSeachModel seachModel)
        {
            CheckPermission();
            if (seachModel.pageSize > 10000)
            {
                seachModel.pageSize = 10000;
            }
            int totalCount = 0;
            var modelList = _farmerRequirement.GetAll(seachModel, out totalCount).Select(model => new FarmerDemandModel
            {
                Id = model.Id,
                Province = model.Province,
                City = model.City,
                Region = model.Region,
                Township = model.Township,
                Village = model.Village,
                CreateTime = model.CreateTime,
                CreateUser = model.CreateUserId.ToString(),
                DemandType = ((FarmerDemandType)model.DemandTypeId).GetDescription(),
                PublishState = ((PublishState)model.PublishStateId).GetDescription(),
                PhoneNumber = model.PhoneNumber,
                CropId = model.CropId,
                AcresId = model.AcresId,
                Brief = model.Brief,
                ExpectedDate = model.ExpectedDate
            }).ToList();

            var userIdListDictionary = new Dictionary<long, string>();
            if (modelList.Count > 0)
            {
                var areaCodeDictionary = new Dictionary<string, string>();
                modelList.Select(m =>
                {
                    if (ValidatorAreaCode(m.Province) && !areaCodeDictionary.ContainsKey(m.Province))
                        areaCodeDictionary.Add(m.Province, string.Empty);

                    if (ValidatorAreaCode(m.City) && !areaCodeDictionary.ContainsKey(m.City))
                        areaCodeDictionary.Add(m.City, string.Empty);

                    if (ValidatorAreaCode(m.Region) && !areaCodeDictionary.ContainsKey(m.Region))
                        areaCodeDictionary.Add(m.Region, string.Empty);
                    var userId = int.Parse(m.CreateUser);
                    if (!userIdListDictionary.ContainsKey(userId))
                        userIdListDictionary.Add(userId, string.Empty);

                    return m;
                }).Count();

                var cropIdList = modelList.Select(p => p.CropId).Distinct().ToList();
                var acresIdList = modelList.Select(p => p.AcresId).Distinct().ToList();
                var areaCodeList = areaCodeDictionary.Keys.ToList();
                //地区列表集合
                var areaInfoList = _areaService.GetAll(p => areaCodeList.Contains(p.AID));
                var userIdList = userIdListDictionary.Keys.ToList();
                //用户列表集合
                var userList = _userService.GetAll(p => userIdList.Contains(p.Id));
                var dicCodeList = new List<int>();
                dicCodeList.AddRange(cropIdList);
                dicCodeList.AddRange(acresIdList);
                //字典信息集合
                var dicInfoList = _sysDictionary.GetAll(p => dicCodeList.Contains(p.Code));

                HSSFWorkbook workbook = new HSSFWorkbook();
                MemoryStream ms = new MemoryStream();
                // 创建一张工作薄。
                var workSheet = workbook.CreateSheet("大农户需求列表");
                //创建列头文本
                var tableHeaderTexts = new string[] { 
                    "需求单编号", 
                    "发起人",
                    "发起人用户编号", 
                    "发起人手机号", 
                    "省", 
                    "市", 
                    "区/县",
                    "乡",
                    "村",
                    "农作物类型",
                    "需求类型",
                    "亩数",
                    "摘要",
                    "发起时间",
                    "期望日期",
                    "需求状态",
                    "应答人用户编号",
                    "应答人手机号",
                    "应答时间",
                    "收到的评价"
                };
                //设置列头样式
                ICellStyle style = workbook.CreateCellStyle();
                style.FillForegroundColor = NPOI.HSSF.Util.HSSFColor.Grey25Percent.Index;
                style.FillPattern = FillPattern.SolidForeground;

                var headerRow = workSheet.CreateRow(0);
                //生成列头
                for (int i = 0; i < tableHeaderTexts.Length; i++)
                {
                    var currentCell = headerRow.CreateCell(i);
                    currentCell.SetCellValue(tableHeaderTexts[i]);
                    currentCell.CellStyle = style;
                }
                workSheet.CreateFreezePane(0, 1, 0, 1);
                var currentRoeIndex = 0;
                var dateFormatString = "yyyy.MM.dd HH:mm:ss";

                //给用地区信息赋上说明
                foreach (var demand in modelList)
                {
                    currentRoeIndex++;
                    //创建内容行
                    var sheetRow = workSheet.CreateRow(currentRoeIndex);//创建一行
                    //创建内容列
                    for (int cellIndex = 0; cellIndex < tableHeaderTexts.Length; cellIndex++)
                    {
                        var cell = sheetRow.CreateCell(cellIndex);
                        cell.SetCellType(CellType.String);
                    }

                    //需求单编号
                    sheetRow.Cells[0].SetCellValue(demand.Id);
                    //发起人
                    var demandCreateUserId = int.Parse(demand.CreateUser);
                    var userInfo = userList.Where(p => p.Id == demandCreateUserId).FirstOrDefault();
                    sheetRow.Cells[1].SetCellValue(userInfo == null ? string.Empty : userInfo.UserName);
                    //发起人用户编号
                    sheetRow.Cells[2].SetCellValue(demand.CreateUser);
                    //发起人手机号
                    sheetRow.Cells[3].SetCellValue(demand.PhoneNumber);
                    //省份
                    var provinceInfo = areaInfoList.Where(p => p.AID == demand.Province).FirstOrDefault();
                    sheetRow.Cells[4].SetCellValue(provinceInfo == null ? string.Empty : provinceInfo.DisplayName);
                    //城市
                    var cityInfo = areaInfoList.Where(p => p.AID == demand.City).FirstOrDefault();
                    sheetRow.Cells[5].SetCellValue(cityInfo == null ? string.Empty : cityInfo.DisplayName);
                    //区县
                    var regionInfo = areaInfoList.Where(p => p.AID == demand.Region).FirstOrDefault();
                    sheetRow.Cells[6].SetCellValue(regionInfo == null ? string.Empty : regionInfo.DisplayName);
                    //乡镇
                    var townshipInfo = areaInfoList.Where(p => p.AID == demand.Township).FirstOrDefault();
                    sheetRow.Cells[7].SetCellValue(townshipInfo == null ? string.Empty : townshipInfo.DisplayName);
                    //村
                    var villageInfo = areaInfoList.Where(p => p.AID == demand.Village).FirstOrDefault();
                    sheetRow.Cells[8].SetCellValue(villageInfo == null ? string.Empty : villageInfo.DisplayName);
                    //农作物类型
                    var cropInfo = dicInfoList.Where(p => p.Code == demand.CropId).FirstOrDefault();
                    sheetRow.Cells[9].SetCellValue(cropInfo == null ? string.Empty : cropInfo.DisplayName);
                    //需求类型
                    sheetRow.Cells[10].SetCellValue(demand.DemandType);
                    //亩数
                    var acresInfo = dicInfoList.Where(p => p.Code == demand.AcresId).FirstOrDefault();
                    sheetRow.Cells[11].SetCellValue(acresInfo == null ? string.Empty : acresInfo.DisplayName);
                    //摘要
                    sheetRow.Cells[12].SetCellValue(demand.Brief ?? string.Empty);
                    //发起时间
                    sheetRow.Cells[13].SetCellValue(demand.CreateTime.ToString(dateFormatString));
                    //期望日期
                    sheetRow.Cells[14].SetCellValue(demand.ExpectedDate ?? string.Empty);
                    //需求状态
                    sheetRow.Cells[15].SetCellValue(demand.PublishState);
                    //应答人用户编号
                    var responseRecord = _farmerRequirement.GetDemandReplyList(demand.Id).FirstOrDefault();
                    if (responseRecord != null)
                    {
                        //应答人用户编号
                        sheetRow.Cells[16].SetCellValue(responseRecord.UserId);
                        //应答人手机号
                        sheetRow.Cells[17].SetCellValue(responseRecord.PhoneNumber);
                        //应答时间
                        sheetRow.Cells[18].SetCellValue(responseRecord.ReplyTime.ToString(dateFormatString));
                        string commentResult = string.Empty;
                        var scoreString = "|" + responseRecord.Score + "星";
                        if (responseRecord.Comments.IsNullOrEmpty() == false || responseRecord.Score > 0)
                        {
                            //收到的评价
                            sheetRow.Cells[19].SetCellValue(responseRecord.Comments.IsNullOrEmpty() ? "[未作评论]" + scoreString : responseRecord.Comments + scoreString);
                        }
                    }
                }

                workbook.Write(ms);
                Response.AddHeader("Content-Disposition", string.Format("attachment;filename=FarmerDemandList" + (DateTime.Now.ToString("yyyyMMddHHmmss")) + ".xls"));
                Response.BinaryWrite(ms.ToArray()); workbook = null;
                return File(ms, "application/ms-excel");
            }
            else
            {
                return Content("no data");
            }
        }
        #endregion

        private static bool ValidatorAreaCode(string areaCode)
        {
            return areaCode.IsNullOrEmpty() == false && areaCode.Length > 3;
        }
    }
}
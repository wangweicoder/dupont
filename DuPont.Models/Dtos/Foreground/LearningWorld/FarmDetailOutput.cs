// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 01-05-2016
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 01-05-2016
// ***********************************************************************
// <copyright file="FarmDetailOutput.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

using System;
using System.Collections.Generic;
using System.Security.AccessControl;

namespace DuPont.Models.Dtos.Foreground.LearningWorld
{
    public class FarmDetailOutput
    {
        //种植面积
        public string PlantAreaYield { get; set; }

        //品种
        public string Variety { get; set; }

        //播种时间
        public string SowTime { get; set; }

        //种植要点
        public string PlantPoint { get; set; }

        //农场是否开放
        public bool IsOpen { get; set; }

        //农场开放开始日期
        public string OpenStartDate { get; set; }

        //农场开放截止日期
        public string OpenEndDate { get; set; }

        //展区列表
        public List<FarmArea> AreaList { get; set; }
    }

    public class FarmArea
    {
        //展区编号
        public int ExhibitionAreaId { get; set; }
        //展区名称
        public string Name { get; set; }
        //展区静态页地址
        public string Url { get; set; }
        //是否是农机区
        public bool IsFarmMachinery { get; set; }
    }
}
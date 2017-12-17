// ***********************************************************************
// Assembly         : DuPont.Models
// Author           : 毛文君
// Created          : 01-07-2016
// Tel              :15801270290
// QQ               :731314565
//
// Last Modified By : 毛文君
// Last Modified On : 01-07-2016
// ***********************************************************************
// <copyright file="FarmListOutput.cs" company="">
//     Copyright (c) . All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************
using System;

namespace DuPont.Models.Dtos.Background.LearningWorld
{
    public class FarmListOutput
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public bool IsOpen { get; set; }
        public bool IsDeleted { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string Region { get; set; }
        public DateTime? OpenStartDate { get; set; }
        public DateTime? OpenEndDate { get; set; }
    }
}

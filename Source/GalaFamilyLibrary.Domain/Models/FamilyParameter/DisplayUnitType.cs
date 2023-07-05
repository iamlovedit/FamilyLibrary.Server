﻿using GalaFamilyLibrary.Infrastructure.Common;
using SqlSugar;

namespace GalaFamilyLibrary.Domain.Models.FamilyParameter
{
    [SugarTable("parameter_displayUnitType")]
    public class DisplayUnitType : IDeletable
    {
        [SugarColumn(ColumnName = "displayUnitType_id", IsPrimaryKey = true)]
        public long Id { get; set; }

        [SugarColumn(ColumnName = "displayUnitType_value", IsPrimaryKey = true)]
        public string Value { get; set; }

        [SugarColumn(ColumnName = "displayUnitType_name")]
        public string Name { get; set; }

        [SugarColumn(ColumnName = "displayUnitType_isDeleted")]
        public bool IsDeleted { get; set; }
    }
}
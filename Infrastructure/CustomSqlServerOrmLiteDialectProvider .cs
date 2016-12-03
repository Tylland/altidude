using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using ServiceStack.OrmLite.SqlServer;

namespace Altidude.Infrastructure.SqlServerOrmLite
{
    public class CustomSqlServerOrmLiteDialectProvider : SqlServerOrmLiteDialectProvider
    {
        private const string DateTimeOffsetColumnDefinition = "datetimeoffset(7)";

        public CustomSqlServerOrmLiteDialectProvider()
        {
            DbTypeMap.Set<DateTimeOffset>(DbType.DateTimeOffset, DateTimeOffsetColumnDefinition);
        }
        public override object ConvertDbValue(object value, Type type)
        {
            return base.ConvertDbValue(value, type);
        }

        public override string GetColumnDefinition(string fieldName, Type fieldType,
                   bool isPrimaryKey, bool autoIncrement, bool isNullable,
                   int? fieldLength, int? scale, string defaultValue)
        {
            var fieldDefinition = base.GetColumnDefinition(fieldName, fieldType,
                                           isPrimaryKey, autoIncrement, isNullable,
                                           fieldLength, scale, defaultValue);

            if (fieldType == typeof(string) && fieldLength > 8000)
            {
                var orig = string.Format(StringLengthColumnDefinitionFormat, fieldLength);
                var max = string.Format(StringLengthColumnDefinitionFormat, "MAX");

                fieldDefinition = fieldDefinition.Replace(orig, max);
            }

            return fieldDefinition;
        }
        
        //public override string GetColumnDefinition(string fieldName, Type fieldType, bool isPrimaryKey, bool autoIncrement, bool isNullable, int? fieldLength, int? scale, string defaultValue)
        //{
        //    return base.GetColumnDefinition(fieldName, fieldType, isPrimaryKey, autoIncrement, isNullable, fieldLength, scale, defaultValue);
        //}
    }
}
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Migrations.Operations;
using Pomelo.EntityFrameworkCore.MySql.Infrastructure.Internal;
using Qj.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Qj.Core
{
    public class MyMigrationsSqlGenerator : MySqlMigrationsSqlGenerator
    {
        public MyMigrationsSqlGenerator(
            MigrationsSqlGeneratorDependencies dependencies,
            IMigrationsAnnotationProvider migrationsAnnotations,
            IMySqlOptions mySqlOptions)
            : base(dependencies, migrationsAnnotations, mySqlOptions)
        {
        }

        protected override void Generate(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            base.Generate(operation, model, builder);

            try
            {
                if (operation is CreateTableOperation || operation is AlterTableOperation)
                    CreateTableComment(operation, model, builder);
            }
            catch (Exception ex)
            {
                Console.WriteLine("生成注释错误：" + ex.Message);
            }


            try
            {
                if (operation is AddColumnOperation || operation is AlterColumnOperation)
                    CreateColumnComment(operation, model, builder);
            }
            catch (Exception ex)
            {
                Console.WriteLine("生成注释错误：" + ex.Message);
            }

        }

        /// <summary>
        /// Create table comment.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="builder"></param>
        private void CreateTableComment(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            string tableName = string.Empty;
            string description = string.Empty;
            if (operation is AlterTableOperation)
            {
                var t = operation as AlterColumnOperation;
                tableName = (operation as AlterTableOperation).Name;
            }

            if (operation is CreateTableOperation)
            {
                var t = operation as CreateTableOperation;
                var addColumnsOperation = t.Columns;
                tableName = (operation as CreateTableOperation).Name;

                foreach (var item in addColumnsOperation)
                {
                    CreateColumnComment(item, model, builder);
                }
            }
            description = DbDescriptionHelperBysummary.GetDescription(tableName);

            if (tableName.IsNullOrWhiteSpace())
                throw new Exception("Create table comment error.");

            var sqlHelper = Dependencies.SqlGenerationHelper;
            builder
            .Append("ALTER TABLE ")
            .Append(sqlHelper.DelimitIdentifier(tableName)/*.ToLower()*/)
            .Append(" COMMENT ")
            .Append("'")
            .Append(description)
            .Append("'")
            .AppendLine(sqlHelper.StatementTerminator)
            .EndCommand();

        }

        /// <summary>
        /// Create column comment.
        /// </summary>
        /// <param name="operation"></param>
        /// <param name="builder"></param>
        private void CreateColumnComment(MigrationOperation operation, IModel model, MigrationCommandListBuilder builder)
        {
            //alter table a1log modify column UUID VARCHAR(26) comment '修改后的字段注释';
            string tableName = string.Empty;
            string columnName = string.Empty;
            string columnType = string.Empty;
            string description = string.Empty;
            if (operation is AlterColumnOperation)
            {
                var t = (operation as AlterColumnOperation);
                tableName = t.Table;
                columnName = t.Name;
                columnType = GetColumnType(t.Schema, t.Table, t.Name, t.ClrType, t.IsUnicode, t.MaxLength, t.IsFixedLength, t.IsRowVersion, model);
                description = DbDescriptionHelperBysummary.GetDescription(tableName, columnName);
            }

            if (operation is AddColumnOperation)
            {
                var t = (operation as AddColumnOperation);
                tableName = t.Table;
                columnName = t.Name;
                columnType = GetColumnType(t.Schema, t.Table, t.Name, t.ClrType, t.IsUnicode, t.MaxLength, t.IsFixedLength, t.IsRowVersion, model);
                description = DbDescriptionHelperBysummary.GetDescription(tableName, columnName);
            }

            if (columnName.IsNullOrWhiteSpace() || tableName.IsNullOrWhiteSpace() || columnType.IsNullOrWhiteSpace())
                throw new Exception("Create columnt comment error." + columnName + "/" + tableName + "/" + columnType);

            int iszzpk = DbDescriptionHelperBysummary.Getzzpk(tableName, columnName);

            var sqlHelper = Dependencies.SqlGenerationHelper;
            builder
            .Append("ALTER TABLE ")
            .Append(sqlHelper.DelimitIdentifier(tableName)/*.ToLower()*/)
            .Append(" MODIFY COLUMN ")
            .Append(" ")
            .Append(columnName == "Option" ? $"`{columnName}`" : columnName)
            .Append(" ")
            .Append(" ")
            .Append(columnType)
            .Append(" COMMENT ")
            .Append("'")
            .Append(description)
            .Append("'")
            .Append(" ")
            .Append(iszzpk == 1 ? "  auto_increment " : "")
            .AppendLine(sqlHelper.StatementTerminator)
            .EndCommand();

        }
    }



}

#region License
//
// Copyright (c) 2007-2018, Sean Chambers <schambers80@gmail.com>
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
//   http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
//
#endregion

using System;
using System.Collections.Generic;
using FluentMigrator.Infrastructure;

namespace FluentMigrator.Expressions
{
    /// <summary>
    /// Expression to move a table from one schema to another
    /// </summary>
    public class AlterSchemaExpression : MigrationExpressionBase
    {
        /// <summary>
        /// Gets or sets the source schema name
        /// </summary>
        public virtual string SourceSchemaName { get; set; }

        /// <summary>
        /// Gets or sets the table name
        /// </summary>
        public virtual string TableName { get; set; }

        /// <summary>
        /// Gets or sets the destination schema name
        /// </summary>
        public virtual string DestinationSchemaName { get; set; }

        /// <inheritdoc />
        public override void CollectValidationErrors(ICollection<string> errors)
        {
            if (String.IsNullOrEmpty(TableName))
                errors.Add(ErrorMessages.TableNameCannotBeNullOrEmpty);
            if (String.IsNullOrEmpty(DestinationSchemaName))
                errors.Add(ErrorMessages.DestinationSchemaCannotBeNull);
        }

        /// <inheritdoc />
        public override void ExecuteWith(IMigrationProcessor processor)
        {
            processor.Process(this);
        }

        /// <inheritdoc />
        public override string ToString()
        {
            return base.ToString() + DestinationSchemaName + " Table " + TableName;
        }
    }
}

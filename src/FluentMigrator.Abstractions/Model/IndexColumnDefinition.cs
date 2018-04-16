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
using FluentMigrator.Infrastructure.Extensions;

namespace FluentMigrator.Model
{
    /// <summary>
    /// Index column definition
    /// </summary>
    public class IndexColumnDefinition : ICloneable, ICanBeValidated, ISupportAdditionalFeatures
    {
        /// <summary>
        /// Gets or sets the column name
        /// </summary>
        public virtual string Name { get; set; }

        /// <summary>
        /// Gets or sets the sort direction of the index column
        /// </summary>
        public virtual Direction Direction { get; set; }

        /// <inheritdoc />
        public virtual IDictionary<string, object> AdditionalFeatures { get; } = new Dictionary<string, object>();

        /// <inheritdoc />
        public virtual void CollectValidationErrors(ICollection<string> errors)
        {
            if (String.IsNullOrEmpty(Name))
                errors.Add(ErrorMessages.ColumnNameCannotBeNullOrEmpty);
        }

        /// <inheritdoc />
        public object Clone()
        {
            var result = new IndexColumnDefinition()
            {
                Name = Name,
                Direction = Direction,
            };

            AdditionalFeatures.CloneTo(result.AdditionalFeatures);

            return result;
        }
    }
}

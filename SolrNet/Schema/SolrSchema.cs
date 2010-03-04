﻿#region license

// Copyright (c) 2007-2010 Mauricio Scheffer
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
//      http://www.apache.org/licenses/LICENSE-2.0
//  
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

#endregion

using System.Collections.Generic;

namespace SolrNet.Schema
{
    /// <summary>
    /// Represents a Solr schema.
    /// </summary>
    public class SolrSchema
    {
        /// <summary>
        /// Gets or sets the solr fields types.
        /// </summary>
        /// <value>The solr fields types.</value>
        public List<SolrFieldType> SolrFieldTypes { get; set; }

        /// <summary>
        /// Gets or sets the solr fields.
        /// </summary>
        /// <value>The solr fields.</value>
        public List<SolrField> SolrFields { get; set; }
        
        /// <summary>
        /// Gets or sets the solr dynamic fields.
        /// </summary>
        /// <value>The solr dynamic fields.</value>
        public List<SolrDynamicField> SolrDynamicFields { get; set; }

        /// <summary>
        /// Gets or sets the solr copy fields.
        /// </summary>
        /// <value>The solr copy fields.</value>
        /// 
        public List<SolrCopyField> SolrCopyFields { get; set; }

        /// <summary>
        /// Gets or sets the unique key.
        /// </summary>
        /// <value>The unique key.</value>
        public string UniqueKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SolrSchema"/> class.
        /// </summary>
        public SolrSchema()
        {
            this.SolrFieldTypes = new List<SolrFieldType>();
            this.SolrFields = new List<SolrField>();
            this.SolrDynamicFields = new List<SolrDynamicField>();
            this.SolrCopyFields = new List<SolrCopyField>();
        }
    }
}
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
using System.Reflection;
using System.Xml;

namespace SolrNet.Impl {
    /// <summary>
    /// Parses documents from query response
    /// </summary>
    /// <typeparam name="T">Document type</typeparam>
    public class SolrDocumentResponseParser<T> : ISolrDocumentResponseParser<T> where T : new() {
        private readonly IReadOnlyMappingManager mappingManager;
        private readonly ISolrDocumentPropertyVisitor propVisitor;

        public SolrDocumentResponseParser(IReadOnlyMappingManager mappingManager, ISolrDocumentPropertyVisitor propVisitor) {
            this.mappingManager = mappingManager;
            this.propVisitor = propVisitor;
        }

        /// <summary>
        /// Parses documents results
        /// </summary>
        /// <param name="parentNode"></param>
        /// <returns></returns>
        public IList<T> ParseResults(XmlNode parentNode) {
            var results = new List<T>();
            if (parentNode == null)
                return results;
            var allFields = mappingManager.GetFields(typeof (T));
            var nodes = parentNode.SelectNodes("doc");
            if (nodes == null)
                return results;
            foreach (XmlNode docNode in nodes) {
                results.Add(ParseDocument(docNode, allFields));
            }
            return results;
        }

        /// <summary>
        /// Builds a document from the corresponding response xml node
        /// </summary>
        /// <param name="node">response xml node</param>
        /// <param name="fields">document fields</param>
        /// <returns>populated document</returns>
        public T ParseDocument(XmlNode node, ICollection<KeyValuePair<PropertyInfo, string>> fields) {
            var doc = new T();
            foreach (XmlNode field in node.ChildNodes) {
                string fieldName = field.Attributes["name"].InnerText;
                propVisitor.Visit(doc, fieldName, field);
            }
            return doc;
        }
    }
}
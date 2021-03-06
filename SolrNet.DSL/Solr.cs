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
using Microsoft.Practices.ServiceLocation;
using Rhino.Commons;
using SolrNet.Commands;
using SolrNet.Commands.Parameters;
using SolrNet.DSL.Impl;
using SolrNet.Impl;

namespace SolrNet.DSL {
    /// <summary>
    /// Solr DSL Entry point
    /// </summary>
    public static class Solr {
        public static DeleteBy Delete {
            get { return new DeleteBy(Connection); }
        }

        private static readonly string SolrConnectionKey = "ISolrConnection";

        /// <summary>
        /// thread-local or webcontext-local connection
        /// </summary>
        /// <seealso cref="http://www.ayende.com/Blog/archive/7447.aspx"/>
        /// <seealso cref="http://rhino-tools.svn.sourceforge.net/svnroot/rhino-tools/trunk/rhino-commons/Rhino.Commons/LocalDataImpl/"/>
        public static ISolrConnection Connection {
            private get { return (ISolrConnection) Local.Data[SolrConnectionKey]; }
            set { Local.Data[SolrConnectionKey] = value; }
        }

        /// <summary>
        /// Adds/updates a document
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="document"></param>
        public static void Add<T>(T document) {
            Add<T>(new[] {document});
        }

        /// <summary>
        /// Adds/updates a list of documents
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="documents"></param>
        public static void Add<T>(IEnumerable<T> documents) {
            var cmd = new AddCommand<T>(documents, ServiceLocator.Current.GetInstance<ISolrDocumentSerializer<T>>());
            cmd.Execute(Connection);
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="s">Query</param>
        /// <param name="start">Pagination item start</param>
        /// <param name="rows">Pagination item count</param>
        /// <returns>Query results</returns>
        public static ISolrQueryResults<T> Query<T>(string s, int start, int rows) where T : new() {
            var q = new SolrQueryExecuter<T>(Connection, ServiceLocator.Current.GetInstance<ISolrQueryResultParser<T>>());
            return q.Execute(new SolrQuery(s), new QueryOptions {Start = start, Rows = rows});
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="s">Query</param>
        /// <returns>Query results</returns>
        public static ISolrQueryResults<T> Query<T>(string s) where T : new() {
            var q = new SolrQueryExecuter<T>(Connection, ServiceLocator.Current.GetInstance<ISolrQueryResultParser<T>>());
            return q.Execute(new SolrQuery(s), null);
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="s">Query</param>
        /// <param name="order">Sort order</param>
        /// <returns>Query results</returns>
        public static ISolrQueryResults<T> Query<T>(string s, SortOrder order) where T : new() {
            return Query<T>(s, new[] {order});
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="s">Query</param>
        /// <param name="order">Sort orders</param>
        /// <returns>Query results</returns>
        public static ISolrQueryResults<T> Query<T>(string s, ICollection<SortOrder> order) where T : new() {
            var q = new SolrQueryExecuter<T>(Connection, ServiceLocator.Current.GetInstance<ISolrQueryResultParser<T>>());
            return q.Execute(new SolrQuery(s), new QueryOptions {OrderBy = order});
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="s">Query</param>
        /// <param name="order">Sort order</param>
        /// <param name="start">Pagination item start</param>
        /// <param name="rows">Pagination item count</param>
        /// <returns>Query results</returns>
        public static ISolrQueryResults<T> Query<T>(string s, SortOrder order, int start, int rows) where T : new() {
            return Query<T>(s, new[] {order}, start, rows);
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="s">Query</param>
        /// <param name="order">Sort orders</param>
        /// <param name="start">Pagination item start</param>
        /// <param name="rows">Pagination item count</param>
        /// <returns>Query results</returns>
        public static ISolrQueryResults<T> Query<T>(string s, ICollection<SortOrder> order, int start, int rows) where T : new() {
            var q = new SolrQueryExecuter<T>(Connection, ServiceLocator.Current.GetInstance<ISolrQueryResultParser<T>>());
            return q.Execute(new SolrQuery(s), new QueryOptions {
                OrderBy = order,
                Start = start,
                Rows = rows,
            });
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="q">Query</param>
        /// <returns>Query results</returns>
        public static ISolrQueryResults<T> Query<T>(ISolrQuery q) where T : new() {
            var queryExecuter = new SolrQueryExecuter<T>(Connection, ServiceLocator.Current.GetInstance<ISolrQueryResultParser<T>>());
            return queryExecuter.Execute(q, null);
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="q">Query</param>
        /// <param name="start">Pagination item start</param>
        /// <param name="rows">Pagination item count</param>
        /// <returns>Query results</returns>
        public static ISolrQueryResults<T> Query<T>(ISolrQuery q, int start, int rows) where T : new() {
            var queryExecuter = new SolrQueryExecuter<T>(Connection, ServiceLocator.Current.GetInstance<ISolrQueryResultParser<T>>());
            return queryExecuter.Execute(q, new QueryOptions {Start = start, Rows = rows});
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="order">Sort order</param>
        /// <returns>Query results</returns>
        public static ISolrQueryResults<T> Query<T>(SolrQuery query, SortOrder order) where T : new() {
            return Query<T>(query, new[] {order});
        }

        /// <summary>
        /// Executes a query
        /// </summary>
        /// <typeparam name="T">Document type</typeparam>
        /// <param name="query">Query</param>
        /// <param name="orders">Sort orders</param>
        /// <returns>Query results</returns>
        public static ISolrQueryResults<T> Query<T>(SolrQuery query, ICollection<SortOrder> orders) where T : new() {
            var queryExecuter = new SolrQueryExecuter<T>(Connection, ServiceLocator.Current.GetInstance<ISolrQueryResultParser<T>>());
            return queryExecuter.Execute(query, new QueryOptions { OrderBy = orders });
        }

        public static IDSLQuery<T> Query<T>() where T : new() {
            return new DSLQuery<T>(Connection);
        }

        /// <summary>
        /// Commits posted documents
        /// </summary>
        public static void Commit() {
            var cmd = new CommitCommand();
            cmd.Execute(Connection);
        }

        /// <summary>
        /// Commits posted documents
        /// </summary>
        /// <param name="waitFlush">wait for flush</param>
        /// <param name="waitSearcher">wait for new searcher</param>
        public static void Commit(bool waitFlush, bool waitSearcher) {
            var cmd = new CommitCommand {WaitFlush = waitFlush, WaitSearcher = waitSearcher};
            cmd.Execute(Connection);
        }

        /// <summary>
        /// Optimizes Solr's index
        /// </summary>
        public static void Optimize() {
            var cmd = new OptimizeCommand();
            cmd.Execute(Connection);
        }

        /// <summary>
        /// Optimizes Solr's index
        /// </summary>
        /// <param name="waitFlush">Wait for flush</param>
        /// <param name="waitSearcher">Wait for new searcher</param>
        public static void Optimize(bool waitFlush, bool waitSearcher) {
            var cmd = new OptimizeCommand {WaitFlush = waitFlush, WaitSearcher = waitSearcher};
            cmd.Execute(Connection);
        }
    }
}
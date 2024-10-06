using Dapper;
using Domain.Repositories;
using Domain.Services.HelperServices;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;


namespace Domain.Services.EntityServices
{
    public class PaginatedResult<T>
    {
        public IEnumerable<T> Items { get; set; }
        public int TotalCount { get; set; }
        public int TotalPages { get; set; }
        public int CurrentPage { get; set; }
        public int PageSize { get; set; }
    }
   public class SQLGenericRepository<T> : ISQLGenericRepository<T> where T : class
    {
        IDbConnection _connection;
        private readonly IConnectionStringBuilder _connectionStringBuilder;
        private readonly string blobConnectionString;

        public SQLGenericRepository(IConnectionStringBuilder connectionStringBuilder)
        {
            _connectionStringBuilder = connectionStringBuilder;
            _connection = new SqlConnection(_connectionStringBuilder.SqlConnectionString);
        }

        public async Task<bool> Add(T entity)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = GetTableName();
                string columns = GetColumns(excludeKey: true);
                string properties = GetPropertyNames(excludeKey: true);
                string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";
                rowsEffected = await _connection.ExecuteAsync(query, entity);
                return rowsEffected > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<T> Add(T entity, string outputParams)
        {
            try
            {
                string tableName = GetTableName();
                string columns = GetColumns(excludeKey: true);
                string properties = GetPropertyNames(excludeKey: true);
                string query = $"INSERT INTO {tableName} ({columns}) {outputParams} VALUES ({properties})";

                var response = await _connection.QuerySingleAsync<T>(query, entity);
                return response;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddBulk(List<T> entities)
        {
            int batchSize = 1000;
            int totalRecords = entities.Count;
            int totalBatches = (int)Math.Ceiling((double)totalRecords / batchSize);
            int rowsEffected = 0;

            try
            {
                string tableName = GetTableName();
                string columns = GetColumns(excludeKey: true);

                for (int i = 0; i < totalBatches; i++)
                {
                    var batchEntities = entities.Skip(i * batchSize).Take(batchSize).ToList();
                    var queryBuilder = new StringBuilder();

                    for (int j = 0; j < batchEntities.Count; j++)
                    {
                        var entity = batchEntities[j];
                        var properties = GetProperties(excludeKey: true);

                        for (int k = 0; k < properties.Count(); k++)
                        {
                            var property = properties.ElementAt(k);
                            var propertyName = property.Name;
                            var parameterName = $"{propertyName}_{j}";

                            var propertyValue = property.GetValue(entity);
                            if (property.PropertyType == typeof(string))
                            {
                                queryBuilder.Append($"'{propertyValue}'");
                            }
                            else
                            {
                                queryBuilder.Append(propertyValue);
                            }

                            if (k < properties.Count() - 1)
                            {
                                queryBuilder.Append(", ");
                            }
                        }

                        if (j < batchEntities.Count - 1)
                        {
                            queryBuilder.Append("), (");
                        }
                    }

                    var query = $"INSERT INTO {tableName} ({columns}) VALUES ({queryBuilder})";
                    rowsEffected += await _connection.ExecuteAsync(query);
                }
            }
            catch (Exception)
            {
                throw;
            }

            return rowsEffected == totalRecords;
        }
        //public async Task<bool> AddBulk(List<T> entity)
        //{
        //    try
        //    {
        //        string tableName = GetTableName();
        //        string columns = GetColumns(excludeKey: true);
        //        string properties = GetPropertyNames(excludeKey: true);
        //        string query = $"INSERT INTO {tableName} ({columns}) VALUES ({properties})";

        //        var response = await _connection.ExecuteAsync(query, entity);
        //        return response > 0 ? true : false;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}
        public async Task<bool> Delete(T entity)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();
                string query = $"DELETE FROM {tableName} WHERE {keyColumn} = @{keyProperty}";
                rowsEffected = _connection.Execute(query, entity);
                return rowsEffected > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }

        }

        public async Task<bool> Delete(string Id, string columnName)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = GetTableName();
                string query = $"DELETE FROM {tableName} WHERE {columnName} = '{Id}'";
                rowsEffected = await _connection.ExecuteAsync(query);
                return rowsEffected > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<T>> GetAll()
        {
            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();
                string query = $"SELECT * FROM {tableName}";
                result = _connection.Query<T>(query);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<PaginatedResult<T>> GetAll(Expression<Func<T, bool>>[] filters, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                pageNumber = pageNumber <= 0 ? 1 : pageNumber;
                pageSize = pageSize <= 0 ? 10 : pageSize;

                string tableName = GetTableName();
                var sqlBuilder = new SqlBuilder();
                var converter = new ExpressionToSqlConverter();

                // Combine all provided filters into one
                var combinedFilter = ExpressionBuilder.CombineExpressions(filters);
                var whereClause = converter.TranslateFilterToWhereClause(combinedFilter);

                var totalCountQuery = $"SELECT COUNT(Id) FROM {tableName} /**where**/";

                var totalCountBuilder = new SqlBuilder();
                var selectorBuilder = new SqlBuilder();
                if (!string.IsNullOrEmpty(whereClause))
                {
                    totalCountBuilder.Where(whereClause);
                    selectorBuilder.Where(whereClause);
                }
                var totalCountTemplate = totalCountBuilder.AddTemplate(totalCountQuery);

                var parameters = converter.GetParameters();
                var offset = (pageNumber - 1) * pageSize;
                var selectorQuery = $"SELECT * FROM {tableName} /**where**/ ORDER BY (SELECT NULL) OFFSET {offset} ROWS FETCH NEXT {pageSize} ROWS ONLY";
                var selectorTemplate = selectorBuilder.AddTemplate(selectorQuery);

                var totalCount = _connection.ExecuteScalar<int>(totalCountTemplate.RawSql, parameters);
                var items = _connection.QueryAsync<T>(selectorTemplate.RawSql, parameters).Result;

                return new PaginatedResult<T>
                {
                    Items = items,
                    TotalCount = totalCount,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling(totalCount / (double)pageSize),
                    PageSize = pageSize
                };

            }
            catch (Exception)
            {

                throw;
            }
        }

        public IEnumerable<T> GetAllAsync(Expression<Func<T, bool>>[] filters)
        {
            try
            {

                string tableName = GetTableName();
                var converter = new ExpressionToSqlConverter();

                // Combine all provided filters into one
                var combinedFilter = ExpressionBuilder.CombineExpressions(filters);
                var whereClause = converter.TranslateFilterToWhereClause(combinedFilter);
                var selectorQuery = $"SELECT * FROM {tableName} /**where**/ ";
                var selectorBuilder = new SqlBuilder();
                if (!string.IsNullOrEmpty(whereClause))
                {
                    selectorBuilder.Where(whereClause);
                }
                var selectorTemplate = selectorBuilder.AddTemplate(selectorQuery);
                var parameters = converter.GetParameters();
                return _connection.Query<T>(selectorTemplate.RawSql, parameters);

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<bool> RecordExists(Expression<Func<T, bool>>[] filters)
        {
            try
            {
                string tableName = GetTableName();
                var converter = new ExpressionToSqlConverter();

                // Combine all provided filters into one
                var combinedFilter = ExpressionBuilder.CombineExpressions(filters);
                var whereClause = converter.TranslateFilterToWhereClause(combinedFilter);
                var selectorQuery = $"SELECT COUNT(1) FROM {tableName} /**where**/ ";
                var selectorBuilder = new SqlBuilder();
                selectorBuilder.Where(whereClause);
                var selectorTemplate = selectorBuilder.AddTemplate(selectorQuery);
                var parameters = converter.GetParameters();

                int count = _connection.ExecuteScalar<int>(selectorTemplate.RawSql, parameters);
                return count > 0;

            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<T> GetById(string Id)
        {
            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string query = $"SELECT * FROM {tableName} WHERE {keyColumn} = '{Id}'";
                result = await _connection.QueryAsync<T>(query);
                return result.FirstOrDefault();
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<IEnumerable<T>> GetByColoumn(string coloumnName, string coloumnValue)
        {
            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();
                string keyColumn = coloumnName;
                string query = $"SELECT * FROM {tableName} WHERE {keyColumn} = '{coloumnValue}'";

                result = await _connection.QueryAsync<T>(query);
                return result;
            }
            catch (Exception)
            {
                throw;
            }

        }
        public async Task<bool> Update(T entity)
        {
            int rowsEffected = 0;
            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                string keyProperty = GetKeyPropertyName();

                StringBuilder query = new StringBuilder();
                query.Append($"UPDATE {tableName} SET ");

                foreach (var property in GetProperties(true))
                {
                    var columnAttr = property.GetCustomAttribute<ColumnAttribute>();

                    string propertyName = property.Name;
                    //string columnName = columnAttr.Name;

                    query.Append($"{propertyName} = @{propertyName},");
                }

                query.Remove(query.Length - 1, 1);

                query.Append($" WHERE {keyColumn} = @{keyProperty}");

                rowsEffected = await _connection.ExecuteAsync(query.ToString(), entity);
                return rowsEffected > 0 ? true : false;
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateBulk(List<T> entities, List<string> changedColumns)
        {
            int batchSize = 1000;
            int totalRecords = entities.Count;
            int totalBatches = (int)Math.Ceiling((double)totalRecords / batchSize);
            int rowsAffected = 0;

            try
            {
                string tableName = GetTableName();
                string keyColumn = GetKeyColumnName();
                _connection.Open();

                using (var transaction = _connection.BeginTransaction())
                {
                    for (int i = 0; i < totalBatches; i++)
                    {
                        var batchEntities = entities.Skip(i * batchSize).Take(batchSize).ToList();
                        var queryBuilder = new StringBuilder();

                        for (int j = 0; j < batchEntities.Count; j++)
                        {
                            var entity = batchEntities[j];
                            var query = new StringBuilder();
                            query.Append($"UPDATE {tableName} SET ");

                            foreach (var propertyName in changedColumns)
                            {
                                var property = typeof(T).GetProperty(propertyName);
                                if (property != null)
                                {
                                    var propertyValue = property.GetValue(entity);
                                    if (property.PropertyType == typeof(string))
                                    {
                                        query.Append($"{propertyName} = '{propertyValue}'");
                                    }
                                    else if (property.PropertyType.BaseType.Name == "ValueType")
                                    {
                                        query.Append($"{propertyName} = '{propertyValue.ToString()}'");
                                    }
                                    else
                                    {
                                        query.Append($"{propertyName} = '{propertyValue}'");
                                    }
                                    query.Append(", ");
                                }
                            }

                            // Remove the last comma and space
                            if (query.Length > 0)
                            {
                                query.Remove(query.Length - 2, 2);
                            }

                            query.Append($" WHERE {keyColumn} = '{GetKeyPropertyValue(entity)}'");
                            queryBuilder.Append(query.ToString());

                            if (j < batchEntities.Count - 1)
                            {
                                queryBuilder.Append(";");
                            }
                        }

                        rowsAffected += await _connection.ExecuteAsync(queryBuilder.ToString(), transaction: transaction);
                    }

                    transaction.Commit();
                }
            }
            catch (Exception ex)
            {
                _connection.Close();
                throw; // You might want to log the exception here
            }
            finally
            {
                _connection.Close();
            }

            return rowsAffected == totalRecords;
        }


        /// <summary>
        /// Execute a stored procedure that does not return a result set.
        /// </summary>
        /// <param name="storedProcedure">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters for the stored procedure.</param>
        /// <returns>True if the operation was successful; otherwise, false.</returns>
        public async Task<bool> ExecuteStoredProcedureAsync(string? storedProcedureName = null, object parameters = null)
        {
            try
            {
                string storedProcedure = string.IsNullOrWhiteSpace(storedProcedureName) ? GetTableName() : storedProcedureName;
                await _connection.ExecuteAsync(storedProcedure, parameters, commandType: CommandType.StoredProcedure);
                return true;
            }
            catch (Exception)
            {
                // Log exception
                throw;
            }
        }

        /// <summary>
        /// Query a stored procedure that returns a result set.
        /// </summary>
        /// <typeparam name="TResult">The type of the result set.</typeparam>
        /// <param name="storedProcedure">The name of the stored procedure.</param>
        /// <param name="parameters">The parameters for the stored procedure.</param>
        /// <returns>The results from the stored procedure.</returns>
        public async Task<IEnumerable<T>> QueryStoredProcedureAsync(object parameters = null)
        {
            try
            {
                string storedProcedureName = GetTableName();
                var result = await _connection.QueryAsync<T>(storedProcedureName, parameters, commandType: CommandType.StoredProcedure);
                return result;
            }
            catch (Exception)
            {
                // Log exception
                throw;
            }
        }

        private object GetKeyPropertyValue(T entity)
        {
            var keyProperty = typeof(T).GetProperty(GetKeyPropertyName());
            return keyProperty.GetValue(entity);
        }

        private string GetTableName()
        {
            string tableName = "";
            var type = typeof(T);
            var tableAttr = type.GetCustomAttribute<TableAttribute>();
            if (tableAttr != null)
            {
                tableName = tableAttr.Name;
                return tableName;
            }

            return type.Name + "s";
        }

        public static string GetKeyColumnName()
        {
            PropertyInfo[] properties = typeof(T).GetProperties();

            foreach (PropertyInfo property in properties)
            {
                object[] keyAttributes = property.GetCustomAttributes(typeof(KeyAttribute), true);

                if (keyAttributes != null && keyAttributes.Length > 0)
                {
                    object[] columnAttributes = property.GetCustomAttributes(typeof(ColumnAttribute), true);

                    if (columnAttributes != null && columnAttributes.Length > 0)
                    {
                        ColumnAttribute columnAttribute = (ColumnAttribute)columnAttributes[0];
                        return columnAttribute.Name;
                    }
                    else
                    {
                        return property.Name;
                    }
                }
            }

            return null;
        }

        private string GetColumns(bool excludeKey = false)
        {
            var type = typeof(T);
            var columns = string.Join(", ", type.GetProperties()
                .Where(p => !excludeKey || (!p.IsDefined(typeof(KeyAttribute)) && !p.IsDefined(typeof(NotMappedAttribute))))
                .Select(p =>
                {
                    var columnAttr = p.GetCustomAttribute<ColumnAttribute>();
                    return columnAttr != null ? columnAttr.Name : p.Name;
                }));

            return columns;
        }

        protected string GetPropertyNames(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null && !p.IsDefined(typeof(NotMappedAttribute)));

            var values = string.Join(", ", properties.Select(p =>
            {
                return $"@{p.Name}";
            }));

            return values;
        }

        protected IEnumerable<PropertyInfo> GetProperties(bool excludeKey = false)
        {
            var properties = typeof(T).GetProperties()
                .Where(p => !excludeKey || p.GetCustomAttribute<KeyAttribute>() == null && !p.IsDefined(typeof(NotMappedAttribute)));

            return properties;
        }

        protected string GetKeyPropertyName()
        {
            var properties = typeof(T).GetProperties()
                .Where(p => p.GetCustomAttribute<KeyAttribute>() != null);

            if (properties.Any())
            {
                return properties.FirstOrDefault().Name;
            }

            return null;
        }

        public IEnumerable<T> ExecuteQuery(string query, object parameters = null)
        {
            return _connection.Query<T>(query, parameters);
        }

        /// <param name="filters">
        /// <summary>
        /// firstElement : column name
        /// secondElement : condition
        /// thirdElement : value
        /// fourthElement : value2
        /// </summary>
        /// </param>
        /// <returns></returns>
        public async Task<IEnumerable<T>> GetByColumnNames(Tuple<string, string, string, string>[] filters)
        {
            IEnumerable<T> result = null;
            try
            {
                string tableName = GetTableName();

                string filtersString = BuildDynamicFilters(filters);
                var query = BuildSelectStatement(filtersString);

                result = await _connection.QueryAsync<T>(query);
                return result;
            }
            catch (Exception) { throw; }

        }

        private string BuildDynamicFilters(Tuple<string, string, string, string>[] filters)
        {
            try
            {

                StringBuilder filtersBuilder = new StringBuilder();

                if (filters != null && filters.Length > 0)
                {
                    filtersBuilder.Append(" WHERE ");
                    for (int i = 0; i < filters.Length; i++)
                    {
                        string columnName = filters[i].Item1;
                        string condition = string.IsNullOrEmpty(filters[i].Item2) ? "EQ" : filters[i].Item2.ToUpper();
                        string columnValue = filters[i].Item3;
                        string columnValue2 = filters[i].Item4;

                        filtersBuilder.Append($"{columnName} {(condition == "EQ" ? "=" : condition)} ");

                        if (condition == "BETWEEN")
                        {
                            filtersBuilder.Append($"'{columnValue}' AND '{columnValue2}'");
                        }
                        else if (condition == "LIKE")
                        {
                            filtersBuilder.Append($"'{columnValue}'");
                        }
                        else if (condition == "IN")
                        {
                            string[] values = columnValue.Split(',');
                            string formattedValues = string.Join(",", values.Select(v => $"'{v.Trim()}'"));
                            filtersBuilder.Append($"({formattedValues})");
                        }
                        else
                        {
                            filtersBuilder.Append($"'{columnValue}'");
                        }

                        if (i < filters.Length - 1)
                        {
                            filtersBuilder.Append(" AND ");
                        }
                    }
                }

                return filtersBuilder.ToString();

            }
            catch (Exception)
            {

                throw;
            }
        }

        private string BuildSelectStatement(string filtersString)
        {
            string query = "";
            try
            {
                string tableName = GetTableName();

                string columns = GetColumns();
                query = $"SELECT {columns} FROM {tableName}{filtersString}";
                return query;
            }
            catch (Exception) { throw; }
        }

        
    }
}

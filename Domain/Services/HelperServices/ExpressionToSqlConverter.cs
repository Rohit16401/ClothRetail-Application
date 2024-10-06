using System.Linq.Expressions;
using System.Text;

namespace Domain.Services.HelperServices
{
    class ExpressionToSqlConverter
    {
        private readonly StringBuilder _sb = new StringBuilder();
        private readonly Dictionary<string, object> _parameters = new Dictionary<string, object>();
        private int _paramIndex = 0;

        public string TranslateFilterToWhereClause<T>(Expression<Func<T, bool>> filter)
        {
            if (filter == null)
                return string.Empty;

            var visitor = new ExpressionToSqlVisitor(_sb, _parameters, ref _paramIndex);
            visitor.Visit(filter.Body);
            return _sb.ToString();
        }

        public Dictionary<string, object> GetParameters()
        {
            return _parameters;
        }

        private class ExpressionToSqlVisitor : ExpressionVisitor
        {
            private readonly StringBuilder _sb;
            private readonly Dictionary<string, object> _parameters;
            private int _paramIndex;

            public ExpressionToSqlVisitor(StringBuilder sb, Dictionary<string, object> parameters, ref int paramIndex)
            {
                _sb = sb;
                _parameters = parameters;
                _paramIndex = paramIndex;
            }

            protected override Expression VisitBinary(BinaryExpression node)
            {
                _sb.Append("(");
                if (node.NodeType == ExpressionType.Add && node.Left.Type == typeof(string) && node.Right.Type == typeof(string))
                {
                    VisitConcatenation(node.Left, node.Right);
                }
                else
                {
                    Visit(node.Left);

                    switch (node.NodeType)
                    {
                        case ExpressionType.Equal:
                            if (IsNullConstant(node.Right))
                                _sb.Append(" IS NULL");
                            else
                            {
                                _sb.Append(" = ");
                                Visit(node.Right);
                            }
                            break;
                        case ExpressionType.NotEqual:
                            if (IsNullConstant(node.Right))
                                _sb.Append(" IS NOT NULL");
                            else
                            {
                                _sb.Append(" <> ");
                                Visit(node.Right);
                            }
                            break;
                        case ExpressionType.GreaterThan:
                            _sb.Append(" > ");
                            Visit(node.Right);
                            break;
                        case ExpressionType.GreaterThanOrEqual:
                            _sb.Append(" >= ");
                            Visit(node.Right);
                            break;
                        case ExpressionType.LessThan:
                            _sb.Append(" < ");
                            Visit(node.Right);
                            break;
                        case ExpressionType.LessThanOrEqual:
                            _sb.Append(" <= ");
                            Visit(node.Right);
                            break;
                        case ExpressionType.AndAlso:
                            _sb.Append(" AND ");
                            Visit(node.Right);
                            break;
                        case ExpressionType.And:
                            _sb.Append(" AND ");
                            Visit(node.Right);
                            break;
                        case ExpressionType.OrElse:
                            _sb.Append(" OR ");
                            Visit(node.Right);
                            break;
                        case ExpressionType.Modulo:
                            _sb.Append(" % ");
                            Visit(node.Right);
                            break;
                        default:
                            throw new NotSupportedException($"The binary operator '{node.NodeType}' is not supported");
                    }
                }
                _sb.Append(")");
                return node;
            }

            //protected override Expression VisitBinary(BinaryExpression node)
            //{
            //    _sb.Append("(");

            //    if (node.NodeType == ExpressionType.Add && node.Left.Type == typeof(string) && node.Right.Type == typeof(string))
            //    {
            //        VisitConcatenation(node.Left, node.Right);
            //    }
            //    else if (node.NodeType == ExpressionType.Equal || node.NodeType == ExpressionType.NotEqual)
            //    {
            //        // Handle string.IsNullOrEmpty checks
            //        if (node.Left is MethodCallExpression methodCall &&
            //            methodCall.Method.DeclaringType == typeof(string))
            //        {
            //            if (methodCall.Method.Name == "IsNullOrEmpty")
            //            {
            //                if (node.NodeType == ExpressionType.Equal)
            //                {
            //                    _sb.Append("(");
            //                    Visit(methodCall.Arguments[0]);
            //                    _sb.Append(" IS NULL OR ");
            //                    Visit(methodCall.Arguments[0]);
            //                    _sb.Append(" = '')");
            //                }
            //                else if (node.NodeType == ExpressionType.NotEqual)
            //                {
            //                    _sb.Append("(");
            //                    Visit(methodCall.Arguments[0]);
            //                    _sb.Append(" IS NOT NULL AND ");
            //                    Visit(methodCall.Arguments[0]);
            //                    _sb.Append(" <> '')");
            //                }
            //            }
            //        }
            //        else if (node.Right is MethodCallExpression rightMethodCall &&
            //                 rightMethodCall.Method.DeclaringType == typeof(string) &&
            //                 rightMethodCall.Method.Name == "IsNullOrEmpty")
            //        {
            //            if (node.NodeType == ExpressionType.Equal)
            //            {
            //                _sb.Append("(");
            //                Visit(node.Left);
            //                _sb.Append(" IS NULL OR ");
            //                Visit(node.Left);
            //                _sb.Append(" = '')");
            //            }
            //            else if (node.NodeType == ExpressionType.NotEqual)
            //            {
            //                _sb.Append("(");
            //                Visit(node.Left);
            //                _sb.Append(" IS NOT NULL AND ");
            //                Visit(node.Left);
            //                _sb.Append(" <> '')");
            //            }
            //        }
            //        else
            //        {
            //            Visit(node.Left);

            //            switch (node.NodeType)
            //            {
            //                case ExpressionType.Equal:
            //                    if (IsNullConstant(node.Right))
            //                        _sb.Append(" IS NULL");
            //                    else
            //                    {
            //                        _sb.Append(" = ");
            //                        Visit(node.Right);
            //                    }
            //                    break;
            //                case ExpressionType.NotEqual:
            //                    if (IsNullConstant(node.Right))
            //                        _sb.Append(" IS NOT NULL");
            //                    else
            //                    {
            //                        _sb.Append(" <> ");
            //                        Visit(node.Right);
            //                    }
            //                    break;
            //                //case ExpressionType.GreaterThan:
            //                //    _sb.Append(" > ");
            //                //    Visit(node.Right);
            //                //    break;
            //                //case ExpressionType.GreaterThanOrEqual:
            //                //    _sb.Append(" >= ");
            //                //    Visit(node.Right);
            //                //    break;
            //                //case ExpressionType.LessThan:
            //                //    _sb.Append(" < ");
            //                //    Visit(node.Right);
            //                //    break;
            //                //case ExpressionType.LessThanOrEqual:
            //                //    _sb.Append(" <= ");
            //                //    Visit(node.Right);
            //                //    break;
            //                //case ExpressionType.AndAlso:
            //                //    _sb.Append(" AND ");
            //                //    Visit(node.Right);
            //                //    break;
            //                //case ExpressionType.OrElse:
            //                //    _sb.Append(" OR ");
            //                //    Visit(node.Right);
            //                //    break;
            //                //case ExpressionType.Modulo:
            //                //    _sb.Append(" % ");
            //                //    Visit(node.Right);
            //                //    break;
            //                default:
            //                    throw new NotSupportedException($"The binary operator '{node.NodeType}' is not supported");
            //            }
            //        }
            //    }
            //    else
            //    {
            //        Visit(node.Left);

            //        switch (node.NodeType)
            //        {
            //            //case ExpressionType.Equal:
            //            //    if (IsNullConstant(node.Right))
            //            //        _sb.Append(" IS NULL");
            //            //    else
            //            //    {
            //            //        _sb.Append(" = ");
            //            //        Visit(node.Right);
            //            //    }
            //            //    break;
            //            //case ExpressionType.NotEqual:
            //            //    if (IsNullConstant(node.Right))
            //            //        _sb.Append(" IS NOT NULL");
            //            //    else
            //            //    {
            //            //        _sb.Append(" <> ");
            //            //        Visit(node.Right);
            //            //    }
            //            //    break;
            //            case ExpressionType.GreaterThan:
            //                _sb.Append(" > ");
            //                Visit(node.Right);
            //                break;
            //            case ExpressionType.GreaterThanOrEqual:
            //                _sb.Append(" >= ");
            //                Visit(node.Right);
            //                break;
            //            case ExpressionType.LessThan:
            //                _sb.Append(" < ");
            //                Visit(node.Right);
            //                break;
            //            case ExpressionType.LessThanOrEqual:
            //                _sb.Append(" <= ");
            //                Visit(node.Right);
            //                break;
            //            case ExpressionType.AndAlso:
            //                _sb.Append(" AND ");
            //                Visit(node.Right);
            //                break;
            //            case ExpressionType.OrElse:
            //                _sb.Append(" OR ");
            //                Visit(node.Right);
            //                break;
            //            case ExpressionType.Modulo:
            //                _sb.Append(" % ");
            //                Visit(node.Right);
            //                break;
            //            default:
            //                throw new NotSupportedException($"The binary operator '{node.NodeType}' is not supported");
            //        }
            //        _sb.Append(" ");
            //        _sb.Append(GetSqlOperator(node.NodeType));
            //        _sb.Append(" ");
            //        Visit(node.Right);
            //    }
            //    //else
            //    //{
            //    //    Visit(node.Left);
            //    //    _sb.Append(" ");
            //    //    _sb.Append(GetSqlOperator(node.NodeType));
            //    //    _sb.Append(" ");
            //    //    Visit(node.Right);
            //    //}

            //    _sb.Append(")");
            //    return node;
            //}


            //private string GetSqlOperator(ExpressionType expressionType)
            //{
            //    switch (expressionType)
            //    {
            //        case ExpressionType.Equal:
            //            return "=";
            //        case ExpressionType.NotEqual:
            //            return "<>";
            //        case ExpressionType.GreaterThan:
            //            return ">";
            //        case ExpressionType.GreaterThanOrEqual:
            //            return ">=";
            //        case ExpressionType.LessThan:
            //            return "<";
            //        case ExpressionType.LessThanOrEqual:
            //            return "<=";
            //        case ExpressionType.AndAlso:
            //            return "AND";
            //        case ExpressionType.And:
            //            return "AND";
            //        case ExpressionType.OrElse:
            //            return "OR";
            //        case ExpressionType.Modulo:
            //            return "%";
            //        default:
            //            throw new NotSupportedException($"The binary operator '{expressionType}' is not supported");
            //    }
            //}


            protected override Expression VisitUnary(UnaryExpression node)
            {
                if (node.NodeType == ExpressionType.Not)
                {
                    _sb.Append("NOT (");
                    Visit(node.Operand);
                    _sb.Append(")");
                    return node;
                }

                return base.VisitUnary(node);
            }

            protected override Expression VisitMember(MemberExpression node)
            {
                if (node.Expression != null && node.Expression.NodeType == ExpressionType.Parameter)
                {
                    if (node.Member.Name == "Length" && node.Expression.Type == typeof(string))
                    {
                        _sb.Append("LEN(");
                        Visit(node.Expression);
                        _sb.Append(")");
                    }
                    else
                    {
                        _sb.Append(node.Member.Name);
                    }
                    return node;
                }
                else
                {
                    VisitConstantOrVariable(node);
                    return node;
                }
            }

            protected override Expression VisitConstant(ConstantExpression node)
            {
                VisitConstantOrVariable(node);
                return node;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                // Append the parameter name (which typically represents a column or field in SQL)
                _sb.Append(node.Name);
                return node;
            }

            protected override Expression VisitMethodCall(MethodCallExpression node)
            {
                if (node.Method.DeclaringType == typeof(string))
                {
                    if (node.Method.Name == "Contains")
                    {
                        _sb.Append("(");
                        Visit(node.Object);
                        _sb.Append(" LIKE ");
                        VisitContainsArgument(node.Arguments[0]);
                        _sb.Append(")");
                        return node;
                    }
                    else if (node.Method.Name == "StartsWith")
                    {
                        _sb.Append("(");
                        Visit(node.Object);
                        _sb.Append(" LIKE ");
                        VisitStartsWithArgument(node.Arguments[0]);
                        _sb.Append(")");
                        return node;
                    }
                    else if (node.Method.Name == "EndsWith")
                    {
                        _sb.Append("(");
                        Visit(node.Object);
                        _sb.Append(" LIKE ");
                        VisitEndsWithArgument(node.Arguments[0]);
                        _sb.Append(")");
                        return node;
                    }
                    else if (node.Method.Name == "Format")
                    {
                        var formatString = (string)GetConstantOrVariableValue(node.Arguments[0]);
                        var args = node.Arguments.Skip(1).Select(GetConstantOrVariableValue).ToArray();
                        var formattedString = string.Format(formatString, args);
                        Visit(Expression.Constant(formattedString));
                        return node;
                    }
                }
                else if (node.Method.DeclaringType == typeof(Enumerable))
                {
                    if (node.Method.Name == "Contains")
                    {
                        VisitEnumerableContains(node);
                        return node;
                    }
                }

                throw new NotSupportedException($"Method calls are not supported in this context: {node.Method.Name}");
            }

            private void VisitEnumerableContains(MethodCallExpression node)
            {
                var collection = (IEnumerable<object>)GetConstantOrVariableValue(node.Arguments[0]);
                var item = node.Arguments[1];

                _sb.Append("(");
                Visit(item);
                _sb.Append(" IN (");

                bool first = true;
                foreach (var element in collection)
                {
                    if (!first)
                    {
                        _sb.Append(", ");
                    }
                    string paramName = $"@p{_paramIndex++}";
                    _parameters[paramName] = element;
                    _sb.Append(paramName);
                    first = false;
                }

                _sb.Append("))");
            }

            private object GetConstantOrVariableValue(Expression expression)
            {
                switch (expression.NodeType)
                {
                    case ExpressionType.Constant:
                        return ((ConstantExpression)expression).Value;
                    case ExpressionType.MemberAccess:
                        return GetMemberValue((MemberExpression)expression);
                    case ExpressionType.Call:
                        return EvaluateMethodCall((MethodCallExpression)expression);
                    case ExpressionType.Convert:
                        return GetConstantOrVariableValue(((UnaryExpression)expression).Operand);
                    case ExpressionType.Parameter:
                        var parameterExpression = (ParameterExpression)expression;
                        return parameterExpression.Name;
                    default:
                        throw new NotSupportedException("Only constant, member access, and method call expressions are supported in this context.");
                }
            }

            private void VisitContainsArgument(Expression expression)
            {
                string paramName = $"@p{_paramIndex++}";
                var value = GetConstantOrVariableValue(expression)?.ToString() ?? "NULL";
                if (value != "NULL" && !value.Contains('%'))
                    value = $"%{value}%";

                _parameters[paramName] = value;
                _sb.Append(paramName);
            }

            private void VisitStartsWithArgument(Expression expression)
            {
                string paramName = $"@p{_paramIndex++}";
                var value = GetConstantOrVariableValue(expression)?.ToString() ?? "NULL";
                if (value != "NULL" && !value.Contains('%'))
                    value = $"{value}%";

                _parameters[paramName] = value;
                _sb.Append(paramName);
            }

            private void VisitEndsWithArgument(Expression expression)
            {
                string paramName = $"@p{_paramIndex++}";
                var value = GetConstantOrVariableValue(expression)?.ToString() ?? "NULL";
                if (value != "NULL" && !value.Contains('%'))
                    value = $"%{value}";

                _parameters[paramName] = value;
                _sb.Append(paramName);
            }

            //private object GetMemberValue(MemberExpression memberExpression)
            //{
            //    if (memberExpression.Expression == null)
            //        throw new ArgumentNullException(nameof(memberExpression.Expression), "MemberExpression's Expression is null.");

            //    var objectMember = Expression.Convert(memberExpression.Expression, typeof(object));
            //    var memberValue = Expression.Lambda<Func<object>>(objectMember).Compile().DynamicInvoke();

            //    var memberInfo = memberExpression.Member;
            //    if (memberInfo is System.Reflection.PropertyInfo propertyInfo)
            //        return propertyInfo.GetValue(memberValue);
            //    if (memberInfo is System.Reflection.FieldInfo fieldInfo)
            //        return fieldInfo.GetValue(memberValue);

            //    throw new NotSupportedException($"Member '{memberInfo.Name}' is not supported.");
            //}

            //private object GetMemberValue(MemberExpression memberExpression)
            //{
            //    if (memberExpression == null)
            //        throw new ArgumentNullException(nameof(memberExpression), "MemberExpression cannot be null.");

            //    if (memberExpression.Expression == null)
            //        throw new ArgumentNullException(nameof(memberExpression.Expression), "MemberExpression's Expression is null.");

            //    // Convert the expression to the type of the member
            //    var objectMember = Expression.Convert(memberExpression.Expression, memberExpression.Expression.Type);
            //    var memberValue = Expression.Lambda<Func<object>>(objectMember).Compile().DynamicInvoke();

            //    if (memberValue == null)
            //        throw new InvalidOperationException("The evaluated value of the member expression is null.");

            //    var memberInfo = memberExpression.Member;
            //    if (memberInfo is System.Reflection.PropertyInfo propertyInfo)
            //        return propertyInfo.GetValue(memberValue);
            //    if (memberInfo is System.Reflection.FieldInfo fieldInfo)
            //        return fieldInfo.GetValue(memberValue);

            //    throw new NotSupportedException($"Member '{memberInfo.Name}' of type '{memberInfo.GetType().Name}' is not supported.");
            //}

            private object GetMemberValue(MemberExpression memberExpression)
            {
                if (memberExpression == null)
                    throw new ArgumentNullException(nameof(memberExpression), "MemberExpression cannot be null.");

                if (memberExpression.Expression == null)
                    throw new ArgumentNullException(nameof(memberExpression.Expression), "MemberExpression's Expression is null.");

                // Convert the expression to the correct type
                var objectMember = Expression.Convert(memberExpression.Expression, typeof(object));
                //var objectMember = Expression.Convert(memberExpression.Expression, memberExpression.Expression.Type);
                var lambda = Expression.Lambda<Func<object>>(objectMember);
                var compiledLambda = lambda.Compile();
                var memberValue = compiledLambda();

                if (memberValue == null)
                    throw new InvalidOperationException("The evaluated value of the member expression is null.");

                var memberInfo = memberExpression.Member;
                if (memberInfo is System.Reflection.PropertyInfo propertyInfo)
                    return propertyInfo.GetValue(memberValue);
                if (memberInfo is System.Reflection.FieldInfo fieldInfo)
                    return fieldInfo.GetValue(memberValue);

                throw new NotSupportedException($"Member '{memberInfo.Name}' of type '{memberInfo.GetType().Name}' is not supported.");
            }

            //private object GetMemberValue(MemberExpression memberExpression)
            //{
            //    if (memberExpression == null)
            //        throw new ArgumentNullException(nameof(memberExpression), "MemberExpression cannot be null.");

            //    if (memberExpression.Expression == null)
            //        throw new ArgumentNullException(nameof(memberExpression.Expression), "MemberExpression's Expression is null.");

            //    // Get the object representing the instance of the member
            //    var instanceExpression = memberExpression.Expression;
            //    var instanceLambda = Expression.Lambda<Func<object>>(Expression.Convert(instanceExpression, typeof(object)));
            //    var instanceFunc = instanceLambda.Compile();
            //    var instanceValue = instanceFunc();

            //    if (instanceValue == null)
            //        throw new InvalidOperationException("The instance value of the member expression is null.");

            //    // Get the member information and value
            //    var memberInfo = memberExpression.Member;
            //    if (memberInfo is System.Reflection.PropertyInfo propertyInfo)
            //        return propertyInfo.GetValue(instanceValue);
            //    if (memberInfo is System.Reflection.FieldInfo fieldInfo)
            //        return fieldInfo.GetValue(instanceValue);

            //    throw new NotSupportedException($"Member '{memberInfo.Name}' of type '{memberInfo.GetType().Name}' is not supported.");
            //}




            private object EvaluateMethodCall(MethodCallExpression methodCallExpression)
            {
                if (methodCallExpression.Method.DeclaringType == typeof(string) && methodCallExpression.Method.Name == "Format")
                {
                    var formatString = (string)GetConstantOrVariableValue(methodCallExpression.Arguments[0]);
                    var args = methodCallExpression.Arguments.Skip(1).Select(GetConstantOrVariableValue).ToArray();
                    return string.Format(formatString, args);
                }

                var arguments = methodCallExpression.Arguments.Select(GetConstantOrVariableValue).ToArray();
                var lambda = Expression.Lambda(methodCallExpression).Compile();
                return lambda.DynamicInvoke(arguments);
            }

            private void VisitConstantOrVariable(Expression node)
            {
                var value = GetConstantOrVariableValue(node);

                if (value == null)
                {
                    // Handle null values directly
                    _sb.Append("NULL");
                }
                else if (value is string || value.GetType().IsPrimitive)
                {
                    // Strings and primitives are handled as SQL parameters
                    string paramName = $"@p{_paramIndex++}";
                    _parameters[paramName] = value;
                    _sb.Append(paramName);
                }
                else
                {
                    // Handle other types as SQL parameters
                    string paramName = $"@p{_paramIndex++}";
                    _parameters[paramName] = value;
                    _sb.Append(paramName);
                }
            }

            private void VisitConcatenation(Expression left, Expression right)
            {
                if (left is BinaryExpression binaryLeft && binaryLeft.NodeType == ExpressionType.Add && left.Type == typeof(string) && right.Type == typeof(string))
                {
                    _sb.Append("CONCAT(");
                    VisitConcatenation(binaryLeft.Left, binaryLeft.Right);
                    _sb.Append(", ");
                    VisitWithParameters(right);
                    _sb.Append(")");
                }
                else
                {
                    _sb.Append("CONCAT(");
                    VisitWithParameters(left);
                    _sb.Append(", ");
                    VisitWithParameters(right);
                    _sb.Append(")");
                }
            }

            private void VisitWithParameters(Expression expression)
            {
                if (expression.NodeType == ExpressionType.Constant)
                {
                    var value = GetConstantOrVariableValue(expression);
                    string paramName = $"@p{_paramIndex++}";
                    _parameters[paramName] = value;
                    _sb.Append(paramName);
                }
                else if (expression.NodeType == ExpressionType.MemberAccess)
                {
                    var value = GetConstantOrVariableValue(expression);
                    string paramName = $"@p{_paramIndex++}";
                    _parameters[paramName] = value;
                    _sb.Append(paramName);
                }
                else if (expression.NodeType == ExpressionType.Parameter)
                {
                    // Keep parameter as is (column or field reference)
                    _sb.Append(((ParameterExpression)expression).Name);
                }
                else
                {
                    Visit(expression);
                }
            }

            private bool IsNullConstant(Expression expression)
            {
                return expression.NodeType == ExpressionType.Constant && ((ConstantExpression)expression).Value == null;
            }
        }
    }


    public static class ExpressionBuilder
    {
        public static Expression<Func<T, bool>> CombineExpressions<T>(params Expression<Func<T, bool>>[] expressions)
        {
            if (expressions == null || expressions.Length == 0)
                return null; // Return a default expression if no expressions are provided.

            var parameter = Expression.Parameter(typeof(T), "x");
            var body = expressions.Select(expr => ReplaceParameter(expr.Body, expr.Parameters[0], parameter))
                                  .Aggregate((accumulated, next) => Expression.AndAlso(accumulated, next));

            return Expression.Lambda<Func<T, bool>>(body, parameter);
        }

        private static Expression ReplaceParameter(Expression body, ParameterExpression oldParam, ParameterExpression newParam)
        {
            return new ParameterReplacer(oldParam, newParam).Visit(body);
        }

        private class ParameterReplacer : ExpressionVisitor
        {
            private readonly ParameterExpression _oldParam;
            private readonly ParameterExpression _newParam;

            public ParameterReplacer(ParameterExpression oldParam, ParameterExpression newParam)
            {
                _oldParam = oldParam;
                _newParam = newParam;
            }

            protected override Expression VisitParameter(ParameterExpression node)
            {
                return node == _oldParam ? _newParam : base.VisitParameter(node);
            }
        }
    }
}

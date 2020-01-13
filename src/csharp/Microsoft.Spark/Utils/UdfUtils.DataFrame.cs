// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Apache.Arrow;
using Microsoft.Data.Analysis;
using Microsoft.Spark.Sql;

namespace Microsoft.Spark.Utils
{
    using DataFrameDelegate = DataFrameWorkerFunction.ExecuteDelegate;

    /// <summary>
    /// UdfUtils provides UDF-related functions and enum.
    /// </summary>
    internal static class DataFrameUdfUtils
    {
        /// <summary>
        /// Mapping of supported types from .NET to org.apache.spark.sql.types.DataType in Scala.
        /// Refer to spark/sql/catalyst/src/main/scala/org/apache/spark/sql/types/DataType.scala
        /// for more information.
        /// </summary>
        private static readonly Dictionary<Type, string> s_returnTypes =
            new Dictionary<Type, string>
            {
                {typeof(string), "string"},
                {typeof(byte[]), "binary"},
                {typeof(bool), "boolean"},
                {typeof(decimal), "decimal(28,12)"},
                {typeof(double), "double"},
                {typeof(float), "float"},
                {typeof(byte), "byte"},
                {typeof(int), "integer"},
                {typeof(long), "long"},
                {typeof(short), "short"},

                // Arrow array types
                {typeof(BooleanArray), "boolean"},
                {typeof(UInt8Array), "byte"},
                {typeof(Int16Array), "short"},
                {typeof(Int32Array), "integer"},
                {typeof(Int64Array), "long"},
                {typeof(FloatArray), "float"},
                {typeof(DoubleArray), "double"},
                {typeof(StringArray), "string"},
                {typeof(BinaryArray), "binary"},

                {typeof(PrimitiveDataFrameColumn<bool>), "boolean"},
                {typeof(PrimitiveDataFrameColumn<byte>), "byte"},
                {typeof(PrimitiveDataFrameColumn<short>), "short"},
                {typeof(PrimitiveDataFrameColumn<int>), "integer"},
                {typeof(PrimitiveDataFrameColumn<long>), "long"},
                {typeof(PrimitiveDataFrameColumn<float>), "float"},
                {typeof(PrimitiveDataFrameColumn<double>), "double"},
                {typeof(ArrowStringDataFrameColumn), "string"},
            };

        /// <summary>
        /// Returns the return type of an UDF in JSON format. This value is used to
        /// create a org.apache.spark.sql.types.DataType object from JSON string.
        /// </summary>
        /// <param name="type">Return type of an UDF</param>
        /// <returns>JSON format of the return type</returns>
        internal static string GetReturnType(Type type)
        {
            if (s_returnTypes.TryGetValue(type, out string value))
            {
                return $@"""{value}""";
            }

            Type dictionaryType = type.ImplementsGenericTypeOf(typeof(IDictionary<,>));
            if (dictionaryType != null)
            {
                Type[] typeArguments = dictionaryType.GenericTypeArguments;
                Type keyType = typeArguments[0];
                Type valueType = typeArguments[1];
                return @"{""type"":""map"", " +
                    $@"""keyType"":{GetReturnType(keyType)}, " +
                    $@"""valueType"":{GetReturnType(valueType)}, " +
                    $@"""valueContainsNull"":{valueType.CanBeNull()}}}";
            }

            Type enumerableType = type.ImplementsGenericTypeOf(typeof(IEnumerable<>));
            if (enumerableType != null)
            {
                Type elementType = enumerableType.GenericTypeArguments[0];
                return @"{""type"":""array"", " +
                    $@"""elementType"":{GetReturnType(elementType)}, " +
                    $@"""containsNull"":{elementType.CanBeNull()}}}";
            }

            throw new ArgumentException($"{type.FullName} is not supported.");
        }

        internal static Delegate CreateVectorUdfWrapper<T, TResult>(Func<T, TResult> udf)
            where T : DataFrameColumn
            where TResult : DataFrameColumn
        {
            return (DataFrameDelegate)new DataFrameUdfWrapper<T, TResult>(udf).Execute;
        }

        internal static Delegate CreateVectorUdfWrapper<T1, T2, TResult>(Func<T1, T2, TResult> udf)
            where T1 : DataFrameColumn
            where T2 : DataFrameColumn
            where TResult : DataFrameColumn
        {
            return (DataFrameDelegate)new DataFrameUdfWrapper<T1, T2, TResult>(udf).Execute;
        }

        internal static Delegate CreateVectorUdfWrapper<T1, T2, T3, TResult>(
            Func<T1, T2, T3, TResult> udf)
            where T1 : DataFrameColumn
            where T2 : DataFrameColumn
            where T3 : DataFrameColumn
            where TResult : DataFrameColumn
        {
            return (DataFrameDelegate)new DataFrameUdfWrapper<T1, T2, T3, TResult>(udf).Execute;
        }

        internal static Delegate CreateVectorUdfWrapper<T1, T2, T3, T4, TResult>(
            Func<T1, T2, T3, T4, TResult> udf)
            where T1 : DataFrameColumn
            where T2 : DataFrameColumn
            where T3 : DataFrameColumn
            where T4 : DataFrameColumn
            where TResult : DataFrameColumn
        {
            return (DataFrameDelegate)
                new DataFrameUdfWrapper<T1, T2, T3, T4, TResult>(udf).Execute;
        }

        internal static Delegate CreateVectorUdfWrapper<T1, T2, T3, T4, T5, TResult>(
            Func<T1, T2, T3, T4, T5, TResult> udf)
            where T1 : DataFrameColumn
            where T2 : DataFrameColumn
            where T3 : DataFrameColumn
            where T4 : DataFrameColumn
            where T5 : DataFrameColumn
            where TResult : DataFrameColumn
        {
            return (DataFrameDelegate)
                new DataFrameUdfWrapper<T1, T2, T3, T4, T5, TResult>(udf).Execute;
        }

        internal static Delegate CreateVectorUdfWrapper<T1, T2, T3, T4, T5, T6, TResult>(
            Func<T1, T2, T3, T4, T5, T6, TResult> udf)
            where T1 : DataFrameColumn
            where T2 : DataFrameColumn
            where T3 : DataFrameColumn
            where T4 : DataFrameColumn
            where T5 : DataFrameColumn
            where T6 : DataFrameColumn
            where TResult : DataFrameColumn
        {
            return (DataFrameDelegate)
                new DataFrameUdfWrapper<
                    T1, T2, T3, T4, T5, T6, TResult>(udf).Execute;
        }

        internal static Delegate CreateVectorUdfWrapper<T1, T2, T3, T4, T5, T6, T7, TResult>(
            Func<T1, T2, T3, T4, T5, T6, T7, TResult> udf)
            where T1 : DataFrameColumn
            where T2 : DataFrameColumn
            where T3 : DataFrameColumn
            where T4 : DataFrameColumn
            where T5 : DataFrameColumn
            where T6 : DataFrameColumn
            where T7 : DataFrameColumn
            where TResult : DataFrameColumn
        {
            return (DataFrameDelegate)
                new DataFrameUdfWrapper<
                    T1, T2, T3, T4, T5, T6, T7, TResult>(udf).Execute;
        }

        internal static Delegate CreateVectorUdfWrapper<T1, T2, T3, T4, T5, T6, T7, T8, TResult>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, TResult> udf)
            where T1 : DataFrameColumn
            where T2 : DataFrameColumn
            where T3 : DataFrameColumn
            where T4 : DataFrameColumn
            where T5 : DataFrameColumn
            where T6 : DataFrameColumn
            where T7 : DataFrameColumn
            where T8 : DataFrameColumn
            where TResult : DataFrameColumn
        {
            return (DataFrameDelegate)
                new DataFrameUdfWrapper<
                    T1, T2, T3, T4, T5, T6, T7, T8, TResult>(udf).Execute;
        }

        internal static Delegate CreateVectorUdfWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult> udf)
            where T1 : DataFrameColumn
            where T2 : DataFrameColumn
            where T3 : DataFrameColumn
            where T4 : DataFrameColumn
            where T5 : DataFrameColumn
            where T6 : DataFrameColumn
            where T7 : DataFrameColumn
            where T8 : DataFrameColumn
            where T9 : DataFrameColumn
            where TResult : DataFrameColumn
        {
            return (DataFrameDelegate)
                new DataFrameUdfWrapper<
                    T1, T2, T3, T4, T5, T6, T7, T8, T9, TResult>(udf).Execute;
        }

        internal static Delegate CreateVectorUdfWrapper<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(
            Func<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult> udf)
            where T1 : DataFrameColumn
            where T2 : DataFrameColumn
            where T3 : DataFrameColumn
            where T4 : DataFrameColumn
            where T5 : DataFrameColumn
            where T6 : DataFrameColumn
            where T7 : DataFrameColumn
            where T8 : DataFrameColumn
            where T9 : DataFrameColumn
            where T10 : DataFrameColumn
            where TResult : DataFrameColumn
        {
            return (DataFrameDelegate)
                new DataFrameUdfWrapper<
                    T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, TResult>(udf).Execute;
        }
    }
}

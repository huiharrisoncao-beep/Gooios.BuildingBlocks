using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using System.Data;
using System.Data.Common;
using System.Linq.Expressions;
using System.Reflection;

namespace Gooios.BuildingBlocks.Infrastructure.Extension;

public class Parameter
{
    public string? Name { get; set; }

    public string? Value { get; set; }
}

public static class EntityFrameworkCoreExtension
{

    public static IList<T> SqlQuery<T>(this DbContext db, string sql, params Parameter[] parameters) where T : new()
    {
        var conn = db.Database.GetDbConnection();
        try
        {
            conn.Open();
            using (var command = conn.CreateCommand())
            {
                command.CommandText = sql;
                var ps = parameters.Select(o => new SqlParameter(o.Name, o.Value)).ToArray();
                command.Parameters.AddRange(ps);
                var propts = typeof(T).GetProperties();
                var rtnList = new List<T>();
                T model;
                object val;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        model = new T();
                        foreach (var l in propts)
                        {
                            val = reader[l.Name];
                            if (val == DBNull.Value)
                            {
                                l.SetValue(model, null);
                            }
                            else
                            {
                                l.SetValue(model, val);
                            }
                        }
                        rtnList.Add(model);
                    }
                }
                return rtnList;
            }
        }
        finally
        {
            conn.Close();
        }
    }

    private static DbCommand CreateCommand(DatabaseFacade facade, string sql, out DbConnection connection, params object[] parameters)
    {
        var conn = facade.GetDbConnection();
        connection = conn;
        conn.Open();
        var cmd = conn.CreateCommand();
        return cmd;
    }

    public static DataTable SqlQuery(this DatabaseFacade facade, string sql, params object[] parameters)
    {
        var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
        var reader = command.ExecuteReader();
        var dt = new DataTable();
        dt.Load(reader);
        reader.Close();
        conn.Close();
        return dt;
    }

    public static T QuerySingleVal<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : struct
    {
        var command = CreateCommand(facade, sql, out DbConnection conn, parameters);
        var val = command.ExecuteScalar();
        if (val == null) throw new EntryPointNotFoundException("The return value is null.");

        var obj = (T)val;

        conn.Close();
        return obj;
    }

    public static List<T> SqlQuery<T>(this DatabaseFacade facade, string sql, params object[] parameters) where T : class, new()
    {
        var dt = SqlQuery(facade, sql, parameters);
        return dt.ToList<T>();
    }

    public static List<T> ToList<T>(this DataTable dt) where T : class, new()
    {
        var propertyInfos = typeof(T).GetProperties();
        var list = new List<T>();
        foreach (DataRow row in dt.Rows)
        {
            var t = new T();
            foreach (PropertyInfo p in propertyInfos)
            {
                if (dt.Columns.IndexOf(p.Name) != -1 && row[p.Name] != DBNull.Value)
                    p.SetValue(t, row[p.Name], null);
            }
            list.Add(t);
        }
        return list;
    }

    public static IQueryable<T> OrderSort<T>(this IQueryable<T> sour, string SortExpression, bool? isAsc)
    {
        if (string.IsNullOrEmpty(SortExpression)) return sour;
        if (isAsc == null) isAsc = true;
        string SortDirection = string.Empty;
        if (isAsc == true)
            SortDirection = "OrderBy";
        else
            SortDirection = "OrderByDescending";
        ParameterExpression pe = Expression.Parameter(typeof(T), SortExpression);
        PropertyInfo? pi = typeof(T).GetProperty(SortExpression);
        if (pi == null) return sour;
        Type[] types = new Type[2];
        types[0] = typeof(T);
        types[1] = pi.PropertyType;
        Expression expr = Expression.Call(typeof(Queryable), SortDirection, types, sour.Expression, Expression.Lambda(Expression.Property(pe, SortExpression), pe));
        IQueryable<T> query = sour.Provider.CreateQuery<T>(expr);
        return query;
    }

}
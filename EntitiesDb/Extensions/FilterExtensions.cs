using System;

namespace EntitiesDb;

public static class FilterExtensions
{
    public static QueryFilter Any<TQueryable>(this TQueryable queryable, Type type)
        where TQueryable : IQueryable
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.AnyTypes.Add(type);
        return queryFilter;
    }

    public static QueryFilter Any<TQueryable>(this TQueryable queryable, params Type[] types)
        where TQueryable : IQueryable
    {
        var queryFilter = queryable.GetQueryFilter();
        foreach (var type in types) queryFilter.AnyTypes.Add(type);
        return queryFilter;
    }

    public static QueryFilter Any<TQueryable, T1>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.AnyTypes.Add(typeof(T1));
        return queryFilter;
    }

    public static QueryFilter Any<TQueryable, T1, T2>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.AnyTypes.Add(typeof(T1));
        queryFilter.AnyTypes.Add(typeof(T2));
        return queryFilter;
    }

    public static QueryFilter Any<TQueryable, T1, T2, T3>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.AnyTypes.Add(typeof(T1));
        queryFilter.AnyTypes.Add(typeof(T2));
        queryFilter.AnyTypes.Add(typeof(T3));
        return queryFilter;
    }

    public static QueryFilter Any<TQueryable, T1, T2, T3, T4>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.AnyTypes.Add(typeof(T1));
        queryFilter.AnyTypes.Add(typeof(T2));
        queryFilter.AnyTypes.Add(typeof(T3));
        queryFilter.AnyTypes.Add(typeof(T4));
        return queryFilter;
    }

    public static QueryFilter Any<TQueryable, T1, T2, T3, T4, T5>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.AnyTypes.Add(typeof(T1));
        queryFilter.AnyTypes.Add(typeof(T2));
        queryFilter.AnyTypes.Add(typeof(T3));
        queryFilter.AnyTypes.Add(typeof(T4));
        queryFilter.AnyTypes.Add(typeof(T5));
        return queryFilter;
    }

    public static QueryFilter Any<TQueryable, T1, T2, T3, T4, T5, T6>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.AnyTypes.Add(typeof(T1));
        queryFilter.AnyTypes.Add(typeof(T2));
        queryFilter.AnyTypes.Add(typeof(T3));
        queryFilter.AnyTypes.Add(typeof(T4));
        queryFilter.AnyTypes.Add(typeof(T5));
        queryFilter.AnyTypes.Add(typeof(T6));
        return queryFilter;
    }

    public static QueryFilter IncludeDisabled<TQueryable>(this TQueryable queryable)
        where TQueryable : IQueryable
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.NoTypes.Remove(typeof(Disabled));
        return queryFilter;
    }

    public static QueryFilter No<TQueryable>(this TQueryable queryable, Type type)
        where TQueryable : IQueryable
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.NoTypes.Add(type);
        return queryFilter;
    }

    public static QueryFilter No<TQueryable>(this TQueryable queryable, params Type[] types)
        where TQueryable : IQueryable
    {
        var queryFilter = queryable.GetQueryFilter();
        foreach (var type in types) queryFilter.NoTypes.Add(type);
        return queryFilter;
    }

    public static QueryFilter No<TQueryable, T1>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.NoTypes.Add(typeof(T1));
        return queryFilter;
    }

    public static QueryFilter No<TQueryable, T1, T2>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.NoTypes.Add(typeof(T1));
        queryFilter.NoTypes.Add(typeof(T2));
        return queryFilter;
    }

    public static QueryFilter No<TQueryable, T1, T2, T3>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.NoTypes.Add(typeof(T1));
        queryFilter.NoTypes.Add(typeof(T2));
        queryFilter.NoTypes.Add(typeof(T3));
        return queryFilter;
    }

    public static QueryFilter No<TQueryable, T1, T2, T3, T4>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.NoTypes.Add(typeof(T1));
        queryFilter.NoTypes.Add(typeof(T2));
        queryFilter.NoTypes.Add(typeof(T3));
        queryFilter.NoTypes.Add(typeof(T4));
        return queryFilter;
    }

    public static QueryFilter No<TQueryable, T1, T2, T3, T4, T5>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.NoTypes.Add(typeof(T1));
        queryFilter.NoTypes.Add(typeof(T2));
        queryFilter.NoTypes.Add(typeof(T3));
        queryFilter.NoTypes.Add(typeof(T4));
        queryFilter.NoTypes.Add(typeof(T5));
        return queryFilter;
    }

    public static QueryFilter No<TQueryable, T1, T2, T3, T4, T5, T6>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.NoTypes.Add(typeof(T1));
        queryFilter.NoTypes.Add(typeof(T2));
        queryFilter.NoTypes.Add(typeof(T3));
        queryFilter.NoTypes.Add(typeof(T4));
        queryFilter.NoTypes.Add(typeof(T5));
        queryFilter.NoTypes.Add(typeof(T6));
        return queryFilter;
    }

    public static QueryFilter Parallel<TQueryable>(this TQueryable queryable)
        where TQueryable : IQueryable
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.Parallel = true;
        return queryFilter;
    }

    public static QueryFilter With<TQueryable>(this TQueryable queryable, Type type)
        where TQueryable : IQueryable
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.WithTypes.Add(type);
        return queryFilter;
    }

    public static QueryFilter With<TQueryable>(this TQueryable queryable, params Type[] types)
        where TQueryable : IQueryable
    {
        var queryFilter = queryable.GetQueryFilter();
        foreach (var type in types) queryFilter.WithTypes.Add(type);
        return queryFilter;
    }

    public static QueryFilter With<TQueryable, T1>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.WithTypes.Add(typeof(T1));
        return queryFilter;
    }

    public static QueryFilter With<TQueryable, T1, T2>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.WithTypes.Add(typeof(T1));
        queryFilter.WithTypes.Add(typeof(T2));
        return queryFilter;
    }

    public static QueryFilter With<TQueryable, T1, T2, T3>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.WithTypes.Add(typeof(T1));
        queryFilter.WithTypes.Add(typeof(T2));
        queryFilter.WithTypes.Add(typeof(T3));
        return queryFilter;
    }

    public static QueryFilter With<TQueryable, T1, T2, T3, T4>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.WithTypes.Add(typeof(T1));
        queryFilter.WithTypes.Add(typeof(T2));
        queryFilter.WithTypes.Add(typeof(T3));
        queryFilter.WithTypes.Add(typeof(T4));
        return queryFilter;
    }

    public static QueryFilter With<TQueryable, T1, T2, T3, T4, T5>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.WithTypes.Add(typeof(T1));
        queryFilter.WithTypes.Add(typeof(T2));
        queryFilter.WithTypes.Add(typeof(T3));
        queryFilter.WithTypes.Add(typeof(T4));
        queryFilter.WithTypes.Add(typeof(T5));
        return queryFilter;
    }

    public static QueryFilter With<TQueryable, T1, T2, T3, T4, T5, T6>(this TQueryable queryable)
        where TQueryable : IQueryable
        where T1 : unmanaged
        where T2 : unmanaged
        where T3 : unmanaged
        where T4 : unmanaged
        where T5 : unmanaged
        where T6 : unmanaged
    {
        var queryFilter = queryable.GetQueryFilter();
        queryFilter.WithTypes.Add(typeof(T1));
        queryFilter.WithTypes.Add(typeof(T2));
        queryFilter.WithTypes.Add(typeof(T3));
        queryFilter.WithTypes.Add(typeof(T4));
        queryFilter.WithTypes.Add(typeof(T5));
        queryFilter.WithTypes.Add(typeof(T6));
        return queryFilter;
    }
}

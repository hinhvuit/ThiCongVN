using Microsoft.Extensions.DependencyInjection;
using System.Linq.Expressions;

namespace SqlSugar;

/// <summary>
/// ????SqlSugar 隞
/// </summary>
public partial class SqlSugarRepository : ISqlSugarRepository
{
    /// <summary>
    /// ?????
    /// </summary>
    private readonly IServiceProvider _serviceProvider;

    /// <summary>
    /// ???
    /// </summary>
    /// <param name="serviceProvider">?????/param>
    /// <param name="db"></param>
    public SqlSugarRepository(IServiceProvider serviceProvider, ISqlSugarClient db)
    {
        _serviceProvider = serviceProvider;
        var tenant = db.AsTenant();
        DynamicContext = Context = (SqlSugarClient)tenant;
        Ado = db.Ado;
    }

    /// <summary>
    /// ?唳摨?銝?
    /// </summary>
    public virtual ISqlSugarClient Context { get; }

    /// <summary>
    /// ?冽?桀?銝???
    /// </summary>
    public virtual dynamic DynamicContext { get; }

    /// <summary>
    /// ?? Ado 撖寡情
    /// </summary>
    public virtual IAdo Ado { get; }

    /// <summary>
    /// ?隞
    /// </summary>
    /// <typeparam name="TEntity">摰?蝐餃?</typeparam>
    /// <returns>隞</returns>
    public virtual ISqlSugarRepository<TEntity> Change<TEntity>()
        where TEntity : class, new()
    {
        return _serviceProvider.GetService<ISqlSugarRepository<TEntity>>()!;
    }
}

/// <summary>
/// SqlSugar 隞摰蝐?
/// </summary>
/// <typeparam name="TEntity"></typeparam>
public partial class SqlSugarRepository<TEntity> : ISqlSugarRepository<TEntity>
where TEntity : class, new()
{
    ///// <summary>
    ///// ????SqlSugar 隞
    ///// </summary>
    //private readonly ISqlSugarRepository _sqlSugarRepository;

    /// <summary>
    /// ???
    /// </summary>
    /// <param name="sqlSugarRepository"></param>
    //public SqlSugarRepository(ISqlSugarRepository sqlSugarRepository)
    //{
    //    _sqlSugarRepository = sqlSugarRepository;

    //    DynamicContext = Context = sqlSugarRepository.Context;

    //    // 霈曄蔭蝘靽⊥
    //    var entityType = typeof(TEntity);
    //    if (entityType.IsDefined(typeof(TenantAttribute), false))
    //    {
    //        var tenantAttribute = entityType.GetCustomAttribute<TenantAttribute>(false)!;
    //        Context.ChangeDatabase(tenantAttribute.configId);
    //    }

    //    Ado = sqlSugarRepository.Ado;
    //}

    public SqlSugarRepository(ISqlSugarClient context)
    {
        var tenant = context.AsTenant();
        DynamicContext = Context = tenant.GetConnectionScopeWithAttr<TEntity>();
        Ado = Context.Ado;
    }

    /// <summary>
    /// 摰???
    /// </summary>
    //public virtual ISugarQueryable<TEntity> Entities => Context.Queryable<TEntity>();
    public virtual ISugarQueryable<TEntity> Entities => Context.Queryable<TEntity>();

    /// <summary>
    /// ?唳摨?銝?
    /// </summary>
    public virtual ISqlSugarClient Context { get; }

    /// <summary>
    /// ?冽?桀?銝???
    /// </summary>
    public virtual dynamic DynamicContext { get; }

    /// <summary>
    /// ?? Ado 撖寡情
    /// </summary>
    public virtual IAdo Ado { get; }

    /// <summary>
    /// ?瑕??餅
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public int Count(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.Count(whereExpression);
    }

    /// <summary>
    /// ?瑕??餅
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public async Task<int> CountAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Entities.CountAsync(whereExpression);
    }

    /// <summary>
    /// 璉?交?血???
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public bool Any(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.Any(whereExpression);
    }

    /// <summary>
    /// 璉?交?血???
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public async Task<bool> AnyAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Entities.AnyAsync(whereExpression);
    }

    /// <summary>
    /// ??銝駁?瑕?摰?
    /// </summary>
    /// <param name="Id"></param>
    /// <returns></returns>
    public TEntity Single(dynamic Id)
    {
        return Entities.InSingle(Id);
    }

    /// <summary>
    /// ?瑕?銝銝芸?雿?
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public TEntity Single(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.Single(whereExpression);
    }

    /// <summary>
    /// ?瑕?銝銝芸?雿?
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public async Task<TEntity> SingleAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Entities.SingleAsync(whereExpression);
    }

    /// <summary>
    /// ?瑕?銝銝芸?雿?
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public TEntity FirstOrDefault(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.First(whereExpression);
    }

    /// <summary>
    /// ?瑕?銝銝芸?雿?
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public async Task<TEntity> FirstOrDefaultAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Entities.FirstAsync(whereExpression);
    }

    /// <summary>
    /// ?瑕??”
    /// </summary>
    /// <returns></returns>
    public List<TEntity> ToList()
    {
        return Entities.ToList();
    }

    /// <summary>
    /// ?瑕??”
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public List<TEntity> ToList(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Entities.Where(whereExpression).ToList();
    }

    /// <summary>
    /// ?瑕??”
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <param name="orderByExpression"></param>
    /// <param name="orderByType"></param>
    /// <returns></returns>
    public List<TEntity> ToList(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
    {
        return Entities.OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(whereExpression).ToList();
    }

    /// <summary>
    /// ?瑕??”
    /// </summary>
    /// <returns></returns>
    public async Task<List<TEntity>> ToListAsync()
    {
        return await Entities.ToListAsync();
    }

    /// <summary>
    /// ?瑕??”
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public async Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Entities.Where(whereExpression).ToListAsync();
    }

    /// <summary>
    /// ?瑕??”
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <param name="orderByExpression"></param>
    /// <param name="orderByType"></param>
    /// <returns></returns>
    public async Task<List<TEntity>> ToListAsync(Expression<Func<TEntity, bool>> whereExpression, Expression<Func<TEntity, object>> orderByExpression = null, OrderByType orderByType = OrderByType.Asc)
    {
        return await Entities.OrderByIF(orderByExpression != null, orderByExpression, orderByType).Where(whereExpression).ToListAsync();
    }

    /// <summary>
    /// ?啣?銝?∟扇敶?
    /// </summary>
    public virtual int Insert(TEntity entity)
    {
        return Context.Insertable(entity).ExecuteCommand();
    }

    /// <summary>
    /// ?啣?銝?∟扇敶??憓d
    /// </summary>
    public int InsertReturnIdentity(TEntity entity)
    {
        return Context.Insertable(entity).ExecuteReturnIdentity();
    }

    /// <summary>
    /// ?啣?銝?∟扇敶? 餈?摰?嚗????芸?隡??摰??嚗??舀?憓?銝??霈文潘?
    /// </summary>
    public bool InsertReturnIndentityIntoEntity(TEntity entity)
    {
        return Context.Insertable(entity).ExecuteCommandIdentityIntoEntity();
    }

    /// <summary>
    /// ?啣?憭霈啣?
    /// </summary>
    public virtual int Insert(params TEntity[] entities)
    {
        return Context.Insertable(entities).ExecuteCommand();
    }

    /// <summary>
    /// ?啣?憭霈啣?
    /// </summary>
    public virtual int Insert(IEnumerable<TEntity> entities)
    {
        return Context.Insertable(entities.ToArray()).ExecuteCommand();
    }

    /// <summary>
    /// ?啣?憭霈啣?, ?寥?餈?銝駁
    /// </summary>
    public virtual List<long> InsertReturnPkList(IEnumerable<TEntity> entities)
    {
        return Context.Insertable(entities.ToArray()).ExecuteReturnPkList<long>();
    }

    /// <summary>
    /// ?啣?銝?∟扇敶?
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<int> InsertAsync(TEntity entity)
    {
        return await Context.Insertable(entity).ExecuteCommandAsync();
    }

    /// <summary>
    /// ?啣?銝?∟扇敶??憓d
    /// </summary>
    public async Task<long> InsertReturnIdentityAsync(TEntity entity)
    {
        return await Context.Insertable(entity).ExecuteReturnBigIdentityAsync();
    }

    /// <summary>
    /// ?啣?銝?∟扇敶? 餈?摰?嚗????芸?隡??摰??嚗??舀?憓?銝??霈文潘?
    /// </summary>
    public async Task<bool> InsertReturnIndentityIntoEntityAsync(TEntity entity)
    {
        return await Context.Insertable(entity).ExecuteCommandIdentityIntoEntityAsync();
    }

    /// <summary>
    /// ?啣?憭霈啣?
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual async Task<int> InsertAsync(params TEntity[] entities)
    {
        return await Context.Insertable(entities).ExecuteCommandAsync();
    }

    /// <summary>
    /// ?啣?憭霈啣?
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual async Task<int> InsertAsync(IEnumerable<TEntity> entities)
    {
        return await Context.Insertable(entities.ToArray()).ExecuteCommandAsync();
    }

    /// <summary>
    /// ?啣?憭霈啣?, ?寥?餈?銝駁
    /// </summary>
    public virtual async Task<List<long>> InsertReturnPkListAsync(IEnumerable<TEntity> entities)
    {
        return await Context.Insertable(entities.ToArray()).ExecuteReturnPkListAsync<long>();
    }

    /// <summary>
    /// ?湔銝?∟扇敶?
    /// </summary>
    public virtual int Update(TEntity entity, bool ignoreAllNullColumns = false)
    {
        return Context.Updateable(entity).IgnoreColumns(ignoreAllNullColumns).ExecuteCommand();
    }

    /// <summary>
    /// ?湔憭霈啣?
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual int Update(params TEntity[] entities)
    {
        return Context.Updateable(entities).ExecuteCommand();
    }

    /// <summary>
    /// ?湔憭霈啣?
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual int Update(IEnumerable<TEntity> entities)
    {
        return Context.Updateable(entities.ToArray()).ExecuteCommand();
    }

    /// <summary>
    /// ?芸?銋隞嗆?啗扇敶?
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public int Update(Expression<Func<TEntity, bool>> columns, Expression<Func<TEntity, bool>> whereExpression)
    {
        return Context.Updateable<TEntity>().SetColumns(columns).Where(whereExpression).ExecuteCommand();
    }

    /// <summary>
    /// ?湔銝?∟扇敶?
    /// </summary>
    public virtual async Task<int> UpdateAsync(TEntity entity, bool ignoreAllNullColumns = false)
    {
        return await Context.Updateable(entity).IgnoreColumns(ignoreAllNullColumns).ExecuteCommandAsync();
    }

    /// <summary>
    /// ?湔憭霈啣?
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual async Task<int> UpdateAsync(params TEntity[] entities)
    {
        return await Context.Updateable(entities).ExecuteCommandAsync();
    }

    /// <summary>
    /// ?湔憭霈啣?
    /// </summary>
    /// <param name="entities"></param>
    /// <returns></returns>
    public virtual async Task<int> UpdateAsync(IEnumerable<TEntity> entities)
    {
        return await Context.Updateable(entities.ToArray()).ExecuteCommandAsync();
    }


    /// <summary>
    /// ?芸?銋隞嗆?啗扇敶?
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public virtual async Task<int> UpdateAsync(Expression<Func<TEntity, bool>> columns, Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Context.Updateable<TEntity>().SetColumns(columns).Where(whereExpression).ExecuteCommandAsync();
    }

    /// <summary>
    /// ?銝?∟扇敶?
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual int Delete(TEntity entity)
    {
        return Context.Deleteable(entity).ExecuteCommand();
    }

    /// <summary>
    /// ?銝?∟扇敶?
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual int Delete(object key)
    {
        return Context.Deleteable<TEntity>().In(key).ExecuteCommand();
    }

    /// <summary>
    /// ?憭霈啣?
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public virtual int Delete(params object[] keys)
    {
        return Context.Deleteable<TEntity>().In(keys).ExecuteCommand();
    }

    /// <summary>
    /// ?芸?銋隞嗅??方扇敶?
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public int Delete(Expression<Func<TEntity, bool>> whereExpression)
    {
        return Context.Deleteable<TEntity>().Where(whereExpression).ExecuteCommand();
    }

    /// <summary>
    /// ?銝?∟扇敶?
    /// </summary>
    /// <param name="entity"></param>
    /// <returns></returns>
    public virtual async Task<int> DeleteAsync(TEntity entity)
    {
        return await Context.Deleteable(entity).ExecuteCommandAsync();
    }

    /// <summary>
    /// ?銝?∟扇敶?
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    public virtual async Task<int> DeleteAsync<TKey>(TKey key)
    {
        return await Context.Deleteable<TEntity>().In(key).ExecuteCommandAsync();
    }

    /// <summary>
    /// ?憭霈啣?
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public virtual async Task<int> DeleteAsync<TKey>(TKey[] keys)
    {
        return await Context.Deleteable<TEntity>().In(keys).ExecuteCommandAsync();
    }

    /// <summary>
    /// ?憭霈啣?
    /// </summary>
    /// <param name="keys"></param>
    /// <returns></returns>
    public virtual async Task<int> DeleteAsync<TKey>(List<TKey> keys)
    {
        return await Context.Deleteable<TEntity>().In(keys).ExecuteCommandAsync();
    }

    /// <summary>
    /// ?芸?銋隞嗅??方扇敶?
    /// </summary>
    /// <param name="whereExpression"></param>
    /// <returns></returns>
    public async Task<int> DeleteAsync(Expression<Func<TEntity, bool>> whereExpression)
    {
        return await Context.Deleteable<TEntity>().Where(whereExpression).ExecuteCommandAsync();
    }

    /// <summary>
    /// ?寞銵刻噢撘霂Ｗ??∟扇敶?
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual ISugarQueryable<TEntity> Where(Expression<Func<TEntity, bool>> predicate)
    {
        return AsQueryable(predicate);
    }

    /// <summary>
    /// ?寞銵刻噢撘霂Ｗ??∟扇敶?
    /// </summary>
    /// <param name="condition"></param>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual ISugarQueryable<TEntity> Where(bool condition, Expression<Func<TEntity, bool>> predicate)
    {
        return AsQueryable().WhereIF(condition, predicate);
    }

    /// <summary>
    /// ?遣 sql 霂剖?亥砭????
    /// </summary>
    public ISugarQueryable<TEntity> SqlQueryable(string sql)
    {
        return Context.SqlQueryable<TEntity>(sql);
    }

    /// <summary>
    /// ?遣 sql 霂剖?亥砭????
    /// </summary>
    public ISugarQueryable<TEntity> SqlQueryable(string sql, object parameters)
    {
        return Context.SqlQueryable<TEntity>(sql).AddParameters(parameters);
    }

    /// <summary>
    /// ?遣 sql 霂剖?亥砭????
    /// </summary>
    public ISugarQueryable<TEntity> SqlQueryable(string sql, params SugarParameter[] parameters)
    {
        return Context.SqlQueryable<TEntity>(sql).AddParameters(parameters);
    }

    /// <summary>
    /// ?遣 sql 霂剖?亥砭????
    /// </summary>
    public ISugarQueryable<TEntity> SqlQueryable(string sql, List<SugarParameter> parameters)
    {
        return Context.SqlQueryable<TEntity>(sql).AddParameters(parameters);
    }

    /// <summary>
    /// ?遣?亥砭????
    /// </summary>
    public virtual ISugarQueryable<TEntity> AsQueryable()
    {
        return Entities;
    }

    /// <summary>
    /// ?遣?亥砭????
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual ISugarQueryable<TEntity> AsQueryable(Expression<Func<TEntity, bool>> predicate)
    {
        return Entities.Where(predicate);
    }

    /// <summary>
    /// ?湔餈??唳摨???
    /// </summary>
    /// <returns></returns>
    public virtual List<TEntity> AsEnumerable()
    {
        return AsQueryable().ToList();
    }

    /// <summary>
    /// ?湔餈??唳摨???
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual List<TEntity> AsEnumerable(Expression<Func<TEntity, bool>> predicate)
    {
        return AsQueryable(predicate).ToList();
    }

    /// <summary>
    /// ?湔餈??唳摨???
    /// </summary>
    /// <returns></returns>
    public virtual async Task<List<TEntity>> AsAsyncEnumerable()
    {
        return await AsQueryable().ToListAsync();
    }

    /// <summary>
    /// ?湔餈??唳摨???
    /// </summary>
    /// <param name="predicate"></param>
    /// <returns></returns>
    public virtual async Task<List<TEntity>> AsAsyncEnumerable(Expression<Func<TEntity, bool>> predicate)
    {
        return await AsQueryable(predicate).ToListAsync();
    }

    /// <summary>
    /// ?隞
    /// </summary>
    /// <typeparam name="TChangeEntity">摰?蝐餃?</typeparam>
    /// <returns>隞</returns>
    public virtual ISqlSugarRepository<TChangeEntity> Change<TChangeEntity>()
        where TChangeEntity : class, new()
    {
        //return _sqlSugarRepository.Change<TChangeEntity>();

        return AttcMN.Framework.App.GetService<ISqlSugarRepository<TEntity>>().Change<TChangeEntity>();
    }

    //public TRepository ChangeRepository<TRepository>() where TRepository : ISugarRepository
    //{
    //    Type typeFromHandle = typeof(TRepository);
    //    bool flag = typeFromHandle.GetConstructors().Any((ConstructorInfo z) => z.GetParameters().Any());
    //    object obj = null;
    //    if (flag)
    //    {
    //        object[] args = new string[1];
    //        obj = Activator.CreateInstance(typeFromHandle, args);
    //    }
    //    else
    //    {
    //        obj = Activator.CreateInstance(typeFromHandle);
    //    }

    //    TRepository result = (TRepository)obj;
    //    if (result.Context == null)
    //    {
    //        result.Context = Context;
    //    }

    //    return result;
    //}
}

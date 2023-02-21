using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using MongoDB.Driver;

namespace ET.Server
{
    [FriendOf(typeof(DBComponent))]
    public static partial class DBComponentSystem
    {

        public static async ETTask<T> QueryOne<T>(this DBComponent self, Expression<Func<T, bool>> filter, string collection = null) where T : Entity
        {
            using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, RandomGenerator.RandInt64() % DBComponent.TaskCount))
            {
                IAsyncCursor<T> cursor = await self.GetCollection<T>(collection).FindAsync(filter);

                return await cursor.FirstOrDefaultAsync();
            }
        }
		public static async ETTask<List<T>> Query<T>(this DBComponent self, Expression<Func<T, bool>> filter,
	int limit, string collection = null) where T : Entity
		{
			using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, RandomGenerator.RandInt64() % DBComponent.TaskCount))
			{
				IAsyncCursor<T> cursor = await self.GetCollection<T>(collection).FindAsync(filter, new FindOptions<T>() { Limit = limit });
				return await cursor.ToListAsync();
			}
		}
		public static async ETTask<List<string>> GetCollectionNames(this DBComponent self)
		{
			using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, RandomGenerator.RandInt64() % DBComponent.TaskCount))
			{
				IAsyncCursor<string> cursor = await self.database.ListCollectionNamesAsync();

				return await cursor.ToListAsync();
			}
		}

		public static async ETTask<bool> HasCollection<T>(this DBComponent self, string collection = null)
		{
			var _name = await self.GetCollectionNames();
			return _name.Contains(collection ?? typeof(T).Name);
		}

		public static async ETTask<long> QueryCount<T>(this DBComponent self, Expression<Func<T, bool>> filter, string collection = null) where T : Entity
		{
			using (await CoroutineLockComponent.Instance.Wait(CoroutineLockType.DB, RandomGenerator.RandInt64() % DBComponent.TaskCount))
			{
				return await self.GetCollection<T>(collection).CountDocumentsAsync(filter);
			}
		}
	}
}
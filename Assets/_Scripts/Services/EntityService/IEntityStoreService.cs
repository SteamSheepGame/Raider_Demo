using System.Collections.Generic;

namespace Demo.Core
{
    public interface IEntityStoreService : IService
    {
        public IEntity GetEntity(string id);
        public void PushEntity(IEntity entity);
        public void HandleImportData(DataImporter importer);
        public IEnumerable<IEntity> GetAllEntities();
    }
}
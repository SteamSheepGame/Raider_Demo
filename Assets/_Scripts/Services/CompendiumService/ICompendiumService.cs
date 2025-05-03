namespace Demo.Core
{
    /// <summary>
    /// 用于从文件夹中加载所有数据的类的接口
    /// </summary>
    public interface ICompendiumService : IService
    {
        /// <summary>
        /// 将外部数据导入到Compendium中
        /// </summary>
        /// <param name="compendium">存储数据的单例</param>
        public void PopulateCompendium(Compendium compendium);
    }
}
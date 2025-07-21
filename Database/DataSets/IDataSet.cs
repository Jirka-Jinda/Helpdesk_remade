namespace Database.DataSets;

internal interface IDataSet
{
    public Task Populate(IServiceProvider serviceProvider);
}

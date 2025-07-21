namespace Database.Data;

internal interface IDataSet
{
    public Task Populate(IServiceProvider serviceProvider);
}

namespace Database.Data;

public interface IDataSet
{
    public Task Populate(IServiceProvider serviceProvider);
}

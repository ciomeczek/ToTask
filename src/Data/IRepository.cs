namespace ToTask.Data;

public interface IRepository<T>
{
    Task<IEnumerable<T>> GetAll();
    Task<T> GetById(int id);
    Task<T> Add(T obj);
    Task<T> Update(T obj);
    Task<bool> Delete(int id);
}
using Application.Helpers.Concrete;

namespace Application.Helpers.Abstract
{
    public interface IEnumHelper
    {
        List<KeyValuePair<int, string>> GetValuesAndNames<T>() where T : Enum;
        List<T> GetList<T>() where T : Enum;
    }
}

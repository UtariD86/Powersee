using Application.Helpers.Concrete;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Core.Helpers.Abstract
{
    public interface IEnumHelper
    {
        List<KeyValuePair<int, string>> GetValuesAndNames<T>() where T : Enum;
        List<T> GetList<T>() where T : Enum;

    }
}

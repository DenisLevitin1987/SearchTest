using System.Collections.Generic;
using SearchEngine.Models;

namespace SearchEngine.Storage
{
    public interface ILaptopDataProvider
    {
        IReadOnlyDictionary<int, Laptop> GetLaptops();

        IReadOnlyDictionary<int, Laptop> GetEquals(string equal);

        IReadOnlyDictionary<int, Laptop> GetNotEquals(string equal);
    }
}
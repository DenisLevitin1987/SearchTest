using System.Collections.Generic;
using SearchEngine.Models;

namespace SearchEngine.Storage
{
    public interface ILaptopDataProvider
    {
        IReadOnlyDictionary<int, Laptop> GetLaptops();

        IReadOnlyCollection<int> GetIdsByEqualString(string equal);
    }
}
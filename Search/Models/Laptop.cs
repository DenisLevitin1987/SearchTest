using System;
using System.Collections.Generic;
using System.Text;

namespace Search.Models
{
    /// <summary>
    /// Ноктбук
    /// </summary>
    public class Laptop
    {
        /// <summary>
        /// Ид
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Полная название
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Аттрибуты из названия
        /// </summary>
        public string[] Attributes { get; set; }
    }
}

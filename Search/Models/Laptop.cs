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
        /// Производитель
        /// </summary>
        public string Producer { get; set; }

        /// <summary>
        /// Серия модели
        /// </summary>
        public string Serial { get; set; }

        /// <summary>
        /// Номер модели
        /// </summary>
        public string ModelNumber { get; set; }

        /// <summary>
        /// Цвет
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Код модели
        /// </summary>
        public string ModelCode { get; set; }

        /// <summary>
        /// Размер экрана
        /// </summary>
        public decimal ScreenSize { get; set; }
    }
}

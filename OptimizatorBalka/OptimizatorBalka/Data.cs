using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OptimizatorBalka
{
   static class Data
   {
        public static string info = ""; // уведомление
        public static Dictionary<string, string> Path = new Dictionary<string, string>(); //  пути результрующих файлов
        public static Dictionary<int, string> K = new Dictionary<int, string>(); // конечные элементы
        public static Dictionary<int, string> Op = new Dictionary<int, string>(); // значения точек опор
        public static Dictionary<int, string> M = new Dictionary<int, string>(); // моменты
        public static Dictionary<int, string> FX = new Dictionary<int, string>(); // силы по X
        public static Dictionary<int, string> FY = new Dictionary<int, string>(); // силы по Y
        public static Dictionary<int, string> Q = new Dictionary<int, string>(); // нагрузки

        public static Dictionary<int, string> Shapes = new Dictionary<int, string>(5) // возможные формы балки
        {
            {1, "Круглая"},
            {2, "Прямоугольная"},
            {3, "Тонкостенное кольцо"},
            {4, "Тонкостенный полый квадрат"},
            {5, "Двутавровый профиль"},
        };
        public static int count; // кол-во ключевых точек
   }
}

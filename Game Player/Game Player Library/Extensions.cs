using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Game_Player
{
    public static class Extensions
    {
        public static int MinMax(this int i, int min, int max)
        {
            return Math.Max(Math.Min(i, max), min);
        }

        public static double MinMax(this double i, double min, double max)
        {
            return Math.Max(Math.Min(i, max), min);
        }

        public static bool Includes(this Array array, object item)
        {
            return Array.IndexOf(array, item) != -1;
        }

        public static T[] Plus<T>(this T[] array, T item)
        {
            Array.Resize<T>(ref array, array.Length + 1);
            array[array.Length - 1] = item;

            return array;
        }

        public static T[] Minus<T>(this T[] array, T item)
        {
            int index = Array.IndexOf(array, item);

            if (index != -1)
            {
                for (int i = index; i < array.Length - 1; i++)
                    array[i] = array[i + 1];
                Array.Resize<T>(ref array, array.Length - 1);
            }

            return array;
        }

        public static void Sort(this int[] array)
        {
            bool swapped;

            do
            {
                swapped = false;
                for (int i = 0; i < array.Length - 1; i++)
                {
                    if (array[i] > array[i + 1])
                    {
                        int temp = array[i];
                        array[i] = array[i + 1];
                        array[i + 1] = temp;

                        swapped = true;
                    }
                }
            } while (swapped);
        }
    }
}

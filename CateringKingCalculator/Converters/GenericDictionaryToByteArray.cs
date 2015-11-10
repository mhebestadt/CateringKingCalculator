using System;
using System.Collections.Generic;
using Windows.UI.Xaml.Data;

namespace CateringKingCalculator.Converters
{
    public class GenericDictionaryToByteArray : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            if (value == null)
                throw new ArgumentNullException();

            byte[] menuItemsWithWeight = { };

            int dictSize = ((Dictionary<int, int>)value).Count;
            int[,] array = new int[dictSize, 2];

            int count = 0;
            foreach (var item in ((Dictionary<int, int>)value))
            {
                array[count, 0] = item.Key;
                array[count, 1] = item.Value;

                count++;
            }

            menuItemsWithWeight = ToByteArray(array);            

            return menuItemsWithWeight;
        }

        public object Convert(object value)
        {
            return this.Convert(value, null, null, "");
        }

        public object ConvertBack(object value)
        {
            return this.ConvertBack(value, null, null, "");
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            if (value == null)
                throw new ArgumentNullException();

            Dictionary<int, int> dictionary = new Dictionary<int, int>();

            int[,] array = ToIntArray((byte[])value);
            int length = array.GetLength(0);

            if (length > 0)
            {
                for (int i = 0; i < length; i++)
                {
                    dictionary.Add(array[i, 0], array[i, 1]);
                }
            }

            return dictionary;
        }

        private byte[] ToByteArray(int[,] nmbs)
        {
            byte[] nmbsBytes = new byte[nmbs.GetLength(0) * nmbs.GetLength(1) * 4];
            int k = 0;

            for (int i = 0; i < nmbs.GetLength(0); i++)
            {
                for (int j = 0; j < nmbs.GetLength(1); j++)
                {
                    byte[] array = BitConverter.GetBytes(nmbs[i, j]);
                    for (int m = 0; m < array.Length; m++)
                    {
                        nmbsBytes[k++] = array[m];
                    }
                }
            }

            return nmbsBytes;
        }

        private int[,] ToIntArray(byte[] nmbsBytes)
        {
            int[,] nmbs = new int[nmbsBytes.Length / 4 / 2, 2];
            int k = 0;
            for (int i = 0; i < nmbs.GetLength(0); i++)
            {
                for (int j = 0; j < nmbs.GetLength(1); j++)
                {
                    nmbs[i, j] = BitConverter.ToInt16(nmbsBytes, k);
                    k += 4;
                }
            }
            return nmbs;
        }
    }
}

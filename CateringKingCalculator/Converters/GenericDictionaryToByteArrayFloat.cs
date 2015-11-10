using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CateringKingCalculator.Converters
{
    public class GenericDictionaryToByteArrayFloat
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            byte[] menuItemsWithWeight = { };

            if (value != null)
            {
                int dictSize = ((Dictionary<float, float>)value).Count;
                float[,] array = new float[dictSize, 2];

                int count = 0;
                foreach (var item in ((Dictionary<float, float>)value))
                {

                    array[count, 0] = item.Key;
                    array[count, 1] = item.Value;

                    count++;
                }

                menuItemsWithWeight = ToByteArray(array);
            }

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
            if (value == null) { return new Dictionary<float, float>(); }

            Dictionary<float, float> dictionary = new Dictionary<float, float>();

            float[,] array = ToFloatArray((byte[])value);
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

        private byte[] ToByteArray(float[,] nmbs)
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

        private float[,] ToFloatArray(byte[] nmbsBytes)
        {
            float[,] nmbs = new float[nmbsBytes.Length / 4 / 2, 2];
            int k = 0;

            for (int i = 0; i < nmbs.GetLength(0); i++)
            {
                for (int j = 0; j < nmbs.GetLength(1); j++)
                {
                    nmbs[i, j] = BitConverter.ToSingle(nmbsBytes, k);
                    k += 4;
                }
            }

            return nmbs;
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

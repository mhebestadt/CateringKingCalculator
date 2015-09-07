using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Windows.UI.Xaml.Data;

namespace hebestadt.CateringKingCalculator.Converters
{
    public class GenericListToByteArray : IValueConverter
    {
        public object Convert(object value, Type type, object parameter, string language)
        {
            return ((List<int>)value).SelectMany(BitConverter.GetBytes).ToArray();
        }

        public object ConvertBack(object value, Type type, object parameter, string language)
        {
            if (value != null)
                return ToListOf((byte[])value, BitConverter.ToInt32);
            else
                return null;
        }

        private static List<T> ToListOf<T>(byte[] array, Func<byte[], int, T> bitConverter)
        {
            var size = Marshal.SizeOf<T>();
            return Enumerable.Range(0, array.Length / size)
                             .Select(i => bitConverter(array, i * size))
                             .ToList();
        }
    }
    
 }

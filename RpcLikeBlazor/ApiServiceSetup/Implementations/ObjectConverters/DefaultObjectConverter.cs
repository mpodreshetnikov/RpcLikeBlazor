using Newtonsoft.Json;
using RpcLikeBlazor.ApiServiceSetup.Abstractions;
using RpcLikeBlazor.Helpers;
using System;
using System.Globalization;

namespace RpcLikeBlazor.ApiServiceSetup.Implementations.ObjectConverters
{
    /// <summary>
    /// Default object converter implementation.
    /// </summary>
    public class DefaultObjectConverter : IObjectConverter
    {
        /// <inheritdoc/>
        public TOut ConvertToObject<TOut>(string stringValue)
        {
            var returnType = typeof(TOut);

            if (stringValue == null)
            {
                return default;
            }
            else if (returnType == typeof(DateTime))
            {
                object value = ParseDateTime(stringValue);
                return (TOut)value;
            }
            else if (returnType.IsEnum)
            {
                var value = Enum.Parse(returnType, stringValue);
                return (TOut)value;
            }
            else if (TypeHelpers.IsSimpleType(returnType))
            {
                return (TOut)Convert.ChangeType(stringValue, returnType);
            }
            return JsonConvert.DeserializeObject<TOut>(stringValue);
        }

        /// <inheritdoc/>
        public string ConvertToString(object objectValue)
        {
            if (TypeHelpers.IsSimpleType(objectValue.GetType()))
            {
                return objectValue.ToString();
            }
            else
            {
                return JsonConvert.SerializeObject(objectValue);
            }
        }

        /// <summary>
        /// Parser for default format: 'yyyy-MM-ddTHH:mm:ss.fffffff+zzz'.
        /// </summary>
        private static DateTime ParseDateTime(string datetime)
        {
            int iter = 1;
            var year = int.Parse(datetime[iter..(iter += 4)]);
            var month = int.Parse(datetime[++iter..(iter += 2)]);
            var day = int.Parse(datetime[++iter..(iter += 2)]);
            var hours = int.Parse(datetime[++iter..(iter += 2)]);
            var minutes = int.Parse(datetime[++iter..(iter += 2)]);
            var seconds = int.Parse(datetime[++iter..(iter += 2)]);
            var milliseconds = int.Parse(datetime[++iter..(iter += 3)]);

            var zpos = datetime.LastIndexOfAny(new[] { '+', '-' });
            var submilliseconds = double.Parse("0." + datetime[iter..zpos], new CultureInfo("en-US"));

            // Timezone value.
            bool isNegative = datetime[zpos] == '-';
            var zHours = int.Parse(datetime[++zpos..(zpos += 2)]);
            var zMinutes = int.Parse(datetime[++zpos..(zpos += 2)]);
            var totalZMinutes = (isNegative ? -1 : 1) * (zHours * 60 + zMinutes);

            return new DateTime(year, month, day, hours, minutes, seconds, milliseconds, DateTimeKind.Utc)
                .AddMilliseconds(submilliseconds)
                // Include timezone info.
                .AddMinutes(-totalZMinutes)
                .ToLocalTime();
        }
    }
}

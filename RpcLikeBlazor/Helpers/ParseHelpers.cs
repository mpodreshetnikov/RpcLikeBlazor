using System;
using System.Globalization;

namespace RpcLikeBlazor.Helpers
{
    internal static class ParseHelpers
    {
        /// <summary>
        /// Parser for format: yyyy-MM-ddTHH:mm:ss.fffffff+zzz.
        /// </summary>
        public static DateTime ParseDateTime(string datetime)
        {
            int iter = 1;
            var year = int.Parse(datetime[iter..(iter += 4)]);
            var month = int.Parse(datetime[++iter..(iter += 2)]);
            var day = int.Parse(datetime[++iter..(iter += 2)]);
            var hours = int.Parse(datetime[++iter..(iter += 2)]);
            var minutes = int.Parse(datetime[++iter..(iter += 2)]);
            var seconds = int.Parse(datetime[++iter..(iter += 2)]);
            var milliseconds = int.Parse(datetime[++iter..(iter += 3)]);

            var pos = datetime.LastIndexOfAny(new[] { '+', '-' });
            var submilliseconds = double.Parse("0." + datetime[iter..pos], new CultureInfo("en-US"));

            return new DateTime(year, month, day, hours, minutes, seconds, milliseconds)
                .AddMilliseconds(submilliseconds);
        }
    }
}

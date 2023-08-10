
namespace MyFlix.Catalog.EndToEndTest.Extensions.DataTime
{
    internal static class DateTimeExtensions
    {
        public static System.DateTime TrimMillisseconds(
        this System.DateTime dateTime
    )
        {
            return new System.DateTime(
                dateTime.Year,
                dateTime.Month,
                dateTime.Day,
                dateTime.Hour,
                dateTime.Minute,
                dateTime.Second,
                0,
                dateTime.Kind
            );
        }
    }
}

namespace RpcLikeBlazor.Helpers
{
    internal static class ApiInterfaceConventionsHelpers
    {
        public static string GetRouteByInterfaceName(string name)
        {
            if (name.StartsWith('I'))
            {
                name = name.Remove(0, 1);
            }
            if (name.EndsWith("Api"))
            {
                name = name[0..^3];
            }
            return name;
        }
    }
}

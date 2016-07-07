namespace ResilientClient.CSharp.Messages
{
    class GetData
    {
        public string GetPath { get; }

        private GetData(string getPath)
        {
            GetPath = getPath;
        }

        public static GetData Create(string getPath)
        {
            return new GetData(getPath);
        }

        public static GetData AlwaysWorks()
        {
            return Create("/alwaysWorks");
        }

        public static GetData AlwaysFails()
        {
            return Create("/alwaysFails");
        }

        public static GetData RandomlyFails()
        {
            return Create("/randomlyFails");
            }

        public static GetData TakesForever()
        {
            return Create("/takesForever");
        }
    }

}

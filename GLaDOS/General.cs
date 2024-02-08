using GLaDOS.ConfigurationTypes;
using GLaDOS.Service;


namespace GLaDOS
{
    public static class General
    {
        public static readonly User User = new();
        public static class Configuration
        {
            private static readonly string dir = @"C:\Users\LaL\source\repos\ConfigY\ConfigY\bin\Debug\net8.0\configuration";
            private static readonly string tokensFile = "tokens.cfg";
            public static Tokens Tokens { get; private set; }
            public static readonly int AnimationMsD = 50;

            static Configuration()
            {
                Tokens = (Serializer.Deserialize<Tokens>(File.ReadAllText(Path.Combine(dir, tokensFile))));
            }
        }
    }
}
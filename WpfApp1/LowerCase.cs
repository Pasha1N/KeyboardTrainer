namespace KeyboardTrainer
{
    internal static class LowerCase
    {
        private static bool lowerCase = true;

        public static bool Register
        {
            get => lowerCase;
            set => lowerCase = value;
        }
    }
}
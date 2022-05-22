namespace WordGame2.Other
{
    internal class TemporalConsole
    {
        private static string _inputLine;
        private static readonly AutoResetEvent GetInputEvent;
        private static readonly AutoResetEvent GottenInputEvent;
        private static readonly Thread _inputThread;

        static TemporalConsole()
        {
            _inputLine = string.Empty;
            GetInputEvent = new AutoResetEvent(false);
            GottenInputEvent = new AutoResetEvent(false);
            _inputThread = new Thread(WaitReadLine) { IsBackground = true };
            _inputThread.Start();
        }

        private static void WaitReadLine()
        {
            while (true)
            {
                GetInputEvent.WaitOne();
                _inputLine = Console.ReadLine() ?? "";
                GottenInputEvent.Set();
            }
        }

        public static string ReadLine(int timeOutMillisecs = Timeout.Infinite)
        {
            GetInputEvent.Set();
            bool success = GottenInputEvent.WaitOne(timeOutMillisecs);

            if (success)
            {
                return _inputLine;
            }

            throw new TimeoutException("Time is over.");
        }
    }
}

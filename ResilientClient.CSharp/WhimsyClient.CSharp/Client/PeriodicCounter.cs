namespace WhimsyClient.CSharp.Client
{
    internal class PeriodicCounter
    {
        private int _currentCounter;
        private readonly int _maxCounter;

        private PeriodicCounter(int currentCounter, int maxCounter)
        {
            _currentCounter = currentCounter;
            _maxCounter = maxCounter;
        }

        internal void UpdateCounter()
        {
            _currentCounter += 1;
            if (_currentCounter % _maxCounter == 0)
            {
                _currentCounter = 0;
            }
        }

        internal bool IsBeforeHalfWay()
        {
            return _currentCounter < _maxCounter / 2;
        }

        public static PeriodicCounter Create(int maxCallCounter)
        {
            return new PeriodicCounter(0, maxCallCounter);
        }
    }
}
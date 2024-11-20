
namespace Lost.Runtime.Footstone.Game
{
    public struct TimerF
    {
        private float currentTime;
        private float targetTime;
        private int lastUpdateFrame;

        public TimerF(float _currentTime, float _targetTime)
        {
            currentTime = _currentTime;
            targetTime = _targetTime;
            lastUpdateFrame = -1;
        }

        public bool Run(float deltaTime, bool isLoop = true)
        {
            currentTime += deltaTime;
            var isArrive = currentTime >= targetTime;
            if (isArrive && isLoop) currentTime -= targetTime;
            return isArrive;
        }

        public bool Run(System.Single deltaTime, int frame)
        {    
            bool isArrive = false;
            if(frame != lastUpdateFrame)
            {
                lastUpdateFrame = frame;
                currentTime += deltaTime;
                isArrive = currentTime >= targetTime;
                if(isArrive) currentTime -= targetTime;
            }
            return isArrive;
        }

    }
}

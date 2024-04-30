namespace ProjOb_24L_01180781.GUI
{
    public class GuiTimer
    {
        public TimerCallback TimerCallback { get; set; }
        public TimeSpan DueTime { get; set; }
        public TimeSpan Period { get; set; }
        public GuiTimer(TimerCallback timerCallback, TimeSpan dueTime, TimeSpan period)
        {
            TimerCallback = timerCallback;
            DueTime = dueTime;
            Period = period;
        }
        public void Start()
        {
            _timer ??= new Timer(TimerCallback, null, DueTime, Period);
        }
        public void Stop()
        {
            if (_timer is not null)
            {
                _timer.Dispose();
                _timer = null;
            }
        }
        private Timer? _timer;
    }
}

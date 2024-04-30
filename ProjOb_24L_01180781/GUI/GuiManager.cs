using FlightTrackerGUI;
using ProjOb_24L_01180781.AviationItems;
using ProjOb_24L_01180781.AviationItems.Interfaces;
using ProjOb_24L_01180781.Database;
using ProjOb_24L_01180781.DataSource.Tcp;
using ProjOb_24L_01180781.Exceptions;

namespace ProjOb_24L_01180781.GUI
{
    public class GuiManager
    {
        public static GuiManager GetInstance()
        {
            _instance ??= new GuiManager();
            return _instance;
        }
        public enum RunnerState
        {
            NotUsed = 0,
            InUse = 1,
            Used = 2
        }
        public bool IsRunnerNotUsed
        {
            get
            {
                lock (_runnerStateLock)
                {
                    return _runnerState == RunnerState.NotUsed;
                }
            }
        }
        public bool IsRunnerInUse
        {
            get
            {
                lock (_runnerStateLock)
                {
                    return _runnerState == RunnerState.InUse;
                }
            }
        }
        public bool IsRunnerUsed
        {
            get
            {
                lock (_runnerStateLock)
                {
                    return _runnerState == RunnerState.Used;
                }
            }
        }
        public void Run()
        {
            if (IsRunnerInUse || IsRunnerUsed) return;
            lock (_runnerStateLock)
            {
                _runnerState = RunnerState.InUse;
                _timer?.Start();
                Task.Factory.StartNew(Runner.Run).ContinueWith(RunContinuation);
            }
        }

        private GuiManager()
        {
            _timer = new(TimerCallback, TimeSpan.Zero, TimeSpan.FromSeconds(1));
            AviationDatabase.Tables[TcpAcronyms.Flight].ElementAdded += SyncFlightDetails;
            AviationDatabase.Tables[TcpAcronyms.Flight].ElementRemoved += SyncFlightDetails;
        }
        private void RunContinuation(Task task)
        {
            lock (_runnerStateLock)
            {
                _timer?.Stop();
                _runnerState = RunnerState.Used;
            }
            Console.WriteLine("Checking the radar window for possible failures...");
            if (task.Exception is not null)
            {
                foreach (var innerException in task.Exception.InnerExceptions)
                {
                    Console.WriteLine(innerException.Message);
                }
            }
            else
            {
                Console.WriteLine("No failures detected.");
            }
            Console.WriteLine("The radar window has been closed.");
        }
        private void TimerCallback(object? state)
        {
            AviationDatabase.Synchronize();

            List<FlightDetails> active;
            lock (_flightDetailsLock)
            {
                var now = DateTime.UtcNow;
                active = _flightDetails.Where(flightDetail =>
                    flightDetail.Flight.TakeOffDateTime <= now &&
                    now <= flightDetail.Flight.LandingDateTime).ToList();

                active.ForEach(FlightDetail => FlightDetail.UpdateFlightPosition());
                active.ForEach(FlightDetails => FlightDetails.UpdateFlightRotation());
            }

            Runner.UpdateGUI(new FlightsGuiDataAdapter(active));
        }
        private static void SyncFlightDetails(object? sender, ElementAddedEventArgs<IAviationItem> args)
        {
            lock (_flightDetailsLock)
            {
                _flightDetails.AddRange(args.AddedElements.Select(item =>
                {
                    UInt64 id;
                    lock (item.Lock)
                        id = item.Id;
                    var flight = (Flight)item;
                    var origin = AviationDatabase.Tables[TcpAcronyms.Airport].Find(flight.OriginId);
                    var target = AviationDatabase.Tables[TcpAcronyms.Airport].Find(flight.TargetId);

                    if (origin is null || target is null)
                        throw new TcpFormatException($"Could not find the airport for flight with ID = {id}.");

                    return new FlightDetails(flight, (Airport)origin, (Airport)target);
                }
                ));
            }
        }
        private static void SyncFlightDetails(object? sender, ElementRemovedEventArgs<IAviationItem> args)
        {
            var ids = args.RemovedElements.Select(x => x.Id).ToHashSet();
            lock (_flightDetailsLock)
            {
                _flightDetails = _flightDetails.Where(fd => !ids.Contains(fd.ID)).ToList();
            }
        }

        private static List<FlightDetails> _flightDetails = [];
        private static readonly object _flightDetailsLock = new();
        private RunnerState _runnerState = RunnerState.NotUsed;
        private static readonly object _runnerStateLock = new();
        private static GuiManager? _instance;
        private readonly GuiTimer? _timer;
    }
}

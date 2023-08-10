using OpenQA.Selenium;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageToDiscordRoles
{
    /// <summary>
    /// In an asynchrous context, one instance of this class should not be
    /// used to acquire more WebElements than one.
    /// Because of this, each object will terminate itself after usage.
    /// 
    /// TODO; Implement IDisposable
    /// </summary>
    public class Force
    {
        public event Action? OnAcquired;
        public event Action? OnFailedToAcquire;
        public event Action? OnNotAcquired;

        private IWebElement? result = null;
        private System.Timers.Timer _clock = new();
        private TaskCompletionSource<IWebElement?> _completionSource = new();
        private int _ticksPassed = 0;


        /// <summary>
        /// In Milliseconds
        /// </summary>
        public double? Ultimatum { get; set; } = null;
        /// <summary>
        /// In Milliseconds
        /// </summary>
        public double Interval { get; set; } = 400;

        /// <summary>
        /// Tries executes the 
        /// </summary>
        public async ValueTask<object?> NotAcquireAsync(Action callersFunction)
        {
            _clock.Interval = Interval;
            _clock.Elapsed += (_, _) =>
            {
                try
                {
                    callersFunction.Invoke();
                    OnAcquired?.Invoke();
                }
                catch
                {
                    _clock.Stop();
                    OnNotAcquired?.Invoke();
                    OnFailedToAcquire?.Invoke();
                }

                if (Ultimatum != null &&
                    Ultimatum < Interval * _ticksPassed)
                {
                    _clock.Stop();
                }
            };
            _clock.Start();

            OnNotAcquired += () => _completionSource.SetResult(null);

            return await _completionSource.Task;
        }

        /// <summary>
        /// Tries to execute the <paramref name="callersFunction"/> until it doesn't fail anymore, 
        /// (TODO: or an Ultimatum is reached).
        /// The object disposes of itself after the function returns;
        /// </summary>
        /// <param name="callersFunction"></param>
        /// <returns></returns>
        /// <exception cref="NullReferenceException"></exception>
        public async ValueTask<IWebElement?> AcquireAsync(Func<IWebElement> callersFunction)
        {
            _clock.Interval = Interval;
            _clock.Elapsed += (_,_) => TryAcquire(callersFunction);
            _clock.Start();


            OnAcquired += () => _completionSource
                .SetResult(result ?? throw new NullReferenceException("Internal Error (This is awkward, something unexpected happened)"));

            OnNotAcquired += () => _completionSource
                .SetResult(null);


            return await _completionSource.Task;
        }

        private void TryAcquire(Func<IWebElement> callersFunction)
        {
            try
            {
                result = callersFunction.Invoke();
                _clock.Stop();
                OnAcquired?.Invoke();
                Console.WriteLine("Acquired");
            }
            catch
            {
                OnFailedToAcquire?.Invoke();
            }

            if (Ultimatum != null &&
                Ultimatum < Interval * _ticksPassed)
            {
                _clock.Stop();
            }
        }
    }
}

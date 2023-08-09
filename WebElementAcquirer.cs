using OpenQA.Selenium;
using System;
using System.Collections.Generic;
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
    public class WebElementAcquirer
    {
        public event Action? OnWebElementAcquired;
        public event Action? OnWebElementNotAcquired;

        private IWebElement? result = null;
        private System.Timers.Timer _clock = new();
        private TaskCompletionSource<IWebElement?> _completionSource = new();
        private int _ticksPassed = 0;


        // TODO
        //public TimeSpan? Ultimatum { get; set; } = null;
        //public TimeSpan Interval { get; set; } = TimeSpan.FromMilliseconds(400);

        public async ValueTask<IWebElement?> AcquireAsync(Func<ValueTask<IWebElement>> callersFunction)
        {
            _clock.Interval = Convert.ToDouble(400);
            //_clock.Elapsed += (_, _) => TryAcquire(callersFunction);
            _clock.Elapsed += (_, _) =>
            {
                try
                {
                    result = callersFunction.Invoke().Result;
                    _clock.Stop();
                    OnWebElementAcquired?.Invoke();
                }
                catch
                {
                }

                //if (Ultimatum != null &&
                //    Interval * _ticksPassed >= Ultimatum)
                //    OnWebElementNotAcquired?.Invoke();
            };

            _clock.Start();

            
            OnWebElementAcquired += () => _completionSource
                .SetResult(result ?? throw new NullReferenceException("Internal Error (This is awkward, something unexpected happened)"));

            OnWebElementNotAcquired += () => _completionSource
                .SetResult(null);

            // TODO: Dispose here: 

            return await _completionSource.Task;
        }
        public async ValueTask<IWebElement?> AcquireAsync(Func<IWebElement> callersFunction)
        {
            _clock.Interval = Convert.ToDouble(400);
            //_clock.Elapsed += (_, _) => TryAcquire(callersFunction);
            _clock.Elapsed += (_, _) =>
            {
                try
                {
                    result = callersFunction.Invoke();
                    _clock.Stop();
                    OnWebElementAcquired?.Invoke();
                }
                catch
                {
                }

                //if (Ultimatum != null &&
                //    Interval * _ticksPassed >= Ultimatum)
                //    OnWebElementNotAcquired?.Invoke();
            };

            _clock.Start();


            OnWebElementAcquired += () => _completionSource
                .SetResult(result ?? throw new NullReferenceException("Internal Error (This is awkward, something unexpected happened)"));

            OnWebElementNotAcquired += () => _completionSource
                .SetResult(null);

            // TODO: Dispose here: 

            return await _completionSource.Task;
        }

        private void TryAcquire(Func<ValueTask<IWebElement>> callersFunction)
        {
            try
            {
                result = callersFunction.Invoke().Result;
                _clock.Stop();
                OnWebElementAcquired?.Invoke();
            }
            catch
            {
            }

            //if (Ultimatum != null &&
            //    Interval * _ticksPassed >= Ultimatum)
            //{
            //    OnWebElementNotAcquired?.Invoke();
            //}
        }
    }
}

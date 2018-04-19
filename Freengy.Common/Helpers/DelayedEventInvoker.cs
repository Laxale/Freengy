// Created by Laxale 19.04.2018
//
//

using System;
using System.Timers;


namespace Freengy.Common.Helpers 
{
    /// <summary>
    /// Allows to generate an event over time delay.
    /// </summary>
    public class DelayedEventInvoker : IDisposable 
    {
        private readonly Timer timer;

        private bool isDisposed;

        /// <summary>
        /// Минимальная задержка между запросом на событие и его вызовом.
        /// </summary>
        public const double MinimumEventDelayInMs = 10;

        /// <summary>
        /// Событие произошло - задержка между запросом на событие и вызовом события истекла.
        /// </summary>
        public event Action DelayedEvent = () => { };


        /// <summary>
        /// Creates new <see cref="DelayedEventInvoker"/> with a given event delay.
        /// </summary>
        /// <param name="eventDelayInMs">Delay before generating an event (in milliseconds).</param>
        public DelayedEventInvoker(int eventDelayInMs) 
        {
            timer = new Timer(eventDelayInMs < MinimumEventDelayInMs ? MinimumEventDelayInMs : eventDelayInMs);

            timer.Elapsed += (sender, args) =>
            {
                timer.Stop();
                DelayedEvent();
            };
        }

        ~DelayedEventInvoker() 
        {
            Dispose();
        }


        /// <summary>
        /// Запросить отложенное событие. Перезапускает таймер, если предыдущий запрос в процессе ожидания.
        /// </summary>
        public void RequestDelayedEvent() 
        {
            timer.Stop();
            timer.Start();
        }

        /// <summary>
        /// Отменить запрос на отложенное событие.
        /// </summary>
        public void RemoveDelayedEventRequest() 
        {
            timer.Stop();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose() 
        {
            if (isDisposed) return;

            timer.Dispose();
        }
    }
}
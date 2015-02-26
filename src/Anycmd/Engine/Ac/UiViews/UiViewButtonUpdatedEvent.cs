﻿
namespace Anycmd.Engine.Ac.UiViews
{
    using UiViews;
    using Events;
    using InOuts;

    /// <summary>
    /// 
    /// </summary>
    public class UiViewButtonUpdatedEvent : DomainEvent
    {
        public UiViewButtonUpdatedEvent(IAcSession acSession, UiViewButtonBase source, IUiViewButtonUpdateIo input)
            : base(acSession, source)
        {
            if (input == null)
            {
                throw new System.ArgumentNullException("input");
            }
            this.Input = input;
        }

        public IUiViewButtonUpdateIo Input { get; private set; }
    }
}

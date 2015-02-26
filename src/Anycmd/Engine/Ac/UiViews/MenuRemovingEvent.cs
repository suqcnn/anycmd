﻿
namespace Anycmd.Engine.Ac.UiViews
{
    using UiViews;
    using Events;

    public class MenuRemovingEvent: DomainEvent
    {
        public MenuRemovingEvent(IAcSession acSession, MenuBase source)
            : base(acSession, source)
        {
        }
    }
}
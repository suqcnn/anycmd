﻿
namespace Anycmd.Engine.Ac.UiViews
{
    using System;

    public class RemoveMenuCommand : RemoveEntityCommand
    {
        public RemoveMenuCommand(IAcSession acSession, Guid menuId)
            : base(acSession, menuId)
        {

        }
    }
}

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DemoGame.Extensions;
using Platyform.Extensions;

namespace DemoGame.Server
{
    public struct UpdateItemFieldValues
    {
        public readonly int ItemGuid;
        public readonly object Value;

        public UpdateItemFieldValues(int itemGuid, object value)
        {
            ItemGuid = itemGuid;
            Value = value;
        }
    }
}
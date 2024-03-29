﻿using System;
using System.Windows.Controls;

namespace KurtzGlide.Utilities
{
    public class Logger
    {
        private ItemsControl itemsControl;

        public Logger(ItemsControl itemsControl) => this.itemsControl = itemsControl;

        public void Log(string text) => itemsControl.Items.Add("[" + DateTime.Now.ToLongTimeString() + "] " + text);
    }
}

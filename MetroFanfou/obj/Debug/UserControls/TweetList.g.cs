﻿#pragma checksum "D:\Project\WindowsPhone7\MetroFanfou\MetroFanfou\UserControls\TweetList.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "E70F53EE0D75EE2FCB443F7EBD5AD9A1"
//------------------------------------------------------------------------------
// <auto-generated>
//     此代码由工具生成。
//     运行时版本:4.0.30319.17379
//
//     对此文件的更改可能会导致不正确的行为，并且如果
//     重新生成代码，这些更改将会丢失。
// </auto-generated>
//------------------------------------------------------------------------------

using Microsoft.Phone.Controls;
using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace TM.QWeibo.Client.UserControls {
    
    
    public partial class TweetList : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid TweetListLayoutRoot;
        
        internal Microsoft.Phone.Controls.LongListSelector TweetListBox;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/MetroFanfou;component/UserControls/TweetList.xaml", System.UriKind.Relative));
            this.TweetListLayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("TweetListLayoutRoot")));
            this.TweetListBox = ((Microsoft.Phone.Controls.LongListSelector)(this.FindName("TweetListBox")));
        }
    }
}


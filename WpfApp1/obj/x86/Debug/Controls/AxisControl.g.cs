﻿#pragma checksum "..\..\..\..\Controls\AxisControl.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "22ADD0E8FA7A260EA00A609DFAB8E399"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Effects;
using System.Windows.Media.Imaging;
using System.Windows.Media.Media3D;
using System.Windows.Media.TextFormatting;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Shell;


namespace CustomControl {
    
    
    /// <summary>
    /// AxisControl
    /// </summary>
    public partial class AxisControl : System.Windows.Controls.UserControl, System.Windows.Markup.IComponentConnector, System.Windows.Markup.IStyleConnector {
        
        
        #line 9 "..\..\..\..\Controls\AxisControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal CustomControl.AxisControl _AxisControl;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\..\Controls\AxisControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Shapes.Rectangle ShadeRectangle_Selected;
        
        #line default
        #line hidden
        
        
        #line 81 "..\..\..\..\Controls\AxisControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox TargetPositionTextBox;
        
        #line default
        #line hidden
        
        
        #line 93 "..\..\..\..\Controls\AxisControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox AxisNameTextBox;
        
        #line default
        #line hidden
        
        
        #line 94 "..\..\..\..\Controls\AxisControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox AxisStatusTextBox;
        
        #line default
        #line hidden
        
        
        #line 95 "..\..\..\..\Controls\AxisControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox CurrentPositionTextBox;
        
        #line default
        #line hidden
        
        
        #line 96 "..\..\..\..\Controls\AxisControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox VelocityTextBox;
        
        #line default
        #line hidden
        
        
        #line 97 "..\..\..\..\Controls\AxisControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox AccelTextBox;
        
        #line default
        #line hidden
        
        
        #line 98 "..\..\..\..\Controls\AxisControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.TextBox DecelTextBox;
        
        #line default
        #line hidden
        
        
        #line 99 "..\..\..\..\Controls\AxisControl.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Button RemoveButton;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Automater;component/controls/axiscontrol.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\..\Controls\AxisControl.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this._AxisControl = ((CustomControl.AxisControl)(target));
            return;
            case 5:
            
            #line 47 "..\..\..\..\Controls\AxisControl.xaml"
            ((System.Windows.Controls.GroupBox)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 47 "..\..\..\..\Controls\AxisControl.xaml"
            ((System.Windows.Controls.GroupBox)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 49 "..\..\..\..\Controls\AxisControl.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 49 "..\..\..\..\Controls\AxisControl.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 7:
            this.ShadeRectangle_Selected = ((System.Windows.Shapes.Rectangle)(target));
            return;
            case 8:
            
            #line 60 "..\..\..\..\Controls\AxisControl.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseLeftButtonDown += new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonDown);
            
            #line default
            #line hidden
            
            #line 60 "..\..\..\..\Controls\AxisControl.xaml"
            ((System.Windows.Controls.Grid)(target)).MouseLeftButtonUp += new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonUp);
            
            #line default
            #line hidden
            return;
            case 9:
            this.TargetPositionTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 81 "..\..\..\..\Controls\AxisControl.xaml"
            this.TargetPositionTextBox.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.AxisControlTextBox_GotKeyboardFocus);
            
            #line default
            #line hidden
            
            #line 81 "..\..\..\..\Controls\AxisControl.xaml"
            this.TargetPositionTextBox.LostKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.AxisControlTextBox_LostKeyboardFocus);
            
            #line default
            #line hidden
            
            #line 81 "..\..\..\..\Controls\AxisControl.xaml"
            this.TargetPositionTextBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.AxisTextbox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 10:
            this.AxisNameTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 11:
            this.AxisStatusTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 12:
            this.CurrentPositionTextBox = ((System.Windows.Controls.TextBox)(target));
            return;
            case 13:
            this.VelocityTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 96 "..\..\..\..\Controls\AxisControl.xaml"
            this.VelocityTextBox.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.AxisControlTextBox_GotKeyboardFocus);
            
            #line default
            #line hidden
            
            #line 96 "..\..\..\..\Controls\AxisControl.xaml"
            this.VelocityTextBox.LostFocus += new System.Windows.RoutedEventHandler(this.AxisControlTextBox_LostKeyboardFocus);
            
            #line default
            #line hidden
            
            #line 96 "..\..\..\..\Controls\AxisControl.xaml"
            this.VelocityTextBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.AxisTextbox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 14:
            this.AccelTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 97 "..\..\..\..\Controls\AxisControl.xaml"
            this.AccelTextBox.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.AxisControlTextBox_GotKeyboardFocus);
            
            #line default
            #line hidden
            
            #line 97 "..\..\..\..\Controls\AxisControl.xaml"
            this.AccelTextBox.LostKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.AxisControlTextBox_LostKeyboardFocus);
            
            #line default
            #line hidden
            
            #line 97 "..\..\..\..\Controls\AxisControl.xaml"
            this.AccelTextBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.AxisTextbox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 15:
            this.DecelTextBox = ((System.Windows.Controls.TextBox)(target));
            
            #line 98 "..\..\..\..\Controls\AxisControl.xaml"
            this.DecelTextBox.GotKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.AxisControlTextBox_GotKeyboardFocus);
            
            #line default
            #line hidden
            
            #line 98 "..\..\..\..\Controls\AxisControl.xaml"
            this.DecelTextBox.LostKeyboardFocus += new System.Windows.Input.KeyboardFocusChangedEventHandler(this.AxisControlTextBox_LostKeyboardFocus);
            
            #line default
            #line hidden
            
            #line 98 "..\..\..\..\Controls\AxisControl.xaml"
            this.DecelTextBox.KeyDown += new System.Windows.Input.KeyEventHandler(this.AxisTextbox_KeyDown);
            
            #line default
            #line hidden
            return;
            case 16:
            this.RemoveButton = ((System.Windows.Controls.Button)(target));
            
            #line 99 "..\..\..\..\Controls\AxisControl.xaml"
            this.RemoveButton.Click += new System.Windows.RoutedEventHandler(this.RemoveButton_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "4.0.0.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        void System.Windows.Markup.IStyleConnector.Connect(int connectionId, object target) {
            System.Windows.EventSetter eventSetter;
            switch (connectionId)
            {
            case 2:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseLeftButtonDownEvent;
            
            #line 14 "..\..\..\..\Controls\AxisControl.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonDown);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseLeftButtonUpEvent;
            
            #line 15 "..\..\..\..\Controls\AxisControl.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonUp);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            case 3:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseLeftButtonDownEvent;
            
            #line 21 "..\..\..\..\Controls\AxisControl.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonDown);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseLeftButtonUpEvent;
            
            #line 22 "..\..\..\..\Controls\AxisControl.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonUp);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            case 4:
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseLeftButtonDownEvent;
            
            #line 29 "..\..\..\..\Controls\AxisControl.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonDown);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            eventSetter = new System.Windows.EventSetter();
            eventSetter.Event = System.Windows.UIElement.MouseLeftButtonUpEvent;
            
            #line 30 "..\..\..\..\Controls\AxisControl.xaml"
            eventSetter.Handler = new System.Windows.Input.MouseButtonEventHandler(this.Control_MouseLeftButtonUp);
            
            #line default
            #line hidden
            ((System.Windows.Style)(target)).Setters.Add(eventSetter);
            break;
            }
        }
    }
}


﻿#pragma checksum "..\..\JoyStickSelecter.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "298FA968B3798CE2F0BC348A33CBDF02"
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
using WpfApp1;


namespace WpfApp1 {
    
    
    /// <summary>
    /// JoyStickSelecter
    /// </summary>
    public partial class JoyStickSelecter : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 32 "..\..\JoyStickSelecter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton RB0;
        
        #line default
        #line hidden
        
        
        #line 35 "..\..\JoyStickSelecter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton RB1;
        
        #line default
        #line hidden
        
        
        #line 38 "..\..\JoyStickSelecter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton RB2;
        
        #line default
        #line hidden
        
        
        #line 41 "..\..\JoyStickSelecter.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton RB3;
        
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
            System.Uri resourceLocater = new System.Uri("/Automater;component/joystickselecter.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\JoyStickSelecter.xaml"
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
            
            #line 8 "..\..\JoyStickSelecter.xaml"
            ((WpfApp1.JoyStickSelecter)(target)).PreviewMouseDown += new System.Windows.Input.MouseButtonEventHandler(this.JSSelector_PreviewMouseDown);
            
            #line default
            #line hidden
            return;
            case 2:
            this.RB0 = ((System.Windows.Controls.RadioButton)(target));
            
            #line 32 "..\..\JoyStickSelecter.xaml"
            this.RB0.Click += new System.Windows.RoutedEventHandler(this.RadioButtonjs_Clicked);
            
            #line default
            #line hidden
            return;
            case 3:
            this.RB1 = ((System.Windows.Controls.RadioButton)(target));
            
            #line 35 "..\..\JoyStickSelecter.xaml"
            this.RB1.Click += new System.Windows.RoutedEventHandler(this.RadioButtonjs_Clicked);
            
            #line default
            #line hidden
            return;
            case 4:
            this.RB2 = ((System.Windows.Controls.RadioButton)(target));
            
            #line 38 "..\..\JoyStickSelecter.xaml"
            this.RB2.Click += new System.Windows.RoutedEventHandler(this.RadioButtonjs_Clicked);
            
            #line default
            #line hidden
            return;
            case 5:
            this.RB3 = ((System.Windows.Controls.RadioButton)(target));
            
            #line 41 "..\..\JoyStickSelecter.xaml"
            this.RB3.Click += new System.Windows.RoutedEventHandler(this.RadioButtonjs_Clicked);
            
            #line default
            #line hidden
            return;
            case 6:
            
            #line 43 "..\..\JoyStickSelecter.xaml"
            ((System.Windows.Controls.Button)(target)).Click += new System.Windows.RoutedEventHandler(this.Button_Click);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


﻿#pragma checksum "..\..\..\MainWindow.xaml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "1DFC95178884A3922C8FD2EB976DB7BE05409E4F"
//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан программой.
//     Исполняемая версия:4.0.30319.42000
//
//     Изменения в этом файле могут привести к неправильной работе и будут потеряны в случае
//     повторной генерации кода.
// </auto-generated>
//------------------------------------------------------------------------------

using Simple_galery;
using System;
using System.Diagnostics;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Controls.Ribbon;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms.Integration;
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


namespace Simple_galery {
    
    
    /// <summary>
    /// MainWindow
    /// </summary>
    public partial class MainWindow : System.Windows.Window, System.Windows.Markup.IComponentConnector {
        
        
        #line 34 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Grid MainGrid;
        
        #line default
        #line hidden
        
        
        #line 51 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuOpen;
        
        #line default
        #line hidden
        
        
        #line 52 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuFullScreen;
        
        #line default
        #line hidden
        
        
        #line 53 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuExit;
        
        #line default
        #line hidden
        
        
        #line 54 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.MenuItem menuBackgroundView;
        
        #line default
        #line hidden
        
        
        #line 55 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtnNone;
        
        #line default
        #line hidden
        
        
        #line 56 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.RadioButton rbtnPreviewImage;
        
        #line default
        #line hidden
        
        
        #line 63 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.DockPanel panelPreview;
        
        #line default
        #line hidden
        
        
        #line 70 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.Image imgPreview;
        
        #line default
        #line hidden
        
        
        #line 82 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.ScrollViewer scrollViewer;
        
        #line default
        #line hidden
        
        
        #line 91 "..\..\..\MainWindow.xaml"
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1823:AvoidUnusedPrivateFields")]
        internal System.Windows.Controls.StackPanel stack;
        
        #line default
        #line hidden
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.3.0")]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Uri resourceLocater = new System.Uri("/Simple_galery;component/mainwindow.xaml", System.UriKind.Relative);
            
            #line 1 "..\..\..\MainWindow.xaml"
            System.Windows.Application.LoadComponent(this, resourceLocater);
            
            #line default
            #line hidden
        }
        
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        [System.CodeDom.Compiler.GeneratedCodeAttribute("PresentationBuildTasks", "7.0.3.0")]
        [System.ComponentModel.EditorBrowsableAttribute(System.ComponentModel.EditorBrowsableState.Never)]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Design", "CA1033:InterfaceMethodsShouldBeCallableByChildTypes")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Maintainability", "CA1502:AvoidExcessiveComplexity")]
        [System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1800:DoNotCastUnnecessarily")]
        void System.Windows.Markup.IComponentConnector.Connect(int connectionId, object target) {
            switch (connectionId)
            {
            case 1:
            this.MainGrid = ((System.Windows.Controls.Grid)(target));
            
            #line 37 "..\..\..\MainWindow.xaml"
            this.MainGrid.PreviewDragEnter += new System.Windows.DragEventHandler(this.MainGrid_PreviewDragEnter);
            
            #line default
            #line hidden
            
            #line 38 "..\..\..\MainWindow.xaml"
            this.MainGrid.PreviewDrop += new System.Windows.DragEventHandler(this.MainGrid_PreviewDrop);
            
            #line default
            #line hidden
            return;
            case 2:
            this.menuOpen = ((System.Windows.Controls.MenuItem)(target));
            
            #line 51 "..\..\..\MainWindow.xaml"
            this.menuOpen.Click += new System.Windows.RoutedEventHandler(this.menuOpen_Click);
            
            #line default
            #line hidden
            return;
            case 3:
            this.menuFullScreen = ((System.Windows.Controls.MenuItem)(target));
            
            #line 52 "..\..\..\MainWindow.xaml"
            this.menuFullScreen.Click += new System.Windows.RoutedEventHandler(this.menuFullScreen_Click);
            
            #line default
            #line hidden
            return;
            case 4:
            this.menuExit = ((System.Windows.Controls.MenuItem)(target));
            
            #line 53 "..\..\..\MainWindow.xaml"
            this.menuExit.Click += new System.Windows.RoutedEventHandler(this.menuExit_Click);
            
            #line default
            #line hidden
            return;
            case 5:
            this.menuBackgroundView = ((System.Windows.Controls.MenuItem)(target));
            return;
            case 6:
            this.rbtnNone = ((System.Windows.Controls.RadioButton)(target));
            
            #line 55 "..\..\..\MainWindow.xaml"
            this.rbtnNone.Checked += new System.Windows.RoutedEventHandler(this.rbtnNone_Checked);
            
            #line default
            #line hidden
            return;
            case 7:
            this.rbtnPreviewImage = ((System.Windows.Controls.RadioButton)(target));
            
            #line 56 "..\..\..\MainWindow.xaml"
            this.rbtnPreviewImage.Checked += new System.Windows.RoutedEventHandler(this.rbtnPreviewImage_Checked);
            
            #line default
            #line hidden
            return;
            case 8:
            this.panelPreview = ((System.Windows.Controls.DockPanel)(target));
            return;
            case 9:
            this.imgPreview = ((System.Windows.Controls.Image)(target));
            
            #line 73 "..\..\..\MainWindow.xaml"
            this.imgPreview.PreviewDragEnter += new System.Windows.DragEventHandler(this.MainGrid_PreviewDragEnter);
            
            #line default
            #line hidden
            
            #line 74 "..\..\..\MainWindow.xaml"
            this.imgPreview.PreviewDrop += new System.Windows.DragEventHandler(this.MainGrid_PreviewDrop);
            
            #line default
            #line hidden
            return;
            case 10:
            this.scrollViewer = ((System.Windows.Controls.ScrollViewer)(target));
            return;
            case 11:
            this.stack = ((System.Windows.Controls.StackPanel)(target));
            
            #line 95 "..\..\..\MainWindow.xaml"
            this.stack.PreviewDragEnter += new System.Windows.DragEventHandler(this.MainGrid_PreviewDragEnter);
            
            #line default
            #line hidden
            
            #line 96 "..\..\..\MainWindow.xaml"
            this.stack.PreviewDragOver += new System.Windows.DragEventHandler(this.MainGrid_PreviewDragEnter);
            
            #line default
            #line hidden
            
            #line 97 "..\..\..\MainWindow.xaml"
            this.stack.PreviewDrop += new System.Windows.DragEventHandler(this.MainGrid_PreviewDrop);
            
            #line default
            #line hidden
            return;
            }
            this._contentLoaded = true;
        }
    }
}


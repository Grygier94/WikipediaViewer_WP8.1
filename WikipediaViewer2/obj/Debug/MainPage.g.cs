﻿

#pragma checksum "D:\VisualStudioProjects\WikipediaViewer2\WikipediaViewer2\MainPage.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "4AB6BD4E960F1DA0E1D2EC1BF95C6EC0"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WikipediaViewer2
{
    partial class MainPage : global::Windows.UI.Xaml.Controls.Page, global::Windows.UI.Xaml.Markup.IComponentConnector
    {
        [global::System.CodeDom.Compiler.GeneratedCodeAttribute("Microsoft.Windows.UI.Xaml.Build.Tasks"," 4.0.0.0")]
        [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
 
        public void Connect(int connectionId, object target)
        {
            switch(connectionId)
            {
            case 1:
                #line 61 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.RandomArticle_Click;
                 #line default
                 #line hidden
                break;
            case 2:
                #line 64 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.UIElement)(target)).KeyUp += this.tbSearch_KeyUp;
                 #line default
                 #line hidden
                break;
            case 3:
                #line 65 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.btnSearch_Click;
                 #line default
                 #line hidden
                break;
            case 4:
                #line 53 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ChangeLanguage_Click;
                 #line default
                 #line hidden
                break;
            case 5:
                #line 56 "..\..\MainPage.xaml"
                ((global::Windows.UI.Xaml.Controls.Primitives.ButtonBase)(target)).Click += this.ChangeLanguage_Click;
                 #line default
                 #line hidden
                break;
            }
            this._contentLoaded = true;
        }
    }
}


﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.0
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WinHAB.Tests {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "4.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class JsonResources {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal JsonResources() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WinHAB.Tests.JsonResources", typeof(JsonResources).Assembly);
                    resourceMan = temp;
                }
                return resourceMan;
            }
        }
        
        /// <summary>
        ///   Overrides the current thread's CurrentUICulture property for all
        ///   resource lookups using this strongly typed resource class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Globalization.CultureInfo Culture {
            get {
                return resourceCulture;
            }
            set {
                resourceCulture = value;
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {&quot;id&quot;:&quot;demo&quot;,&quot;title&quot;:&quot;Demo House&quot;,&quot;link&quot;:&quot;http://demo.openhab.org:8080/rest/sitemaps/demo/demo&quot;,&quot;leaf&quot;:&quot;false&quot;,&quot;widget&quot;:[{&quot;widgetId&quot;:&quot;demo_0&quot;,&quot;type&quot;:&quot;Frame&quot;,&quot;label&quot;:&quot;&quot;,&quot;icon&quot;:&quot;frame&quot;,&quot;widget&quot;:[{&quot;widgetId&quot;:&quot;demo_0_0&quot;,&quot;type&quot;:&quot;Group&quot;,&quot;label&quot;:&quot;First Floor&quot;,&quot;icon&quot;:&quot;firstfloor&quot;,&quot;item&quot;:{&quot;type&quot;:&quot;GroupItem&quot;,&quot;name&quot;:&quot;gFF&quot;,&quot;state&quot;:&quot;Undefined&quot;,&quot;link&quot;:&quot;http://demo.openhab.org:8080/rest/items/gFF&quot;},&quot;linkedPage&quot;:{&quot;id&quot;:&quot;0000&quot;,&quot;title&quot;:&quot;First Floor&quot;,&quot;icon&quot;:&quot;firstfloor&quot;,&quot;link&quot;:&quot;http://demo.openhab.org:8080/rest/sitemaps/demo/0 [rest of string was truncated]&quot;;.
        /// </summary>
        internal static string DemoDemo {
            get {
                return ResourceManager.GetString("DemoDemo", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to {&quot;sitemap&quot;: [ {&quot;name&quot;:&quot;demo&quot;,&quot;label&quot;:&quot;Demo House&quot;,&quot;link&quot;:&quot;http://demo.openhab.org:8080/rest/sitemaps/demo&quot;,&quot;homepage&quot;:{&quot;link&quot;:&quot;http://demo.openhab.org:8080/rest/sitemaps/demo/demo&quot;,&quot;leaf&quot;:&quot;false&quot;}}, {&quot;name&quot;:&quot;demo 1&quot;,&quot;label&quot;:&quot;Demo House 1&quot;,&quot;link&quot;:&quot;http://demo.openhab.org:8080/rest/sitemaps/demo &quot;,&quot;homepage&quot;:{&quot;link&quot;:&quot;http://demo.openhab.org:8080/rest/sitemaps/demo1/demo1&quot;,&quot;leaf&quot;:&quot;false&quot;}}]}.
        /// </summary>
        internal static string Sitemaps {
            get {
                return ResourceManager.GetString("Sitemaps", resourceCulture);
            }
        }
    }
}

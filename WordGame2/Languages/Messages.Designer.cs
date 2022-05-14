﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WordGame2.Languages {
    using System;
    
    
    /// <summary>
    ///   A strongly-typed resource class, for looking up localized strings, etc.
    /// </summary>
    // This class was auto-generated by the StronglyTypedResourceBuilder
    // class via a tool like ResGen or Visual Studio.
    // To add or remove a member, edit your .ResX file then rerun ResGen
    // with the /str option, or rebuild your VS project.
    [global::System.CodeDom.Compiler.GeneratedCodeAttribute("System.Resources.Tools.StronglyTypedResourceBuilder", "17.0.0.0")]
    [global::System.Diagnostics.DebuggerNonUserCodeAttribute()]
    [global::System.Runtime.CompilerServices.CompilerGeneratedAttribute()]
    internal class Messages {
        
        private static global::System.Resources.ResourceManager resourceMan;
        
        private static global::System.Globalization.CultureInfo resourceCulture;
        
        [global::System.Diagnostics.CodeAnalysis.SuppressMessageAttribute("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        internal Messages() {
        }
        
        /// <summary>
        ///   Returns the cached ResourceManager instance used by this class.
        /// </summary>
        [global::System.ComponentModel.EditorBrowsableAttribute(global::System.ComponentModel.EditorBrowsableState.Advanced)]
        internal static global::System.Resources.ResourceManager ResourceManager {
            get {
                if (object.ReferenceEquals(resourceMan, null)) {
                    global::System.Resources.ResourceManager temp = new global::System.Resources.ResourceManager("WordGame2.Languages.Messages", typeof(Messages).Assembly);
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
        ///   Looks up a localized string similar to Enter first player name:.
        /// </summary>
        internal static string FirstPlayerName {
            get {
                return ResourceManager.GetString("FirstPlayerName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to First player&apos;s turn:.
        /// </summary>
        internal static string FirstPlayerTurn {
            get {
                return ResourceManager.GetString("FirstPlayerTurn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The word composed incrorectly. The player lost..
        /// </summary>
        internal static string IncorectCompose {
            get {
                return ResourceManager.GetString("IncorectCompose", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter a word:.
        /// </summary>
        internal static string InputWord {
            get {
                return ResourceManager.GetString("InputWord", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Press eny key to continue....
        /// </summary>
        internal static string KeyToContinue {
            get {
                return ResourceManager.GetString("KeyToContinue", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to ^[a-zA-Z]*$.
        /// </summary>
        internal static string LettersRegex {
            get {
                return ResourceManager.GetString("LettersRegex", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to No.
        /// </summary>
        internal static string No {
            get {
                return ResourceManager.GetString("No", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter the main word:.
        /// </summary>
        internal static string PrimaryWordInput {
            get {
                return ResourceManager.GetString("PrimaryWordInput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The main word: .
        /// </summary>
        internal static string PrimaryWordOutput {
            get {
                return ResourceManager.GetString("PrimaryWordOutput", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Want to play again?.
        /// </summary>
        internal static string Restart {
            get {
                return ResourceManager.GetString("Restart", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Enter second player name:.
        /// </summary>
        internal static string SecondPlayerName {
            get {
                return ResourceManager.GetString("SecondPlayerName", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Second player&apos;s turn:.
        /// </summary>
        internal static string SecondPlayerTurn {
            get {
                return ResourceManager.GetString("SecondPlayerTurn", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Choose language:.
        /// </summary>
        internal static string SelectLanguage {
            get {
                return ResourceManager.GetString("SelectLanguage", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Time is over. The player lost..
        /// </summary>
        internal static string TimerElapsed {
            get {
                return ResourceManager.GetString("TimerElapsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The word must contain only English letters..
        /// </summary>
        internal static string WordCharactersError {
            get {
                return ResourceManager.GetString("WordCharactersError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to This word has already been used. The player lost..
        /// </summary>
        internal static string WordIsUsed {
            get {
                return ResourceManager.GetString("WordIsUsed", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to The main word should not be longer than 30 characters and shorter than 8..
        /// </summary>
        internal static string WordLengthError {
            get {
                return ResourceManager.GetString("WordLengthError", resourceCulture);
            }
        }
        
        /// <summary>
        ///   Looks up a localized string similar to Yes.
        /// </summary>
        internal static string Yes {
            get {
                return ResourceManager.GetString("Yes", resourceCulture);
            }
        }
    }
}

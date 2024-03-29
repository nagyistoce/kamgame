﻿#region License

/*
Microsoft Public License (Ms-PL)
MonoGame - Copyright © 2009 The MonoGame Team

All rights reserved.

This license governs use of the accompanying software. If you use the software, you accept this license. If you do not
accept the license, do not use the software.

1. Definitions
The terms "reproduce," "reproduction," "derivative works," and "distribution" have the same meaning here as under 
U.S. copyright law.

A "contribution" is the original software, or any additions or changes to the software.
A "contributor" is any person that distributes its contribution under this license.
"Licensed patents" are a contributor's patent claims that read directly on its contribution.

2. Grant of Rights
(A) Copyright Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
each contributor grants you a non-exclusive, worldwide, royalty-free copyright license to reproduce its contribution, prepare derivative works of its contribution, and distribute its contribution or any derivative works that you create.
(B) Patent Grant- Subject to the terms of this license, including the license conditions and limitations in section 3, 
each contributor grants you a non-exclusive, worldwide, royalty-free license under its licensed patents to make, have made, use, sell, offer for sale, import, and/or otherwise dispose of its contribution in the software or derivative works of the contribution in the software.

3. Conditions and Limitations
(A) No Trademark License- This license does not grant you rights to use any contributors' name, logo, or trademarks.
(B) If you bring a patent claim against any contributor over patents that you claim are infringed by the software, 
your patent license from such contributor to the software ends automatically.
(C) If you distribute any portion of the software, you must retain all copyright, patent, trademark, and attribution 
notices that are present in the software.
(D) If you distribute any portion of the software in source code form, you may do so only under this license by including 
a complete copy of this license with your distribution. If you distribute any portion of the software in compiled or object 
code form, you may only do so under a license that complies with this license.
(E) The software is licensed "as-is." You bear the risk of using it. The contributors give no express warranties, guarantees
or conditions. You may have additional consumer rights under your local laws which this license cannot change. To the extent
permitted under your local laws, the contributors exclude the implied warranties of merchantability, fitness for a particular
purpose and non-infringement.
*/

#endregion License

#region Using clause

using System;
using System.Collections.Generic;
using System.Threading;
using Android.App;
using Android.Widget;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

#endregion Using clause

namespace Microsoft.Xna.Framework.GamerServices
{
    public static class Guide
    {
        private static bool isVisible;
        private static bool isMessageBoxShowing;
        private static bool isKeyboardInputShowing;

        internal static void Initialise(Game game)
        {
            MonoGameGamerServicesHelper.Initialise(game);
        }

        public static string ShowKeyboardInput(
            PlayerIndex player,
            string title,
            string description,
            string defaultText,
            bool usePasswordMode)
        {
            string result = defaultText;
            if (!isKeyboardInputShowing)
            {
                isKeyboardInputShowing = true;
            }

            isVisible = isKeyboardInputShowing;

            Game.Activity.RunOnUiThread(() =>
                {
                    var alert = new AlertDialog.Builder(Game.Activity);

                    alert.SetTitle(title);
                    alert.SetMessage(description);

                    var input = new EditText(Game.Activity) {Text = defaultText};
                    alert.SetView(input);

                    alert.SetPositiveButton("Ok", (dialog, whichButton) =>
                        {
                            result = input.Text;
                            isVisible = false;
                        });

                    alert.SetNegativeButton("Cancel", (dialog, whichButton) =>
                        {
                            result = null;
                            isVisible = false;
                        });

                    alert.Show();
                });

            while (isVisible)
            {
                Thread.Sleep(1);
            }

            return result;
        }

        public static IAsyncResult BeginShowKeyboardInput(
            PlayerIndex player,
            string title,
            string description,
            string defaultText,
            AsyncCallback callback,
            Object state)
        {
            return BeginShowKeyboardInput(player, title, description, defaultText, callback, state, false);
        }

        public static IAsyncResult BeginShowKeyboardInput(
            PlayerIndex player,
            string title,
            string description,
            string defaultText,
            AsyncCallback callback,
            Object state,
            bool usePasswordMode)
        {
            ShowKeyboardInputDelegate ski = ShowKeyboardInput;

            return ski.BeginInvoke(player, title, description, defaultText, usePasswordMode, callback, ski);
        }

        public static string EndShowKeyboardInput(IAsyncResult result)
        {
            try
            {
                var ski = (ShowKeyboardInputDelegate) result.AsyncState;

                return ski.EndInvoke(result);
            }
            finally
            {
                isVisible = false;
            }
        }

        public static int? ShowMessageBox(string title,
                                          string text,
                                          IEnumerable<string> buttons,
                                          int focusButton,
                                          MessageBoxIcon icon)
        {
            int? result = null;

            if (!isMessageBoxShowing)
            {
                isMessageBoxShowing = true;

                /*UIAlertView alert = new UIAlertView();
				alert.Title = title;
				foreach( string btn in buttons )
				{
					alert.AddButton(btn);
				}
				alert.Message = text;
				alert.Dismissed += delegate(object sender, UIButtonEventArgs e) 
								{ 
									result = e.ButtonIndex;
									isMessageBoxShowing = false;
								};
				alert.Clicked += delegate(object sender, UIButtonEventArgs e) 
								{ 
									result = e.ButtonIndex; 
									isMessageBoxShowing = false;
								};
				
				GetInvokeOnMainThredObj().InvokeOnMainThread(delegate {    
       		 		alert.Show();   
    			});*/
            }

            isVisible = isMessageBoxShowing;

            return result;
        }

        public static IAsyncResult BeginShowMessageBox(
            PlayerIndex player,
            string title,
            string text,
            IEnumerable<string> buttons,
            int focusButton,
            MessageBoxIcon icon,
            AsyncCallback callback,
            Object state
            )
        {
            ShowMessageBoxDelegate smb = ShowMessageBox;

            return smb.BeginInvoke(title, text, buttons, focusButton, icon, callback, smb);
        }

        public static IAsyncResult BeginShowMessageBox(
            string title,
            string text,
            IEnumerable<string> buttons,
            int focusButton,
            MessageBoxIcon icon,
            AsyncCallback callback,
            Object state
            )
        {
            return BeginShowMessageBox(PlayerIndex.One, title, text, buttons, focusButton, icon, callback, state);
        }

        public static int? EndShowMessageBox(IAsyncResult result)
        {
            try
            {
                var smbd = (ShowMessageBoxDelegate) result.AsyncState;

                return smbd.EndInvoke(result);
            }
            finally
            {
                isVisible = false;
            }
        }


        public static void ShowMarketplace(PlayerIndex player)
        {
        }

        public static void Show()
        {
            ShowSignIn(1, false);
        }

        public static void ShowSignIn(int paneCount, bool onlineOnly)
        {
            if (paneCount != 1)
            {
                new ArgumentException("paneCount Can only be 1 on iPhone");
                return;
            }

            MonoGameGamerServicesHelper.ShowSigninSheet();

            if (GamerServicesComponent.LocalNetworkGamer == null)
            {
                GamerServicesComponent.LocalNetworkGamer = new LocalNetworkGamer();
            }
            else
            {
                GamerServicesComponent.LocalNetworkGamer.SignedInGamer.BeginAuthentication(null, null);
            }
        }

        public static void ShowLeaderboard()
        {
            if ((Gamer.SignedInGamers.Count > 0) && (Gamer.SignedInGamers[0].IsSignedInToLive))
            {
            }
        }

        public static void ShowAchievements()
        {
            if ((Gamer.SignedInGamers.Count > 0) && (Gamer.SignedInGamers[0].IsSignedInToLive))
            {
            }
        }

        public static void ShowPeerPicker()
        {
            if ((Gamer.SignedInGamers.Count > 0) && (Gamer.SignedInGamers[0].IsSignedInToLive))
            {
            }
        }


        public static void ShowMatchMaker()
        {
            if ((Gamer.SignedInGamers.Count > 0) && (Gamer.SignedInGamers[0].IsSignedInToLive))
            {
            }
        }

        public static IAsyncResult BeginShowStorageDeviceSelector(AsyncCallback callback, object state)
        {
            return null;
        }

        public static StorageDevice EndShowStorageDeviceSelector(IAsyncResult result)
        {
            return null;
        }

        #region Properties

        public static bool IsScreenSaverEnabled { get; set; }

        public static bool IsTrialMode { get; set; }

        public static bool IsVisible
        {
            get { return isVisible; }
            set { isVisible = value; }
        }

        public static bool SimulateTrialMode { get; set; }

        public static AndroidGameWindow Window { get; set; }

        #endregion

        private delegate string ShowKeyboardInputDelegate(
            PlayerIndex player,
            string title,
            string description,
            string defaultText,
            bool usePasswordMode);

        private delegate int? ShowMessageBoxDelegate(string title,
                                                     string text,
                                                     IEnumerable<string> buttons,
                                                     int focusButton,
                                                     MessageBoxIcon icon);
    }
}
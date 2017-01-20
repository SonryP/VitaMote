using System;
using System.Collections.Generic;
using Android.App;
using Android.OS;
using Android.Views;
using Android.Widget;

namespace VitaMote {

    [Activity(Label = "Custom Mapping")]
    public class CusMap : Activity {
        //KeyCodes
        Keycode [] dk = { Keycode.A, Keycode.B, Keycode.C, Keycode.D, Keycode.E, Keycode.F, Keycode.G, Keycode.H, Keycode.I, Keycode.J, Keycode.K, Keycode.L, Keycode.M, Keycode.N, Keycode.O, Keycode.P, Keycode.Q, Keycode.R, Keycode.S, Keycode.T, Keycode.U, Keycode.V, Keycode.W, Keycode.X, Keycode.Y, Keycode.Z, Keycode.AltLeft, Keycode.AltRight, Keycode.Apostrophe, Keycode.AppSwitch, Keycode.Assist, Keycode.At, Keycode.AvrInput, Keycode.AvrPower, Keycode.Back, Keycode.Backslash, Keycode.Bookmark, Keycode.Break, Keycode.BrightnessUp, Keycode.BrightnessDown, Keycode.Button1, Keycode.Button2, Keycode.Button3, Keycode.Button4, Keycode.Button5, Keycode.Button6, Keycode.Button7, Keycode.Button8, Keycode.Button9, Keycode.Button10, Keycode.Button11, Keycode.Button12, Keycode.Button13, Keycode.Button14, Keycode.Button15, Keycode.Button16, Keycode.ButtonA, Keycode.ButtonB, Keycode.ButtonC, Keycode.ButtonX, Keycode.ButtonY, Keycode.ButtonZ, Keycode.ButtonL1, Keycode.ButtonL2, Keycode.ButtonR1, Keycode.ButtonR2, Keycode.ButtonSelect, Keycode.ButtonStart, Keycode.ButtonMode, Keycode.ButtonThumbl, Keycode.ButtonThumbr, Keycode.Calculator, Keycode.Calendar, Keycode.Call, Keycode.Camera, Keycode.CapsLock, Keycode.Captions, Keycode.ChannelUp, Keycode.ChannelDown, Keycode.Clear, Keycode.Comma, Keycode.Contacts, Keycode.Copy, Keycode.CtrlLeft, Keycode.CtrlRight, Keycode.Cut, Keycode.Del, Keycode.DpadCenter, Keycode.DpadDownLeft, Keycode.DpadDownRight, Keycode.DpadUpLeft, Keycode.DpadUpRight, Keycode.DpadLeft, Keycode.DpadRight, Keycode.DpadUp, Keycode.DpadDown, Keycode.Dvr, Keycode.Eisu, Keycode.Endcall, Keycode.Enter, Keycode.Envelope, Keycode.Equals, Keycode.Escape, Keycode.Explorer, Keycode.F1, Keycode.F2, Keycode.F3, Keycode.F4, Keycode.F5, Keycode.F6, Keycode.F7, Keycode.F8, Keycode.F9, Keycode.F10, Keycode.F11, Keycode.F12, Keycode.Focus, Keycode.Forward, Keycode.ForwardDel, Keycode.Function, Keycode.Grave, Keycode.Guide, Keycode.Headsethook, Keycode.Help, Keycode.Henkan, Keycode.Home, Keycode.Info, Keycode.Insert, Keycode.K11, Keycode.K12, Keycode.Kana, Keycode.KatakanaHiragana, Keycode.LanguageSwitch, Keycode.LastChannel, Keycode.LeftBracket, Keycode.MannerMode, Keycode.MediaAudioTrack, Keycode.MediaClose, Keycode.MediaEject, Keycode.MediaFastForward, Keycode.MediaNext, Keycode.MediaPause, Keycode.MediaPlay, Keycode.MediaPlayPause, Keycode.MediaPrevious, Keycode.MediaRecord, Keycode.MediaRewind, Keycode.MediaStop, Keycode.Menu, Keycode.Minus, Keycode.Music, Keycode.Mute, Keycode.NavigateIn, Keycode.NavigateOut, Keycode.NavigatePrevious, Keycode.Notification, Keycode.Num, Keycode.Num0, Keycode.Num1, Keycode.Num2, Keycode.Num3, Keycode.Num4, Keycode.Num5, Keycode.Num6, Keycode.Num7, Keycode.Num8, Keycode.Num9, Keycode.NumLock, Keycode.Numpad0, Keycode.Numpad1, Keycode.Numpad2, Keycode.Numpad3, Keycode.Numpad4, Keycode.Numpad5, Keycode.Numpad6, Keycode.Numpad7, Keycode.Numpad8, Keycode.Numpad9, Keycode.NumpadAdd, Keycode.NumpadComma, Keycode.NumpadDivide, Keycode.NumpadDot, Keycode.NumpadEnter, Keycode.NumpadEquals, Keycode.NumpadMultiply, Keycode.NumpadSubtract, Keycode.NumpadLeftParen, Keycode.NumpadRightParen, Keycode.PageDown, Keycode.PageUp, Keycode.Pairing, Keycode.Paste, Keycode.Period, Keycode.Plus, Keycode.Pound, Keycode.Power, Keycode.ProgGreen, Keycode.ProgRed, Keycode.ProgBlue, Keycode.ProgYellow, Keycode.RightBracket, Keycode.Search, Keycode.Semicolon, Keycode.Settings, Keycode.ShiftLeft, Keycode.ShiftRight, Keycode.Slash, Keycode.Sleep, Keycode.Space, Keycode.Star, Keycode.Sym, Keycode.Sysrq, Keycode.Tab, Keycode.VolumeDown, Keycode.VolumeUp, Keycode.VolumeMute, Keycode.Wakeup, Keycode.Window, Keycode.ZoomIn, Keycode.ZoomOut };
        List<Keycode> allM = new List<Keycode>();
        private String [] dispKeys;
        Spinner s1;
        Spinner s2;
        Spinner s3;
        Spinner s4;
        Spinner s5;
        Spinner s6;
        Spinner s7;
        Spinner s8;
        Spinner s9;
        Spinner s10;
        Spinner s11;
        Spinner s12;
        Spinner s13;
        Spinner s14;
        Spinner s15;
        Spinner s16;
        Spinner s17;
        Spinner s18;
        Spinner s19;
        Spinner s20;
        protected override void OnCreate(Bundle savedInstanceState) {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.Mapping);
            //It's stupid, but.. there is no other form right now
            for (int i = 0; i < dk.Length; i++) {
                allM.Add(dk [i]);
            }
            //Key Strings (It was tedious to write them all >.<, there are some missed, but are not too interesting...) 
            this.dispKeys = new String[] {
            "A", "B", "C", "D", "E", "F", "G", "H", "I", "J", "K", "L", "M", "N", "O", "P", "Q", "R", "S", "T", "U", "V", "W", "X", "Y", "Z", "Alt Left", "Alt Right", "Apostrophe", "App Switch", "Assist", "At", "Avr Input", "Avr Power", "Back", "Backslash", "Bookmark", "Break", "Brightness Up", "Brightness Down", "Button 1", "Button 2", "Button 3", "Button 4", "Button 5", "Button 6", "Button 7", "Button 8", "Button 9", "Button 10", "Button 11", "Button 12", "Button 13", "Button 14", "Button 15", "Button 16", "Button A", "Button B", "Button C", "Button X", "Button Y", "Button Z", "Button L1", "Button L2", "Button R1", "Button R2", "Button Select", "Button Start", "Button Mode", "Button Thumb L", "Button Thumb R", "Calculator", "Calendar", "Call", "Camera", "Caps Lock", "Caption", "Channel Up", "Channel Down", "Clear", "Comma", "Contacts", "Copy", "Ctrl Left", "Ctrl Right", "Cut", "Del", "Dpad Center", "Dpad DownLeft", "Dpad DownRight", "Dpad UpLeft", "Dpad UpRight", "Dpad Left", "Dpad Right", "Dpad Up", "Dpad Down", "Dvr", "Eisu", "End Call", "Enter", "Envelope", "Equals", "Escape", "Explorer", "F1", "F2", "F3", "F4", "F5", "F6", "F7", "F8", "F9", "F10", "F11", "F12", "Focus", "Forward", "Forward Del", "Function", "Grave", "Guide", "Headset Hook", "Help", "Henkan", "Home", "Info", "Insert", "K11", "K12", "Kana", "Katakana Higragana", "Languaje Switch", "Last Channel", "Left Bracket", "Manner Mode", "Media Audio Track", "Media Close", "Media Eject", "Media Fast Forward", "Media Next", "Media Pause", "Media Play", "Media Play Pause", "Media Previous", "Media Record", "Media Rewind", "Media Stop", "Menu", "Minus", "Music", "Mute", "Navigate In", "Navigate Out", "Navigate Previous", "Notification", "Num", "Num 0 ", "Num 1", "Num 2", "Num 3", "Num 4", "Num 5", "Num 6", "Num 7", "Num 8", "Num 9", "Num Lock", "NumPad 0 ", "NumPad 1", "NumPad 2", "NumPad 3", "NumPad 4", "NumPad 5", "NumPad 6", "NumPad 7", "NumPad 8", "NumPad 9", "NumPad Add", "NumPad Comma", "NumPad Divide", "NumPad Dot", "NumPad Enter", "NumPad Equals", "NumPad Multiply", "NumPad Substract", "NumPad Left Parent", "NumPad Right Parent", "Page Down", "Page Up", "Pairing", "Paste", "Period", "Plus", "Pound", "Power", "Prog Green", "Prog Red", "Prog Blue", "Prog Yellow", "Right Bracket", "Search", "Semicolon", "Settings", "Shift Left", "Shift Right", "Slash", "Sleep", "Space", "Star (*)", "Sym", "SysRq", "Tab", "Volume Down", "Volume Up", "Volume Mute", "Wake Up", "Window", "Zoom In", "Zoom Out"
        };
            //Some piece of code, spinners, and buttons...
            s1 = (Spinner)FindViewById(Resource.Id.spinner1);
            s2 = (Spinner)FindViewById(Resource.Id.spinner2);
            s3 = (Spinner)FindViewById(Resource.Id.spinner3);
            s4 = (Spinner)FindViewById(Resource.Id.spinner4);
            s5 = (Spinner)FindViewById(Resource.Id.spinner5);
            s6 = (Spinner)FindViewById(Resource.Id.spinner6);
            s7 = (Spinner)FindViewById(Resource.Id.spinner7);
            s8 = (Spinner)FindViewById(Resource.Id.spinner8);
            s9 = (Spinner)FindViewById(Resource.Id.spinner9);
            s10 = (Spinner)FindViewById(Resource.Id.spinner10);
            s11 = (Spinner)FindViewById(Resource.Id.spinner11);
            s12 = (Spinner)FindViewById(Resource.Id.spinner12);
            s13 = (Spinner)FindViewById(Resource.Id.spinner13);
            s14 = (Spinner)FindViewById(Resource.Id.spinner14);
            s15 = (Spinner)FindViewById(Resource.Id.spinner15);
            s16 = (Spinner)FindViewById(Resource.Id.spinner16);
            s17 = (Spinner)FindViewById(Resource.Id.spinner17);
            s18 = (Spinner)FindViewById(Resource.Id.spinner18);
            s19 = (Spinner)FindViewById(Resource.Id.spinner19);
            s20 = (Spinner)FindViewById(Resource.Id.spinner20);
            Button butLo = (Button)FindViewById(Resource.Id.btnLoad);
            Button butSa = (Button)FindViewById(Resource.Id.btnSve);
            ArrayAdapter<String> adapter = new ArrayAdapter<String>(this,
                    Android.Resource.Layout.SimpleSpinnerItem, dispKeys);
            s1.Adapter = adapter;
            s2.Adapter = adapter;
            s3.Adapter = adapter;
            s4.Adapter = adapter;
            s5.Adapter = adapter;
            s6.Adapter = adapter;
            s7.Adapter = adapter;
            s8.Adapter = adapter;
            s9.Adapter = adapter;
            s10.Adapter = adapter;
            s11.Adapter = adapter;
            s12.Adapter = adapter;
            s13.Adapter = adapter;
            s14.Adapter = adapter;
            s15.Adapter = adapter;
            s16.Adapter = adapter;
            s17.Adapter = adapter;
            s18.Adapter = adapter;
            s19.Adapter = adapter;
            s20.Adapter = adapter;
            loadCM();
            butLo.Click += delegate {
                setDefaults();
            };
            butSa.Click += delegate { 
                saveCM();
            };
        }
        private void saveCM() {
            string cmtxt = (int)allM [(int)s1.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s2.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s3.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s4.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s5.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s6.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s7.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s8.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s9.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s10.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s11.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s12.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s13.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s14.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s15.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s16.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s17.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s18.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s19.SelectedItemId] + System.Environment.NewLine + (int)allM [(int)s20.SelectedItemId];
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath + "/SonryVitaMote");
            dir.Mkdirs();
            Java.IO.File file = new Java.IO.File(dir, "cm.scf");
            if (!file.Exists()) {
                file.CreateNewFile();
                file.Mkdir();
                Java.IO.FileWriter writer = new Java.IO.FileWriter(file);
                writer.Write(cmtxt);
                writer.Flush();
                writer.Close();
            } else {
                Java.IO.FileWriter writer = new Java.IO.FileWriter(file);
                writer.Write(cmtxt);
                writer.Flush();
                writer.Close();
                Toast.MakeText(this, "Successfully Saved", ToastLength.Long).Show();
            }
        }

        public void loadCM() {
            try {
                int l = 0;
                string line;
                Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
                Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath + "/SonryVitaMote");
                Java.IO.File file = new Java.IO.File(dir, "cm.scf");
                Java.IO.FileReader fread = new Java.IO.FileReader(file);
                Java.IO.BufferedReader br = new Java.IO.BufferedReader(fread);
                while ((line = br.ReadLine()) != null) {
                    switch (l) {
                        case 0:
                            s1.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 1:
                            s2.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 2:
                            s3.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 3:
                            s4.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 4:
                            s5.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 5:
                            s6.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 6:
                            s7.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 7:
                            s8.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 8:
                            s9.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 9:
                            s10.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 10:
                            s11.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 11:
                            s12.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 12:
                            s13.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 13:
                            s14.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 14:
                            s15.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 15:
                            s16.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 16:
                            s17.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 17:
                            s18.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 18:
                            s19.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                        case 19:
                            s20.SetSelection(allM.IndexOf((Keycode)Java.Lang.Integer.ParseInt(line)));
                            break;
                    }
                    l++;
                }
                fread.Close();
            }
            catch (Exception) {
                setDefaults();
                saveCM();
            }
        }
        private void setDefaults() {
            //Default Keys if you want to back
            s1.SetSelection(allM.IndexOf(Keycode.DpadUp));
            s2.SetSelection(allM.IndexOf(Keycode.DpadRight));
            s3.SetSelection(allM.IndexOf(Keycode.DpadDown));
            s4.SetSelection(allM.IndexOf(Keycode.DpadLeft));
            s5.SetSelection(allM.IndexOf(Keycode.ButtonL1));
            s6.SetSelection(allM.IndexOf(Keycode.ButtonR1));
            s7.SetSelection(allM.IndexOf(Keycode.ButtonA));
            s8.SetSelection(allM.IndexOf(Keycode.ButtonB));
            s9.SetSelection(allM.IndexOf(Keycode.ButtonY));
            s10.SetSelection(allM.IndexOf(Keycode.ButtonX));
            s11.SetSelection(allM.IndexOf(Keycode.DpadCenter));
            s12.SetSelection(allM.IndexOf(Keycode.Back));
            s13.SetSelection(allM.IndexOf(Keycode.W));
            s14.SetSelection(allM.IndexOf(Keycode.D));
            s15.SetSelection(allM.IndexOf(Keycode.S));
            s16.SetSelection(allM.IndexOf(Keycode.A));
            s17.SetSelection(allM.IndexOf(Keycode.I));
            s18.SetSelection(allM.IndexOf(Keycode.L));
            s19.SetSelection(allM.IndexOf(Keycode.K));
            s20.SetSelection(allM.IndexOf(Keycode.J));
        }

    }
}
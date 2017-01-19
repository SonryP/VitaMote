using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.InputMethodServices;
using static Android.InputMethodServices.KeyboardView;
using Java.Lang;
using Android.Views.InputMethods;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.IO;
using Android.Util;

namespace VitaMote {
    [Service(Label = "VitaIME", Permission = "android.permission.BIND_INPUT_METHOD")]
    [MetaData(name: "android.view.im", Resource = "@xml/method")]
    [IntentFilter(new [] { "android.view.InputMethod" })]
    class VitaIME : InputMethodService, IOnKeyboardActionListener {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        private KeyboardView kv;
        private Keyboard keyboard;
        private bool caps = false;
        IInputConnection ic;
        //Buttons
        int b1, b2, b3, b4, b5, b6, b7, b8,b9;
        //Type 1
        const int btnL = 128;
        const int btnR = 32;
        const int btnD = 64;
        const int btnU = 16;
        const int btnSel = 1;
        const int btnSta = 8;
        //Combos
        //Dpad Combos
        const int btnLU = btnL + btnU;
        const int btnLD = btnL + btnD;
        const int btnRU = btnR + btnU;
        const int btnRD = btnR + btnD;
        //Dpad + Sel or Sta
        const int btnUSt = btnU + btnSta;
        const int btnUSe = btnU + btnSel;
        const int btnDSt = btnD + btnSta;
        const int btnDSe = btnD + btnSel;
        const int btnLSt = btnL + btnSta;
        const int btnLSe = btnL + btnSel;
        const int btnRSt = btnR + btnSta;
        const int btnRSe = btnR + btnSel;
        const int btnSeSt = btnSel + btnSta;
        //Triple Combo (Dpad Combo + Sta)
        const int btnLUSt = btnLU + btnSta;
        const int btnLUSe = btnLU + btnSel;
        const int btnLDSt = btnLD + btnSta;
        const int btnLDSe = btnLD + btnSel;
        const int btnRUSt = btnRU + btnSta;
        const int btnRUSe = btnRU + btnSel;
        const int btnRDSt = btnRD + btnSta;
        const int btnRDSe = btnRD + btnSel;
        //Type 2
        const int btnX = 64;
        const int btnC = 32;
        const int btnT = 16;
        const int btnS = 128;
        const int btnLt = 1;
        const int btnRt = 2;
        //Double Combo
        const int btnXC = btnX + btnC;
        const int btnXS = btnX + btnS;
        const int btnCT = btnC + btnT;
        const int btnTS = btnT + btnS;
        const int btnXLt = btnX + btnLt;
        const int btnCLt = btnC + btnLt;
        const int btnTLt = btnT + btnLt;
        const int btnSLt = btnS + btnLt;
        const int btnXRt = btnX + btnRt;
        const int btnCRt = btnC + btnRt;
        const int btnTRt = btnT + btnRt;
        const int btnSRt = btnS + btnRt;
        const int btnLR = btnLt + btnRt;
        //Triple Combo (L+R+XTCS)
        const int btnLRC = btnLt + btnRt + btnC;
        const int btnLRS = btnLt + btnRt + btnS;
        const int btnLRT = btnLt + btnRt + btnT;
        const int btnLRX = btnLt + btnRt + btnX;
        //Keys (Custom Mapping Soon!)
        //DPAD
        Android.Views.Keycode bUp = Android.Views.Keycode.DpadUp;
        Android.Views.Keycode bDo = Android.Views.Keycode.DpadDown;
        Android.Views.Keycode bLe = Android.Views.Keycode.DpadLeft;
        Android.Views.Keycode bRi = Android.Views.Keycode.DpadRight;
        //L-R Triggers
        Android.Views.Keycode bLt = Android.Views.Keycode.ButtonL1;
        Android.Views.Keycode bRt = Android.Views.Keycode.ButtonR1;
        //XCTS
        Android.Views.Keycode bX = Android.Views.Keycode.ButtonA;
        Android.Views.Keycode bC = Android.Views.Keycode.ButtonB;
        Android.Views.Keycode bT = Android.Views.Keycode.ButtonY;
        Android.Views.Keycode bS = Android.Views.Keycode.ButtonX;
        //SEL-STA
        Android.Views.Keycode bSe = Android.Views.Keycode.DpadCenter;
        Android.Views.Keycode bSt = Android.Views.Keycode.Back;
        //Analogs
        //LEFT
        Android.Views.Keycode aLu = Android.Views.Keycode.W;
        Android.Views.Keycode aLd = Android.Views.Keycode.S;
        Android.Views.Keycode aLl = Android.Views.Keycode.A;
        Android.Views.Keycode aLr = Android.Views.Keycode.D;
        //RIGHT
        Android.Views.Keycode aRu = Android.Views.Keycode.I;
        Android.Views.Keycode aRd = Android.Views.Keycode.K;
        Android.Views.Keycode aRl = Android.Views.Keycode.J;
        Android.Views.Keycode aRr = Android.Views.Keycode.L;

        //Custom Mapping System
        public void loadCM() {
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
                        bUp = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 1:
                        bDo = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 2:
                        bLe = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 3:
                        bRi = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 4:
                        bLt = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 5:
                        bRt = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 6:
                        bX = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 7:
                        bC = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 8:
                        bT = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 9:
                        bS = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 10:
                        bSe = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 11:
                        bSt = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 12:
                        aLu = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 13:
                        aLd = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 14:
                        aLl = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 15:
                        aLr = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 16:
                        aRu = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 17:
                        aRd = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 18:
                        aRl = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                    case 19:
                        aRr = (Android.Views.Keycode)Integer.ParseInt(line);
                        break;
                }
                l++;
            }
            fread.Close();
            
        }


        bool timer = false;

        public void OnKey([GeneratedEnum] Android.Views.Keycode primaryCode, [GeneratedEnum] Android.Views.Keycode [] keyCodes) {
            IInputConnection ic = CurrentInputConnection;
            switch ((int)primaryCode) {
                case (int)Android.Views.Keycode.Del:
                    //ic.SendKeyEvent(new KeyEvent(KeyEventActions.Down,Android.Views.Keycode.Del));
                    ic.DeleteSurroundingText(1, 0);
                    break;
                case -1:
                    caps = !caps;
                    keyboard.SetShifted(caps);
                    kv.InvalidateAllKeys();
                    break;
                case (int)Android.Views.Keycode.Enter:
                    ic.SendKeyEvent(new KeyEvent(KeyEventActions.Down, Android.Views.Keycode.Enter));
                    break;
                case (int)Android.Views.Keycode.Button9:
                    try {
                        onREC();
                    }
                    catch (System.Exception ex) {
                        Toast.MakeText(this, "PSVITA Connected", ToastLength.Long).Show();
                        Log.Info("Exception: ",ex.ToString());
                    }

                    break;
                default:
                    char code = (char)primaryCode;
                    if (Character.IsLetter(code) && caps) {
                        code = Character.ToUpperCase(code);
                    }
                    ic.CommitText(Java.Lang.String.ValueOf(code), 1);
                    break;
            }
        }
        //This part of the code are useless, but there is no form to continue without this part of the code
        public void OnPress([GeneratedEnum] Android.Views.Keycode primaryCode) {
            //OnKey(primaryCode);
            //throw new NotImplementedException();
        }

        public void OnRelease([GeneratedEnum] Android.Views.Keycode primaryCode) {
            // throw new NotImplementedException();
        }

        public void OnText(ICharSequence text) {
            //  throw new NotImplementedException();
        }

        public void SwipeDown() {
            //   throw new NotImplementedException();
        }

        public void SwipeLeft() {
            //throw new NotImplementedException();
        }

        public void SwipeRight() {
            //throw new NotImplementedException();
        }

        public void SwipeUp() {
            //  throw new NotImplementedException();
        }
        public override void OnCreate() {
            base.OnCreate();
            loadCM();
            string ip = "0";
            timer = true;
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath + "/SonryVitaMote");
            Java.IO.File file = new Java.IO.File(dir, "ip.scf");
            Java.IO.FileReader fread = new Java.IO.FileReader(file);
            Java.IO.BufferedReader br = new Java.IO.BufferedReader(fread);
            ip = br.ReadLine();
            fread.Close();
            try {
                clientSocket.Connect(ip, 5000);
                if (clientSocket.Connected) {
                    Toast.MakeText(this, "PS VITA Connected", ToastLength.Long).Show();
                    RunUpdateLoop();
                } else {
                    Toast.MakeText(this, "Couldn't connect", ToastLength.Long).Show();
                }

            }
            catch (System.Exception ex) {
                Toast.MakeText(this, "Network Error, try again", ToastLength.Long).Show();
                Log.Info("Exception: ", ex.ToString());
            }


        }
        public void onREC() {
            string ip = "0";
            timer = true;
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath + "/SonryVitaMote");
            Java.IO.File file = new Java.IO.File(dir, "ip.scf");
            Java.IO.FileReader fread = new Java.IO.FileReader(file);
            Java.IO.BufferedReader br = new Java.IO.BufferedReader(fread);
            ip = br.ReadLine();
            fread.Close();
            try {
                clientSocket.Connect(ip, 5000);
                if (clientSocket.Connected) {
                    Toast.MakeText(this, "PS VITA Connected", ToastLength.Long).Show();
                    RunUpdateLoop();
                } else {
                    Toast.MakeText(this, "Couldn't Connect", ToastLength.Long).Show();
                }

            }
            catch (System.Exception ex) {
                Toast.MakeText(this, "Network Error, try again", ToastLength.Long).Show();
                Log.Info("Exception: ", ex.ToString());
            }
        }
        public override void OnDestroy() {
            base.OnDestroy();

        }
        public override View OnCreateInputView() {
            kv = (KeyboardView)LayoutInflater.Inflate(Resource.Layout.keyboard, null);
            keyboard = new Keyboard(this, Resource.Xml.qwerty);
            kv.Keyboard = keyboard;
            kv.OnKeyboardActionListener = this;
            return kv;
        }


        private async void RunUpdateLoop() {
            NetworkStream serverStream = clientSocket.GetStream();
            //int count = 1;
            while (timer) {
                try {
                    await Task.Delay(100);
                    byte [] outStream = System.Text.Encoding.ASCII.GetBytes("request");
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();
                    b1 = serverStream.ReadByte();//DPAD + SEL + STA
                    b2 = serverStream.ReadByte();//XCTS + LT + RT
                    b3 = serverStream.ReadByte();//NOT USED
                    b4 = serverStream.ReadByte();//NOT USED
                    b5 = serverStream.ReadByte();//L ANALOG X DATA
                    b6 = serverStream.ReadByte();//L ANALOG Y DATA
                    b7 = serverStream.ReadByte();//R ANALOG X DATA
                    b8 = serverStream.ReadByte();//R ANLAOG Y DATA
                    byte [] inStream = new byte [clientSocket.ReceiveBufferSize];
                    serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                    b9 = BitConverter.ToInt32(inStream, 0);//TOUCHSCREEN DATA (Not Used Yet)
                    keySystem(b1, b2, b3, b4, b5, b6, b7, b8);
                }
                catch (System.Exception ex) {
                    Toast.MakeText(this, "PSVITA Disconnected", ToastLength.Long).Show();
                    Log.Info("Exception: ", ex.ToString());
                    timer = false;
                }
            }
        }
        private void keySystem(int a, int b, int c, int d, int e, int f, int g, int h) {
            ic = CurrentInputConnection;
            //DPAD + SEL STA
            if (a == btnU) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bUp);
                ic.SendKeyEvent(ks);
            } else {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, bUp);
                ic.SendKeyEvent(ks);
            }
            if (a == btnR) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bRi);
                ic.SendKeyEvent(ks);
            } else {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, bRi);
                ic.SendKeyEvent(ks);
            }
            if (a == btnD) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bDo);
                ic.SendKeyEvent(ks);
            } else {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, bDo);
                ic.SendKeyEvent(ks);
            }
            if (a == btnL) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bLe);
                ic.SendKeyEvent(ks);
            } else {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, bLe);
                ic.SendKeyEvent(ks);
            }
            if (a == btnSel) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bSe);
                ic.SendKeyEvent(ks);
            } else {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, bSe);
                ic.SendKeyEvent(ks);
            }
            if (a == btnSta) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bSt);
                ic.SendKeyEvent(ks);
            } else {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, bSt);
                ic.SendKeyEvent(ks);
            }
            //Combos (Dpad & Dpad + Sel or Sta)
            if (a == btnLU) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bLe);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bUp);
                ic.SendKeyEvent(kd);
            }
            if (a == btnLD) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bLe);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bDo);
                ic.SendKeyEvent(kd);
            }
            if (a == btnRU) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bRi);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bUp);
                ic.SendKeyEvent(kd);
            }
            if (a == btnRD) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bRi);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bDo);
                ic.SendKeyEvent(kd);
            }
            if (a == btnUSt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bUp);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSt);
                ic.SendKeyEvent(kd);
            }
            if (a == btnUSe) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bSe);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bUp);
                ic.SendKeyEvent(kd);
            }
            if (a == btnDSt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bDo);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSt);
                ic.SendKeyEvent(kd);
            }
            if (a == btnDSe) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bDo);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSe);
                ic.SendKeyEvent(kd);
            }
            if (a == btnLSt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bLe);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSt);
                ic.SendKeyEvent(kd);
            }
            if (a == btnLSe) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bLe);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSe);
                ic.SendKeyEvent(kd);
            }
            if (a == btnRSt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bRi);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSt);
                ic.SendKeyEvent(kd);
            }
            if (a == btnRSe) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bRi);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSe);
                ic.SendKeyEvent(kd);
            }

            if (a == btnSeSt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bSe);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSt);
                ic.SendKeyEvent(kd);
            }
            //Triple (Dpad + Sel or Sta)
            if (a == btnLUSt) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bLe);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bSt);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bUp);
                ic.SendKeyEvent(kd);
            }
            if (a == btnLUSe) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bLe);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bSe);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bUp);
                ic.SendKeyEvent(kd);
            }
            if (a == btnLDSt) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bLe);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bDo);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSt);
                ic.SendKeyEvent(kd);
            }
            if (a == btnLDSe) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bLe);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bSe);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bDo);
                ic.SendKeyEvent(kd);
            }

            if (a == btnRUSt) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bRi);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bUp);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSt);
                ic.SendKeyEvent(kd);
            }
            if (a == btnRUSe) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bRi);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bUp);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSe);
                ic.SendKeyEvent(kd);
            }
            if (a == btnRDSt) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bRi);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bDo);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSt);
                ic.SendKeyEvent(kd);
            }
            if (a == btnRDSe) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bRi);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bDo);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bSe);
                ic.SendKeyEvent(kd);
            }

            //TCXS + L R
            if (b == 16) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bT);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, bT);
                ic.SendKeyEvent(ka);
            }
            if (b == 32) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bC);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, bC);
                ic.SendKeyEvent(ka);
            }
            if (b == 64) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bX);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, bX);
                ic.SendKeyEvent(ka);
            }
            if (b == 128) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bS);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, bS);
                ic.SendKeyEvent(ka);
            }
            if (b == 1) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bLt);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, bLt);
                ic.SendKeyEvent(ka);
            }
            if (b == 2) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bRt);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, bRt);
                ic.SendKeyEvent(ka);
            }
            if (b == btnXC) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bX);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bC);
                ic.SendKeyEvent(kd);
            }
            if (b == btnXS) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bX);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bS);
                ic.SendKeyEvent(kd);
            }
            if (b == btnCT) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bC);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bT);
                ic.SendKeyEvent(kd);
            }
            if (b == btnTS) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bT);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bS);
                ic.SendKeyEvent(kd);
            }
            if (b == btnXLt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bX);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bLt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnXRt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bX);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bRt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnCLt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bC);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bLt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnCRt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bC);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bRt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnTLt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bT);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bLt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnTRt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bT);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bRt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnSLt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bS);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bLt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnSRt) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bS);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bRt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnLR) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bLt);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bRt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnLRC) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bC);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bLt);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bRt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnLRT) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bT);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bLt);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bRt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnLRS) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bS);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bLt);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bRt);
                ic.SendKeyEvent(kd);
            }
            if (b == btnLRX) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, bX);
                ic.SendKeyEvent(ka);
                KeyEvent ks = new KeyEvent(KeyEventActions.Down, bLt);
                ic.SendKeyEvent(ks);
                KeyEvent kd = new KeyEvent(KeyEventActions.Down, bRt);
                ic.SendKeyEvent(kd);
            }
            //Analogs
            if (e <= 50 && e >= 0) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, aLl);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, aLl);
                ic.SendKeyEvent(ka);
            }
            if (e >= 200 && e <= 255) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, aLr);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, aLr);
                ic.SendKeyEvent(ka);
            }

            if (f <= 50 && f >= 0) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, aLu);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, aLu);
                ic.SendKeyEvent(ka);
            }
            if (f >= 200 && f <= 255) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, aLd);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, aLd);
                ic.SendKeyEvent(ka);
            }


            //Analogs
            if (g <= 50 && g >= 0) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, aRl);
                ic.SendKeyEvent(ka);

            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, aRl);
                ic.SendKeyEvent(ka);
            }
            if (g >= 200 && g <= 255) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, aRr);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, aRr);
                ic.SendKeyEvent(ka);
            }

            if (h <= 50 && g >= 0) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, aRu);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, aRu);
                ic.SendKeyEvent(ka);
            }
            if (h >= 200 && h <= 255) {
                KeyEvent ka = new KeyEvent(KeyEventActions.Down, aRd);
                ic.SendKeyEvent(ka);
            } else {
                KeyEvent ka = new KeyEvent(KeyEventActions.Up, aRd);
                ic.SendKeyEvent(ka);
            }
        }
     }
}
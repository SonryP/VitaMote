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

namespace VitaMote {
    [Service (Label = "VitaIME", Permission = "android.permission.BIND_INPUT_METHOD")]
    [MetaData (name:"android.view.im",Resource = "@xml/method")]
    [IntentFilter(new[] { "android.view.InputMethod" })]
    class VitaIME : InputMethodService, IOnKeyboardActionListener {
        System.Net.Sockets.TcpClient clientSocket = new System.Net.Sockets.TcpClient();
        int btn = 0;
        private KeyboardView kv;
        private Keyboard keyboard;
        private bool caps = false;
        IInputConnection ic;
        //Button Listing:
        const int btnDpadL = 128;
        const int btnDpadR = 32;
        const int btnDpadD = 64;
        const int btnDpadU = 16;
        const int btnX = 16384;
        const int btnC = 8192;
        const int btnT = 4096;
        const int btnS = 32768;
        const int btnL = 256;
        const int btnR = 512;
        const int btnSel = 1;
        const int btnSta = 8;
        //Status Check (Need to Replace that)
        bool P_btnDpadL = false;
        bool P_btnDpadR = false;
        bool P_btnDpadD = false;
        bool P_btnDpadU = false;
        bool P_btnX = false;
        bool P_btnC = false;
        bool P_btnT = false;
        bool P_btnS = false;
        bool P_btnL = false;
        bool P_btnR = false;
        bool P_btnSel = false;
        bool P_btnSta = false;

        bool timer = false;
        Android.Views.Keycode preBtnX = Android.Views.Keycode.A;
        Android.Views.Keycode preBtnY = Android.Views.Keycode.A;

        public void OnKey([GeneratedEnum] Android.Views.Keycode primaryCode, [GeneratedEnum] Android.Views.Keycode[] keyCodes) {
            IInputConnection ic = CurrentInputConnection;
            switch ((int)primaryCode) {
                case (int)Android.Views.Keycode.Del:
                    //ic.SendKeyEvent(new KeyEvent(KeyEventActions.Down,Android.Views.Keycode.Del));
                    ic.DeleteSurroundingText(1, 0);
                    break;
                case -1:
                    caps=!caps;
                    keyboard.SetShifted(caps);
                    kv.InvalidateAllKeys();
                    break;
                case (int)Android.Views.Keycode.Enter:
                    ic.SendKeyEvent(new KeyEvent(KeyEventActions.Down, Android.Views.Keycode.Enter));
                    break;
                case (int)Android.Views.Keycode.Button9:
                    try{
                        onREC();
                    }
                    catch(System.Exception ex) {
                        Toast.MakeText(this, "PSVITA Connected", ToastLength.Long).Show();
                    }
                    
                    break;
                default:
                    char code = (char)primaryCode;
                    if (Character.IsLetter(code)&&caps) {
                        code=Character.ToUpperCase(code);
                    }
                    ic.CommitText(Java.Lang.String.ValueOf(code), 1);
                    break;
            }
        }

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
            string ip = "0";
            timer=true;
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath+"/SonryVitaMote");
            Java.IO.File file = new Java.IO.File(dir, "ip.scf");
            Java.IO.FileReader fread = new Java.IO.FileReader(file);
            Java.IO.BufferedReader br = new Java.IO.BufferedReader(fread);
            ip=br.ReadLine();
            fread.Close();
            try{
                clientSocket.Connect(ip, 5000);
                if (clientSocket.Connected) {
                    Toast.MakeText(this, "PS VITA Connected", ToastLength.Long).Show();
                    RunUpdateLoop();
                }else {
                    Toast.MakeText(this, "Couldn't connect", ToastLength.Long).Show();
                }
               
            }
            catch(System.Exception ex) {
                Toast.MakeText(this, "Network Error, try again", ToastLength.Long).Show();
            }
            
            
        }
        public void onREC() {
            string ip = "0";
            timer=true;
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath+"/SonryVitaMote");
            Java.IO.File file = new Java.IO.File(dir, "ip.scf");
            Java.IO.FileReader fread = new Java.IO.FileReader(file);
            Java.IO.BufferedReader br = new Java.IO.BufferedReader(fread);
            ip=br.ReadLine();
            fread.Close();
            try {
                clientSocket.Connect(ip, 5000);
                if (clientSocket.Connected) {
                    Toast.MakeText(this, "PS VITA Connected", ToastLength.Long).Show();
                    RunUpdateLoop();
                }
                else {
                    Toast.MakeText(this, "Couldn't Connect", ToastLength.Long).Show();
                }

            }
            catch (System.Exception ex) {
                Toast.MakeText(this, "Network Error, try again", ToastLength.Long).Show();
            }
        }
        public override void OnDestroy() {
            base.OnDestroy();

        }
        public override View OnCreateInputView() {
            kv=(KeyboardView)LayoutInflater.Inflate(Resource.Layout.keyboard, null);
            keyboard=new Keyboard(this, Resource.Xml.qwerty);
            kv.Keyboard=keyboard;
            kv.OnKeyboardActionListener=this;
            return kv;
        }


        private async void RunUpdateLoop() {
            NetworkStream serverStream = clientSocket.GetStream();
            //int count = 1;
            while (timer) {
                try {
                    await Task.Delay(100);
                    byte[] outStream = System.Text.Encoding.ASCII.GetBytes("request");
                    serverStream.Write(outStream, 0, outStream.Length);
                    serverStream.Flush();

                    byte[] inStream = new byte[clientSocket.ReceiveBufferSize];
                    serverStream.Read(inStream, 0, (int)clientSocket.ReceiveBufferSize);
                    btn=BitConverter.ToInt32(inStream, 0);
                    fakeKeyboard(btn);
                }catch(System.Exception ex) {
                    Toast.MakeText(this, "PSVITA Disconnected", ToastLength.Long).Show();
                    timer=false;
                }
            }
        }
        int co = 0;
        private void fakeKeyboard(int keyCode) {
            upEventK();
            long eventTime = JavaSystem.CurrentTimeMillis();
            switch (keyCode) {
                case btnDpadU:
                    sEventK(Android.Views.Keycode.DpadUp);
                    P_btnDpadU=true;
                    break;
                case btnDpadR:
                    sEventK(Android.Views.Keycode.DpadRight);
                    P_btnDpadR=true;
                    break;
                case btnDpadD:
                    sEventK(Android.Views.Keycode.DpadDown);
                    P_btnDpadD=true;
                    break;
                case btnDpadL:
                    sEventK(Android.Views.Keycode.DpadLeft);
                    P_btnDpadL=true;
                    break;
                case btnL:
                    sEventK(Android.Views.Keycode.L);
                    P_btnL=true;
                    break;
                case btnR:
                    sEventK(Android.Views.Keycode.R);
                    P_btnR=true;
                    break;
                case btnX:
                    sEventK(Android.Views.Keycode.X);
                    P_btnX=true;
                    break;
                case btnC:
                    sEventK(Android.Views.Keycode.C);
                    P_btnC=true;
                    break;
                case btnT:
                    sEventK(Android.Views.Keycode.T);
                    P_btnT=true;
                    break;
                case btnS:
                    sEventK(Android.Views.Keycode.S);
                    P_btnS=true;
                    break;
                case btnSel:
                    sEventK(Android.Views.Keycode.DpadCenter);
                    P_btnSel=true;
                    break;
                case btnSta:
                    sEventK(Android.Views.Keycode.Back);
                    P_btnSta=true;
                    break;
                case btnDpadU+btnX:
                    sEventK(Android.Views.Keycode.DpadUp,Android.Views.Keycode.X);
                    P_btnDpadU=true;
                    P_btnX=true;
                    break;
                case btnDpadU+btnC:
                    sEventK(Android.Views.Keycode.DpadUp, Android.Views.Keycode.C);
                    P_btnDpadU=true;
                    P_btnC=true;
                    break;
                case btnDpadU+btnT:
                    sEventK(Android.Views.Keycode.DpadUp, Android.Views.Keycode.T);
                    P_btnDpadU=true;
                    P_btnT=true;
                    break;
                case btnDpadU+btnS:
                    sEventK(Android.Views.Keycode.DpadUp, Android.Views.Keycode.S);
                    P_btnDpadU=true;
                    P_btnS=true;
                    break;
                case btnDpadU+btnL:
                    sEventK(Android.Views.Keycode.DpadUp, Android.Views.Keycode.L);
                    P_btnDpadU=true;
                    P_btnL=true;
                    break;
                case btnDpadU+btnR:
                    sEventK(Android.Views.Keycode.DpadUp, Android.Views.Keycode.R);
                    P_btnDpadU=true;
                    P_btnR=true;
                    break;
                case btnDpadU+btnSel:
                    sEventK(Android.Views.Keycode.DpadUp, Android.Views.Keycode.DpadCenter);
                    P_btnDpadU=true;
                    P_btnSel=true;
                    break;
                case btnDpadU+btnSta:
                    sEventK(Android.Views.Keycode.DpadUp, Android.Views.Keycode.Escape);
                    P_btnDpadU=true;
                    P_btnSta=true;
                    break;
                case btnDpadR+btnX:
                    sEventK(Android.Views.Keycode.DpadRight, Android.Views.Keycode.X);
                    P_btnDpadR=true;
                    P_btnX=true;
                    break;
                case btnDpadR+btnC:
                    sEventK(Android.Views.Keycode.DpadRight, Android.Views.Keycode.C);
                    P_btnDpadR=true;
                    P_btnC=true;
                    break;
                case btnDpadR+btnT:
                    sEventK(Android.Views.Keycode.DpadRight, Android.Views.Keycode.T);
                    P_btnDpadR=true;
                    P_btnC=true;
                    break;
                case btnDpadR+btnS:
                    sEventK(Android.Views.Keycode.DpadRight, Android.Views.Keycode.S);
                    P_btnDpadR=true;
                    P_btnS=true;
                    break;
                case btnDpadR+btnL:
                    sEventK(Android.Views.Keycode.DpadRight, Android.Views.Keycode.L);
                    P_btnDpadR=true;
                    P_btnL=true;
                    break;
                case btnDpadR+btnR:
                    sEventK(Android.Views.Keycode.DpadRight, Android.Views.Keycode.R);
                    P_btnDpadR=true;
                    P_btnX=true;
                    break;
                case btnDpadR+btnSel:
                    sEventK(Android.Views.Keycode.DpadRight, Android.Views.Keycode.DpadCenter);
                    P_btnDpadR=true;
                    P_btnSel=true;
                    break;
                case btnDpadR+btnSta:
                    sEventK(Android.Views.Keycode.DpadRight, Android.Views.Keycode.Escape);
                    P_btnDpadR=true;
                    P_btnSta=true;
                    break;
                case btnDpadD+btnX:
                    sEventK(Android.Views.Keycode.DpadDown, Android.Views.Keycode.X);
                    P_btnDpadD=true;
                    P_btnX=true;
                    break;
                case btnDpadD+btnC:
                    sEventK(Android.Views.Keycode.DpadDown, Android.Views.Keycode.C);
                    P_btnDpadD=true;
                    P_btnC=true;
                    break;
                case btnDpadD+btnT:
                    sEventK(Android.Views.Keycode.DpadDown, Android.Views.Keycode.T);
                    P_btnDpadD=true;
                    P_btnT=true;
                    break;
                case btnDpadD+btnS:
                    sEventK(Android.Views.Keycode.DpadDown, Android.Views.Keycode.S);
                    P_btnDpadD=true;
                    P_btnT=true;
                    break;
                case btnDpadD+btnL:
                    sEventK(Android.Views.Keycode.DpadDown, Android.Views.Keycode.L);
                    P_btnDpadD=true;
                    P_btnL=true;
                    break;
                case btnDpadD+btnR:
                    sEventK(Android.Views.Keycode.DpadDown, Android.Views.Keycode.R);
                    P_btnDpadD=true;
                    P_btnR=true;
                    break;
                case btnDpadD+btnSel:
                    sEventK(Android.Views.Keycode.DpadDown, Android.Views.Keycode.DpadCenter);
                    P_btnDpadD=true;
                    P_btnSel=true;
                    break;
                case btnDpadD+btnSta:
                    sEventK(Android.Views.Keycode.DpadDown, Android.Views.Keycode.Escape);
                    P_btnDpadD=true;
                    P_btnSta=true;
                    break;
                case btnDpadL+btnX:
                    sEventK(Android.Views.Keycode.DpadLeft, Android.Views.Keycode.X);
                    P_btnDpadL=true;
                    P_btnX=true;
                    break;
                case btnDpadL+btnC:
                    sEventK(Android.Views.Keycode.DpadLeft, Android.Views.Keycode.C);
                    P_btnDpadL=true;
                    P_btnC=true;
                    break;
                case btnDpadL+btnT:
                    sEventK(Android.Views.Keycode.DpadLeft, Android.Views.Keycode.T);
                    P_btnDpadL=true;
                    P_btnT=true;
                    break;
                case btnDpadL+btnS:
                    sEventK(Android.Views.Keycode.DpadLeft, Android.Views.Keycode.S);
                    P_btnDpadL=true;
                    P_btnS=true;
                    break;
                case btnDpadL+btnL:
                    sEventK(Android.Views.Keycode.DpadLeft, Android.Views.Keycode.L);
                    P_btnDpadL=true;
                    P_btnL=true;
                    break;
                case btnDpadL+btnR:
                    sEventK(Android.Views.Keycode.DpadLeft, Android.Views.Keycode.R);
                    P_btnDpadL=true;
                    P_btnR=true;
                    break;
                case btnDpadL+btnSel:
                    sEventK(Android.Views.Keycode.DpadLeft, Android.Views.Keycode.DpadCenter);
                    P_btnDpadL=true;
                    P_btnSel=true;
                    break;
                case btnDpadL+btnSta:
                    sEventK(Android.Views.Keycode.DpadLeft, Android.Views.Keycode.Escape);
                    P_btnDpadL=true;
                    P_btnSta=true;
                    break;
                //Extras
                case btnDpadU+btnDpadL:
                    sEventK(Android.Views.Keycode.DpadUp, Android.Views.Keycode.DpadLeft);
                    P_btnDpadU=true;
                    P_btnDpadL=true;
                    break;
                case btnDpadU+btnDpadR:
                    sEventK(Android.Views.Keycode.DpadUp, Android.Views.Keycode.DpadRight);
                    P_btnDpadU=true;
                    P_btnDpadR=true;
                    break;
                case btnDpadD+btnDpadL:
                    sEventK(Android.Views.Keycode.DpadDown, Android.Views.Keycode.DpadLeft);
                    P_btnDpadD=true;
                    P_btnDpadL=true;
                    break;
                case btnDpadD+btnDpadR:
                    sEventK(Android.Views.Keycode.DpadDown, Android.Views.Keycode.DpadRight);
                    P_btnDpadD=true;
                    P_btnDpadR=true;
                    break;
                default:
                    upEventK();
                    break;
            }

        }
        
        private void upEventK() {
            ic=CurrentInputConnection;
            if (P_btnDpadU==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.DpadUp);
                ic.SendKeyEvent(ks);
                P_btnDpadU=false;
            }
            if (P_btnDpadR==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.DpadRight);
                ic.SendKeyEvent(ks);
                P_btnDpadR=false;
            }
            if (P_btnDpadD==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.DpadDown);
                ic.SendKeyEvent(ks);
                P_btnDpadD=false;
            }
            if (P_btnDpadL==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.DpadLeft);
                ic.SendKeyEvent(ks);
                P_btnDpadL=false;
            }
            if (P_btnX==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.X);
                ic.SendKeyEvent(ks);
                P_btnX=false;
            }
            if (P_btnT==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.T);
                ic.SendKeyEvent(ks);
                P_btnT=false;
            }
            if (P_btnC==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.C);
                ic.SendKeyEvent(ks);
                P_btnC=false;
            }
            if (P_btnS==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.S);
                ic.SendKeyEvent(ks);
                P_btnS=false;
            }
            if (P_btnL==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.L);
                ic.SendKeyEvent(ks);
                P_btnL=false;
            }
            if (P_btnR==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.R);
                ic.SendKeyEvent(ks);
                P_btnR=false;
            }
            if (P_btnSel==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.DpadCenter);
                ic.SendKeyEvent(ks);
                P_btnSel=false;
            }
            if (P_btnSta==true) {
                KeyEvent ks = new KeyEvent(KeyEventActions.Up, Android.Views.Keycode.Escape);
                ic.SendKeyEvent(ks);
                P_btnSta=false;
            }
        }
        private void sEventK(Android.Views.Keycode kc) {
            co=1;
            preBtnX=kc;
            ic=CurrentInputConnection;
            long eventTime = JavaSystem.CurrentTimeMillis();
            KeyEvent ks = new KeyEvent(KeyEventActions.Down, kc);
            ic.SendKeyEvent(ks);
        }
        private void sEventK(Android.Views.Keycode kc, Android.Views.Keycode kd) {
            co=3;
            preBtnX=kc;
            preBtnY=kd;
            ic = CurrentInputConnection;
            long eventTime = JavaSystem.CurrentTimeMillis();
            KeyEvent kx = new KeyEvent(KeyEventActions.Down, kc);
            KeyEvent ky = new KeyEvent(KeyEventActions.Down, kd);
            ic.SendKeyEvent(kx);
            ic.SendKeyEvent(ky);
        }

    }
}
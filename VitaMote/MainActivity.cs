using System;
using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;

namespace VitaMote {
    [Activity(Label = "VitaMote", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {
        TextView label1;
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            // Get our button from the layout resource,
            // and attach an event to it
            Button button = FindViewById<Button>(Resource.Id.MyButton);
            Button btnime = FindViewById<Button>(Resource.Id.btnIME);
            EditText text1 = FindViewById<EditText>(Resource.Id.editText1);
            label1 = FindViewById<TextView>(Resource.Id.textView7);
            label1.Text=reFile();
            button.Click+=delegate {
                saveFl(text1.Text); //button.Text=string.Format("{0} clicks xD!", count++);
                label1.Text=reFile();
            };
            btnime.Click += delegate {
                InputMethodManager imeManager = (InputMethodManager)GetSystemService(InputMethodService);
                imeManager.ShowInputMethodPicker();
            };
        }

        private void saveFl(string texto) {
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath+"/SonryVitaMote");
            dir.Mkdirs();
            Java.IO.File file = new Java.IO.File(dir, "ip.scf");
            if (!file.Exists()) {
                file.CreateNewFile();
                file.Mkdir();
                Java.IO.FileWriter writer = new Java.IO.FileWriter(file);
                writer.Write(texto);
                writer.Flush();
                writer.Close();
                Toast.MakeText(this, "Successfully Saved", ToastLength.Long).Show();
            }
            else {
                Java.IO.FileWriter writer = new Java.IO.FileWriter(file);
                writer.Write(texto);
                writer.Flush();
                writer.Close();
                Toast.MakeText(this, "Successfully Edited", ToastLength.Long).Show();
            }
        }
        private string reFile() {
            string ip = "0";
            Java.IO.File sdCard = Android.OS.Environment.ExternalStorageDirectory;
            Java.IO.File dir = new Java.IO.File(sdCard.AbsolutePath+"/SonryVitaMote");
            Java.IO.File file = new Java.IO.File(dir, "ip.scf");
            if (!file.Exists()) {
                Toast.MakeText(this, "Remember to store an IP", ToastLength.Long).Show();
                return "There is NO IP Stored";
            }
            else {
                Java.IO.FileReader fread = new Java.IO.FileReader(file);
                Java.IO.BufferedReader br = new Java.IO.BufferedReader(fread);
                ip=br.ReadLine();
                fread.Close();
                return ip;
            }
               
        }
    }
}


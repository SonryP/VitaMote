using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views.InputMethods;

namespace VitaMote {
    [Activity(Label = "VitaMote", MainLauncher = true, Icon = "@drawable/icon")]
    public class MainActivity : Activity {
        TextView label1;
        protected override void OnCreate(Bundle bundle) {
            base.OnCreate(bundle);
            
            SetContentView(Resource.Layout.Main);

            Button button = FindViewById<Button>(Resource.Id.MyButton);
            Button btnime = FindViewById<Button>(Resource.Id.btnIME);
            Button btnmap = FindViewById<Button>(Resource.Id.btnMap);
            EditText text1 = FindViewById<EditText>(Resource.Id.editText1);
            label1 = FindViewById<TextView>(Resource.Id.textView7);
            label1.Text=reFile();
            button.Click+=delegate {
                saveFl(text1.Text);
                label1.Text=reFile();
            };
            btnime.Click += delegate {
                InputMethodManager imeManager = (InputMethodManager)GetSystemService(InputMethodService);
                imeManager.ShowInputMethodPicker();
            };
            btnmap.Click += delegate {
                StartActivity(typeof(CusMap));           
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
                return "No IP Saved";
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


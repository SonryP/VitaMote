package cl.sonry.vitamote.common;

import static android.content.Context.INPUT_METHOD_SERVICE;

import android.content.Context;
import android.view.inputmethod.InputMethodManager;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
public class Utils  {

    public static void saveFile(String text, Context context) {
        File dir = context.getExternalFilesDir("SonryVitaMote");
        dir.mkdirs();

        File file = new File(dir, "ip.scf");

        try {
            if (!file.exists()) {
                file.createNewFile();
            }

            FileWriter writer = new FileWriter(file);
            writer.write(text);
            writer.flush();
            writer.close();

            Toast.makeText(context, "Successfully Saved", Toast.LENGTH_LONG).show();
        } catch (IOException e) {
            e.printStackTrace();
            Toast.makeText(context, "Error saving file", Toast.LENGTH_LONG).show();
        }
    }

    public static String readFile(Context context) {
        String ip = "0";
        File dir = context.getExternalFilesDir("SonryVitaMote");
        File file = new File(dir, "ip.scf");

        if (!file.exists()) {
            Toast.makeText(context, "Remember to store an IP", Toast.LENGTH_LONG).show();
            return "No IP Saved";
        } else {
            try {
                FileReader fread = new FileReader(file);
                BufferedReader br = new BufferedReader(fread);
                ip = br.readLine();
                fread.close();
            } catch (IOException e) {
                e.printStackTrace();
                return "Error reading IP";
            }
            return ip;
        }
    }

    public static void changeIme(Context context){
        InputMethodManager imeManager = (InputMethodManager) context.getSystemService(INPUT_METHOD_SERVICE);
        if (imeManager != null) {
            imeManager.showInputMethodPicker();
        }
    }
}

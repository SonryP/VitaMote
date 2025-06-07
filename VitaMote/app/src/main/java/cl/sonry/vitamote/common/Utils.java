package cl.sonry.vitamote.common;

import static android.content.Context.INPUT_METHOD_SERVICE;
import android.content.Context;
import android.content.Intent;
import android.provider.Settings;
import android.util.Log;
import android.view.KeyEvent;
import android.view.inputmethod.InputMethodManager;
import android.widget.Toast;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.Arrays;
import java.util.List;

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
    public static void enableIme(Context context){
        Intent intent = new Intent(Settings.ACTION_INPUT_METHOD_SETTINGS);
        intent.setFlags(Intent.FLAG_ACTIVITY_NEW_TASK);
        context.startActivity(intent);

    }
    public static List<Integer> defaultKeyCodes = Arrays.asList(
            KeyEvent.KEYCODE_DPAD_CENTER,
            KeyEvent.KEYCODE_BACK,
            KeyEvent.KEYCODE_DPAD_UP,
            KeyEvent.KEYCODE_DPAD_RIGHT,
            KeyEvent.KEYCODE_DPAD_DOWN,
            KeyEvent.KEYCODE_DPAD_LEFT,
            KeyEvent.KEYCODE_BUTTON_L2,
            KeyEvent.KEYCODE_BUTTON_R2,
            KeyEvent.KEYCODE_BUTTON_Y,
            KeyEvent.KEYCODE_BUTTON_B,
            KeyEvent.KEYCODE_BUTTON_X,
            KeyEvent.KEYCODE_BUTTON_A,
            KeyEvent.KEYCODE_W,
            KeyEvent.KEYCODE_D,
            KeyEvent.KEYCODE_S,
            KeyEvent.KEYCODE_A,
            KeyEvent.KEYCODE_I,
            KeyEvent.KEYCODE_L,
            KeyEvent.KEYCODE_K,
            KeyEvent.KEYCODE_J
    );
    public static List<Integer> loadCM(Context context) {
        try {
            File dir = context.getExternalFilesDir("SonryVitaMote");
            File file = new File(dir, "cm.scf");
            Integer[] keys = new Integer[20];
            try (BufferedReader br = new BufferedReader(new FileReader(file))) {
                String line;
                int l = 0;
                while ((line = br.readLine()) != null && l < 20) {
                    try {
                        int keyCode = Integer.parseInt(line.trim());
                        keys[l] = keyCode;
                    } catch (NumberFormatException e) {
                        Log.e("loadCM", "Invalid number format on line " + l, e);
                    }
                    l++;
                }
            }
            return new ArrayList<>(Arrays.asList(keys));
        } catch (IOException ex) {
            Log.e("loadCM", "Error reading custom mapping file", ex);
            return null;
        }
    }
}

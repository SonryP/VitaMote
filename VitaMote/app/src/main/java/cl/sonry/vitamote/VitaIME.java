package cl.sonry.vitamote;

import android.inputmethodservice.InputMethodService;
import android.inputmethodservice.Keyboard;
import android.inputmethodservice.KeyboardView;
import android.os.Environment;
import android.os.StrictMode;
import android.util.Log;
import android.view.KeyEvent;
import android.view.View;
import android.view.inputmethod.InputConnection;
import android.widget.Button;
import android.widget.Toast;

import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.InputStream;
import java.net.Socket;

public class VitaIME  extends InputMethodService {
    private boolean caps = false;
    private InputConnection ic;
    private Socket clientSocket;
    private boolean timer = false;

    int b1, b2, b3, b4, b5, b6, b7, b8,b9;
    //Type 1
    int btnL = 128;
    int btnR = 32;
    int btnD = 64;
    int btnU = 16;
    int btnSel = 1;
    int btnSta = 8;



    @Override
    public void onCreate() {
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder()
                .permitAll().build();
        StrictMode.setThreadPolicy(policy);
        super.onCreate();
        loadCustomMapping();
        connectToVita();
    }

    @Override
    public View onCreateInputView() {
        View view = getLayoutInflater().inflate(R.layout.custom_keyboard, null);

        ic = getCurrentInputConnection();

        Button keyQ = view.findViewById(R.id.key_q);
        Button keyW = view.findViewById(R.id.key_w);

        keyQ.setOnClickListener(v -> sendKey('q'));
        keyW.setOnClickListener(v -> sendKey('w'));

        return view;
    }

    private void sendKey(char c) {
        if (ic != null) {
            ic.commitText(String.valueOf(c), 1);
        }
    }

    private void connectToVita() {
        File dir = getExternalFilesDir("SonryVitaMote");
        File ipFile = new File(dir, "ip.scf");
        try (BufferedReader br = new BufferedReader(new FileReader(ipFile))) {
            String ip = br.readLine();
            clientSocket = new Socket(ip, 5000);
            Toast.makeText(this, "PS VITA Connected", Toast.LENGTH_LONG).show();
            timer = true;
            new Thread(this::runUpdateLoop).start();
        } catch (Exception ex) {
            Toast.makeText(this, "Connection failed", Toast.LENGTH_LONG).show();
            Log.e("VitaIME", "Connection error", ex);
        }
    }

    private void runUpdateLoop() {

        try (BufferedOutputStream out = new BufferedOutputStream(clientSocket.getOutputStream());
             InputStream in = clientSocket.getInputStream()) {

            while (timer) {
                Thread.sleep(100);
                out.write("request".getBytes());
                out.flush();

                b1 = in.read();  // Dpad
                b2 = in.read();  // Buttons
                // TODO: Do a better and cleaner job here
                byte [] inStream = new byte [clientSocket.getReceiveBufferSize()];
                in.read(inStream, 0, (int)clientSocket.getReceiveBufferSize());
                testKeys(b1);
            }

        } catch (Exception ex) {
            Log.e("VitaIME", "Disconnected", ex);
            runOnUiThread(() -> Toast.makeText(this, "Disconnected", Toast.LENGTH_LONG).show());
        }
    }
    private void runOnUiThread(Runnable runnable) {
        new android.os.Handler(getMainLooper()).post(runnable);
    }

    private void testKeys(int a){
        ic = getCurrentInputConnection();
        if (a == btnU) {
            KeyEvent ks = new KeyEvent(0,KeyEvent.KEYCODE_DPAD_UP);
            ic.sendKeyEvent(ks);
        }
    }

    private void loadCustomMapping() {
        // Read cm.scf and assign values like KeyEvent.KEYCODE_DPAD_UP etc.
    }

}

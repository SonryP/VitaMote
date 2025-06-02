package cl.sonry.vitamote;

import android.inputmethodservice.InputMethodService;
import android.inputmethodservice.Keyboard;
import android.inputmethodservice.KeyboardView;
import android.os.Environment;
import android.os.StrictMode;
import android.util.Log;
import android.view.KeyEvent;
import android.view.MotionEvent;
import android.view.View;
import android.view.inputmethod.InputConnection;
import android.widget.Button;
import android.widget.Toast;

import java.io.BufferedOutputStream;
import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.InputStream;
import java.lang.reflect.Type;
import java.net.Socket;
import java.util.ArrayList;
import java.util.EnumSet;
import java.util.HashMap;
import java.util.List;
import java.util.Map;
import java.util.function.Function;

import cl.sonry.vitamote.enums.TypeA;
import cl.sonry.vitamote.enums.TypeB;

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

    HashMap<TypeA, Integer> typeAButtons = new HashMap<>();
    HashMap<TypeB, Integer> typeBButtons = new HashMap<>();


    @Override
    public void onCreate() {
        StrictMode.ThreadPolicy policy = new StrictMode.ThreadPolicy.Builder()
                .permitAll().build();
        StrictMode.setThreadPolicy(policy);
        super.onCreate();
        initializeButtons();
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
                b3 = in.read();  // Not Used
                b4 = in.read();  // Not Used
                b5 = in.read();  // L Analog X
                b6 = in.read();  // L Analog Y
                b7 = in.read();  // R Analog X
                b8 = in.read();  // R Analog Y

                // TODO: Do a better and cleaner job here
                byte [] inStream = new byte [clientSocket.getReceiveBufferSize()];
                in.read(inStream, 0, (int)clientSocket.getReceiveBufferSize());

                sendType(b1,TypeA::fromCombinedValue, typeAButtons, lastKeyA);
                sendType(b2, TypeB::fromCombinedValue, typeBButtons, lastKeyB);
            }

        } catch (Exception ex) {
            Log.e("VitaIME", "Disconnected", ex);
            runOnUiThread(() -> Toast.makeText(this, "Disconnected", Toast.LENGTH_LONG).show());
            timer=false;
        }
    }
    private void runOnUiThread(Runnable runnable) {
        new android.os.Handler(getMainLooper()).post(runnable);
    }

    ArrayList<Integer> lastKeyA =new ArrayList<>();
    ArrayList<Integer> lastKeyB =new ArrayList<>();
    private <T extends Enum<T>> void sendType(
            int a,
            Function<Integer, EnumSet<T>> fromCombinedValue,
            Map<T, Integer> buttonMap,
            List<Integer> lastKeyList
    ) {
        ic = getCurrentInputConnection();

        if (a != 0) {
            EnumSet<T> buttons = fromCombinedValue.apply(a);
            for (T button : buttons) {
                if (buttonMap.containsKey(button)) {
                    int keyCode = buttonMap.getOrDefault(button, 0);
                    KeyEvent ks = new KeyEvent(0, keyCode);
                    ic.sendKeyEvent(ks);
                    lastKeyList.add(keyCode);
                }
            }
        } else if (!lastKeyList.isEmpty()) {
            for (Integer keyCode : lastKeyList) {
                KeyEvent ks = new KeyEvent(1, keyCode);
                ic.sendKeyEvent(ks);
            }
            lastKeyList.clear();
        }
    }

    private void loadCustomMapping() {
        // Read cm.scf and assign values like KeyEvent.KEYCODE_DPAD_UP etc.
    }

    private void initializeButtons(){
        typeAButtons.put(TypeA.UP, KeyEvent.KEYCODE_DPAD_UP);
        typeAButtons.put(TypeA.RIGHT, KeyEvent.KEYCODE_DPAD_RIGHT);
        typeAButtons.put(TypeA.DOWN, KeyEvent.KEYCODE_DPAD_DOWN);
        typeAButtons.put(TypeA.LEFT, KeyEvent.KEYCODE_DPAD_LEFT);
        typeAButtons.put(TypeA.SELECT, KeyEvent.KEYCODE_DPAD_CENTER);
        typeAButtons.put(TypeA.START, KeyEvent.KEYCODE_BACK);
        typeBButtons.put(TypeB.LT,KeyEvent.KEYCODE_BUTTON_L2);
        typeBButtons.put(TypeB.RT,KeyEvent.KEYCODE_BUTTON_R2);
        typeBButtons.put(TypeB.T,KeyEvent.KEYCODE_BUTTON_Y);
        typeBButtons.put(TypeB.C,KeyEvent.KEYCODE_BUTTON_B);
        typeBButtons.put(TypeB.S,KeyEvent.KEYCODE_BUTTON_X);
        typeBButtons.put(TypeB.X,KeyEvent.KEYCODE_BUTTON_A);
    }

}

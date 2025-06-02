package cl.sonry.vitamote;

import android.os.Bundle;
import android.view.View;
import android.view.inputmethod.InputMethodManager;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;

public class MainActivity extends AppCompatActivity {
    TextView ipText;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);

        Button sveBtn = findViewById(R.id.save_button);
        Button btnIme = findViewById(R.id.btnIME);
        Button btnMap = findViewById(R.id.btnMap);
        EditText ipInput = findViewById(R.id.ip_input);
        ipText = findViewById(R.id.ip_text);
        ipText.setText(readFile());

        sveBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                saveFile(ipInput.getText().toString());
                ipText.setText(readFile());
            }
        });

        btnIme.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                InputMethodManager imeManager = (InputMethodManager) getSystemService(INPUT_METHOD_SERVICE);
                if (imeManager != null) {
                    imeManager.showInputMethodPicker();
                }
            }
        });

        btnMap.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                Toast.makeText(MainActivity.this,  "Not implemented on this version", Toast.LENGTH_LONG).show();
            }
        });



        ViewCompat.setOnApplyWindowInsetsListener(findViewById(R.id.main), (v, insets) -> {
            Insets systemBars = insets.getInsets(WindowInsetsCompat.Type.systemBars());
            v.setPadding(systemBars.left, systemBars.top, systemBars.right, systemBars.bottom);
            return insets;
        });
    }

    private void saveFile(String texto) {
        File dir = getExternalFilesDir("SonryVitaMote");
        dir.mkdirs();

        File file = new File(dir, "ip.scf");

        try {
            if (!file.exists()) {
                file.createNewFile();
            }

            FileWriter writer = new FileWriter(file);
            writer.write(texto);
            writer.flush();
            writer.close();

            Toast.makeText(this, "Successfully Saved", Toast.LENGTH_LONG).show();
        } catch (IOException e) {
            e.printStackTrace();
            Toast.makeText(this, "Error saving file", Toast.LENGTH_LONG).show();
        }
    }

    private String readFile() {
        String ip = "0";
        File dir = getExternalFilesDir("SonryVitaMote");
        File file = new File(dir, "ip.scf");

        if (!file.exists()) {
            Toast.makeText(this, "Remember to store an IP", Toast.LENGTH_LONG).show();
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
}
package cl.sonry.vitamote;

import static cl.sonry.vitamote.common.Utils.*;

import android.os.Bundle;
import android.view.View;
import android.widget.Button;
import android.widget.EditText;
import android.widget.TextView;
import android.widget.Toast;

import androidx.activity.EdgeToEdge;
import androidx.appcompat.app.AppCompatActivity;
import androidx.core.graphics.Insets;
import androidx.core.view.ViewCompat;
import androidx.core.view.WindowInsetsCompat;



public class MainActivity extends AppCompatActivity {
    TextView ipText;
    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        EdgeToEdge.enable(this);
        setContentView(R.layout.activity_main);

        Button sveBtn = findViewById(R.id.save_button);
        Button btnEnime = findViewById(R.id.btnEnime);
        Button btnIme = findViewById(R.id.btnIME);
        Button btnMap = findViewById(R.id.btnMap);
        EditText ipInput = findViewById(R.id.ip_input);
        ipText = findViewById(R.id.ip_text);
        ipText.setText(readFile(MainActivity.this));

        sveBtn.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                saveFile(ipInput.getText().toString(), MainActivity.this);
                ipText.setText(readFile(MainActivity.this));
            }
        });

        btnEnime.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                enableIme(MainActivity.this);
            }
        });

        btnIme.setOnClickListener(new View.OnClickListener() {
            @Override
            public void onClick(View v) {
                changeIme(MainActivity.this);
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




}
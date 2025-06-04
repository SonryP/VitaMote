package cl.sonry.vitamote;


import static cl.sonry.vitamote.common.Utils.defaultKeyCodes;

import android.app.Activity;
import android.os.Bundle;
import android.widget.ArrayAdapter;
import android.widget.Button;
import android.widget.Spinner;
import android.widget.Toast;

import java.io.BufferedReader;
import java.io.File;
import java.io.FileReader;
import java.io.FileWriter;
import java.io.IOException;
import java.util.ArrayList;
import java.util.List;

import cl.sonry.vitamote.helpers.KeyCodeHelper;

public class CusMap extends Activity {
    private Spinner[] spinners = new Spinner[20];
    private Button btnSave;
    private Button btnLoad;
    private List<String> keyNames = new ArrayList<>();
    private List<Integer> keyCodes = new ArrayList<>();

    @Override
    protected void onCreate(Bundle savedInstanceState) {
        super.onCreate(savedInstanceState);
        setContentView(R.layout.mapping);


        for (KeyCodeHelper.KeyCodeEntry entry : KeyCodeHelper.getAllKeyCodeEntries()) {
            keyCodes.add(entry.code);
            keyNames.add(entry.name);
        }

        for (int i = 0; i < spinners.length; i++) {
            int resId = getResources().getIdentifier("spinner" + (i + 1), "id", getPackageName());
            spinners[i] = findViewById(resId);
            ArrayAdapter<String> adapter = new ArrayAdapter<>(this, android.R.layout.simple_spinner_item, keyNames);
            adapter.setDropDownViewResource(android.R.layout.simple_spinner_dropdown_item);
            spinners[i].setAdapter(adapter);
        }

        btnLoad = findViewById(R.id.btnLoad);
        btnSave = findViewById(R.id.btnSave);
        btnLoad.setOnClickListener(v -> setDefaults());
        btnSave.setOnClickListener(v -> saveMapping());
        loadMapping();
    }

    private void saveMapping() {
        File dir = this.getExternalFilesDir("SonryVitaMote");
        File file = new File(dir, "cm.scf");
        try {
            if (!file.exists()) {
                file.createNewFile();
            }

            FileWriter writer = new FileWriter(file);
            for (Spinner spinner : spinners) {
                int selectedIndex = spinner.getSelectedItemPosition();
                int keycode = keyCodes.get(selectedIndex);
                writer.write((keycode + System.lineSeparator()));
            }
            writer.flush();
            writer.close();
            Toast.makeText(this, "Successfully Saved", Toast.LENGTH_LONG).show();
        } catch (IOException e) {
            Toast.makeText(this, "Error saving file", Toast.LENGTH_LONG).show();
        }
    }

    private void loadMapping() {
        try {
            System.out.println("Loading mapping...");
            File dir = getExternalFilesDir("SonryVitaMote");
            File file = new File(dir, "cm.scf");
            BufferedReader br = new BufferedReader(new FileReader(file));
            String line;
            int index = 0;
            while ((line = br.readLine()) != null && index < spinners.length) {
                System.out.println("Line: " + line);
                int keyCode = Integer.parseInt(line);
                int pos = keyCodes.indexOf(keyCode);
                System.out.println("Pos: " + pos);
                if (pos >= 0) spinners[index].setSelection(pos);
                index++;
            }
            br.close();
        } catch (Exception e) {
            System.out.println("Error loading mapping: " + e.getMessage());
            setDefaults();
            saveMapping();
        }
    }

    private void setDefaults() {
        int i = 0;
        for(Spinner spinner : spinners){
            if(i < defaultKeyCodes.size()){
                spinner.setSelection(keyCodes.indexOf(defaultKeyCodes.get(i++)));
            }
        }
    }
}

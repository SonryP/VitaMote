package cl.sonry.vitamote.helpers;
import android.view.KeyEvent;
import java.lang.reflect.Field;
import java.lang.reflect.Modifier;
import java.util.ArrayList;
import java.util.List;

public class KeyCodeHelper {

    public static class KeyCodeEntry {
        public final int code;
        public final String name;

        public KeyCodeEntry(int code, String name) {
            this.code = code;
            this.name = name;
        }
    }

    public static List<KeyCodeEntry> getAllKeyCodeEntries() {
        List<KeyCodeEntry> entries = new ArrayList<>();
        Field[] fields = KeyEvent.class.getDeclaredFields();

        for (Field field : fields) {
            if (Modifier.isStatic(field.getModifiers())
                    && Modifier.isFinal(field.getModifiers())
                    && field.getName().startsWith("KEYCODE_")) {
                try {
                    int code = field.getInt(null);
                    String name = field.getName().replace("KEYCODE_", "");
                    entries.add(new KeyCodeEntry(code, name));
                } catch (IllegalAccessException e) {
                    e.printStackTrace();
                }
            }
        }

        return entries;
    }
}
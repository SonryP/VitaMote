package cl.sonry.vitamote.enums;
import java.util.EnumSet;

public enum TypeA {
    SELECT(1),
    START(8),
    UP(16),
    RIGHT(32),
    DOWN(64),
    LEFT(128);
    private final int value;

    TypeA(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }

    public static EnumSet<TypeA> fromCombinedValue(int combinedValue) {
        //TODO: Move to a common enum
        EnumSet<TypeA> result = EnumSet.noneOf(TypeA.class);
        for (TypeA type : TypeA.values()) {
            if ((type.getValue() & combinedValue) != 0) {
                result.add(type);
            }
        }
        return result;
    }

}

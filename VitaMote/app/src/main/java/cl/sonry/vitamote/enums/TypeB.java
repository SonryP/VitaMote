package cl.sonry.vitamote.enums;
import java.util.EnumSet;

public enum TypeB {
    LT(1),
    RT(2),
    T(16),
    C(32),
    X(64),
    S(128);

    private final int value;

    TypeB(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }

    public static EnumSet<TypeB> fromCombinedValue(int combinedValue) {
        //TODO: Move to a common enum
        EnumSet<TypeB> result = EnumSet.noneOf(TypeB.class);
        for (TypeB type : TypeB.values()) {
            if ((type.getValue() & combinedValue) != 0) {
                result.add(type);
            }
        }
        return result;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrappedValue {
    public WrappedValue(float min, float max) {
        MinValue = min;
        MaxValue = max;
    }

    public WrappedValue(float min, float max, float value) {
        MinValue = min;
        MaxValue = max;
        Value = value;
    }

    public float MinValue;

    public float MaxValue;

    public float Value {
        get {
            return val;
        }
        set {
            val = value.Wrap(MinValue, MaxValue);
        }
    }

    float minValue;
    float maxValue;
    float val;

    #region Self Operations
    public static bool operator ==(WrappedValue lhs, WrappedValue rhs) => lhs.Value == rhs.Value;
    public static bool operator !=(WrappedValue lhs, WrappedValue rhs) => lhs.Value != rhs.Value;
    public static bool operator >(WrappedValue lhs, WrappedValue rhs) => lhs.Value > rhs.Value;
    public static bool operator <(WrappedValue lhs, WrappedValue rhs) => lhs.Value < rhs.Value;
    public static bool operator >=(WrappedValue lhs, WrappedValue rhs) => lhs.Value >= rhs.Value;
    public static bool operator <=(WrappedValue lhs, WrappedValue rhs) => lhs.Value <= rhs.Value;

    public static WrappedValue operator +(WrappedValue lhs, WrappedValue rhs) {
        lhs.Value += rhs.Value;
        return lhs;
    }
    public static WrappedValue operator -(WrappedValue lhs, WrappedValue rhs) {
        lhs.Value -= rhs.Value;
        return lhs;
    }
    public static WrappedValue operator *(WrappedValue lhs, WrappedValue rhs) {
        lhs.Value *= rhs.Value;
        return lhs;
    }
    public static WrappedValue operator /(WrappedValue lhs, WrappedValue rhs) {
        lhs.Value /= rhs.Value;
        return lhs;
    }
    #endregion

    #region Float Operations
    public static bool operator ==(WrappedValue lhs, float rhs) => lhs.Value == rhs;
    public static bool operator !=(WrappedValue lhs, float rhs) => lhs.Value != rhs;
    public static bool operator >(WrappedValue lhs, float rhs) => lhs.Value > rhs;
    public static bool operator <(WrappedValue lhs, float rhs) => lhs.Value < rhs;
    public static bool operator >=(WrappedValue lhs, float rhs) => lhs.Value >= rhs;
    public static bool operator <=(WrappedValue lhs, float rhs) => lhs.Value <= rhs;

    public static WrappedValue operator +(WrappedValue lhs, float rhs) {
        lhs.Value += rhs;
        return lhs;
    }
    public static WrappedValue operator -(WrappedValue lhs, float rhs) {
        lhs.Value -= rhs;
        return lhs;
    }
    public static WrappedValue operator *(WrappedValue lhs, float rhs) {
        lhs.Value *= rhs;
        return lhs;
    }
    public static WrappedValue operator /(WrappedValue lhs, float rhs) {
        lhs.Value /= rhs;
        return lhs;
    }

    public static bool operator ==(float lhs, WrappedValue rhs) => lhs == rhs.Value;
    public static bool operator !=(float lhs, WrappedValue rhs) => lhs != rhs.Value;
    public static bool operator >(float lhs, WrappedValue rhs) => lhs > rhs.Value;
    public static bool operator <(float lhs, WrappedValue rhs) => lhs < rhs.Value;
    public static bool operator >=(float lhs, WrappedValue rhs) => lhs >= rhs.Value;
    public static bool operator <=(float lhs, WrappedValue rhs) => lhs <= rhs.Value;

    public static WrappedValue operator +(float lhs, WrappedValue rhs) {
        rhs.Value += lhs;
        return rhs;
    }
    public static WrappedValue operator -(float lhs, WrappedValue rhs) {
        rhs.Value -= lhs;
        return rhs;
    }
    public static WrappedValue operator *(float lhs, WrappedValue rhs) {
        rhs.Value *= lhs;
        return rhs;
    }
    public static WrappedValue operator /(float lhs, WrappedValue rhs) {
        rhs.Value /= lhs;
        return rhs;
    }
    #endregion

    #region Int Operations
    public static bool operator ==(WrappedValue lhs, int rhs) => lhs.Value == rhs;
    public static bool operator !=(WrappedValue lhs, int rhs) => lhs.Value != rhs;
    public static bool operator >(WrappedValue lhs, int rhs) => lhs.Value > rhs;
    public static bool operator <(WrappedValue lhs, int rhs) => lhs.Value < rhs;
    public static bool operator >=(WrappedValue lhs, int rhs) => lhs.Value >= rhs;
    public static bool operator <=(WrappedValue lhs, int rhs) => lhs.Value <= rhs;

    public static WrappedValue operator +(WrappedValue lhs, int rhs) {
        lhs.Value += rhs;
        return lhs;
    }
    public static WrappedValue operator -(WrappedValue lhs, int rhs) {
        lhs.Value -= rhs;
        return lhs;
    }
    public static WrappedValue operator *(WrappedValue lhs, int rhs) {
        lhs.Value *= rhs;
        return lhs;
    }
    public static WrappedValue operator /(WrappedValue lhs, int rhs) {
        lhs.Value /= rhs;
        return lhs;
    }

    public static bool operator ==(int lhs, WrappedValue rhs) => lhs == rhs.Value;
    public static bool operator !=(int lhs, WrappedValue rhs) => lhs != rhs.Value;
    public static bool operator >(int lhs, WrappedValue rhs) => lhs > rhs.Value;
    public static bool operator <(int lhs, WrappedValue rhs) => lhs < rhs.Value;
    public static bool operator >=(int lhs, WrappedValue rhs) => lhs >= rhs.Value;
    public static bool operator <=(int lhs, WrappedValue rhs) => lhs <= rhs.Value;

    public static WrappedValue operator +(int lhs, WrappedValue rhs) {
        rhs.Value += lhs;
        return rhs;
    }
    public static WrappedValue operator -(int lhs, WrappedValue rhs) {
        rhs.Value -= lhs;
        return rhs;
    }
    public static WrappedValue operator *(int lhs, WrappedValue rhs) {
        rhs.Value *= lhs;
        return rhs;
    }
    public static WrappedValue operator /(int lhs, WrappedValue rhs) {
        rhs.Value /= lhs;
        return rhs;
    }
    #endregion

    #region Casts
    public static implicit operator float(WrappedValue input) => input.Value;

    public static implicit operator int(WrappedValue input) => (int)input.Value;
    #endregion
}
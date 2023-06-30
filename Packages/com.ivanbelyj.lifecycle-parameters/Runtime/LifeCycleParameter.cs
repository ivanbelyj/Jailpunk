using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

///<summary>
/// Represents a life cycle parameter. Responsible for storing data and calling
/// events when they change
///</summary>
[System.Serializable]
public class LifecycleParameter
{
    [SerializeField]
    private uint parameterId;
    public uint ParameterId {
        get => parameterId;
        private set => parameterId = value;
    }

    private enum ValueState { Min, Intermediate, Max };
    private ValueState lastValueState;
    private bool isRecovered;

    [SerializeField]
    private float minValue = 0;
    public float MinValue { get => minValue; set => minValue = value; }
    [SerializeField]
    private float maxValue = 1;
    public float MaxValue { get => maxValue; set => maxValue = value; }

    public event UnityAction OnMin;
    public event UnityAction OnMax;
    public event UnityAction OnRecovered;
    public event UnityAction OnNotRecovered;

    /// <summary>
    /// The value that the restorative effects will strive for
    /// </summary>
    [SerializeField]
    private float recoveredValue = 1;
    public float RecoveredValue {
        get => recoveredValue;
        set {
            float newInitial = Mathf.Clamp(value, MinValue, MaxValue);
            float oldInitial = recoveredValue;

            if (newInitial != oldInitial) {
                recoveredValue = newInitial;
                OnRecoveredValueChanged?.Invoke(oldInitial, newInitial);
                RecoveredValueChangedActions(oldInitial, newInitial);
            }
        }
    }

    public delegate void ValueChanged(float oldValue, float newValue);
    public event ValueChanged OnValueChanged;

    [SerializeField]
    private float value = 1;
    public float Value {
        get => value;
        set {
            float oldValue = this.value;
            this.value = Mathf.Clamp(value, MinValue, MaxValue);
            if (oldValue != this.value)
                OnValueChanged?.Invoke(oldValue, this.value);
            else
                return;

            // If the minimum is reached, and before that the value was different
            if (this.value == MinValue && lastValueState != ValueState.Min) {
                lastValueState = ValueState.Min;
                OnMin?.Invoke();
            }
            if (this.value == MaxValue && lastValueState != ValueState.Max) {
                lastValueState = ValueState.Max;
                OnMax?.Invoke();
            }
            if (this.value > MinValue && this.value < MaxValue
                && lastValueState != ValueState.Intermediate) {
                lastValueState = ValueState.Intermediate;
                // An event for an intermediate value doesn't make much sense
            }

            if (this.value == RecoveredValue && !isRecovered) {
                isRecovered = true;
                OnRecovered?.Invoke();
            }
            if (this.value != RecoveredValue && isRecovered) {
                isRecovered = false;
                OnNotRecovered?.Invoke();
            }
        }
    }

    public delegate void RecoveredValueChanged(float oldRecoveredValue, float newRecoveredValue);
    /// <summary>
    /// An event called when value to which the restoring effects strive is changed.
    /// For example, a player got sick and his maximum health decreased
    /// </summary>
    public event RecoveredValueChanged OnRecoveredValueChanged;

    private void RecoveredValueChangedActions(float oldRecoveredValue, float newRecoveredValue)  {
        // Ситуация 1: норма была 100% здоровья. Игрок заболел, теперь никакие аптечки
        // не восстановят выше 80%.
        // Если у игрока до болезни было 100% (parameter == initial), то
        // болезнь снижает лишь максимум, а не текущее значение.
        // Если до болезни было 20%, опять же, в этом плане ничего не изменилось
        // Однако, если смена initial попала прямо в текущий параметр,
        // например, игрок заболел, initial стало 80%, а у него и так было 80%,
        // нужно запустить OnInitial (например, остановить эффекты, которые 
        // заканчиваются по наступлении initial)
        
        if (oldRecoveredValue > newRecoveredValue) {
            if (newRecoveredValue == value) {
                isRecovered = true;
                OnRecovered?.Invoke();
            }
        }

        // Ситуация 2: Игрок вылечился, норма 80% поднялась до нормы 100%
        if (oldRecoveredValue < newRecoveredValue) {
            // Если у игрока до болезни было 80% (parameter == initial),
            // то теперь нужно дать NotInitial, чтобы, например, запустить регенерацию
            if (value < newRecoveredValue) {
                isRecovered = false;
                OnNotRecovered?.Invoke();
            }
            // Если до выздоровления было 20%, то в этом плане без разницы,
            // является ли максимум 80%, или же 100%

            // Если лечение подняло норму до 100%, а оказалось, что
            // у игрока было выше нормы (not initial), теперь
            // значение совершенно нормально. Например, можно остановить эффекты возврата
            if (value == newRecoveredValue) {
                isRecovered = true;
                OnRecovered?.Invoke();
                // Строчки повторяются со случаем выше для ясности рассуждений
            }
        }

        // oldInitial == newInitial не бывает по логике
    }
}

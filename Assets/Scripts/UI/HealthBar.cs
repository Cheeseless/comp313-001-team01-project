#region usings

using UnityEngine;
using UnityEngine.UI;

#endregion

public class HealthBar : MonoBehaviour {

    Slider slider;

    void Awake() {
        slider = gameObject.GetComponentInChildren<Slider>();
    }

    public int MaxValue {
        get { return (int)slider.maxValue; }
        set { slider.maxValue = value; }
    }
    public int Value {
        get { return (int)slider.value; }
        set { slider.value = value; }
    }

    public void SetMaxValue(int value) {
        if (slider == null) {
            slider = gameObject.GetComponentInChildren<Slider>();

        }
        Debug.Log(slider);
        slider.maxValue = value;
        slider.value = value;
    }

    public void FullHeal() {
        slider.value = slider.maxValue;
    }

    public void Heal(int amount) {
        slider.value = slider.value + amount;
    }

    public void Damage(int amount) {
        slider.value = slider.value - amount; // we don't care about any death conditions here.
        if (slider.value < 0) {
            slider.value = 0; // might as well have this for now, no need to display overkill unless we need the feedback
        }
    }

}
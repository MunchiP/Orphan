using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SliderButtonController : MonoBehaviour, ISelectHandler
{
    public Slider slider;
    public Sprite spriteOn;
    public Sprite spriteOff;
    public Image imageSwitch;
    public float step = 0.2f;
    public float repeatRate = 0.1f;
    public bool requiereSeleccion = true;
    public bool esMusica = true;

    private float holdTimer = 0f;
    private float lastNonZeroValue = 1f;

    void Start()
    {
        if (slider == null)
        {
            Debug.LogError("Slider no asignado en " + gameObject.name, this);
            enabled = false;
            return;
        }

        if (imageSwitch == null)
            Debug.LogWarning("Image Switch no asignado en " + gameObject.name, this);

        if (AudioManager.instance == null)
        {
            Debug.LogError("No se encontró AudioManager en la escena", this);
        }
        else
        {
            float initialValue = esMusica ? AudioManager.instance.GetMusicVolume() : AudioManager.instance.GetSFXVolume();
            slider.value = initialValue;
            lastNonZeroValue = initialValue > 0 ? initialValue : 1f;
        }

        slider.onValueChanged.AddListener(OnSliderValueChanged);
        OnSliderValueChanged(slider.value);
    }

    void Update()
    {
        if (slider == null || !enabled) return;

        if (requiereSeleccion && EventSystem.current != null)
        {
            if (EventSystem.current.currentSelectedGameObject != gameObject)
            {
                holdTimer = repeatRate;
                return;
            }
        }

        float input = 0f;

        // Teclado
        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
                input = -1f;
            else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
                input = 1f;
        }

        // Gamepad (D-Pad y Stick izquierdo)
        if (Gamepad.current != null)
        {
            if (Gamepad.current.dpad.left.isPressed || Gamepad.current.leftStick.left.isPressed)
                input = -1f;
            else if (Gamepad.current.dpad.right.isPressed || Gamepad.current.leftStick.right.isPressed)
                input = 1f;
        }

        if (Mathf.Abs(input) > 0.1f)
        {
            holdTimer += Time.unscaledDeltaTime;

            if (holdTimer >= repeatRate)
            {
                float newValue = Mathf.Clamp(slider.value + step * input, slider.minValue, slider.maxValue);
                slider.value = newValue;
                holdTimer = 0f;
            }
        }
        else
        {
            holdTimer = repeatRate;
        }
    }

    public void OnSelect(BaseEventData eventData)
    {
        if (AudioManager.instance != null && slider != null)
        {
            float currentValue = esMusica ? AudioManager.instance.GetMusicVolume() : AudioManager.instance.GetSFXVolume();
            slider.value = currentValue;
            OnSliderValueChanged(currentValue);
        }
    }

    public void ToggleSwitch()
    {
        if (slider == null) return;

        if (slider.value > 0)
        {
            lastNonZeroValue = slider.value;
            slider.value = 0;
        }
        else
        {
            slider.value = lastNonZeroValue > 0 ? lastNonZeroValue : 1f;
        }
    }

    private void OnSliderValueChanged(float value)
    {
        if (imageSwitch != null)
        {
            imageSwitch.sprite = value > 0 ? spriteOn : spriteOff;
        }

        if (AudioManager.instance != null)
        {
            if (esMusica)
                AudioManager.instance.SetMusicVolume(value);
            else
                AudioManager.instance.SetSFXVolume(value);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems; // Importante para ISelectHandler
using UnityEngine.InputSystem;

public class SliderButtonController : MonoBehaviour, ISelectHandler // <--- Implementar ISelectHandler
{
    public Slider slider;
    public Sprite spriteOn;
    public Sprite spriteOff;
    public Image imageSwitch;
    public float step = 0.2f;
    public float repeatRate = 0.1f;
    public bool requiereSeleccion = true;
    public bool esMusica = true; // ✅ Activa esta opción si este slider es de música

    private float holdTimer = 0f;
    private float lastNonZeroValue = 1f;

    void Start()
    {
        // Verificar asignación de componentes esenciales
        if (slider == null)
        {
            Debug.LogError("Slider no asignado en el Inspector para " + gameObject.name + ". Este script no funcionará correctamente.", this);
            enabled = false; // Deshabilitar el script para evitar más errores
            return;
        }
        if (imageSwitch == null)
        {
            Debug.LogError("Image Switch no asignado en el Inspector para " + gameObject.name + ". La imagen del interruptor no se actualizará.", this);
            // No deshabilitamos el script, ya que el slider aún puede funcionar
        }
        if (spriteOn == null)
        {
            Debug.LogWarning("Sprite On no asignado para " + gameObject.name + ". El sprite no cambiará a 'ON'.", this);
        }
        if (spriteOff == null)
        {
            Debug.LogWarning("Sprite Off no asignado para " + gameObject.name + ". El sprite no cambiará a 'OFF'.", this);
        }

        // Verificar la existencia del AudioManager
        if (AudioManager.Instance == null)
        {
            Debug.LogError("No se encontró una instancia de AudioManager en la escena. Asegúrate de tener un GameObject con el script AudioManager.", this);
            // El script podría seguir funcionando para el slider visual, pero no guardará/cargará volumen.
        }
        else
        {
            // Cargar el valor inicial del AudioManager
            float initialValue = esMusica ? AudioManager.Instance.GetMusicVolume() : AudioManager.Instance.GetSFXVolume();
            slider.value = initialValue;
            lastNonZeroValue = initialValue > 0 ? initialValue : 1f;
        }

        slider.onValueChanged.AddListener(OnSliderValueChanged);
        OnSliderValueChanged(slider.value); // Para inicializar el sprite correctamente al inicio
    }

    void Update()
    {
        if (slider == null || !enabled) return;

        if (requiereSeleccion && EventSystem.current != null)
        {
            // Solo procesa la entrada si este GameObject está seleccionado
            if (EventSystem.current.currentSelectedGameObject != gameObject)
            {
                holdTimer = repeatRate; // Resetea el temporizador cuando no está seleccionado
                return;
            }
        }
        else if (requiereSeleccion && EventSystem.current == null)
        {
            Debug.LogWarning("No se encontró un EventSystem en la escena. Si 'requiereSeleccion' es verdadero, la entrada del teclado puede no funcionar como se espera.", this);
        }

        float input = 0f;

        if (Keyboard.current != null)
        {
            if (Keyboard.current.leftArrowKey.isPressed || Keyboard.current.aKey.isPressed)
                input = -1f;
            else if (Keyboard.current.rightArrowKey.isPressed || Keyboard.current.dKey.isPressed)
                input = 1f;
        }
        else
        {
            Debug.LogWarning("No se detectó un teclado. La entrada por teclado no funcionará para " + gameObject.name, this);
        }

        if (Mathf.Abs(input) > 0.1f)
        {
            holdTimer += Time.unscaledDeltaTime;

            if (holdTimer >= repeatRate)
            {
                float newValue = Mathf.Clamp(slider.value + step * input, slider.minValue, slider.maxValue);
                slider.value = newValue; // Esto triggea OnSliderValueChanged
                holdTimer = 0f;
            }
        }
        else
        {
            holdTimer = repeatRate;
        }
    }

    // --- NUEVO MÉTODO PARA CUANDO EL BOTÓN ES SELECCIONADO ---
    public void OnSelect(BaseEventData eventData)
    {
        // Asegúrate de que el AudioManager exista antes de intentar obtener su valor
        if (AudioManager.Instance != null && slider != null)
        {
            float currentValue = esMusica ? AudioManager.Instance.GetMusicVolume() : AudioManager.Instance.GetSFXVolume();
            slider.value = currentValue;
            // También podemos llamar a OnSliderValueChanged para actualizar la imagen del switch si es necesario
            OnSliderValueChanged(currentValue);
            Debug.Log($"Slider {gameObject.name} actualizado al ser seleccionado. Valor: {currentValue}");
        }
        else
        {
            // Mensajes de depuración si faltan referencias
            if (AudioManager.Instance == null)
                Debug.LogError("AudioManager.Instance es nulo al seleccionar el slider. No se puede actualizar el valor.", this);
            if (slider == null)
                Debug.LogError("El slider es nulo al seleccionar el GameObject. No se puede actualizar el valor.", this);
        }
    }
    // --------------------------------------------------------

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
            if (value > 0)
            {
                imageSwitch.sprite = spriteOn;
            }
            else
            {
                imageSwitch.sprite = spriteOff;
            }
        }
        
        if (AudioManager.Instance != null)
        {
            if (esMusica)
                AudioManager.Instance.SetMusicVolume(value);
            else
                AudioManager.Instance.SetSFXVolume(value);
        }
    }
}
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class EnemyAudio : MonoBehaviour
{
    private AudioSource audioSource;
    private Transform listener; // Generalmente será el Transform del jugador

    [Header("Volumen base y distancia")]
    [Range(0f, 1f)] public float baseVolume = 1f;
    public float maxDistance = 15f;

    [Header("Sonidos adicionales")]
    public AudioClip hurtClip;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("EnemyAudio requiere un componente AudioSource en el mismo GameObject.", this);
            enabled = false; // Deshabilita el script si no hay AudioSource
            return;
        }

        // Configuración inicial del AudioSource
        audioSource.volume = 0f; // Inicialmente en 0
        audioSource.loop = true; // Para sonidos ambientales o de respiración del enemigo
        audioSource.playOnAwake = false; // No reproducir automáticamente al despertar
        // Si tienes un sonido de "ambiente" para el enemigo, puedes asignarlo aquí o en el Inspector
        // Si no, no es necesario llamar a Play() en Awake, lo haremos en Update si el volumen > 0
    }

    private void Start()
    {
        // Busca al jugador por tag
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            listener = player.transform;
        }
        else
        {
            // Advertencia si el "Player" no se encuentra. Crucial para la atenuación de volumen.
            Debug.LogWarning("No se encontró ningún GameObject con el tag 'Player'. La atenuación de volumen por distancia para el AudioSource de " + gameObject.name + " no funcionará.", this);
        }

        // Asegúrate de que el AudioManager esté presente
        if (AudioManager.Instance == null)
        {
            Debug.LogError("No se encontró una instancia de AudioManager en la escena. El volumen de los SFX de este enemigo no se ajustará correctamente.", this);
            // Considera deshabilitar el script o solo la lógica de volumen si AudioManager es vital.
        }

        // Si tienes un clip de ambiente para el enemigo que debe sonar en loop, reprodúcelo aquí
        // Por ejemplo, si tienes un 'idleLoopClip' público:
        // if (idleLoopClip != null) {
        //    audioSource.clip = idleLoopClip;
        //    audioSource.Play();
        // }
        // Si tu intención es un sonido de ambiente constante del enemigo, asegúrate de asignarle un clip al audioSource en el Inspector.
        // Si no tiene un clip en Awake, la llamada a Play() no hará nada hasta que se le asigne uno.
        // Asumiendo que el audioSource ya tiene un clip de ambiente asignado en el Inspector, si es lo que quieres lo de Play()
        if (audioSource.clip != null && !audioSource.isPlaying)
        {
            audioSource.Play(); // Inicia la reproducción si hay un clip y no está sonando.
        }
    }

    private void Update()
    {
        // Protecciones contra objetos nulos
        if (listener == null || AudioManager.Instance == null || audioSource == null)
        {
            // Si el AudioManager es nulo, no podemos obtener el volumen global de SFX.
            // Si el listener es nulo, no podemos calcular la distancia.
            // Si el audioSource es nulo, no podemos manipular el sonido.
            audioSource.volume = 0f; // Asegura que no haya sonido si hay un problema crítico
            return;
        }

        float distance = Vector3.Distance(transform.position, listener.position);

        if (distance > maxDistance)
        {
            audioSource.volume = 0f; // Fuera de rango, silenciar
            return;
        }

        // Calcula el factor de atenuación basado en la distancia
        // 1f - (distance / maxDistance) hará que el volumen disminuya linealmente con la distancia.
        // Clamp01 asegura que el valor esté entre 0 y 1.
        float distanceFactor = Mathf.Clamp01(1f - (distance / maxDistance));

        // Calcula el volumen final: volumen base del enemigo * atenuación por distancia * volumen global de SFX
        float finalVolume = baseVolume * distanceFactor * AudioManager.Instance.GetSFXVolume(); // ¡Aquí está el cambio clave!

        audioSource.volume = finalVolume;
    }

    // 🔊 Reproduce un sonido de daño sin interrumpir el loop del audioSource principal
    public void PlayHurtSound()
    {
        if (hurtClip == null)
        {
            Debug.LogWarning("hurtClip no asignado en EnemyAudio para " + gameObject.name + ". No se puede reproducir el sonido de daño.", this);
            return;
        }

        if (audioSource == null) return; // Protección

        // Verificar la existencia del AudioManager antes de usarlo
        if (AudioManager.Instance == null)
        {
            Debug.LogError("No se encontró una instancia de AudioManager. No se puede ajustar el volumen del sonido de daño.", this);
            audioSource.PlayOneShot(hurtClip, baseVolume); // Reproducir con volumen base si no hay AudioManager
            return;
        }

        // Usa PlayOneShot para reproducir el clip de daño sin afectar el clip principal que pueda estar en loop
        // Multiplica el volumen base por el volumen global de SFX del AudioManager
        audioSource.PlayOneShot(hurtClip, baseVolume * AudioManager.Instance.GetSFXVolume());

        // Para el efecto de pitch temporal, puedes modificar el pitch del audioSource principal
        // y restaurarlo después de un corto tiempo.
        // Si quieres que el pitch afecte solo a este PlayOneShot, tendrías que crear un nuevo AudioSource
        // o usar una lógica más compleja con el mismo AudioSource.
        // Para este caso, restaurar el pitch global después de que el hurtClip termine con su pitch modificado:
        float originalPitch = audioSource.pitch; // Guarda el pitch actual
        audioSource.pitch = 1.6f; // Aplica el pitch modificado al AudioSource

        // Cancela cualquier Invoke anterior de ResetPitch para evitar conflictos
        CancelInvoke(nameof(ResetPitch));
        // Programa la restauración del pitch después de la duración del hurtClip
        // Asegúrate de que el pitch del PlayOneShot sea el que esperas, ya que PlayOneShot usa el pitch actual del AudioSource.
        Invoke(nameof(ResetPitch), hurtClip.length / 1.6f); // Divide por el pitch aplicado para obtener el tiempo real.
    }

    private void ResetPitch()
    {
        // Resetea el pitch del AudioSource principal a 1f (o al valor original si lo guardaste en una variable de clase)
        audioSource.pitch = 1f;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, maxDistance);
    }
}
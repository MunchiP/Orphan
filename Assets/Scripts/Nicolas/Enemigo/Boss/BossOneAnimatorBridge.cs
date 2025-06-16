using UnityEngine;

public class BossOneAnimatorBridge : MonoBehaviour
{
    public FixedPositionSwordSpawner fixedPositionSwordSpawner;
    public BossOneBehaviour bossOneBehaviour;
    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        bossOneBehaviour = FindAnyObjectByType<BossOneBehaviour>();
        fixedPositionSwordSpawner = FindAnyObjectByType<FixedPositionSwordSpawner>();
    }
    public void ActivarEspadasHorizontales()
    {
        bossOneBehaviour.enabled = false;
        // fixedPositionSwordSpawner.enabled = true;
        fixedPositionSwordSpawner.StartSpawningFromAnimation();
    }
}

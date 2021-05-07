using System.Collections;

using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Bomb : MonoBehaviour, IExplosive
{
    TextMeshProUGUI tmproText;

    [SerializeField] private float timeBeforeExploding;
    private float timer = 0f;

    private BombCountdownCanvas bombCountdownCanvas;
    private GameController gameController;
    private Animator animator;
    private SoundController soundController;
    private Cow cow;

    SphereCollider bombRadius;
    private float bombRadiusSize = 2f;

    private string exploded = "exploded";
    private string pickedUp = "pickedUp";

    private bool hasBeenPickedUp;
    private bool hasExploded;
    private bool exploding;
    private bool buildUpActivated;

    private readonly float levelStartTime = 1.5f;
    private float levelStartTimer;

    private Color dangerColor = new Color(.66f, 0f, 0f, 1f);

    // Start is called before the first frame update
    void Start()
    {
        bombRadius = GetComponent<SphereCollider>();
        gameController = FindObjectOfType<GameController>();
        bombCountdownCanvas = FindObjectOfType<BombCountdownCanvas>();
        animator = GetComponentInChildren<Animator>();
        soundController = FindObjectOfType<SoundController>();
        cow = FindObjectOfType<Cow>();
    }

    private void FixedUpdate()
    {
        if (timer >= timeBeforeExploding)
        {
            bombRadius.radius = bombRadiusSize;
        }

    }

    // Update is called once per frame
    void Update()
    {
        levelStartTimer += Time.deltaTime;

        if (gameController.isGameReady() && levelStartTimer > levelStartTime)
        {
            bombCountdownCanvas.SetBombPositionAndTime(
                    gameObject.transform.position,
                    timeBeforeExploding - timer,
                    this,
                    hasExploded,
                    hasBeenPickedUp);

            timer += Time.deltaTime;
            if (timer >= timeBeforeExploding)
            {
                exploding = true;
                //Debug.Log("MooBooM!"); // :)
            }
            if (exploding && !hasExploded)
            {
                hasExploded = true;
                //LocalisationSystem.SetLanguage("English");
                AnimateExplosion();
            }
            else if (timeBeforeExploding - timer < 3 && !buildUpActivated && !exploding && !hasBeenPickedUp)
            {
                soundController.PlaySound("ExplosionBuildup");
                StartCoroutine(Blink());
                buildUpActivated = true;
            }

            if (hasBeenPickedUp)
            {
                soundController.StopSound("ExplosionBuildup");
                soundController.StopSound("ExplosionBuildup");
            }
        }
    }

    private IEnumerator Blink() {
        Material mat =
                GetComponentInChildren<AwesomeToon.AwesomeToonHelper>()
                .GetMaterialInstance();
        Color std = mat.GetColor("Color_5f0c695d5224454a8f080ea40f7f4289");
        Color lerp;

        while(timeBeforeExploding - timer >= .5f) {
            lerp = Color.Lerp(
                    std,
                    dangerColor,
                    Mathf.Sin((timeBeforeExploding - timer) * 9) * .5f + .5f);

            mat.SetColor("Color_5f0c695d5224454a8f080ea40f7f4289", lerp);
            yield return null;
		}
        mat.SetColor("Color_5f0c695d5224454a8f080ea40f7f4289", dangerColor);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (!exploding)
            {
                //LocalisationSystem.SetLanguage("Swedish");
                hasBeenPickedUp = true;
                AnimatePickup();
            }
            else if(exploding && !hasBeenPickedUp)
            {
                HurtCow();
            }
        }
    }

    private void HurtCow()
    {
        Debug.Log("OUCHIEEE -> Bomb");
        cow.Explosion(transform.position);
        soundController.PlaySound("HurtCow");

        if (!gameController.isCowAlreadyHurt())
        {
            gameController.CowTakesDamage();
            gameController.GameOver();
        }
        
    }

    private void AnimateExplosion()
    {
        animator.SetBool(exploded, true);
    }

    private void AnimatePickup()
    {
        animator.SetBool(pickedUp, true);
    }

    public void Exploded() // called by animation event
    {
        gameController.BombExploded(this);
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }

    public void PickMeUp()
    {
        //hasBeenPickedUp = true;
        gameController.BombPickedUp(this);
    }

    public float GetTimeBeforeExploding()
    {
        return timeBeforeExploding;
    }
    public float GetTimeUntilExploding()
    {
        return timer;
    }

}

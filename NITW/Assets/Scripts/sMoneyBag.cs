using UnityEngine;

public class sMoneyBag : MonoBehaviour
{
    cMoney money;

    public float amount = 50.26f;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        money = cMoney.moneyGlobal;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            //Debug.Log("Player triggered money bag");

            money.GetMoney(amount);

            sPlayer.playerGlobal.DisplayText("Fuck yeah gimme " + (int)amount + " bucks!", 3f);

            sAudioPlayer.audioPlayerGlobal.TriggerSFX("getPaid", eSFXTriggerType.eSFXtriggerBasic, eAudioMixerType.ui);

            Destroy(this.gameObject);
        }
    }
}
